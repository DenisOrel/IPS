// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.ItemsToCreate.AttributeGroupToCreate
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData.ItemsToCreate;

internal class AttributeGroupToCreate : 
  ItemToCreate,
  IAttributeGroupToCreate,
  IItemToCreate,
  ICustomTypeDescriptor
{
  private string _note = string.Empty;

  public AttributeGroupToCreate(bool isNew, string name, Guid guid, long sysID)
    : base(isNew, name, guid, sysID)
  {
  }

  public AttributeGroupToCreate(bool isNew, string name, Guid guid, long sysID, string note)
    : base(isNew, name, guid, sysID)
  {
    this._note = note;
  }

  [DisplayName("Комментарии")]
  public string Note
  {
    get => this._note;
    set => this._note = value;
  }
}
