// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.ToLocman.Types.IPSToLocmanConfig
// Assembly: Intermech.XmlExchange.IpsXml.Converter.ToLocman, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 76EBC069-92E6-4D74-866F-DCC1A2BB2547
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.ToLocman.dll

using Intermech.XmlExchange.IpsXml.Interfaces.Converter.ConfigTypes;
using System.Xml.Linq;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.ToLocman.Types;

public class IPSToLocmanConfig : PluginConfig
{
  public const string AN_EXPORT_MODE = "export_mode";
  private ExportMode _exportMode;

  public ExportMode ExportMode
  {
    get => this._exportMode;
    set => this._exportMode = value;
  }

  protected override void InternalLoadConfig(XDocument doc)
  {
    base.InternalLoadConfig(doc);
    XElement xelement = doc.Root.Element((XName) "output_file_info");
    if (xelement == null)
      return;
    XAttribute xattribute = xelement.Attribute((XName) "export_mode");
    this._exportMode = xattribute != null ? xattribute.Value.ParseNodeType() : ExportMode.emArtSostav;
  }
}
