// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.ManufactOrdersEditorForm
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Client.Core;
using Intermech.Client.Core.ObjectCreator;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.Navigator.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>Редактор состава производственного заказа</summary>
internal class ManufactOrdersEditorForm : Form
{
  /// <summary>
  /// Работа в режиме диалогового окна (не странички или закладки)
  /// </summary>
  private bool _dialogMode;
  /// <summary>
  /// Контейнер сервисов (контекст) для выделенных элементов пространства навигации
  /// </summary>
  private AdvancedServiceContainer _services = new AdvancedServiceContainer();
  /// <summary>
  /// Коллекция выделенных элементов пространства навигации, на основании данных которых работает закладка
  /// </summary>
  private ISelectedItems _items;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private ManufactOrdersEditor _manufactOrdersEditor;
  private Panel panelBottom;
  private Button _cancelButton;
  private Button _okButton;

  /// <summary>Создать экземпляр класса</summary>
  public ManufactOrdersEditorForm()
  {
    this.InitializeComponent();
    if (ServicesManager.GetService(typeof (ManufactOrdersEditorForm)) is ManufactOrdersEditorForm)
      ServicesManager.RemoveService(typeof (ManufactOrdersEditorForm));
    ServicesManager.AddService(typeof (ManufactOrdersEditorForm), (object) this);
  }

  /// <summary>Создать экземпляр класса, заполнить редактор</summary>
  /// <param name="provider">Контейнер сервисов</param>
  /// <param name="items">Коллекция выделенных элементов</param>
  public ManufactOrdersEditorForm(IServiceProvider provider, ISelectedItems items)
    : this()
  {
    this._services.AdvancedProvider = provider;
    this._items = items;
    this._manufactOrdersEditor.Changed += new ManufactOrdersChangedEventHandler(this.ManufactOrdersEditor_OnChanged);
    this._manufactOrdersEditor.ErrorsInEditor += new ManufactOrdersErrorsInEditorEventHandler(this.ManufactOrdersEditor_OnChanged);
    Rectangle primaryWorkingArea = MultiscreenHelper.PrimaryWorkingArea;
    this.Size = new Size(primaryWorkingArea.Width / 100 * 90, primaryWorkingArea.Height / 100 * 90);
    this.Location = new Point((primaryWorkingArea.Width - this.Size.Width) / 2 + primaryWorkingArea.Left, (primaryWorkingArea.Height - this.Size.Height) / 2 + primaryWorkingArea.Top);
    this.LoadViewData();
  }

  /// <summary>
  /// Отобразить редактор для состава производственного заказа
  /// </summary>
  /// <param name="provider">Контейнер сервисов</param>
  /// <param name="items">Коллекция выделенных элементов</param>
  public static DialogResult Execute(IServiceProvider provider, ISelectedItems items)
  {
    using (ManufactOrdersEditorForm ordersEditorForm = new ManufactOrdersEditorForm(provider, items))
    {
      ordersEditorForm._dialogMode = true;
      int num = (int) ordersEditorForm.ShowDialog();
      if (num == 1)
      {
        ManufactureOrderHolder manufactureOrderHolder = ordersEditorForm._manufactOrdersEditor._manufactureOrderHolder;
        if (ServicesManager.GetService(typeof (ManufactureOrderHolder)) != null)
          ServicesManager.RemoveService(typeof (ManufactureOrderHolder));
        ServicesManager.AddService(typeof (ManufactureOrderHolder), (object) manufactureOrderHolder);
      }
      return (DialogResult) num;
    }
  }

  private void ManufactOrdersEditorForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void ManufactOrdersEditorForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void ManufactOrdersEditor_OnChanged(object sender, EventArgs e) => this.UpdateControls();

  private void ManufactOrdersEditor_OnErrorsInEditor(object sender, EventArgs e)
  {
    this.UpdateControls();
  }

  private void OKButton_Click(object sender, EventArgs e)
  {
    if (this._dialogMode)
    {
      if (this._manufactOrdersEditor.IsChanged)
        this._manufactOrdersEditor.Fix();
      this.DialogResult = DialogResult.OK;
    }
    else
    {
      if (!this._manufactOrdersEditor.IsChanged)
        return;
      this._manufactOrdersEditor.Fix();
    }
  }

  private void CancelButton_Click(object sender, EventArgs e)
  {
    if (this._dialogMode)
    {
      this.DialogResult = DialogResult.Cancel;
    }
    else
    {
      if (!this._manufactOrdersEditor.IsChanged)
        return;
      this._manufactOrdersEditor.Undo();
    }
  }

  private void UpdateControls()
  {
    this._okButton.Enabled = this._dialogMode && !this._manufactOrdersEditor.HasErrorsInEditor || !this._dialogMode && this._manufactOrdersEditor.IsChanged && !this._manufactOrdersEditor.HasErrorsInEditor;
    this._cancelButton.Enabled = true;
  }

  /// <summary>
  /// Заполнить элементы управления закладки данными, полученными в методе Initialize
  /// </summary>
  private void LoadViewData()
  {
    this.Clear();
    this._manufactOrdersEditor.Init("", (CreatedObjectItem) null, this._items, (IServiceProvider) this._services);
  }

  /// <summary>Выполнить очистку элементов управления в закладке</summary>
  private void Clear()
  {
    this._manufactOrdersEditor.Clear();
    this.UpdateControls();
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && ServicesManager.GetService(typeof (ManufactOrdersEditorForm)) is ManufactOrdersEditorForm service && service == this)
      ServicesManager.RemoveService(typeof (ManufactOrdersEditorForm));
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ManufactOrdersEditorForm));
    this._manufactOrdersEditor = new ManufactOrdersEditor();
    this.panelBottom = new Panel();
    this._cancelButton = new Button();
    this._okButton = new Button();
    this.panelBottom.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this._manufactOrdersEditor, "_manufactOrdersEditor");
    this._manufactOrdersEditor.HasErrorsInEditor = true;
    this._manufactOrdersEditor.HeaderVisiblity = true;
    this._manufactOrdersEditor.IsChanged = false;
    this._manufactOrdersEditor.Name = "_manufactOrdersEditor";
    this._manufactOrdersEditor.Changed += new ManufactOrdersChangedEventHandler(this.ManufactOrdersEditor_OnChanged);
    this._manufactOrdersEditor.ErrorsInEditor += new ManufactOrdersErrorsInEditorEventHandler(this.ManufactOrdersEditor_OnErrorsInEditor);
    this.panelBottom.BorderStyle = BorderStyle.Fixed3D;
    this.panelBottom.Controls.Add((Control) this._cancelButton);
    this.panelBottom.Controls.Add((Control) this._okButton);
    componentResourceManager.ApplyResources((object) this.panelBottom, "panelBottom");
    this.panelBottom.Name = "panelBottom";
    componentResourceManager.ApplyResources((object) this._cancelButton, "_cancelButton");
    this._cancelButton.DialogResult = DialogResult.Cancel;
    this._cancelButton.Name = "_cancelButton";
    this._cancelButton.UseVisualStyleBackColor = true;
    this._cancelButton.Click += new EventHandler(this.CancelButton_Click);
    componentResourceManager.ApplyResources((object) this._okButton, "_okButton");
    this._okButton.DialogResult = DialogResult.OK;
    this._okButton.Name = "_okButton";
    this._okButton.UseVisualStyleBackColor = true;
    this._okButton.Click += new EventHandler(this.OKButton_Click);
    this.AcceptButton = (IButtonControl) this._okButton;
    this.AutoScaleMode = AutoScaleMode.Inherit;
    this.CancelButton = (IButtonControl) this._cancelButton;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this._manufactOrdersEditor);
    this.Controls.Add((Control) this.panelBottom);
    this.KeyPreview = true;
    this.Name = nameof (ManufactOrdersEditorForm);
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.FormClosed += new FormClosedEventHandler(this.ManufactOrdersEditorForm_FormClosed);
    this.Load += new EventHandler(this.ManufactOrdersEditorForm_Load);
    this.panelBottom.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
