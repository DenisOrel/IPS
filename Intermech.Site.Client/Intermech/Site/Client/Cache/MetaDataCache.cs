// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Cache.MetaDataCache
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.IO;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.Site.Client.Cache;

internal sealed class MetaDataCache : IPortalMetadata
{
  private Intermech.Site.Client.Cache.Cache _cache;
  private string _fileFullName = string.Empty;

  public DateTime LastModify => this._cache != null ? this._cache.LastModify : DateTime.MinValue;

  public MetaDataCache()
  {
    string appSetting = ConfigurationManager.AppSettings["LogPath"];
    this._fileFullName = Path.Combine(!string.IsNullOrEmpty(appSetting) ? Environment.ExpandEnvironmentVariables(appSetting) : Path.GetDirectoryName(this.GetType().Module.FullyQualifiedName), "portalcache.dat");
  }

  private void ReloadCache(IUserSession session) => this.ReloadCache(session, true);

  private void ReloadCache(IUserSession session, bool loadIcons)
  {
    Guid connectGuid = Guid.Empty;
    IPortalConnector customService1 = (IPortalConnector) session.GetCustomService(typeof (IPortalConnector));
    ISitesCacheService customService2 = (ISitesCacheService) session.GetCustomService(typeof (ISitesCacheService));
    try
    {
      if (customService1 == null || customService1.IsOffline)
        return;
      connectGuid = customService1.Login(session.SessionGUID);
      DateTime lastModify = customService1.LastModifyMetadata(connectGuid);
      if (!(lastModify != this.LastModify))
        return;
      PortalObjectType[] objectTypesTree = customService1.GetObjectTypesTree(connectGuid);
      PortalAttributeType[] relationAttributes = customService1.GetPublishRelationAttributes(connectGuid);
      AttributePossibleValues[] attributePossibleValues = customService1.GetAttributePossibleValues(connectGuid);
      this._cache = new Intermech.Site.Client.Cache.Cache(lastModify, objectTypesTree, relationAttributes, attributePossibleValues);
      if (loadIcons)
        this.LoadIcons();
      this.Save();
    }
    catch (Exception ex)
    {
      session.EventLog.AddToTrace(string.Format(LocalizationHolder.rm.GetString("Site.Client_1"), (object) ex.Message), Intermech.Consts.traceAlways, string.Empty);
    }
    finally
    {
      if (connectGuid != Guid.Empty && customService1 != null)
        customService1.Logout(connectGuid);
    }
  }

  public PortalObjectType[] GetChildObjectTypes(
    IUserSession session,
    int parentType,
    bool recursive)
  {
    this.ReloadCache(session);
    return this._cache.ObjTypes == null ? (PortalObjectType[]) null : this.GetChildTypes(parentType, recursive).ToArray();
  }

  public PortalObjectType[] GetPublishObjectTypes(IUserSession session)
  {
    this.ReloadCache(session);
    return this._cache.ObjTypes;
  }

  public int GetPublishObjectTypeID(string name)
  {
    if (this._cache.ObjTypes != null && this._cache.ObjTypes.Length != 0)
    {
      for (int index = 0; index < this._cache.ObjTypes.Length; ++index)
      {
        if (this._cache.ObjTypes[index].Name == name)
          return this._cache.ObjTypes[index].ID;
      }
    }
    return -1;
  }

  public string GetPublishObjectTypeName(int typeID)
  {
    if (this._cache.ObjTypes != null && this._cache.ObjTypes.Length != 0)
    {
      for (int index = 0; index < this._cache.ObjTypes.Length; ++index)
      {
        if (this._cache.ObjTypes[index].ID == typeID)
          return this._cache.ObjTypes[index].Name;
      }
    }
    return string.Empty;
  }

  private void LoadIcons()
  {
    if (this._cache == null || this._cache.ObjTypes == null || this._cache.ObjTypes.Length == 0)
      return;
    ICategoryTypeIconService service = (ICategoryTypeIconService) ServicesManager.GetService(typeof (ICategoryTypeIconService));
    for (int index = 0; index < this._cache.ObjTypes.Length; ++index)
    {
      PortalObjectType objType = this._cache.ObjTypes[index];
      if (objType.Icon != null && objType.Icon.Length != 0)
      {
        using (MemoryStream memoryStream = new MemoryStream(objType.Icon))
        {
          using (Icon icon = new Icon((Stream) memoryStream))
          {
            if (objType.GUID == PortalConsts.objtypePacket.ToString())
              service.AddIcon(icon, SiteClientConsts.CategoryPublishPacket, objType.ID);
            service.AddIcon(icon, SiteClientConsts.CategoryPublishType, this._cache.ObjTypes[index].ID);
            service.AddIcon(icon, SiteClientConsts.CategoryPublishObject, this._cache.ObjTypes[index].ID);
          }
        }
      }
    }
  }

  public void Load(IUserSession session)
  {
    try
    {
      this._cache = new Intermech.Site.Client.Cache.Cache();
      if (File.Exists(this._fileFullName) && new FileInfo(this._fileFullName).Length > 0L)
      {
        using (ImChunkedStream imChunkedStream = new ImChunkedStream())
        {
          try
          {
            if (ServicesManager.GetService(typeof (IPackedStream)) is IPackedStream service)
            {
              FileStream inStream = File.OpenRead(this._fileFullName);
              try
              {
                service.UnpackStream((Stream) imChunkedStream, (Stream) inStream);
              }
              finally
              {
                inStream.Flush();
                inStream.Close();
              }
            }
            if (imChunkedStream.Length > 0L)
            {
              BinaryFormatter binaryFormatter = new BinaryFormatter();
              imChunkedStream.Position = 0L;
              this._cache = (Intermech.Site.Client.Cache.Cache) binaryFormatter.Deserialize((Stream) imChunkedStream);
            }
          }
          finally
          {
            imChunkedStream.Flush();
            imChunkedStream.Close();
          }
        }
      }
      this.ReloadCache(session, false);
      this.LoadIcons();
    }
    catch
    {
      this._cache = new Intermech.Site.Client.Cache.Cache();
    }
  }

  public void Save()
  {
    using (ImChunkedStream imChunkedStream = new ImChunkedStream())
    {
      new BinaryFormatter().Serialize((Stream) imChunkedStream, (object) this._cache);
      imChunkedStream.Position = 0L;
      FileStream outStream = new FileStream(this._fileFullName, FileMode.Create, FileAccess.Write);
      try
      {
        if (!(ServicesManager.GetService(typeof (IPackedStream)) is IPackedStream service))
          return;
        service.PackStream((Stream) outStream, (Stream) imChunkedStream, 9);
      }
      finally
      {
        outStream.Flush();
        outStream.Close();
      }
    }
  }

  private List<PortalObjectType> GetChildTypes(int parentID, bool recursive)
  {
    List<PortalObjectType> childTypes = new List<PortalObjectType>();
    for (int index = 0; index < this._cache.ObjTypes.Length; ++index)
    {
      PortalObjectType objType = this._cache.ObjTypes[index];
      if (objType.ParentID == parentID)
      {
        childTypes.Add(objType);
        if (recursive)
          childTypes.AddRange((IEnumerable<PortalObjectType>) this.GetChildTypes(objType.ID, recursive));
      }
    }
    return childTypes;
  }

  public PortalObjectType GetPublishObjectType(int typeID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this._cache.ObjTypes == null)
        this.ReloadCache(sessionKeeper.Session);
      if (this._cache.ObjTypes != null)
      {
        for (int index = 0; index < this._cache.ObjTypes.Length; ++index)
        {
          if (this._cache.ObjTypes[index].ID == typeID)
            return this._cache.ObjTypes[index];
        }
      }
    }
    return (PortalObjectType) null;
  }

  public PortalObjectType GetPublishObjectType(Guid typeGuid)
  {
    string str = typeGuid.ToString();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this._cache.ObjTypes == null)
        this.ReloadCache(sessionKeeper.Session);
      if (this._cache.ObjTypes != null)
      {
        for (int index = 0; index < this._cache.ObjTypes.Length; ++index)
        {
          if (this._cache.ObjTypes[index].GUID == str)
            return this._cache.ObjTypes[index];
        }
      }
    }
    return (PortalObjectType) null;
  }

  public PortalAttributeType[] GetPublishRelationAttributes()
  {
    return this._cache != null ? this._cache.PublishRelationAttributes : (PortalAttributeType[]) null;
  }

  public PortalAttributeType GetAttribute(Guid attributeGuid)
  {
    if (this._cache != null)
    {
      Tuple<string, PortalAttributeType> tuple = this._cache.PublishAttributes.Find((Predicate<Tuple<string, PortalAttributeType>>) (x => x.Item1.Equals(attributeGuid.ToString())));
      if (tuple == null)
      {
        PortalAttributeType attribute = Array.Find<PortalAttributeType>(this._cache.PublishRelationAttributes, (Predicate<PortalAttributeType>) (x => x.GUID.Equals(attributeGuid.ToString())));
        if (attribute != null)
          return attribute;
      }
      if (tuple != null)
        return tuple.Item2;
    }
    return (PortalAttributeType) null;
  }

  public Dictionary<object, string> GetPossibleValues(int attributeID)
  {
    Dictionary<object, string> dictionary;
    return this._cache.PossibleValues != null && this._cache.PossibleValues.TryGetValue(attributeID, out dictionary) ? dictionary : (Dictionary<object, string>) null;
  }

  public PortalAttributeType GetAttribute(int attributeID)
  {
    if (this._cache != null)
    {
      Tuple<string, PortalAttributeType> tuple = this._cache.PublishAttributes.Find((Predicate<Tuple<string, PortalAttributeType>>) (x => x.Item2.ID.Equals(attributeID)));
      if (tuple != null)
        return tuple.Item2;
    }
    return (PortalAttributeType) null;
  }
}
