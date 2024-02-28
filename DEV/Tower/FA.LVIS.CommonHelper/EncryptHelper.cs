using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.CommonHelper
{
    public static class EncryptHelper
    {

        public static string Decrypt(this string encryptedString, string cipherKey, string cipherIV)
        {
            if (IsBase64String(encryptedString))
            {
                try
                {
                    if (!string.IsNullOrEmpty(cipherKey) && !string.IsNullOrEmpty(cipherIV))
                    {
                        //Local Variable Declaration to store decrypted value
                        string decryptedData;

                        //Generating Salt Value
                        byte[] salt = cipherIV.Split(',').Select(n => Convert.ToByte(n, 16)).ToArray();

                        //Setting up the Decryptor using RijndaelManaged Class
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(cipherKey, salt);
                        using (RijndaelManaged RMCrypto = new RijndaelManaged())
                        {
                            RMCrypto.Key = pdb.GetBytes(32);
                            RMCrypto.IV = pdb.GetBytes(16);
                            var decryptor = RMCrypto.CreateDecryptor(RMCrypto.Key, RMCrypto.IV);
                            var cipher = Convert.FromBase64String(encryptedString);

                            //Decrypting the text read into a memory stream
                            using (var msDecrypt = new MemoryStream(cipher))
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                decryptedData = srDecrypt.ReadToEnd();
                            }

                            return decryptedData;
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (CryptographicException ex)
                {
                    return "";
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            else
            {
                return encryptedString;
            }
        }

        public static string Encrypt(this string decryptedString, string cipherKey, string cipherIV)
        {
            try
            {
                byte[] cipherTextBytes = null;
                string encryptedData = null;
                if (decryptedString != null && decryptedString.Length > 0)
                {
                    if (!string.IsNullOrEmpty(cipherKey) && !string.IsNullOrEmpty(cipherIV))
                    {
                        //Local Variable Declaration to store decrypted value                       
                        //Generating Salt Value
                        byte[] salt = cipherIV.Split(',').Select(n => Convert.ToByte(n, 16)).ToArray();

                        //Setting up the Decryptor using RijndaelManaged Class
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(cipherKey, salt);
                        using (RijndaelManaged RMCrypto = new RijndaelManaged())
                        {
                            RMCrypto.Key = pdb.GetBytes(32);
                            RMCrypto.IV = pdb.GetBytes(16);
                            var encryptor = RMCrypto.CreateEncryptor(RMCrypto.Key, RMCrypto.IV);
                            //Decrypting the text read into a memory stream
                            using (var msEncrypt = new MemoryStream())
                            {
                                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                                {
                                    using (var srEncrypt = new StreamWriter(csEncrypt))
                                    {
                                        srEncrypt.Write(decryptedString);
                                    }
                                    cipherTextBytes = msEncrypt.ToArray();
                                    encryptedData = Convert.ToBase64String(cipherTextBytes);
                                }
                            }
                        }
                    }
                    return encryptedData;
                }
                else
                {
                    return "";
                }
            }
            catch (CryptographicException ex)
            {
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private static bool IsBase64String(string text)
        {
            if (text  == null)
                return false;
            text = text.Trim();
            if (text.Length % 4 == 0)
            {
                try
                {
                    Convert.FromBase64String(text);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else return false;
        }


        public static string Encrypt(this string sEncrypttext)
        {

            using (TerminalEncryptEntities dbContext1 = new TerminalEncryptEntities())
            {
                var Cipherkeys = dbContext1.GetEnvironmentKey().Where(i => i.ApplicationId == null && i.EnvironmentId == 2).FirstOrDefault();

                if (Cipherkeys != null)
                    return sEncrypttext.Encrypt(Cipherkeys.EnvironmentKeyValue, Cipherkeys.EnvironmentKeyVI);


            }

            return string.Empty;
        }

        public static string Decrypt(this string str)
        {

            using (TerminalEncryptEntities dbContext1 = new TerminalEncryptEntities())
            {
                var Cipherkeys = dbContext1.GetEnvironmentKey().Where(i => i.ApplicationId == null && i.EnvironmentId == 2).FirstOrDefault();

                if (Cipherkeys != null)
                    return str.Decrypt(Cipherkeys.EnvironmentKeyValue, Cipherkeys.EnvironmentKeyVI);


            }

            return string.Empty;

        }
    }


}
