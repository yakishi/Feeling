using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// 暗号化用のクラス
/// </summary>
public class Rijndael
{
    RijndaelManaged rijndael;

    string password_;
    string salt_;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="password">暗号化用のパス</param>
    /// <param name="salt">salt文字列</param>
    /// <param name="keySize">暗号化の鍵長</param>
    /// <param name="blockSize">ブロック長</param>
    public Rijndael(string password = "gl(4Dt(E+&U7jdJ/65H7", string salt = "saltword", int keySize = 128, int blockSize = 128)
    {
        rijndael = new RijndaelManaged();

        password_ = password;
        salt_ = salt;
        rijndael.KeySize = keySize;
        rijndael.BlockSize = blockSize;

        var bSalt = Encoding.UTF8.GetBytes(salt_);
        Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password_, bSalt);
        deriveBytes.IterationCount = 1000;

        rijndael.Key = deriveBytes.GetBytes(rijndael.KeySize / 8);
        rijndael.IV = deriveBytes.GetBytes(rijndael.BlockSize / 8);
    }
    
    /// <summary>
    /// 暗号化
    /// </summary>
    /// <param name="str">暗号化する文字列</param>
    /// <returns>暗号化されたデータ</returns>
    public byte[] encryption(string str)
    {
        var src = Encoding.UTF8.GetBytes(str);

        // 暗号化
        ICryptoTransform encryptor = rijndael.CreateEncryptor();
        byte[] encrypted = encryptor.TransformFinalBlock(src, 0, src.Length);
        encryptor.Dispose();
        return encrypted;
    }

    public string decryption(byte[] src)
    {
        var decryptor = rijndael.CreateDecryptor();
        var plain = decryptor.TransformFinalBlock(src, 0, src.Length);
        decryptor.Dispose();
        return Encoding.UTF8.GetString(plain);
    }
}
