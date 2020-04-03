using System;
using System.IO;
using System.Security.Cryptography;

namespace symmetric_encryption
{
    class Program
    {
        // Credit to: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netframework-4.8

        // To use the program:
        // Select encoding or use the default.
        // Generate a key and initialization vector and enter them in the variables below
        // Never share your key and initialization vector in an unencrypted manner.
        private static readonly Encoding _encoding = Encoding.Base64;
        private static readonly string _iv = "QW+rLzLK0UGB1r4A7gfv7A==";
        private static readonly string _key = "3d8Y5KHn9NR58cqIOcKkZmB6B8H+VsPKG1mwZDeBNWE=";

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Want do you want to do?");
                    Console.WriteLine("[E]necrypt");
                    Console.WriteLine("[D]ecrypt");
                    Console.WriteLine("[G]enerate Key and IV");
                    Console.WriteLine("[Q]uit");
                    Console.WriteLine("");

                    var selection = Console.ReadKey();

                    if (selection.KeyChar == 'E' | selection.KeyChar == 'e')
                    {
                        StartEncrypting();
                    }
                    else if (selection.KeyChar == 'D' | selection.KeyChar == 'd')
                    {
                        StartDectypting();
                    }
                    else if (selection.KeyChar == 'G' | selection.KeyChar == 'g')
                    {
                        StartGenerating();
                    }
                    else if (selection.KeyChar == 'Q' | selection.KeyChar == 'q')
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void StartGenerating()
        {
            // Create a new instance of the Aes class.
            // This generates a new key and initialization vector (IV).
            while (true)
            {
                using (Aes myAes = Aes.Create())
                {
                    Console.WriteLine("IV:          {0}", Encode(myAes.IV));
                    Console.WriteLine("Key:         {0}", Encode(myAes.Key));
                    Console.WriteLine();
                    var selection = Console.ReadKey();
                    Console.WriteLine();
                    if (selection.KeyChar == 'Q' | selection.KeyChar == 'q')
                    {
                        return;
                    }
                }

                Console.WriteLine("Press Q to [Q]uit.");
                Console.WriteLine("Press any other key to generate a new Key and IV.");
            }
        }

        private static void StartDectypting()
        {
            while (true)
            {
                Console.WriteLine("Enter Q to [Q]uit.");
                Console.WriteLine("Enter hex blocks to decrypt: ");
                var textToDecrypt = Console.ReadLine();
                if (ShouldQuit(textToDecrypt))
                {
                    return;
                }
                Console.WriteLine(DecryptStringFromBytes_Aes(Decode(textToDecrypt), Decode(_key), Decode(_iv)));
            }
        }

        private static void StartEncrypting()
        {
            while (true)
            {
                Console.WriteLine("Enter Q to [Q]uit.");
                Console.WriteLine("Enter the text to encrypt: ");
                var textToEncrypt = Console.ReadLine();
                if (ShouldQuit(textToEncrypt))
                {
                    return;
                }
                Console.WriteLine(Encode(EncryptStringToBytes_Aes(textToEncrypt, Decode(_key), Decode(_iv))));
            }
        }

        private static string Encode(byte[] data)
        {
            switch (_encoding)
            {
                case Encoding.Hex:
                    return data.ToHexString();
                case Encoding.Base64:
                    return data.ToBase64String();
                default: throw new NotImplementedException();
            }
        }

        private static byte[] Decode(string data)
        {
            switch (_encoding)
            {
                case Encoding.Hex:
                    return data.FromHexToByteArray();
                case Encoding.Base64:
                    return data.FromBase64ToByteArray();
                default: throw new NotImplementedException();
            }
        }


        static bool ShouldQuit(string inputString)
        {
            return inputString.Replace(" ", "") == "Q" || inputString.Replace(" ", "") == "q";
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}