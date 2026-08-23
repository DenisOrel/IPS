// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.Win32
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Signs.Interfaces;

[ComVisible(false)]
public class Win32
{
  public const int X509_ASN_ENCODING = 1;
  public const int PKCS_7_ASN_ENCODING = 65536 /*0x010000*/;
  public const int CMSG_SIGNED = 2;
  public const int CMSG_DETACHED_FLAG = 4;
  public const int AT_KEYEXCHANGE = 1;
  public const int AT_SIGNATURE = 2;
  public const int CMSG_CTRL_VERIFY_SIGNATURE = 1;
  public const int CMSG_CERT_PARAM = 12;
  public const int CMSG_SIGNER_CERT_INFO_PARAM = 7;
  public const int CERT_STORE_PROV_MSG = 1;
  public const int CERT_CLOSE_STORE_FORCE_FLAG = 1;

  [ComVisible(false)]
  [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern bool CryptAcquireContext(
    ref IntPtr hProv,
    string pszContainer,
    string pszProvider,
    int dwProvType,
    int dwFlags);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern IntPtr CryptMsgOpenToEncode(
    int dwMsgEncodingType,
    int dwFlags,
    int dwMsgType,
    ref Intermech.Signs.Interfaces.Win32.CMSG_SIGNED_ENCODE_INFO pvMsgEncodeInfo,
    string pszInnerContentObjID,
    ref Intermech.Signs.Interfaces.Win32.CMSG_STREAM_INFO pStreamInfo);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern IntPtr CryptMsgOpenToDecode(
    int dwMsgEncodingType,
    int dwFlags,
    int dwMsgType,
    IntPtr hCryptProv,
    IntPtr pRecipientInfo,
    ref Intermech.Signs.Interfaces.Win32.CMSG_STREAM_INFO pStreamInfo);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CryptMsgClose(IntPtr hCryptMsg);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CryptMsgUpdate(
    IntPtr hCryptMsg,
    byte[] pbData,
    int cbData,
    bool fFinal);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CryptMsgUpdate(
    IntPtr hCryptMsg,
    IntPtr pbData,
    int cbData,
    bool fFinal);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CryptMsgGetParam(
    IntPtr hCryptMsg,
    int dwParamType,
    int dwIndex,
    IntPtr pvData,
    ref int pcbData);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CryptMsgControl(
    IntPtr hCryptMsg,
    int dwFlags,
    int dwCtrlType,
    IntPtr pvCtrlPara);

  [ComVisible(false)]
  [DllImport("advapi32.dll", SetLastError = true)]
  public static extern bool CryptReleaseContext(IntPtr hProv, int dwFlags);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern IntPtr CertCreateCertificateContext(
    int dwCertEncodingType,
    IntPtr pbCertEncoded,
    int cbCertEncoded);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CertFreeCertificateContext(IntPtr pCertContext);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern IntPtr CertOpenStore(
    int lpszStoreProvider,
    int dwMsgAndCertEncodingType,
    IntPtr hCryptProv,
    int dwFlags,
    IntPtr pvPara);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern IntPtr CertGetSubjectCertificateFromStore(
    IntPtr hCertStore,
    int dwCertEncodingType,
    IntPtr pCertId);

  [ComVisible(false)]
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern IntPtr CertCloseStore(IntPtr hCertStore, int dwFlags);

  [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern Intermech.Signs.Interfaces.Win32.CertHandle CertDuplicateCertificateContext(
    [In] IntPtr pCertContext);

  [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern bool CertGetCertificateContextProperty(
    [In] Intermech.Signs.Interfaces.Win32.CertHandle pCertContext,
    [In] uint dwPropId,
    [In, Out] IntPtr pvData,
    [In, Out] ref uint pcbData);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  internal static extern Intermech.Signs.Interfaces.Win32.SafeHandle LocalAlloc(
    [In] uint uFlags,
    [In] IntPtr sizetdwBytes);

  [DllImport("kernel32.dll", SetLastError = true)]
  private static extern IntPtr LocalFree(IntPtr handle);

  [ComVisible(false)]
  public struct CRYPT_ALGORITHM_IDENTIFIER
  {
    public string pszObjId;
    public Intermech.Signs.Interfaces.Win32.BLOB Parameters;
  }

  [ComVisible(false)]
  public struct CERT_ID
  {
    public int dwIdChoice;
    public Intermech.Signs.Interfaces.Win32.BLOB IssuerSerialNumberOrKeyIdOrHashId;
  }

  [ComVisible(false)]
  public struct CMSG_SIGNER_ENCODE_INFO
  {
    public int cbSize;
    public IntPtr pCertInfo;
    public IntPtr hCryptProvOrhNCryptKey;
    public int dwKeySpec;
    public Intermech.Signs.Interfaces.Win32.CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;
    public IntPtr pvHashAuxInfo;
    public int cAuthAttr;
    public IntPtr rgAuthAttr;
    public int cUnauthAttr;
    public IntPtr rgUnauthAttr;
    public Intermech.Signs.Interfaces.Win32.CERT_ID SignerId;
    public Intermech.Signs.Interfaces.Win32.CRYPT_ALGORITHM_IDENTIFIER HashEncryptionAlgorithm;
    public IntPtr pvHashEncryptionAuxInfo;
  }

  [ComVisible(false)]
  public struct CERT_CONTEXT
  {
    public int dwCertEncodingType;
    public IntPtr pbCertEncoded;
    public int cbCertEncoded;
    public IntPtr pCertInfo;
    public IntPtr hCertStore;
  }

  [ComVisible(false)]
  public struct BLOB
  {
    public int cbData;
    public IntPtr pbData;
  }

  [ComVisible(false)]
  public struct CMSG_SIGNED_ENCODE_INFO
  {
    public int cbSize;
    public int cSigners;
    public IntPtr rgSigners;
    public int cCertEncoded;
    public IntPtr rgCertEncoded;
    public int cCrlEncoded;
    public IntPtr rgCrlEncoded;
    public int cAttrCertEncoded;
    public IntPtr rgAttrCertEncoded;
  }

  [ComVisible(false)]
  public struct CMSG_STREAM_INFO
  {
    public int cbContent;
    public Intermech.Signs.Interfaces.Win32.StreamOutputCallbackDelegate pfnStreamOutput;
    public IntPtr pvArg;
  }

  [ComVisible(false)]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct CRYPT_KEY_PROV_INFO
  {
    internal string pwszContainerName;
    internal string pwszProvName;
    internal uint dwProvType;
    internal uint dwFlags;
    internal uint cProvParam;
    internal IntPtr rgProvParam;
    internal uint dwKeySpec;
  }

  [ComVisible(false)]
  public delegate bool StreamOutputCallbackDelegate(
    IntPtr pvArg,
    IntPtr pbData,
    int cbData,
    bool fFinal);

  public class SafeHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    public SafeHandle(IntPtr handle)
      : base(true)
    {
      this.SetHandle(handle);
    }

    public SafeHandle()
      : base(true)
    {
    }

    protected override bool ReleaseHandle() => Intermech.Signs.Interfaces.Win32.LocalFree(this.handle) == IntPtr.Zero;
  }

  public class CertHandle : SafeHandleZeroOrMinusOneIsInvalid
  {
    public CertHandle()
      : base(true)
    {
    }

    public CertHandle(bool ownsHandle)
      : base(ownsHandle)
    {
    }

    protected override bool ReleaseHandle() => Intermech.Signs.Interfaces.Win32.CertFreeCertificateContext(this.handle);
  }
}
