// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.SignsProcs
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Intermech.Signs.Interfaces;

public class SignsProcs
{
  /// <summary>
  /// Подписать сообщение, используя сертификат : старая функция для криптоподписей версии ниже HashVersionsCrypto.Version4
  /// Более не используется.
  /// </summary>
  /// <param name="msg"></param>
  /// <param name="signerCert">сертификат</param>
  /// <param name="pkcs7">пакет PKCS#7</param>
  /// <param name="verifyCert">проверить сертификат через сервер отзыва</param>
  /// <param name="verifyResult">результаты проверки при SignResult.NotVerified</param>
  /// <returns></returns>
  [Obsolete]
  public static SignResult SignMsg(
    byte[] msg,
    X509Certificate2 signerCert,
    out byte[] pkcs7,
    bool verifyCert,
    out X509ChainStatus[] verifyChainResult)
  {
    pkcs7 = (byte[]) null;
    verifyChainResult = (X509ChainStatus[]) null;
    if (verifyCert)
    {
      X509ChainStatus[] chStatus = (X509ChainStatus[]) null;
      if (!CertProcs.GetX509VerifyResults(signerCert, out chStatus))
        return SignResult.NotVerified;
    }
    SignedCms signedCms = new SignedCms(new ContentInfo(msg), false);
    CmsSigner signer = new CmsSigner(signerCert);
    signedCms.ComputeSignature(signer, false);
    pkcs7 = signedCms.Encode();
    return SignResult.OK;
  }

  /// <summary>
  /// Проверить подпись.
  /// Для старых криптоподписей версии ниже HashVersionsCrypto.Version4
  /// </summary>
  /// <param name="encodedSignedCms"></param>
  /// <param name="verifyCert">дополнительно проверять сертификат через сервер отзыва сертификатов</param>
  /// <returns></returns>
  [Obsolete]
  public static SignResult VerifyMsg(
    byte[] encodedSignedCms,
    byte[] msg,
    bool verifyCert,
    out X509Certificate2Collection certificates,
    out string errorMessage)
  {
    errorMessage = string.Empty;
    certificates = (X509Certificate2Collection) null;
    SignedCms signedCms = new SignedCms();
    signedCms.Decode(encodedSignedCms);
    try
    {
      if (Convert.ToBase64String(signedCms.ContentInfo.Content).Equals(Convert.ToBase64String(msg)))
      {
        if (verifyCert)
        {
          certificates = signedCms.Certificates;
          foreach (X509Certificate2 cert in certificates)
          {
            X509ChainStatus[] chStatus = (X509ChainStatus[]) null;
            if (!CertProcs.GetX509VerifyResults(cert, out chStatus))
            {
              string empty = string.Empty;
              if (chStatus != null)
              {
                for (int index = 0; index < chStatus.Length; ++index)
                {
                  string str = $"{chStatus[index].StatusInformation} ({cert.Subject}; {cert.Issuer})\n";
                  empty += str;
                }
              }
              throw new CryptographicException(empty);
            }
          }
        }
        signedCms.CheckSignature(true);
        return SignResult.OK;
      }
    }
    catch (CryptographicException ex)
    {
      errorMessage = ex.Message;
    }
    return SignResult.NotVerified;
  }
}
