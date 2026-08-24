// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CERT_RDN_ATTR
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

#nullable disable
namespace Intermech.Office.Client;

internal struct CERT_RDN_ATTR
{
  public string pszObjId;
  public int dwValueType;
  public CRYPTOAPI_BLOB Value;
}
