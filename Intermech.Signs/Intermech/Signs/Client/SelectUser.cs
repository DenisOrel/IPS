// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SelectUser
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Окно ввода информации пользователя (Имя и пароль)</summary>
public class SelectUser : Form
{
  private Panel panel1;
  private Panel panel2;
  private Button button1;
  private Button button2;
  private Label label1;
  private Label label2;
  private TextBox textBox1;
  private TextBox textBox2;
  private PictureBox pictureBox1;
  private Panel panel3;
  private Panel panel4;
  private System.ComponentModel.Container components;

  /// <summary>Конструктор</summary>
  public SelectUser() => this.InitializeComponent();

  /// <summary>Возвращает имя(логин) пользователя</summary>
  public string UserName => this.textBox1.Text;

  /// <summary>Возвращает пароль пользователя</summary>
  public string Password => this.textBox2.Text;

  /// <summary>
  /// 
  /// </summary>
  /// <param name="disposing"></param>
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectUser));
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this.button2 = new Button();
    this.button1 = new Button();
    this.label1 = new Label();
    this.label2 = new Label();
    this.textBox1 = new TextBox();
    this.textBox2 = new TextBox();
    this.pictureBox1 = new PictureBox();
    this.panel3 = new Panel();
    this.panel4 = new Panel();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.panel3.SuspendLayout();
    this.panel4.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.panel2);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    this.panel2.Controls.Add((Control) this.button2);
    this.panel2.Controls.Add((Control) this.button1);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this.button2.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.button2, "button2");
    this.button2.Name = "button2";
    componentResourceManager.ApplyResources((object) this.button1, "button1");
    this.button1.Name = "button1";
    this.button1.Click += new EventHandler(this.button1_Click);
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.Name = "label2";
    componentResourceManager.ApplyResources((object) this.textBox1, "textBox1");
    this.textBox1.Name = "textBox1";
    componentResourceManager.ApplyResources((object) this.textBox2, "textBox2");
    this.textBox2.Name = "textBox2";
    componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.TabStop = false;
    this.panel3.Controls.Add((Control) this.textBox1);
    this.panel3.Controls.Add((Control) this.label1);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    this.panel4.Controls.Add((Control) this.textBox2);
    this.panel4.Controls.Add((Control) this.label2);
    componentResourceManager.ApplyResources((object) this.panel4, "panel4");
    this.panel4.Name = "panel4";
    this.AcceptButton = (IButtonControl) this.button1;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.CancelButton = (IButtonControl) this.button2;
    this.Controls.Add((Control) this.panel4);
    this.Controls.Add((Control) this.panel3);
    this.Controls.Add((Control) this.pictureBox1);
    this.Controls.Add((Control) this.panel1);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SelectUser);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.panel3.ResumeLayout(false);
    this.panel3.PerformLayout();
    this.panel4.ResumeLayout(false);
    this.panel4.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  /// <summary>Кнопка "Ок"</summary>
  private void button1_Click(object sender, EventArgs e)
  {
    if (!this.textBox1.Text.Equals(string.Empty))
    {
      this.Close();
      this.DialogResult = DialogResult.OK;
    }
    else
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("Signs_88"));
    }
  }
}
