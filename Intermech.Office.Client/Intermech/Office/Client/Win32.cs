// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.Win32
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Intermech.Office.Client;

public class Win32
{
  public const int X509_ASN_ENCODING = 1;
  public const int PKCS_7_ASN_ENCODING = 65536 /*0x010000*/;
  public static int ENCODING_TYPE = 65537 /*0x010001*/;

  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CryptSignMessage(
    ref CRYPT_SIGN_MESSAGE_PARA pSignPara,
    bool fDetachedSignature,
    int cToBeSigned,
    IntPtr[] rgpbToBeSigned,
    int[] rgcbToBeSigned,
    byte[] pbSignedBlob,
    ref int pcbSignedBlob);

  [DllImport("crypt32.dll")]
  public static extern bool CryptDecodeObject(
    uint CertEncodingType,
    uint lpszStructType,
    byte[] pbEncoded,
    uint cbEncoded,
    uint flags,
    [In, Out] byte[] pvStructInfo,
    ref uint cbStructInfo);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern IntPtr CertFindCertificateInStore(
    IntPtr hCertStore,
    uint dwCertEncodingType,
    uint dwFindFlags,
    uint dwFindType,
    [MarshalAs(UnmanagedType.LPWStr), In] string pszFindString,
    IntPtr pPrevCertCntxt);

  [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern IntPtr CertOpenStore(
    [MarshalAs(UnmanagedType.LPStr)] string storeProvider,
    uint dwMsgAndCertEncodingType,
    IntPtr hCryptProv,
    uint dwFlags,
    string cchNameString);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern bool CertCloseStore(IntPtr hCertStore, uint dwFlags);

  [DllImport("Advapi32.dll", CharSet = CharSet.Unicode)]
  internal static extern bool CryptEncrypt(
    IntPtr hKey,
    IntPtr hHash,
    bool final,
    uint flags,
    [In, Out] byte[] data,
    [In, Out] uint DataLen,
    uint MaxBufLen);

  [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern IntPtr CertOpenSystemStore(IntPtr hCryptProv, string storename);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern IntPtr CertEnumCertificatesInStore(
    IntPtr storeProvider,
    IntPtr prevCertContext);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern IntPtr CertDuplicateCertificateContext(IntPtr pCertContext);

  [DllImport("crypt32.dll")]
  public static extern bool CryptDecodeObject(
    uint CertEncodingType,
    uint lpszStructType,
    IntPtr pbEncoded,
    int cbEncoded,
    uint flags,
    IntPtr pvStructInfo,
    ref int cbStructInfo);

  [DllImport("CRYPT32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  public static extern bool CertCloseStore(IntPtr storeProvider, int flags);

  [DllImport("crypt32.dll", SetLastError = true)]
  public static extern bool CertVerifyCRLRevocation(
    uint dwCertEncodingType,
    IntPtr pCertId,
    uint cCrlInfo,
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2), In] IntPtr[] rgpCrlInfo);

  [DllImport("CRYPT32.DLL", SetLastError = true)]
  public static extern IntPtr CertCreateCRLContext(
    uint dwCertEncodingType,
    [In] byte[] pbCrlEncoded,
    [In, Out] uint cbCrlEncoded);

  [DllImport("Crypt32.dll", SetLastError = true)]
  internal static extern bool CryptVerifyDetachedMessageSignature(
    ref CRYPT_VERIFY_MESSAGE_PARA pVerifyPara,
    int dwSignerIndex,
    byte[] pbDetachedSignBlob,
    int cbDetachedSignBlob,
    int cToBeSigned,
    IntPtr[] rgpbToBeSigned,
    int[] rgcbToBeSigned,
    IntPtr ppSignerCert);

  [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  protected internal static extern bool CertFreeCertificateContext([In] IntPtr pCertContext);

  public static int CheckMessageSign(
    string sign,
    byte[] messageData,
    ref X509Certificate2 certs,
    out int lastError)
  {
    lastError = 0;
    byte[] pbDetachedSignBlob = Convert.FromBase64String(sign);
    IntPtr destination = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof (byte)) * messageData.Length);
    Marshal.Copy(messageData, 0, destination, messageData.Length);
    IntPtr[] rgpbToBeSigned = new IntPtr[1]{ destination };
    int[] rgcbToBeSigned = new int[1]{ messageData.Length };
    CRYPT_VERIFY_MESSAGE_PARA pVerifyPara = new CRYPT_VERIFY_MESSAGE_PARA();
    pVerifyPara.cbSize = Marshal.SizeOf<CRYPT_VERIFY_MESSAGE_PARA>(pVerifyPara);
    pVerifyPara.dwMsgAndCertEncodingType = Win32.ENCODING_TYPE;
    pVerifyPara.hCryptProv = IntPtr.Zero;
    pVerifyPara.pfnGetSignerCertificate = IntPtr.Zero;
    pVerifyPara.pvGetArg = IntPtr.Zero;
    GCHandle gcHandle = GCHandle.Alloc((object) IntPtr.Zero, GCHandleType.Pinned);
    try
    {
      if (!Win32.CryptVerifyDetachedMessageSignature(ref pVerifyPara, 0, pbDetachedSignBlob, pbDetachedSignBlob.Length, 1, rgpbToBeSigned, rgcbToBeSigned, gcHandle.AddrOfPinnedObject()))
      {
        lastError = Marshal.GetLastWin32Error();
        return -1;
      }
      try
      {
        X509Certificate2 certificate = new X509Certificate2((IntPtr) gcHandle.Target);
        certs = certificate;
        X509Chain x509Chain = new X509Chain()
        {
          ChainPolicy = {
            RevocationFlag = X509RevocationFlag.EntireChain,
            RevocationMode = X509RevocationMode.Offline,
            VerificationFlags = X509VerificationFlags.NoFlag,
            VerificationTime = DateTime.Now,
            UrlRetrievalTimeout = new TimeSpan(0, 0, 30)
          }
        };
        x509Chain.Build(certificate);
        if (x509Chain.ChainStatus.Length == 0 || !((IEnumerable<X509ChainStatus>) x509Chain.ChainStatus).Any<X509ChainStatus>((Func<X509ChainStatus, bool>) (status => status.Status == X509ChainStatusFlags.NotTimeValid || status.Status == X509ChainStatusFlags.Revoked || status.Status == X509ChainStatusFlags.UntrustedRoot)))
          return 0;
        foreach (X509ChainStatus chainStatu in x509Chain.ChainStatus)
        {
          switch (chainStatu.Status)
          {
            case X509ChainStatusFlags.NotTimeValid:
              return -2;
            case X509ChainStatusFlags.Revoked:
              return -3;
            case X509ChainStatusFlags.UntrustedRoot:
              return -4;
            default:
              continue;
          }
        }
        return 0;
      }
      catch
      {
        return -1;
      }
      finally
      {
        Win32.CertFreeCertificateContext((IntPtr) gcHandle.Target);
      }
    }
    catch
    {
      return -1;
    }
    finally
    {
      gcHandle.Free();
    }
  }
}
