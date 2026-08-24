// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.TreeResolutionsView
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

[ViewDescriptionProvider(typeof (TreeResolutionsView.TreeResolutionsViewDescriptionProvider))]
internal class TreeResolutionsView : UserControl, IView
{
  [NotNull]
  private readonly AdvancedServiceContainer _services;
  private bool _dataLoaded;
  private long _objectID;
  private int _objectType;
  [NotNull]
  private NavigatorControl _navControl;
  private IContainer components;

  public TreeResolutionsView()
  {
    this.InitializeComponent();
    this._services = new AdvancedServiceContainer();
    this.ImageIndex = Holder.NamedList.ImageIndex("Office.ResolutionsList");
  }

  public void Initialize([NotNull] ISelectedItems items, IServiceProvider provider)
  {
    IDBTypedObjectID itemData = items.GetItemData<IDBTypedObjectID>(0);
    this._objectID = itemData.ObjectID;
    this._objectType = itemData.ObjectType;
    this._dataLoaded = false;
    this._services.AdvancedProvider = provider;
  }

  public void Activate(IView previousView) => this.LoadData();

  public void Deactivate(IView nextView)
  {
  }

  [NotNull]
  public string Caption => Localization.GetString("Office.Client_16");

  public int ImageIndex { get; }

  public int OrderID => 19;

  private void LoadData()
  {
    if (this._dataLoaded)
      return;
    this._navControl.NavTreeView.SupportedColumns = TreeResolutionsView.SupportedTreeColumns;
    this._navControl.NavTreeView.SetColumns(TreeResolutionsView.DefaultTreeColumns);
    this._navControl.NavTreeView.Build((IDescriptor) new ResolutionsDescriptor(this._objectID, this._objectType, (IServiceProvider) this._services));
    this._dataLoaded = true;
  }

  [NotNull]
  public static NodeColumnCollection DefaultTreeColumns
  {
    get => Utils.CaptionAndStatesesColumns(NodeColumnSortOrder.None);
  }

  [NotNull]
  public static NodeColumnCollection SupportedTreeColumns
  {
    get => Utils.NavigatorColumns(NodeColumnSortOrder.None);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this._navControl = new NavigatorControl();
    this.SuspendLayout();
    this._navControl.Dock = DockStyle.Fill;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._navControl);
    this.Name = nameof (TreeResolutionsView);
    this.Size = new Size(367, 266);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private sealed class TreeResolutionsViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      return new ViewDescription()
      {
        Caption = Localization.GetString("Office.Client_16"),
        ImageIndex = Holder.NamedList.ImageIndex("Office.ResolutionsList"),
        OrderID = 19
      };
    }
  }
}
