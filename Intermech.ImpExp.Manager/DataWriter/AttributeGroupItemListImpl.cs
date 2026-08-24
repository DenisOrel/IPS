// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.AttributeGroupItemListImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class AttributeGroupItemListImpl(IDataWriterProxy dataWriter) : 
  TypeItemListImpl<IAttributeGroupItem>(dataWriter, "IMS_ATTR_GROUPS"),
  IAttributeGroupItemList,
  ITypeItemList<IAttributeGroupItem>,
  IList<IAttributeGroupItem>,
  ICollection<IAttributeGroupItem>,
  IEnumerable<IAttributeGroupItem>,
  IEnumerable,
  IList,
  ICollection
{
  public void LinkAttributeTypeToGroup(int attrTypeId, Guid attrTypeGuid, int attrGroupId)
  {
    this.dataWriter.CreateLinkAttrTypeToGroup(attrTypeId, attrTypeGuid, attrGroupId);
  }

  public IAttributeGroupItem Add(string groupName) => this.Add(groupName, string.Empty);

  public IAttributeGroupItem Add(string groupName, string note)
  {
    return this.Add(groupName, this.dataWriter.NewPumpGuid(), note, string.Empty, string.Empty);
  }

  public IAttributeGroupItem Add(
    string groupName,
    Guid groupGuid,
    string note,
    string area,
    string lang)
  {
    return this.dataWriter.CreateAttributeGroup(groupName, groupGuid, note, area, lang);
  }
}
