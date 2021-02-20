using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;

namespace CBF_API.Models
{
    public class Encryption
    {
        public const string passkey = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    //    private static string EncryptionKey
    //    {
    //        get
    //        {

    //            return "Lala456birjuABCjhakjsdhahufhiuahfiuhausghf";
    //        }
    //    }
    //    private class Encryption64
    //    {
    //        static private byte[] key = {

    //};
    //        static private byte[] IV = {
    //    0x12,
    //    0x34,
    //    0x56,
    //    0x78,
    //    0x90,
    //    0xab,
    //    0xcd,
    //    0xef

    //};
    //        //public static string DecryptString(string stringToDecrypt, string sEncryptionKey)
    //        //{
    //        //    byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
    //        //    try
    //        //    {
    //        //        //key = System.Text.Encoding.UTF8.GetBytes(Strings.Left(sEncryptionKey, 8));
    //        //        key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
    //        //        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
    //        //        inputByteArray = Convert.FromBase64String(stringToDecrypt);
    //        //        MemoryStream ms = new MemoryStream();
    //        //        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
    //        //        cs.Write(inputByteArray, 0, inputByteArray.Length);
    //        //        cs.FlushFinalBlock();
    //        //        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
    //        //        return encoding.GetString(ms.ToArray());



    //        //    }
    //        //    catch (Exception e)
    //        //    {
    //        //        return e.Message;
    //        //    }
    //        //}
    //        //public static string EncryptString(string stringToEncrypt, string SEncryptionKey)
    //        //{
    //        //    try
    //        //    {
    //        //        //key = System.Text.Encoding.UTF8.GetBytes(Strings.Left(SEncryptionKey, 8));
    //        //        key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey.Substring(0, 8));
    //        //        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
    //        //        byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(stringToEncrypt);
    //        //        System.IO.MemoryStream ms = new MemoryStream();
    //        //        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
    //        //        cs.Write(inputByteArray, 0, inputByteArray.Length);
    //        //        cs.FlushFinalBlock();
    //        //        return Convert.ToBase64String(ms.ToArray());
    //        //    }
    //        //    catch (Exception e)
    //        //    {
    //        //        return e.Message;
    //        //    }
    //        //}
    //        //public static string EncryptData(string strData, string strKey)
    //        //{
    //        //    byte[] key = { }; //Encryption Key   
    //        //    byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
    //        //    byte[] inputByteArray;

    //        //    try
    //        //    {
    //        //        key = Encoding.UTF8.GetBytes(strKey);
    //        //        // DESCryptoServiceProvider is a cryptography class defind in c#.  
    //        //        DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();
    //        //        inputByteArray = Encoding.UTF8.GetBytes(strData);
    //        //        MemoryStream Objmst = new MemoryStream();
    //        //        CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateEncryptor(key, IV), CryptoStreamMode.Write);
    //        //        Objcs.Write(inputByteArray, 0, inputByteArray.Length);
    //        //        Objcs.FlushFinalBlock();

    //        //        return Convert.ToBase64String(Objmst.ToArray());//encrypted string  
    //        //    }
    //        //    catch (System.Exception ex)
    //        //    {
    //        //        throw ex;
    //        //    }
    //        //}
    //        //public static string DecryptData(string strData, string strKey)
    //        //{
    //        //    byte[] key = { };// Key   
    //        //    byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
    //        //    byte[] inputByteArray = new byte[strData.Length];

    //        //    try
    //        //    {
    //        //        key = Encoding.UTF8.GetBytes(strKey);
    //        //        DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();
    //        //        inputByteArray = Convert.FromBase64String(strData);

    //        //        MemoryStream Objmst = new MemoryStream();
    //        //        CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateDecryptor(key, IV), CryptoStreamMode.Write);
    //        //        Objcs.Write(inputByteArray, 0, inputByteArray.Length);
    //        //        Objcs.FlushFinalBlock();

    //        //        Encoding encoding = Encoding.UTF8;
    //        //        return encoding.GetString(Objmst.ToArray());
    //        //    }
    //        //    catch (System.Exception ex)
    //        //    {
    //        //        throw ex;
    //        //    }
    //        //}
    //    }
        //private static string EncryptString(string input)
        //{
        //    return Encryption64.EncryptString(input, EncryptionKey);

        //}
        //private static string DecryptString(string input)
        //{
        //    return Encryption64.DecryptString(input, EncryptionKey);
        //}



        ///<summary>
        /// Base 64 Encoding with URL and Filename Safe Alphabet using UTF-8 character set.
        ///</summary>
        ///<param name="str">The origianl string</param>
        ///<returns>The Base64 encoded string</returns>
        //private static string Base64ForUrlEncode(string str)
        //{
        //    //str= EncryptString(str);
        //    byte[] encbuff = Encoding.UTF8.GetBytes(str);
        //    return WebUtility.HtmlEncode(str);
        //}
        /////<summary>
        ///// Decode Base64 encoded string with URL and Filename Safe Alphabet using UTF-8.
        /////</summary>
        /////<param name="str">Base64 code</param>
        /////<returns>The decoded string.</returns>
        //private static string Base64ForUrlDecode(string str)
        //{

        //     str = WebUtility.HtmlDecode(str);
        //    //str = DecryptString(str);

        //    return str;
        //}


        ///<summary>
        /// Base 64 Encoding with URL and Filename Safe Alphabet using UTF-8 character set.
        ///</summary>
        ///<param name="str">The origianl string</param>
        ///<returns>The Base64 encoded string</returns>
        public static string Base64Encrypt(string str)
        {
            // str = EncryptString(str);
            // byte[] encbuff = Encoding.UTF8.GetBytes(str);
            return str;// WebUtility.UrlEncode(str);
        }
        ///<summary>
        /// Decode Base64 encoded string with URL and Filename Safe Alphabet using UTF-8.
        ///</summary>
        ///<param name="str">Base64 code</param>
        ///<returns>The decoded string.</returns>
        public static string Base64Decrypt(string str)
        {

           // str = WebUtility.UrlDecode(str);
           // str = DecryptString(str);

            return str;
        }

        public static string EncryptString(string Message, string Passphrase = passkey)
        {
            byte[] Results;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;

            TDESAlgorithm.Mode = CipherMode.ECB;

            TDESAlgorithm.Padding = PaddingMode.PKCS7;


            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);

            }

            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();

            }
            return Convert.ToBase64String(Results);

        }
        public static string DecryptString(string Message, string Passphrase = passkey)
        {

            byte[] Results;

            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;

            TDESAlgorithm.Mode = CipherMode.ECB;

            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }

            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return UTF8.GetString(Results);

        }
    }

}
