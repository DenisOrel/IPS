// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.AttributeTypeItemListImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal class AttributeTypeItemListImpl(IDataWriterProxy dataWriter) : 
  TypeItemListImpl<IAttributeTypeItem>(dataWriter, "IMS_ATTRIBUTES"),
  IAttributeTypeItemList,
  ITypeItemList<IAttributeTypeItem>,
  IList<IAttributeTypeItem>,
  ICollection<IAttributeTypeItem>,
  IEnumerable<IAttributeTypeItem>,
  IEnumerable,
  IList,
  ICollection
{
  protected Dictionary<string, IAttributeTypeItem> dictionaryAlias = new Dictionary<string, IAttributeTypeItem>();

  protected override bool addToHTs(IAttributeTypeItem item)
  {
    if (this.ExistsByAlias(item.Alias) || !base.addToHTs(item))
      return false;
    if (!item.Alias.Equals(string.Empty))
      this.dictionaryAlias.Add(item.Alias, item);
    return true;
  }

  public override void Clear()
  {
    this.dictionaryAlias.Clear();
    base.Clear();
  }

  public bool ExistsByAlias(string alias)
  {
    return alias != null && !alias.Equals(string.Empty) && this.dictionaryAlias.ContainsKey(alias);
  }

  public IAttributeTypeItem GetByAlias(string alias)
  {
    return this.ExistsByAlias(alias) ? this.dictionaryAlias[alias] : (IAttributeTypeItem) null;
  }

  public IAttributeTypeItem Add(string name, FieldTypes fieldType, long size)
  {
    Guid guid = this.dataWriter.NewPumpGuid();
    return this.Add(name, guid.ToString(), fieldType, size);
  }

  public IAttributeTypeItem Add(string name, string guidStr, FieldTypes fieldType, long size)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    string empty4 = string.Empty;
    MultiValueModes multiMode = MultiValueModes.SingleValue;
    ComputeValueModes computeMode = ComputeValueModes.NotComputableValue;
    UniqueValueModes uniqueMode = UniqueValueModes.NotUnique;
    int level = 0;
    string empty5 = string.Empty;
    string empty6 = string.Empty;
    Guid guid = new Guid(guidStr);
    string empty7 = string.Empty;
    bool isContent = false;
    short inView = 0;
    AttributeOptions options = AttributeOptions.None;
    string empty8 = string.Empty;
    return this.Add(name, empty1, empty2, empty3, fieldType, empty4, multiMode, computeMode, uniqueMode, size, level, empty5, empty6, guid, empty7, isContent, inView, options, empty8, 0);
  }

  public IAttributeTypeItem Add(
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    long size,
    Guid guid)
  {
    MultiValueModes multiMode = MultiValueModes.SingleValue;
    return this.Add(name, shortName, alias, fieldType, multiMode, size, guid);
  }

  public IAttributeTypeItem Add(
    string name,
    string shortName,
    string alias,
    FieldTypes fieldType,
    MultiValueModes multiMode,
    long size,
    Guid guid)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    ComputeValueModes computeMode = ComputeValueModes.NotComputableValue;
    UniqueValueModes uniqueMode = UniqueValueModes.NotUnique;
    int level = 0;
    string empty3 = string.Empty;
    string empty4 = string.Empty;
    string empty5 = string.Empty;
    bool isContent = false;
    short inView = 0;
    AttributeOptions options = AttributeOptions.None;
    string empty6 = string.Empty;
    return this.Add(name, shortName, alias, empty1, fieldType, empty2, multiMode, computeMode, uniqueMode, size, level, empty3, empty4, guid, empty5, isContent, inView, options, empty6, 0);
  }

  public IAttributeTypeItem Add(
    string name,
    string shortName,
    string alias,
    string note,
    FieldTypes fieldType,
    string defVal,
    MultiValueModes multiMode,
    ComputeValueModes computeMode,
    UniqueValueModes uniqueMode,
    long size,
    int level,
    string formula,
    string language,
    Guid guid,
    string area,
    bool isContent,
    short inView,
    AttributeOptions options,
    string mask,
    int groupID)
  {
    return this.dataWriter.CreateAttributeType(name, shortName, alias, note, fieldType, defVal, multiMode, computeMode, uniqueMode, size, level, formula, language, guid, area, isContent, inView, options, mask, groupID);
  }
}
