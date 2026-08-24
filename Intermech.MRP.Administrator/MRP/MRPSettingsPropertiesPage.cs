// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.MRPSettingsPropertiesPage
// Assembly: Intermech.MRP.Administrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 6B87B3A6-A601-4A16-AA63-05D1A823449F
// Assembly location: D:\IPS\Client\Intermech.MRP.Administrator.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.Administrator.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP;

/// <summary>Страничка настроек MRP-системы</summary>
internal class MRPSettingsPropertiesPage : 
  UserControl,
  IPropertyPage,
  IPropertyPageSearchOptionEvents
{
  /// <summary>Настройки MRP-системы</summary>
  private IMRPSettings _mrpSettings;
  /// <summary>Можно ли дёргать OnChange</summary>
  private bool _inEvent;
  /// <summary>Контейнер служб</summary>
  private IServiceProvider _provider;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private Label lbCompositionContexts;
  private CheckBox cbCompositionContexts;
  private CheckBox cbSubstitutes;
  private Label lbDocumentation;
  private CheckBox cbDocumentation;
  private Label lbSubstitutes;
  private Label label1;
  private CheckBox cbBoughtArticles;

  /// <summary>Создать экземпляр закладки</summary>
  /// <param name="provider">Контейнер сервисов</param>
  public MRPSettingsPropertiesPage(IServiceProvider provider)
  {
    this.InitializeComponent();
    this._mrpSettings = ServicesManager.GetService(typeof (IMRPSettings)) as IMRPSettings;
    this._provider = provider;
    this.FillEditors();
    this.UpdateControls();
    if (!(this._provider.GetService(typeof (IPropertyPagesService)) is IPropertyPagesService service))
      return;
    service.AddPage(LocalizationHolder.rm.GetString("MRP_ADMIN_1"), (IPropertyPage) this);
  }

  /// <summary>Обновить статус контролов</summary>
  protected virtual void UpdateControls()
  {
  }

  /// <summary>Заполнить поля редактора настроек MRP-системы</summary>
  protected virtual void FillEditors()
  {
    bool inEvent = this._inEvent;
    try
    {
      this._inEvent = true;
      this.cbCompositionContexts.Checked = this._mrpSettings.UseCompositionContext;
      this.cbSubstitutes.Checked = this._mrpSettings.UseSubstitutes;
      this.cbDocumentation.Checked = this._mrpSettings.UseDocumentation;
      this.cbBoughtArticles.Checked = this._mrpSettings.UseBoughtArticles;
    }
    finally
    {
      this._inEvent = inEvent;
    }
  }

  /// <summary>Загрузить из редакторов настройки допустимых замен</summary>
  protected virtual void LoadFromEditors()
  {
    this._mrpSettings.UseCompositionContext = this.cbCompositionContexts.Checked;
    this._mrpSettings.UseSubstitutes = this.cbSubstitutes.Checked;
    this._mrpSettings.UseDocumentation = this.cbDocumentation.Checked;
    this._mrpSettings.UseBoughtArticles = this.cbBoughtArticles.Checked;
  }

  /// <summary>Событие будет дёргаться при необходимости</summary>
  private void OnChanged()
  {
    if (this.Changed == null)
      return;
    this.Changed((object) this, new EventArgs());
  }

  /// <summary>Обработчик событий</summary>
  public event EventHandler Changed;

  /// <summary>
  /// Что за хрень мы добавили в окно настроек? Ответ - контрол
  /// </summary>
  public PropertyPageType Type => PropertyPageType.Control;

  /// <summary>
  /// Контрол, который будет размещён на главной форме настроек
  /// </summary>
  public object Control => (object) this;

  /// <summary>Название странички в главной форме настроек</summary>
  public string PageName => LocalizationHolder.rm.GetString("MRP_ADMIN_2");

  /// <summary>
  /// Текст заголовка (пустое значение - заголовок не отображается)
  /// </summary>
  public string HeaderText
  {
    [DebuggerStepThrough] get => LocalizationHolder.rm.GetString("MRP_ADMIN_3");
  }

  /// <summary>Применить изменения редактора</summary>
  public void Apply()
  {
    this.LoadFromEditors();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._mrpSettings.SaveSettings(sessionKeeper.Session);
      if (sessionKeeper.Session.GetCustomService(typeof (IMRPSettings)) is IMRPSettings customService)
        customService.LoadSettings(sessionKeeper.Session.SessionGUID);
    }
    this.FillEditors();
  }

  /// <summary>Отменить изменения редактора</summary>
  public void Cancel() => this.FillEditors();

  /// <summary>вернуть id раздела в хелпе для данной страницы</summary>
  public string HelpTopicID => "0";

  /// <summary>
  /// Возвращает список имен настроек, содержащихся в контроле
  /// </summary>
  public List<string> GetOptionNames()
  {
    return !(this.Control is System.Windows.Forms.Control control) ? new List<string>() : IPropertyPageHelper.GetOptionNames(control);
  }

  /// <summary>Изменился статус чекбокса</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void cbCheckedChanged(object sender, EventArgs e)
  {
    if (this._inEvent)
      return;
    this.OnChanged();
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MRPSettingsPropertiesPage));
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.label1 = new Label();
    this.cbBoughtArticles = new CheckBox();
    this.lbDocumentation = new Label();
    this.cbDocumentation = new CheckBox();
    this.lbSubstitutes = new Label();
    this.cbSubstitutes = new CheckBox();
    this.lbCompositionContexts = new Label();
    this.cbCompositionContexts = new CheckBox();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
    this.tableLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.label1, 0, 11);
    this.tableLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.cbBoughtArticles, 0, 10);
    this.tableLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.lbDocumentation, 0, 8);
    this.tableLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.cbDocumentation, 0, 7);
    this.tableLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.lbSubstitutes, 0, 5);
    this.tableLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.cbSubstitutes, 0, 4);
    this.tableLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.lbCompositionContexts, 0, 2);
    this.tableLayoutPanel1.Controls.Add((System.Windows.Forms.Control) this.cbCompositionContexts, 0, 1);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.cbBoughtArticles, "cbBoughtArticles");
    this.cbBoughtArticles.Name = "cbBoughtArticles";
    this.cbBoughtArticles.UseVisualStyleBackColor = true;
    this.cbBoughtArticles.CheckedChanged += new EventHandler(this.cbCheckedChanged);
    componentResourceManager.ApplyResources((object) this.lbDocumentation, "lbDocumentation");
    this.lbDocumentation.Name = "lbDocumentation";
    componentResourceManager.ApplyResources((object) this.cbDocumentation, "cbDocumentation");
    this.cbDocumentation.Name = "cbDocumentation";
    this.cbDocumentation.UseVisualStyleBackColor = true;
    this.cbDocumentation.CheckedChanged += new EventHandler(this.cbCheckedChanged);
    componentResourceManager.ApplyResources((object) this.lbSubstitutes, "lbSubstitutes");
    this.lbSubstitutes.Name = "lbSubstitutes";
    componentResourceManager.ApplyResources((object) this.cbSubstitutes, "cbSubstitutes");
    this.cbSubstitutes.Name = "cbSubstitutes";
    this.cbSubstitutes.UseVisualStyleBackColor = true;
    this.cbSubstitutes.CheckedChanged += new EventHandler(this.cbCheckedChanged);
    componentResourceManager.ApplyResources((object) this.lbCompositionContexts, "lbCompositionContexts");
    this.lbCompositionContexts.Name = "lbCompositionContexts";
    componentResourceManager.ApplyResources((object) this.cbCompositionContexts, "cbCompositionContexts");
    this.cbCompositionContexts.Name = "cbCompositionContexts";
    this.cbCompositionContexts.UseVisualStyleBackColor = true;
    this.cbCompositionContexts.CheckedChanged += new EventHandler(this.cbCheckedChanged);
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.Controls.Add((System.Windows.Forms.Control) this.tableLayoutPanel1);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Name = nameof (MRPSettingsPropertiesPage);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }
}
