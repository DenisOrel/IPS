// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.ConfigurationImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.PropertyEditors.ChangeHighlighting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal sealed class ConfigurationImpl : 
  IConfigurationService,
  IConfiguration,
  IEnumerable<IConfiguration>,
  IEnumerable
{
  private const string _xmlNodePlugins = "Plugins";
  private const string _xmlNodePlugin = "Plugin";
  private const string _xmlNodeSettings = "Settings";
  private const string _xmlNodeTempFolders = "TempFolders";
  private const string _xmlNodeTempFolder = "TempFolder";
  private const string _xmlNodeDBConfigs = "DBConfigs";
  private const string _xmlNodeDBConfig = "DBConfig";
  private const string _xmlNodeNameDropIndexes = "DropIndexes";
  private const string _xmlNodeNameDataMigrate = "DataMigrate";
  private const string _xmlNodeNameUnknownMeasure = "UnknownMeasure";
  private const string _xmlNodeNameSettings = "Settings";
  private const string _xmlNodeNameCache = "Cache";
  private const string _xmlNodeNamePacketSize = "PacketSize";
  private const string _xmlNodeNameCommandTimeout = "CommandTimeout";
  private const string _xmlNodeNamePLPumpingResume = "PLPumpingResume";
  private const string _xmlAttributeLocation = "location";
  private const string _xmlAttributeName = "name";
  private const string _xmlAttributeValue = "value";
  private const string _xmlAttributeDescription = "description";
  private const string _xmlAttributeEnable = "enable";
  private MainConfiguration _impExpConfiguration;

  public ConfigurationImpl()
  {
  }

  public ConfigurationImpl(XmlNode node)
    : this()
  {
    this.Node = node;
  }

  public IConfiguration Load(string filename)
  {
    XmlDocument xmlDocument = new XmlDocument();
    if (File.Exists(filename))
    {
      FileStream inStream = new FileStream(filename, FileMode.Open);
      xmlDocument.Load((Stream) inStream);
      this.Node = (XmlNode) xmlDocument.DocumentElement;
      inStream.Close();
    }
    else
    {
      this.Node = (XmlNode) xmlDocument.CreateElement("Settings");
      xmlDocument.AppendChild(this.Node);
    }
    return (IConfiguration) this;
  }

  public MainConfiguration Configuration
  {
    get
    {
      if (this._impExpConfiguration == null)
      {
        IConfiguration configuration1 = this.Open("Plugins");
        this._impExpConfiguration = new MainConfiguration();
        List<PluginItem> collection = new List<PluginItem>();
        foreach (IConfiguration configuration2 in (IEnumerable<IConfiguration>) configuration1)
        {
          if (configuration2.Node.Name.Equals("Plugin") && configuration2.HasAttribute("location"))
            collection.Add(new PluginItem(configuration2.GetAttribute("location"), configuration2.GetAttribute("description"), Convert.ToBoolean(configuration2.GetAttribute("enable"))));
        }
        this._impExpConfiguration.Plugins = new ChangeTrackingListAdapter<PluginItem>((IEnumerable<PluginItem>) collection);
        foreach (IConfiguration configuration3 in (IEnumerable<IConfiguration>) this.Open("TempFolders"))
        {
          if (configuration3.Node.Name.Equals("TempFolder") && configuration3.HasAttribute("name") && configuration3.HasAttribute("location"))
          {
            if (configuration3.GetAttribute("name") == "Settings")
              this._impExpConfiguration.SettingsTempFolder = configuration3.GetAttribute("location");
            if (configuration3.GetAttribute("name") == "Cache")
              this._impExpConfiguration.CacheTempFolder = configuration3.GetAttribute("location");
          }
        }
        IConfiguration configuration4 = this.Open("DBConfigs");
        if (configuration4 != null)
        {
          foreach (IConfiguration configuration5 in (IEnumerable<IConfiguration>) configuration4)
          {
            if (configuration5.Node.Name.Equals("DBConfig") && configuration5.HasAttribute("name") && configuration5.HasAttribute("value"))
            {
              if (configuration5.GetAttribute("name") == "DropIndexes")
                this._impExpConfiguration.DropIndexes = Convert.ToBoolean(configuration5.GetAttribute("value"));
              if (configuration5.GetAttribute("name") == "DataMigrate")
                this._impExpConfiguration.DataMigrate = Convert.ToBoolean(configuration5.GetAttribute("value"));
              if (configuration5.GetAttribute("name") == "UnknownMeasure")
                this._impExpConfiguration.UnknownMeasure = configuration5.GetAttribute("value");
              if (configuration5.GetAttribute("name") == "PacketSize")
                this._impExpConfiguration.PacketSize = Convert.ToInt32(configuration5.GetAttribute("value"));
              if (configuration5.GetAttribute("name") == "CommandTimeout")
                this._impExpConfiguration.CommandTimeout = Convert.ToInt32(configuration5.GetAttribute("value"));
              if (configuration5.GetAttribute("name") == "PLPumpingResume")
                this._impExpConfiguration.PLPumpingResume = Convert.ToBoolean(configuration5.GetAttribute("value"));
            }
          }
        }
      }
      return this._impExpConfiguration;
    }
    set
    {
      this.CheckMainConfiguration(value);
      IConfiguration configuration1 = this.Open("Plugins", true);
      configuration1.Node.RemoveAll();
      XmlDocument ownerDocument = configuration1.Node.OwnerDocument;
      foreach (PluginItem plugin in value.Plugins)
        this.AppentNode(ownerDocument, configuration1.Node, "Plugin", new Tuple<string, string>("location", plugin.FileName), new Tuple<string, string>("description", plugin.Description), new Tuple<string, string>("enable", plugin.Enable.ToString()));
      IConfiguration configuration2 = this.Open("TempFolders", true);
      configuration2.Node.RemoveAll();
      this.AppentNode(ownerDocument, configuration2.Node, "TempFolder", new Tuple<string, string>("name", "Settings"), new Tuple<string, string>("location", value.SettingsTempFolder));
      this.AppentNode(ownerDocument, configuration2.Node, "TempFolder", new Tuple<string, string>("name", "Cache"), new Tuple<string, string>("location", value.CacheTempFolder));
      IConfiguration configuration3 = this.Open("DBConfigs", true);
      configuration3.Node.RemoveAll();
      this.AppentNode(ownerDocument, configuration3.Node, "DBConfig", new Tuple<string, string>("name", "DropIndexes"), new Tuple<string, string>(nameof (value), value.DropIndexes.ToString()));
      this.AppentNode(ownerDocument, configuration3.Node, "DBConfig", new Tuple<string, string>("name", "DataMigrate"), new Tuple<string, string>(nameof (value), value.DataMigrate.ToString()));
      this.AppentNode(ownerDocument, configuration3.Node, "DBConfig", new Tuple<string, string>("name", "UnknownMeasure"), new Tuple<string, string>(nameof (value), value.UnknownMeasure));
      XmlDocument document1 = ownerDocument;
      XmlNode node1 = configuration3.Node;
      Tuple<string, string>[] tupleArray1 = new Tuple<string, string>[2]
      {
        new Tuple<string, string>("name", "PacketSize"),
        null
      };
      int num = value.PacketSize;
      tupleArray1[1] = new Tuple<string, string>(nameof (value), num.ToString());
      this.AppentNode(document1, node1, "DBConfig", tupleArray1);
      XmlDocument document2 = ownerDocument;
      XmlNode node2 = configuration3.Node;
      Tuple<string, string>[] tupleArray2 = new Tuple<string, string>[2]
      {
        new Tuple<string, string>("name", "CommandTimeout"),
        null
      };
      num = value.CommandTimeout;
      tupleArray2[1] = new Tuple<string, string>(nameof (value), num.ToString());
      this.AppentNode(document2, node2, "DBConfig", tupleArray2);
      this.AppentNode(ownerDocument, configuration3.Node, "DBConfig", new Tuple<string, string>("name", "PLPumpingResume"), new Tuple<string, string>(nameof (value), value.PLPumpingResume.ToString()));
      this._impExpConfiguration = new MainConfiguration(value.Plugins, value.CacheTempFolder, value.SettingsTempFolder, value.UnknownMeasure, value.DataMigrate, value.DropIndexes, value.PacketSize, value.CommandTimeout, value.PLPumpingResume);
    }
  }

  private void CheckMainConfiguration(MainConfiguration value)
  {
    foreach (PluginItem plugin in value.Plugins)
    {
      if (plugin.FileName == string.Empty)
        throw new Exception("В свойствах загружаемого модуля не указано имя файла!");
    }
    if (value.UnknownMeasure == string.Empty)
      throw new Exception("Не указана единица измерения по умолчанию!");
  }

  private void AppentNode(
    XmlDocument document,
    XmlNode parent,
    string nodeName,
    params Tuple<string, string>[] attributes)
  {
    XmlNode element = (XmlNode) document.CreateElement(nodeName);
    foreach (Tuple<string, string> attribute1 in attributes)
    {
      XmlAttribute attribute2 = document.CreateAttribute(attribute1.Item1);
      attribute2.Value = attribute1.Item2;
      element.Attributes.Append(attribute2);
    }
    parent.AppendChild(element);
  }

  public void Save(string filename)
  {
    FileStream outStream = new FileStream(filename, FileMode.Create);
    try
    {
      XmlDocument ownerDocument = this.Node.OwnerDocument;
      bool flag = false;
      foreach (XmlNode childNode in ownerDocument.ChildNodes)
      {
        if (ownerDocument.ChildNodes[0].NodeType.Equals((object) XmlNodeType.XmlDeclaration))
          flag = true;
      }
      if (!flag)
      {
        XmlDeclaration xmlDeclaration = ownerDocument.CreateXmlDeclaration("1.0", Encoding.UTF8.WebName, "yes");
        ownerDocument.InsertBefore((XmlNode) xmlDeclaration, ownerDocument.ChildNodes[0]);
      }
      this.Node.OwnerDocument.Save((Stream) outStream);
      outStream.Flush();
    }
    finally
    {
      outStream.Close();
    }
  }

  public IConfiguration Open(string name) => this.Open(name, false);

  public IConfiguration Open(string name, bool viaCreate)
  {
    List<string> stringList = new List<string>((IEnumerable<string>) name.Split('\\'));
    if (stringList.Count == 1)
    {
      XmlNode node = (XmlNode) this.Node[name];
      if (node != null)
        return (IConfiguration) new ConfigurationImpl(node);
      if (!viaCreate)
        return (IConfiguration) null;
      XmlNode element = (XmlNode) this.Node.OwnerDocument.CreateElement(name);
      this.Node.AppendChild(element);
      return (IConfiguration) new ConfigurationImpl(element);
    }
    IConfiguration configuration = this.Open(stringList[0], viaCreate);
    if (configuration == null)
      return (IConfiguration) null;
    stringList.RemoveAt(0);
    return configuration.Open(string.Join("\\", stringList.ToArray()), viaCreate);
  }

  public bool HasAttribute(string name) => this.Node.Attributes[name] != null;

  public string GetAttribute(string name) => this.Node.Attributes[name]?.Value;

  public void SetAttribute(string name, string value)
  {
    XmlAttribute attribute1 = this.Node.Attributes[name];
    if (attribute1 != null)
    {
      attribute1.Value = value;
    }
    else
    {
      XmlAttribute attribute2 = this.Node.OwnerDocument.CreateAttribute(name);
      attribute2.Value = value;
      this.Node.Attributes.Append(attribute2);
    }
  }

  public bool HasText() => !this.Node.HasChildNodes;

  public string GetText() => this.Node.HasChildNodes ? (string) null : this.Node.InnerText;

  public void SetText(string value)
  {
    if (this.Node.HasChildNodes)
      return;
    this.Node.InnerText = value;
  }

  public XmlNode Node { get; private set; }

  public IEnumerator<IConfiguration> GetEnumerator()
  {
    return (IEnumerator<IConfiguration>) new ConfigurationEnumerator(this.Node);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
