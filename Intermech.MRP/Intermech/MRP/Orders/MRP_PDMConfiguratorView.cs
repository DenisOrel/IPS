// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.MRP_PDMConfiguratorView
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Interfaces;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.PdmConfigurator;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>Закладка "Применить с опциями"</summary>
/// <summary>Закладка конфигуратора составов</summary>
internal class MRP_PDMConfiguratorView : MRP_BaseView
{
  /// <summary>Скрыта ли панель подсказки</summary>
  internal static bool hiddenHintPanel;
  /// <summary>Удаление используемых ресурсов</summary>
  private IContainer components;
  private ObjectContextEditor objectOptions;
  private Panel panelHint;
  private RichTextBox edHint;
  private Button btnHideHint;

  /// <summary>Создать экземпляр класса</summary>
  public MRP_PDMConfiguratorView() => this.InitializeComponent();

  /// <summary>Заголовок закладки</summary>
  public override string Caption
  {
    [DebuggerStepThrough] get => LocalizationHolder.rm.GetString("MRP_8");
  }

  /// <summary>Порядковый номер закладки</summary>
  public override int OrderID => -10;

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
    this._imgView = this._images != null ? this._images.ImageIndex("MRP.imgConfigurator") : -1;
  }

  /// <summary>
  /// Заполнить элементы управления закладки данными, полученными в методе Initialize
  /// </summary>
  protected override void LoadViewData()
  {
    this.Clear();
    PdmConfiguratorContext configuratorContext = this.GetConfiguratorContext(this._items, this.Services);
    try
    {
      this.inEvents = true;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBRelation relation = configuratorContext == null || configuratorContext.Key == null || configuratorContext.Key.F_PRJLINK_ID == 0L ? (IDBRelation) null : sessionKeeper.Session.GetRelation(configuratorContext.Key.F_PRJLINK_ID, false);
        IDBObject dbObject = configuratorContext == null || configuratorContext.Key == null || configuratorContext.Key.F_PRJLINK_ID != 0L || configuratorContext.Key.TOP_OBJECT_ID != configuratorContext.Key.F_PROJ_ID || !MetaDataHelper.IsPdmRootObjectType(configuratorContext.Key.TOP_OBJECT_TYPE) ? (IDBObject) null : sessionKeeper.Session.GetObject(configuratorContext.Key.TOP_OBJECT_ID, false);
        this.objectOptions.AccessRights = this.objectOptions.CheckAccessRights(relation != null ? (IDBAttributable) relation : (IDBAttributable) dbObject);
        this.objectOptions.Services = this.Services;
        this.objectOptions.Context = configuratorContext;
        if (relation != null)
        {
          if (configuratorContext != null)
            goto label_9;
        }
        this.objectOptions.ClearKeys();
        this.objectOptions.Clear();
      }
    }
    finally
    {
      this.inEvents = false;
    }
label_9:
    this.UpdateControls();
  }

  /// <summary>Управление контролами на закладке</summary>
  protected override void UpdateControls()
  {
    base.UpdateControls();
    this.panelHint.Visible = !MRP_PDMConfiguratorView.hiddenHintPanel;
  }

  /// <summary>Очистить редактор</summary>
  protected override void Clear()
  {
    base.Clear();
    this.objectOptions.ClearKeys();
    this.objectOptions.Clear();
  }

  /// <summary>
  /// В редакторе произошли изменения, вносим их в базу данных
  /// </summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoRaiseOnChanged(object sender, EventArgs e)
  {
    if (this.inEvents || this.objectOptions == null || !this.objectOptions.IsChanged)
      return;
    PdmConfiguratorContext context = this.objectOptions.Context;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (context == null || context.Key == null || context.Key.F_PRJLINK_ID == 0L)
        return;
      IDBRelation relation = sessionKeeper.Session.GetRelation(context.Key.F_PRJLINK_ID, false);
      if (relation == null)
        return;
      try
      {
        context.Services.AddService(typeof (IUserSession), (object) sessionKeeper.Session);
        context.Services.AddService(typeof (object), (object) relation);
        context.SaveToObject((IDBAttributable) relation);
      }
      finally
      {
        context.Services.RemoveService(typeof (object));
        context.Services.RemoveService(typeof (IUserSession));
        PdmConfiguratorObjectOptionsCache.ResetExpired();
        if (sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) is IPdmConfiguratorService customService)
          customService.ResetSessionCache((object) sessionKeeper.Session.SessionGUID);
      }
    }
    this.objectOptions.Fix();
    this.RaiseOnChanged();
  }

  /// <summary>Нажата кнопка "Скрыть подсказку"</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы отправителя</param>
  private void DoHideHint(object sender, EventArgs e)
  {
    this.panelHint.Visible = false;
    MRP_PDMConfiguratorView.hiddenHintPanel = true;
  }

  /// <summary>Удаление используемых ресурсов</summary>
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MRP_PDMConfiguratorView));
    this.objectOptions = new ObjectContextEditor();
    this.panelHint = new Panel();
    this.edHint = new RichTextBox();
    this.btnHideHint = new Button();
    this.panelHint.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.objectOptions, "objectOptions");
    this.objectOptions.IsChanged = false;
    this.objectOptions.IsOptionValueStatus = true;
    this.objectOptions.MinimumSize = new Size(307, 123);
    this.objectOptions.Name = "objectOptions";
    this.objectOptions.OnChanged += new ObjectContextEditor.ContextChangedEventHandler(this.DoRaiseOnChanged);
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
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((Control) this.objectOptions);
    this.Controls.Add((Control) this.panelHint);
    this.MinimumSize = new Size(450, 300);
    this.Name = nameof (MRP_PDMConfiguratorView);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.panelHint.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
