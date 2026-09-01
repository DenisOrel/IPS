// Decompiled with JetBrains decompiler
// Type: IPSAddIn.Installer.ExtensionsXML
// Assembly: IPSAddIn.Installer, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: 0B42B756-5F54-4959-820D-851B2C3E0C84
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn.Installer.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace IPSAddIn.Installer;

internal class ExtensionsXML
{
  public readonly string ExtensionsFile = "ExtensionsRegistry.xml";
  private readonly string _altiumDataFolder;

  public bool ExistsInformation { get; private set; }

  public ExtensionsXML(string altiumDataFolder) => this._altiumDataFolder = altiumDataFolder;

  public AddInFolder GetFolderForExtension()
  {
    XmlNode ipsNode = this.GetIpsNode(this.RootNode, false);
    string foundFolder = string.Empty;
    if (ipsNode != null)
    {
      XmlNode xmlNode = ipsNode.SelectSingleNode("Path");
      if (xmlNode != null)
        foundFolder = xmlNode.InnerText;
      this.ExistsInformation = true;
    }
    return AddInFolder.Create(this._altiumDataFolder, foundFolder);
  }

  public void CreateBackupFile()
  {
    string extensionsFilePath = this.GetExtensionsFilePath();
    File.Copy(extensionsFilePath, extensionsFilePath + ".bak", true);
  }

  private string GetExtensionsFilePath()
  {
    string path = Path.Combine(this._altiumDataFolder, Consts.ExtensionsRootFolderName, this.ExtensionsFile);
    return File.Exists(path) ? path : throw new Exception("Отсутствует файл с описанием установленных расширений Altium Designer " + path);
  }

  private void SetNodeValue(XmlNode parentNode, string childNodeName, string childNodeValue)
  {
    XmlNode newChild = parentNode.SelectSingleNode(childNodeName);
    if (newChild == null)
    {
      newChild = (XmlNode) parentNode.OwnerDocument.CreateElement(childNodeName);
      parentNode.AppendChild(newChild);
    }
    newChild.InnerText = childNodeValue;
  }

  private XmlNode GetIpsNode(XmlNode rootNode, bool createIfNotExists)
  {
    XmlNode ipsNode = rootNode.SelectSingleNode("Item[@HRID='IPSAddIn']");
    if (ipsNode != null)
      return ipsNode;
    if (!createIfNotExists)
      return (XmlNode) null;
    XmlNode element = (XmlNode) rootNode.OwnerDocument.CreateElement("Item");
    XmlAttribute attribute1 = rootNode.OwnerDocument.CreateAttribute("HRID");
    attribute1.Value = "IPSAddIn";
    element.Attributes.Append(attribute1);
    XmlAttribute attribute2 = rootNode.OwnerDocument.CreateAttribute("Guid");
    attribute2.Value = this.ConvertGuidToString(Consts.PluginGuid);
    element.Attributes.Append(attribute2);
    rootNode.AppendChild(element);
    return element;
  }

  public void SetPluginInfo(PluginInfo info)
  {
    XmlDocument xmlDocument = new XmlDocument();
    string extensionsFilePath = this.GetExtensionsFilePath();
    xmlDocument.Load(extensionsFilePath);
    XmlElement documentElement = xmlDocument.DocumentElement;
    XmlNode ipsNode = this.GetIpsNode((XmlNode) documentElement, true);
    this.SetNodeValue(ipsNode, "Title", info.Title);
    this.SetNodeValue(ipsNode, "Path", info.FolderPath);
    this.SetNodeValue(ipsNode, "LongDescription", info.Description);
    this.SetNodeValue(ipsNode, "Version", info.Version);
    this.SetNodeValue(ipsNode, "VersionGuid", this.ConvertGuidToString(info.VersionGuid));
    this.SetNodeValue(ipsNode, "ReleasedDate", info.Date.ToOADate().ToString((IFormatProvider) CultureInfo.InvariantCulture));
    this.SetNodeValue(ipsNode, "DateInstalled", DateTime.Now.ToOADate().ToString((IFormatProvider) CultureInfo.InvariantCulture));
    if (!this.ExistsInformation)
    {
      this.SetNodeValue(ipsNode, "Status", "0");
      this.SetNodeValue(ipsNode, "VaultGuid", string.Empty);
      this.SetNodeValue(ipsNode, "CreatedBy", info.CreatedBy);
      this.SetNodeValue(ipsNode, "CategoryGuid", this.ConvertGuidToString(info.CategoryGuid));
      this.SetNodeValue(ipsNode, "CategoryName", string.Empty);
      this.SetNodeValue(ipsNode, "ReadMe", string.Empty);
      this.SetNodeValue(ipsNode, "Help", string.Empty);
      this.SetNodeValue(ipsNode, "Requirements", string.Empty);
      this.SetNodeValue(ipsNode, "ShortDescription", string.Empty);
      this.SetNodeValue(ipsNode, "Help", string.Empty);
      this.SetNodeValue(ipsNode, "SmallImage", string.Empty);
      this.SetNodeValue(ipsNode, "LargeImage", string.Empty);
      this.SetNodeValue(ipsNode, "ReleaseNotes", string.Empty);
      List<PlatformVersions> platformVersionsList = ExtensionsXML.ReadAnyPlatformVersions(documentElement);
      if (platformVersionsList.Count > 0)
      {
        XmlElement element1 = ipsNode.OwnerDocument.CreateElement("PlatformVersions");
        foreach (PlatformVersions platformVersions in platformVersionsList)
        {
          XmlElement element2 = element1.OwnerDocument.CreateElement(platformVersions.PlatformName);
          XmlAttribute attribute = element2.OwnerDocument.CreateAttribute("BuildNumber");
          attribute.Value = platformVersions.PlatformVersion;
          element2.Attributes.Append(attribute);
          element1.AppendChild((XmlNode) element2);
        }
        ipsNode.AppendChild((XmlNode) element1);
      }
    }
    XmlWriterSettings settings = new XmlWriterSettings()
    {
      Indent = true,
      Encoding = (Encoding) new UTF8Encoding(false)
    };
    xmlDocument.Save(XmlWriter.Create(extensionsFilePath, settings));
  }

  private static List<PlatformVersions> ReadAnyPlatformVersions(XmlElement rootNode)
  {
    List<PlatformVersions> platformVersionsList = new List<PlatformVersions>();
    XmlNode source = rootNode.SelectSingleNode("//PlatformVersions");
    if (source != null)
      platformVersionsList.AddRange(source.Cast<XmlNode>().Select<XmlNode, PlatformVersions>((Func<XmlNode, PlatformVersions>) (childNode => new PlatformVersions(childNode.Name, childNode.Attributes?["BuildNumber"]?.Value ?? string.Empty))));
    else
      platformVersionsList.AddRange((IEnumerable<PlatformVersions>) new PlatformVersions[4]
      {
        new PlatformVersions("DXP", "0.0.0.0"),
        new PlatformVersions("EDP", "0.0.0.0"),
        new PlatformVersions("MaxDXP", "0.0.0.0"),
        new PlatformVersions("MaxEDP", "0.0.0.0")
      });
    return platformVersionsList;
  }

  private string ConvertGuidToString(Guid guid) => guid.ToString("D").ToUpper();

  private XmlNode RootNode
  {
    get
    {
      XmlDocument xmlDocument = new XmlDocument();
      string extensionsFilePath = this.GetExtensionsFilePath();
      xmlDocument.Load(extensionsFilePath);
      return (XmlNode) (xmlDocument.DocumentElement ?? throw new Exception($"Неверный формат файла {extensionsFilePath}."));
    }
  }
}
