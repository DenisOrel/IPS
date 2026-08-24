// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.TabSettingsControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal abstract class TabSettingsControl : UserControl, ICompareRulesTab
{
  protected ICategoryTypeIconService iconService;

  public TabSettingsControl()
  {
    this.iconService = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
  }

  public abstract void RefreshData();

  public abstract Guid ID { get; }

  public abstract string Caption { get; }

  public abstract string ToolTipText { get; }

  public abstract int Index { get; }

  public virtual int ImageIndex => -1;

  public Control Control => (Control) this;

  public event TabDataChangedEventHandler TabDataChangedEvent;

  protected virtual void DataChanged()
  {
    TabDataChangedEventHandler dataChangedEvent = this.TabDataChangedEvent;
    if (dataChangedEvent == null)
      return;
    dataChangedEvent((object) this, new TabDataChangedEventArgs(this.ID));
  }

  public virtual void AnotherTabDataChanged(TabDataChangedEventArgs e)
  {
    if (!e.TabGuid.Equals(CompositionSettingsControl.GUID))
      return;
    this.RefreshData();
  }

  protected TreeNode CreateObjectTypeNode(int objectTypeID, object tag, TreeNodeCollection nodes)
  {
    return ControlsHelper.CreateObjectTypeNode(objectTypeID, tag, nodes, this.iconService);
  }

  protected TreeNode CreateRelationTypeNode(int id, object tag, TreeNodeCollection nodes)
  {
    return ControlsHelper.CreateRelationTypeNode(id, tag, nodes, this.iconService);
  }

  public CompoitionSettings Settings { get; set; }
}
