// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.EncryptTo
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#nullable disable
namespace Intermech.Office.Client;

public class EncryptTo
{
  private const uint CERT_SYSTEM_STORE_CURRENT_USER = 65536 /*0x010000*/;
  private const uint CERT_STORE_READONLY_FLAG = 32768 /*0x8000*/;
  private const uint CERT_STORE_OPEN_EXISTING_FLAG = 16384 /*0x4000*/;
  private const uint CERT_FIND_SUBJECT_STR = 524295 /*0x080007*/;
  private const uint X509_ASN_ENCODING = 1;
  private const uint PKCS_7_ASN_ENCODING = 65536 /*0x010000*/;
  internal const uint RSA_CSP_PUBLICKEYBLOB = 19;
  internal const uint X509_MULTI_BYTE_UINT = 38;
  internal const uint X509_DSS_PUBLICKEY = 38;
  internal const uint X509_DSS_PARAMETERS = 39;
  internal const uint X509_DSS_SIGNATURE = 40;
  private const int AT_KEYEXCHANGE = 1;
  private const int AT_SIGNATURE = 2;
  private static uint ENCODING_TYPE = 65537 /*0x010001*/;
  private X509Certificate recipcert;
  private byte[] certkeymodulus;
  private byte[] certkeyexponent;
  private uint certkeysize;
  private bool verbose;

  public EncryptTo(string certName)
  {
    this.recipcert = this.GetRecipientStoreCert(certName);
    this.verbose = true;
    if (this.recipcert == null)
      return;
    this.GetCertPublicKey(this.recipcert);
  }

  private X509Certificate GetRecipientStoreCert(string searchstr)
  {
    X509Certificate recipientStoreCert = (X509Certificate) null;
    IntPtr num1 = IntPtr.Zero;
    IntPtr num2 = IntPtr.Zero;
    string[] strArray = new string[1]{ "MY" };
    uint dwFlags = 114688 /*0x01C000*/;
    foreach (string cchNameString in strArray)
    {
      num1 = Win32.CertOpenStore("System", EncryptTo.ENCODING_TYPE, IntPtr.Zero, dwFlags, cchNameString);
      if (num1 == IntPtr.Zero)
      {
        Console.WriteLine("Failed to open system store {0}", (object) cchNameString);
      }
      else
      {
        num2 = Win32.CertFindCertificateInStore(num1, EncryptTo.ENCODING_TYPE, 0U, 524295U /*0x080007*/, searchstr, IntPtr.Zero);
        if (num2 != IntPtr.Zero)
        {
          recipientStoreCert = new X509Certificate(num2);
          Console.WriteLine("\nFound certificate in {0} store with SubjectName string \"{1}\"", (object) cchNameString, (object) searchstr);
          Console.WriteLine("SubjectName:\t{0}", (object) recipientStoreCert.Subject);
          break;
        }
      }
    }
    if (num2 != IntPtr.Zero)
      Win32.CertFreeCertificateContext(num2);
    if (num1 != IntPtr.Zero)
      Win32.CertCloseStore(num1, 0);
    return recipientStoreCert;
  }

  private bool GetCertPublicKey(X509Certificate cert)
  {
    byte[] publicKey = cert.GetPublicKey();
    uint cbStructInfo = 0;
    if (this.verbose)
    {
      Console.WriteLine();
      EncryptTo.showBytes("Encoded publickey", publicKey);
      Console.WriteLine();
    }
    if (Win32.CryptDecodeObject(EncryptTo.ENCODING_TYPE, 38U, publicKey, (uint) publicKey.Length, 0U, (byte[]) null, ref cbStructInfo))
    {
      byte[] numArray1 = new byte[(int) cbStructInfo];
      if (Win32.CryptDecodeObject(EncryptTo.ENCODING_TYPE, 38U, publicKey, (uint) publicKey.Length, 0U, numArray1, ref cbStructInfo) && this.verbose)
        EncryptTo.showBytes("CryptoAPI publickeyblob", numArray1);
      int num1 = Marshal.SizeOf<PUBKEYBLOBHEADERS>(new PUBKEYBLOBHEADERS());
      IntPtr num2 = Marshal.AllocHGlobal(num1);
      Marshal.Copy(numArray1, 0, num2, num1);
      PUBKEYBLOBHEADERS structure = (PUBKEYBLOBHEADERS) Marshal.PtrToStructure(num2, typeof (PUBKEYBLOBHEADERS));
      Marshal.FreeHGlobal(num2);
      if (this.verbose)
      {
        Console.WriteLine("\n ---- PUBLICKEYBLOB headers ------");
        Console.WriteLine("  btype     {0}", (object) structure.bType);
        Console.WriteLine("  bversion  {0}", (object) structure.bVersion);
        Console.WriteLine("  reserved  {0}", (object) structure.reserved);
        Console.WriteLine("  aiKeyAlg  0x{0:x8}", (object) structure.aiKeyAlg);
        string str = new ASCIIEncoding().GetString(BitConverter.GetBytes(structure.magic));
        Console.WriteLine("  magic     0x{0:x8}     '{1}'", (object) structure.magic, (object) str);
        Console.WriteLine("  bitlen    {0}", (object) structure.bitlen);
        Console.WriteLine("  pubexp    {0}", (object) structure.pubexp);
        Console.WriteLine(" --------------------------------");
      }
      this.certkeysize = structure.bitlen;
      byte[] bytes = BitConverter.GetBytes(structure.pubexp);
      Array.Reverse((Array) bytes);
      this.certkeyexponent = bytes;
      if (this.verbose)
        EncryptTo.showBytes("\nPublic key exponent (big-endian order):", bytes);
      int length = (int) structure.bitlen / 8;
      byte[] numArray2 = new byte[length];
      try
      {
        Array.Copy((Array) numArray1, num1, (Array) numArray2, 0, length);
        Array.Reverse((Array) numArray2);
        this.certkeymodulus = numArray2;
        if (this.verbose)
          EncryptTo.showBytes("\nPublic key modulus  (big-endian order):", numArray2);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Problem getting modulus from publickeyblob");
        return false;
      }
      return true;
    }
    Console.WriteLine("Couldn't decode publickeyblob from certificate publickey");
    return false;
  }

  private bool TripleDESEncrypt(
    string content,
    string encContent,
    string encKeyfile,
    string encIVfile)
  {
    FileStream fileStream1 = new FileStream(content, FileMode.Open, FileAccess.Read);
    FileStream fileStream2 = new FileStream(encContent, FileMode.OpenOrCreate, FileAccess.Write);
    byte[] buffer = new byte[1000];
    try
    {
      TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
      CryptoStream cryptoStream = new CryptoStream((Stream) fileStream2, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
      Console.WriteLine("\nEncrypting content ... ");
      int count;
      while ((count = fileStream1.Read(buffer, 0, 1000)) > 0)
        cryptoStream.Write(buffer, 0, count);
      cryptoStream.Close();
      Console.WriteLine("Encrypting 3DES Key and IV ... ");
      byte[] data1 = this.DoRSAEncrypt(cryptoServiceProvider.Key, (byte[]) this.certkeymodulus.Clone(), (byte[]) this.certkeyexponent.Clone());
      if (data1 == null)
        return false;
      this.PutFileBytes(encKeyfile, data1, data1.Length);
      byte[] data2 = this.DoRSAEncrypt(cryptoServiceProvider.IV, (byte[]) this.certkeymodulus.Clone(), (byte[]) this.certkeyexponent.Clone());
      if (data2 == null)
        return false;
      this.PutFileBytes(encIVfile, data2, data2.Length);
      return true;
    }
    catch (Exception ex)
    {
      return false;
    }
  }

  private byte[] DoRSAEncrypt(byte[] keydata, byte[] modulus, byte[] exponent)
  {
    if (keydata == null || modulus == null || exponent == null)
      return (byte[]) null;
    try
    {
      RSAParameters parameters = new RSAParameters();
      parameters.Modulus = modulus;
      parameters.Exponent = exponent;
      RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
      cryptoServiceProvider.ImportParameters(parameters);
      return cryptoServiceProvider.Encrypt(keydata, false);
    }
    catch (CryptographicException ex)
    {
      return (byte[]) null;
    }
  }

  private static void usage()
  {
    Console.WriteLine("\nUsage:\nEncryptTo.exe [ContentFile] [outFile] [outKeyfile] [outIVfile]");
  }

  private void PutFileBytes(string outfile, byte[] data, int bytes)
  {
    FileStream fileStream = (FileStream) null;
    if (bytes > data.Length)
    {
      Console.WriteLine("Too many bytes");
    }
    else
    {
      try
      {
        fileStream = new FileStream(outfile, FileMode.Create);
        fileStream.Write(data, 0, bytes);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      finally
      {
        fileStream.Close();
      }
    }
  }

  private static void showBytes(string info, byte[] data)
  {
    Console.WriteLine("{0}  [{1} bytes]", (object) info, (object) data.Length);
    for (int index = 1; index <= data.Length; ++index)
    {
      Console.Write("{0:X2}  ", (object) data[index - 1]);
      if (index % 16 /*0x10*/ == 0)
        Console.WriteLine();
    }
    Console.WriteLine();
  }
}
