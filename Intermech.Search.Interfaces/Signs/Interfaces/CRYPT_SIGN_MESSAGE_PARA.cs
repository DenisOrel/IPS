// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.CRYPT_SIGN_MESSAGE_PARA
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>
/// 
/// </summary>
public struct CRYPT_SIGN_MESSAGE_PARA
{
  /// <summary>размер структуры</summary>
  public int cbSize;
  /// <summary>тип кодировки</summary>
  public int dwMsgEncodingType;
  /// <summary>указатель на сертификат</summary>
  public IntPtr pSigningCert;
  /// <summary>алгоритм для подписания</summary>
  public CRYPT_ALGORITHM_IDENTIFIER HashAlgorithm;
  /// <summary>null</summary>
  public IntPtr pvHashAuxInfo;
  /// <summary>кол-во сертификатов включённых в подпись</summary>
  public int cMsgCert;
  /// <summary>сертификаты, включённые в подпись</summary>
  public IntPtr rgpMsgCert;
  /// <summary>
  /// 
  /// </summary>
  public int cMsgCrl;
  /// <summary>
  /// 
  /// </summary>
  public IntPtr rgpMsgCrl;
  /// <summary>
  /// 
  /// </summary>
  public int cAuthAttr;
  /// <summary>
  /// 
  /// </summary>
  public IntPtr rgAuthAttr;
  /// <summary>
  /// 
  /// </summary>
  public int cUnauthAttr;
  /// <summary>
  /// 
  /// </summary>
  public IntPtr rgUnauthAttr;
  /// <summary>флаги обычно 0</summary>
  public int dwFlags;
  /// <summary>0</summary>
  public int dwInnerContentType;
}
