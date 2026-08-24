// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.To1C.Resources.CustomDisplayName
// Assembly: Intermech.XmlExchange.IpsXml.Converter.To1C, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 449F0722-988D-4220-8C90-DEA703EA2A9B
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.To1C.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.To1C.Resources;

internal class CustomDisplayName : DisplayNameAttribute
{
  public CustomDisplayName(string displayName)
  {
    object obj = (object) LocalizationHolder.rma.GetString(displayName);
    this.DisplayNameValue = obj != null ? (string) obj : string.Empty;
  }
}
