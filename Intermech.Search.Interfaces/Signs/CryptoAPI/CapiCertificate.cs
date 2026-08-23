// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.CryptoAPI.CapiCertificate
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Intermech.Signs.CryptoAPI;

/// <summary>для работы с сертификатами через Crypto API</summary>
public class CapiCertificate : IDisposable
{
  /// <summary>указатель на сертификат</summary>
  private IntPtr pSignerCert = IntPtr.Zero;
  /// <summary>has the last call succeded ?</summary>
  private bool lastResult;
  /// <summary>алгоритм сертификата</summary>
  private string algKey = string.Empty;
  /// <summary>массив подписываемых сообщений</summary>
  private IntPtr[] messages = new IntPtr[1];

  /// <summary>Последняя ошибка, возникшая при выполнении</summary>
  public int Error => Marshal.GetLastWin32Error();

  /// <summary>Конструктор</summary>
  /// <param name="cert">Сертификат, с помощью которого подписываем</param>
  public CapiCertificate(X509Certificate2 cert)
  {
    this.pSignerCert = cert != null ? cert.Handle : throw new Exception(LocalizationHolder.rm.GetString("Search.Interfaces_12"));
    this.algKey = cert.SignatureAlgorithm.Value;
  }

  /// <summary>Конструктор</summary>
  public CapiCertificate()
  {
  }

  /// <summary>Подписать хэш объекта</summary>
  /// <param name="objectHash">подписываемый хэш </param>
  /// <returns> подпись</returns>
  public byte[] SignObjectHash(byte[] objectHash)
  {
    if (!(this.pSignerCert != IntPtr.Zero))
      throw new Exception(LocalizationHolder.rm.GetString("Search.Interfaces_12"));
    CRYPT_SIGN_MESSAGE_PARA pSignPara = new CRYPT_SIGN_MESSAGE_PARA();
    pSignPara.cbSize = Marshal.SizeOf<CRYPT_SIGN_MESSAGE_PARA>(pSignPara);
    pSignPara.dwMsgEncodingType = 65537 /*0x010001*/;
    pSignPara.pSigningCert = this.pSignerCert;
    pSignPara.HashAlgorithm.pszObjId = this.algKey;
    pSignPara.HashAlgorithm.Parameters.pbData = IntPtr.Zero;
    pSignPara.HashAlgorithm.Parameters.cbData = 0;
    pSignPara.pvHashAuxInfo = IntPtr.Zero;
    pSignPara.cMsgCert = 1;
    GCHandle gcHandle = GCHandle.Alloc((object) this.pSignerCert, GCHandleType.Pinned);
    pSignPara.rgpMsgCert = gcHandle.AddrOfPinnedObject();
    gcHandle.Free();
    pSignPara.cMsgCrl = 0;
    pSignPara.rgpMsgCrl = IntPtr.Zero;
    pSignPara.cAuthAttr = 0;
    pSignPara.rgAuthAttr = IntPtr.Zero;
    pSignPara.cUnauthAttr = 0;
    pSignPara.rgUnauthAttr = IntPtr.Zero;
    pSignPara.dwFlags = 0;
    pSignPara.dwInnerContentType = 0;
    int length = objectHash.Length;
    this.messages[0] = Marshal.AllocHGlobal(length);
    Marshal.Copy(objectHash, 0, this.messages[0], length);
    int[] rgcbToBeSigned = new int[1]{ length };
    int pcbSignedBlob = 0;
    this.lastResult = CAPICertificate.CryptSignMessage(ref pSignPara, false, 1, this.messages, rgcbToBeSigned, (byte[]) null, ref pcbSignedBlob);
    if (!this.lastResult)
      throw new Win32Exception(this.Error);
    byte[] pbSignedBlob = new byte[pcbSignedBlob];
    this.lastResult = CAPICertificate.CryptSignMessage(ref pSignPara, false, 1, this.messages, rgcbToBeSigned, pbSignedBlob, ref pcbSignedBlob);
    if (!this.lastResult)
      throw new Win32Exception(this.Error);
    return pbSignedBlob;
  }

  /// <summary>проверить подпись</summary>
  /// <param name="sign">проверяемое значение эцп объекта</param>
  /// <param name="objectHash">хэш объект (данные для сравнения)</param>
  /// <returns>true - подпись верна, false - подпись не верна</returns>
  public bool VerifyObjectSign(byte[] sign, byte[] objectHash)
  {
    CRYPT_VERIFY_MESSAGE_PARA pVerifyPara = new CRYPT_VERIFY_MESSAGE_PARA();
    pVerifyPara.cbSize = Marshal.SizeOf<CRYPT_VERIFY_MESSAGE_PARA>(pVerifyPara);
    pVerifyPara.dwMsgAndCertEncodingType = 65537 /*0x010001*/;
    pVerifyPara.hCryptProv = IntPtr.Zero;
    pVerifyPara.pfnGetSignerCertificate = IntPtr.Zero;
    pVerifyPara.pvGetArg = IntPtr.Zero;
    int pcbDecoded = 0;
    this.lastResult = CAPICertificate.CryptVerifyMessageSignature(ref pVerifyPara, 0, sign, sign.Length, (byte[]) null, ref pcbDecoded, IntPtr.Zero);
    if (!this.lastResult)
      throw new Win32Exception(this.Error);
    byte[] numArray = new byte[pcbDecoded];
    this.lastResult = CAPICertificate.CryptVerifyMessageSignature(ref pVerifyPara, 0, sign, sign.Length, numArray, ref pcbDecoded, IntPtr.Zero);
    if (!this.lastResult)
      throw new Win32Exception(this.Error);
    return Convert.ToBase64String(numArray).Equals(Convert.ToBase64String(objectHash));
  }

  /// <summary>
  /// 
  /// </summary>
  public void Dispose()
  {
    if (!(this.messages[0] != IntPtr.Zero))
      return;
    Marshal.FreeHGlobal(this.messages[0]);
  }
}
