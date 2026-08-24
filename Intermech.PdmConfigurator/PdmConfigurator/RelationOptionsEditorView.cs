// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.RelationOptionsEditorView
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator;

[ViewDescriptionProvider(typeof (RelationOptionsEditorView.RelationOptionsEditorViewDescriptionProvider))]
public class RelationOptionsEditorView : UserControl, IView
{
  internal INamedImageList _images;
  internal ICurrentUserAndRole _userAndRole;
  internal ICategoryTypeIconService _categoryImages;
  internal static int _imgOption = -1;
  internal NotificationEventHandler _notifyHandler;
  internal ISelectedItems _items;
  private INotificationService _notifications;
  internal IServiceProvider _services;
  internal RelationPair _key = new RelationPair();
  private IContainer components;
  private ImageList imageList;
  private ToolTip toolTip;
  private Panel panelBottom;
  private Button btnCancel;
  private Button btnApply;
  private Button btnView;
  private ObjectContextEditor contextEditor;
  private Panel panelHint;
  private Label labelWarning;
  private Label labelPicture;

  public RelationOptionsEditorView()
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
    RelationOptionsEditorView._imgOption = RelationOptionsEditorView._imgOption < 0 ? this._images.ImageIndex("imgPdmConfigurator.Options") : RelationOptionsEditorView._imgOption;
  }

  public void DisposeViewResources()
  {
    this._images = (INamedImageList) null;
    this._categoryImages = (ICategoryTypeIconService) null;
    this._items = (ISelectedItems) null;
    this._services = (IServiceProvider) null;
    this._notifications = (INotificationService) null;
  }

  public string Caption
  {
    [DebuggerStepThrough] get => LocalizationHolder.rm.GetString("PdmConfigurator_95");
  }

  public int ImageIndex
  {
    [DebuggerStepThrough] get => RelationOptionsEditorView._imgOption;
  }

  public int OrderID
  {
    [DebuggerStepThrough] get => 14;
  }

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
    this.contextEditor.ReadOnly = ((service != null ? (long) service.ViewState : 0L) & 2L) == 2L;
    this.LoadViewData();
  }

  public void Deactivate(IView nextView)
  {
    if (this._notifications != null && this._notifyHandler != null)
    {
      this._notifications.Unsubscribe(this._notifyHandler);
      this._notifyHandler = (NotificationEventHandler) null;
    }
    if (!this.contextEditor.IsChanged || MessageBox.Show(LocalizationHolder.rm.GetString("PdmConfigurator_73"), LocalizationHolder.rm.GetString("PdmConfigurator_74"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
      return;
    this.DoApply((object) this, (EventArgs) null);
  }

  protected virtual void NotificationEventFired(object sender, NotificationEventArgs e)
  {
  }

  internal void Clear()
  {
    this.contextEditor.Clear();
    this.UpdateControls();
  }

  internal void LoadViewData()
  {
    this.Clear();
    if (this._items == null || this._items.Count == 0)
      return;
    IDBRelationID itemData1 = this._items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    if (this._items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData2 && MetaDataHelper.IsPdmConfigurableObjectType(itemData2.ObjectType))
    {
      IDBTypedObjectID topObjectId = PdmConfiguratorHelper.GetTopObjectID(this._services, this._items);
      this.contextEditor.LoadInfo(this._services, PdmConfiguratorHelper.CreateKey(topObjectId != null ? topObjectId.ObjectID : 0L, topObjectId != null ? topObjectId.ObjectType : -1, itemData1, itemData2), PdmConfiguratorHelper.CreateParentKey(topObjectId != null ? topObjectId.ObjectID : 0L, topObjectId != null ? topObjectId.ObjectType : -1, this._items, 0), (IDBObject) null, (IDBRelation) null);
    }
    this.UpdateControls();
  }

  internal void UpdateControls()
  {
    this.panelHint.Visible = this.contextEditor.AccessRights != PdmContextAccessRights.FullAccess;
    this.btnView.Enabled = this.contextEditor.IsChanged;
    this.btnApply.Enabled = this.contextEditor.IsChanged && (this.contextEditor.AccessRights & PdmContextAccessRights.FullAccess) == PdmContextAccessRights.FullAccess && this.contextEditor.Context.ContextType != PdmContextType.ConfigurableObject;
    this.btnCancel.Enabled = this.contextEditor.IsChanged;
  }

  private void editor_OnChanged(object sender, EventArgs e) => this.UpdateControls();

  private void DoView(object sender, EventArgs e)
  {
    this.contextEditor.Fix();
    PdmConfiguratorContext context = this.contextEditor.Context;
    RelationPair key = context.Key;
    if (context.ContextsCache != null)
      context.ContextsCache[key] = context;
    if (!key.Empty && key.USER_ID != 0L)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService)
        {
          ObjectOptionsHolder objectsOption = context.ObjectsOptions.Count > 0 ? context.ObjectsOptions[0] : (ObjectOptionsHolder) null;
          if (objectsOption != null && objectsOption.Options.Count > 0)
            customService.LoadOptions((object) sessionKeeper.Session.SessionGUID, (IList<long>) objectsOption.Options);
          customService[(object) sessionKeeper.Session.SessionGUID, key] = context;
        }
      }
    }
    if (key.F_PRJLINK_ID != 0L)
      (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) this, (NotificationEventArgs) new DBRelationsEventArgs("RelationsChanged", key.F_PRJLINK_ID));
    else
      (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", key.F_PROJ_ID));
  }

  private void DoApply(object sender, EventArgs e)
  {
    if (!this.contextEditor.IsChanged || this.contextEditor.AccessRights != PdmContextAccessRights.FullAccess || this.contextEditor.Context.ContextType == PdmContextType.ConfigurableObject)
      return;
    PdmConfiguratorContext context = this.contextEditor.Context;
    RelationPair key = context.Key;
    if (key.Empty)
      return;
    try
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IPdmConfiguratorService customService = sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) as IPdmConfiguratorService;
        IDBAttributable dbAttributable = (IDBAttributable) null;
        if (key.F_PRJLINK_ID != 0L)
          dbAttributable = (IDBAttributable) sessionKeeper.Session.GetRelation(key.F_PRJLINK_ID, false);
        if ((dbAttributable == null || !context.SaveToObject(dbAttributable)) && context.ContextsCache != null)
          context.ContextsCache[key] = context;
        if (customService != null)
        {
          ObjectOptionsHolder objectsOption = context.ObjectsOptions.Count > 0 ? context.ObjectsOptions[0] : (ObjectOptionsHolder) null;
          if (objectsOption != null && objectsOption.Options.Count > 0)
            customService.LoadOptions((object) sessionKeeper.Session.SessionGUID, (IList<long>) objectsOption.Options);
          customService[(object) sessionKeeper.Session.SessionGUID, key] = this.contextEditor.Context;
        }
      }
      this.contextEditor.Fix();
    }
    catch
    {
      throw;
    }
    if (key.F_PRJLINK_ID != 0L)
      (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) this, (NotificationEventArgs) new DBRelationsEventArgs("RelationsChanged", key.F_PRJLINK_ID));
    else
      (ServicesManager.GetService(typeof (INotificationService)) as INotificationService).FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", key.F_PROJ_ID));
  }

  private void DoCancel(object sender, EventArgs e)
  {
    if (!this.contextEditor.IsChanged || this.contextEditor.AccessRights != PdmContextAccessRights.FullAccess)
      return;
    this.contextEditor.Undo();
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RelationOptionsEditorView));
    this.imageList = new ImageList(this.components);
    this.toolTip = new ToolTip(this.components);
    this.panelBottom = new Panel();
    this.btnView = new Button();
    this.btnCancel = new Button();
    this.btnApply = new Button();
    this.contextEditor = new ObjectContextEditor();
    this.panelHint = new Panel();
    this.labelWarning = new Label();
    this.labelPicture = new Label();
    this.panelBottom.SuspendLayout();
    this.panelHint.SuspendLayout();
    this.SuspendLayout();
    this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
    this.imageList.TransparentColor = Color.Transparent;
    this.imageList.Images.SetKeyName(0, "warning.png");
    this.panelBottom.BorderStyle = BorderStyle.Fixed3D;
    this.panelBottom.Controls.Add((Control) this.btnView);
    this.panelBottom.Controls.Add((Control) this.btnCancel);
    this.panelBottom.Controls.Add((Control) this.btnApply);
    componentResourceManager.ApplyResources((object) this.panelBottom, "panelBottom");
    this.panelBottom.Name = "panelBottom";
    componentResourceManager.ApplyResources((object) this.btnView, "btnView");
    this.btnView.Cursor = Cursors.Default;
    this.btnView.Name = "btnView";
    this.btnView.Click += new EventHandler(this.DoView);
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Cursor = Cursors.Default;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Click += new EventHandler(this.DoCancel);
    componentResourceManager.ApplyResources((object) this.btnApply, "btnApply");
    this.btnApply.Cursor = Cursors.Default;
    this.btnApply.Name = "btnApply";
    this.btnApply.Click += new EventHandler(this.DoApply);
    componentResourceManager.ApplyResources((object) this.contextEditor, "contextEditor");
    this.contextEditor.IsChanged = false;
    this.contextEditor.IsOptionValueStatus = false;
    this.contextEditor.Name = "contextEditor";
    this.contextEditor.OnChanged += new ObjectContextEditor.ContextChangedEventHandler(this.editor_OnChanged);
    this.panelHint.BorderStyle = BorderStyle.Fixed3D;
    this.panelHint.Controls.Add((Control) this.labelWarning);
    this.panelHint.Controls.Add((Control) this.labelPicture);
    componentResourceManager.ApplyResources((object) this.panelHint, "panelHint");
    this.panelHint.Name = "panelHint";
    componentResourceManager.ApplyResources((object) this.labelWarning, "labelWarning");
    this.labelWarning.Name = "labelWarning";
    componentResourceManager.ApplyResources((object) this.labelPicture, "labelPicture");
    this.labelPicture.ImageList = this.imageList;
    this.labelPicture.Name = "labelPicture";
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.contextEditor);
    this.Controls.Add((Control) this.panelHint);
    this.Controls.Add((Control) this.panelBottom);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (RelationOptionsEditorView);
    this.panelBottom.ResumeLayout(false);
    this.panelHint.ResumeLayout(false);
    this.panelHint.PerformLayout();
    this.ResumeLayout(false);
  }

  private sealed class RelationOptionsEditorViewDescriptionProvider : BaseViewDescriptionProvider
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
        Caption = LocalizationHolder.rm.GetString("PdmConfigurator_95"),
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgPdmConfigurator.Options") : -1,
        OrderID = 14
      };
    }
  }
}
