// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.OptionEditorView
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Controls;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator;

[ViewDescriptionProvider(typeof (OptionEditorView.OptionEditorViewDescriptionProvider))]
public class OptionEditorView : UserControl, IView
{
  internal INamedImageList _images;
  internal ICurrentUserAndRole _userAndRole;
  internal ICategoryTypeIconService _categoryImages;
  internal static int _imgOption = -1;
  internal NotificationEventHandler _notifyHandler;
  internal ISelectedItems _items;
  private INotificationService _notifications;
  internal IServiceProvider _services;
  private IContainer components;
  private ImageList imageList;
  private ToolTip toolTip;
  private Panel panelBottom;
  private Label labelPicture;
  private Label labelWarning;
  private Button btnCancel;
  private Button btnApply;
  private OptionEditor editor;

  public OptionEditorView()
  {
    this.InitializeComponent();
    this.InitViewResources();
  }

  public void InitViewResources()
  {
    this._images = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._userAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    this._categoryImages = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._notifications = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    OptionEditorView._imgOption = OptionEditorView._imgOption < 0 ? this._images.ImageIndex("imgPdmConfigurator.Options") : OptionEditorView._imgOption;
  }

  public void DisposeViewResources()
  {
    this._images = (INamedImageList) null;
    this._categoryImages = (ICategoryTypeIconService) null;
    this._items = (ISelectedItems) null;
    this._services = (IServiceProvider) null;
    this._notifications = (INotificationService) null;
  }

  public string Caption => LocalizationHolder.rm.GetString("PdmConfigurator_82");

  public int ImageIndex => OptionEditorView._imgOption;

  public int OrderID => 0;

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    this._items = items;
    this._services = provider;
  }

  public void Activate(IView previousView)
  {
    if (this._notifications != null && this._notifyHandler == null)
    {
      this._notifyHandler = new NotificationEventHandler(this.NotificationEventFired);
      this._notifications.Subscribe(this._notifyHandler);
    }
    IViewState service = this._services != null ? this._services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    this.editor.ReadOnly = ((service != null ? (long) service.ViewState : 0L) & 2L) == 2L;
    this.panelBottom.Visible = !this.editor.ReadOnly;
    this.LoadViewData();
  }

  public void Deactivate(IView nextView)
  {
    if (this._notifications != null && this._notifyHandler != null)
    {
      this._notifications.Unsubscribe(this._notifyHandler);
      this._notifyHandler = (NotificationEventHandler) null;
    }
    if (!this.editor.IsChanged || MessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_83"), LocalizationHolder.rm.GetString("PdmConfigurator_84"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
      return;
    this.DoApply((object) this, (EventArgs) null);
  }

  protected virtual void NotificationEventFired(object sender, NotificationEventArgs e)
  {
  }

  internal void Clear()
  {
    this.editor.Clear();
    this.UpdateControls();
  }

  internal void LoadViewData()
  {
    this.Clear();
    if (this._items == null || this._items.Count == 0)
      return;
    IDBTypedObjectID itemData = this._items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    if (!MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID))
      return;
    this.editor.Services = this._services;
    this.editor.CurrentOptionObjectID = itemData.ObjectID;
    this.UpdateControls();
  }

  internal void UpdateControls()
  {
    this.labelPicture.Visible = this.editor.AccessRights != OptionAccessRights.FullAccess;
    this.labelWarning.Visible = this.labelPicture.Visible;
    this.btnApply.Enabled = this.editor.IsChanged;
    this.btnCancel.Enabled = this.editor.IsChanged;
  }

  private void editor_OnChanged(object sender, EventArgs e) => this.UpdateControls();

  private void DoApply(object sender, EventArgs e)
  {
    if (!this.editor.IsChanged)
      return;
    if (this.editor.AccessRights != OptionAccessRights.FullAccess)
      return;
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetObject(this.editor.CurrentOptionObjectID, false) is IDBConfiguratorOption configuratorOption)
          this.editor.Option.SaveToObject((IDBAttributable) configuratorOption);
      }
      this.editor.Fix();
      (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", this.editor.CurrentOptionObjectID));
    }
    catch (Exception ex)
    {
      if (ex is PdmConfiguratorExeption)
      {
        int num1 = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), ex.Message, MessageBoxButtons.OK, IMMessageBoxImage.Information);
      }
      else if (ex.InnerException is PdmConfiguratorExeption)
      {
        int num2 = (int) IMMessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_7"), ex.InnerException.Message, MessageBoxButtons.OK, IMMessageBoxImage.Information);
      }
      else
        throw;
    }
  }

  private void DoCancel(object sender, EventArgs e)
  {
    if (!this.editor.IsChanged || this.editor.AccessRights != OptionAccessRights.FullAccess)
      return;
    this.editor.Undo();
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OptionEditorView));
    this.imageList = new ImageList(this.components);
    this.toolTip = new ToolTip(this.components);
    this.panelBottom = new Panel();
    this.labelPicture = new Label();
    this.labelWarning = new Label();
    this.btnCancel = new Button();
    this.btnApply = new Button();
    this.editor = new OptionEditor();
    this.panelBottom.SuspendLayout();
    this.SuspendLayout();
    this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
    this.imageList.TransparentColor = Color.Transparent;
    this.imageList.Images.SetKeyName(0, "warning.png");
    componentResourceManager.ApplyResources((object) this.panelBottom, "panelBottom");
    this.panelBottom.Controls.Add((Control) this.labelPicture);
    this.panelBottom.Controls.Add((Control) this.labelWarning);
    this.panelBottom.Controls.Add((Control) this.btnCancel);
    this.panelBottom.Controls.Add((Control) this.btnApply);
    this.panelBottom.Name = "panelBottom";
    componentResourceManager.ApplyResources((object) this.labelPicture, "labelPicture");
    this.labelPicture.ImageList = this.imageList;
    this.labelPicture.Name = "labelPicture";
    componentResourceManager.ApplyResources((object) this.labelWarning, "labelWarning");
    this.labelWarning.Name = "labelWarning";
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Cursor = Cursors.Default;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Click += new EventHandler(this.DoCancel);
    componentResourceManager.ApplyResources((object) this.btnApply, "btnApply");
    this.btnApply.Cursor = Cursors.Default;
    this.btnApply.Name = "btnApply";
    this.btnApply.Click += new EventHandler(this.DoApply);
    componentResourceManager.ApplyResources((object) this.editor, "editor");
    this.editor.IsChanged = false;
    this.editor.Name = "editor";
    this.editor.Changed += new EventHandler(this.editor_OnChanged);
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.editor);
    this.Controls.Add((Control) this.panelBottom);
    this.MinimumSize = new Size(450, (int) byte.MaxValue);
    this.Name = nameof (OptionEditorView);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.panelBottom.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private sealed class OptionEditorViewDescriptionProvider : BaseViewDescriptionProvider
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
        Caption = LocalizationHolder.rm.GetString("PdmConfigurator_82"),
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgPdmConfigurator.Options") : -1,
        OrderID = 0
      };
    }
  }
}
