// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.SignStatuses
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Localization;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>Статусы подписи</summary>
[TypeConverter(typeof (EnumDescConverter))]
[CustomDescription("Attribute.Search.Interfaces_16")]
[Category("Misc")]
public enum SignStatuses
{
  /// <summary>Подпись с криптозащитой устарела</summary>
  [CustomDescription("Attribute.Search.Interfaces_17")] CryptoSignOutOfDate,
  /// <summary>Подпись с криптозащитой актуальна</summary>
  [CustomDescription("Attribute.Search.Interfaces_18")] CryptoSignActual,
  /// <summary>Подпись устарела</summary>
  [CustomDescription("Attribute.Search.Interfaces_19")] SignOutOfDate,
  /// <summary>Подпись актуальна</summary>
  [CustomDescription("Attribute.Search.Interfaces_20")] SignActual,
  /// <summary>Подпись неверна</summary>
  [CustomDescription("Attribute.Search.Interfaces_21")] SignIncorrect,
  /// <summary>
  /// Подпись требует проверки
  /// Наличие статуса означает, что подпись еще не проверена.
  /// </summary>
  [CustomDescription("SignNeedToVerify")] SignNeedToVerify,
}
