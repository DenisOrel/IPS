// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareTreeView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Pdm.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

public sealed class CompareTreeView : Intermech.VirtualTreeView.VirtualTreeView, IIOSource
{
  private readonly IUserNamesCache _userNamesCache;
  private readonly IProjectNamesCache _projectNamesCache;
  private readonly IObjectLevelIDsCache _objectLevelIDsCache;
  private readonly ICategoryTypeIconService _objtypesIcons;
  private readonly INamedImageList _namedImageList;
  private readonly Dictionary<int, Image> _typesIcons = new Dictionary<int, Image>();
  private readonly Dictionary<CompositionItemFlags, Image> _statusIcons;
  private readonly Dictionary<int, Image> _levelIcons = new Dictionary<int, Image>();
  private readonly string[] _forbiddenContextMenuItems = new string[11]
  {
    "Delete",
    "CheckOut",
    "CheckIn",
    "CancelChanges",
    "SaveChanges",
    "Attributes",
    "Lifecycle",
    "ObjectComposition",
    "EditDocument",
    "Create",
    "Cut"
  };
  public bool DisableExpandCollapse;

  public object Control { get; set; }

  public IServiceProvider Services { get; set; }

  public ISelectedItems SelectedItems
  {
    get
    {
      return this.SelectedRow != null && this.SelectedRow.Item is CompositionItem row ? this.GetSelectedItems(row) : (ISelectedItems) null;
    }
    set
    {
    }
  }

  private ISelectedItems GetSelectedItems(CompositionItem row)
  {
    if (row.Empty)
      return (ISelectedItems) null;
    return ObjectExtensions.GetItems(row.ObjectID);
  }

  public CompareTreeView()
  {
    this._userNamesCache = CacheManager.Cache("UserNamesCache") as IUserNamesCache;
    this._projectNamesCache = CacheManager.Cache("ProjectNamesCache") as IProjectNamesCache;
    this._objectLevelIDsCache = CacheManager.Cache("ObjectLevelIDsCache") as IObjectLevelIDsCache;
    this._objtypesIcons = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    this._namedImageList = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    this._statusIcons = new Dictionary<CompositionItemFlags, Image>()
    {
      {
        CompositionItemFlags.AnotherVersion,
        (Image) Resources.another_version
      },
      {
        CompositionItemFlags.ChangedInComposition,
        (Image) Resources.composition_changed
      },
      {
        CompositionItemFlags.AttributesChangedInCompositionObject,
        (Image) Resources.composition_changed
      }
    };
    this.AllowMultiSelect = false;
    this.CellMouseUp += new MouseEventHandler(this.OnCellMouseUp);
    this.GetCellData += new GetCellDataHandler(this.OnGetCellData);
    this.KeyUp += new KeyEventHandler(this.OnKeyUp);
  }

  private void OnCellMouseUp(object sender, MouseEventArgs e)
  {
    if (e.Button != MouseButtons.Right || ((CellWidget) sender).Row == null)
      return;
    Row row = ((CellWidget) sender).Row;
    if (((CompositionItem) row.Item).Empty)
      return;
    AdvancedServiceContainer viewServices = new AdvancedServiceContainer();
    viewServices.AddService(typeof (IViewState), (object) new ViewStateService());
    MenuBarItem contextMenu = Intermech.Navigator.ContextMenu.Services.GetMenu(this.GetSelectedItems((CompositionItem) row.Item), (IServiceProvider) viewServices);
    if (contextMenu != null && contextMenu.Items.Count > 0)
    {
      for (int i = contextMenu.Items.Count - 1; i >= 0; i--)
      {
        if (Array.Exists<string>(this._forbiddenContextMenuItems, (Predicate<string>) (_ => _.Equals(contextMenu.Items[i].CommandName))))
          contextMenu.Items.RemoveAt(i);
      }
    }
    contextMenu.Show((System.Windows.Forms.Control) this, e.Location);
  }

  private void OnKeyUp(object sender, KeyEventArgs e)
  {
    (ServicesManager.GetService(typeof (IIODispatcher)) as IIODispatcher).ProcessEvent((IIOEvent) new IOEvent((IIOSource) this, IOEventFlags.efNone, IOEventType.evKeyUp, (object) e, (object) null));
  }

  private void OnGetCellData(object sender, GetCellDataEventArgs e)
  {
    CompositionItem compositionItem = (CompositionItem) null;
    int attributeID = Convert.ToInt32(e.Column.Name);
    if (e.Row.Item is CompositionItem)
      compositionItem = (CompositionItem) e.Row.Item;
    if (compositionItem == null || compositionItem.Empty)
      return;
    bool flag = false;
    if (attributeID < 0)
    {
      switch ((ObligatoryObjectAttributes) attributeID)
      {
        case ObligatoryObjectAttributes.F_PROJECT_ID:
          if (compositionItem.ProjectID != 0L)
            e.CellData.Value = (object) this._projectNamesCache.GetProjectName(compositionItem.ProjectID);
          flag = true;
          break;
        case ObligatoryObjectAttributes.F_LEVEL_ID:
          e.CellData.Value = (object) this._objectLevelIDsCache.GetName(compositionItem.Level);
          flag = true;
          break;
        case ObligatoryObjectAttributes.F_OWNER_ID:
          e.CellData.Value = (object) this._userNamesCache.GetUserName(compositionItem.Owner);
          flag = true;
          break;
        case ObligatoryObjectAttributes.F_CHKOUT_BY:
          if (compositionItem.CheckOut != 0L)
            e.CellData.Value = (object) this._userNamesCache.GetUserName(compositionItem.CheckOut);
          flag = true;
          break;
      }
    }
    if (!flag)
    {
      CompositionItemAttribute compositionItemAttribute = compositionItem.Attributes.Find((Predicate<CompositionItemAttribute>) (x => x.AttributeID == attributeID));
      e.CellData.Value = (object) compositionItemAttribute?.AttributeValueText;
    }
    if ((compositionItem.CompositionItemFlag & CompositionItemFlags.Equal) != CompositionItemFlags.Equal)
      return;
    StyleDelta styleDelta = new StyleDelta()
    {
      HorzAlignment = e.Column.CellStyle.HorzAlignment
    };
    if ((compositionItem.CompositionItemFlag & CompositionItemFlags.Removed) == CompositionItemFlags.Removed)
      styleDelta.BackColor = ControlsHelper.RemovedColor;
    else if ((compositionItem.CompositionItemFlag & CompositionItemFlags.Added) == CompositionItemFlags.Added)
      styleDelta.BackColor = ControlsHelper.AddedColor;
    else if ((compositionItem.CompositionItemFlag & CompositionItemFlags.AttributesChanged) == CompositionItemFlags.AttributesChanged)
      styleDelta.BackColor = ControlsHelper.ChangedColor;
    StyleDelta delta1 = new StyleDelta()
    {
      BackColor = styleDelta.BackColor,
      HorzAlignment = styleDelta.HorzAlignment
    };
    e.CellData.OddStyle = new Style(e.Row.Tree.RowOddStyle, delta1);
    StyleDelta delta2 = new StyleDelta()
    {
      BackColor = styleDelta.BackColor,
      HorzAlignment = styleDelta.HorzAlignment
    };
    e.CellData.EvenStyle = new Style(e.Row.Tree.RowEvenStyle, delta2);
  }

  public VScrollBar VScrollBar => this.VertScrollBar;

  public HScrollBar HScrollBar => this.HorzScrollBar;

  protected override CellWidget CreateCellWidget(Infralution.Controls.VirtualTree.RowWidget rowWidget, Column column)
  {
    int int32 = Convert.ToInt32(column.Name);
    if (int32 < 0)
    {
      switch (int32)
      {
        case -50:
          return (CellWidget) new CaptionCellWidget(rowWidget, column, this._typesIcons, this._objtypesIcons);
        case -9:
          return (CellWidget) new LCLevelCellWidget(rowWidget, column, this._objtypesIcons, this._levelIcons);
        case -6:
          using (SessionKeeper sessionKeeper = new SessionKeeper())
            return (CellWidget) new CheckOutByCellWidget(rowWidget, column, this._namedImageList, sessionKeeper.Session.UserID);
        case -2:
          return (CellWidget) new ObjectIDCellWidget(rowWidget, column, this._namedImageList);
      }
    }
    else if (int32 == ControlsHelper.AttributeChangesID)
      return (CellWidget) new ChangesCellWidget(rowWidget, column, this._statusIcons);
    return (CellWidget) new CompareCellWidget(rowWidget, column);
  }

  protected override Infralution.Controls.VirtualTree.RowWidget CreateRowWidget(
    PanelWidget panelWidget,
    Row row)
  {
    return (Infralution.Controls.VirtualTree.RowWidget) new CompareTreeRowWidget(panelWidget, row);
  }

  protected override bool ProcessNormalCmdKeys(Keys keys)
  {
    if (!this.DisableExpandCollapse)
      return base.ProcessNormalCmdKeys(keys);
    switch (keys & Keys.KeyCode)
    {
      case Keys.Left:
        return false;
      case Keys.Right:
        return false;
      default:
        return base.ProcessNormalCmdKeys(keys);
    }
  }
}
