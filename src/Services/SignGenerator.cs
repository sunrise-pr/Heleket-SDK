using Heleket.Models;
using Heleket.Internal;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Heleket.Services
{
    /// <summary>
    /// Legacy static signature helper retained for compatibility.
    /// </summary>
    public static class SignatureGenerator
    {

        /// <summary>
        /// Serializes a legacy invoice request into deterministic JSON.
        /// </summary>
        /// <param name="request">The legacy invoice request.</param>
        /// <returns>The serialized JSON string, or an empty string when serialization fails.</returns>
        public static string GetTextJson(CreateInvoiceRequest? request)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                // 1. & 2. Get non-null properties and their JSON names
                var propertiesToInclude = new SortedDictionary<string, object>(StringComparer.Ordinal); // Use SortedDictionary for automatic key sorting

                var properties = request.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in properties)
                {
                    // Skip if property cannot be read
                    if (!prop.CanRead) continue;

                    // Get the value
                    object? value = prop.GetValue(request);

                    // 3. Include only non-null values
                    if (value != null)
                    {
                        // Get the JSON property name from the attribute, default to property name if attribute not found
                        var jsonPropertyNameAttr = prop.GetCustomAttribute<JsonPropertyAttribute>();
                        string jsonName = jsonPropertyNameAttr?.PropertyName ?? prop.Name;

                        propertiesToInclude.Add(jsonName, value);
                    }
                }

                // Check if there are any properties to include
                if (!propertiesToInclude.Any())
                {
                    // Handle cases where the object might be entirely null properties (unlikely for required fields)
                    // Decide behavior: return empty hash, null, or throw? Returning null seems reasonable.
                    Console.WriteLine("Warning: No non-null properties found to generate signature."); // Or use ILogger
                    return string.Empty;
                }

                return HeleketJson.Serialize(propertiesToInclude);

                // 5. Base64 encode the JSON string (using UTF-8)

            }
            catch (Exception ex)
            {
                // Log the exception (using ILogger is recommended)
                Console.WriteLine($"Error generating request signature: {ex.Message}");
                // Depending on policy, re-throw, return null, or a specific error indicator
                // throw new InvalidOperationException("Failed to generate request signature.", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Creates an uppercase MD5 hash for compatibility with older callers.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <returns>The uppercase hexadecimal MD5 hash.</returns>
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Inserts a slash escape byte before every slash byte.
        /// </summary>
        /// <param name="inputArray">The source byte array.</param>
        /// <returns>The transformed byte array, or null when the source is null.</returns>
        public static byte[]? InsertByteBeforeSpecificByte(byte[]? inputArray)
        {
            if (inputArray == null)
            {
                return null;
            }

            // Используем List<byte> для удобства добавления элементов
            List<byte> resultList = new List<byte>(inputArray.Length); // Задаем начальную емкость

            const byte byteToFind = 47;    // Байт '/'
            const byte byteToInsert = 92;  // Байт '\'

            foreach (byte b in inputArray)
            {
                if (b == byteToFind)
                {
                    // Если нашли байт 47 ('/'), сначала добавляем байт 92 ('\')
                    resultList.Add(byteToInsert);
                }
                // Затем всегда добавляем текущий байт (либо исходный, либо 47)
                resultList.Add(b);
            }

            // Преобразуем список обратно в массив байтов
            return resultList.ToArray();
        }

        /// <summary>
        /// Generates an MD5 signature for a CreateInvoiceRequest object,
        /// considering only non-null properties and using a process similar to webhook verification.
        /// </summary>
        /// <param name="request">The CreateInvoiceRequest object.</param>
        /// <param name="apiSecretKey">Your secret API key used for signing.</param>
        /// <returns>The calculated MD5 signature as a lowercase hexadecimal string, or null if input is invalid.</returns>
        /// <exception cref="ArgumentNullException">Thrown if request or apiSecretKey is null.</exception>
        public static string? GenerateRequestSignature(object request, string? apiSecretKey)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNullOrEmpty(apiSecretKey, "API Secret Key cannot be null or empty.");
           

            try
            {
                string jsonToSign = HeleketJson.Serialize(request);
                return HeleketSigner.SignJsonValue(jsonToSign, apiSecretKey);
            }
            catch (Exception ex)
            {
                // Log the exception (using ILogger is recommended)
                Console.WriteLine($"Error generating request signature: {ex.Message}");
                // Depending on policy, re-throw, return null, or a specific error indicator
                // throw new InvalidOperationException("Failed to generate request signature.", ex);
                return null;
            }
        }

        /// <summary>
        /// Validates a legacy webhook signature model.
        /// </summary>
        /// <param name="response">The webhook payload without the sign field.</param>
        /// <param name="apiSecretKey">The API key used to validate the signature.</param>
        /// <param name="sign">The received signature.</param>
        /// <returns><see langword="true"/> when the signature matches.</returns>
        public static bool ValidateSignature(WebhookPaymentResponseNoSign response, string apiSecretKey, string sign)
        {
            try
            {
                // 1. & 2. Get non-null properties and their JSON names

                string jsonToSign = HeleketJson.Serialize(response);

                // 5. Base64 encode the JSON string (using UTF-8)
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonToSign);

                string base64EncodedJson = Convert.ToBase64String(jsonBytes);

                // 6. Concatenate with the secret API key
                string stringToHash = base64EncodedJson + apiSecretKey;

                // 7. Calculate the MD5 hash
                byte[] inputBytes = Encoding.UTF8.GetBytes(stringToHash);
                byte[] hashBytes = MD5.HashData(inputBytes);

                // 8. Convert hash to lowercase hexadecimal string
                // return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant(); // Alternative

                return Convert.ToHexString(hashBytes).ToLowerInvariant() == sign; // .NET 5+ preferred
            }
            catch (Exception ex)
            {
                // Log the exception (using ILogger is recommended)
                Console.WriteLine($"Error generating request signature: {ex.Message}");
                // Depending on policy, re-throw, return null, or a specific error indicator
                // throw new InvalidOperationException("Failed to generate request signature.", ex);
                return false;
            }
        }
    }
}
