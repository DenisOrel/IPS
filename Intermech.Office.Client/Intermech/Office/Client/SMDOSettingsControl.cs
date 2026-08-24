// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SMDOSettingsControl
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Office.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

public class SMDOSettingsControl : UserControl, IPropertyPage
{
  private bool _changed;
  private IContainer components;
  private Label label1;
  private Label label2;
  private TextBox txbSmdoEmail;
  private TextBox txbCompanyName;
  private TextBox txbSmdoCompanyID;
  private Label label3;
  private GroupBox groupBox1;
  private NumericUpDown nudPort;
  private Label label7;
  private TextBox tbHost;
  private Label label6;
  private TextBox tbPassword;
  private Label label5;
  private TextBox tbUserName;
  private Label label4;
  private GroupBox groupBox2;
  private TextBox tbCompanyEmail;
  private Label label8;
  private TextBox tbSysID;
  private Label lblSysID;
  private CheckBox cbSSL;

  public SMDOSettingsControl(IUserSession session)
  {
    this.InitializeComponent();
    this.ReadSettings(session);
  }

  private void ReadSettings(IUserSession session)
  {
    this._changed = false;
    SMDOSettings settings = ((ISMDOSettingsService) session.GetCustomService(typeof (ISMDOSettingsService))).Settings;
    this.txbSmdoEmail.Text = settings.SmdoEmail;
    this.txbSmdoCompanyID.Text = settings.CompanySMDOid;
    this.txbCompanyName.Text = settings.CompanyName;
    this.tbCompanyEmail.Text = settings.MyCompanyEmail;
    this.tbUserName.Text = settings.UserName;
    this.tbHost.Text = settings.SMDOHost;
    this.tbPassword.Text = settings.Password;
    this.nudPort.Value = (Decimal) settings.Port;
    this.cbSSL.Checked = settings.SSL;
    this.tbSysID.Text = settings.SystemID;
  }

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Control;

  public object Control => (object) this;

  public string PageName => "Настройки СМДО";

  public void Apply()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.Save(sessionKeeper.Session);
  }

  public void Cancel() => this._changed = false;

  public string HelpTopicID => "unknown";

  public string HeaderText => "Параметры для настройки службы обмена данными СМДО";

  private void OnChanged()
  {
    if (this._changed || this.Changed == null)
      return;
    this._changed = true;
    this.Changed((object) this, new EventArgs());
  }

  private void txtSmdoID_TextChanged(object sender, EventArgs e) => this.OnChanged();

  private void Save(IUserSession session)
  {
    ((ISMDOSettingsService) session.GetCustomService(typeof (ISMDOSettingsService))).Save(session.SessionGUID, new SMDOSettings(this.txbSmdoEmail.Text, this.txbSmdoCompanyID.Text, this.txbCompanyName.Text, this.tbUserName.Text, this.tbPassword.Text, this.tbHost.Text, (int) this.nudPort.Value, this.cbSSL.Checked, this.tbCompanyEmail.Text, this.tbSysID.Text));
  }

  private void nudPort_KeyPress(object sender, KeyPressEventArgs e) => this.OnChanged();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.label1 = new Label();
    this.label2 = new Label();
    this.txbSmdoEmail = new TextBox();
    this.txbCompanyName = new TextBox();
    this.txbSmdoCompanyID = new TextBox();
    this.label3 = new Label();
    this.groupBox1 = new GroupBox();
    this.nudPort = new NumericUpDown();
    this.label7 = new Label();
    this.tbHost = new TextBox();
    this.label6 = new Label();
    this.tbPassword = new TextBox();
    this.label5 = new Label();
    this.tbUserName = new TextBox();
    this.label4 = new Label();
    this.groupBox2 = new GroupBox();
    this.tbSysID = new TextBox();
    this.lblSysID = new Label();
    this.tbCompanyEmail = new TextBox();
    this.label8 = new Label();
    this.cbSSL = new CheckBox();
    this.groupBox1.SuspendLayout();
    this.nudPort.BeginInit();
    this.groupBox2.SuspendLayout();
    this.SuspendLayout();
    this.label1.AutoSize = true;
    this.label1.Location = new Point(3, 16 /*0x10*/);
    this.label1.Name = "label1";
    this.label1.Size = new Size(116, 13);
    this.label1.TabIndex = 2;
    this.label1.Text = "E-mail сервера СМДО";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(3, 94);
    this.label2.Name = "label2";
    this.label2.Size = new Size(151, 13);
    this.label2.TabIndex = 2;
    this.label2.Text = "Наименование организации";
    this.txbSmdoEmail.Location = new Point(6, 32 /*0x20*/);
    this.txbSmdoEmail.Name = "txbSmdoEmail";
    this.txbSmdoEmail.Size = new Size(243, 20);
    this.txbSmdoEmail.TabIndex = 3;
    this.txbSmdoEmail.TextChanged += new EventHandler(this.txtSmdoID_TextChanged);
    this.txbCompanyName.Location = new Point(6, 110);
    this.txbCompanyName.Name = "txbCompanyName";
    this.txbCompanyName.Size = new Size(243, 20);
    this.txbCompanyName.TabIndex = 3;
    this.txbCompanyName.TextChanged += new EventHandler(this.txtSmdoID_TextChanged);
    this.txbSmdoCompanyID.Location = new Point(6, 71);
    this.txbSmdoCompanyID.Name = "txbSmdoCompanyID";
    this.txbSmdoCompanyID.Size = new Size(243, 20);
    this.txbSmdoCompanyID.TabIndex = 1;
    this.txbSmdoCompanyID.TextChanged += new EventHandler(this.txtSmdoID_TextChanged);
    this.label3.AutoSize = true;
    this.label3.Location = new Point(3, 55);
    this.label3.Name = "label3";
    this.label3.Size = new Size(246, 13);
    this.label3.TabIndex = 0;
    this.label3.Text = "Идентификатор организации в системе СМДО";
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.cbSSL);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.nudPort);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.label7);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.tbHost);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.label6);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.tbPassword);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.label5);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.tbUserName);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.label4);
    this.groupBox1.Location = new Point(7, 229);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size((int) byte.MaxValue, 208 /*0xD0*/);
    this.groupBox1.TabIndex = 4;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Настройки подключения к СМДО";
    this.nudPort.Location = new Point(7, 155);
    this.nudPort.Maximum = new Decimal(new int[4]
    {
      9999,
      0,
      0,
      0
    });
    this.nudPort.Name = "nudPort";
    this.nudPort.Size = new Size(242, 20);
    this.nudPort.TabIndex = 2;
    this.nudPort.ValueChanged += new EventHandler(this.txtSmdoID_TextChanged);
    this.nudPort.KeyPress += new KeyPressEventHandler(this.nudPort_KeyPress);
    this.label7.AutoSize = true;
    this.label7.Location = new Point(3, 138);
    this.label7.Name = "label7";
    this.label7.Size = new Size((int) sbyte.MaxValue, 13);
    this.label7.TabIndex = 0;
    this.label7.Text = "Порт исходящей почты:";
    this.tbHost.Location = new Point(6, 115);
    this.tbHost.Name = "tbHost";
    this.tbHost.Size = new Size(243, 20);
    this.tbHost.TabIndex = 1;
    this.tbHost.TextChanged += new EventHandler(this.txtSmdoID_TextChanged);
    this.label6.AutoSize = true;
    this.label6.Location = new Point(3, 99);
    this.label6.Name = "label6";
    this.label6.Size = new Size(177, 13);
    this.label6.TabIndex = 0;
    this.label6.Text = "IP адрес маршрутизатора СМДО:";
    this.tbPassword.Location = new Point(6, 76);
    this.tbPassword.Name = "tbPassword";
    this.tbPassword.PasswordChar = '*';
    this.tbPassword.Size = new Size(243, 20);
    this.tbPassword.TabIndex = 1;
    this.tbPassword.TextChanged += new EventHandler(this.txtSmdoID_TextChanged);
    this.label5.AutoSize = true;
    this.label5.Location = new Point(3, 60);
    this.label5.Name = "label5";
    this.label5.Size = new Size(48 /*0x30*/, 13);
    this.label5.TabIndex = 0;
    this.label5.Text = "Пароль:";
    this.tbUserName.Location = new Point(6, 37);
    this.tbUserName.Name = "tbUserName";
    this.tbUserName.Size = new Size(243, 20);
    this.tbUserName.TabIndex = 1;
    this.tbUserName.TextChanged += new EventHandler(this.txtSmdoID_TextChanged);
    this.label4.AutoSize = true;
    this.label4.Location = new Point(3, 21);
    this.label4.Name = "label4";
    this.label4.Size = new Size(161, 13);
    this.label4.TabIndex = 0;
    this.label4.Text = "Имя пользователя в системе:";
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.tbSysID);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.lblSysID);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.tbCompanyEmail);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.label8);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.label1);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.label3);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.txbCompanyName);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.txbSmdoCompanyID);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.txbSmdoEmail);
    this.groupBox2.Controls.Add((System.Windows.Forms.Control) this.label2);
    this.groupBox2.Location = new Point(7, 1);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size((int) byte.MaxValue, 222);
    this.groupBox2.TabIndex = 5;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Общие настройки";
    this.tbSysID.Location = new Point(6, 188);
    this.tbSysID.Name = "tbSysID";
    this.tbSysID.Size = new Size(243, 20);
    this.tbSysID.TabIndex = 7;
    this.lblSysID.AutoSize = true;
    this.lblSysID.Location = new Point(3, 172);
    this.lblSysID.Name = "lblSysID";
    this.lblSysID.Size = new Size(248, 13);
    this.lblSysID.TabIndex = 6;
    this.lblSysID.Text = "Идентификатор системы в справочнике СМДО";
    this.tbCompanyEmail.Location = new Point(6, 149);
    this.tbCompanyEmail.Name = "tbCompanyEmail";
    this.tbCompanyEmail.Size = new Size(243, 20);
    this.tbCompanyEmail.TabIndex = 5;
    this.tbCompanyEmail.TextChanged += new EventHandler(this.txtSmdoID_TextChanged);
    this.label8.AutoSize = true;
    this.label8.Location = new Point(3, 133);
    this.label8.Name = "label8";
    this.label8.Size = new Size(182, 13);
    this.label8.TabIndex = 4;
    this.label8.Text = "E-mail компании в системе СМДО:";
    this.cbSSL.AutoSize = true;
    this.cbSSL.Location = new Point(7, 181);
    this.cbSSL.Name = "cbSSL";
    this.cbSSL.Size = new Size(46, 17);
    this.cbSSL.TabIndex = 3;
    this.cbSSL.Text = "SSL";
    this.cbSSL.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this.groupBox2);
    this.Controls.Add((System.Windows.Forms.Control) this.groupBox1);
    this.Name = nameof (SMDOSettingsControl);
    this.Size = new Size(279, 463);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.nudPort.EndInit();
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.ResumeLayout(false);
  }
}
