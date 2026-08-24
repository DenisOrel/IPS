// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ListInstancesNavWindow
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Docking;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Pdm;

internal sealed class ListInstancesNavWindow : NavWindow
{
  private static readonly Guid _persistStateGuid = new Guid("FABD2D0F-07C6-4165-B648-7B858A2BA6B8");

  public ListInstancesNavWindow() => this.Guid = ListInstancesNavWindow._persistStateGuid;

  public void SetDescriptor(IDescriptor descriptor)
  {
    if (this.RootDescriptor == null || this.RootDescriptor != descriptor)
      this.RootDescriptor = descriptor;
    this.SetTreeViewColumns();
    this.TreeView.Build(descriptor);
  }

  public new static DockControl RestoreWindowCallback(Guid guid, string persistString)
  {
    return guid != ListInstancesNavWindow._persistStateGuid ? (DockControl) null : NavWindow.RestoreWindow((NavWindow) new ListInstancesNavWindow(), guid, persistString);
  }
}
