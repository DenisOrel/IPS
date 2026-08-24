// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.CustomDisplayName
// Assembly: Intermech.Interfaces.ECO, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B25D666E-9146-4B6E-9222-8722321C22A6
// Assembly location: D:\IPS\Client\Intermech.Interfaces.ECO.dll
// XML documentation location: D:\IPS\Client\Intermech.Interfaces.ECO.xml

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
