// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.PCERT_EXTENSION
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;

#nullable disable
namespace Intermech.Office.Client;

internal struct PCERT_EXTENSION
{
  public IntPtr pszObjId;
  public bool fCritical;
  public CRYPTOAPI_BLOB Value;
}
