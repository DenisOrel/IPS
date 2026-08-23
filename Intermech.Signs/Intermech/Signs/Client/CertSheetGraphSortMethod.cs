// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetGraphSortMethod
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Localization;
using System.ComponentModel;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>
/// Настройка сортировки граф:
///     ByDefault:  сначала графы с подписями по дате подписи, затем графы по Описанию
///       ByValue:  все графы сортируются по значению атрибута графы для подписей
/// ByDescription:  все графы сортируются по описанию атрибута графы для подписей
/// </summary>
[TypeConverter(typeof (EnumDescConverter))]
internal enum CertSheetGraphSortMethod
{
  [CustomDescription("CertSheetGraphSortMethod.ByDefault")] ByDefault,
  [CustomDescription("CertSheetGraphSortMethod.ByValue")] ByValue,
  [CustomDescription("CertSheetGraphSortMethod.ByDescription")] ByDescription,
}
