// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Resources.CustomCategory
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using System.ComponentModel;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.Resources;

internal class CustomCategory(string сategory) : CategoryAttribute(сategory)
{
  protected override string GetLocalizedString(string value)
  {
    return LocalizationHolder.rma.GetString(value) == null ? string.Empty : LocalizationHolder.rma.GetString(value);
  }
}
