// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.RelationTypeItemListImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class RelationTypeItemListImpl(IDataWriterProxy dataWriter) : 
  TypeItemListImpl<IRelationTypeItem>(dataWriter, "IMS_RELATION_TYPES"),
  IRelationTypeItemList,
  ITypeItemList<IRelationTypeItem>,
  IList<IRelationTypeItem>,
  ICollection<IRelationTypeItem>,
  IEnumerable<IRelationTypeItem>,
  IEnumerable,
  IList,
  ICollection
{
  public void LinkAttributeTypeToRelationType(
    int attrTypeId,
    int relTypeId,
    RequiredModes requiredMod,
    string validationRule,
    ComputeValueModes computeMode,
    string formula,
    string defaultValue,
    short inViewMode,
    bool isContent,
    AttributeOptions options,
    string mask,
    int masterId,
    int sourceId)
  {
    this.dataWriter.CreateLinkAttrTypeToRelType(attrTypeId, relTypeId, requiredMod, validationRule, computeMode, formula, defaultValue, inViewMode, isContent, options, mask, masterId, sourceId);
  }
}
