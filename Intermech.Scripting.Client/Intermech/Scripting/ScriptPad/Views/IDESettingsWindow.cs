// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.Views.IDESettingsWindow
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Mvp;
using Intermech.Mvp.Components;
using Intermech.Mvp.Winforms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.ScriptPad.Views;

internal class IDESettingsWindow : 
  MvpWindow,
  IIDESettingsEditorView,
  IView,
  IOperationConfirmationView
{
  private IContainer components;
  private GroupBox gbCodeEditors;
  private Button btSelectFont;
  private TextBox tbFontSize;
  private Label lbFontSize;
  private Label lbFontFamily;
  private TextBox tbFontFamily;
  private FontDialog dlgSelectFont;
  private Button btOK;
  private Button btCancel;
  private CheckBox cbEnableCodeCompletion;
  private GroupBox gbCodeCompletion;
  private Label lbXmlDocPathList;
  private TextBox tbXmlDocPathList;

  public IDESettingsWindow() => this.InitializeComponent();

  public string FontFamily
  {
    get => this.tbFontFamily.Text;
    set => this.tbFontFamily.Text = value;
  }

  public string FontSize
  {
    get => this.tbFontSize.Text;
    set => this.tbFontSize.Text = value;
  }

  public bool EnableCodeCompletion
  {
    get => this.cbEnableCodeCompletion.Checked;
    set => this.cbEnableCodeCompletion.Checked = value;
  }

  public ICollection<string> XmlDocPathList
  {
    get
    {
      return (ICollection<string>) this.tbXmlDocPathList.Text.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.RemoveEmptyEntries);
    }
    set
    {
      if (value != null && value.Count != 0)
        this.tbXmlDocPathList.Text = string.Join(Environment.NewLine, (IEnumerable<string>) value);
      else
        this.tbXmlDocPathList.Text = string.Empty;
    }
  }

  public event EventHandler OperationConfirmed;

  private void btSelectFont_Click(object sender, EventArgs e)
  {
    Font currentFont = this.TryCreateCurrentFont();
    try
    {
      this.dlgSelectFont.Font = currentFont;
      if (this.dlgSelectFont.ShowDialog() != DialogResult.OK)
        return;
      this.TryUpdateCurrentFont(this.dlgSelectFont.Font);
    }
    finally
    {
      if (currentFont != null)
      {
        this.dlgSelectFont.Font = (Font) null;
        currentFont.Dispose();
      }
    }
  }

  private Font TryCreateCurrentFont()
  {
    try
    {
      return new Font(this.tbFontFamily.Text, float.Parse(this.tbFontSize.Text), GraphicsUnit.Point);
    }
    catch
    {
      return (Font) null;
    }
  }

  private void TryUpdateCurrentFont(Font font)
  {
    this.tbFontFamily.Text = font.FontFamily.Name;
    this.tbFontSize.Text = Math.Round((double) font.SizeInPoints).ToString();
  }

  private void btOK_Click(object sender, EventArgs e)
  {
    try
    {
      EventHandler operationConfirmed = this.OperationConfirmed;
      if (operationConfirmed == null)
        return;
      operationConfirmed(sender, EventArgs.Empty);
    }
    catch (ApplicationException ex)
    {
      int num = (int) MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      this.DialogResult = DialogResult.None;
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
    this.gbCodeEditors = new GroupBox();
    this.btSelectFont = new Button();
    this.tbFontSize = new TextBox();
    this.lbFontSize = new Label();
    this.lbFontFamily = new Label();
    this.tbFontFamily = new TextBox();
    this.dlgSelectFont = new FontDialog();
    this.btOK = new Button();
    this.btCancel = new Button();
    this.cbEnableCodeCompletion = new CheckBox();
    this.gbCodeCompletion = new GroupBox();
    this.tbXmlDocPathList = new TextBox();
    this.lbXmlDocPathList = new Label();
    this.gbCodeEditors.SuspendLayout();
    this.gbCodeCompletion.SuspendLayout();
    this.SuspendLayout();
    this.gbCodeEditors.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.gbCodeEditors.Controls.Add((Control) this.btSelectFont);
    this.gbCodeEditors.Controls.Add((Control) this.tbFontSize);
    this.gbCodeEditors.Controls.Add((Control) this.lbFontSize);
    this.gbCodeEditors.Controls.Add((Control) this.lbFontFamily);
    this.gbCodeEditors.Controls.Add((Control) this.tbFontFamily);
    this.gbCodeEditors.Location = new Point(17, 17);
    this.gbCodeEditors.Margin = new Padding(8);
    this.gbCodeEditors.Name = "gbCodeEditors";
    this.gbCodeEditors.Size = new Size(620, 85);
    this.gbCodeEditors.TabIndex = 0;
    this.gbCodeEditors.TabStop = false;
    this.gbCodeEditors.Text = "Редакторы кода";
    this.btSelectFont.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.btSelectFont.Location = new Point(539, 35);
    this.btSelectFont.Name = "btSelectFont";
    this.btSelectFont.Size = new Size(75, 23);
    this.btSelectFont.TabIndex = 4;
    this.btSelectFont.Text = "Выбрать";
    this.btSelectFont.UseVisualStyleBackColor = true;
    this.btSelectFont.Click += new EventHandler(this.btSelectFont_Click);
    this.tbFontSize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.tbFontSize.BackColor = SystemColors.Window;
    this.tbFontSize.Location = new Point(473, 37);
    this.tbFontSize.Name = "tbFontSize";
    this.tbFontSize.ReadOnly = true;
    this.tbFontSize.Size = new Size(46, 20);
    this.tbFontSize.TabIndex = 3;
    this.tbFontSize.TabStop = false;
    this.lbFontSize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.lbFontSize.AutoSize = true;
    this.lbFontSize.Location = new Point(421, 40);
    this.lbFontSize.Name = "lbFontSize";
    this.lbFontSize.Size = new Size(46, 13);
    this.lbFontSize.TabIndex = 2;
    this.lbFontSize.Text = "Размер";
    this.lbFontFamily.AutoSize = true;
    this.lbFontFamily.Location = new Point(6, 40);
    this.lbFontFamily.Name = "lbFontFamily";
    this.lbFontFamily.Size = new Size(41, 13);
    this.lbFontFamily.TabIndex = 0;
    this.lbFontFamily.Text = "Шрифт";
    this.tbFontFamily.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbFontFamily.BackColor = SystemColors.Window;
    this.tbFontFamily.Location = new Point(53, 37);
    this.tbFontFamily.Name = "tbFontFamily";
    this.tbFontFamily.ReadOnly = true;
    this.tbFontFamily.Size = new Size(362, 20);
    this.tbFontFamily.TabIndex = 1;
    this.tbFontFamily.TabStop = false;
    this.dlgSelectFont.FixedPitchOnly = true;
    this.dlgSelectFont.ShowEffects = false;
    this.btOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btOK.DialogResult = DialogResult.OK;
    this.btOK.Location = new Point(486, 386);
    this.btOK.Name = "btOK";
    this.btOK.Size = new Size(75, 23);
    this.btOK.TabIndex = 2;
    this.btOK.Text = "OK";
    this.btOK.UseVisualStyleBackColor = true;
    this.btOK.Click += new EventHandler(this.btOK_Click);
    this.btCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btCancel.DialogResult = DialogResult.Cancel;
    this.btCancel.Location = new Point(567, 386);
    this.btCancel.Name = "btCancel";
    this.btCancel.Size = new Size(75, 23);
    this.btCancel.TabIndex = 3;
    this.btCancel.Text = "Отмена";
    this.btCancel.UseVisualStyleBackColor = true;
    this.cbEnableCodeCompletion.AutoSize = true;
    this.cbEnableCodeCompletion.Location = new Point(9, 37);
    this.cbEnableCodeCompletion.Name = "cbEnableCodeCompletion";
    this.cbEnableCodeCompletion.Size = new Size(348, 17);
    this.cbEnableCodeCompletion.TabIndex = 0;
    this.cbEnableCodeCompletion.Text = "Включить автодополнение кода и подсказки сигнатур методов";
    this.cbEnableCodeCompletion.UseVisualStyleBackColor = true;
    this.gbCodeCompletion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.gbCodeCompletion.Controls.Add((Control) this.lbXmlDocPathList);
    this.gbCodeCompletion.Controls.Add((Control) this.tbXmlDocPathList);
    this.gbCodeCompletion.Controls.Add((Control) this.cbEnableCodeCompletion);
    this.gbCodeCompletion.Location = new Point(17, 118);
    this.gbCodeCompletion.Margin = new Padding(8);
    this.gbCodeCompletion.Name = "gbCodeCompletion";
    this.gbCodeCompletion.Size = new Size(620, 220);
    this.gbCodeCompletion.TabIndex = 1;
    this.gbCodeCompletion.TabStop = false;
    this.gbCodeCompletion.Text = "Автодополнение кода";
    this.tbXmlDocPathList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tbXmlDocPathList.Location = new Point(9, 82);
    this.tbXmlDocPathList.Multiline = true;
    this.tbXmlDocPathList.Name = "tbXmlDocPathList";
    this.tbXmlDocPathList.ScrollBars = ScrollBars.Both;
    this.tbXmlDocPathList.Size = new Size(605, 117);
    this.tbXmlDocPathList.TabIndex = 2;
    this.tbXmlDocPathList.WordWrap = false;
    this.lbXmlDocPathList.AutoSize = true;
    this.lbXmlDocPathList.Location = new Point(6, 66);
    this.lbXmlDocPathList.Name = "lbXmlDocPathList";
    this.lbXmlDocPathList.Size = new Size(233, 13);
    this.lbXmlDocPathList.TabIndex = 1;
    this.lbXmlDocPathList.Text = "Список путей к папкам с xml-документацией";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btCancel;
    this.ClientSize = new Size(654, 421);
    this.Controls.Add((Control) this.gbCodeCompletion);
    this.Controls.Add((Control) this.btCancel);
    this.Controls.Add((Control) this.btOK);
    this.Controls.Add((Control) this.gbCodeEditors);
    this.FormBorderStyle = FormBorderStyle.FixedDialog;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.MinimumSize = new Size(670, 460);
    this.Name = nameof (IDESettingsWindow);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Общие настройки";
    this.gbCodeEditors.ResumeLayout(false);
    this.gbCodeEditors.PerformLayout();
    this.gbCodeCompletion.ResumeLayout(false);
    this.gbCodeCompletion.PerformLayout();
    this.ResumeLayout(false);
  }
}
