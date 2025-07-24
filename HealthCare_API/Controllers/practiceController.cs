using Microsoft.AspNetCore.Mvc; 
using System.Text;              
using System.Security.Cryptography; 

namespace HealthCare_API.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]            
    public class PracticeController : ControllerBase

    //HMAC (Hash-based Message Authentication Code) ek technique hai jo message + secret key ka use karke ek secure hash (code) banata hai.
    //AES ek symmetric encryption algorithm hai jisme ek hi key se data encrypt (chhupana) aur decrypt (waapas normal banana) hota hai.
    {
        [HttpGet("generate")] 
        public IActionResult GenerateHmac(string message)
        {
            string key = "secret123"; // Shared secret key
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)); // Create HMAC object with key
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message)); // Compute HMAC hash
            string result = BitConverter.ToString(hash).Replace("-", "").ToLower(); // Convert byte[] to hex string

            return Ok(new { message, hmac = result }); // Return message and HMAC to client
        }

        [HttpGet("compare")] 
        public IActionResult CompareAll()
        {
            string key = "secret123"; // Shared key
            string message = "Hello"; // Message to hash

            var sha256 = ComputeHmac(message, key, new HMACSHA256());
            var sha384 = ComputeHmac(message, key, new HMACSHA384());
            var sha512 = ComputeHmac(message, key, new HMACSHA512());

            // Return all three HMACs
            return Ok(new
            {
                message,
                hmac_sha256 = sha256,
                hmac_sha384 = sha384,
                hmac_sha512 = sha512
            });
        }

        [HttpGet("aes-encrypt")] 
        public IActionResult Encrypt()
        {
            string plainText = "Hello World!"; // Text to encrypt
            string key = "ThisIsA256bitKey1234567890123456"; // 32-byte (256-bit) key
            string iv = "ThisIsA16ByteIV!"; // 16-byte IV (Initialization Vector)

            using Aes aes = Aes.Create(); // Create AES object
            aes.Key = Encoding.UTF8.GetBytes(key); // Set AES key
            aes.IV = Encoding.UTF8.GetBytes(iv); // Set AES IV
            aes.Mode = CipherMode.CBC; // Set AES mode to CBC
            aes.Padding = PaddingMode.PKCS7; // Set padding method

            using var encryptor = aes.CreateEncryptor(); // Create AES encryptor
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText); // Convert message to bytes
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length); // Encrypt

            // Generate HMAC-SHA512 from encrypted data
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            byte[] hmacBytes = hmac.ComputeHash(encryptedBytes); // Compute HMAC

            // Return encrypted text and HMAC
            return Ok(new
            {
                plainText,
                cipherText = Convert.ToBase64String(encryptedBytes), // Encrypted bytes as Base64 string
                hmac = BitConverter.ToString(hmacBytes).Replace("-", "").ToLower() // HMAC as hex string
            });
        }

        private string ComputeHmac(string message, string key, HMAC hmac)
        {
            hmac.Key = Encoding.UTF8.GetBytes(key); // Set HMAC key
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message)); // Compute HMAC
            return BitConverter.ToString(hash).Replace("-", "").ToLower(); // Convert to hex string
        }
    }
}
