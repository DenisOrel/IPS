// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareNavWindow
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Docking;
using Intermech.Interfaces.Pdm;
using Intermech.Navigator.Controls;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal class CompareNavWindow : NavWindow
{
  private static readonly Guid _persistStateGuid = new Guid("{5BA39D40-B30B-4c6b-BC56-5FCE7F9F68D1}");

  public static CompareNavWindow Create()
  {
    NavWindowBase.OverrideTreeViewClass = typeof (CompareObjectsTreeView);
    return new CompareNavWindow();
  }

  protected CompareNavWindow() => this.Guid = CompareNavWindow._persistStateGuid;

  public static DockControl RestoreCompareNavWindowCallback(Guid guid, string persistString)
  {
    if (guid != CompareNavWindow._persistStateGuid)
      return (DockControl) null;
    NavWindowBase.OverrideTreeViewClass = typeof (CompareObjectsTreeView);
    return NavWindow.RestoreWindow((NavWindow) new CompareNavWindow(), guid, persistString);
  }

  protected override void OnBeforeFirstShown(EventArgs e)
  {
    base.OnBeforeFirstShown(e);
    this.Text = PDMPluginConsts.CompareObjectsWindow;
  }
}
