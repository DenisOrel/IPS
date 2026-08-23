// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.CAPICertificate
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>методы для работы с сертификатами</summary>
public class CAPICertificate
{
  /// <summary>Открыть хранилище сертификатов</summary>
  /// <param name="lpszStoreProvider">указатель на сертификат</param>
  /// <param name="dwMsgAndCertEncodingType">тип сертификата</param>
  /// <param name="hCryptProv"></param>
  /// <param name="dwFlags"></param>
  /// <param name="pvPara"></param>
  /// <returns></returns>
  [DllImport("Crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern IntPtr CertOpenStore(
    int lpszStoreProvider,
    int dwMsgAndCertEncodingType,
    IntPtr hCryptProv,
    int dwFlags,
    string pvPara);

  /// <summary>Поставить подпись</summary>
  /// <param name="pSignPara">структура с описанием параметров подписи</param>
  /// <param name="fDetachedSignature">true - отдельная от данных подпись, false - совмещённая с данными</param>
  /// <param name="cToBeSigned">кол-во подписываемых сообщений</param>
  /// <param name="rgpbToBeSigned"> массив с подписываемыми сообщенями</param>
  /// <param name="rgcbToBeSigned"> массив с размерами подписываемых сообщений</param>
  /// <param name="pbSignedBlob"> указатель на буфер для полученной подписи</param>
  /// <param name="pcbSignedBlob">размер буфера, в который будет помещена подпись</param>
  /// <returns></returns>
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CryptSignMessage(
    ref CRYPT_SIGN_MESSAGE_PARA pSignPara,
    bool fDetachedSignature,
    int cToBeSigned,
    IntPtr[] rgpbToBeSigned,
    int[] rgcbToBeSigned,
    byte[] pbSignedBlob,
    ref int pcbSignedBlob);

  /// <summary>Проверить поставленную подпись</summary>
  /// <param name="pVerifyPara">Структура с параметрами для проверки</param>
  /// <param name="dwSignerIndex">Индекс для проверяемой подписи</param>
  /// <param name="pbSignedBlob">Проверяемые подписи</param>
  /// <param name="cbSignedBlob">Размеры проверяемых подписей</param>
  /// <param name="pbDecoded">Раскодированное проверяемое сообщение</param>
  /// <param name="pcbDecoded">Размер проверяемого сообщения</param>
  /// <param name="ppSignerCert">Указатель на сертификат</param>
  /// <returns></returns>
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CryptVerifyMessageSignature(
    ref CRYPT_VERIFY_MESSAGE_PARA pVerifyPara,
    int dwSignerIndex,
    byte[] pbSignedBlob,
    int cbSignedBlob,
    byte[] pbDecoded,
    ref int pcbDecoded,
    IntPtr ppSignerCert);

  /// <summary>Освободить контекст сертификата</summary>
  /// <param name="pCertContext">Дескриптор сертификата</param>
  /// <returns></returns>
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CertFreeCertificateContext(IntPtr pCertContext);

  /// <summary>Закрыть хранилище сертификатов</summary>
  /// <param name="hCertStore">Дескриптор хранилища</param>
  /// <param name="dwFlags">Флаги</param>
  /// <returns></returns>
  [DllImport("Crypt32.dll", SetLastError = true)]
  public static extern bool CertCloseStore(IntPtr hCertStore, int dwFlags);
}
