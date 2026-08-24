// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.AttributableTypeItem
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal class AttributableTypeItem(IDataWriterProxy dataWriter, int id, Guid guid, string name) : 
  TypeItemImpl(dataWriter, id, guid, name),
  IAttributableTypeItem
{
  protected List<int> attrTypeIDs = new List<int>();

  public void AddAttrTypeId(int attrTypeID)
  {
    if (this.AttrTypeExists(attrTypeID))
      return;
    this.attrTypeIDs.Add(attrTypeID);
  }

  public int[] AttrTypeIDs => this.attrTypeIDs.ToArray();

  public bool AttrTypeExists(int attrTypeID) => this.attrTypeIDs.Contains(attrTypeID);
}
