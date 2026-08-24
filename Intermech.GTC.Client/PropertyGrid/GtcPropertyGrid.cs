// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.GtcPropertyGrid
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Client.Core;
using Intermech.Holders;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

public class GtcPropertyGrid : System.Windows.Forms.PropertyGrid
{
  private bool _blockOnValueChange;
  private bool _blockOnPropertyTabChange;
  private bool _blockOnMasterAssign;
  private string _safeGridItemLabel = string.Empty;
  private GridItemType _safeGridItemType;
  private string _objVerIdString;
  private bool _lockTypeChange;
  private bool _isChanged;
  private Type[] _tabTypes;
  private bool _internalMenuEnabled = true;
  private bool _needBaseCallback;
  private ContextMenu _contextMenu;
  private ContextMenu _contextMenuSafe;
  private MenuItemExt _addMenuItem;
  private MenuItemExt _deleteMenuItem;
  private MenuItemExt _showObjMenuItem;
  internal GtcPropDescriptorHolder PropertyDescriptorHolder = new GtcPropDescriptorHolder();
  public static readonly Guid AddMenuItemId = Guid.NewGuid();
  public static readonly Guid DeleteMenuItemId = Guid.NewGuid();
  public static readonly Guid OpenObjMenuItemId = Guid.NewGuid();

  [Browsable(false)]
  public long Id => this.PropertyDescriptorHolder.Id;

  [Browsable(false)]
  public GetAttributeValuesModes AttributeValuesModes
  {
    get => this.PropertyDescriptorHolder.AttributeValuesModes;
  }

  [Browsable(false)]
  public bool IsChanged => this._isChanged;

  public bool LockTypeChange
  {
    get => this._lockTypeChange;
    set => this._lockTypeChange = value;
  }

  public bool InternalMenuEnabled
  {
    get => this._internalMenuEnabled;
    set
    {
      this._internalMenuEnabled = value;
      this.ContextMenu = this._internalMenuEnabled ? this._contextMenu : (ContextMenu) null;
    }
  }

  public override ContextMenu ContextMenu
  {
    get => base.ContextMenu == this._contextMenu ? (ContextMenu) null : base.ContextMenu;
    set => base.ContextMenu = value;
  }

  public event GtcPropertyGrid.GridChangedDelegate GridChanged;

  private void CreateContextMenu()
  {
    this._addMenuItem = new MenuItemExt(ServiceHolder.Rm.GetString("GTC_23"), new EventHandler(this.OnAddMenuItem), (object) GtcPropertyGrid.AddMenuItemId);
    this._deleteMenuItem = new MenuItemExt(ServiceHolder.Rm.GetString("GTC_24"), new EventHandler(this.OnDeleteMenuItem), (object) GtcPropertyGrid.DeleteMenuItemId);
    this._showObjMenuItem = new MenuItemExt(ServiceHolder.Rm.GetString("GTC_25"), new EventHandler(this.OnShowObjMenuItem), (object) GtcPropertyGrid.OpenObjMenuItemId);
    this._contextMenu = new ContextMenu();
  }

  private MenuItemExt GetMenuItemByTag(object tag, ContextMenu aContextMenu)
  {
    for (int index = 0; index < aContextMenu.MenuItems.Count; ++index)
    {
      if (aContextMenu.MenuItems[index] is MenuItemExt && ((MenuItemExt) aContextMenu.MenuItems[index]).Tag.Equals(tag))
        return (MenuItemExt) aContextMenu.MenuItems[index];
    }
    return (MenuItemExt) null;
  }

  private void PlugContextMenuItems(ContextMenu aContextMenu)
  {
    if (aContextMenu == null)
      return;
    aContextMenu.Popup += new EventHandler(this.contextMenu_Popup);
    if (this.GetMenuItemByTag((object) GtcPropertyGrid.AddMenuItemId, aContextMenu) == null)
      aContextMenu.MenuItems.Add(0, (MenuItem) this._addMenuItem);
    if (this.GetMenuItemByTag((object) GtcPropertyGrid.DeleteMenuItemId, aContextMenu) == null)
      aContextMenu.MenuItems.Add(1, (MenuItem) this._deleteMenuItem);
    if (this.GetMenuItemByTag((object) GtcPropertyGrid.OpenObjMenuItemId, aContextMenu) != null)
      return;
    aContextMenu.MenuItems.Add(2, (MenuItem) this._showObjMenuItem);
  }

  private void UnplugContextMenuItems(ContextMenu aContextMenu)
  {
    if (aContextMenu == null)
      return;
    aContextMenu.Popup -= new EventHandler(this.contextMenu_Popup);
    MenuItemExt menuItemByTag1 = this.GetMenuItemByTag((object) GtcPropertyGrid.AddMenuItemId, aContextMenu);
    MenuItemExt menuItemByTag2 = this.GetMenuItemByTag((object) GtcPropertyGrid.DeleteMenuItemId, aContextMenu);
    MenuItemExt menuItemByTag3 = this.GetMenuItemByTag((object) GtcPropertyGrid.OpenObjMenuItemId, aContextMenu);
    if (menuItemByTag1 != null)
      aContextMenu.MenuItems.Remove((MenuItem) menuItemByTag1);
    if (menuItemByTag2 != null)
      aContextMenu.MenuItems.Remove((MenuItem) menuItemByTag2);
    if (menuItemByTag3 == null)
      return;
    aContextMenu.MenuItems.Remove((MenuItem) menuItemByTag3);
  }

  private void OnAddMenuItem(object sender, EventArgs args)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this.SelectedGridItem != null && this.SelectedGridItem.PropertyDescriptor is SimplePropDescriptor && ((SimplePropDescriptor) this.SelectedGridItem.PropertyDescriptor).ParentListPropDescriptor != null)
      {
        if (!this.PropertyDescriptorHolder.AddListProperty(((SimplePropDescriptor) this.SelectedGridItem.PropertyDescriptor).ParentListPropDescriptor))
          return;
        this._isChanged = true;
        if (this.GridChanged == null)
          return;
        this.GridChanged((object) this, new GridChangedEventArgs(this._isChanged, false));
      }
      else
      {
        DataTable possibleAttributes = this.PropertyDescriptorHolder.GetPossibleAttributes(!this.PropertyDescriptorHolder.AnyAttributes, false);
        AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(true);
        List<int> intList = new List<int>()
        {
          this.PropertyDescriptorHolder.ElementType
        };
        if (this.PropertyDescriptorHolder.ElementKind == AttributableElements.Object)
          attributesSelectDlg.LoadAttrDialogForObjectsTypes(intList);
        if (this.PropertyDescriptorHolder.ElementKind == AttributableElements.Relation)
          attributesSelectDlg.LoadAttrDialogForRelationsTypes(intList);
        attributesSelectDlg.ForbiddenAttrsTypesFilter.AddRange((IEnumerable<FieldTypes>) new FieldTypes[4]
        {
          FieldTypes.ftBlob,
          FieldTypes.ftShortBlob,
          FieldTypes.ftFile,
          FieldTypes.ftSystem
        });
        if (attributesSelectDlg.ShowDialog() != DialogResult.OK || attributesSelectDlg.SelectedAttributesID.Count <= 0)
          return;
        ArrayList arrayList = new ArrayList();
        for (int index = 0; index < attributesSelectDlg.SelectedAttributesID.Count; ++index)
        {
          DataRow[] dataRowArray = (DataRow[]) null;
          if (possibleAttributes != null)
            dataRowArray = possibleAttributes.Select("F_ATTRIBUTE_ID=" + (object) attributesSelectDlg.SelectedAttributesID[index]);
          if (possibleAttributes == null || dataRowArray.Length != 0)
          {
            IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(Convert.ToInt32(attributesSelectDlg.SelectedAttributesID[index]));
            AttributeValues attributeValues = new AttributeValues(attributeType.AttributeID, attributeType.AttributeType, attributeType.MultipleValued, attributeType.Computed);
            attributeValues.AttributeName = attributeType.Name;
            object[] objArray1 = new object[1];
            object[] objArray2 = new object[1];
            bool flag = true;
            string str = string.Empty;
            ArrayList groupById = DataHolders.AttributesHolder.GetGroupByID(attributeType.AttributeID);
            if (groupById != null && groupById.Count > 0)
              str = DataHolders.AttributeGroupsHolder.GetNamebyID((int) groupById[0]);
            IDBAttributable attributable = this.PropertyDescriptorHolder.GetAttributable(sessionKeeper.Session);
            if (attributable != null)
            {
              if (attributable.GetAttributeByID(attributeType.AttributeID) != null && !this.PropertyDescriptorHolder.CheckIfDeleted(attributeType.AttributeID))
              {
                int num = (int) MessageBox.Show(string.Format(ServiceHolder.Rm.GetString("GTC_26"), (object) attributeType.Name));
                continue;
              }
              if (attributeType.AttributeType != FieldTypes.ftAutoInc)
              {
                IDBAttribute dbAttribute = attributable.Attributes.AddTemporaryAttribute(attributeType.AttributeID, false);
                if (dbAttribute != null)
                {
                  objArray1 = new object[dbAttribute.Values.Length];
                  dbAttribute.Values.CopyTo((Array) objArray1, 0);
                  objArray2 = new object[dbAttribute.Values.Length];
                  flag = dbAttribute.ReadOnly;
                  if (str == string.Empty)
                    str = dbAttribute.GroupName;
                }
              }
            }
            attributeValues.Values = objArray1;
            attributeValues.ReadOnly = flag;
            attributeValues.GroupName = str;
            attributeValues.Descriptions = objArray2;
            arrayList.Add((object) attributeValues);
          }
          else
          {
            IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(Convert.ToInt32(attributesSelectDlg.SelectedAttributesID[index]));
            string str = attributeType != null ? attributeType.Name : Convert.ToString(attributesSelectDlg.SelectedAttributesID[index]);
            int num = (int) MessageBox.Show(string.Format(this.PropertyDescriptorHolder.AttributableElement != AttributableElements.Object ? ServiceHolder.Rm.GetString("GTC_28") : ServiceHolder.Rm.GetString("GTC_27"), (object) str));
          }
        }
        bool directWriteOccured;
        if (arrayList.Count <= 0 || !this.PropertyDescriptorHolder.AddProperty((AttributeValues[]) arrayList.ToArray(typeof (AttributeValues)), out directWriteOccured))
          return;
        this._isChanged = true;
        List<AttributeValues> attributeValuesList = new List<AttributeValues>();
        for (int index = 0; index < arrayList.Count; ++index)
        {
          int attributeValueListIndex = ObjectPropDescriptorHolder.GetAttributeValueListIndex(this.PropertyDescriptorHolder.AttributeValuesList, ((AttributeValues) arrayList[index]).AttributeID);
          if (attributeValueListIndex != -1)
          {
            AttributeValues attributeValues = (AttributeValues) this.PropertyDescriptorHolder.AttributeValuesList[attributeValueListIndex];
            if (attributeValues.AttributeType == FieldTypes.ftObjectLink && (attributeValues.MultipleValued == MultiValueModes.SingleValue || attributeValues.MultipleValued == MultiValueModes.SingleValueFromList))
              attributeValuesList.Add(attributeValues);
          }
        }
        if (attributeValuesList.Count > 0)
          this.PropertyDescriptorHolder.AddProperty((AttributeValues[]) arrayList.ToArray(typeof (AttributeValues)), out bool _, true, false);
        string attributeName = ((AttributeValues) arrayList[0]).AttributeName;
        GridItemType type = GridItemType.Property;
        if (this.SelectedObject != null && this.SelectedGridItem != null && attributeName != string.Empty)
        {
          GridItem gridItem = this.FindGridItem(attributeName, type, this.SelectedGridItem);
          if (gridItem != null)
            this.SelectedGridItem = gridItem;
        }
        if (this.GridChanged == null)
          return;
        this.GridChanged((object) this, new GridChangedEventArgs(this._isChanged, directWriteOccured));
      }
    }
  }

  private void OnDeleteMenuItem(object sender, EventArgs args)
  {
    if (this.SelectedGridItem == null)
      return;
    bool directWriteOccured;
    int num = this.PropertyDescriptorHolder.DeleteProperty((PropDescriptor) this.SelectedGridItem.PropertyDescriptor, out directWriteOccured) ? 1 : 0;
    if (num != 0)
      this._isChanged = true;
    if ((num | (directWriteOccured ? 1 : 0)) == 0 || this.GridChanged == null)
      return;
    this.GridChanged((object) this, new GridChangedEventArgs(this._isChanged, directWriteOccured));
  }

  private void OnShowObjMenuItem(object sender, EventArgs args)
  {
    if (this.SelectedGridItem == null)
      return;
    PropDescriptor propertyDescriptor = (PropDescriptor) this.SelectedGridItem.PropertyDescriptor;
    if (propertyDescriptor == null || !(propertyDescriptor.GetValue((object) this) is ObjectPropertyClass objectPropertyClass))
      return;
    Utils.OpenNewWindow((IDescriptor) new Intermech.Navigator.DBObjects.Descriptor(objectPropertyClass.ObjectID), (IServiceProvider) new AdvancedServiceContainer());
  }

  private void contextMenu_Popup(object sender, EventArgs e)
  {
    bool flag1 = true;
    bool flag2 = true;
    bool flag3 = true;
    bool flag4 = false;
    if (this.SelectedGridItem != null)
    {
      int attributeIdbyGridItem = GtcPropertyGrid.GetAttributeIDbyGridItem(this.SelectedGridItem);
      bool flag5 = this.PropertyDescriptorHolder.LockedAttributes.IndexOf(attributeIdbyGridItem) != -1;
      if (this.SelectedGridItem.PropertyDescriptor is SimplePropDescriptor && ((SimplePropDescriptor) this.SelectedGridItem.PropertyDescriptor).ParentListPropDescriptor != null && ((SimplePropDescriptor) this.SelectedGridItem.PropertyDescriptor).ParentListPropDescriptor.IsReadOnly | flag5)
      {
        flag2 = false;
        flag3 = false;
      }
      if (this.SelectedGridItem.PropertyDescriptor is SimplePropDescriptor && ((SimplePropDescriptor) this.SelectedGridItem.PropertyDescriptor).ParentListPropDescriptor == null && flag5)
        flag3 = false;
      if (this.SelectedGridItem.PropertyDescriptor is SimplePropDescriptor && this.SelectedGridItem.PropertyDescriptor.PropertyType == typeof (ObjectPropertyClass) && this.SelectedGridItem.PropertyDescriptor.GetValue((object) this) is ObjectPropertyClass)
        flag4 = true;
      int attributeValueListIndex = this.PropertyDescriptorHolder.GetAttributeValueListIndex(attributeIdbyGridItem);
      if (attributeValueListIndex != -1)
        flag1 = AttributeValuesEditor.IsSystemAttributeValue((AttributeValues) this.PropertyDescriptorHolder.AttributeValuesList[attributeValueListIndex]);
      if (flag3 & flag5)
        flag3 = false;
    }
    this._deleteMenuItem.Visible = this.SelectedGridItem != null && this.SelectedGridItem.PropertyDescriptor is PropDescriptor && !flag1;
    this._showObjMenuItem.Visible = this.SelectedGridItem != null && this.SelectedGridItem.PropertyDescriptor is PropDescriptor;
    this._addMenuItem.Enabled = flag2;
    this._deleteMenuItem.Enabled = flag3;
    this._showObjMenuItem.Enabled = flag4;
  }

  private void UpdatePropDescriptorDescription(PropertyDescriptor propertyDescriptor)
  {
    if (propertyDescriptor == null || !(propertyDescriptor is PropDescriptor) || !(propertyDescriptor.PropertyType == typeof (ObjectPropertyClass)))
      return;
    string aDescription = propertyDescriptor.Description;
    if (aDescription != null)
    {
      int startIndex = aDescription.IndexOf("\n", StringComparison.Ordinal);
      if (startIndex >= 0)
        aDescription = aDescription.Remove(startIndex);
    }
    if (propertyDescriptor.GetValue((object) this) is ObjectPropertyClass objectPropertyClass && objectPropertyClass.ObjectID != 0L)
      aDescription = $"{aDescription}\n{this._objVerIdString}={(object) objectPropertyClass.ObjectID}";
    ((PropDescriptor) propertyDescriptor).SetDescription(aDescription);
    this.Refresh();
  }

  private GridItem FindRootGridItem(GridItem gi)
  {
    while (gi.Parent != null && gi.GridItemType != GridItemType.Root)
      gi = gi.Parent;
    return gi;
  }

  private GridItem FindGridItem(string label, GridItemType type, GridItem gi)
  {
    gi = this.FindRootGridItem(gi);
    return this.FindGridItemCustom(label, type, gi);
  }

  private GridItem FindGridItemCustom(string label, GridItemType type, GridItem gi)
  {
    if (this.GridItemEqual(label, type, gi))
      return gi;
    GridItem gridItemCustom = (GridItem) null;
    foreach (GridItem gridItem in gi.GridItems)
    {
      if (this.GridItemEqual(label, type, gridItem))
      {
        gridItemCustom = gridItem;
        break;
      }
      gridItemCustom = this.FindGridItemCustom(label, type, gridItem);
      if (gridItemCustom != null)
        break;
    }
    return gridItemCustom;
  }

  private bool GridItemEqual(string label, GridItemType type, GridItem gi)
  {
    return gi.GridItemType == type && gi.Label == label;
  }

  public PropertyTab PropertyTabByGuid(Guid guid)
  {
    PropertyTab propertyTab = (PropertyTab) null;
    for (int index = 0; index < this.PropertyTabs.Count; ++index)
    {
      if (this.PropertyTabs[index] is IObjectPropertyGridTab && ((IObjectPropertyGridTab) this.PropertyTabs[index]).TabGuid.Equals(guid))
      {
        propertyTab = this.PropertyTabs[index];
        break;
      }
    }
    return propertyTab;
  }

  public bool Load(
    long aId,
    AttributableElements aAttributableElement,
    GetAttributeValuesModes aAttributeValuesModes,
    bool aIsChanged,
    params Type[] tabTypes)
  {
    string safeGridItemLabel = this._safeGridItemLabel;
    GridItemType safeGridItemType = this._safeGridItemType;
    this._tabTypes = tabTypes;
    if (!this.PropertyDescriptorHolder.AssignData(aId, aAttributableElement, aAttributeValuesModes, this, this._lockTypeChange, this._tabTypes))
      return false;
    if (this.SelectedObject != null && this.SelectedGridItem != null)
    {
      if (safeGridItemLabel != string.Empty)
      {
        GridItem gridItem = this.FindGridItem(safeGridItemLabel, safeGridItemType, this.SelectedGridItem);
        if (gridItem != null)
          this.SelectedGridItem = gridItem;
      }
      else
      {
        GridItem rootGridItem = this.FindRootGridItem(this.SelectedGridItem);
        if (rootGridItem != null && rootGridItem.GridItems.Count > 0)
          this.SelectedGridItem = rootGridItem.GridItems[0];
      }
    }
    this._isChanged = aIsChanged;
    return true;
  }

  public bool Save() => this.Save(false);

  public bool Save(bool blankMode)
  {
    ArrayList origList;
    ArrayList fireList;
    if (!this._isChanged || !this.PropertyDescriptorHolder.SaveData(out origList, out fireList))
      return true;
    this._isChanged = false;
    if (ServicesManager.GetService(typeof (INotificationService)) is INotificationService service)
    {
      switch (this.PropertyDescriptorHolder.AttributableElement)
      {
        case AttributableElements.Object:
          DBObjectsExtendedEventArgs e = new DBObjectsExtendedEventArgs("ObjectsChanged", this.PropertyDescriptorHolder.Id, this.PropertyDescriptorHolder.ElementType, (AttributeValues[]) origList.ToArray(typeof (AttributeValues)), (AttributeValues[]) fireList.ToArray(typeof (AttributeValues)));
          if (blankMode)
            e.VerType = ObjectRecordKind.Blank;
          service.FireEvent((object) this, (NotificationEventArgs) e);
          break;
        case AttributableElements.Relation:
          service.FireEvent((object) this, (NotificationEventArgs) new DBRelationsExtendedEventArgs("RelationsChanged", this.PropertyDescriptorHolder.Id, this.PropertyDescriptorHolder.ElementType, (AttributeValues[]) origList.ToArray(typeof (AttributeValues)), (AttributeValues[]) fireList.ToArray(typeof (AttributeValues))));
          break;
      }
    }
    return true;
  }

  public static int GetAttributeIDbyGridItem(GridItem gridItem)
  {
    int attributeIdbyGridItem = 0;
    if (gridItem != null)
    {
      if (gridItem.PropertyDescriptor is ListPropDescriptor || gridItem.PropertyDescriptor is SimplePropDescriptor && ((SimplePropDescriptor) gridItem.PropertyDescriptor).ParentListPropDescriptor == null)
        attributeIdbyGridItem = ((PropDescriptor) gridItem.PropertyDescriptor).PropID;
      else if (gridItem.PropertyDescriptor is SimplePropDescriptor && ((SimplePropDescriptor) gridItem.PropertyDescriptor).ParentListPropDescriptor != null)
        attributeIdbyGridItem = ((SimplePropDescriptor) gridItem.PropertyDescriptor).ParentListPropDescriptor.PropID;
    }
    return attributeIdbyGridItem;
  }

  public GtcPropertyGrid()
  {
    this.DrawFlatToolbar = true;
    this._objVerIdString = ServiceHolder.Rm.GetString("GTC_17");
    this.CreateContextMenu();
  }

  protected override void OnContextMenuChanged(EventArgs e)
  {
    if (!this._internalMenuEnabled)
    {
      base.OnContextMenuChanged(e);
    }
    else
    {
      this.UnplugContextMenuItems(this._contextMenuSafe);
      this._contextMenuSafe = base.ContextMenu;
      this.PlugContextMenuItems(this._contextMenuSafe);
      if (base.ContextMenu == null)
      {
        this._needBaseCallback = true;
        this.ContextMenu = this._contextMenu;
      }
      else
      {
        if (!this._needBaseCallback && base.ContextMenu == this._contextMenu)
          return;
        this._needBaseCallback = false;
        base.OnContextMenuChanged(e);
      }
    }
  }

  protected override void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
  {
    if (this._blockOnValueChange)
      return;
    bool flag = false;
    PropDescriptor propertyDescriptor1 = (PropDescriptor) e.ChangedItem.PropertyDescriptor;
    if (propertyDescriptor1 == null)
      throw new Exception("PropDescriptor not found!");
    this.UpdatePropDescriptorDescription((PropertyDescriptor) propertyDescriptor1);
    if (this.PropertyDescriptorHolder.AttributableElement == AttributableElements.Object && propertyDescriptor1.PropID == Convert.ToInt32((object) ObligatoryObjectAttributes.F_OBJECT_TYPE) || this.PropertyDescriptorHolder.AttributableElement == AttributableElements.Relation && propertyDescriptor1.PropID == Convert.ToInt32((object) ObligatoryObjectAttributes.F_RELATION_TYPE))
    {
      flag = true;
      if (MessageBox.Show(ServiceHolder.Rm.GetString("GTC_18"), ServiceHolder.Rm.GetString("GTC_19"), MessageBoxButtons.YesNo) != DialogResult.Yes)
      {
        try
        {
          this._blockOnValueChange = true;
          if (e.ChangedItem.PropertyDescriptor == null)
            return;
          e.ChangedItem.PropertyDescriptor.SetValue((object) this, e.OldValue);
          return;
        }
        finally
        {
          this._blockOnValueChange = false;
        }
      }
    }
    if (flag)
    {
      int num1 = -1;
      if (this.PropertyDescriptorHolder.AttributableElement == AttributableElements.Object)
      {
        ObjectTypePropertyClass typePropertyClass = (ObjectTypePropertyClass) propertyDescriptor1.GetValue((object) this);
        if (typePropertyClass != null)
          num1 = typePropertyClass.ObjectType;
      }
      if (this.PropertyDescriptorHolder.AttributableElement == AttributableElements.Relation)
      {
        RelationTypePropertyClass typePropertyClass = (RelationTypePropertyClass) propertyDescriptor1.GetValue((object) this);
        if (typePropertyClass != null)
          num1 = typePropertyClass.RelationType;
      }
      if (!this.Save())
      {
        try
        {
          this._blockOnValueChange = true;
          if (e.ChangedItem.PropertyDescriptor != null)
            e.ChangedItem.PropertyDescriptor.SetValue((object) this, e.OldValue);
          int num2 = (int) MessageBox.Show(ServiceHolder.Rm.GetString("GTC_20"), ServiceHolder.Rm.GetString("GTC_21"), MessageBoxButtons.OK);
          return;
        }
        finally
        {
          this._blockOnValueChange = false;
        }
      }
      else
      {
        try
        {
          try
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              if (this.PropertyDescriptorHolder.AttributableElement == AttributableElements.Object)
              {
                IDBObject dbObject = sessionKeeper.Session.GetObject(this.PropertyDescriptorHolder.Id);
                if (dbObject != null)
                  dbObject.ObjectType = num1;
              }
              if (this.PropertyDescriptorHolder.AttributableElement == AttributableElements.Relation)
              {
                IDBRelation relation = sessionKeeper.Session.GetRelation(this.PropertyDescriptorHolder.Id);
                if (relation != null)
                  relation.RelationType = num1;
              }
            }
          }
          catch (Exception ex)
          {
            this._blockOnValueChange = true;
            if (e.ChangedItem.PropertyDescriptor != null)
              e.ChangedItem.PropertyDescriptor.SetValue((object) this, e.OldValue);
            this._blockOnValueChange = false;
            ExceptionHelper.ExceptionService.ShowException(ex);
            return;
          }
          this._blockOnValueChange = true;
          if (!this.Load(this.PropertyDescriptorHolder.Id, this.PropertyDescriptorHolder.AttributableElement, this.PropertyDescriptorHolder.AttributeValuesModes, false, this._tabTypes))
          {
            int num3 = (int) MessageBox.Show(ServiceHolder.Rm.GetString("GTC_22"), ServiceHolder.Rm.GetString("GTC_21"), MessageBoxButtons.OK);
          }
          else if (this.PropertyDescriptorHolder != null)
          {
            DBObjectsEventArgs e1 = new DBObjectsEventArgs("ObjectsChanged", this.PropertyDescriptorHolder.Id);
            if (ServicesManager.GetService(typeof (INotificationService)) is INotificationService service)
              service.FireEvent((object) this, (NotificationEventArgs) e1);
          }
        }
        finally
        {
          this._blockOnValueChange = false;
        }
      }
    }
    else
    {
      this._isChanged = true;
      PropDescriptor propertyDescriptor2 = (PropDescriptor) e.ChangedItem.PropertyDescriptor;
      if (propertyDescriptor2 != null)
        propertyDescriptor2.ValueChanged = true;
    }
    base.OnPropertyValueChanged(e);
    if (flag)
      return;
    if (this._blockOnMasterAssign)
      return;
    try
    {
      this._blockOnMasterAssign = true;
      PropDescriptor propertyDescriptor3 = (PropDescriptor) e.ChangedItem.PropertyDescriptor;
      if (!(propertyDescriptor3 is SimplePropDescriptor) || ((SimplePropDescriptor) propertyDescriptor3).ParentListPropDescriptor != null)
        return;
      int attributeValueListIndex = ObjectPropDescriptorHolder.GetAttributeValueListIndex(this.PropertyDescriptorHolder.AttributeValuesList, propertyDescriptor3.PropID);
      if (attributeValueListIndex == -1)
        return;
      AttributeValues attributeValues = (AttributeValues) ((AttributeValues) this.PropertyDescriptorHolder.AttributeValuesList[attributeValueListIndex]).Clone();
      if (attributeValues.AttributeType != FieldTypes.ftObjectLink || attributeValues.MultipleValued != MultiValueModes.SingleValue && attributeValues.MultipleValued != MultiValueModes.SingleValueFromList)
        return;
      bool directWriteOccured;
      if (!(this.PropertyDescriptorHolder.AddProperty(new AttributeValues[1]
      {
        attributeValues
      }, out directWriteOccured, true, true) | directWriteOccured) || this.GridChanged == null)
        return;
      this.GridChanged((object) this, new GridChangedEventArgs(this._isChanged, directWriteOccured));
    }
    finally
    {
      this._blockOnMasterAssign = false;
    }
  }

  protected override void OnPropertyTabChanged(PropertyTabChangedEventArgs e)
  {
    if (this._blockOnPropertyTabChange)
      return;
    base.OnPropertyTabChanged(e);
  }

  protected override void OnSelectedGridItemChanged(SelectedGridItemChangedEventArgs e)
  {
    base.OnSelectedGridItemChanged(e);
    GridItem gridItem = e.NewSelection;
    if (gridItem == null)
    {
      this._safeGridItemLabel = string.Empty;
    }
    else
    {
      if (gridItem.GridItemType == GridItemType.Property)
      {
        while (gridItem.Parent != null && gridItem.Parent.GridItemType == GridItemType.Property)
          gridItem = gridItem.Parent;
      }
      this._safeGridItemLabel = gridItem.Label;
      this._safeGridItemType = gridItem.GridItemType;
      this.UpdatePropDescriptorDescription(gridItem.PropertyDescriptor);
    }
  }

  protected override Type DefaultTabType => typeof (GtcPropertiesTabCustom);

  public delegate void GridChangedDelegate(object sender, GridChangedEventArgs e);
}
