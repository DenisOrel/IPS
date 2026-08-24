// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.ItemView.GtcProperitesView
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Commands;
using Intermech.DataFormats;
using Intermech.GTC.Client.PropertyGrid;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Notifications;
using Intermech.Navigator.Views;
using Intermech.PropertyEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace Intermech.GTC.Client.ItemView;

public class GtcProperitesView : UserControl, INavigatorView, IView, INodeView
{
  private static Dictionary<ViewStateFlags, int> _splitWidth = new Dictionary<ViewStateFlags, int>();
  private static Dictionary<ViewStateFlags, PropertySort> _propertySort = new Dictionary<ViewStateFlags, PropertySort>();
  private int _imageIndex;
  protected INode ParentNode;
  protected INodeID NodeId;
  protected int ObjTypeId;
  protected long ObjId;
  protected long ProjId;
  protected long PrjLinkId;
  protected INotificationService NotificationService;
  protected INotificationService GlobalNotificationService;
  protected NotificationEventHandler NotifyHandler;
  protected NotificationEventHandler GlobalNotifyHandler;
  protected EventHandler<BeforeObjectCommandArgs> CommandsBeforeCheckInHandler;
  protected IServiceProvider Services;
  protected IViewState ViewState;
  protected bool Reinitialize;
  protected bool FirstInitialized;
  protected const GetAttributeValuesModes GridMode = GetAttributeValuesModes.IncludeName | GetAttributeValuesModes.IncludeGroupName | GetAttributeValuesModes.CheckWriteAccess | GetAttributeValuesModes.IncludeDescriptions | GetAttributeValuesModes.CheckVisibility;
  protected static readonly Type[] TabTypes = new Type[1]
  {
    typeof (Intermech.GTC.Client.PropertyGrid.ObjectAllAttributesGridTab)
  };
  private IContainer components;
  protected Panel pnButtons;
  protected Button btCancel;
  protected Button btApply;
  private Panel panel1;
  private GtcPropertyGrid gtcPropertyGrid;

  public virtual GtcPropertyGrid PropertyGrid
  {
    [DebuggerStepThrough] get => this.gtcPropertyGrid;
  }

  static GtcProperitesView()
  {
    GtcProperitesView._splitWidth.Add(ViewStateFlags.InDialog, 0);
    GtcProperitesView._splitWidth.Add(ViewStateFlags.InParametersCard, 0);
    GtcProperitesView._splitWidth.Add(ViewStateFlags.NodeInTree, 0);
    GtcProperitesView._splitWidth.Add(ViewStateFlags.NodeInViews, 0);
    GtcProperitesView._splitWidth.Add(ViewStateFlags.NodeUnderTree, 0);
    GtcProperitesView._propertySort.Add(ViewStateFlags.InDialog, PropertySort.CategorizedAlphabetical);
    GtcProperitesView._propertySort.Add(ViewStateFlags.InParametersCard, PropertySort.CategorizedAlphabetical);
    GtcProperitesView._propertySort.Add(ViewStateFlags.NodeInTree, PropertySort.CategorizedAlphabetical);
    GtcProperitesView._propertySort.Add(ViewStateFlags.NodeInViews, PropertySort.CategorizedAlphabetical);
    GtcProperitesView._propertySort.Add(ViewStateFlags.NodeUnderTree, PropertySort.Alphabetical);
  }

  public GtcProperitesView()
  {
    this.InitializeComponent();
    this.Reinitialize = false;
    this.InitResources();
  }

  protected virtual void InitResources() => this._imageIndex = -1;

  protected virtual void ReleaseResources()
  {
  }

  protected virtual void InitServices(IServiceProvider services)
  {
    if (this.CommandsBeforeCheckInHandler == null)
    {
      this.CommandsBeforeCheckInHandler = new EventHandler<BeforeObjectCommandArgs>(this.CommandsBeforeCheckIn);
      ObjectCommandEvents.Checkin.Before += this.CommandsBeforeCheckInHandler;
    }
    if (services != null)
    {
      if (this.NotificationService != null && this.NotifyHandler != null)
      {
        this.NotificationService.Unsubscribe(this.NotifyHandler);
        this.NotifyHandler = (NotificationEventHandler) null;
      }
      this.NotificationService = services.GetService(typeof (INotificationService)) as INotificationService;
      this.ViewState = services.GetService(typeof (IViewState)) as IViewState;
    }
    else
    {
      this.NotificationService = (INotificationService) null;
      this.ViewState = (IViewState) null;
    }
    if (this.NotifyHandler == null && this.NotificationService != null)
    {
      this.NotifyHandler = new NotificationEventHandler(this.NotificationEventFired);
      this.NotificationService.Subscribe(this.NotifyHandler);
    }
    if (this.GlobalNotificationService != null)
      return;
    this.GlobalNotificationService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    if (this.GlobalNotifyHandler != null || this.GlobalNotificationService == null)
      return;
    this.GlobalNotifyHandler = new NotificationEventHandler(this.GlobalNotificationEventFired);
    this.GlobalNotificationService.Subscribe(this.GlobalNotifyHandler);
  }

  protected virtual void ReleaseServices()
  {
    if (this.CommandsBeforeCheckInHandler != null)
    {
      ObjectCommandEvents.Checkin.Before -= this.CommandsBeforeCheckInHandler;
      this.CommandsBeforeCheckInHandler = (EventHandler<BeforeObjectCommandArgs>) null;
    }
    if (this.GlobalNotificationService == null)
      return;
    if (this.NotifyHandler != null && this.NotificationService != null)
      this.NotificationService.Unsubscribe(this.NotifyHandler);
    if (this.GlobalNotifyHandler != null && this.GlobalNotificationService != null)
      this.GlobalNotificationService.Unsubscribe(this.GlobalNotifyHandler);
    this.GlobalNotificationService = (INotificationService) null;
    this.NotifyHandler = (NotificationEventHandler) null;
    this.GlobalNotifyHandler = (NotificationEventHandler) null;
    this.ViewState = (IViewState) null;
  }

  public void Initialize(long objId, int objType, long relId, IServiceProvider services)
  {
    this.Services = services;
    this.InitServices(this.Services);
    this.ParentNode = (INode) null;
    this.NodeId = (INodeID) null;
    this.ObjId = objId;
    this.ObjTypeId = objType;
    this.PrjLinkId = relId;
    this.Reinitialize = true;
    this.UpdateControls();
  }

  public virtual void Initialize(ISelectedItems items, IServiceProvider services)
  {
    this.Services = services;
    if (!this.FirstInitialized)
    {
      NavigatorViewOptions service = this.Services != null ? this.Services.GetService(typeof (NavigatorViewOptions)) as NavigatorViewOptions : (NavigatorViewOptions) null;
      this.gtcPropertyGrid.HelpVisible = service == null || service.Context == NavigatorViewContext.MainViews;
      this.gtcPropertyGrid.PropertySort = !this.gtcPropertyGrid.HelpVisible ? PropertySort.Alphabetical : PropertySort.CategorizedAlphabetical;
      this.FirstInitialized = true;
    }
    this.ParentNode = items.GetItemData(0, typeof (INode)) as INode;
    this.NodeId = items.GetItemID(0);
    this.ProjId = items.GetParentData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID parentData ? parentData.ObjectID : 0L;
    this.GetDataFromNodeId();
    this.InitServices(this.Services);
    this.Reinitialize = true;
  }

  public void Activate(IView previousView)
  {
    if (previousView == PageViewsManager.BlackHoleView || !this.Reinitialize)
      return;
    this.LoadData();
    this.Reinitialize = false;
  }

  public void Deactivate(IView nextView)
  {
    this.SaveIfModified();
    this.Reinitialize = true;
  }

  public string Caption => ServiceHolder.Rm.GetString("GTC_6");

  public int ImageIndex
  {
    get
    {
      if (this._imageIndex < 0)
        this._imageIndex = ServiceHolder.NamedImageList.ImageIndex(Const.IconName);
      return this._imageIndex;
    }
  }

  public int OrderID
  {
    [DebuggerStepThrough] get => 1;
  }

  protected virtual void CommandsBeforeCheckIn(object sender, BeforeObjectCommandArgs e)
  {
    if (e.ObjectId != this.ObjId || !this.PropertyGrid.Visible)
      return;
    this.SaveData();
  }

  protected virtual void NotificationEventFired(object sender, NotificationEventArgs e)
  {
    if (sender == this || this.ParentNode == null)
      return;
    IUpdateAnalyser analyser = this.ParentNode.GetAnalyser(this.Capabilities, sender, e);
    if (analyser == null)
      return;
    UpdateManager.UpdateView((INodeView) this, analyser);
  }

  protected virtual void GlobalNotificationEventFired(object sender, NotificationEventArgs e)
  {
    if (!(e.EventName == "ApplicationClosing"))
      return;
    ApplicationClosingEventArgs closingEventArgs = e as ApplicationClosingEventArgs;
    if (!this.PropertyGrid.IsChanged)
      return;
    int num = (int) MessageBox.Show(ServiceHolder.Rm.GetString("GTC_13"), ServiceHolder.Rm.GetString("GTC_14"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
    if (num == 6)
      this.SaveData();
    if (num != 2)
      return;
    closingEventArgs.Cancel = true;
  }

  private void btCancel_Click(object sender, EventArgs e) => this.ViewCancelClick(sender, e);

  private void btApply_Click(object sender, EventArgs e) => this.ViewApplyClick(sender, e);

  private void gtcPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
  {
    this.ViewPropertyValueChanged(s, e);
  }

  private void gtcPropertyGrid_GridChanged(object sender, GridChangedEventArgs e)
  {
    this.ViewGridChanged(sender, e);
  }

  protected virtual void ViewCancelClick(object sender, EventArgs e)
  {
    if (!this.PropertyGrid.Visible)
      return;
    this.LoadData();
  }

  protected virtual void ViewApplyClick(object sender, EventArgs e)
  {
    if (!this.PropertyGrid.Visible)
      return;
    this.SaveData();
  }

  protected virtual void ViewPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
  {
    this.UpdateControls();
  }

  protected virtual void ViewGridChanged(object sender, GridChangedEventArgs e)
  {
    if (!e.ApplyNeeded)
      return;
    this.UpdateControls();
  }

  protected virtual void GetDataFromNodeId()
  {
    IDBTypedObjectID data1 = (IDBTypedObjectID) this.ParentNode.GetData(this.NodeId, typeof (IDBTypedObjectID));
    IDBRelationID data2 = (IDBRelationID) this.ParentNode.GetData(this.NodeId, typeof (IDBRelationID));
    this.ObjId = data1.ObjectID;
    this.ObjTypeId = data1.ObjectType;
    this.PrjLinkId = data2 == null ? -1L : data2.Value;
  }

  protected virtual void SaveIfModified()
  {
    if (!this.PropertyGrid.IsChanged)
      return;
    if (MessageBox.Show(ServiceHolder.Rm.GetString("GTC_13"), ServiceHolder.Rm.GetString("GTC_14")) == DialogResult.Yes)
      this.SaveData();
    else
      this.LoadData();
  }

  protected virtual void LoadData()
  {
    Control parent = this.PropertyGrid.Parent;
    try
    {
      this.PropertyGrid.Parent = (Control) null;
      this.PropertyGrid.Load(this.ObjId, AttributableElements.Object, GetAttributeValuesModes.IncludeName | GetAttributeValuesModes.IncludeGroupName | GetAttributeValuesModes.CheckWriteAccess | GetAttributeValuesModes.IncludeDescriptions | GetAttributeValuesModes.CheckVisibility, false, GtcProperitesView.TabTypes);
    }
    finally
    {
      this.PropertyGrid.Parent = parent;
    }
    this.UpdateControls();
  }

  protected virtual void SaveData()
  {
    if (this.PropertyGrid.IsChanged)
      this.PropertyGrid.Save();
    this.UpdateControls();
  }

  protected virtual void UpdateControls()
  {
    this.btApply.Enabled = this.PropertyGrid.IsChanged;
    this.btCancel.Enabled = this.btApply.Enabled;
    bool isReadOnly = this.IsReadOnly;
    if (this.pnButtons.Visible == isReadOnly)
      this.pnButtons.Visible = !isReadOnly;
    if (!isReadOnly)
      return;
    this.ForceGridToReadOnly();
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool IsReadOnly
  {
    [DebuggerStepThrough] get
    {
      return this.ViewState != null && (this.ViewState.ViewState & ViewStateFlags.ReadOnly) == ViewStateFlags.ReadOnly;
    }
  }

  protected void ForceGridToReadOnly()
  {
    if (this.gtcPropertyGrid == null || this.gtcPropertyGrid.PropertyDescriptorHolder == null || this.gtcPropertyGrid.PropertyDescriptorHolder.PropertyDescriptorList == null)
      return;
    foreach (PropDescriptor propertyDescriptor in this.gtcPropertyGrid.PropertyDescriptorHolder.PropertyDescriptorList)
    {
      if (propertyDescriptor != null)
      {
        if (!propertyDescriptor.IsReadOnly)
          propertyDescriptor.SetReadOnly(true);
        if (propertyDescriptor.Editor != null)
          propertyDescriptor.Editor = (object) null;
      }
    }
    int count1 = this.gtcPropertyGrid.PropertyDescriptorHolder.PropertyDescriptorList.Count;
    this.gtcPropertyGrid.Refresh();
    int count2 = this.gtcPropertyGrid.PropertyDescriptorHolder.PropertyDescriptorList.Count;
    if (count1 == count2)
      return;
    foreach (PropDescriptor propertyDescriptor in this.gtcPropertyGrid.PropertyDescriptorHolder.PropertyDescriptorList)
    {
      if (propertyDescriptor != null && !propertyDescriptor.IsReadOnly)
      {
        if (!propertyDescriptor.IsReadOnly)
          propertyDescriptor.SetReadOnly(true);
        if (propertyDescriptor.Editor != null)
          propertyDescriptor.Editor = (object) null;
      }
    }
    this.gtcPropertyGrid.Refresh();
  }

  public virtual NodeViewCapabilities Capabilities
  {
    get
    {
      ContentType contentType = ContentType.None;
      if (this.ParentNode != null && this.NodeId != null)
        contentType = (this.ParentNode.GetAttributesOf(this.NodeId) & ContentAttributes.Folder) != ContentAttributes.None ? ContentType.Folders : ContentType.NonFolders;
      return new NodeViewCapabilities(contentType, (NodeColumnCollection) null, false);
    }
  }

  public int Count
  {
    [DebuggerStepThrough] get => 1;
  }

  public INodeID this[int index]
  {
    [DebuggerStepThrough] get => this.NodeId;
  }

  public void Append(NodeIDCollection partialNodeIDs)
  {
  }

  public void Update(IList indexes)
  {
    this.Deactivate((IView) null);
    this.Reinitialize = true;
    this.Activate((IView) null);
  }

  public void Replace(IList indexes, NodeIDCollection replacementNodeIDs)
  {
    bool reinitialize = this.Reinitialize;
    try
    {
      if (!this.Reinitialize)
        this.Deactivate((IView) null);
      this.NodeId = replacementNodeIDs[0];
      this.GetDataFromNodeId();
    }
    finally
    {
      if (!reinitialize)
        this.Activate((IView) null);
    }
  }

  public void Remove(IList indexes)
  {
  }

  protected virtual ViewStateFlags CurrentState
  {
    get
    {
      if (this.ViewState == null)
        return ViewStateFlags.NodeInViews;
      if ((this.ViewState.ViewState & ViewStateFlags.NodeUnderTree) == ViewStateFlags.NodeUnderTree)
        return ViewStateFlags.NodeUnderTree;
      if ((this.ViewState.ViewState & ViewStateFlags.InDialog) == ViewStateFlags.InDialog)
        return ViewStateFlags.InDialog;
      if ((this.ViewState.ViewState & ViewStateFlags.InParametersCard) == ViewStateFlags.InParametersCard)
        return ViewStateFlags.InParametersCard;
      return (this.ViewState.ViewState & ViewStateFlags.NodeInTree) == ViewStateFlags.NodeInTree ? ViewStateFlags.NodeInTree : ViewStateFlags.NodeInViews;
    }
  }

  protected virtual void SaveGridSplitterPos()
  {
    GtcProperitesView._propertySort[this.CurrentState] = this.gtcPropertyGrid.PropertySort;
    try
    {
      object target = this.gtcPropertyGrid.GetType().BaseType.GetField("gridView", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic).GetValue((object) this.gtcPropertyGrid);
      int num = (int) target.GetType().InvokeMember("GetLabelWidth", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder) null, target, new object[0]);
      if (num <= 0)
        return;
      GtcProperitesView._splitWidth[this.CurrentState] = num;
    }
    catch
    {
    }
  }

  protected virtual void RestoreGridSplitterPos()
  {
    if (GtcProperitesView._splitWidth[this.CurrentState] == 0)
      return;
    this.gtcPropertyGrid.PropertySort = GtcProperitesView._propertySort[this.CurrentState];
    try
    {
      object target = this.gtcPropertyGrid.GetType().BaseType.GetField("gridView", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic).GetValue((object) this.gtcPropertyGrid);
      Type type = target.GetType();
      int num = GtcProperitesView._splitWidth[this.CurrentState];
      type.InvokeMember("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, target, new object[1]
      {
        (object) num
      });
      type.InvokeMember("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, target, new object[1]
      {
        (object) num
      });
    }
    catch
    {
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.pnButtons = new Panel();
    this.btCancel = new Button();
    this.btApply = new Button();
    this.panel1 = new Panel();
    this.gtcPropertyGrid = new GtcPropertyGrid();
    this.pnButtons.SuspendLayout();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this.pnButtons.Controls.Add((Control) this.btCancel);
    this.pnButtons.Controls.Add((Control) this.btApply);
    this.pnButtons.Dock = DockStyle.Bottom;
    this.pnButtons.Font = new Font("Tahoma", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.pnButtons.Location = new Point(2, 283);
    this.pnButtons.Name = "pnButtons";
    this.pnButtons.Size = new Size(531, 40);
    this.pnButtons.TabIndex = 5;
    this.btCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btCancel.Enabled = false;
    this.btCancel.FlatStyle = FlatStyle.System;
    this.btCancel.ImeMode = ImeMode.NoControl;
    this.btCancel.Location = new Point(402, 7);
    this.btCancel.Name = "btCancel";
    this.btCancel.Size = new Size(121, 27);
    this.btCancel.TabIndex = 1;
    this.btCancel.Text = "Отмена";
    this.btCancel.Click += new EventHandler(this.btCancel_Click);
    this.btApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btApply.Enabled = false;
    this.btApply.FlatStyle = FlatStyle.System;
    this.btApply.ImeMode = ImeMode.NoControl;
    this.btApply.Location = new Point(275, 6);
    this.btApply.Name = "btApply";
    this.btApply.Size = new Size(121, 27);
    this.btApply.TabIndex = 0;
    this.btApply.Text = "Применить";
    this.btApply.Click += new EventHandler(this.btApply_Click);
    this.panel1.Controls.Add((Control) this.gtcPropertyGrid);
    this.panel1.Dock = DockStyle.Fill;
    this.panel1.Location = new Point(2, 2);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(531, 281);
    this.panel1.TabIndex = 7;
    this.gtcPropertyGrid.CommandsActiveLinkColor = SystemColors.ActiveCaption;
    this.gtcPropertyGrid.CommandsDisabledLinkColor = SystemColors.ControlDark;
    this.gtcPropertyGrid.CommandsLinkColor = SystemColors.ActiveCaption;
    this.gtcPropertyGrid.Dock = DockStyle.Fill;
    this.gtcPropertyGrid.InternalMenuEnabled = true;
    this.gtcPropertyGrid.LineColor = SystemColors.ScrollBar;
    this.gtcPropertyGrid.Location = new Point(0, 0);
    this.gtcPropertyGrid.LockTypeChange = false;
    this.gtcPropertyGrid.Name = "gtcPropertyGrid";
    this.gtcPropertyGrid.PropertySort = PropertySort.Alphabetical;
    this.gtcPropertyGrid.Size = new Size(531, 281);
    this.gtcPropertyGrid.TabIndex = 7;
    this.gtcPropertyGrid.GridChanged += new GtcPropertyGrid.GridChangedDelegate(this.gtcPropertyGrid_GridChanged);
    this.gtcPropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.gtcPropertyGrid_PropertyValueChanged);
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.pnButtons);
    this.Name = nameof (GtcProperitesView);
    this.Padding = new Padding(2);
    this.Size = new Size(535, 325);
    this.pnButtons.ResumeLayout(false);
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
