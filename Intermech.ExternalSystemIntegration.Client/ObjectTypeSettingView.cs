// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ObjectTypeSettingView
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.ExternalSystemIntegration.Client.ObjectTypeSetting;
using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Controls;
using Intermech.Navigator.CustomNode;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

[ViewDescriptionProvider(typeof (ObjectTypeSettingView.ObjectTypeSettingViewDescriptionProvider))]
public class ObjectTypeSettingView : UserControl, IView
{
  internal ISelectedItems _items;
  internal IServiceProvider _provider;
  private ServiceContainer _services;
  private IContainer components;
  private ButtonedEdit edObjectType;
  private TabControl tabControlConfigs;
  private TabPage tabPageRequestConfigs;
  private CustomObjectsListView RequestConfigView;
  private TabPage tabPageResponceConfigs;
  private CustomObjectsListView ResponceConfigView;

  public ObjectTypeSettingView() => this.InitializeComponent();

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    this._items = items;
    this._provider = provider;
    this._services = (ServiceContainer) new AdvancedServiceContainer(provider);
    this._services.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.None));
  }

  public void Activate(IView previousView)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this._items == null || this._items.Count == 0 || !(this._items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || !(sessionKeeper.Session.GetObject(itemData.ObjectID, true) is IObjTypeSettingItemObject settingItemObject))
        return;
      this.edObjectType.Value = sessionKeeper.Session.GetObjectType(new Guid(settingItemObject.ObjTypeGUID)).ObjectTypeName;
      this.edObjectType.Image = ServiceHolder.CategoryTypeIconService.ImageList.Images[ServiceHolder.CategoryTypeIconService.IndexOf(4, MetaDataHelper.GetObjectTypeID(new Guid(settingItemObject.ObjTypeGUID)))];
      this.RequestConfigView.Initialize((IDescriptor) ObjectsSelectionDescriptor.CreateFromValue<string>(Const.RequestConfigObjTypeID, "Конфигурации исходящих запросов", Const.LinkObjectAttrTypeID, settingItemObject.LinkObjGuid), (IServiceProvider) this._services);
      this.RequestConfigView.Activate((IView) null);
      this.ResponceConfigView.Initialize((IDescriptor) ObjectsSelectionDescriptor.CreateFromValue<string>(Const.ResponceConfigObjTypeID, "Конфигурации входящих запросов", Const.LinkObjectAttrTypeID, settingItemObject.LinkObjGuid), (IServiceProvider) this._services);
      this.ResponceConfigView.Activate((IView) null);
    }
  }

  public void Deactivate(IView nextView)
  {
    this.RequestConfigView.Deactivate(nextView);
    this.ResponceConfigView.Deactivate(nextView);
  }

  public string Caption => Const.ObjectTypeSettingItemTabName;

  public int ImageIndex
  {
    get => ServiceHolder.NamedImageList.ImageIndex(Const.ObjectTypeSettingItemIconName);
  }

  public int OrderID => 1;

  private void RequestConfigView_AddButtonClick(object sender, EventArgs e)
  {
    long num1 = 0;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this._items == null || this._items.Count == 0 || !(this._items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
        return;
      if (sessionKeeper.Session.GetObject(itemData.ObjectID, false) is IObjTypeSettingItemObject settingItemObject)
      {
        if (sessionKeeper.Session.GetObjectCollection(Const.RequestConfigObjTypeID).Create() is IRequestConfigObject requestConfigObject)
        {
          num1 = requestConfigObject.ObjectID;
          requestConfigObject.LinkObjGuid = settingItemObject.LinkObjGuid;
        }
      }
    }
    if (num1 == 0L)
      return;
    int num2 = (int) PropertiesWindow.Execute(string.Empty, string.Empty, num1, "RequestConfigPage");
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetObject(num1, true) is IRequestConfigObject requestConfigObject))
        return;
      if (requestConfigObject.SchemeTransfLink != 0L)
      {
        requestConfigObject.CommitCreation(true);
        ServiceHolder.NotificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", requestConfigObject.ObjectID));
      }
      else
      {
        int num3 = (int) MessageBox.Show("Не указана схема трансформации. Созданная конфигурация будет удалена!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        requestConfigObject.Delete(0L);
      }
    }
  }

  private void ResponceConfigView_AddButtonClick(object sender, EventArgs e)
  {
    long num1 = 0;
    if (this._items == null || this._items.Count == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this._items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData)
      {
        if (sessionKeeper.Session.GetObject(itemData.ObjectID, true) is IObjTypeSettingItemObject settingItemObject)
        {
          if (sessionKeeper.Session.GetObjectCollection(Const.ResponceConfigObjTypeID).Create() is IResponceConfigObject responceConfigObject)
          {
            num1 = responceConfigObject.ObjectID;
            responceConfigObject.LinkObjGuid = settingItemObject.LinkObjGuid;
          }
        }
      }
    }
    if (num1 == 0L)
      return;
    int num2 = (int) PropertiesWindow.Execute(string.Empty, string.Empty, num1, "ResponceConfigPage");
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetObject(num1, true) is IResponceConfigObject responceConfigObject))
        return;
      if (responceConfigObject.SchemeTransfLink != 0L)
      {
        responceConfigObject.CommitCreation(true);
        ServiceHolder.NotificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", responceConfigObject.ObjectID));
      }
      else
      {
        int num3 = (int) MessageBox.Show("Не указана схема трансформации. Созданная конфигурация будет удалена!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        responceConfigObject.Delete(0L);
      }
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
      this._services?.Dispose();
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.edObjectType = new ButtonedEdit();
    this.tabControlConfigs = new TabControl();
    this.tabPageRequestConfigs = new TabPage();
    this.RequestConfigView = new CustomObjectsListView();
    this.tabPageResponceConfigs = new TabPage();
    this.ResponceConfigView = new CustomObjectsListView();
    this.tabControlConfigs.SuspendLayout();
    this.tabPageRequestConfigs.SuspendLayout();
    this.tabPageResponceConfigs.SuspendLayout();
    this.SuspendLayout();
    this.edObjectType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.edObjectType.ButtonImage = (Image) null;
    this.edObjectType.ButtonText = "...";
    this.edObjectType.Caption = "Тип объекта:";
    this.edObjectType.CaptionFont = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.edObjectType.Image = (Image) null;
    this.edObjectType.Location = new Point(10, 10);
    this.edObjectType.MinimumSize = new Size(40, 20);
    this.edObjectType.Name = "edObjectType";
    this.edObjectType.ReadOnly = true;
    this.edObjectType.ShowButton = false;
    this.edObjectType.Size = new Size(580, 40);
    this.edObjectType.TabIndex = 3;
    this.tabControlConfigs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tabControlConfigs.Controls.Add((Control) this.tabPageRequestConfigs);
    this.tabControlConfigs.Controls.Add((Control) this.tabPageResponceConfigs);
    this.tabControlConfigs.Location = new Point(10, 58);
    this.tabControlConfigs.Name = "tabControlConfigs";
    this.tabControlConfigs.SelectedIndex = 0;
    this.tabControlConfigs.Size = new Size(580, 329);
    this.tabControlConfigs.TabIndex = 7;
    this.tabPageRequestConfigs.Controls.Add((Control) this.RequestConfigView);
    this.tabPageRequestConfigs.Location = new Point(4, 22);
    this.tabPageRequestConfigs.Name = "tabPageRequestConfigs";
    this.tabPageRequestConfigs.Padding = new Padding(3);
    this.tabPageRequestConfigs.Size = new Size(572, 303);
    this.tabPageRequestConfigs.TabIndex = 0;
    this.tabPageRequestConfigs.Text = "Конфигурации исходящих запросов";
    this.tabPageRequestConfigs.UseVisualStyleBackColor = true;
    this.RequestConfigView.AllowCustomGroupValues = true;
    this.RequestConfigView.Control = (object) this.RequestConfigView;
    this.RequestConfigView.DisableColumnsGrouping = true;
    this.RequestConfigView.DisableColumnsSettings = true;
    this.RequestConfigView.DisableContextSearch = true;
    this.RequestConfigView.DisableFiltration = true;
    this.RequestConfigView.DisableGroupBox = true;
    this.RequestConfigView.DisableHeaderContextMenu = true;
    this.RequestConfigView.DisableKeyDownEvents = false;
    this.RequestConfigView.Dock = DockStyle.Fill;
    this.RequestConfigView.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.RequestConfigView.Font = new Font("Tahoma", 8.25f);
    this.RequestConfigView.Location = new Point(3, 3);
    this.RequestConfigView.Name = "RequestConfigView";
    this.RequestConfigView.Size = new Size(566, 297);
    this.RequestConfigView.TabIndex = 6;
    this.RequestConfigView.AddButtonClick += new EventHandler(this.RequestConfigView_AddButtonClick);
    this.tabPageResponceConfigs.Controls.Add((Control) this.ResponceConfigView);
    this.tabPageResponceConfigs.Location = new Point(4, 22);
    this.tabPageResponceConfigs.Name = "tabPageResponceConfigs";
    this.tabPageResponceConfigs.Padding = new Padding(3);
    this.tabPageResponceConfigs.Size = new Size(572, 303);
    this.tabPageResponceConfigs.TabIndex = 1;
    this.tabPageResponceConfigs.Text = "Конфигурации входящих запросов";
    this.tabPageResponceConfigs.UseVisualStyleBackColor = true;
    this.ResponceConfigView.AllowCustomGroupValues = true;
    this.ResponceConfigView.Control = (object) this.ResponceConfigView;
    this.ResponceConfigView.DisableColumnsGrouping = true;
    this.ResponceConfigView.DisableColumnsSettings = true;
    this.ResponceConfigView.DisableContextSearch = true;
    this.ResponceConfigView.DisableFiltration = true;
    this.ResponceConfigView.DisableGroupBox = true;
    this.ResponceConfigView.DisableHeaderContextMenu = true;
    this.ResponceConfigView.DisableKeyDownEvents = false;
    this.ResponceConfigView.Dock = DockStyle.Fill;
    this.ResponceConfigView.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.ResponceConfigView.Font = new Font("Tahoma", 8.25f);
    this.ResponceConfigView.Location = new Point(3, 3);
    this.ResponceConfigView.Name = "ResponceConfigView";
    this.ResponceConfigView.Size = new Size(566, 297);
    this.ResponceConfigView.TabIndex = 7;
    this.ResponceConfigView.AddButtonClick += new EventHandler(this.ResponceConfigView_AddButtonClick);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tabControlConfigs);
    this.Controls.Add((Control) this.edObjectType);
    this.Name = nameof (ObjectTypeSettingView);
    this.Size = new Size(600, 400);
    this.tabControlConfigs.ResumeLayout(false);
    this.tabPageRequestConfigs.ResumeLayout(false);
    this.tabPageResponceConfigs.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private sealed class ObjectTypeSettingViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      return new ViewDescription()
      {
        Caption = Const.ObjectTypeSettingItemTabName,
        ImageIndex = ServiceHolder.NamedImageList.ImageIndex(Const.ObjectTypeSettingItemIconName),
        OrderID = 1
      };
    }
  }
}
