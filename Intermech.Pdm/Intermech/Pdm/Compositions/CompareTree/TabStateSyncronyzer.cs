// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.TabStateSyncronyzer
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class TabStateSyncronyzer
{
  private bool _selfScroll;
  private TabControl _tabControl1;
  private TabControl _tabControl2;

  public TabStateSyncronyzer(TabControl tabControl1, TabControl tabControl2)
  {
    this._tabControl1 = tabControl1;
    this._tabControl2 = tabControl2;
    this._tabControl1.SelectedIndexChanged += new EventHandler(this.TabControl1_IndexChanged);
    this._tabControl2.SelectedIndexChanged += new EventHandler(this.TabControl2_IndexChanged);
  }

  private void TabControl2_IndexChanged(object sender, EventArgs e)
  {
    this.OnTabControlIndexChanged(this._tabControl1, this._tabControl2);
  }

  private void TabControl1_IndexChanged(object sender, EventArgs e)
  {
    this.OnTabControlIndexChanged(this._tabControl2, this._tabControl1);
  }

  private void OnTabControlIndexChanged(TabControl tabControlSlave, TabControl tabControlMaster)
  {
    if (this._selfScroll)
      return;
    this._selfScroll = true;
    try
    {
      tabControlSlave.SelectedIndex = tabControlMaster.SelectedIndex;
    }
    finally
    {
      this._selfScroll = false;
    }
  }
}
