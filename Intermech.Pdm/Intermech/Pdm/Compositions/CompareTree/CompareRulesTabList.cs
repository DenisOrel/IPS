// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareRulesTabList
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class CompareRulesTabList
{
  private List<ICompareRulesTab> _tabs;
  private CompoitionSettings _settings;

  public CompareRulesTabList() => this._tabs = this.InitializeList();

  public void LoadSettingsToControls(CompoitionSettings settings)
  {
    this._settings = settings;
    foreach (ICompareRulesTab tab in this._tabs)
    {
      tab.Settings = this._settings;
      tab.RefreshData();
    }
  }

  private List<ICompareRulesTab> InitializeList()
  {
    List<ICompareRulesTab> source = new List<ICompareRulesTab>()
    {
      (ICompareRulesTab) new CompositionSettingsControl(),
      (ICompareRulesTab) new IDAttributesControl(),
      (ICompareRulesTab) new CompareAttributesControl(),
      (ICompareRulesTab) new SortingAttributesControl()
    };
    foreach (ICompareRulesTab compareRulesTab in source)
      compareRulesTab.TabDataChangedEvent += new TabDataChangedEventHandler(this.OnTabDataChangedEvent);
    return source.OrderBy<ICompareRulesTab, int>((Func<ICompareRulesTab, int>) (x => x.Index)).ToList<ICompareRulesTab>();
  }

  private void OnTabDataChangedEvent(object sender, TabDataChangedEventArgs e)
  {
    foreach (ICompareRulesTab tab in this._tabs)
    {
      if (!tab.ID.Equals(e.TabGuid))
        tab.AnotherTabDataChanged(e);
    }
    EventHandler dataChanged = this.DataChanged;
    if (dataChanged == null)
      return;
    dataChanged((object) this, new EventArgs());
  }

  public void AppendTabs(TabControl tabControl)
  {
    tabControl.TabPages.Clear();
    foreach (ICompareRulesTab tab in this._tabs)
    {
      TabPage tabPage = new TabPage(tab.Caption)
      {
        ToolTipText = tab.ToolTipText
      };
      tabPage.Controls.Add(tab.Control);
      tab.Control.Dock = DockStyle.Fill;
      tabPage.ImageIndex = tab.ImageIndex;
      tabControl.TabPages.Add(tabPage);
    }
  }

  public event EventHandler DataChanged;
}
