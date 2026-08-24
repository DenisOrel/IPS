// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.ItemsToCreate.AttributeTypeToCreateList
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

internal class AttributeTypeToCreateList : 
  ItemToCreateList<IAttributeTypeToCreate>,
  IAttributeTypeToCreateList,
  IItemToCreateList<IAttributeTypeToCreate>
{
  private Dictionary<string, IAttributeTypeToCreate> aliasesDict = new Dictionary<string, IAttributeTypeToCreate>();

  protected override bool addToDictionaries(IAttributeTypeToCreate item)
  {
    if (item.Name.Trim() == string.Empty)
      item.Name = item.GUID.ToString();
    if (this.ExistsByAlias(item.Alias) || !base.addToDictionaries(item))
      return false;
    string key = item.Alias.ToUpper().Trim();
    if (!key.Equals(string.Empty))
      this.aliasesDict.Add(key, item);
    return true;
  }

  public override void Clear()
  {
    this.aliasesDict.Clear();
    base.Clear();
  }

  public bool Reload()
  {
    bool flag = false;
    if (ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service)
    {
      this.Clear();
      foreach (IAttributeTypeItem attributeType in (IEnumerable<IAttributeTypeItem>) service.AttributeTypes)
      {
        bool valueInList = attributeType.MultiValueMode == MultiValueModes.MultiValuesFromList || attributeType.MultiValueMode == MultiValueModes.SingleValueFromList;
        this.AddItem(false, attributeType.Name, attributeType.ShortName, attributeType.Alias, (FieldTypes) attributeType.AttrValueType, (long) attributeType.MaxSize, attributeType.GUID, (long) attributeType.ID, valueInList, -1, Convert.ToString(attributeType.DefaultValue), attributeType.MultiValueMode);
      }
      Dictionary<object, DictionaryValue> category = (ServicesManager.GetService(typeof (ICache)) as ICache).GetCache(ImportingCategory.AttributeTypesToCreate).GetCategory(ImportingCategory.AttributeTypesToCreate);
      if (category != null)
      {
        foreach (DictionaryValue dictionaryValue in category.Values)
        {
          AttributeType tag = dictionaryValue.Tag as AttributeType;
          if (!this.ExistsByGuid(tag.AttrGuid))
            this.AddItem(true, tag.Name, tag.ShortName, tag.Alias, tag.FieldType, tag.Size, tag.AttrGuid, tag.SystemID, tag.ValuesListIds != null && tag.ValuesListIds.Count > 0, tag.ValuesListIds, tag.ValuesListMeasureIDs, tag.DefaultValue, tag.MultiValueMode);
        }
      }
      flag = true;
    }
    return flag;
  }

  public IAttributeTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList)
  {
    return this.AddItem(isNew, name, shortName, alias, fieldType, size, guid, sysID, valueInList, -1);
  }

  public IAttributeTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList,
    int valueListID)
  {
    AttributeTypeToCreate attributeTypeToCreate = new AttributeTypeToCreate(isNew, name, shortName, alias, fieldType, size, guid, sysID, valueInList, valueListID);
    return !this.add((IAttributeTypeToCreate) attributeTypeToCreate) ? (IAttributeTypeToCreate) null : (IAttributeTypeToCreate) attributeTypeToCreate;
  }

  public IAttributeTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList,
    int valueListID,
    string defaultValue,
    MultiValueModes multiValueMode)
  {
    AttributeTypeToCreate attributeTypeToCreate = new AttributeTypeToCreate(isNew, name, shortName, alias, fieldType, size, guid, sysID, valueInList, valueListID, defaultValue, multiValueMode);
    return !this.add((IAttributeTypeToCreate) attributeTypeToCreate) ? (IAttributeTypeToCreate) null : (IAttributeTypeToCreate) attributeTypeToCreate;
  }

  public IAttributeTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid,
    long sysID,
    bool valueInList,
    List<int> valueListIDs,
    Dictionary<int, string> valuesListMeasureIDs,
    string defaultValue,
    MultiValueModes multiValueMode)
  {
    int valueListID = valueListIDs == null || valueListIDs.Count <= 0 ? -1 : valueListIDs[0];
    AttributeTypeToCreate attributeTypeToCreate = new AttributeTypeToCreate(isNew, name, shortName, alias, fieldType, size, guid, sysID, valueInList, valueListID, defaultValue, multiValueMode);
    if (valueListIDs != null && valueListIDs.Count > 1)
    {
      for (int index = 0; index < valueListIDs.Count; ++index)
      {
        if (index != 0)
          attributeTypeToCreate.AddValueInListId(valueListIDs[index], string.Empty);
      }
    }
    if (valuesListMeasureIDs != null)
      attributeTypeToCreate.ValuesListMeasureIDs = valuesListMeasureIDs;
    return !this.add((IAttributeTypeToCreate) attributeTypeToCreate) ? (IAttributeTypeToCreate) null : (IAttributeTypeToCreate) attributeTypeToCreate;
  }

  public bool ExistsByAlias(string alias)
  {
    string key = alias.ToUpper().Trim();
    return !key.Equals(string.Empty) && this.aliasesDict.ContainsKey(key);
  }

  public IAttributeTypeToCreate GetByAlias(string alias)
  {
    string key = alias.ToUpper().Trim();
    return this.aliasesDict.ContainsKey(key) ? this.aliasesDict[key] : (IAttributeTypeToCreate) null;
  }

  public void UpdateCasheAlias(string oldAlias, IAttributeTypeToCreate item)
  {
    if (item == null)
      item = this.GetByAlias(oldAlias);
    this.aliasesDict.Remove(oldAlias.ToUpper().Trim());
    if (item == null || item.Alias.ToUpper().Trim().Equals(string.Empty))
      return;
    this.aliasesDict.Add(item.Alias.ToUpper().Trim(), item);
  }
}
