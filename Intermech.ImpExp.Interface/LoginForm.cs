// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.LoginForm
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Summary description for LoginForm.</summary>
internal class LoginForm : Form
{
  private PictureBox pictureBox1;
  private Label label1;
  private Label label2;
  private TextBox _edUser;
  private TextBox _edPassword;
  private Button _btOK;
  private Button _btCancel;
  private Panel panel1;
  private IContainer components;

  public LoginForm() => this.InitializeComponent();

  /// <summary>Clean up any resources being used.</summary>
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoginForm));
    this.pictureBox1 = new PictureBox();
    this.label1 = new Label();
    this._edUser = new TextBox();
    this._edPassword = new TextBox();
    this.label2 = new Label();
    this._btOK = new Button();
    this._btCancel = new Button();
    this.panel1 = new Panel();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.SuspendLayout();
    this.pictureBox1.AccessibleDescription = (string) null;
    this.pictureBox1.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
    this.pictureBox1.BackgroundImage = (Image) null;
    this.pictureBox1.Font = (Font) null;
    this.pictureBox1.ImageLocation = (string) null;
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.TabStop = false;
    this.label1.AccessibleDescription = (string) null;
    this.label1.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Font = (Font) null;
    this.label1.Name = "label1";
    this._edUser.AccessibleDescription = (string) null;
    this._edUser.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this._edUser, "_edUser");
    this._edUser.BackgroundImage = (Image) null;
    this._edUser.Font = (Font) null;
    this._edUser.Name = "_edUser";
    this._edPassword.AccessibleDescription = (string) null;
    this._edPassword.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this._edPassword, "_edPassword");
    this._edPassword.BackgroundImage = (Image) null;
    this._edPassword.Font = (Font) null;
    this._edPassword.Name = "_edPassword";
    this._edPassword.TextChanged += new EventHandler(this._edPassword_TextChanged);
    this.label2.AccessibleDescription = (string) null;
    this.label2.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.Font = (Font) null;
    this.label2.Name = "label2";
    this._btOK.AccessibleDescription = (string) null;
    this._btOK.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this._btOK, "_btOK");
    this._btOK.BackgroundImage = (Image) null;
    this._btOK.DialogResult = DialogResult.OK;
    this._btOK.Font = (Font) null;
    this._btOK.Name = "_btOK";
    this._btCancel.AccessibleDescription = (string) null;
    this._btCancel.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this._btCancel, "_btCancel");
    this._btCancel.BackgroundImage = (Image) null;
    this._btCancel.DialogResult = DialogResult.Cancel;
    this._btCancel.Font = (Font) null;
    this._btCancel.Name = "_btCancel";
    this.panel1.AccessibleDescription = (string) null;
    this.panel1.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.BackgroundImage = (Image) null;
    this.panel1.Font = (Font) null;
    this.panel1.Name = "panel1";
    this.AcceptButton = (IButtonControl) this._btOK;
    this.AccessibleDescription = (string) null;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.BackgroundImage = (Image) null;
    this.CancelButton = (IButtonControl) this._btCancel;
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this._btCancel);
    this.Controls.Add((Control) this._btOK);
    this.Controls.Add((Control) this._edPassword);
    this.Controls.Add((Control) this.label2);
    this.Controls.Add((Control) this._edUser);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.pictureBox1);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (LoginForm);
    this.SizeGripStyle = SizeGripStyle.Hide;
    this.Resize += new EventHandler(this.LoginForm_Resize);
    this.Layout += new LayoutEventHandler(this.LoginForm_Layout);
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private void _edPassword_TextChanged(object sender, EventArgs e)
  {
    this._btOK.Enabled = this._edUser.Text.Length > 0;
  }

  public string UserName
  {
    get => this._edUser.Text;
    set => this._edUser.Text = value;
  }

  public string Password
  {
    get => this._edPassword.Text;
    set => this._edPassword.Text = value;
  }

  public static bool ShowLogin(ref string userName, ref string password, string caption)
  {
    LoginForm loginForm = new LoginForm();
    loginForm.StartPosition = FormStartPosition.CenterScreen;
    loginForm.Text = caption;
    if (userName != string.Empty)
      loginForm.UserName = userName;
    if (password != string.Empty)
      loginForm.Password = password;
    if (loginForm.ShowDialog() != DialogResult.OK)
      return false;
    userName = loginForm.UserName;
    password = loginForm.Password;
    return true;
  }

  private void panel1_Paint(object sender, PaintEventArgs e)
  {
    Rectangle displayRectangle = this.panel1.DisplayRectangle;
    Bitmap image = (Bitmap) this.pictureBox1.Image;
    ControlPaint.Dark(image.GetPixel(image.Width - 1, 1), 0.3f);
    using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(displayRectangle, Color.FromArgb(41, 71, 219), Color.FromArgb(27, 50, 160 /*0xA0*/), LinearGradientMode.Horizontal))
      e.Graphics.FillRectangle((Brush) linearGradientBrush, displayRectangle);
    e.Graphics.DrawLine(Pens.White, displayRectangle.Left, displayRectangle.Height - 2, displayRectangle.Right - 1, displayRectangle.Height - 2);
    e.Graphics.DrawLine(Pens.White, displayRectangle.Left, displayRectangle.Height - 1, displayRectangle.Right - 1, displayRectangle.Height - 1);
  }

  private void LoginForm_Layout(object sender, LayoutEventArgs e)
  {
    Rectangle displayRectangle = this.DisplayRectangle;
    this.panel1.Height = this.pictureBox1.Height;
    this.panel1.Top = this.pictureBox1.Top;
    this.panel1.Left = this.pictureBox1.Right;
    this.panel1.Width = displayRectangle.Width - this.panel1.Left - 1;
  }

  private void LoginForm_Resize(object sender, EventArgs e)
  {
    this.LoginForm_Layout((object) null, (LayoutEventArgs) null);
  }
}
