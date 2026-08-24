// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.To1C.Resources.CustomDescription
// Assembly: Intermech.XmlExchange.IpsXml.Converter.To1C, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 449F0722-988D-4220-8C90-DEA703EA2A9B
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.To1C.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.To1C.Resources;

internal class CustomDescription : DescriptionAttribute
{
  public CustomDescription(string description)
  {
    object obj = (object) LocalizationHolder.rma.GetString(description);
    this.DescriptionValue = obj != null ? (string) obj : string.Empty;
  }
}
