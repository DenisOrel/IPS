// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CRYPT_SIGN_MESSAGE_PARA
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Signs.Interfaces;
using System;

#nullable disable
namespace Intermech.Office.Client;

public struct CRYPT_SIGN_MESSAGE_PARA
{
  public int cbSize;
  public int dwMsgEncodingType;
  public IntPtr pSigningCert;
  public CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;
  public IntPtr pvHashAuxInfo;
  public int cMsgCert;
  public IntPtr rgpMsgCert;
  public int cMsgCrl;
  public IntPtr rgpMsgCrl;
  public int cAuthAttr;
  public IntPtr rgAuthAttr;
  public int cUnauthAttr;
  public IntPtr rgUnauthAttr;
  public int dwFlags;
  public int dwInnerContentType;
}
