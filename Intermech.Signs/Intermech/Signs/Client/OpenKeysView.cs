// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.OpenKeysView
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

[ViewDescriptionProvider(typeof (OpenKeysView.OpenKeysViewDescriptionProvider))]
internal class OpenKeysView : UserControl, IView
{
  private Panel panel1;
  private Panel panel2;
  private Button _bApply;
  private Button _bCancel;
  private PropertyGrid _Grid;
  private Button _bAddKey;
  private Button _bRemoveKey;
  private GridItem _selItem;
  private ContextMenu _сMenu;
  private MenuItem _cAdd;
  private MenuItem _cRemove;
  private INotificationService _notificationService;
  private bool _modified;
  private long _objectID;
  private bool _first;
  private bool _activeView;
  private OpenKeysCollection _keys = new OpenKeysCollection();

  private bool Modified
  {
    get => this._modified;
    set
    {
      this._modified = value;
      this.panel2.Enabled = value;
    }
  }

  private object MainObject
  {
    set
    {
      this._Grid.SelectedObject = (object) null;
      this._selItem = (GridItem) null;
      this._Grid.SelectedObject = value;
      this._selItem = this._Grid.SelectedGridItem;
      this._сMenu_Popup((object) null, (EventArgs) null);
    }
  }

  public OpenKeysView()
  {
    this.InitializeComponent();
    this._notificationService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    this._notificationService.Subscribe("ObjectsChanged", new NotificationEventHandler(this.NotifyEvent));
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
      this._notificationService.Unsubscribe("ObjectsChanged", new NotificationEventHandler(this.NotifyEvent));
    base.Dispose(disposing);
  }

  private void NotifyEvent(object sender, NotificationEventArgs e)
  {
    if (sender != null && sender.Equals((object) this))
      return;
    DBObjectsEventArgs objectsEventArgs = e as DBObjectsEventArgs;
    if (!(e.EventName == "ObjectsChanged") || objectsEventArgs == null || !objectsEventArgs.ObjectIDs.Contains(this._objectID))
      return;
    this._first = true;
    if (!this._activeView)
      return;
    this.Activate((IView) null);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OpenKeysView));
    this.panel1 = new Panel();
    this._bRemoveKey = new Button();
    this._bAddKey = new Button();
    this.panel2 = new Panel();
    this._bCancel = new Button();
    this._bApply = new Button();
    this._Grid = new PropertyGrid();
    this._сMenu = new ContextMenu();
    this._cAdd = new MenuItem();
    this._cRemove = new MenuItem();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this._bRemoveKey);
    this.panel1.Controls.Add((Control) this._bAddKey);
    this.panel1.Controls.Add((Control) this.panel2);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this._bRemoveKey, "_bRemoveKey");
    this._bRemoveKey.Name = "_bRemoveKey";
    this._bRemoveKey.Click += new EventHandler(this._bRemoveKey_Click);
    componentResourceManager.ApplyResources((object) this._bAddKey, "_bAddKey");
    this._bAddKey.Name = "_bAddKey";
    this._bAddKey.Click += new EventHandler(this._bAddKey_Click);
    this.panel2.Controls.Add((Control) this._bCancel);
    this.panel2.Controls.Add((Control) this._bApply);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this._bCancel, "_bCancel");
    this._bCancel.Name = "_bCancel";
    this._bCancel.Click += new EventHandler(this._bCancel_Click);
    componentResourceManager.ApplyResources((object) this._bApply, "_bApply");
    this._bApply.Name = "_bApply";
    this._bApply.Click += new EventHandler(this._bApply_Click);
    this._Grid.ContextMenu = this._сMenu;
    componentResourceManager.ApplyResources((object) this._Grid, "_Grid");
    this._Grid.LineColor = SystemColors.ScrollBar;
    this._Grid.Name = "_Grid";
    this._Grid.ToolbarVisible = false;
    this._Grid.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(this._Grid_SelectedGridItemChanged);
    this._сMenu.MenuItems.AddRange(new MenuItem[2]
    {
      this._cAdd,
      this._cRemove
    });
    this._сMenu.Popup += new EventHandler(this._сMenu_Popup);
    componentResourceManager.ApplyResources((object) this._cAdd, "_cAdd");
    this._cAdd.Index = 0;
    this._cAdd.Click += new EventHandler(this._bAddKey_Click);
    this._cRemove.Index = 1;
    componentResourceManager.ApplyResources((object) this._cRemove, "_cRemove");
    this._cRemove.Click += new EventHandler(this._bRemoveKey_Click);
    this.Controls.Add((Control) this._Grid);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (OpenKeysView);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Tag = (object) "     ";
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  public int ImageIndex => -1;

  public int OrderID => 21;

  public string Caption => LocalizationHolder.rm.GetString("Signs_50");

  public void Initialize(ISelectedItems items, IServiceProvider services)
  {
    this._objectID = (items.GetItemData(0, typeof (IDBObjectID)) as IDBObjectID).Value;
    this._first = true;
  }

  public void Deactivate(IView nextView)
  {
    this._activeView = false;
    if (!this.Modified)
      return;
    if (MessageBox.Show(LocalizationHolder.rm.GetString("Signs_51"), LocalizationHolder.rm.GetString("Signs_52"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals((object) DialogResult.Yes))
      this._bApply_Click((object) null, (EventArgs) null);
    else
      this._bCancel_Click((object) null, (EventArgs) null);
  }

  public void Activate(IView previousView)
  {
    this._activeView = true;
    if (!this._first)
      return;
    this._first = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._objectID);
      IDBAttribute dbAttribute = dbObject.GetAttributeByID(SignsHolder.OpenKeysAttrTypeID) ?? dbObject.Attributes.AddAttribute(SignsHolder.OpenKeysAttrTypeID, false);
      this._keys = new OpenKeysCollection();
      foreach (object obj in dbAttribute.Values)
      {
        OpenKey openKey = (OpenKey) null;
        try
        {
          openKey = new OpenKey(Convert.ToString(obj));
        }
        catch
        {
        }
        if (openKey != null)
          this._keys.Add((object) openKey);
      }
      this.MainObject = (object) new OpenKeyClassWrapper(this._keys.Values);
      this._сMenu_Popup((object) null, (EventArgs) null);
    }
    this.Modified = false;
  }

  private void _bApply_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._objectID);
      AttributeValues attributeValues = new AttributeValues(SignsHolder.OpenKeysAttrTypeID);
      ArrayList arrayList = new ArrayList();
      foreach (OpenKey key in (ArrayList) this._keys)
        arrayList.Add((object) key.ToString());
      attributeValues.Values = arrayList.ToArray();
      if (attributeValues.Values.Length.Equals(0))
        attributeValues.Values = new object[1];
      dbObject.SetAttributesValues(new AttributeValues[1]
      {
        attributeValues
      });
      this.Modified = false;
      this._notificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsChanged", this._objectID));
    }
  }

  private void _bCancel_Click(object sender, EventArgs e)
  {
    this._first = true;
    this.Activate((IView) null);
  }

  private void _bAddKey_Click(object sender, EventArgs e)
  {
  }

  private void _bRemoveKey_Click(object sender, EventArgs e)
  {
    this._keys.Remove((object) (this._selItem.PropertyDescriptor as OpenKeyPropertyDescriptor).Parent);
    this.Modified = true;
    this.MainObject = (object) new OpenKeyClassWrapper(this._keys.Values);
  }

  private void _Grid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
  {
    this._selItem = e.NewSelection;
  }

  private void _сMenu_Popup(object sender, EventArgs e)
  {
    this._cRemove.Enabled = this._selItem != null && this._selItem.PropertyDescriptor != null;
    this._bRemoveKey.Enabled = this._cRemove.Enabled;
  }

  private sealed class OpenKeysViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList))
        ServicesManager.GetService(typeof (INamedImageList));
      return new ViewDescription()
      {
        Caption = LocalizationHolder.rm.GetString("Signs_50"),
        ImageIndex = -1,
        OrderID = 21
      };
    }
  }
}
