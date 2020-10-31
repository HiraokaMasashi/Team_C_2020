using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;
using System.Security.Cryptography;

public class Cryptor
{
    static readonly int KeySize = 256;
    static readonly int BlockSize = 128;
    static readonly string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
    static readonly string EncryptionIV = "0123456789ABCDEF";

    /// <summary>
    /// データを規定のパラメータを用いて暗号化
    /// </summary>
    /// <param name="rawData"></param>
    /// <returns></returns>
    public static byte[] Encrypt(byte[] rawData)
    {
        return Encrypt(rawData, EncryptionKey, EncryptionIV);
    }

    /// <summary>
    /// 指定された暗号化キーと初期化ベクトルを利用してデータを暗号化
    /// </summary>
    /// <param name="rawData"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static byte[] Encrypt(byte[] rawData, string key, string iv)
    {
        byte[] result = null;

        using (AesManaged aes = new AesManaged())
        {
            SetAesParams(aes, key, iv);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream encryptedStream = new MemoryStream())
            {
                using (CryptoStream cryptStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptStream.Write(rawData, 0, rawData.Length);
                }
                result = encryptedStream.ToArray();
            }
        }

        return result;
    }

    /// <summary>
    /// データを規定のパラメータを用いて復号化
    /// </summary>
    /// <param name="encryptedData"></param>
    /// <returns></returns>
    public static byte[] Decrypt(byte[] encryptedData)
    {
        return Decrypt(encryptedData, EncryptionKey, EncryptionIV);
    }

    /// <summary>
    /// 指定された暗号化キーと初期化ベクトルを利用してデータを復号化
    /// </summary>
    /// <param name="encryptedData"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static byte[] Decrypt(byte[] encryptedData, string key, string iv)
    {
        byte[] result = null;

        using (AesManaged aes = new AesManaged())
        {
            SetAesParams(aes, key, iv);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream encryptedStream = new MemoryStream(encryptedData))
            {
                using (MemoryStream decryptedStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(decryptedStream);
                    }
                    result = decryptedStream.ToArray();
                }
            }
        }

        return result;
    }

    static void SetAesParams(AesManaged aes, string key, string iv)
    {
        aes.KeySize = KeySize;
        aes.BlockSize = BlockSize;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        aes.Key = Encoding.UTF8.GetBytes(CreateKeyFromString(key));
        aes.IV = Encoding.UTF8.GetBytes(CreateIVFromString(iv));
    }

    static string CreateKeyFromString(string str)
    {
        return PaddingString(str, KeySize / 8);
    }

    static string CreateIVFromString(string str)
    {
        return PaddingString(str, BlockSize / 8);
    }

    static string PaddingString(string str, int len)
    {
        const char PaddingCharacter = '.';

        if (str.Length < len)
        {
            string key = str;
            for (int i = 0; i < len - str.Length; ++i)
            {
                key += PaddingCharacter;
            }
            return key;
        }
        else if (str.Length > len)
        {
            return str.Substring(0, len);
        }
        else
        {
            return str;
        }
    }
}