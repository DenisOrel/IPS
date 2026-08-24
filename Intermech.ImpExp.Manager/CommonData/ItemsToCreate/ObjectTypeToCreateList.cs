// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.ItemsToCreate.ObjectTypeToCreateList
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData.ItemsToCreate;

internal class ObjectTypeToCreateList : 
  ItemToCreateList<IObjectTypeToCreate>,
  IObjectTypeToCreateList,
  IItemToCreateList<IObjectTypeToCreate>
{
  private Dictionary<string, IObjectTypeToCreate> shortNamesDict = new Dictionary<string, IObjectTypeToCreate>();

  protected override bool addToDictionaries(IObjectTypeToCreate item)
  {
    if (this.ExistsByShortName(item.ShortName) || !base.addToDictionaries(item))
      return false;
    string key = item.ShortName.ToUpper().Trim();
    if (!key.Equals(string.Empty))
      this.shortNamesDict.Add(key, item);
    return true;
  }

  public override void Clear()
  {
    this.shortNamesDict.Clear();
    base.Clear();
  }

  public bool Reload()
  {
    bool flag = false;
    if (ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service)
    {
      this.Clear();
      foreach (IObjectTypeItem objectType in (IEnumerable<IObjectTypeItem>) service.ObjectTypes)
        this.AddItem(false, objectType.Name, objectType.ShortName, objectType.ObjectName, objectType.GUID, (long) objectType.ID, (byte[]) null, objectType.VersionableMode, objectType.AnyAttribute, objectType.ShemaId, objectType.RelationID, objectType.ParentID);
      flag = true;
    }
    Dictionary<object, DictionaryValue> category = (ServicesManager.GetService(typeof (ICache)) as ICache).GetCache(ImportingCategory.ObjectTypesToCreate).GetCategory(ImportingCategory.ObjectTypesToCreate);
    if (category != null)
    {
      foreach (DictionaryValue dictionaryValue in category.Values)
      {
        ObjectType tag = dictionaryValue.Tag as ObjectType;
        if (!this.ExistsByGuid(tag.Guid))
          this.AddItem(true, tag.Name, tag.ShortName, tag.InstanceName, tag.Guid, tag.SysID, tag.Icon, tag.ObjectVersionMode, tag.AnyAttribute, tag.LcShema, tag.DefaultRelation, tag.ParentType);
      }
    }
    return flag;
  }

  public IObjectTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    Guid guid,
    long sysID)
  {
    return this.AddItem(isNew, name, shortName, name, guid, sysID);
  }

  public IObjectTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID)
  {
    return this.AddItem(isNew, name, shortName, instanceName, guid, sysID, (byte[]) null);
  }

  public IObjectTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID,
    byte[] icon)
  {
    return this.AddItem(isNew, name, shortName, instanceName, guid, sysID, icon, ObjectVersionModes.SingleVersion);
  }

  public IObjectTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID,
    byte[] icon,
    ObjectVersionModes versionable)
  {
    return this.AddItem(isNew, name, shortName, instanceName, guid, sysID, icon, versionable, true, Guid.Empty, Guid.Empty, Guid.Empty);
  }

  public IObjectTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID,
    byte[] icon,
    ObjectVersionModes versionable,
    bool anyAttribute,
    Guid LcShemaId,
    Guid defaultRelationID,
    Guid parentTypeId)
  {
    ObjectTypeToCreate objectTypeToCreate = new ObjectTypeToCreate(isNew, name, shortName, instanceName, guid, sysID, icon, versionable, anyAttribute, LcShemaId, defaultRelationID, parentTypeId);
    return !this.add((IObjectTypeToCreate) objectTypeToCreate) ? (IObjectTypeToCreate) null : (IObjectTypeToCreate) objectTypeToCreate;
  }

  public bool ExistsByShortName(string shortName)
  {
    string key = shortName.ToUpper().Trim();
    return !key.Equals(string.Empty) && this.shortNamesDict.ContainsKey(key);
  }

  public IObjectTypeToCreate GetByShortName(string shortName)
  {
    string key = shortName.ToUpper().Trim();
    return this.shortNamesDict.ContainsKey(key) ? this.shortNamesDict[key] : (IObjectTypeToCreate) null;
  }

  public void UpdateCasheShortName(string shortName, IObjectTypeToCreate item)
  {
    if (item == null)
      item = this.GetByShortName(shortName);
    this.shortNamesDict.Remove(shortName.ToUpper().Trim());
    if (item == null || item.ShortName.ToUpper().Trim().Equals(string.Empty))
      return;
    this.shortNamesDict.Add(item.ShortName.ToUpper().Trim(), item);
  }
}
