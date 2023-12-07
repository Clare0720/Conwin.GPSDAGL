using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Framework
{
    public class TripleDESEncode
    {
        private const string DEFAULT_KEY = "{A090CB24-AF38-4544-92F8-A5B9F1A36ABC}";

        private readonly TripleDESService _TripleDESService = null;

        public TripleDESEncode()
            : this(DEFAULT_KEY)
        {

        }

        public TripleDESEncode(string desKey)
        {
            _TripleDESService = new TripleDESService(desKey);
        }


        public string Encode(string value)
        {
            return _TripleDESService.Encrypt(value);
        }

        public string Decode(string value)
        {
            return _TripleDESService.Decrypt(value);
        }


        public byte[] DecodeToBytes(string value)
        {
            throw new NotImplementedException();
        }


        public string EncodeFromBytes(byte[] value)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 三重DES.
    /// </summary>
    internal class TripleDESService : IDisposable
    {
        private TripleDES mydes;
        /// <summary>
        /// 密钥值.
        /// </summary>
        public string Key;
        /// <summary>
        /// 初始向量值.
        /// </summary>
        public string IV;

        /// <summary>
        /// 对称加密类的构造函数.
        /// </summary>
        public TripleDESService(string key)
        {
            mydes = new TripleDESCryptoServiceProvider();
            Key = key;
            IV = "#$^%&&*Yisifhsfjsljfslhgosdshf26382837sdfjskhf97(*&(*";
        }

        /// <summary>
        /// 对称加密类的构造函数.
        /// </summary>
        public TripleDESService(string key, string iv)
        {
            mydes = new TripleDESCryptoServiceProvider();
            Key = key;
            IV = iv;
        }

        /// <summary>
        /// 获得密钥.
        /// </summary>
        /// <returns>密钥.</returns>
        private byte[] GetLegalKey()
        {
            string sTemp = Key;
            mydes.GenerateKey();
            byte[] bytTemp = mydes.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
                sTemp = sTemp.Substring(0, KeyLength);
            else if (sTemp.Length < KeyLength)
                sTemp = sTemp.PadRight(KeyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 获得初始向量 IV.
        /// </summary>
        /// <returns>初试向量 IV.</returns>
        private byte[] GetLegalIV()
        {
            string sTemp = IV;
            mydes.GenerateIV();
            byte[] bytTemp = mydes.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
                sTemp = sTemp.Substring(0, IVLength);
            else if (sTemp.Length < IVLength)
                sTemp = sTemp.PadRight(IVLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }

        /// <summary>
        /// 加密方法.
        /// </summary>
        /// <param name="Source">待加密的串.</param>
        /// <returns>经过加密的串.</returns>
        public string Encrypt(string Source)
        {
            try
            {
                byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
                MemoryStream ms = new MemoryStream();
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                throw new Exception("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 解密方法.
        /// </summary>
        /// <param name="Source">待解密的串.</param>
        /// <returns>经过解密的串.</returns>
        public string Decrypt(string Source)
        {
            try
            {
                byte[] bytIn = Convert.FromBase64String(Source);
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 加密方法byte[] to byte[].
        /// </summary>
        /// <param name="Source">待加密的byte数组.</param>
        /// <returns>经过加密的byte数组.</returns>
        public byte[] Encrypt(byte[] Source)
        {
            try
            {
                byte[] bytIn = Source;
                MemoryStream ms = new MemoryStream();
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                ms.Close();
                byte[] bytOut = ms.ToArray();
                return bytOut;
            }
            catch (Exception ex)
            {
                throw new Exception("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 解密方法byte[] to byte[].
        /// </summary>
        /// <param name="Source">待解密的byte数组.</param>
        /// <returns>经过解密的byte数组.</returns>
        public byte[] Decrypt(byte[] Source)
        {
            try
            {
                byte[] bytIn = Source;
                MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                return UTF8Encoding.UTF8.GetBytes(sr.ReadToEnd());
            }
            catch (Exception ex)
            {
                throw new Exception("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 加密方法File to File.
        /// </summary>
        /// <param name="inFileName">待加密文件的路径.</param>
        /// <param name="outFileName">待加密后文件的输出路径.</param>

        public void Encrypt(string inFileName, string outFileName)
        {
            try
            {

                FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                fout.SetLength(0);

                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();

                byte[] bin = new byte[100];
                long rdlen = 0;
                long totlen = fin.Length;
                int len;

                ICryptoTransform encrypto = mydes.CreateEncryptor();
                CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                while (rdlen < totlen)
                {
                    len = fin.Read(bin, 0, 100);
                    cs.Write(bin, 0, len);
                    rdlen = rdlen + len;
                }
                cs.Close();
                fout.Close();
                fin.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 解密方法File to File.
        /// </summary>
        /// <param name="inFileName">待解密文件的路径.</param>
        /// <param name="outFileName">待解密后文件的输出路径.</param>
        public void Decrypt(string inFileName, string outFileName)
        {
            try
            {
                FileStream fin = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
                FileStream fout = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);
                fout.SetLength(0);

                byte[] bin = new byte[100];
                long rdlen = 0;
                long totlen = fin.Length;
                int len;
                mydes.Key = GetLegalKey();
                mydes.IV = GetLegalIV();
                ICryptoTransform encrypto = mydes.CreateDecryptor();
                CryptoStream cs = new CryptoStream(fout, encrypto, CryptoStreamMode.Write);
                while (rdlen < totlen)
                {
                    len = fin.Read(bin, 0, 100);
                    cs.Write(bin, 0, len);
                    rdlen = rdlen + len;
                }
                cs.Close();
                fout.Close();
                fin.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// 销毁.
        /// </summary>
        public void Dispose()
        {
            //;
        }

        #endregion
    }


    internal static class DESHelper
    {
        private static byte[] Keys = Encoding.UTF8.GetBytes("{A090CB24-AF38-4544-92F8-A5B9F1A36ABC}");
        private const string key = "av&6^3*E";

        public static string Encrypt(string source)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] bytes = Encoding.UTF8.GetBytes(source);

            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();

            StringBuilder sb = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }

        public static string Decrypt(string source)
        {
            if (source == null || source.Length == 0)
            {
                return source;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] bytes = new byte[source.Length / 2];
            for (int x = 0; x < source.Length / 2; x++)
            {
                int i = (Convert.ToInt32(source.Substring(x * 2, 2), 16));
                bytes[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();

            return Encoding.UTF8.GetString(ms.ToArray());
        }


        ///// <summary>         
        ///// DES加密字符串         
        ///// </summary>        
        ///// <param name="encryptString">待加密的字符串</param>    
        ///// <param name="encryptKey">加密密钥,要求为8位</param>  
        ///// <returns>加密成功返回加密后的字符串，失败返回源串</returns>   
        //public static string Encrypt(string encryptString, string encryptKey)
        //{
        //    try
        //    {
        //        byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
        //        byte[] rgbIV = Keys;
        //        byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
        //        DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
        //        MemoryStream mStream = new MemoryStream();
        //        CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        //        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        //        cStream.FlushFinalBlock();
        //        return Convert.ToBase64String(mStream.ToArray());
        //    }
        //    catch
        //    {
        //        return encryptString;
        //    }
        //}
        ///// <summary>   
        ///// DES解密字符串      
        ///// </summary>     
        ///// <param name="decryptString">待解密的字符串</param>     
        ///// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>  
        ///// <returns>解密成功返回解密后的字符串，失败返源串</returns>     
        //public static string Decrypt(string decryptString, string decryptKey)
        //{
        //    try
        //    {
        //        byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
        //        byte[] rgbIV = Keys;
        //        byte[] inputByteArray = Convert.FromBase64String(decryptString);
        //        DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
        //        MemoryStream mStream = new MemoryStream();
        //        CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        //        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        //        cStream.FlushFinalBlock();
        //        return Encoding.UTF8.GetString(mStream.ToArray());
        //    }
        //    catch
        //    {
        //        return decryptString;
        //    }
        //}
    }
}
