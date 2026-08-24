// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Orders.QuantityFm
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP.Orders;

/// <summary>
/// Форма позволяет указывать значение атрибута "Количество" для указанной связи
/// </summary>
public class QuantityFm : Form
{
  /// <summary>Идентификатор связи</summary>
  private long prjLinkID;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private PictureBox picture;
  private TextBox edBought;
  private Label lbBought;
  private Button btnCancel;
  private Button btnOK;

  /// <summary>Создать экземпляр класса</summary>
  public QuantityFm() => this.InitializeComponent();

  /// <summary>
  /// Создать экземпляр класса для редактирования количества указанной связи
  /// </summary>
  /// <param name="prjLinkID">Идентификатор связи</param>
  /// <param name="caption">Заголовок</param>
  public QuantityFm(long prjLinkID, string caption)
    : this()
  {
    this.prjLinkID = prjLinkID;
    this.Text = caption;
    Rectangle primaryWorkingArea = MultiscreenHelper.PrimaryWorkingArea;
    this.Location = new Point((primaryWorkingArea.Width - this.Size.Width) / 2 + primaryWorkingArea.Left, (primaryWorkingArea.Height - this.Size.Height) / 2 + primaryWorkingArea.Top);
    this.LoadData();
  }

  /// <summary>
  /// Открыть форму для редактирования значения атрибута "Количество" указанной связи
  /// </summary>
  /// <param name="prjLinkID">Идентификатор связи</param>
  /// <param name="caption">Заголовок формы</param>
  public static DialogResult Execute(long prjLinkID, string caption)
  {
    using (QuantityFm quantityFm = new QuantityFm(prjLinkID, caption))
      return quantityFm.ShowDialog();
  }

  /// <summary>Загрузить информацию в форму</summary>
  private void LoadData()
  {
    if (this.prjLinkID == 0L)
      throw new ArgumentException("prjLinkID");
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute attributeById = sessionKeeper.Session.GetRelation(this.prjLinkID).GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545"));
      MeasuredValue measuredValue = attributeById != null ? DataSetProcessor.GetMeasuredValue(attributeById.Value, (MeasuredValue) null) : (MeasuredValue) null;
      this.edBought.Text = measuredValue != null ? measuredValue.Caption : string.Empty;
    }
  }

  /// <summary>
  /// Записать новое значение количества в одноименный атрибут связи
  /// </summary>
  /// <param name="newVal">Новое значение для атрибута "Количество"</param>
  private void SaveData(MeasuredValue newVal)
  {
    if (this.prjLinkID == 0L)
      throw new ArgumentException("prjLinkID");
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      sessionKeeper.Session.GetRelation(this.prjLinkID).TryToAddOrDelAttribute(MetaDataHelper.GetAttributeTypeID("cad00267-306c-11d8-b4e9-00304f19f545"), (object) newVal);
  }

  /// <summary>Сохранить указанное количество покупных в настройках</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void DoApply(object sender, EventArgs e)
  {
    MeasureDescriptor measureDescriptor = (MeasureDescriptor) null;
    foreach (MeasureDescriptor measure in MeasureHelper.Measures)
    {
      if (measure.PhysicalQuantityGuid == SystemGUIDs.objectQuantityGuid)
      {
        measureDescriptor = measure;
        break;
      }
    }
    MeasuredValue newVal;
    try
    {
      newVal = !string.IsNullOrEmpty(this.edBought.Text.Trim()) ? MeasureHelper.ConvertToMeasuredValue($"{this.edBought.Text.Trim()} {measureDescriptor.ShortName}") : (MeasuredValue) null;
    }
    catch
    {
      newVal = (MeasuredValue) null;
    }
    if (newVal == null)
    {
      try
      {
        newVal = !string.IsNullOrEmpty(this.edBought.Text.Trim()) ? MeasureHelper.ConvertToMeasuredValue(this.edBought.Text.Trim()) : (MeasuredValue) null;
      }
      catch
      {
        newVal = (MeasuredValue) null;
      }
    }
    this.UpdateControls();
    if (newVal == null || newVal.Value < 0.0)
    {
      int num = (int) MessageBox.Show((IWin32Window) this, LocalizationHolder.rm.GetString("MRP_66"), LocalizationHolder.rm.GetString("MRP_45"), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
    else
    {
      this.SaveData(newVal);
      this.DialogResult = DialogResult.OK;
    }
  }

  /// <summary>Загрузим положение формы из настроек пользователя</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void QuantityFm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  /// <summary>Сохраним положение формы в настройках пользователя</summary>
  /// <param name="sender">Отправитель</param>
  /// <param name="e">Аргументы события</param>
  private void QuantityFm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  /// <summary>Управление контролами на закладке</summary>
  public void UpdateControls()
  {
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (QuantityFm));
    this.picture = new PictureBox();
    this.edBought = new TextBox();
    this.lbBought = new Label();
    this.btnCancel = new Button();
    this.btnOK = new Button();
    ((ISupportInitialize) this.picture).BeginInit();
    this.SuspendLayout();
    this.picture.AccessibleDescription = (string) null;
    this.picture.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.picture, "picture");
    this.picture.BackgroundImage = (Image) null;
    this.picture.Font = (Font) null;
    this.picture.ImageLocation = (string) null;
    this.picture.Name = "picture";
    this.picture.TabStop = false;
    this.edBought.AccessibleDescription = (string) null;
    this.edBought.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.edBought, "edBought");
    this.edBought.BackgroundImage = (Image) null;
    this.edBought.Font = (Font) null;
    this.edBought.Name = "edBought";
    this.lbBought.AccessibleDescription = (string) null;
    this.lbBought.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.lbBought, "lbBought");
    this.lbBought.Font = (Font) null;
    this.lbBought.Name = "lbBought";
    this.btnCancel.AccessibleDescription = (string) null;
    this.btnCancel.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.BackgroundImage = (Image) null;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Font = (Font) null;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnOK.AccessibleDescription = (string) null;
    this.btnOK.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
    this.btnOK.BackgroundImage = (Image) null;
    this.btnOK.Font = (Font) null;
    this.btnOK.Name = "btnOK";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.DoApply);
    this.AcceptButton = (IButtonControl) this.btnOK;
    this.AccessibleDescription = (string) null;
    this.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackgroundImage = (Image) null;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnOK);
    this.Controls.Add((Control) this.picture);
    this.Controls.Add((Control) this.edBought);
    this.Controls.Add((Control) this.lbBought);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.KeyPreview = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (QuantityFm);
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.Load += new EventHandler(this.QuantityFm_Load);
    this.FormClosed += new FormClosedEventHandler(this.QuantityFm_FormClosed);
    ((ISupportInitialize) this.picture).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
