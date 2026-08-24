// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.SettingsGroupService
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData;

internal sealed class SettingsGroupService : ISettingsGroupService
{
  private List<ISettingsGroup> groups;

  public event ItemBindChangedEventHandler ItemBindChangedEvent;

  public List<ISettingsGroup> Groups
  {
    get
    {
      if (this.groups == null)
        this.groups = new List<ISettingsGroup>();
      return this.groups;
    }
  }

  public void FireItemBindChanged(ISettingsGroup group, ISettingsItem item)
  {
    ItemBindChangedEventHandler bindChangedEvent = this.ItemBindChangedEvent;
    if (bindChangedEvent == null)
      return;
    bindChangedEvent((object) this, new ItemBindChangedEventArgs(group, item));
  }
}
