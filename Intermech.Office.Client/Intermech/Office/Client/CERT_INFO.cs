// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CERT_INFO
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Signs.Interfaces;

#nullable disable
namespace Intermech.Office.Client;

internal struct CERT_INFO
{
  public int dwVersion;
  public CRYPTOAPI_BLOB SerialNumber;
  public CRYPT_ALGORITHM_IDENTIFIER SignatureAlgorithm;
  public CRYPTOAPI_BLOB Issuer;
  public FILETIME NotBefore;
  public FILETIME NotAfter;
  public CRYPTOAPI_BLOB Subject;
  public CERT_PUBLIC_KEY_INFO SubjectPublicKeyInfo;
  public CRYPTOAPI_BLOB IssuerUniqueId;
  public CRYPTOAPI_BLOB SubjectUniqueId;
  public int cExtension;
  public PCERT_EXTENSION rgExtension;
}
