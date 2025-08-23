// Decompiled with JetBrains decompiler
// Type: TaxProEWB.API.TPCrypto
// Assembly: TaxProEWB.API, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 651C2957-9A00-43E1-9864-7C8CEF88DD73
// Assembly location: C:\inetpub\wwwroot\TaxProEWBApiIntigrationDemo.NET\bin\x86\Debug\TaxProEWB.API.dll

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TaxProEWB.API
{
  public static class TPCrypto
  {
    public static X509Certificate2 GSTcert;

    public static string EncryptUsingPubKey(byte[] StringToEnc)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Assembly.GetExecutingAssembly().GetManifestResourceStream("TaxProEWB.API.Tools.ewaybill_public.cer").CopyTo((Stream) memoryStream);
        TPCrypto.GSTcert = new X509Certificate2(memoryStream.ToArray());
      }
      return Convert.ToBase64String((TPCrypto.GSTcert.PublicKey.Key as RSACryptoServiceProvider).Encrypt(StringToEnc, false));
    }

    public static byte[] GenAesKey()
    {
      AesManaged aesManaged = new AesManaged();
      aesManaged.KeySize = 256;
      aesManaged.GenerateKey();
      return aesManaged.Key;
    }

    public static string AesEncryptBase64(string payLoad, string key)
    {
      using (AesManaged aesManaged = new AesManaged())
      {
        aesManaged.Key = Convert.FromBase64String(key);
        aesManaged.Mode = CipherMode.ECB;
        aesManaged.Padding = PaddingMode.PKCS7;
        byte[] inputBuffer = Convert.FromBase64String(payLoad);
        return Convert.ToBase64String(aesManaged.CreateEncryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
      }
    }

    public static string AesEncryptUTF8(string payLoad, byte[] key)
    {
      using (AesManaged aesManaged = new AesManaged())
      {
        aesManaged.Key = key;
        aesManaged.BlockSize = 128;
        aesManaged.Mode = CipherMode.ECB;
        aesManaged.Padding = PaddingMode.PKCS7;
        byte[] bytes = Encoding.UTF8.GetBytes(payLoad);
        return Convert.ToBase64String(aesManaged.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
      }
    }

    public static string AesDecryptBase64(string respLoad, string key)
    {
      using (AesManaged aesManaged = new AesManaged())
      {
        aesManaged.Padding = PaddingMode.PKCS7;
        aesManaged.Mode = CipherMode.ECB;
        byte[] inputBuffer = Convert.FromBase64String(respLoad);
        return Convert.ToBase64String(aesManaged.CreateDecryptor(Convert.FromBase64String(key), (byte[]) null).TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
      }
    }

    public static string AesDecryptData(string respLoad, string key)
    {
      return Encoding.UTF8.GetString(Convert.FromBase64String(TPCrypto.AesDecryptBase64(respLoad, key)));
    }
  }
}
