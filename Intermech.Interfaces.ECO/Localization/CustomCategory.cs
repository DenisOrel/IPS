// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.CustomCategory
// Assembly: Intermech.Interfaces.ECO, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B25D666E-9146-4B6E-9222-8722321C22A6
// Assembly location: D:\IPS\Client\Intermech.Interfaces.ECO.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.ECO.xml

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
    return LocalizationHolder.rma.GetString(value) == null ? string.Empty : LocalizationHolder.rma.GetString(value);
  }
}
