using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class ExtensionMethods
{
    #region Seo - HtmlTagControl
    public static string ClearHtmlTag(this string data)
    {
        string Temp;
        Temp = data.ToLower();
        Temp = Temp.Replace(" ", "-");
        Temp = Temp.Replace("\"", "");
        Temp = Temp.Replace("/", "");
        Temp = Temp.Replace("(", "");
        Temp = Temp.Replace(")", "");
        Temp = Temp.Replace("{", "");
        Temp = Temp.Replace("}", "");
        Temp = Temp.Replace("%", "");
        Temp = Temp.Replace("&", "");
        Temp = Temp.Replace("+", "");
        Temp = Temp.Replace("?", "");
        Temp = Temp.Replace(",", "");
        Temp = Temp.Replace("%20", "");
        Temp = Temp.Replace("'", "");
        Temp = Temp.Replace(":", "");
        Temp = Temp.Replace("!", "");
        Temp = Temp.Replace(";", "");
        Temp = Temp.Replace("#", "");
        Temp = Temp.Replace("*", "");
        Temp = Temp.Replace("&", "");
        Temp = Temp.Replace("″", "-");
        Temp = Temp.Replace(">", "");
        Temp = Temp.Replace("<", "");
        Temp = Temp.Replace("=", "");
        Temp = Temp.Replace("|", "");
        Temp = Temp.Replace("~", "");
        Temp = Temp.Replace("€", "");
        Temp = Temp.Replace("£", "");
        Temp = Temp.Replace("$", "");

        return Temp;
    }
    public static string ClearHtmlTagAndCharacter(this string Metin)
    {
        string Temp;
        Temp = Metin.ClearHtmlTag();

        Temp = Temp.Replace("ç", "c");
        Temp = Temp.Replace("Ç", "C");
        Temp = Temp.Replace("ğ", "g");
        Temp = Temp.Replace("Ğ", "G");
        Temp = Temp.Replace("ı", "i");
        Temp = Temp.Replace("ü", "u");
        Temp = Temp.Replace("ö", "o");
        Temp = Temp.Replace("Ö", "O");
        Temp = Temp.Replace("Ü", "u");
        Temp = Temp.Replace("Ş", "S");
        Temp = Temp.Replace("ş", "s");

        return Temp;
    }

    public static bool IsThereTurkishCharacter(this string data,string skipCharacter = "")
    {
        string characters = "çöğşüı";
        return CharacterControl(characters, data, skipCharacter);
    }

    public static bool IsThereHtmlTag(this string data, string skipCharacter = "")
    {
        string characters = "$=<>|_-\\}]][{½$#£><é!'^+%&/()=?,:\"~€@æß£*";
        return CharacterControl(characters,data, skipCharacter);
    }

    private static bool CharacterControl(string key, string data, string skip = "null")
    {
        bool _return = false;

        if (!string.IsNullOrEmpty(data))
        {
            data = data.ToLower();

            foreach (var item in key.ToCharArray())
            {
                if (skip.IndexOf(item) == -1)
                {
                    if (data.IndexOf(item) > -1)
                    {
                        _return = true;
                        break;
                    }
                }
            }
        }

        return _return;
    }
    #endregion

    #region Encryption

    public static string MD5Hash(this string psw)
    {
        using (var md5 = MD5.Create())
        {
            var result = md5.ComputeHash(Encoding.ASCII.GetBytes(psw));
            return Encoding.ASCII.GetString(result);
        }
    }
    public static string SHA256Encrypt(this string text)
    {
        using (var sha256 = SHA256.Create())
        {
            // Send a sample text to hash.  
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
            // Get the hashed string.  
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToUpper().Substring(0, 20);
        }
    }

    public static string Encrypt(this string plainText, string password)
    {
        if (plainText == null)
        {
            return null;
        }

        if (password == null)
        {
            password = String.Empty;
        }

        // Get the bytes of the string
        var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        // Hash the password with SHA256
        passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

        return Convert.ToBase64String(bytesEncrypted);
    }

    /// <summary>
    /// Decrypt a string.
    /// </summary>
    /// <param name="encryptedText">String to be decrypted</param>
    /// <param name="password">Password used during encryption</param>
    /// <exception cref="FormatException"></exception>
    public static string Decrypt(this string encryptedText, string password)
    {
        if (encryptedText == null)
        {
            return null;
        }

        if (password == null)
        {
            password = String.Empty;
        }

        // Get the bytes of the string
        var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

        return Encoding.UTF8.GetString(bytesDecrypted);
    }

    private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
    {
        byte[] encryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                AES.KeySize = 256;
                AES.BlockSize = 128;
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

    private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
    {
        byte[] decryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.
        var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        using (MemoryStream ms = new MemoryStream())
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                AES.KeySize = 256;
                AES.BlockSize = 128;
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

    #endregion
}