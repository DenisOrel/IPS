// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.CRYPT_VERIFY_MESSAGE_PARA
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Структура с информацией для проверки подписи</summary>
public struct CRYPT_VERIFY_MESSAGE_PARA
{
  /// <summary>размер структуры в байтах</summary>
  public int cbSize;
  /// <summary>тип кодировки</summary>
  public int dwMsgAndCertEncodingType;
  /// <summary>не используется</summary>
  public IntPtr hCryptProv;
  /// <summary>0</summary>
  public IntPtr pfnGetSignerCertificate;
  /// <summary>0</summary>
  public IntPtr pvGetArg;
}
