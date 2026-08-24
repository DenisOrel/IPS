// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.AttributeGroupItemImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal class AttributeGroupItemImpl : TypeItemImpl, IAttributeGroupItem, ITypeItem
{
  private string note = string.Empty;
  protected List<int> attrTypeIDs = new List<int>();

  public AttributeGroupItemImpl(
    IDataWriterProxy dataWriter,
    int groupTypeID,
    Guid groupGuid,
    string groupName,
    string note)
    : base(dataWriter, groupTypeID, groupGuid, groupName)
  {
    this.note = note;
  }

  public void attrTypeIdAdd(int attrTypeID)
  {
    if (this.attrTypeIDs.Contains(attrTypeID))
      return;
    this.attrTypeIDs.Add(attrTypeID);
  }

  public void attrTypesClear() => this.attrTypeIDs.Clear();

  public int[] AttrTypeIDs => this.attrTypeIDs.ToArray();

  public bool AttrTypeExists(int attrTypeID) => this.attrTypeIDs.Contains(attrTypeID);

  public string Note
  {
    get => this.note;
    set => this.note = value;
  }
}
