// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SelectOpenKeyValueType
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Что выбрано в диалоге</summary>
public enum SelectOpenKeyValueType
{
  /// <summary>Ничего не выбрано</summary>
  None,
  /// <summary>Открытый ключ</summary>
  OpenKey,
  /// <summary>Сертификат</summary>
  Certificate,
}
