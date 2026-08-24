// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.MRP_BoughtArticlesView
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Bars;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>Закладка "Покупное изделие"</summary>
/// <summary>Закладка по управлению покупными изделиями</summary>
internal sealed class MRP_BoughtArticlesView : MRP_BaseView
{
  /// <summary>Для регистрации своих категорий;</summary>
  private IGuidMapper _guidMapper;
  /// <summary>Редактируемый объект</summary>
  private IDBTypedObjectID _object;
  /// <summary>Редактируемая связь</summary>
  private IDBRelationID _relation;
  /// <summary>Редактируемые данные</summary>
  private BoughtArticleItemSettings _settings;
  /// <summary>Скрыта ли панель подсказки</summary>
  internal static bool hiddenHintPanel;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Panel panelMain;
  private CheckBox cbBought;
  private Label lbTotal;
  private TextBox edTotal;
  private TextBox edBought;
  private Label lbBought;
  private PictureBox picture;
  private Button btn25pc;
  private Button btn100pc;
  private Button btn75pc;
  private Button btn50pc;
  private Button btnApply;
  private ToolTip toolTips;
  private Button btn0pc;
  private Panel panelHint;
  private RichTextBox edHint;
  private Button btnHideHint;

  /// <summary>Создать экземпляр класса</summary>
  public MRP_BoughtArticlesView()
  {
    this.InitializeComponent();
    this._guidMapper = ServicesManager.GetService(typeof (IGuidMapper)) as IGuidMapper;
    this.ToolbarRendererChanged((object) (ServicesManager.GetService(typeof (BarManager)) as BarManager), EventArgs.Empty);
  }

  /// <summary>
  /// Пришло событие "Изменился рендерер панелей инструментов"
  /// </summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  protected override void ToolbarRendererChanged(object sender, EventArgs e)
  {
    IToolBarRenderer renderer = (sender as BarManager).Renderer;
  }

  /// <summary>Заголовок закладки</summary>
  public override string Caption
  {
    [DebuggerStepThrough] get => LocalizationHolder.rm.GetString("MRP_23");
  }

  /// <summary>Порядковый номер закладки</summary>
  public override int OrderID => -4;

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
    if (this.inEvents)
      return;
    this.LoadViewData();
  }

  /// <summary>Инициализация ресурсов закладки</summary>
  public override void InitViewResources()
  {
    base.InitViewResources();
    this._imgView = this._images != null ? this._images.ImageIndex("MRP.imgBoughtArticle") : -1;
  }

  /// <summary>Выполнить проверку и корректировку настроек</summary>
  private void CheckSettings()
  {
    if (this._settings == null)
      return;
    this._settings.CheckSettings();
    try
    {
      this.inEvents = true;
    }
    finally
    {
      this.inEvents = false;
    }
  }

  /// <summary>Забрать изменения из закладки в контейнер настроек</summary>
  protected override void CaptureChanges() => this.CheckSettings();

  /// <summary>Заполнить поля на страничке информацией из настроек</summary>
  private void FillWithData()
  {
    this.edTotal.Text = this._settings == null || this._settings.SourceQuantity == null ? string.Empty : this._settings.SourceQuantity.Caption;
    this.edBought.Text = this._settings == null || this._settings.BoughtQuantity == null ? string.Empty : this._settings.BoughtQuantity.Caption;
  }

  /// <summary>
  /// Заполнить элементы управления закладки данными, полученными в методе Initialize
  /// </summary>
  protected override void LoadViewData()
  {
    this.Clear();
    if (this._items == null || this._items.Count == 0)
      return;
    bool flag = false;
    this._object = this._items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    this._relation = this._items.GetItemData(0, typeof (IDBRelationID)) as IDBRelationID;
    ManufactureOrderHolder service = this.Services != null ? this.Services.GetService(typeof (ManufactureOrderHolder)) as ManufactureOrderHolder : (ManufactureOrderHolder) null;
    this._settings = service == null || this._relation == null ? (BoughtArticleItemSettings) null : service.GetRelationSetting(this._relation.Value, typeof (BoughtArticleItemSettings)) as BoughtArticleItemSettings;
    if (this._settings == null && this._relation != null)
    {
      flag = true;
      this._settings = new BoughtArticleItemSettings();
      service.SetRelationSetting(this._relation.Value, (IOrderItemSetting) this._settings);
    }
    if (this._relation != null && this._relation.Value != 0L && this._object != null && this._object.ObjectID != 0L)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._object.ObjectID, false);
        IDBRelation relation = sessionKeeper.Session.GetRelation(this._relation.Value, false);
        if (relation == null || dbObject == null)
        {
          this._settings.Clear();
        }
        else
        {
          IDBAttribute attributeById1 = relation.GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545"));
          this._settings.SourceQuantity = attributeById1 != null ? DataSetProcessor.GetMeasuredValue(attributeById1.Value, (MeasuredValue) null) : (MeasuredValue) null;
          if (flag)
          {
            IDBAttribute attributeById2 = dbObject.GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad0038f-306c-11d8-b4e9-00304f19f545"));
            this._settings.IsBoughtArticle = attributeById2 == null ? 1L : DataSetProcessor.GetInt64Value(attributeById2.Value, 1L);
          }
        }
      }
    }
    this.CheckSettings();
    this.FillWithData();
    this.UpdateControls();
  }

  /// <summary>Выполнить очистку элементов управления в закладке</summary>
  protected override void Clear()
  {
    base.Clear();
    this._object = (IDBTypedObjectID) null;
    this._relation = (IDBRelationID) null;
    this._settings = (BoughtArticleItemSettings) null;
    this.FillWithData();
  }

  /// <summary>Управление контролами на закладке</summary>
  protected override void UpdateControls()
  {
    base.UpdateControls();
    bool flag = this._object != null && this.IsInstanceOrParty(this._object.ObjectType);
    this.cbBought.Enabled = this._settings != null && this._settings.SourceQuantity != null && this._settings.SourceQuantity.Value > 0.0 && !flag;
    this.cbBought.Checked = this.cbBought.Enabled && this._settings.IsBoughtArticle == 2L;
    this.edBought.Enabled = this.cbBought.Checked && !flag;
    this.lbBought.Enabled = this.cbBought.Checked && !flag;
    this.btnApply.Enabled = this.cbBought.Checked && !flag;
    this.btn0pc.Enabled = this.cbBought.Checked && !flag;
    this.btn25pc.Enabled = this.cbBought.Checked && !flag;
    this.btn50pc.Enabled = this.cbBought.Checked && !flag;
    this.btn75pc.Enabled = this.cbBought.Checked && !flag;
    this.btn100pc.Enabled = this.cbBought.Checked && !flag;
    this.lbTotal.Enabled = this.edTotal.Enabled = !flag;
    this.panelHint.Visible = !MRP_BoughtArticlesView.hiddenHintPanel;
  }

  private bool IsInstanceOrParty(int objectTypeID)
  {
    int objectTypeId1 = MetaDataHelper.GetObjectTypeID(new Guid("cad00583-306c-11d8-b4e9-00304f19f545"));
    int objectTypeId2 = MetaDataHelper.GetObjectTypeID(new Guid("cadd950b-306c-11d8-b4e9-00304f19f545"));
    return objectTypeId1 == objectTypeID || MetaDataHelper.GetObjectTypeChildrenIDRecursive(objectTypeId1).Contains(objectTypeID) || objectTypeId2 == objectTypeID || MetaDataHelper.GetObjectTypeChildrenIDRecursive(objectTypeId2).Contains(objectTypeID);
  }

  /// <summary>Управление признаком покупного изделия</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void cbBought_CheckedChanged(object sender, EventArgs e)
  {
    if (this.cbBought.Checked != (this._settings.IsBoughtArticle == 2L))
      this._settings.IsBoughtArticle = this.cbBought.Checked ? 2L : 1L;
    this.CaptureChanges();
    this.FillWithData();
    this.UpdateControls();
  }

  /// <summary>Сохранить указанное количество покупных в настройках</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoApply(object sender, EventArgs e)
  {
    MeasuredValue measuredValue;
    try
    {
      measuredValue = !string.IsNullOrEmpty(this.edBought.Text.Trim()) ? MeasureHelper.ConvertToMeasuredValue(this.edBought.Text.Trim()) : (MeasuredValue) null;
    }
    catch
    {
      measuredValue = this._settings != null ? this._settings.BoughtQuantity : (MeasuredValue) null;
    }
    if (this._settings != null)
      this._settings.BoughtQuantity = measuredValue;
    this.CaptureChanges();
    this.FillWithData();
    this.UpdateControls();
  }

  /// <summary>
  /// Установить указанное в поле Tag отправителя количество покупных в настройках
  /// </summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoSetPercents(object sender, EventArgs e)
  {
    if (this._settings == null || this._settings.SourceQuantity == null)
      return;
    double num1 = this._settings.SourceQuantity.Value;
    try
    {
      double num2 = sender is Control ? DataSetProcessor.GetDoubleValue(((Control) sender).Tag, 0.0) : 0.0;
      if (num2 <= 0.0)
        num2 = 0.0;
      if (num2 > 100.0)
        num2 = 100.0;
      double num3 = this._settings.SourceQuantity.Value / 100.0;
      if (num2 == 0.0)
        this.cbBought.Checked = false;
      else if (num2 == 100.0)
      {
        this._settings.BoughtQuantity = new MeasuredValue(this._settings.SourceQuantity.Value, this._settings.SourceQuantity.MeasureID);
      }
      else
      {
        double aValue = Math.Round(num3 * num2, MidpointRounding.AwayFromZero);
        if (aValue > this._settings.SourceQuantity.Value)
          aValue = this._settings.SourceQuantity.Value;
        this._settings.BoughtQuantity = new MeasuredValue(aValue, this._settings.SourceQuantity.MeasureID);
      }
    }
    catch
    {
      this.FillWithData();
      this.UpdateControls();
      return;
    }
    this.CaptureChanges();
    this.FillWithData();
    this.UpdateControls();
  }

  /// <summary>Нажата кнопка "Скрыть подсказку"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы отправителя</param>
  private void DoHideHint(object sender, EventArgs e)
  {
    this.panelHint.Visible = false;
    MRP_BoughtArticlesView.hiddenHintPanel = true;
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      BarManager service = ServicesManager.GetService(typeof (BarManager)) as BarManager;
    }
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MRP_BoughtArticlesView));
    this.panelMain = new Panel();
    this.btn0pc = new Button();
    this.btnApply = new Button();
    this.btn100pc = new Button();
    this.btn75pc = new Button();
    this.btn50pc = new Button();
    this.btn25pc = new Button();
    this.picture = new PictureBox();
    this.edBought = new TextBox();
    this.lbBought = new Label();
    this.edTotal = new TextBox();
    this.lbTotal = new Label();
    this.cbBought = new CheckBox();
    this.toolTips = new ToolTip(this.components);
    this.panelHint = new Panel();
    this.edHint = new RichTextBox();
    this.btnHideHint = new Button();
    this.panelMain.SuspendLayout();
    ((ISupportInitialize) this.picture).BeginInit();
    this.panelHint.SuspendLayout();
    this.SuspendLayout();
    this.panelMain.Controls.Add((Control) this.panelHint);
    this.panelMain.Controls.Add((Control) this.btn0pc);
    this.panelMain.Controls.Add((Control) this.btnApply);
    this.panelMain.Controls.Add((Control) this.btn100pc);
    this.panelMain.Controls.Add((Control) this.btn75pc);
    this.panelMain.Controls.Add((Control) this.btn50pc);
    this.panelMain.Controls.Add((Control) this.btn25pc);
    this.panelMain.Controls.Add((Control) this.picture);
    this.panelMain.Controls.Add((Control) this.edBought);
    this.panelMain.Controls.Add((Control) this.lbBought);
    this.panelMain.Controls.Add((Control) this.edTotal);
    this.panelMain.Controls.Add((Control) this.lbTotal);
    this.panelMain.Controls.Add((Control) this.cbBought);
    componentResourceManager.ApplyResources((object) this.panelMain, "panelMain");
    this.panelMain.Name = "panelMain";
    componentResourceManager.ApplyResources((object) this.btn0pc, "btn0pc");
    this.btn0pc.Name = "btn0pc";
    this.btn0pc.Tag = (object) "0";
    this.toolTips.SetToolTip((Control) this.btn0pc, componentResourceManager.GetString("btn0pc.ToolTip"));
    this.btn0pc.UseVisualStyleBackColor = true;
    this.btn0pc.Click += new EventHandler(this.DoSetPercents);
    componentResourceManager.ApplyResources((object) this.btnApply, "btnApply");
    this.btnApply.Name = "btnApply";
    this.btnApply.Tag = (object) "0";
    this.toolTips.SetToolTip((Control) this.btnApply, componentResourceManager.GetString("btnApply.ToolTip"));
    this.btnApply.UseVisualStyleBackColor = true;
    this.btnApply.Click += new EventHandler(this.DoApply);
    componentResourceManager.ApplyResources((object) this.btn100pc, "btn100pc");
    this.btn100pc.Name = "btn100pc";
    this.btn100pc.Tag = (object) "100";
    this.toolTips.SetToolTip((Control) this.btn100pc, componentResourceManager.GetString("btn100pc.ToolTip"));
    this.btn100pc.UseVisualStyleBackColor = true;
    this.btn100pc.Click += new EventHandler(this.DoSetPercents);
    componentResourceManager.ApplyResources((object) this.btn75pc, "btn75pc");
    this.btn75pc.Name = "btn75pc";
    this.btn75pc.Tag = (object) "75";
    this.toolTips.SetToolTip((Control) this.btn75pc, componentResourceManager.GetString("btn75pc.ToolTip"));
    this.btn75pc.UseVisualStyleBackColor = true;
    this.btn75pc.Click += new EventHandler(this.DoSetPercents);
    componentResourceManager.ApplyResources((object) this.btn50pc, "btn50pc");
    this.btn50pc.Name = "btn50pc";
    this.btn50pc.Tag = (object) "50";
    this.toolTips.SetToolTip((Control) this.btn50pc, componentResourceManager.GetString("btn50pc.ToolTip"));
    this.btn50pc.UseVisualStyleBackColor = true;
    this.btn50pc.Click += new EventHandler(this.DoSetPercents);
    componentResourceManager.ApplyResources((object) this.btn25pc, "btn25pc");
    this.btn25pc.Name = "btn25pc";
    this.btn25pc.Tag = (object) "25";
    this.toolTips.SetToolTip((Control) this.btn25pc, componentResourceManager.GetString("btn25pc.ToolTip"));
    this.btn25pc.UseVisualStyleBackColor = true;
    this.btn25pc.Click += new EventHandler(this.DoSetPercents);
    componentResourceManager.ApplyResources((object) this.picture, "picture");
    this.picture.Name = "picture";
    this.picture.TabStop = false;
    componentResourceManager.ApplyResources((object) this.edBought, "edBought");
    this.edBought.Name = "edBought";
    this.toolTips.SetToolTip((Control) this.edBought, componentResourceManager.GetString("edBought.ToolTip"));
    componentResourceManager.ApplyResources((object) this.lbBought, "lbBought");
    this.lbBought.Name = "lbBought";
    componentResourceManager.ApplyResources((object) this.edTotal, "edTotal");
    this.edTotal.Name = "edTotal";
    this.edTotal.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this.lbTotal, "lbTotal");
    this.lbTotal.Name = "lbTotal";
    componentResourceManager.ApplyResources((object) this.cbBought, "cbBought");
    this.cbBought.Name = "cbBought";
    this.toolTips.SetToolTip((Control) this.cbBought, componentResourceManager.GetString("cbBought.ToolTip"));
    this.cbBought.UseVisualStyleBackColor = true;
    this.cbBought.CheckedChanged += new EventHandler(this.cbBought_CheckedChanged);
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
    this.btnHideHint.UseVisualStyleBackColor = true;
    this.btnHideHint.Click += new EventHandler(this.DoHideHint);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.panelMain);
    this.MinimumSize = new Size(550, 225);
    this.Name = nameof (MRP_BoughtArticlesView);
    this.panelMain.ResumeLayout(false);
    this.panelMain.PerformLayout();
    ((ISupportInitialize) this.picture).EndInit();
    this.panelHint.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
