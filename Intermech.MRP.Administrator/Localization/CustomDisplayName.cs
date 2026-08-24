// Decompiled with JetBrains decompiler
// Type: Intermech.Localization.CustomDisplayName
// Assembly: Intermech.MRP.Administrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6B87B3A6-A601-4A16-AA63-05D1A823449F
// Assembly location: D:\IPS\Client\Intermech.MRP.Administrator.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.Administrator.xml

using System.ComponentModel;

#nullable disable
namespace Intermech.Localization;

/// <summary>
/// Класс позволяет получать отображаемое имя (DisplayName) из ресурсов текущей сборки
/// </summary>
internal class CustomDisplayName : DisplayNameAttribute
{
  public CustomDisplayName(string displayName)
  {
    object obj = (object) LocalizationHolder.rma.GetString(displayName);
    this.DisplayNameValue = obj != null ? (string) obj : string.Empty;
  }
}
