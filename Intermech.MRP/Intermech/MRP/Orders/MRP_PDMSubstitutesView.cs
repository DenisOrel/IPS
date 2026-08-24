// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.MRP_PDMSubstitutesView
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Infralution.Controls.VirtualTree;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>Закладка "Допустимые замены"</summary>
/// <summary>
/// Закладка выбора актуальных заменителей среди допустимых
/// </summary>
internal class MRP_PDMSubstitutesView : MRP_BaseView
{
  /// <summary>Колонка "Заголовок"</summary>
  private const string colCAPTION = "colCAPTION";
  /// <summary>Колонка "Позиция"</summary>
  private const string colPOSITION = "colPOSITION";
  /// <summary>Колонка "Количество"</summary>
  private const string colQUANTITY = "colQUANTITY";
  /// <summary>Колонка "Примечание"</summary>
  private const string colNOTE = "colNOTE";
  /// <summary>Колонка "Идентификатор связи"</summary>
  private const string colPrjLinkID = "colPrjLinkID";
  /// <summary>Колонка "Номер группы заменителей"</summary>
  private const string colGroupNo = "colGroupNo";
  /// <summary>Колонка "Номер заменителя в группе"</summary>
  private const string colSubGroup = "colSubGroup";
  /// <summary>Допустимые замены в группах</summary>
  protected SubstituteObjects substitutes;
  /// <summary>Тип объекта, тип связи, идентификатор связи</summary>
  protected Tuple<int, int, long> objRelTypeLinkID;
  /// <summary>Список актуальных заменителей</summary>
  protected Dictionary<long, long> actualSubstitutes = new Dictionary<long, long>();
  /// <summary>Запрет на обработку событий от дерева</summary>
  private bool FDisableTreeEvents;
  /// <summary>
  /// Статус выделенных связей (если редактируется одно из исполнений)
  /// </summary>
  private ArticleRelationState selSubstState;
  /// <summary>Есть ли хотя бы одна выделенная группа</summary>
  private bool hasSelGroup;
  /// <summary>Есть ли хотя бы один выделенный заменитель в группе</summary>
  private bool hasSelSubst;
  /// <summary>Есть ли хотя бы одна выделенная связь в заменителе</summary>
  private bool hasSelRel;
  /// <summary>Задействованные группы</summary>
  private List<long> groups = new List<long>();
  /// <summary>Выделенные в дереве группы и их заменители</summary>
  private Dictionary<long, List<long>> groupsSubsts = new Dictionary<long, List<long>>();
  /// <summary>Задействованные связи</summary>
  private List<long> selRelations = new List<long>();
  /// <summary>Номер текущего заменителя</summary>
  private long currSubstNo = -1;
  /// <summary>Служба для работы с допустимыми заменами, PDM-плагин</summary>
  private IPDMSubstitutesService pdmSubstitutesService;
  /// <summary>Для регистрации своих категорий</summary>
  private IGuidMapper FGuidMapper;
  /// <summary>
  /// Служба генерации примечаний для связей, участвующих в допустимых заменах
  /// </summary>
  private ISubstitutesRemarksService _substsRemarks;
  /// <summary>Настройки допустимых замен</summary>
  private ISubstitutesSettings _substsSettings;
  /// <summary>Значок для группы заменителей</summary>
  private static Icon _iconGroup;
  /// <summary>Значок для актуального заменителя в группе</summary>
  private static Icon _iconActualSubstitute;
  /// <summary>Значок для заменителя в группе</summary>
  private static Icon _iconSubstitute;
  /// <summary>
  /// Список дополнительных атрибутов, которые будут загружаться в узлы состава.
  /// ВНИМАНИЕ!!! В качестве ID можно использовать только Int32 !!!
  /// </summary>
  internal List<NodeColumnID> _advAttributes = new List<NodeColumnID>();
  /// <summary>Примечания для допустимых замен</summary>
  private Dictionary<long, string> remarks;
  /// <summary>Скрыта ли панель подсказки</summary>
  internal static bool hiddenHintPanel;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private ImageList imagesTreeview;
  private Column columnMain;
  private Column columnPos;
  private Column columnCount;
  private Column columnNote;
  private CellEditor cellEditor;
  protected internal iGrid grid;
  private iGCellStyle gridCol0CellStyle;
  private iGColHdrStyle gridCol0ColHdrStyle;
  private iGCellStyle gridCol1CellStyle;
  private iGColHdrStyle gridCol1ColHdrStyle;
  private iGCellStyle gridCol2CellStyle;
  private iGColHdrStyle gridCol2ColHdrStyle;
  private iGCellStyle gridCol3CellStyle;
  private iGColHdrStyle gridCol3ColHdrStyle;
  private iGCellStyleDesign groupRowsLevel1;
  private iGCellStyleDesign groupRowsLevel2;
  private iGCellStyleDesign rowsLevel3;
  private iGCellStyleDesign rowsLevel3Right;
  private iGCellStyle gridCol4CellStyle;
  private iGColHdrStyle gridCol4ColHdrStyle;
  private iGCellStyle gridCol5CellStyle;
  private iGColHdrStyle gridCol5ColHdrStyle;
  private iGCellStyle gridCol6CellStyle;
  private iGColHdrStyle gridCol6ColHdrStyle;
  private ImageList imagesToolbars;
  private Intermech.Bars.ToolBar toolBarGrid;
  private ButtonItem btCollapseAll;
  private ButtonItem btRefresh;
  private ButtonItem btActualize;
  private ButtonItem btExpandAll;
  private Panel panelHint;
  private Button btnHideHint;
  private ToolTip toolTips;
  private RichTextBox edHint;

  /// <summary>Создать экземпляр класса</summary>
  public MRP_PDMSubstitutesView()
  {
    this.InitializeComponent();
    this.pdmSubstitutesService = ServicesManager.GetService(typeof (IPDMSubstitutesService)) as IPDMSubstitutesService;
    this.FGuidMapper = ServicesManager.GetService(typeof (IGuidMapper)) as IGuidMapper;
    this._substsRemarks = ServicesManager.GetService(typeof (ISubstitutesRemarksService)) as ISubstitutesRemarksService;
    this._substsSettings = ServicesManager.GetService(typeof (ISubstitutesSettings)) as ISubstitutesSettings;
    if (MRP_PDMSubstitutesView._iconGroup == null)
    {
      MRP_PDMSubstitutesView._iconGroup = ImageHelper.BitmapToIcon(this.imagesTreeview.Images[0] as Bitmap);
      MRP_PDMSubstitutesView._iconActualSubstitute = ImageHelper.BitmapToIcon(this.imagesTreeview.Images[1] as Bitmap);
      MRP_PDMSubstitutesView._iconSubstitute = ImageHelper.BitmapToIcon(this.imagesTreeview.Images[2] as Bitmap);
    }
    this.grid.ImageList = Images32x16_Cache.GetImageList32x16();
    this._imgView = this._images != null ? this._images.ImageIndex("imgSubstitutes.PDM") : -1;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      SubstituteObjects.InitStaticFields(sessionKeeper.Session);
      this._advAttributes = new List<NodeColumnID>(SubstituteObjects.AttrColumns.Count);
      for (int index = 0; index < SubstituteObjects.AttrColumns.Count; ++index)
        this._advAttributes.Add(new NodeColumnID(SubstituteObjects.AttrColumns[index].AttributeID, SubstituteObjects.AttrColumns[index].AttributeSource));
      this.remarks = new Dictionary<long, string>();
    }
    this.ToolbarRendererChanged((object) (ServicesManager.GetService(typeof (BarManager)) as BarManager), EventArgs.Empty);
  }

  /// <summary>
  /// Пришло событие "Изменился рендерер панелей инструментов"
  /// </summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  protected override void ToolbarRendererChanged(object sender, EventArgs e)
  {
    this.toolBarGrid.Renderer = (sender as BarManager).Renderer;
  }

  /// <summary>Заголовок закладки</summary>
  public override string Caption
  {
    [DebuggerStepThrough] get => LocalizationHolder.rm.GetString("MRP_4");
  }

  /// <summary>Порядковый номер закладки</summary>
  public override int OrderID => -8;

  /// <summary>Инициализировать закладку</summary>
  /// <param name="items">Коллекция выделенных элементов пространства навигации</param>
  /// <param name="provider">Контейнер сервисов</param>
  public override void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    base.Initialize(items, provider);
  }

  /// <summary>
  /// Активировать закладку (чтение из базы данных, загрузка информации и т.п.)
  /// </summary>
  /// <param name="previousView">Предыдущая закладка</param>
  public override void Activate(IView previousView)
  {
    IViewState service = this._services != null ? this._services.GetService(typeof (IViewState)) as IViewState : (IViewState) null;
    if (service != null)
    {
      long viewState = (long) service.ViewState;
    }
    this.LoadViewData();
  }

  /// <summary>Деактивировать закладку</summary>
  /// <param name="nextView">Следующая закладка</param>
  public override void Deactivate(IView nextView)
  {
  }

  /// <summary>Инициализация ресурсов закладки</summary>
  public override void InitViewResources()
  {
    base.InitViewResources();
    this._imgView = this._images != null ? this._images.ImageIndex("MRP.imgSubstitutes") : -1;
  }

  /// <summary>Забрать изменения из закладки в контейнер настроек</summary>
  protected override void CaptureChanges()
  {
    this._items.GetItemData(0, typeof (IDBTypedObjectID));
    IDBRelationID itemData = this._items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    ManufactureOrderHolder service = this.Services != null ? this.Services.GetService(typeof (ManufactureOrderHolder)) as ManufactureOrderHolder : (ManufactureOrderHolder) null;
    SubstitutesItemSettings relationSetting = service == null || itemData == null ? (SubstitutesItemSettings) null : service.GetRelationSetting(itemData.Value, typeof (SubstitutesItemSettings)) as SubstitutesItemSettings;
    if (relationSetting != null)
      relationSetting.ActualSubstitutes = new Dictionary<long, long>((IDictionary<long, long>) this.actualSubstitutes);
    else if (itemData.Value != 0L && itemData.Value != -1L)
      service.SetRelationSetting(itemData.Value, (IOrderItemSetting) new SubstitutesItemSettings()
      {
        ActualSubstitutes = new Dictionary<long, long>((IDictionary<long, long>) this.actualSubstitutes)
      });
    this.RaiseOnChanged();
  }

  /// <summary>
  /// Заполнить элементы управления закладки данными, полученными в методе Initialize
  /// </summary>
  protected override void LoadViewData()
  {
    if (this._items == null || this._items.Count == 0)
    {
      this.Clear();
    }
    else
    {
      ManufactureOrderHolder service = this.Services != null ? this.Services.GetService(typeof (ManufactureOrderHolder)) as ManufactureOrderHolder : (ManufactureOrderHolder) null;
      IDBTypedObjectID itemData1 = this._items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
      IDBRelationID itemData2 = this._items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
      if (this.objRelTypeLinkID == null || this.substitutes == null || itemData1 == null || this.objRelTypeLinkID.Item1 != itemData1.ObjectType || itemData2 == null || this.objRelTypeLinkID.Item2 != itemData2.RelationType || this.objRelTypeLinkID.Item3 != itemData2.Value)
      {
        this.Clear();
        this.substitutes = (SubstituteObjects) null;
        if (itemData1 != null && MetaDataHelper.HasObjectTypeSubstRelTypes(itemData1.ObjectType) && itemData2 != null && itemData2.Value != 0L && service != null)
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            if (sessionKeeper.Session.GetCustomService(typeof (ISubstitutesService)) is ISubstitutesService customService)
              this.substitutes = customService.LoadSubstitutes(sessionKeeper.Session.SessionGUID, service.FiltrationSettings.OwnerID, service.CompositionContexts, itemData1.ObjectID, itemData2.RelationType);
          }
        }
      }
      this.substitutes = this.substitutes ?? new SubstituteObjects();
      this.substitutes.RebuildGroups();
      this.substitutes.Groups.Sort();
      SubstitutesItemSettings relationSetting = service == null || itemData2 == null ? (SubstitutesItemSettings) null : service.GetRelationSetting(itemData2.Value, typeof (SubstitutesItemSettings)) as SubstitutesItemSettings;
      if (this.objRelTypeLinkID == null)
      {
        this.substitutes.Groups.Sort();
        List<long> groups = this.substitutes.Groups;
        for (int index = 0; index < groups.Count; ++index)
          this.actualSubstitutes[groups[index]] = 0L;
        if (relationSetting != null)
        {
          if (relationSetting.ActualSubstitutes == null || relationSetting.ActualSubstitutes.Count != this.actualSubstitutes.Count)
            relationSetting.ActualSubstitutes = new Dictionary<long, long>((IDictionary<long, long>) this.actualSubstitutes);
          else
            this.actualSubstitutes = new Dictionary<long, long>((IDictionary<long, long>) relationSetting.ActualSubstitutes);
        }
        else if (itemData2.Value != 0L && itemData2.Value != -1L)
          service.SetRelationSetting(itemData2.Value, (IOrderItemSetting) new SubstitutesItemSettings()
          {
            ActualSubstitutes = new Dictionary<long, long>((IDictionary<long, long>) this.actualSubstitutes)
          });
        this.RebuildGrid();
        this.objRelTypeLinkID = new Tuple<int, int, long>(itemData1 != null ? itemData1.ObjectType : -1, itemData2 != null ? itemData2.RelationType : -1, itemData2 != null ? itemData2.Value : 0L);
      }
      this.UpdateControls();
    }
  }

  /// <summary>Обновить значки в гриде допустимых замен</summary>
  private void RefreshGridIcons()
  {
    for (int index = 0; index < this.grid.Rows.Count; ++index)
    {
      iGRow row = this.grid.Rows[index];
      if (row.Level == 2)
      {
        long int64Value1 = DataSetProcessor.GetInt64Value(row.Cells["colGroupNo"].Value, 0L);
        long int64Value2 = DataSetProcessor.GetInt64Value(row.Cells["colSubGroup"].Value, 0L);
        row.RowTextCell.ImageIndex = !this.actualSubstitutes.ContainsKey(int64Value1) || this.actualSubstitutes[int64Value1] != int64Value2 ? this._images.ImageIndex("imgObjects.Substitute") : this._images.ImageIndex("imgObjects.ActualSubstitute");
      }
    }
  }

  /// <summary>Выполнить очистку элементов управления в закладке</summary>
  protected override void Clear()
  {
    base.Clear();
    this.objRelTypeLinkID = (Tuple<int, int, long>) null;
    this.actualSubstitutes.Clear();
  }

  /// <summary>Управление контролами на закладке</summary>
  protected override void UpdateControls()
  {
    base.UpdateControls();
    this.btCollapseAll.Enabled = this.grid.Rows.Count > 0;
    this.btExpandAll.Enabled = this.btCollapseAll.Enabled;
    int index = this.grid.SelectedCells.Count > 0 ? this.grid.SelectedCells[0].RowIndex : -1;
    iGRow row = index < 0 || index >= this.grid.Rows.Count ? (iGRow) null : this.grid.Rows[index];
    long int64Value1 = row != null ? DataSetProcessor.GetInt64Value(row.Cells["colGroupNo"].Value, 0L) : 0L;
    long int64Value2 = row != null ? DataSetProcessor.GetInt64Value(row.Cells["colSubGroup"].Value, 0L) : 0L;
    this.btActualize.Enabled = row != null && row.Level == 2 && this.actualSubstitutes.ContainsKey(int64Value1) && this.actualSubstitutes[int64Value1] != int64Value2;
    this.btRefresh.Enabled = true;
    this.panelHint.Visible = !MRP_PDMSubstitutesView.hiddenHintPanel;
  }

  /// <summary>
  /// Сделать заменители, начинающиеся с указанной строки, актуальными
  /// </summary>
  /// <param name="rowIdx">Номер строки, с которой начинается очередной заменитель в группе</param>
  protected virtual void ActualizeSubGroup(int rowIdx)
  {
    iGRow row = this.grid.Rows[rowIdx];
    if (row.Level != 2)
      return;
    long int64Value1 = DataSetProcessor.GetInt64Value(row.Cells["colGroupNo"].Value, 0L);
    long int64Value2 = DataSetProcessor.GetInt64Value(row.Cells["colSubGroup"].Value, 0L);
    if (!this.actualSubstitutes.ContainsKey(int64Value1) || this.actualSubstitutes[int64Value1] == int64Value2)
      return;
    this.actualSubstitutes[int64Value1] = int64Value2;
    this.RefreshGridIcons();
    this.CaptureChanges();
    this.UpdateControls();
  }

  /// <summary>Получить ссылку на строку для группы допустимых замен</summary>
  /// <param name="group">Номер группы</param>
  /// <param name="autoCreate">true - автоматически создать такую строку, если её нет в гриде</param>
  /// <returns>Ссылка на строку или null</returns>
  protected virtual iGRow GetGroupRow(long group, bool autoCreate)
  {
    if (this.substitutes == null || this.substitutes.Groups.IndexOf(group) < 0)
      return (iGRow) null;
    for (int index = 0; index < this.grid.Rows.Count; ++index)
    {
      iGRow row = this.grid.Rows[index];
      if (row.Type != iGRowType.Normal && row.Level == 1 && DataSetProcessor.GetInt64Value(row.Tag, 0L) == group)
        return row;
    }
    if (!autoCreate)
      return (iGRow) null;
    iGRow groupRow = this.grid.Rows.Add();
    groupRow.Tag = (object) group;
    groupRow.Type = iGRowType.AutoGroupRow;
    groupRow.Level = 1;
    groupRow.RowTextCell.Value = (object) this.substitutes.GetSubstGroupName(group);
    groupRow.RowTextCell.Style = (iGCellStyle) this.groupRowsLevel1;
    groupRow.RowTextCell.ImageList = this._images.ImageList;
    groupRow.TreeButton = iGTreeButtonState.Visible;
    groupRow.Expanded = true;
    groupRow.Cells["colPrjLinkID"].Value = (object) 0L;
    groupRow.Cells["colGroupNo"].Value = (object) group;
    groupRow.Cells["colSubGroup"].Value = (object) 0;
    for (int colIndex = 0; colIndex < groupRow.Cells.Count; ++colIndex)
      groupRow.Cells[colIndex].Style = (iGCellStyle) this.groupRowsLevel1;
    return groupRow;
  }

  /// <summary>Получить ссылку на строку для подгруппы заменителя</summary>
  /// <param name="group">Номер группы</param>
  /// <param name="subGroup">Номер заменителя в группе</param>
  /// <param name="autoCreate">true - автоматически создать такую строку, если её нет в гриде</param>
  /// <returns>Ссылка на строку или null</returns>
  protected virtual iGRow GetSubGroupRow(long group, long subGroup, bool autoCreate)
  {
    if (this.substitutes == null || this.substitutes.Groups.IndexOf(group) < 0)
      return (iGRow) null;
    List<List<long>> longListList = this.substitutes.Items[group];
    if (subGroup < 0L || subGroup >= (long) longListList.Count)
      return (iGRow) null;
    iGRow groupRow = this.GetGroupRow(group, autoCreate);
    if (groupRow == null)
      return (iGRow) null;
    int num = groupRow.Index + 1;
    while (num < this.grid.Rows.Count)
    {
      iGRow row = this.grid.Rows[num];
      num = row.Index;
      if (row.Type == iGRowType.Normal || row.Level != 1)
      {
        if (row.Type != iGRowType.Normal && row.Level == 2)
        {
          if (DataSetProcessor.GetInt64Value(row.Tag, 0L) == subGroup)
            return row;
          ++num;
        }
        else
          ++num;
      }
      else
        break;
    }
    if (!autoCreate)
      return (iGRow) null;
    iGRow subGroupRow = this.grid.Rows.Insert(num);
    subGroupRow.Tag = (object) subGroup;
    subGroupRow.Type = iGRowType.ManualGroupRow;
    subGroupRow.Level = 2;
    subGroupRow.RowTextCell.Style = (iGCellStyle) this.groupRowsLevel2;
    subGroupRow.RowTextCell.Value = (object) $"{this.substitutes.GetSubstGroupName(group)}.{subGroup}";
    subGroupRow.RowTextCell.ImageList = this._images.ImageList;
    subGroupRow.TreeButton = iGTreeButtonState.Hidden;
    subGroupRow.Expanded = true;
    subGroupRow.Cells["colPrjLinkID"].Value = (object) 0L;
    subGroupRow.Cells["colGroupNo"].Value = (object) group;
    subGroupRow.Cells["colSubGroup"].Value = (object) subGroup;
    subGroupRow.RowTextCell.ImageIndex = !this.actualSubstitutes.ContainsKey(group) || this.actualSubstitutes[group] != subGroup ? this._images.ImageIndex("imgObjects.Substitute") : this._images.ImageIndex("imgObjects.ActualSubstitute");
    for (int colIndex = 0; colIndex < subGroupRow.Cells.Count; ++colIndex)
      subGroupRow.Cells[colIndex].Style = (iGCellStyle) this.groupRowsLevel2;
    return subGroupRow;
  }

  /// <summary>Получить ссылку на строку для указанной связи</summary>
  /// <param name="group">Номер группы</param>
  /// <param name="subGroup">Номер заменителя в группе</param>
  /// <param name="afterRow">Индекс строки, после которой надо добавлять новую строку</param>
  /// <param name="prjLinkID">Идентификатор связи</param>
  /// <returns>Ссылка на строку или null</returns>
  protected virtual iGRow GetRelationRow(long group, long subGroup, int afterRow, long prjLinkID)
  {
    if (this.substitutes == null || prjLinkID == 0L)
      return (iGRow) null;
    if (afterRow >= this.grid.Rows.Count)
      afterRow = this.grid.Rows.Count - 1;
    iGRow relationRow = afterRow >= 0 ? this.grid.Rows.Insert(afterRow + 1) : this.grid.Rows.Add();
    relationRow.Tag = (object) prjLinkID;
    relationRow.Type = iGRowType.Normal;
    relationRow.Level = 3;
    relationRow.TreeButton = iGTreeButtonState.Hidden;
    relationRow.Cells["colCAPTION"].Value = this.substitutes.RelationAttributes[prjLinkID, -50];
    relationRow.Cells["colCAPTION"].Style = (iGCellStyle) this.rowsLevel3;
    relationRow.Cells["colCAPTION"].ImageList = Images32x16_Cache.GetImageList32x16();
    relationRow.Cells["colCAPTION"].Style.ImageList = Images32x16_Cache.GetImageList32x16();
    DataSetProcessor.GetInt32Value(this.substitutes.RelationAttributes[prjLinkID, -7], -1);
    relationRow.Cells["colCAPTION"].ImageIndex = -1;
    relationRow.Cells["colPOSITION"].Value = this.substitutes.RelationAttributes[prjLinkID, MetaDataHelper.GetAttributeTypeID("cad00270-306c-11d8-b4e9-00304f19f545")];
    relationRow.Cells["colPOSITION"].Style = (iGCellStyle) this.rowsLevel3Right;
    relationRow.Cells["colQUANTITY"].Value = this.substitutes.RelationAttributes[prjLinkID, MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545")];
    relationRow.Cells["colQUANTITY"].Style = (iGCellStyle) this.rowsLevel3Right;
    if (this.pdmSubstitutesService != null)
    {
      relationRow.Cells["colNOTE"].Value = this.remarks.ContainsKey(prjLinkID) ? (object) this.remarks[prjLinkID] : (object) string.Empty;
      relationRow.Cells["colNOTE"].Style = (iGCellStyle) this.rowsLevel3;
      relationRow.Cells["colNOTE"].ImageList = this._images.ImageList;
      if (this.substitutes.IsRelationDesignerActualVariant(prjLinkID))
        relationRow.Cells["colNOTE"].ImageIndex = this._images.ImageIndex("imgMakeActualSubstitute.PDM");
      relationRow.AutoHeight();
    }
    relationRow.Cells["colPrjLinkID"].Value = (object) 0L;
    relationRow.Cells["colGroupNo"].Value = (object) group;
    relationRow.Cells["colSubGroup"].Value = (object) subGroup;
    return relationRow;
  }

  /// <summary>Перестроить грид</summary>
  protected virtual void RebuildGrid()
  {
    if (this.substitutes == null)
      return;
    bool fdisableTreeEvents = this.FDisableTreeEvents;
    try
    {
      this.FDisableTreeEvents = true;
      if (this._substsRemarks != null)
        this.remarks = this._substsRemarks.CalcSubstituteRemarks(this._substsSettings, this.substitutes);
      this.grid.Rows.Clear();
      for (int index1 = 0; index1 < this.substitutes.Groups.Count; ++index1)
      {
        this.GetGroupRow(this.substitutes.Groups[index1], true);
        List<List<long>> substitute = this.substitutes[this.substitutes.Groups[index1]];
        for (int index2 = 0; index2 < substitute.Count; ++index2)
        {
          iGRow subGroupRow = this.GetSubGroupRow(this.substitutes.Groups[index1], (long) index2, true);
          for (int index3 = 0; index3 < substitute[index2].Count; ++index3)
            this.GetRelationRow(this.substitutes.Groups[index1], (long) index2, subGroupRow.Index + index3, substitute[index2][index3]);
        }
      }
    }
    finally
    {
      this.FDisableTreeEvents = fdisableTreeEvents;
    }
  }

  /// <summary>
  /// Обработчик события "Перед подтверждением редактирования"
  /// </summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void grid_BeforeCommitEdit(object sender, iGBeforeCommitEditEventArgs e)
  {
  }

  /// <summary>Обработчик события "Подтверждение редактирования"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void grid_AfterCommitEdit(object sender, iGAfterCommitEditEventArgs e)
  {
  }

  /// <summary>Обработчик события "Отмена редактирования"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void grid_CancelEdit(object sender, iGCancelEditEventArgs e)
  {
  }

  /// <summary>
  /// Обработчик события "Изменились выделенные строки в гриде"
  /// </summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void grid_SelectionChanged(object sender, EventArgs e) => this.UpdateControls();

  /// <summary>Обработчик события "Двойной клик в ячейке"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void grid_CellDoubleClick(object sender, iGCellDoubleClickEventArgs e)
  {
    this.ActualizeSubGroup(e.RowIndex);
  }

  /// <summary>Обработчик события "Нажатие клавиши в ячейке"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void grid_KeyUp(object sender, KeyEventArgs e)
  {
    if (e.KeyCode != Keys.Space)
      return;
    int rowIdx = this.grid.SelectedCells.Count > 0 ? this.grid.SelectedCells[0].RowIndex : -1;
    if (rowIdx < 0 || rowIdx >= this.grid.Rows.Count)
      return;
    this.ActualizeSubGroup(rowIdx);
  }

  /// <summary>Обработчик события "Актуализировать заменитель"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoActualize(object sender, EventArgs e)
  {
    int rowIdx = this.grid.SelectedCells.Count > 0 ? this.grid.SelectedCells[0].RowIndex : -1;
    if (rowIdx < 0 || rowIdx >= this.grid.Rows.Count)
      return;
    this.ActualizeSubGroup(rowIdx);
  }

  /// <summary>Обработчик события "Динамический шрифт"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void grid_DynamicFont(object sender, iGDynamicFontEventArgs e)
  {
  }

  /// <summary>Обработчик события "Свернуть всё"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoCollapseAll(object sender, EventArgs e)
  {
    this.grid.PerformAction(iGActions.CollapseAll);
    this.UpdateControls();
  }

  /// <summary>Обработчик события "Раскрыть группы"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoCollapseSubGroups(object sender, EventArgs e)
  {
    this.grid.PerformAction(iGActions.CollapseAll);
    for (int index = 0; index < this.grid.Rows.Count; ++index)
    {
      if (this.grid.Rows[index].Level == 1)
        this.grid.Rows[index].Expanded = true;
    }
    this.UpdateControls();
  }

  /// <summary>Обработчик события "Раскрыть всё"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoExpandAll(object sender, EventArgs e)
  {
    this.grid.PerformAction(iGActions.ExpandAll);
    this.UpdateControls();
  }

  /// <summary>Нажата кнопка "Скрыть подсказку"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы отправителя</param>
  private void DoHideHint(object sender, EventArgs e)
  {
    this.panelHint.Visible = false;
    MRP_PDMSubstitutesView.hiddenHintPanel = true;
  }

  /// <summary>Удаление используемых ресурсов</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (BarManager)) is BarManager)
      this.toolBarGrid.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
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
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MRP_PDMSubstitutesView));
    iGColPattern iGcolPattern1 = new iGColPattern();
    iGColPattern iGcolPattern2 = new iGColPattern();
    iGColPattern iGcolPattern3 = new iGColPattern();
    iGColPattern iGcolPattern4 = new iGColPattern();
    iGColPattern iGcolPattern5 = new iGColPattern();
    iGColPattern iGcolPattern6 = new iGColPattern();
    iGColPattern iGcolPattern7 = new iGColPattern();
    this.gridCol0CellStyle = new iGCellStyle(true);
    this.gridCol0ColHdrStyle = new iGColHdrStyle(true);
    this.gridCol1CellStyle = new iGCellStyle(true);
    this.gridCol1ColHdrStyle = new iGColHdrStyle(true);
    this.gridCol2CellStyle = new iGCellStyle(true);
    this.gridCol2ColHdrStyle = new iGColHdrStyle(true);
    this.gridCol3CellStyle = new iGCellStyle(true);
    this.gridCol3ColHdrStyle = new iGColHdrStyle(true);
    this.gridCol6CellStyle = new iGCellStyle(true);
    this.gridCol6ColHdrStyle = new iGColHdrStyle(true);
    this.gridCol4CellStyle = new iGCellStyle(true);
    this.gridCol4ColHdrStyle = new iGColHdrStyle(true);
    this.gridCol5CellStyle = new iGCellStyle(true);
    this.gridCol5ColHdrStyle = new iGColHdrStyle(true);
    this.columnMain = new Column();
    this.columnPos = new Column();
    this.columnCount = new Column();
    this.columnNote = new Column();
    this.cellEditor = new CellEditor();
    this.imagesTreeview = new ImageList(this.components);
    this.grid = new iGrid();
    this.groupRowsLevel1 = new iGCellStyleDesign();
    this.groupRowsLevel2 = new iGCellStyleDesign();
    this.rowsLevel3 = new iGCellStyleDesign();
    this.rowsLevel3Right = new iGCellStyleDesign();
    this.imagesToolbars = new ImageList(this.components);
    this.toolBarGrid = new Intermech.Bars.ToolBar();
    this.btExpandAll = new ButtonItem();
    this.btCollapseAll = new ButtonItem();
    this.btActualize = new ButtonItem();
    this.btRefresh = new ButtonItem();
    this.panelHint = new Panel();
    this.edHint = new RichTextBox();
    this.btnHideHint = new Button();
    this.toolTips = new ToolTip(this.components);
    ((ISupportInitialize) this.grid).BeginInit();
    this.panelHint.SuspendLayout();
    this.SuspendLayout();
    this.gridCol0CellStyle.TypeFlags = iGCellTypeFlags.CheckThreeState;
    this.gridCol0ColHdrStyle.ImageAlign = iGContentAlignment.MiddleLeft;
    this.gridCol0ColHdrStyle.TextAlign = iGContentAlignment.MiddleLeft;
    this.gridCol1ColHdrStyle.ImageAlign = iGContentAlignment.MiddleLeft;
    this.gridCol1ColHdrStyle.TextAlign = iGContentAlignment.MiddleLeft;
    this.gridCol2ColHdrStyle.ImageAlign = iGContentAlignment.MiddleLeft;
    this.gridCol2ColHdrStyle.TextAlign = iGContentAlignment.MiddleLeft;
    this.gridCol3ColHdrStyle.ImageAlign = iGContentAlignment.MiddleLeft;
    this.gridCol3ColHdrStyle.TextAlign = iGContentAlignment.MiddleLeft;
    this.columnMain.AutoSizePolicy = ColumnAutoSizePolicy.Manual;
    componentResourceManager.ApplyResources((object) this.columnMain, "columnMain");
    this.columnMain.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("columnMain.HeaderStyle.HorzAlignment");
    this.columnMain.Movable = false;
    this.columnMain.Name = "columnMain";
    this.columnMain.Sortable = false;
    this.columnMain.SortDirection = ListSortDirection.Ascending;
    this.columnPos.AutoSizePolicy = ColumnAutoSizePolicy.Manual;
    componentResourceManager.ApplyResources((object) this.columnPos, "columnPos");
    this.columnPos.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("columnPos.HeaderStyle.HorzAlignment");
    this.columnPos.Movable = false;
    this.columnPos.Name = "columnPos";
    this.columnPos.Sortable = false;
    this.columnPos.SortDirection = ListSortDirection.Ascending;
    this.columnCount.AutoSizePolicy = ColumnAutoSizePolicy.Manual;
    componentResourceManager.ApplyResources((object) this.columnCount, "columnCount");
    this.columnCount.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("columnCount.HeaderStyle.HorzAlignment");
    this.columnCount.Movable = false;
    this.columnCount.Name = "columnCount";
    this.columnCount.Sortable = false;
    this.columnCount.SortDirection = ListSortDirection.Ascending;
    this.columnNote.AutoSizePolicy = ColumnAutoSizePolicy.AutoSize;
    componentResourceManager.ApplyResources((object) this.columnNote, "columnNote");
    this.columnNote.CellStyle.WordWrap = (bool) componentResourceManager.GetObject("columnNote.CellStyle.WordWrap");
    this.columnNote.HeaderStyle.HorzAlignment = (StringAlignment) componentResourceManager.GetObject("columnNote.HeaderStyle.HorzAlignment");
    this.columnNote.Movable = false;
    this.columnNote.Name = "columnNote";
    this.columnNote.Sortable = false;
    this.columnNote.SortDirection = ListSortDirection.Ascending;
    this.cellEditor.CellAlignment = ContentAlignment.MiddleLeft;
    this.cellEditor.Control = (Control) null;
    this.cellEditor.DisplayMode = CellEditorDisplayMode.OnEdit;
    this.imagesTreeview.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesTreeview.ImageStream");
    this.imagesTreeview.TransparentColor = Color.Transparent;
    this.imagesTreeview.Images.SetKeyName(0, "group.ico");
    this.imagesTreeview.Images.SetKeyName(1, "main.ico");
    this.imagesTreeview.Images.SetKeyName(2, "alt.ico");
    this.grid.AllowDrop = true;
    this.grid.AutoResizeCols = true;
    this.grid.AutoWidthColMode = iGAutoWidthColMode.Cells;
    this.grid.BackColorEvenRows = SystemColors.Window;
    this.grid.BackColorOddRows = SystemColors.Window;
    iGcolPattern1.AllowGrouping = false;
    iGcolPattern1.AllowMoving = false;
    iGcolPattern1.CellStyle = this.gridCol0CellStyle;
    iGcolPattern1.ColHdrStyle = this.gridCol0ColHdrStyle;
    componentResourceManager.ApplyResources((object) iGcolPattern1, "iGColPattern1");
    iGcolPattern2.AllowGrouping = false;
    iGcolPattern2.AllowMoving = false;
    iGcolPattern2.AllowSizing = false;
    iGcolPattern2.CellStyle = this.gridCol1CellStyle;
    iGcolPattern2.ColHdrStyle = this.gridCol1ColHdrStyle;
    componentResourceManager.ApplyResources((object) iGcolPattern2, "iGColPattern2");
    iGcolPattern3.AllowGrouping = false;
    iGcolPattern3.AllowMoving = false;
    iGcolPattern3.AllowSizing = false;
    iGcolPattern3.CellStyle = this.gridCol2CellStyle;
    iGcolPattern3.ColHdrStyle = this.gridCol2ColHdrStyle;
    componentResourceManager.ApplyResources((object) iGcolPattern3, "iGColPattern3");
    iGcolPattern4.AllowGrouping = false;
    iGcolPattern4.AllowMoving = false;
    iGcolPattern4.CellStyle = this.gridCol3CellStyle;
    iGcolPattern4.ColHdrStyle = this.gridCol3ColHdrStyle;
    componentResourceManager.ApplyResources((object) iGcolPattern4, "iGColPattern4");
    iGcolPattern5.AllowGrouping = false;
    iGcolPattern5.AllowMoving = false;
    iGcolPattern5.AllowSizing = false;
    iGcolPattern5.CellStyle = this.gridCol6CellStyle;
    iGcolPattern5.ColHdrStyle = this.gridCol6ColHdrStyle;
    componentResourceManager.ApplyResources((object) iGcolPattern5, "iGColPattern5");
    iGcolPattern5.Visible = false;
    iGcolPattern6.AllowGrouping = false;
    iGcolPattern6.AllowMoving = false;
    iGcolPattern6.AllowSizing = false;
    iGcolPattern6.CellStyle = this.gridCol4CellStyle;
    iGcolPattern6.ColHdrStyle = this.gridCol4ColHdrStyle;
    componentResourceManager.ApplyResources((object) iGcolPattern6, "iGColPattern6");
    iGcolPattern6.Visible = false;
    iGcolPattern7.AllowGrouping = false;
    iGcolPattern7.AllowMoving = false;
    iGcolPattern7.AllowSizing = false;
    iGcolPattern7.CellStyle = this.gridCol5CellStyle;
    iGcolPattern7.ColHdrStyle = this.gridCol5ColHdrStyle;
    componentResourceManager.ApplyResources((object) iGcolPattern7, "iGColPattern7");
    iGcolPattern7.Visible = false;
    this.grid.Cols.AddRange(new iGColPattern[7]
    {
      iGcolPattern1,
      iGcolPattern2,
      iGcolPattern3,
      iGcolPattern4,
      iGcolPattern5,
      iGcolPattern6,
      iGcolPattern7
    });
    this.grid.Cursor = Cursors.Default;
    this.grid.DefaultAutoGroupRow.Height = 21;
    this.grid.DefaultCol.AllowGrouping = false;
    this.grid.DefaultCol.AllowMoving = false;
    this.grid.DefaultCol.Width = (int) componentResourceManager.GetObject("resource.Width");
    this.grid.DefaultRow.Height = (int) componentResourceManager.GetObject("resource.Height");
    this.grid.DefaultRow.NormalCellHeight = (int) componentResourceManager.GetObject("resource.NormalCellHeight");
    componentResourceManager.ApplyResources((object) this.grid, "grid");
    this.grid.GroupBox.BackColor = SystemColors.AppWorkspace;
    this.grid.GroupBox.HintBackColor = SystemColors.AppWorkspace;
    this.grid.GroupBox.HintForeColor = SystemColors.ControlText;
    this.grid.GroupBox.Text = componentResourceManager.GetString("grid.GroupBox.Text");
    this.grid.GroupRowLevelStyles = new iGCellStyle[2]
    {
      (iGCellStyle) this.groupRowsLevel1,
      (iGCellStyle) this.groupRowsLevel2
    };
    this.grid.Header.AutoHeightFlags = iGHdrAutoHeightFlags.OnAddCol | iGHdrAutoHeightFlags.OnRemoveCol | iGHdrAutoHeightFlags.OnShowCol | iGHdrAutoHeightFlags.OnContentsChange | iGHdrAutoHeightFlags.OnThemeChange | iGHdrAutoHeightFlags.OnResizeCol;
    this.grid.Header.Height = (int) componentResourceManager.GetObject("grid.Header.Height");
    this.grid.HighlightBackColorNoFocus = SystemColors.ControlDark;
    this.grid.HighlightForeColorNoFocus = SystemColors.HighlightText;
    this.grid.HotTracking = false;
    this.grid.LayoutObject.Flags = iGLayoutFlags.Grouping | iGLayoutFlags.Sorting | iGLayoutFlags.ColVisibility | iGLayoutFlags.ColWidth | iGLayoutFlags.ColOrder;
    this.grid.Name = "grid";
    this.grid.PageCapacity = 500;
    this.grid.PressedMouseMoveMode = iGPressedMouseMoveMode.Normal;
    this.grid.ProcessTab = false;
    this.grid.ReadOnly = true;
    this.grid.RowMode = true;
    this.grid.RowTextStartColNear = 211;
    this.grid.ShowControlsInAllCells = false;
    this.grid.SilentValidation = true;
    this.grid.SortByLevels = true;
    this.grid.CancelEdit += new iGCancelEditEventHandler(this.grid_CancelEdit);
    this.grid.KeyUp += new KeyEventHandler(this.grid_KeyUp);
    this.grid.SelectionChanged += new EventHandler(this.grid_SelectionChanged);
    this.grid.BeforeCommitEdit += new iGBeforeCommitEditEventHandler(this.grid_BeforeCommitEdit);
    this.grid.CellDoubleClick += new iGCellDoubleClickEventHandler(this.grid_CellDoubleClick);
    this.grid.DynamicFont += new iGDynamicFontEventHandler(this.grid_DynamicFont);
    this.grid.AfterCommitEdit += new iGAfterCommitEditEventHandler(this.grid_AfterCommitEdit);
    this.groupRowsLevel1.BackColor = SystemColors.ControlLight;
    this.groupRowsLevel1.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
    this.groupRowsLevel1.ReadOnly = iGBool.True;
    this.groupRowsLevel1.TextAlign = iGContentAlignment.MiddleLeft;
    this.groupRowsLevel2.BackColor = SystemColors.Control;
    this.groupRowsLevel2.ReadOnly = iGBool.True;
    this.groupRowsLevel2.TextAlign = iGContentAlignment.MiddleLeft;
    this.groupRowsLevel2.TypeFlags = iGCellTypeFlags.CheckThreeState;
    this.rowsLevel3.ReadOnly = iGBool.True;
    this.rowsLevel3.TextAlign = iGContentAlignment.MiddleLeft;
    this.rowsLevel3.TextFormatFlags = iGStringFormatFlags.WordWrap;
    this.rowsLevel3Right.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
    this.rowsLevel3Right.ReadOnly = iGBool.True;
    this.rowsLevel3Right.TextAlign = iGContentAlignment.MiddleRight;
    this.rowsLevel3Right.TextFormatFlags = iGStringFormatFlags.WordWrap;
    this.imagesToolbars.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolbars.ImageStream");
    this.imagesToolbars.TransparentColor = Color.Transparent;
    this.imagesToolbars.Images.SetKeyName(0, "Collapse.ico");
    this.imagesToolbars.Images.SetKeyName(1, "Expand.ico");
    this.imagesToolbars.Images.SetKeyName(2, "actual.ico");
    this.imagesToolbars.Images.SetKeyName(3, "refresh.png");
    this.toolBarGrid.AddRemoveButtonsVisible = false;
    this.toolBarGrid.AllowHorizontalDock = false;
    this.toolBarGrid.DockLine = 3;
    this.toolBarGrid.DrawActionsButton = false;
    this.toolBarGrid.FullMenus = true;
    this.toolBarGrid.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarGrid.Hidden = false;
    this.toolBarGrid.ImageList = this.imagesToolbars;
    this.toolBarGrid.Items.AddRange(new ToolbarItemBase[4]
    {
      (ToolbarItemBase) this.btExpandAll,
      (ToolbarItemBase) this.btCollapseAll,
      (ToolbarItemBase) this.btActualize,
      (ToolbarItemBase) this.btRefresh
    });
    componentResourceManager.ApplyResources((object) this.toolBarGrid, "toolBarGrid");
    this.toolBarGrid.MinimumFloatingSize = new Size(250, 30);
    this.toolBarGrid.Name = "toolBarGrid";
    this.toolBarGrid.Overflow = ToolBarOverflow.Wrap;
    this.toolBarGrid.Stretch = true;
    this.toolBarGrid.Tearable = false;
    componentResourceManager.ApplyResources((object) this.btExpandAll, "btExpandAll");
    this.btExpandAll.ImageIndex = 1;
    this.btExpandAll.ShowText = true;
    this.btExpandAll.Click += new EventHandler(this.DoExpandAll);
    this.btCollapseAll.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btCollapseAll, "btCollapseAll");
    this.btCollapseAll.ImageIndex = 0;
    this.btCollapseAll.ShowText = true;
    this.btCollapseAll.Click += new EventHandler(this.DoCollapseAll);
    this.btActualize.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btActualize, "btActualize");
    this.btActualize.ImageIndex = 2;
    this.btActualize.ShowText = true;
    this.btActualize.Click += new EventHandler(this.DoActualize);
    this.btRefresh.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btRefresh, "btRefresh");
    this.btRefresh.ImageIndex = 3;
    this.btRefresh.ShowText = true;
    this.btRefresh.Visible = false;
    this.panelHint.Controls.Add((Control) this.edHint);
    this.panelHint.Controls.Add((Control) this.btnHideHint);
    componentResourceManager.ApplyResources((object) this.panelHint, "panelHint");
    this.panelHint.Name = "panelHint";
    componentResourceManager.ApplyResources((object) this.edHint, "edHint");
    this.edHint.BackColor = SystemColors.Control;
    this.edHint.Cursor = Cursors.Arrow;
    this.edHint.DetectUrls = false;
    this.edHint.Name = "edHint";
    this.edHint.ReadOnly = true;
    this.edHint.ShortcutsEnabled = false;
    componentResourceManager.ApplyResources((object) this.btnHideHint, "btnHideHint");
    this.btnHideHint.Name = "btnHideHint";
    this.btnHideHint.Tag = (object) "0";
    this.toolTips.SetToolTip((Control) this.btnHideHint, componentResourceManager.GetString("btnHideHint.ToolTip"));
    this.btnHideHint.UseVisualStyleBackColor = true;
    this.btnHideHint.Click += new EventHandler(this.DoHideHint);
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.grid);
    this.Controls.Add((Control) this.panelHint);
    this.Controls.Add((Control) this.toolBarGrid);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.MinimumSize = new Size(450, 300);
    this.Name = nameof (MRP_PDMSubstitutesView);
    ((ISupportInitialize) this.grid).EndInit();
    this.panelHint.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
