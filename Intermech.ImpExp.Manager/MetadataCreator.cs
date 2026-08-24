// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.MetadataCreator
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Manager;

internal sealed class MetadataCreator
{
  private ILogFile _logFile;
  private IMetadataInfo _metadataInfo;
  private ICache _cache;

  public MetadataCreator(ILogFile logFile)
  {
    this._logFile = logFile;
    this._metadataInfo = ServicesManager.ServiceContainer.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    this._cache = ServicesManager.GetService(typeof (ICache)) as ICache;
  }

  public bool Create()
  {
    this._cache.DeleteCache(ImportingCategory.AttributeTypesToCreate, ImportingCategory.ObjectTypesToCreate);
    List<Guid> usedGuids = new List<Guid>();
    this._logFile.WriteMessage("***************************** Результаты привязки метаданных: *****************************");
    this._logFile.WriteMessage("[имя в исходной базе]=[глобальный идентификатор в базе назначения]");
    this._logFile.WriteMessage("(+) метаданное создано в базе назначения.");
    ISettingsGroupService service = ServicesManager.ServiceContainer.GetService(typeof (ISettingsGroupService)) as ISettingsGroupService;
    this.FillUsedGuidsForGroups(service, usedGuids);
    this.CreateAttributeGroups(usedGuids);
    this.CreateAttributeTypes(usedGuids);
    this.CreateObjectTypes(usedGuids);
    bool flag = true;
    foreach (ISettingsGroup group in service.Groups)
    {
      try
      {
        group.DoObjectCreated();
      }
      catch (Exception ex)
      {
        this._logFile.WriteMessage(ex.Message);
        this._logFile.WriteMessage(ex.StackTrace);
        flag = false;
      }
    }
    return flag;
  }

  private void FillUsedGuidsForGroups(ISettingsGroupService service, List<Guid> usedGuids)
  {
    foreach (ISettingsGroup group in service.Groups)
    {
      foreach (ISettingsGroupItem groupItem in group.GroupItems)
      {
        if (groupItem is ISettingsItem)
        {
          if (!((groupItem as ISettingsItem).AttrGuid == Guid.Empty))
            usedGuids.Add((groupItem as ISettingsItem).AttrGuid);
          else
            continue;
        }
        this.FillUsedGuids((IList<Guid>) usedGuids, (IList<ISettingsItem>) groupItem.SettingsItems);
      }
    }
  }

  private void WriteToLog(IItemToCreate item)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendFormat("{0}={1}", (object) item.Name, (object) item.GUID);
    if (item.IsNew)
      stringBuilder.Append(" (+)");
    this._logFile.WriteMessage(stringBuilder.ToString());
  }

  private void CreateAttributeGroups(List<Guid> usedGuids)
  {
    IAttributeGroupToCreateList service = ServicesManager.ServiceContainer.GetService(typeof (IAttributeGroupToCreateList)) as IAttributeGroupToCreateList;
    if (service.Items.Count > 0)
      this._logFile.WriteMessage("Результаты привязки групп атрибутов:");
    foreach (IAttributeGroupToCreate attributeGroupToCreate in (IEnumerable<IAttributeGroupToCreate>) service.Items)
    {
      if (attributeGroupToCreate.IsNew && usedGuids.Contains(attributeGroupToCreate.GUID))
        this._metadataInfo.AttributeGroups.Add(attributeGroupToCreate.Name, attributeGroupToCreate.GUID, attributeGroupToCreate.Note, string.Empty, string.Empty);
      this.WriteToLog((IItemToCreate) attributeGroupToCreate);
    }
  }

  private void CreateAttributeTypes(List<Guid> usedGuids)
  {
    IMeasures service1 = ServicesManager.GetService(typeof (IMeasures)) as IMeasures;
    IImportingData cache = this._cache.GetCache(ImportingCategory.AttributeTypesToCreate);
    IAttributeGroupItem byGuid = this._metadataInfo.AttributeGroups.GetByGuid(new Guid("cadd9ab4-306c-11d8-b4e9-00304f19f545"));
    int id = byGuid != null ? byGuid.ID : 0;
    IAttributeTypeToCreateList service2 = ServicesManager.ServiceContainer.GetService(typeof (IAttributeTypeToCreateList)) as IAttributeTypeToCreateList;
    this._logFile.WriteMessage("Результаты привязки атрибутов:");
    try
    {
      foreach (IAttributeTypeToCreate attributeTypeToCreate in (IEnumerable<IAttributeTypeToCreate>) service2.Items)
      {
        if (attributeTypeToCreate.IsNew && usedGuids.Contains(attributeTypeToCreate.GUID))
        {
          AttributesHelper.SaveAttributeToCreate(attributeTypeToCreate, service1, cache);
          this._metadataInfo.AttributeTypes.Add(attributeTypeToCreate.Name, attributeTypeToCreate.ShortName, attributeTypeToCreate.Alias, string.Empty, attributeTypeToCreate.FieldType, attributeTypeToCreate.DefaultValue, attributeTypeToCreate.MultiValueMode, ComputeValueModes.NotComputableValue, UniqueValueModes.NotUnique, attributeTypeToCreate.Size, 0, string.Empty, string.Empty, attributeTypeToCreate.GUID, string.Empty, false, (short) 0, attributeTypeToCreate.Options, string.Empty, id);
        }
        this.WriteToLog((IItemToCreate) attributeTypeToCreate);
      }
    }
    finally
    {
      if (this._cache != null)
        this._cache.ReleaseCache(ImportingCategory.AttributeTypesToCreate);
    }
  }

  private void CreateObjectTypes(List<Guid> usedGuids)
  {
    IImportingData cache = this._cache.GetCache(ImportingCategory.ObjectTypesToCreate);
    try
    {
      List<IObjectTypeToCreate> objectTypeToCreateList = new List<IObjectTypeToCreate>();
      IObjectTypeToCreateList service = ServicesManager.ServiceContainer.GetService(typeof (IObjectTypeToCreateList)) as IObjectTypeToCreateList;
      if (service.Items.Count > 0)
        this._logFile.WriteMessage("Результат привязки типов объектов:");
      foreach (IObjectTypeToCreate objectTypeToCreate in (IEnumerable<IObjectTypeToCreate>) service.Items)
      {
        IObjectTypeToCreate item = objectTypeToCreate;
        $"{item.Name}={item.GUID}";
        if (item.IsNew && usedGuids.Contains(item.GUID))
        {
          int index = objectTypeToCreateList.FindIndex((Predicate<IObjectTypeToCreate>) (x => x.ParentTypeId == item.GUID));
          if (index < 0)
            objectTypeToCreateList.Add(item);
          else
            objectTypeToCreateList.Insert(index, item);
        }
        this.WriteToLog((IItemToCreate) item);
      }
      foreach (IObjectTypeToCreate objectTypeToCreate in objectTypeToCreateList)
      {
        if (!this._metadataInfo.ObjectTypes.ExistsByName(objectTypeToCreate.Name))
          this._metadataInfo.ObjectTypes.Add(objectTypeToCreate.ParentTypeId, objectTypeToCreate.Name, objectTypeToCreate.InstanceName, objectTypeToCreate.ShortName, objectTypeToCreate.VersionMode, objectTypeToCreate.Note, objectTypeToCreate.DefaultRelationId, objectTypeToCreate.GUID, objectTypeToCreate.Area, objectTypeToCreate.CaptionAttrId, objectTypeToCreate.AnyAttributes, objectTypeToCreate.LcMode, objectTypeToCreate.DaysToDelete, objectTypeToCreate.LcShemaId, objectTypeToCreate.Icon);
        if (cache.GetNewKey(ImportingCategory.ObjectTypesToCreate, (object) objectTypeToCreate.Name) == 0L)
          cache.AddValue(ImportingCategory.ObjectTypesToCreate, (object) objectTypeToCreate.Name, long.MinValue, (ITagImportObject) new ObjectType(objectTypeToCreate.Name, objectTypeToCreate.ShortName, objectTypeToCreate.InstanceName, objectTypeToCreate.GUID, objectTypeToCreate.SystemId, objectTypeToCreate.Icon, objectTypeToCreate.VersionMode, objectTypeToCreate.AnyAttributes, objectTypeToCreate.LcShemaId, objectTypeToCreate.DefaultRelationId, objectTypeToCreate.ParentTypeId));
      }
    }
    finally
    {
      if (this._cache != null)
        this._cache.ReleaseCache(ImportingCategory.ObjectTypesToCreate);
    }
  }

  private void FillUsedGuids(IList<Guid> list, IList<ISettingsItem> items)
  {
    foreach (ISettingsItem settingsItem in (IEnumerable<ISettingsItem>) items)
    {
      list.Add(settingsItem.AttrGuid);
      if (settingsItem is ISettingsGroupItem)
        this.FillUsedGuids(list, (IList<ISettingsItem>) (settingsItem as ISettingsGroupItem).SettingsItems);
    }
  }
}
