// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.RadioGroupDialog
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace Intermech.MRP2;

public class RadioGroupDialog : Form
{
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Button cancelBtn;
  private Button okBtn;
  private GroupBox groupBox1;

  public RadioGroupDialog(string Caption, string RadioCaption, string[] items)
  {
    this.InitializeComponent();
    this.Text = Caption;
    this.groupBox1.Text = RadioCaption;
    this.SuspendLayout();
    RadioGroupDialog.FillGroupBoxControl(this.groupBox1, items);
    this.ResumeLayout();
  }

  public static void FillGroupBoxControl(GroupBox box, string[] items)
  {
    int y = 19;
    int num = 0;
    bool flag = true;
    foreach (string str in items)
    {
      RadioButton radioButton = new RadioButton();
      box.Controls.Add((Control) radioButton);
      radioButton.AutoSize = true;
      radioButton.Location = new Point(6, y);
      y += 23;
      radioButton.Size = new Size(85, 17);
      radioButton.TabIndex = num++;
      radioButton.TabStop = true;
      radioButton.Text = str;
      radioButton.UseVisualStyleBackColor = true;
      radioButton.Checked = flag;
      flag = false;
    }
  }

  public string Selected
  {
    get
    {
      foreach (Control control in (ArrangedElementCollection) this.groupBox1.Controls)
      {
        if (control is RadioButton && (control as RadioButton).Checked)
          return (control as RadioButton).Text;
      }
      return "";
    }
  }

  public static DialogResult ExecuteDialog(
    string Caption,
    string RadioCaption,
    string[] items,
    out string Value)
  {
    RadioGroupDialog radioGroupDialog = new RadioGroupDialog(Caption, RadioCaption, items);
    int num = (int) radioGroupDialog.ShowDialog();
    if (num == 1)
    {
      Value = radioGroupDialog.Selected;
      return (DialogResult) num;
    }
    Value = "";
    return (DialogResult) num;
  }

  public static DialogResult ExecuteDialog(
    string Caption,
    string RadioCaption,
    Type enumType,
    out object Value)
  {
    string[] names = Enum.GetNames(enumType);
    string[] strArray = new string[names.Length];
    for (int index = 0; index < names.Length; ++index)
    {
      FieldInfo field = enumType.GetField(names[index]);
      if (field != (FieldInfo) null && Attribute.GetCustomAttribute((MemberInfo) field, typeof (DescriptionAttribute)) is DescriptionAttribute customAttribute)
        strArray[index] = customAttribute.Description;
    }
    string str;
    int num = (int) RadioGroupDialog.ExecuteDialog(Caption, RadioCaption, strArray, out str);
    if (num == 1)
    {
      int index = Array.IndexOf<string>(strArray, str);
      Value = Enum.Parse(enumType, names[index]);
      return (DialogResult) num;
    }
    Value = (object) null;
    return (DialogResult) num;
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
    this.cancelBtn = new Button();
    this.okBtn = new Button();
    this.groupBox1 = new GroupBox();
    this.SuspendLayout();
    this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.cancelBtn.DialogResult = DialogResult.Cancel;
    this.cancelBtn.Location = new Point(244, 216);
    this.cancelBtn.Name = "cancelBtn";
    this.cancelBtn.Size = new Size(75, 23);
    this.cancelBtn.TabIndex = 0;
    this.cancelBtn.Text = "Отмена";
    this.cancelBtn.UseVisualStyleBackColor = true;
    this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.okBtn.DialogResult = DialogResult.OK;
    this.okBtn.Location = new Point(163, 216);
    this.okBtn.Name = "okBtn";
    this.okBtn.Size = new Size(75, 23);
    this.okBtn.TabIndex = 1;
    this.okBtn.Text = "&OK";
    this.okBtn.UseVisualStyleBackColor = true;
    this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox1.Location = new Point(12, 12);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(307, 198);
    this.groupBox1.TabIndex = 3;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "groupBox1";
    this.AcceptButton = (IButtonControl) this.okBtn;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.cancelBtn;
    this.ClientSize = new Size(327, 246);
    this.Controls.Add((Control) this.groupBox1);
    this.Controls.Add((Control) this.okBtn);
    this.Controls.Add((Control) this.cancelBtn);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (RadioGroupDialog);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = nameof (RadioGroupDialog);
    this.ResumeLayout(false);
  }
}
