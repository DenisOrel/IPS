// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.CustomDisplayName
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using System.ComponentModel;

#nullable disable
namespace Intermech.Localization;

/// <summary>
/// Класс позволяет получать отображаемое имя (DisplayName) из ресурсов текущей сборки
/// </summary>
internal class CustomDisplayName : DisplayNameAttribute
{
  /// <summary>
  /// Загрузить атрибут с указанным именем из ресурсов [CustomAttributesResources] текущей сборки
  /// </summary>
  /// <param name="displayName">Имя атрибута в ресурсах [CustomAttributesResources] текущей сборки</param>
  public CustomDisplayName(string displayName)
  {
    object obj = (object) LocalizationHolder.rma.GetString(displayName);
    this.DisplayNameValue = obj != null ? (string) obj : string.Empty;
  }
}
