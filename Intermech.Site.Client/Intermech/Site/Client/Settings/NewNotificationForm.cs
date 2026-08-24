// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.NewNotificationForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using ImSSP;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client.Settings;

public class NewNotificationForm : Form
{
  private TaskNotification _notififation;
  private IContainer components;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;
  private Panel panel2;
  private Label label2;
  private Label label1;
  private TextBox tbEmail;
  private TextBox tbUser;
  private CheckBox cbEnable;

  public NewNotificationForm() => this.InitializeComponent();

  private void Initialize(TaskNotification notififation)
  {
    this._notififation = notififation == null ? new TaskNotification(string.Empty, string.Empty, false) : notififation;
    this.tbUser.Text = this._notififation.User;
    this.tbEmail.Text = this._notififation.Email;
    this.cbEnable.Checked = this._notififation.Enable;
  }

  public static bool Show(TaskNotification notififation)
  {
    using (NewNotificationForm notificationForm = new NewNotificationForm())
    {
      notificationForm.Initialize(notififation);
      if (notificationForm.ShowDialog() != DialogResult.OK)
        return false;
      notififation = notificationForm._notififation;
      return true;
    }
  }

  private void bOK_Click(object sender, EventArgs e)
  {
    this._notififation.User = this.tbUser.Text;
    this._notififation.Email = this.tbEmail.Text;
    if (this.tbEmail.Text == string.Empty)
      throw new Exception(LocalizationHolder.rm.GetString(sc_18665.ssp_webportal_18666()));
    if (!this.IsEmail(this.tbEmail.Text))
      throw new Exception(LocalizationHolder.rm.GetString(sc_18665.ssp_webportal_18667()));
    this._notififation.Enable = this.cbEnable.Checked;
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  public bool IsEmail(string emailAddress)
  {
    return new Regex("^(([^<>()[\\]\\\\.,;:\\s@\\\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\\\"]+)*)|(\\\".+\\\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$").IsMatch(emailAddress);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (NewNotificationForm));
    this.panel1 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.panel2 = new Panel();
    this.cbEnable = new CheckBox();
    this.label2 = new Label();
    this.label1 = new Label();
    this.tbEmail = new TextBox();
    this.tbUser = new TextBox();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.bOK.Click += new EventHandler(this.bOK_Click);
    this.panel2.Controls.Add((Control) this.cbEnable);
    this.panel2.Controls.Add((Control) this.label2);
    this.panel2.Controls.Add((Control) this.label1);
    this.panel2.Controls.Add((Control) this.tbEmail);
    this.panel2.Controls.Add((Control) this.tbUser);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.cbEnable, "cbEnable");
    this.cbEnable.Checked = true;
    this.cbEnable.CheckState = CheckState.Checked;
    this.cbEnable.Name = "cbEnable";
    this.cbEnable.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.Name = "label2";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.tbEmail, "tbEmail");
    this.tbEmail.Name = "tbEmail";
    this.tbUser.BackColor = SystemColors.Window;
    componentResourceManager.ApplyResources((object) this.tbUser, "tbUser");
    this.tbUser.Name = "tbUser";
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (NewNotificationForm);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.ResumeLayout(false);
  }
}
