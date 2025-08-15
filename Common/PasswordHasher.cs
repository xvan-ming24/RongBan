using System;
using System.Security.Cryptography;
using System.Text;

public class PasswordHasher
{
    // 生成随机盐（通常16-32字节）
    public static byte[] GenerateSalt(int size = 32)
    {
        byte[] salt = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    // 计算密码的哈希值（盐 + 密码）
    public static string HashPassword(string password, byte[] salt)
    {
        using (var sha256 = SHA256.Create())
        {
            // 将盐和密码拼接
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];
            Array.Copy(salt, 0, saltedPassword, 0, salt.Length);
            Array.Copy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

            // 计算哈希
            byte[] hash = sha256.ComputeHash(saltedPassword);

            // 返回盐和哈希的Base64字符串（便于存储）
            return Convert.ToBase64String(salt) + "|" + Convert.ToBase64String(hash);
        }
    }
    // 验证密码
    public static bool VerifyPassword(string inputPassword, string storedHashWithSalt)
    {
        // 从存储的字符串中解析盐和哈希
        string[] parts = storedHashWithSalt.Split('|');
        if (parts.Length != 2)
            return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] storedHash = Convert.FromBase64String(parts[1]);

        // 用相同的盐计算输入密码的哈希
        using (var sha256 = SHA256.Create())
        {
            byte[] inputPasswordBytes = Encoding.UTF8.GetBytes(inputPassword);
            byte[] saltedInputPassword = new byte[salt.Length + inputPasswordBytes.Length];
            Array.Copy(salt, 0, saltedInputPassword, 0, salt.Length);
            Array.Copy(inputPasswordBytes, 0, saltedInputPassword, salt.Length, inputPasswordBytes.Length);

            byte[] computedHash = sha256.ComputeHash(saltedInputPassword);

            // 比较哈希值（安全的方式：逐字节比较）
            return CompareByteArrays(storedHash, computedHash);
        }
    }

    // 安全比较两个字节数组（防止时序攻击）
    private static bool CompareByteArrays(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;

        int result = 0;
        for (int i = 0; i < a.Length; i++)
        {
            result |= a[i] ^ b[i];
        }

        return result == 0;
    }
}