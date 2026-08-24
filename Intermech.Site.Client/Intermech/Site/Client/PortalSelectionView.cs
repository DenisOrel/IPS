// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalSelectionView
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

[ViewDescriptionProvider(typeof (PortalSelectionView.PortalSelectionViewDescriptionProvider))]
public class PortalSelectionView : UserControl, IView
{
  private bool _activate;
  private long _objectID;
  private int _objectTypeID;
  private IServiceProvider _services;
  private PortalSelectionDialog sForm;
  private INotificationService _notificationService;
  private int imageIndex = -1;
  private IContainer components;

  public PortalSelectionView() => this.InitializeComponent();

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    this._services = provider;
    this._objectID = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
    this._objectTypeID = (items.GetItemData(0, typeof (IDBObjectTypeID)) as IDBObjectTypeID).Value;
    if (this._services != null)
      this._notificationService = this._services.GetService(typeof (INotificationService)) as INotificationService;
    INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    if (service != null)
      this.imageIndex = service.ImageIndex("imgCard");
    this._activate = false;
  }

  public void Activate(IView previousView)
  {
    if (this.sForm == null)
    {
      this.sForm = new PortalSelectionDialog();
      this.sForm.SetParent((Control) this, true);
    }
    if (this._activate)
      return;
    this.sForm.SelectionLoad(this._objectID);
    this._activate = true;
  }

  public void Deactivate(IView nextView)
  {
    if (this.sForm == null)
      return;
    this._services.GetService(typeof (IViewState));
    if (!this.sForm.IsModified)
      return;
    if (MessageBox.Show(LocalizationHolder.rm.GetString("Site.Client_33"), LocalizationHolder.rm.GetString("Site.Client_34"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      this.sForm.SelectionSave();
    else
      this._activate = false;
    this.sForm.IsModified = false;
  }

  public string Caption => LocalizationHolder.rm.GetString("Site.Client_35");

  public int ImageIndex => this.imageIndex;

  public int OrderID => 1;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PortalSelectionView));
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (PortalSelectionView);
    this.ResumeLayout(false);
  }

  private sealed class PortalSelectionViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList service))
        service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      INamedImageList namedImageList = service;
      return new ViewDescription()
      {
        Caption = LocalizationHolder.rm.GetString("Site.Client_35"),
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgCard") : -1,
        OrderID = 1
      };
    }
  }
}
