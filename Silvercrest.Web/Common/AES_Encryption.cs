using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Silvercrest.Web.Common
{

    public static class Hash
    {
        private static Getter get = new Getter();
        private static Dictionary<string, string> hash;

        static Hash()
        {
            hash = new Dictionary<string, string>();
           
            get.Get();
        }

        public static string GetDecryptedValue(string encryptedValue)
        {
            if (encryptedValue.Length == 0)
                return "";

            string decryptedValue = AES_Encryption.DecryptText(encryptedValue) ;  

            return decryptedValue;
        }

        public static string GetEncryptedValue(string decryptedValue)
        {
            if (decryptedValue.Length == 0)
                return "";

            string encryptedValue = null;
            if (!hash.ContainsKey(decryptedValue.ToLower()))
            {
                encryptedValue = AES_Encryption.EncryptText(decryptedValue.ToLower());
                hash.Add(decryptedValue.ToLower(), encryptedValue);
                try
                {
                    get.AddRecord(decryptedValue.ToLower(), encryptedValue);
                }
                catch(Exception e)
                {

                }
            }
            else
            {
                encryptedValue = hash[decryptedValue.ToLower()].ToString();
            }

            return encryptedValue;


        }

        public static List<Silvercrest.Entities.Account> EncryptEntities(List<Silvercrest.Entities.Account> entities)
        {
            Silvercrest.Entities.Account acct;
            for(int i=0; i < entities.Count; i++)
            {
                acct = entities[i];
                acct.EntityIdEnc = acct.EntityId == null ? null : GetEncryptedValue(acct.EntityId.ToString());
                acct.ContactIdEnc = acct.ContactId == null ? null : GetEncryptedValue(acct.ContactId.ToString());
                acct.IsGroupEnc = acct.IsGroup == null ? null : GetEncryptedValue(acct.IsGroup.ToString());
                acct.IsClientGroupEnc = acct.IsClientGroup == null ? null : GetEncryptedValue(acct.IsClientGroup.ToString());
            }

            return entities;

        }

        //PUTTING AES_ENCRPTION CLASS WITHIN THE Hash Class
        protected static class AES_Encryption
        {
            private static byte[] passwordBytes;

            static AES_Encryption()
            {
                string password = "SilvercrestPortal2017";
                // Get the bytes of the string
                passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            }


            public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted/*, byte[] passwordBytes*/)
            {
                byte[] encryptedBytes = null;
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }

                return encryptedBytes;
            }

            public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted/*, byte[] passwordBytes*/)
            {
                byte[] decryptedBytes = null;

                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }

            public static string EncryptText(string input)
            {
                if (input == "")
                    return "";
                /*
                            string password = "SilvercrestPortal2017";
                            // Get the bytes of the string
                            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                            // Hash the password with SHA256
                            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                */
                byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
                byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted/*, passwordBytes*/);

                string result = Convert.ToBase64String(bytesEncrypted);

                result = Uri.EscapeDataString(result);
                return result;
            }
            public static string[] EncryptText(string[] input)
            {
                /*
                            string password = "SilvercrestPortal2017";
                            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                */
                // Get the bytes of the string
                string[] result = new string[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input[i]);
                    byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted/*, passwordBytes*/);
                    result[i] = Convert.ToBase64String(bytesEncrypted);
                }
                return result;
            }

            public static string DecryptText(string input)
            {
                for (int i = 0; i < 4; i++)
                {
                    input = Uri.UnescapeDataString(input);
                }
                input = Uri.UnescapeDataString(input);
                /*
                            string password = "SilvercrestPortal2017";
                            // Get the bytes of the string
                            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                */

                byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
                byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted/*, passwordBytes*/);

                string result = Encoding.UTF8.GetString(bytesDecrypted);
                return result;
            }

            public static string[] DecryptText(string[] input)
            {
                /*            string password = "SilvercrestPortal2017";
                            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                */
                // Get the bytes of the string
                string[] result = new string[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    byte[] bytesToBeDecrypted = Encoding.UTF8.GetBytes(input[i]);
                    byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted/*, passwordBytes*/);
                    result[i] = Convert.ToBase64String(bytesDecrypted);
                }
                return result;
            }

            //public static Hashtable EncryptHash()
            //{
            //    Hashtable hs = new Hashtable();
            //    for (int i = 700000; i < 1000000; i++)
            //    {
            //        hs.Add(i.ToString(), EncryptText(i.ToString()));
            //    }
            //    return hs;
            //}

            //public static Hashtable AES_Encrypt1()
            //{
            //    byte[] bytesToBeEncrypted;
            //    byte[] passwordBytes = Encoding.UTF8.GetBytes("SilvercrestPortal2017");
            //    byte[] encryptedBytes = null;
            //    byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            //    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            //    Hashtable hs = new Hashtable();
            //    for (int i = 800000; i < 850000; i++)
            //    {
            //        using (MemoryStream ms = new MemoryStream())
            //        {
            //            using (RijndaelManaged AES = new RijndaelManaged())
            //            {
            //                AES.KeySize = 256;
            //                AES.BlockSize = 128;
            //                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            //                AES.Key = key.GetBytes(AES.KeySize / 8);
            //                AES.IV = key.GetBytes(AES.BlockSize / 8);
            //                AES.Mode = CipherMode.CBC;

            //                bytesToBeEncrypted = Encoding.UTF8.GetBytes(i.ToString());
            //                using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
            //                {
            //                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
            //                    cs.Close();
            //                }
            //                encryptedBytes = ms.ToArray();

            //                var result = Convert.ToBase64String(encryptedBytes);
            //                result = Uri.EscapeDataString(result);
            //                hs.Add(i.ToString(), result);
            //            }
            //        }
            //    }
            //    return hs;
            //}
        }


        //Putting GETTER Also inside Class HASH
        protected class Getter
        {
            private SLVR_DEVEntities _context;
            public Getter()
            {
                _context = new SLVR_DEVEntities();
            }

            public void Get()
            {
//                Hashtable table = new Hashtable();
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Web_Encryption_Dictionary", connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var key = reader.GetString(reader.GetOrdinal("val"));
                                    var value = reader.GetString(reader.GetOrdinal("encrypted_val"));
                                    Hash.hash.Add(key, value);
//                                    table.Add(key, value);
                                }
                            }
                        }
                    }
                }
//                return table;
            }

            public void AddRecord(string val_inp, string encryptedVal_inp)
            {
                var ed = new Web_Encryption_Dictionary
                {
                    val = val_inp,
                    encrypted_val = encryptedVal_inp
                };

                _context.Web_Encryption_Dictionary.Add(ed);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception x)
                {
                    throw x;
                }
            }

        }




    }

}