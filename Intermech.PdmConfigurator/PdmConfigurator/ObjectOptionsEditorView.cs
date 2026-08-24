// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.ObjectOptionsEditorView
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Client.Core;
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
namespace Intermech.PdmConfigurator;

[ViewDescriptionProvider(typeof (ObjectOptionsEditorView.ObjectOptionsEditorViewDescriptionProvider))]
public class ObjectOptionsEditorView : UserControl, IView
{
  internal INamedImageList _images;
  internal ICurrentUserAndRole _userAndRole;
  internal ICategoryTypeIconService _categoryImages;
  internal static int _imgOption = -1;
  internal NotificationEventHandler _notifyHandler;
  internal NotificationEventHandler _globalNotifyHandler;
  internal ISelectedItems _items;
  private INotificationService _notifications;
  private INotificationService _globalNotificationService;
  internal IServiceProvider _services;
  private IContainer components;
  private ToolTip toolTip;
  private Panel panelBottom;
  private Button btnCancel;
  private Button btnApply;
  private ObjectOptionsEditor editor;

  public ObjectOptionsEditorView()
  {
    this.InitializeComponent();
    this.InitViewServices();
  }

  public void InitViewServices()
  {
    if (this._globalNotificationService != null)
      return;
    this._images = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._userAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    this._categoryImages = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._globalNotificationService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    this._notifications = this._services == null ? (INotificationService) null : this._services.GetService(typeof (INotificationService)) as INotificationService;
    if (this._notifyHandler == null && this._notifications != null)
    {
      this._notifyHandler = new NotificationEventHandler(this.NotificationEventFired);
      this._notifications.Subscribe(this._notifyHandler);
    }
    if (this._globalNotifyHandler == null && this._globalNotificationService != null)
    {
      this._globalNotifyHandler = new NotificationEventHandler(this.GlobalNotificationEventFired);
      this._globalNotificationService.Subscribe(this._globalNotifyHandler);
    }
    ObjectOptionsEditorView._imgOption = ObjectOptionsEditorView._imgOption < 0 ? this._images.ImageIndex("imgPdmConfigurator.Options") : ObjectOptionsEditorView._imgOption;
  }

  public void DisposeViewServices()
  {
    if (this._globalNotificationService == null)
      return;
    if (this._notifyHandler != null && this._notifications != null)
      this._notifications.Unsubscribe(this._notifyHandler);
    if (this._globalNotifyHandler != null && this._globalNotificationService != null)
      this._globalNotificationService.Unsubscribe(this._globalNotifyHandler);
    this._globalNotificationService = (INotificationService) null;
    this._notifications = (INotificationService) null;
    this._notifyHandler = (NotificationEventHandler) null;
    this._globalNotifyHandler = (NotificationEventHandler) null;
  }

  public string Caption => LocalizationHolder.rm.GetString("PdmConfigurator_72");

  public int ImageIndex => ObjectOptionsEditorView._imgOption;

  public int OrderID => 14;

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    this.DisposeViewServices();
    this._items = items;
    this._services = provider;
  }

  public void Activate(IView previousView)
  {
    this.InitViewServices();
    IViewState service = this._services != null ? this._services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    this.editor.ReadOnly = ((service != null ? (long) service.ViewState : 0L) & 2L) == 2L;
    this.panelBottom.Visible = !this.editor.ReadOnly;
    this.LoadViewData();
  }

  public void Deactivate(IView nextView)
  {
    this.DisposeViewServices();
    if (!this.editor.IsChanged || MessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_73"), LocalizationHolder.rm.GetString("PdmConfigurator_74"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
      return;
    this.DoApply((object) this, (EventArgs) null);
  }

  protected virtual void NotificationEventFired(object sender, NotificationEventArgs e)
  {
  }

  protected virtual void GlobalNotificationEventFired(object sender, NotificationEventArgs e)
  {
    if (!(e.EventName == "ApplicationClosing"))
      return;
    ApplicationClosingEventArgs closingEventArgs = e as ApplicationClosingEventArgs;
    if (!this.editor.IsChanged)
      return;
    int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_73"), LocalizationHolder.rm.GetString("PdmConfigurator_74"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
    if (num == 6)
      this.DoApply((object) this, (EventArgs) null);
    if (num != 2)
      return;
    closingEventArgs.Cancel = true;
  }

  internal void Clear()
  {
    this.editor.Clear();
    this.UpdateControls();
  }

  internal void LoadViewData()
  {
    ObjectOptionsEditor.State state = this.editor.GetState();
    try
    {
      this.Clear();
      if (this._items == null || this._items.Count == 0)
        return;
      IDBTypedObjectID itemData1 = this._items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      IDBRelationID itemData2 = this._items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
      if (itemData1 != null && MetaDataHelper.IsPdmConfigurableObjectType(itemData1.ObjectType))
      {
        IDBTypedObjectID topObjectId = PdmConfiguratorHelper.GetTopObjectID(this._services, this._items);
        this.editor.LoadInfo(this._services, PdmConfiguratorHelper.CreateKey(topObjectId != null ? topObjectId.ObjectID : 0L, topObjectId != null ? topObjectId.ObjectType : -1, itemData2, itemData1), PdmConfiguratorHelper.CreateParentKey(topObjectId != null ? topObjectId.ObjectID : 0L, topObjectId != null ? topObjectId.ObjectType : -1, this._items, 0));
      }
      this.UpdateControls();
    }
    finally
    {
      this.editor.SetState(state);
    }
  }

  internal void UpdateControls()
  {
    this.btnApply.Enabled = this.editor.IsChanged;
    this.btnCancel.Enabled = this.editor.IsChanged;
  }

  private void editor_OnChanged(object sender, EventArgs e) => this.UpdateControls();

  private void DoApply(object sender, EventArgs e)
  {
    if (!this.editor.IsChanged)
      return;
    this.editor.Save();
    this.UpdateControls();
  }

  private void DoCancel(object sender, EventArgs e)
  {
    if (!this.editor.IsChanged)
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ObjectOptionsEditorView));
    this.toolTip = new ToolTip(this.components);
    this.panelBottom = new Panel();
    this.btnCancel = new Button();
    this.btnApply = new Button();
    this.editor = new ObjectOptionsEditor();
    this.panelBottom.SuspendLayout();
    this.SuspendLayout();
    this.panelBottom.BorderStyle = BorderStyle.Fixed3D;
    this.panelBottom.Controls.Add((Control) this.btnCancel);
    this.panelBottom.Controls.Add((Control) this.btnApply);
    componentResourceManager.ApplyResources((object) this.panelBottom, "panelBottom");
    this.panelBottom.Name = "panelBottom";
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Cursor = Cursors.Default;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Click += new EventHandler(this.DoCancel);
    componentResourceManager.ApplyResources((object) this.btnApply, "btnApply");
    this.btnApply.Cursor = Cursors.Default;
    this.btnApply.Name = "btnApply";
    this.btnApply.Click += new EventHandler(this.DoApply);
    this.editor.DisableHeader = false;
    componentResourceManager.ApplyResources((object) this.editor, "editor");
    this.editor.IsChanged = false;
    this.editor.IsInternalChanged = false;
    this.editor.Name = "editor";
    this.editor.OnChanged += new ObjectOptionsEditor.ObjectOptionsChangedEventHandler(this.editor_OnChanged);
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.editor);
    this.Controls.Add((Control) this.panelBottom);
    this.Name = nameof (ObjectOptionsEditorView);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.panelBottom.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private sealed class ObjectOptionsEditorViewDescriptionProvider : BaseViewDescriptionProvider
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
        Caption = LocalizationHolder.rm.GetString("PdmConfigurator_72"),
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgPdmConfigurator.Options") : -1,
        OrderID = 14
      };
    }
  }
}
