// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ObjectTypeItemListImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class ObjectTypeItemListImpl(IDataWriterProxy dataWriter) : 
  TypeItemListImpl<IObjectTypeItem>(dataWriter, "IMS_OBJECT_TYPES"),
  IObjectTypeItemList,
  ITypeItemList<IObjectTypeItem>,
  IList<IObjectTypeItem>,
  ICollection<IObjectTypeItem>,
  IEnumerable<IObjectTypeItem>,
  IEnumerable,
  IList,
  ICollection
{
  private Dictionary<string, IObjectTypeItem> dictionaryShortName = new Dictionary<string, IObjectTypeItem>();

  protected override bool addToHTs(IObjectTypeItem item)
  {
    if (this.ExistsByShortName(item.ShortName) || !base.addToHTs(item))
      return false;
    string key = item.ShortName.ToUpper().Trim();
    if (!key.Equals(string.Empty))
      this.dictionaryShortName.Add(key, item);
    return true;
  }

  public override void Clear()
  {
    this.dictionaryShortName.Clear();
    base.Clear();
  }

  public bool ExistsByShortName(string shortName)
  {
    string key = shortName.ToUpper().Trim();
    return !key.Equals(string.Empty) && this.dictionaryShortName.ContainsKey(key);
  }

  public IObjectTypeItem GetByShortName(string shortName)
  {
    string key = shortName.ToUpper().Trim();
    return this.ExistsByShortName(shortName) ? this.dictionaryShortName[key] : (IObjectTypeItem) null;
  }

  public IObjectTypeItem Add(
    Guid parentID,
    string name,
    string objectName,
    string shortName,
    ObjectVersionModes versionable,
    string note,
    Guid defRelId,
    Guid guid,
    string area,
    int captionAttribute,
    bool anyAttributes,
    lcType publicLc,
    int delTime,
    Guid shemaId,
    byte[] icon)
  {
    return this.dataWriter.CreateObjectType(parentID, name, objectName, shortName, versionable, note, defRelId, guid, area, captionAttribute, anyAttributes, publicLc, delTime, shemaId, icon);
  }

  public void LinkAttributeTypeToObjectType(
    int attrTypeId,
    int objTypeId,
    bool isPublic,
    RequiredModes requiredMod,
    string validationRule,
    ComputeValueModes computeMode,
    string formula,
    UniqueValueModes uniqueMode,
    int level,
    string defaultValue,
    OptimizationModes inViewMode,
    bool isContent,
    AttributeOptions options,
    string mask,
    int masterId,
    int sourceId)
  {
    this.dataWriter.CreateLinkAttrTypeToObjType(attrTypeId, objTypeId, isPublic, requiredMod, validationRule, computeMode, formula, uniqueMode, level, defaultValue, inViewMode, isContent, options, mask, masterId, sourceId);
  }

  public List<int> GetChildTypesRecursive(params int[] parentTypeIDs)
  {
    List<int> childTypesRecursive = new List<int>();
    foreach (int parentTypeId in parentTypeIDs)
    {
      childTypesRecursive.Add(parentTypeId);
      IObjectTypeItem byId = this.GetByID(parentTypeId);
      if (byId != null && byId.ChildIDs != null && byId.ChildIDs.Length != 0)
      {
        foreach (int childId in byId.ChildIDs)
          childTypesRecursive.AddRange((IEnumerable<int>) this.GetChildTypesRecursive(new int[1]
          {
            childId
          }));
      }
    }
    return childTypesRecursive;
  }
}
