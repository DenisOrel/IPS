// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.CustomCategory
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System.ComponentModel;

#nullable disable
namespace Intermech.Localization;

/// <summary>
/// Класс позволяет категорию (Category) из ресурсов текущей сборки
/// </summary>
internal class CustomCategory(string сategory) : CategoryAttribute(сategory)
{
  /// <summary>
  /// Загрузить атрибут с указанным именем из ресурсов [CustomAttributesResources] текущей сборки
  /// </summary>
  /// <param name="value">Имя атрибута в ресурсах [CustomAttributesResources] текущей сборки</param>
  protected override string GetLocalizedString(string value)
  {
    return LocalizationHolder.rma.GetString(value) ?? string.Empty;
  }
}
