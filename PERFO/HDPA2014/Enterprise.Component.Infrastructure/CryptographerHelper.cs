using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

//using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Enterprise.Component.Infrastructure
{

    /// <summary>
    /// 加解密操作方法
    /// </summary>
    public class CryptographerHelper
    {
        /// <summary>
        /// 企业库Md5加密 不可逆
        /// </summary>
        /// <param name="plaintext">待加密</param>
        /// <returns></returns>
        public string Md5(string plaintext)
        {
            return "";//Cryptographer.CreateHash("MD5Cng", plaintext);
        }

        /// <summary>
        /// 企业库对称加密方法 可逆
        /// </summary>
        /// <param name="plaintext">待加密</param>
        /// <returns></returns>
        public string Encrypt(string plaintext)
        {
            return "";//Cryptographer.EncryptSymmetric("DPAPI Symmetric Crypto Provider", plaintext);
        }

        /// <summary>
        /// 企业库对称解密方法 可逆
        /// </summary>
        /// <param name="plaintext">待加密</param>
        /// <returns></returns>
        public string Decrypt(string plaintext)
        {
            return "";//Cryptographer.DecryptSymmetric("DPAPI Symmetric Crypto Provider", plaintext);
        }
    }


    /// <summary>
    /// 3DES 加密与解密（只支持UTF-8编码）
    /// </summary>
    public class Crypto3DES
    {
        
        private System.Text.Encoding encoding;

        /// <summary>
        /// 获取密匙
        /// </summary>
        public string Key
        {
            get
            {
                return "VavicApp";
            }
        }

        /// <summary>
        /// 获取或设置加密解密的编码
        /// </summary>
        public System.Text.Encoding Encoding
        {
            get
            {
                if (encoding == null)
                {
                    encoding = System.Text.Encoding.UTF8;
                }
                return encoding;
            }
            set
            {
                encoding = value;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="strString"></param>
        /// <returns></returns>
        public string Encrypt3DES(string strString)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = Encoding.GetBytes(this.Key);
            DES.Mode = CipherMode.ECB;
            DES.Padding = PaddingMode.Zeros;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            byte[] Buffer = encoding.GetBytes(strString);
            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="strString"></param>
        /// <returns></returns>
        public string Decrypt3DES(string strString)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = Encoding.UTF8.GetBytes(this.Key);
            DES.Mode = CipherMode.ECB;
            DES.Padding = PaddingMode.Zeros;
            ICryptoTransform DESDecrypt = DES.CreateDecryptor();
            byte[] Buffer = Convert.FromBase64String(strString);
            return UTF8Encoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

    }
}
