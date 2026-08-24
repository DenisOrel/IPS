// Decompiled with JetBrains decompiler
// Type: Intermech.ImShape.Client.ImShapeSystemSettingsService
// Assembly: Intermech.ImShape.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EAEE73DE-1C1F-4401-8BB6-D181BFA32870
// Assembly location: D:\IPS\Client\Intermech.ImShape.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

#nullable disable
namespace Intermech.ImShape.Client;

public class ImShapeSystemSettingsService
{
  private const string ROOT_NODE = "ImShapeSystemSettings";
  private const string OBJ_TYPES = "ObjectTypes";
  private const string OBJ_TYPE = "ObjectType";
  private const string TYPE_GUID = "TypeGuid";
  private const string AUTO_REG = "AutoReg";
  private ImShapeCommandProvider _commandProvider = new ImShapeCommandProvider();
  private Dictionary<int, bool> _typeIDs = new Dictionary<int, bool>();
  private int _partTypeID = -1;

  public Dictionary<int, bool> TypeIDs
  {
    get => this._typeIDs;
    private set
    {
      this.UnregisterTypes();
      this._typeIDs = value;
      this.RegisterTypes();
    }
  }

  public ImShapeSystemSettingsService()
  {
    this._partTypeID = MetaDataHelper.GetObjectTypeID("cad00250-306c-11d8-b4e9-00304f19f545");
    this.LoadSettings();
  }

  private void RegisterTypes()
  {
    IFactory service = ServiceUtils.GetService<IFactory>((object) ServicesManager.ServiceContainer, true);
    if (service == null)
      return;
    foreach (KeyValuePair<int, bool> typeId in this.TypeIDs)
      service.AddCommandsProvider(1, typeId.Key, (ICommandsProvider) this._commandProvider);
    service.AddCommandsProvider(1, this._partTypeID, (ICommandsProvider) this._commandProvider);
  }

  private void UnregisterTypes()
  {
    IFactory service = ServiceUtils.GetService<IFactory>((object) ServicesManager.ServiceContainer, true);
    if (service == null)
      return;
    foreach (KeyValuePair<int, bool> typeId in this.TypeIDs)
      service.RemoveCommandsProvider(1, typeId.Key, (ICommandsProvider) this._commandProvider);
    service.RemoveCommandsProvider(1, this._partTypeID, (ICommandsProvider) this._commandProvider);
  }

  private void LoadSettings()
  {
    Dictionary<int, bool> dictionary = (Dictionary<int, bool>) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      BlobInformation config_info;
      byte[] config_file;
      sessionKeeper.Session.Configurations.LoadConfigData("ImShape.SystemSettings", out config_info, out config_file, 0L);
      if (config_info.RealFileSize != 0L)
      {
        if (config_file != null)
        {
          if (config_file.Length != 0)
          {
            string xml = string.Empty;
            lock (this)
            {
              using (MemoryStream inStream = new MemoryStream(config_file))
              {
                inStream.Position = 0L;
                using (MemoryStream memoryStream = new MemoryStream(config_file.Length / 4))
                {
                  ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true).UnpackStream((Stream) memoryStream, (Stream) inStream);
                  memoryStream.Position = 0L;
                  using (BinaryReader binaryReader = new BinaryReader((Stream) memoryStream))
                    xml = binaryReader.ReadString();
                }
              }
            }
            dictionary = !string.IsNullOrEmpty(xml) ? this.ParseXML(xml) : (Dictionary<int, bool>) null;
          }
        }
      }
    }
    List<int> objectTypeChildrenId = MetaDataHelper.GetObjectTypeChildrenID(MetaDataHelper.GetObjectTypeID("cad0078f-306c-11d8-b4e9-00304f19f545"));
    bool flag = false;
    if (dictionary != null)
    {
      foreach (int key in objectTypeChildrenId)
      {
        if (dictionary.ContainsKey(key))
        {
          this._typeIDs.Add(key, dictionary[key]);
        }
        else
        {
          this._typeIDs.Add(key, false);
          flag = true;
        }
      }
    }
    else
    {
      this._typeIDs = objectTypeChildrenId.ToDictionary<int, int, bool>((Func<int, int>) (x => x), (Func<int, bool>) (y => false));
      flag = true;
    }
    if (flag)
      this.SaveSistemSettings(this._typeIDs);
    else
      this.RegisterTypes();
  }

  private Dictionary<int, bool> ParseXML(string xml)
  {
    Dictionary<int, bool> dictionary = (Dictionary<int, bool>) null;
    if (!string.IsNullOrEmpty(xml))
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.InnerXml = xml;
      XmlNode xmlNode = xmlDocument.SelectSingleNode($"{"ImShapeSystemSettings"}/{"ObjectTypes"}");
      if (xmlNode != null && xmlNode.ChildNodes.Count > 0)
      {
        dictionary = new Dictionary<int, bool>(xmlNode.ChildNodes.Count);
        foreach (XmlNode childNode in xmlNode.ChildNodes)
        {
          XmlAttribute attribute1 = childNode.Attributes["TypeGuid"];
          if (attribute1 != null && GuidHelper.IsGuid(attribute1.Value))
          {
            Guid objTypeGuid = new Guid(attribute1.Value);
            if (!(objTypeGuid == Guid.Empty))
            {
              int objectTypeId = MetaDataHelper.GetObjectTypeID(objTypeGuid);
              if (objectTypeId != -1)
              {
                XmlAttribute attribute2 = childNode.Attributes["AutoReg"];
                dictionary.Add(objectTypeId, attribute2 != null && Convert.ToBoolean(attribute2.Value));
              }
            }
          }
        }
      }
    }
    return dictionary == null || dictionary.Count <= 0 ? (Dictionary<int, bool>) null : dictionary;
  }

  public void SaveSistemSettings(Dictionary<int, bool> typeIDs)
  {
    this.TypeIDs = typeIDs;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!sessionKeeper.Session.IsAdmin)
        return;
      string str = this.BuildSettings(typeIDs);
      IDBConfigurations configurations = sessionKeeper.Session.Configurations;
      if (configurations == null)
        return;
      using (MemoryStream memoryStream = new MemoryStream(str.Length))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write(str);
          binaryWriter.Flush();
          IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
          memoryStream.Position = 0L;
          using (MemoryStream outStream = new MemoryStream((int) memoryStream.Length / 2))
          {
            service.PackStream((Stream) outStream, (Stream) memoryStream, 9);
            BlobInformation config_info = new BlobInformation(outStream.Length, outStream.Length, DateTime.Now, "ImShape.SystemSettings", ArcMethods.ZLibPacked, string.Empty);
            configurations.WriteConfigData(config_info, outStream.ToArray(), 0L);
          }
        }
      }
    }
  }

  private string BuildSettings(Dictionary<int, bool> typeIDs)
  {
    string empty = string.Empty;
    XmlDocument xmlDocument = new XmlDocument();
    XmlNode element1 = (XmlNode) xmlDocument.CreateElement("ImShapeSystemSettings");
    XmlNode element2 = (XmlNode) xmlDocument.CreateElement("ObjectTypes");
    if (typeIDs != null)
    {
      foreach (KeyValuePair<int, bool> typeId in typeIDs)
      {
        Guid objectTypeGuid = MetaDataHelper.GetObjectTypeGuid(typeId.Key);
        if (!(objectTypeGuid == Guid.Empty))
        {
          XmlElement element3 = xmlDocument.CreateElement("ObjectType");
          element3.SetAttribute("TypeGuid", Convert.ToString((object) objectTypeGuid));
          element3.SetAttribute("AutoReg", Convert.ToString(typeId.Value));
          element2.AppendChild((XmlNode) element3);
        }
      }
    }
    element1.AppendChild(element2);
    return element1.OuterXml;
  }
}
