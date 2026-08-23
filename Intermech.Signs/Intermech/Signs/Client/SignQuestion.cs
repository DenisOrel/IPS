// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignQuestion
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

public class SignQuestion : Form
{
  private List<string> messageText;
  private bool isCollapsed;
  private string resolution = string.Empty;
  private readonly long attrResolutionSize;
  private IContainer components;
  private PictureBox pictureBox1;
  private Button btnOk;
  private Button btnCancel;
  private Button btnResolution;
  private Label label1;
  private TextBox tbResolution;
  private RichTextBox rtbMessage;
  private Panel panel1;
  private Panel panel2;
  private Panel panel3;

  public string Resolution
  {
    get => this.resolution;
    set => this.resolution = value;
  }

  public bool IsCollapsed
  {
    get => this.isCollapsed;
    set
    {
      this.isCollapsed = value;
      this.tbResolution.Visible = this.label1.Visible = value;
    }
  }

  public List<string> MessageText
  {
    get => this.messageText;
    set
    {
      this.messageText = value;
      this.rtbMessage.Text = string.Join("\r\n", this.messageText.ToArray());
      this.rtbMessage.Height = this.rtbMessage.Font.Height * ((this.messageText.Count <= 15 ? this.messageText.Count : 16 /*0x10*/) + 5);
      this.panel1.Height = this.rtbMessage.Height + 15;
      this.Height = this.rtbMessage.Height + 110;
    }
  }

  public SignQuestion()
  {
    this.InitializeComponent();
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(SignsHolder.ResolutionAttrTypeID);
    if (attributeType != null)
      this.attrResolutionSize = attributeType.SizeType;
    this.IsCollapsed = false;
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1597);
  }

  private void btnResolution_Click(object sender, EventArgs e)
  {
    if (this.isCollapsed)
    {
      this.btnResolution.Text = LocalizationHolder.rm.GetString("Signs_124");
      this.Height -= 60;
      this.panel3.Height = 0;
    }
    else
    {
      this.panel3.Height = 50;
      this.btnResolution.Text = LocalizationHolder.rm.GetString("Signs_125");
      this.Height += 60;
    }
    this.IsCollapsed = !this.IsCollapsed;
  }

  private void btnOk_Click(object sender, EventArgs e)
  {
    if ((long) this.tbResolution.Text.Length > this.attrResolutionSize)
    {
      int num = (int) MessageBox.Show(string.Format(LocalizationHolder.rm.GetString("Signs_127"), (object) this.attrResolutionSize), LocalizationHolder.rm.GetString("Signs_112"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
    else
    {
      this.resolution = this.tbResolution.Text;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
  }

  private void SignQuestion_Load(object sender, EventArgs e)
  {
  }

  private void SignQuestion_FormClosed(object sender, FormClosedEventArgs e)
  {
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SignQuestion));
    this.pictureBox1 = new PictureBox();
    this.btnOk = new Button();
    this.btnCancel = new Button();
    this.btnResolution = new Button();
    this.label1 = new Label();
    this.tbResolution = new TextBox();
    this.rtbMessage = new RichTextBox();
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this.panel3 = new Panel();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.panel3.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.TabStop = false;
    componentResourceManager.ApplyResources((object) this.btnOk, "btnOk");
    this.btnOk.Name = "btnOk";
    this.btnOk.UseVisualStyleBackColor = true;
    this.btnOk.Click += new EventHandler(this.btnOk_Click);
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.btnResolution, "btnResolution");
    this.btnResolution.Name = "btnResolution";
    this.btnResolution.UseVisualStyleBackColor = true;
    this.btnResolution.Click += new EventHandler(this.btnResolution_Click);
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.tbResolution, "tbResolution");
    this.tbResolution.Name = "tbResolution";
    this.rtbMessage.BackColor = SystemColors.Control;
    this.rtbMessage.BorderStyle = BorderStyle.None;
    this.rtbMessage.Cursor = Cursors.Default;
    this.rtbMessage.HideSelection = false;
    componentResourceManager.ApplyResources((object) this.rtbMessage, "rtbMessage");
    this.rtbMessage.Name = "rtbMessage";
    this.rtbMessage.ReadOnly = true;
    this.rtbMessage.TabStop = false;
    this.panel1.Controls.Add((Control) this.pictureBox1);
    this.panel1.Controls.Add((Control) this.rtbMessage);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    this.panel2.Controls.Add((Control) this.btnOk);
    this.panel2.Controls.Add((Control) this.btnCancel);
    this.panel2.Controls.Add((Control) this.btnResolution);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this.panel3.BackColor = SystemColors.Control;
    this.panel3.Controls.Add((Control) this.tbResolution);
    this.panel3.Controls.Add((Control) this.label1);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    this.AcceptButton = (IButtonControl) this.btnOk;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.panel3);
    this.Controls.Add((Control) this.panel2);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.HelpButton = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SignQuestion);
    this.ShowIcon = false;
    this.FormClosed += new FormClosedEventHandler(this.SignQuestion_FormClosed);
    this.Load += new EventHandler(this.SignQuestion_Load);
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    this.panel2.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.panel3.PerformLayout();
    this.ResumeLayout(false);
  }
}
