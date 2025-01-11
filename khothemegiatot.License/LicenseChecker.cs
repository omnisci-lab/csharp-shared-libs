using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

#nullable disable

namespace khothemegiatot.License;

public class LicenseChecker
{
    private static readonly byte[] Key = Convert.FromBase64String("Re0fHFtvKq+iJYVhCpBcG+GycJoR/6JaUe+VuTrXMbM="); // 32 bytes
    private static readonly byte[] IV = Convert.FromBase64String("vikWrXWXXl2SUqD+DwZpBQ=="); // 16 bytes

    public static async Task<JObject> GetLicenseJsonAsync(string host, string key)
    {
        string endpoint = $"/api/license/get/";
        using HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri(host)
        };

        var formData = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("key", key)
        };

        FormUrlEncodedContent content = new FormUrlEncodedContent(formData);
        HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);
        if (!response.IsSuccessStatusCode)
            return null;

        string jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<JObject>(jsonResponse);
    }

    public static async Task SaveLicenseAsync(string filePath, string licenseInfo)
    {
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;
        aes.Mode = CipherMode.CBC; // Chế độ CBC
        aes.Padding = PaddingMode.PKCS7; // Chế độ Padding

        using MemoryStream ms = new MemoryStream();
        using CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
        using StreamWriter writer = new StreamWriter(cs);

        writer.Write(licenseInfo); // Mã hóa văn bản
        writer.Close();
        await File.WriteAllBytesAsync(filePath, ms.ToArray());
    }

    public static async Task<string> LoadLicenseAsync(string filePath)
    {
        byte[] encryptedBytes = await File.ReadAllBytesAsync(filePath);

        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;
        aes.Mode = CipherMode.CBC; // Chế độ CBC
        aes.Padding = PaddingMode.PKCS7; // Chế độ Padding

        using MemoryStream ms = new MemoryStream(encryptedBytes);
        using CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader reader = new StreamReader(cs);

        string decryptedKey = await reader.ReadToEndAsync();
        return decryptedKey;
    }


    public static async Task<bool> GetAndSaveLicenseAsync(string key)
    {
        JObject license = await GetLicenseJsonAsync(AppLicense.LicenseMgrHost, key);

        if (license is null)
            return false;

        if ((bool?)license["isActive"] == true)
        {
            await SaveLicenseAsync("license.dat", license.ToString());
            return true;
        }

        return false;
    }

    public static async Task<bool> ValidateLicense()
    {
        string licenseJson = await LoadLicenseAsync("license.dat");
        if (string.IsNullOrEmpty(licenseJson))
            return false;

        try
        {
            JObject licenseJObject = JsonConvert.DeserializeObject<JObject>(licenseJson);
            return (bool)licenseJObject["isActive"];
        }
        catch (JsonException)
        {
            return false;
        }
    }
}