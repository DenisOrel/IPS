// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.StreamCms
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>
/// Класс для потокового подписывания, в том числе очень больших объемов
/// </summary>
[ComVisible(false)]
public class StreamCms
{
  private Stream _callbackFile;

  /// <summary>Encode StreamCms with streaming to support large data</summary>
  /// <param name="cert"></param>
  /// <param name="inFile"></param>
  /// <param name="outFile">подпись, если isDetached=true;   подпись+подписываемые данные, если isDetached=false; закрывается автоматически</param>
  /// <param name="isDetached"></param>
  public void Encode(X509Certificate2 cert, Stream inFile, Stream outFile, bool isDetached)
  {
    IntPtr zero = IntPtr.Zero;
    IntPtr num1 = IntPtr.Zero;
    IntPtr hglobal = IntPtr.Zero;
    IntPtr num2 = IntPtr.Zero;
    try
    {
      this._callbackFile = outFile;
      X509Chain x509Chain = new X509Chain();
      x509Chain.Build(cert);
      X509ChainElement[] array = new X509ChainElement[x509Chain.ChainElements.Count];
      x509Chain.ChainElements.CopyTo(array, 0);
      X509Certificate2[] x509Certificate2Array = new X509Certificate2[array.Length];
      for (int index = 0; index < array.Length; ++index)
        x509Certificate2Array[index] = array[index].Certificate;
      Win32.CERT_CONTEXT[] certContextArray = new Win32.CERT_CONTEXT[x509Certificate2Array.Length];
      for (int index = 0; index < x509Certificate2Array.Length; ++index)
        certContextArray[index] = (Win32.CERT_CONTEXT) Marshal.PtrToStructure(x509Certificate2Array[index].Handle, typeof (Win32.CERT_CONTEXT));
      Win32.BLOB[] blobArray = new Win32.BLOB[certContextArray.Length];
      for (int index = 0; index < certContextArray.Length; ++index)
      {
        blobArray[index].cbData = certContextArray[index].cbCertEncoded;
        blobArray[index].pbData = certContextArray[index].pbCertEncoded;
      }
      Win32.CRYPT_KEY_PROV_INFO parameters;
      StreamCms.GetPrivateKeyInfo(StreamCms.GetCertContext(cert), out parameters);
      if (!Win32.CryptAcquireContext(ref zero, parameters.pwszContainerName, parameters.pwszProvName, (int) parameters.dwProvType, 0))
        throw new Exception("CryptAcquireContext error #" + Marshal.GetLastWin32Error().ToString(), (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      Win32.CMSG_SIGNER_ENCODE_INFO structure = new Win32.CMSG_SIGNER_ENCODE_INFO();
      structure.cbSize = Marshal.SizeOf<Win32.CMSG_SIGNER_ENCODE_INFO>(structure);
      structure.pCertInfo = certContextArray[0].pCertInfo;
      structure.hCryptProvOrhNCryptKey = zero;
      structure.dwKeySpec = (int) parameters.dwKeySpec;
      structure.HashAlgorithm.pszObjId = cert.SignatureAlgorithm.Value;
      Win32.CMSG_SIGNED_ENCODE_INFO pvMsgEncodeInfo = new Win32.CMSG_SIGNED_ENCODE_INFO();
      pvMsgEncodeInfo.cbSize = Marshal.SizeOf<Win32.CMSG_SIGNED_ENCODE_INFO>(pvMsgEncodeInfo);
      pvMsgEncodeInfo.cSigners = 1;
      num1 = Marshal.AllocHGlobal(Marshal.SizeOf<Win32.CMSG_SIGNER_ENCODE_INFO>(structure));
      Marshal.StructureToPtr<Win32.CMSG_SIGNER_ENCODE_INFO>(structure, num1, false);
      pvMsgEncodeInfo.rgSigners = num1;
      pvMsgEncodeInfo.cCertEncoded = blobArray.Length;
      hglobal = Marshal.AllocHGlobal(Marshal.SizeOf<Win32.BLOB>(blobArray[0]) * blobArray.Length);
      for (int index = 0; index < blobArray.Length; ++index)
        Marshal.StructureToPtr<Win32.BLOB>(blobArray[index], new IntPtr(hglobal.ToInt64() + (long) (Marshal.SizeOf<Win32.BLOB>(blobArray[index]) * index)), false);
      pvMsgEncodeInfo.rgCertEncoded = hglobal;
      Win32.CMSG_STREAM_INFO pStreamInfo = new Win32.CMSG_STREAM_INFO()
      {
        cbContent = (int) inFile.Length,
        pfnStreamOutput = new Win32.StreamOutputCallbackDelegate(this.StreamOutputCallback)
      };
      num2 = Win32.CryptMsgOpenToEncode(65537 /*0x010001*/, isDetached ? 4 : 0, 2, ref pvMsgEncodeInfo, (string) null, ref pStreamInfo);
      if (num2.Equals((object) IntPtr.Zero))
        throw new Exception("CryptMsgOpenToEncode error #" + Marshal.GetLastWin32Error().ToString(), (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      this.ProcessMessage(num2, inFile);
    }
    finally
    {
      if (!hglobal.Equals((object) IntPtr.Zero))
        Marshal.FreeHGlobal(hglobal);
      if (!num1.Equals((object) IntPtr.Zero))
        Marshal.FreeHGlobal(num1);
      if (!zero.Equals((object) IntPtr.Zero))
        Win32.CryptReleaseContext(zero, 0);
      if (!num2.Equals((object) IntPtr.Zero))
        Win32.CryptMsgClose(num2);
    }
  }

  /// <summary>
  /// Decode StreamCms with streaming to support large data
  /// Проверка подписи на большом массиве данных.
  /// После успешной проверки подписи можно проверить и валидность сертификата - вызвать CertProcs.GetX509VerifyResults
  /// </summary>
  /// <param name="dataFile">поток-файл данных для проверки подписи; null при isDetached = false</param>
  /// <param name="signFile">поток-файл подписи</param>
  /// <param name="outFile">выходной поток: (данный параметр очевидно нужно убрать или подавать Stream.Null)
  /// при isDetached = true: пуст
  /// при isDetached = false: если dataFile верный, то на выходе поток-файл клон dataFile, а если неверный, то exception; </param>
  /// <param name="isDetached"></param>
  public void Decode(
    Stream dataFile,
    Stream signFile,
    Stream outFile,
    bool isDetached,
    out X509Certificate2 cert)
  {
    cert = (X509Certificate2) null;
    IntPtr num1 = IntPtr.Zero;
    IntPtr num2 = IntPtr.Zero;
    IntPtr num3 = IntPtr.Zero;
    IntPtr hCertStore = IntPtr.Zero;
    try
    {
      this._callbackFile = outFile;
      Win32.CMSG_STREAM_INFO pStreamInfo = new Win32.CMSG_STREAM_INFO()
      {
        cbContent = (int) signFile.Length,
        pfnStreamOutput = new Win32.StreamOutputCallbackDelegate(this.StreamOutputCallback)
      };
      num1 = Win32.CryptMsgOpenToDecode(65537 /*0x010001*/, isDetached ? 4 : 0, 0, IntPtr.Zero, IntPtr.Zero, ref pStreamInfo);
      if (num1.Equals((object) IntPtr.Zero))
        throw new Exception("CryptMsgOpenToDecode error #" + Marshal.GetLastWin32Error().ToString(), (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      if (isDetached)
      {
        this.ProcessMessage(num1, signFile);
        this.ProcessMessage(num1, dataFile);
      }
      else
        this.ProcessMessage(num1, signFile);
      int pcbData = 0;
      if (!Win32.CryptMsgGetParam(num1, 7, 0, IntPtr.Zero, ref pcbData))
        throw new Exception("CryptMsgGetParam error #" + Marshal.GetLastWin32Error().ToString(), (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      num2 = Marshal.AllocHGlobal(pcbData);
      if (!Win32.CryptMsgGetParam(num1, 7, 0, num2, ref pcbData))
        throw new Exception("CryptMsgGetParam error #" + Marshal.GetLastWin32Error().ToString(), (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      hCertStore = Win32.CertOpenStore(1, 65537 /*0x010001*/, IntPtr.Zero, 0, num1);
      num3 = !hCertStore.Equals((object) IntPtr.Zero) ? Win32.CertGetSubjectCertificateFromStore(hCertStore, 65537 /*0x010001*/, num2) : throw new Exception("CertOpenStore error #" + Marshal.GetLastWin32Error().ToString(), (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      Win32.CERT_CONTEXT certContext = !num3.Equals((object) IntPtr.Zero) ? (Win32.CERT_CONTEXT) Marshal.PtrToStructure(num3, typeof (Win32.CERT_CONTEXT)) : throw new Exception("CertGetSubjectCertificateFromStore error #" + Marshal.GetLastWin32Error().ToString(), (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      if (!Win32.CryptMsgControl(num1, 0, 1, certContext.pCertInfo))
        throw new Exception("CryptMsgControl error #" + Marshal.GetLastWin32Error().ToString(), (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      if (num3.Equals((object) IntPtr.Zero))
        return;
      cert = new X509Certificate2(num3);
    }
    finally
    {
      if (!num3.Equals((object) IntPtr.Zero))
        Win32.CertFreeCertificateContext(num3);
      if (!num2.Equals((object) IntPtr.Zero))
        Marshal.FreeHGlobal(num2);
      if (!hCertStore.Equals((object) IntPtr.Zero))
        Win32.CertCloseStore(hCertStore, 0);
      if (!num1.Equals((object) IntPtr.Zero))
        Win32.CryptMsgClose(num1);
    }
  }

  private bool StreamOutputCallback(IntPtr pvArg, IntPtr pbData, int cbData, bool fFinal)
  {
    if (cbData == 0)
      return true;
    byte[] numArray = new byte[cbData];
    Marshal.Copy(pbData, numArray, 0, cbData);
    this._callbackFile.Write(numArray, 0, cbData);
    if (fFinal)
    {
      this._callbackFile.Flush();
      this._callbackFile.Close();
      this._callbackFile = (Stream) null;
    }
    return true;
  }

  /// <summary>
  /// Если Decode() == true, то дополнительно проверить правильность сертификата
  /// </summary>
  /// <param name="cert"></param>
  /// <returns></returns>
  private void ProcessMessage(IntPtr hMsg, Stream dataStream)
  {
    long length1 = dataStream.Length;
    if (length1 == 0L)
      throw new CryptographicException("Cannot encode zero length data");
    GCHandle gcHandle = new GCHandle();
    int length2 = length1 < 1048576L /*0x100000*/ ? (int) length1 : 1048576 /*0x100000*/;
    byte[] buffer = new byte[length2];
    try
    {
      long num = length1;
      gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
      IntPtr pbData = gcHandle.AddrOfPinnedObject();
      while (num > 0L)
      {
        dataStream.Read(buffer, 0, length2);
        if (!Win32.CryptMsgUpdate(hMsg, pbData, length2, num <= (long) length2))
          throw new Exception("CryptMsgUpdate error #" + Marshal.GetLastWin32Error().ToString(), (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
        num -= (long) length2;
        if (num < (long) length2)
          length2 = (int) num;
      }
    }
    finally
    {
      if (gcHandle.IsAllocated)
        gcHandle.Free();
    }
  }

  internal static Win32.CertHandle GetCertContext(X509Certificate2 certificate)
  {
    Win32.CertHandle certContext = Win32.CertDuplicateCertificateContext(certificate.Handle);
    GC.KeepAlive((object) certificate);
    return certContext;
  }

  internal static bool GetPrivateKeyInfo(
    Win32.CertHandle safeCertContext,
    out Win32.CRYPT_KEY_PROV_INFO parameters)
  {
    parameters = new Win32.CRYPT_KEY_PROV_INFO();
    Win32.SafeHandle safeHandle1 = new Win32.SafeHandle(IntPtr.Zero);
    uint pcbData = 0;
    if (!Win32.CertGetCertificateContextProperty(safeCertContext, 2U, safeHandle1.DangerousGetHandle(), ref pcbData))
    {
      if (Marshal.GetLastWin32Error() != -2146885628)
        throw new CryptographicException(Marshal.GetLastWin32Error());
      return false;
    }
    Win32.SafeHandle safeHandle2 = Win32.LocalAlloc(0U, new IntPtr((long) pcbData));
    if (!Win32.CertGetCertificateContextProperty(safeCertContext, 2U, safeHandle2.DangerousGetHandle(), ref pcbData))
    {
      if (Marshal.GetLastWin32Error() != -2146885628)
        throw new CryptographicException(Marshal.GetLastWin32Error());
      return false;
    }
    parameters = (Win32.CRYPT_KEY_PROV_INFO) Marshal.PtrToStructure(safeHandle2.DangerousGetHandle(), typeof (Win32.CRYPT_KEY_PROV_INFO));
    safeHandle2.Dispose();
    return true;
  }
}
