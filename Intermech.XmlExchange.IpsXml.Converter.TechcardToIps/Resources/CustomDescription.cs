// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources.CustomDescription
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E8F758AE-29ED-44FC-8EF9-3A977263FAEF
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Resources;

internal class CustomDescription : DescriptionAttribute
{
  public CustomDescription(string description)
  {
    object obj = (object) LocalizationHolder.rma.GetString(description);
    this.DescriptionValue = obj != null ? (string) obj : string.Empty;
  }
}
