using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace BillPayment.Utils
{

    public class Helpers
    {
        private static Random random = new Random();
        public static string GeneratePassword(int length) //length of salt    
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string EncodePassword(string pass, string salt) //encrypt password    
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            //return Convert.ToBase64String(inArray);    
            return EncodePasswordMd5(Convert.ToBase64String(inArray));
        }
        public static string EncodePasswordMd5(string pass) //Encrypt using MD5    
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string    
            return BitConverter.ToString(encodedBytes);
        }
    }
    public class LoginInfo
    {
        private HttpSessionState _session;
        

        public string Username
        {
            get { return (this._session["Username"] ?? string.Empty).ToString(); }
            set {

                var userType = (string)this._session["Username"] ?? string.Empty;
                if (userType == null)
                {
                    _session["Username"] = value;
                }
                else
                {
                    // test "2" etc
                }
                 }
        }

        public string FullName
        {
            get { return (this._session["FullName"] ?? string.Empty).ToString(); }
            set { this._session["FullName"] = value; }
        }
        public int ID
        {
            get { return Convert.ToInt32((this._session["UID"] ?? -1)); }
            set { this._session["UID"] = value; }
        }

        public string Role
        {
            get { return (this._session["Role"] ?? string.Empty).ToString(); }
            set { this._session["Role"] = value; }
        }

    }
}