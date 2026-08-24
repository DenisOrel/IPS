// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.Settings.BaseSchemeItemFrm
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Client.Core;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client.Settings;

public class BaseSchemeItemFrm : Form
{
  private IContainer components;
  private Panel panelBottom;
  private Button buttonCancel;
  private Button buttonOK;
  private Label labelName;
  private ImageList imageListScheme;
  protected TextBox edName;
  protected ButtonedEdit edValue;
  protected CheckBox chbCData;

  public event BaseSchemeItemFrm.InsertAttributeEventHandler OnInsertAttribute;

  public BaseSchemeItemFrm() => this.InitializeComponent();

  public string NodeName
  {
    get => this.edName.Text;
    set => this.edName.Text = value;
  }

  public string NodeValue
  {
    get => this.edValue.Value;
    set => this.edValue.Value = value;
  }

  public bool CDATA
  {
    get => this.chbCData.Checked;
    set => this.chbCData.Checked = value;
  }

  protected virtual void buttonOK_Click(object sender, EventArgs e)
  {
    try
    {
      XmlConvert.VerifyName(this.edName.Text);
      this.DialogResult = DialogResult.OK;
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show($"Недопустимое имя элемента! {Environment.NewLine}{ex.Message}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  private void buttonCancel_Click(object sender, EventArgs e)
  {
    this.DialogResult = DialogResult.Cancel;
  }

  private void tbItemData_ButtonClick(object sender, EventArgs e)
  {
    BaseSchemeItemFrm.InsertAttributeEventHandler onInsertAttribute = this.OnInsertAttribute;
    if (onInsertAttribute == null)
      return;
    onInsertAttribute((object) this, new ButtonEditEventArgs(sender as ButtonedEdit));
  }

  private void edName_TextChanged(object sender, EventArgs e)
  {
    try
    {
      XmlConvert.VerifyName((sender as TextBox).Text);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show($"Недопустимое имя элемента! {Environment.NewLine}{ex.Message}", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BaseSchemeItemFrm));
    this.panelBottom = new Panel();
    this.buttonCancel = new Button();
    this.buttonOK = new Button();
    this.edName = new TextBox();
    this.labelName = new Label();
    this.imageListScheme = new ImageList(this.components);
    this.edValue = new ButtonedEdit();
    this.chbCData = new CheckBox();
    this.panelBottom.SuspendLayout();
    this.SuspendLayout();
    this.panelBottom.BorderStyle = BorderStyle.FixedSingle;
    this.panelBottom.Controls.Add((Control) this.buttonCancel);
    this.panelBottom.Controls.Add((Control) this.buttonOK);
    this.panelBottom.Dock = DockStyle.Bottom;
    this.panelBottom.Location = new Point(0, 133);
    this.panelBottom.Name = "panelBottom";
    this.panelBottom.Size = new Size(358, 46);
    this.panelBottom.TabIndex = 0;
    this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.buttonCancel.DialogResult = DialogResult.Cancel;
    this.buttonCancel.Location = new Point(259, 9);
    this.buttonCancel.Name = "buttonCancel";
    this.buttonCancel.Size = new Size(75, 23);
    this.buttonCancel.TabIndex = 0;
    this.buttonCancel.Text = "Отмена";
    this.buttonCancel.UseVisualStyleBackColor = true;
    this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
    this.buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.buttonOK.Location = new Point(178, 9);
    this.buttonOK.Name = "buttonOK";
    this.buttonOK.Size = new Size(75, 23);
    this.buttonOK.TabIndex = 0;
    this.buttonOK.Text = "OK";
    this.buttonOK.UseVisualStyleBackColor = true;
    this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
    this.edName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.edName.Location = new Point(23, 31 /*0x1F*/);
    this.edName.Name = "edName";
    this.edName.Size = new Size(313, 20);
    this.edName.TabIndex = 0;
    this.edName.TextChanged += new EventHandler(this.edName_TextChanged);
    this.labelName.AutoSize = true;
    this.labelName.Location = new Point(20, 15);
    this.labelName.Name = "labelName";
    this.labelName.Size = new Size(135, 13);
    this.labelName.TabIndex = 2;
    this.labelName.Text = "Наименование элемента";
    this.imageListScheme.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageListScheme.ImageStream");
    this.imageListScheme.TransparentColor = Color.Transparent;
    this.imageListScheme.Images.SetKeyName(0, "Element.png");
    this.imageListScheme.Images.SetKeyName(1, "Attribute.png");
    this.edValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.edValue.ButtonImage = (Image) null;
    this.edValue.ButtonText = "...";
    this.edValue.Caption = "Значение элемента";
    this.edValue.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.edValue.Image = (Image) null;
    this.edValue.Location = new Point(23, 57);
    this.edValue.MinimumSize = new Size(40, 20);
    this.edValue.Name = "edValue";
    this.edValue.Size = new Size(313, 38);
    this.edValue.TabIndex = 4;
    this.edValue.ButtonClick += new EventHandler(this.tbItemData_ButtonClick);
    this.chbCData.AutoSize = true;
    this.chbCData.Location = new Point(23, 107);
    this.chbCData.Name = "chbCData";
    this.chbCData.Size = new Size(62, 17);
    this.chbCData.TabIndex = 5;
    this.chbCData.Text = "CDATA";
    this.chbCData.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.buttonOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.buttonCancel;
    this.ClientSize = new Size(358, 179);
    this.Controls.Add((Control) this.chbCData);
    this.Controls.Add((Control) this.edValue);
    this.Controls.Add((Control) this.labelName);
    this.Controls.Add((Control) this.edName);
    this.Controls.Add((Control) this.panelBottom);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (BaseSchemeItemFrm);
    this.ShowInTaskbar = false;
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "SchemeItem";
    this.panelBottom.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  public delegate void InsertAttributeEventHandler(object sender, ButtonEditEventArgs args);
}
