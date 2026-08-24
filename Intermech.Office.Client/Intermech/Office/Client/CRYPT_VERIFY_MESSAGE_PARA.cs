// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CRYPT_VERIFY_MESSAGE_PARA
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;

#nullable disable
namespace Intermech.Office.Client;

public struct CRYPT_VERIFY_MESSAGE_PARA
{
  public int cbSize;
  public int dwMsgAndCertEncodingType;
  public IntPtr hCryptProv;
  public IntPtr pfnGetSignerCertificate;
  public IntPtr pvGetArg;
}
