using CBF_API.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static CBF_API.Helpers.enumeration;

namespace CBF_API.Helpers
{
    public class GNF
    {
        public static int PageSize = 100;
        public static LoginDetails GetLoginObjectFromCookies()
        {
            LoginDetails objLogin = new LoginDetails();

            return objLogin;
        }
        public static string GetcurrentUserEmail()
        {
            LoginDetails objLogin = GetLoginObjectFromCookies();
            if (objLogin != null)
            {
                return objLogin.Email_Address;
            }
            else
            {
                return "";
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string RandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new System.Text.StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public static int GetMemberId()
        {
            LoginDetails objLogin = GetLoginObjectFromCookies();
            if (objLogin != null)
            {
                return Convert.ToInt32(objLogin.Member_Id);
            }

            else
            {
                return 0;
            }
        }

        public static DateTime GetFormattedDateDDMMYYYY(string dateString)
        {
            // CultureInfo.InvariantCulture

            DateTime outputDateTimeValue;
            if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outputDateTimeValue))
            {
                return outputDateTimeValue;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        public static void SaveException(Exception ex, CbfDbContext _context)
        {
            try
            {
                ErrorException objEexception = new ErrorException();

                objEexception.Level = eLevel.Low.ToString();
                //objEexception.Logger = Request.AppRelativeCurrentExecutionFilePath;
                objEexception.Thread = ex.StackTrace.ToString();
                objEexception.Message = ex.Message;
                objEexception.Exception = ex.ToString();
                objEexception.Context = ex.Source;
                objEexception.date = DateTime.Now;
                _context.ErrorExceptions.Add(objEexception);
                _context.Entry(objEexception).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _context.SaveChanges();

                string html = "";
                html += "<table width='100%' style='font-size:12px' border='1' cellpadding='10'><tr>";
                html += "<td width='150px'><b>Message:</b></td><td>" + objEexception.Message + "</td>";
                html += "</tr><tr>";

                html += "<td><b>Level:</b></td><td>" + objEexception.Level + "</td>";
                html += "</tr><tr>";
                html += "<td><b>Logger:</b></td><td>" + objEexception.Logger + "</td>";
                html += "</tr><tr>";
                html += "<td><b>Thread:</b></td><td>" + objEexception.Thread + "</td>";
                html += "</tr><tr>";
                html += "<td><b>Exception:</b></td><td>" + objEexception.Exception + "</td>";
                html += "</tr><tr>";
                html += "<td><b>Context:</b></td><td>" + objEexception.Context + "</td>";
                html += "</tr></table>";


                string msg = "<h5>Following error occured</h5> ";

                EmailHelper.SendMail("askdev@nowgray.com", "Critical!!!!! Exception Occured", msg + html, true);

            }
            catch (Exception exe)
            {

            }
        }
        public static void SaveMailingList(Members objUser, CbfDbContext _context)
        {
            try
            {
                MailingList objEmail = _context.MailingList.FirstOrDefault(x => x.Email == objUser.Email_Address);
                if (objEmail != null)
                {
                    return;
                }

                MailingList objMailing = new MailingList();
                objMailing.Active = true;
                objMailing.CreatedOn = DateTime.Now;
                objMailing.Email = objUser.Email_Address;
                objMailing.Referer = "clubbigfun.com";
                objMailing.MailingList_ID = 0;
                _context.MailingList.Add(objMailing);
                _context.Entry(objMailing).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _context.SaveChanges();
            }
            catch (Exception exe)
            {

            }
        }
    }
}








public class Encryption64
{
    static private byte[] key = {

    };
    static private byte[] IV = {
        0x12,
        0x34,
        0x56,
        0x78,
        0x90,
        0xab,
        0xcd,
        0xef

    };
    public static string DecryptString(string stringToDecrypt, string sEncryptionKey)
    {
        byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
        try
        {
            //key = System.Text.Encoding.UTF8.GetBytes(Strings.Left(sEncryptionKey, 8));
            key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(stringToDecrypt);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());



        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    public static string EncryptString(string stringToEncrypt, string SEncryptionKey)
    {
        try
        {
            //key = System.Text.Encoding.UTF8.GetBytes(Strings.Left(SEncryptionKey, 8));
            key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }



}


