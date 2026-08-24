// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionCopyEntersInView
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.MRP2;

[ViewDescriptionProvider(typeof (ProductionCopyEntersInView.ProductionCopyEntersInViewDescriptionProvider))]
public class ProductionCopyEntersInView : ChildrenView
{
  private long _objectID;
  private IServiceProvider _provider;
  private string PLEntersInStatesName = "PLEntersIn_{8A053E90-FA03-4F0E-AF3E-EEF039FD4E85}";
  private int _imageIndex = -1;
  private ListDescriptor _descriptor;
  /// <summary>Required designer variable.</summary>
  private IContainer components;

  public ProductionCopyEntersInView()
  {
    this.InitializeComponent();
    if (!(ServicesManager.GetService(typeof (INamedImageList)) is INamedImageList service))
      return;
    this._imageIndex = service.ImageIndex("imgEntersTo");
  }

  protected override bool UseInheritedNavViews
  {
    [DebuggerStepThrough] get => false;
    set => base.UseInheritedNavViews = false;
  }

  public override string StateStreamPrefix => this.PLEntersInStatesName;

  public override string Caption => "Применяемость в ПВ";

  public override int ImageIndex => this._imageIndex;

  public override int OrderID => 16 /*0x10*/;

  public override bool DisablePacketsReading => true;

  public override void Activate(IView previousView)
  {
    if (Intermech.Consts.IsUndefinedObjectId(this._objectID) || previousView == PageViewsManager.BlackHoleView)
      return;
    this.LoadItems(true);
    ISelectedItems selectedItems = this.SelectedItems;
    base.Activate(previousView);
  }

  private void LoadItems(bool needCallInitialize)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._objectID);
      long conditionValue;
      if (MetaDataHelper.IsObjectTypeChildOf(dbObject.ObjectType, new Guid("cad00268-306c-11d8-b4e9-00304f19f545")))
      {
        conditionValue = this._objectID;
      }
      else
      {
        IDBAttribute attributeById = dbObject.GetAttributeByID(MRP2Consts.attrIdArticleLink);
        if (attributeById == null)
        {
          this._descriptor = new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, MRP2Consts.objtypeIdProductionLists, "", (IList) null);
          this._node = this._descriptor.GetChild((INodeID) null);
          if (!needCallInitialize)
            return;
          this.Initialize((IDescriptor) this._descriptor, this._provider);
          return;
        }
        conditionValue = attributeById.AsInteger;
      }
      DataTable dataTable = sessionKeeper.Session.GetObjectCollection(MRP2Consts.objtypeIdProductionCopy).Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(MRP2Consts.attrIdArticleLink, RelationalOperators.Equal, (object) conditionValue, LogicalOperators.AND, 0, false)
      }, new object[2]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID,
        (object) ObligatoryObjectAttributes.F_OBJECT_TYPE
      })
      {
        RecordCount = -1
      });
      List<ObjInfoItem> objects = new List<ObjInfoItem>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        objects.Add(new ObjInfoItem(Convert.ToInt64(row[0]), Convert.ToInt32(row[1])));
      if (objects.Count == 0)
        return;
      IFiltrationService service = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
      VersionsRule ruleClass = service.RuleClass;
      string filtrationServiceOwnerId = service.FiltrationServiceOwnerID;
      VersionsRule versionsRule = (VersionsRule) null;
      string filtrationOwnerId = "cad001e0-306c-11d8-b4e9-00304f19f545";
      ConditionStructure[] conditions = new ConditionStructure[1]
      {
        new ConditionStructure(MRP2Consts.attrIdDeleteTag, RelationalOperators.Equal, (object) false, LogicalOperators.AND, 0, false)
      };
      DataTable source = ServiceUtils.GetService<ICompositionLoadService>((object) sessionKeeper.Session, true).LoadComplexCompositions((object) sessionKeeper.Session, (IEnumerable<ObjInfoItem>) objects, (IEnumerable<int>) new int[1]
      {
        MRP2Consts.reltypeIdProductComposition
      }, (IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(MRP2Consts.objtypeIdProductionLists), (IEnumerable<ColumnDescriptor>) new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID)
      }, false, false, versionsRule, (IEnumerable<ConditionStructure>) conditions, filtrationOwnerId, (Dictionary<long, HybridDictionary>) null, -1, (IEnumerable<int>) MetaDataHelper.GetObjectTypeChildrenIDRecursive(MRP2Consts.objtypeIdProductionObjects));
      if (source == null)
        return;
      this._descriptor = new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, MRP2Consts.objtypeIdProductionLists, "", (IList) source.AsEnumerable().Select<DataRow, long>((System.Func<DataRow, long>) (row => Convert.ToInt64(row[0]))).ToList<long>());
      if (needCallInitialize)
        this.Initialize((IDescriptor) this._descriptor, this._provider);
      if (this._node != null)
        return;
      this._node = this._descriptor.GetChild((INodeID) null);
    }
  }

  protected override INode GetNode()
  {
    if (this._descriptor == null)
      return base.GetNode();
    INode child = this._descriptor.GetChild((INodeID) null);
    if (!(child is IContextAware contextAware))
      return child;
    contextAware.Services = (IServiceProvider) this.Services;
    return child;
  }

  public override void Deactivate(IView nextView) => base.Deactivate(nextView);

  public override void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    this.AllowEditing = false;
    this.DisableParentSelectedItems = true;
    this.DisableFiltration = true;
    this._objectID = 0L;
    if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData))
      return;
    this._objectID = itemData.ObjectID;
    this._provider = provider;
  }

  protected override void NotificationEventFired(object sender, NotificationEventArgs e)
  {
    base.NotificationEventFired(sender, e);
    if (this._descriptor == null || !(e.EventName == "ObjectsCheckedIn") && !(e.EventName == "ObjectsChangesCancelled") && !(e.EventName == "ObjectsCheckedOut"))
      return;
    bool flag = false;
    if (e is DBObjectsEventArgs objectsEventArgs && objectsEventArgs.ObjectIDs != null)
    {
      foreach (long objectId in (IEnumerable<long>) objectsEventArgs.ObjectIDs)
      {
        flag = this._descriptor.ObjectIDs.Contains((object) objectId);
        if (flag)
          break;
      }
    }
    if (!flag)
      return;
    this.ReloadItems(new int?());
  }

  public override ContentType ViewContentType => ContentType.NonFolders;

  public override void ReloadItems(int? count = null)
  {
    if (!Intermech.Consts.IsUndefinedObjectId(this._objectID))
      this.LoadItems(false);
    base.ReloadItems();
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    ((ISupportInitialize) this._grid).BeginInit();
    ((ISupportInitialize) this._pictureBox).BeginInit();
    this.SuspendLayout();
    this._toolBar.Size = new Size(642, 62);
    this._grid.DefaultAutoGroupRow.Height = 25;
    this._grid.FrozenArea.ColCount = 1;
    this._grid.FrozenArea.SortFrozenRows = true;
    this._grid.GroupBox.BackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this._grid.GroupBox.HintForeColor = SystemColors.ControlText;
    this._grid.GroupBox.Text = "Перетащите заголовок колонки в эту область для группировки по значениям этой колонки";
    this._grid.GroupBox.Visible = true;
    this._grid.Header.AutoHeightFlags = iGHdrAutoHeightFlags.OnAddCol | iGHdrAutoHeightFlags.OnRemoveCol | iGHdrAutoHeightFlags.OnShowCol | iGHdrAutoHeightFlags.OnContentsChange | iGHdrAutoHeightFlags.OnThemeChange | iGHdrAutoHeightFlags.OnResizeCol;
    this._grid.Header.Height = 20;
    this._grid.LayoutObject.Flags = iGLayoutFlags.Grouping | iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this._grid.Location = new Point(0, 62);
    this._grid.Size = new Size(642, 344);
    this._pageViewsManager.Size = new Size(642, 0);
    this._filtersComboBoxItem.Padding.Bottom = 0;
    this._filtersComboBoxItem.Padding.Left = 1;
    this._filtersComboBoxItem.Padding.Right = 1;
    this._filtersComboBoxItem.Padding.Top = 0;
    this.buttonHeightSet.Padding.Bottom = 0;
    this.buttonHeightSet.Padding.Left = 0;
    this.buttonHeightSet.Padding.Right = 0;
    this.buttonHeightSet.Padding.Top = 0;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (ProductionCopyEntersInView);
    this.Size = new Size(642, 435);
    ((ISupportInitialize) this._grid).EndInit();
    ((ISupportInitialize) this._pictureBox).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private sealed class ProductionCopyEntersInViewDescriptionProvider : BaseViewDescriptionProvider
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
        Caption = "Применяемость в ПВ",
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgEntersTo") : -1,
        OrderID = 16 /*0x10*/
      };
    }
  }
}
