// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.ItemsToCreate.AttributeGroupToCreateList
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

internal class AttributeGroupToCreateList : 
  ItemToCreateList<IAttributeGroupToCreate>,
  IAttributeGroupToCreateList,
  IItemToCreateList<IAttributeGroupToCreate>
{
  public bool Reload()
  {
    bool flag = false;
    if (ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service)
    {
      this.Clear();
      foreach (IAttributeGroupItem attributeGroup in (IEnumerable<IAttributeGroupItem>) service.AttributeGroups)
        this.AddItem(false, attributeGroup.Name, attributeGroup.GUID, (long) attributeGroup.ID);
      flag = true;
    }
    return flag;
  }

  public IAttributeGroupToCreate AddItem(bool isNew, string name, Guid guid, long sysID)
  {
    IAttributeGroupToCreate attributeGroupToCreate = (IAttributeGroupToCreate) new AttributeGroupToCreate(isNew, name, guid, sysID);
    return !this.add(attributeGroupToCreate) ? (IAttributeGroupToCreate) null : attributeGroupToCreate;
  }
}
