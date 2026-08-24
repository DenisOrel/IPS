// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ImportFileSelector
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

public class ImportFileSelector : Form
{
  private IContainer components;
  private ListBox listBox1;
  private Button buttonOK;
  private Button buttonCancel;

  public ImportFileSelector(string[] files)
  {
    this.InitializeComponent();
    if (files != null && files.Length != 0)
    {
      this.listBox1.Items.AddRange((object[]) files);
      this.listBox1.SelectedIndex = 0;
    }
    this.listBox1_SelectedValueChanged((object) this, (EventArgs) null);
  }

  private void listBox1_SelectedValueChanged(object sender, EventArgs e)
  {
    this.buttonOK.Enabled = this.listBox1.SelectedItems.Count > 0;
  }

  public string[] SelectedFiles
  {
    get
    {
      if (this.listBox1.SelectedItems.Count == 0)
        return (string[]) null;
      List<string> stringList = new List<string>();
      foreach (object selectedItem in this.listBox1.SelectedItems)
        stringList.Add(selectedItem.ToString());
      return stringList.ToArray();
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
    this.listBox1 = new ListBox();
    this.buttonOK = new Button();
    this.buttonCancel = new Button();
    this.SuspendLayout();
    this.listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.listBox1.FormattingEnabled = true;
    this.listBox1.Location = new Point(12, 12);
    this.listBox1.Name = "listBox1";
    this.listBox1.SelectionMode = SelectionMode.MultiExtended;
    this.listBox1.Size = new Size(442, 290);
    this.listBox1.TabIndex = 0;
    this.listBox1.SelectedValueChanged += new EventHandler(this.listBox1_SelectedValueChanged);
    this.buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.buttonOK.DialogResult = DialogResult.OK;
    this.buttonOK.Location = new Point(206, 324);
    this.buttonOK.Name = "buttonOK";
    this.buttonOK.Size = new Size(121, 27);
    this.buttonOK.TabIndex = 2;
    this.buttonOK.Text = "ОК";
    this.buttonOK.UseVisualStyleBackColor = true;
    this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.buttonCancel.DialogResult = DialogResult.Cancel;
    this.buttonCancel.Location = new Point(333, 324);
    this.buttonCancel.Name = "buttonCancel";
    this.buttonCancel.Size = new Size(121, 27);
    this.buttonCancel.TabIndex = 3;
    this.buttonCancel.Text = "Отмена";
    this.buttonCancel.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.buttonOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.buttonCancel;
    this.ClientSize = new Size(466, 369);
    this.Controls.Add((Control) this.buttonCancel);
    this.Controls.Add((Control) this.buttonOK);
    this.Controls.Add((Control) this.listBox1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.MinimumSize = new Size(289, 237);
    this.Name = nameof (ImportFileSelector);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Доступные файлы для импорта";
    this.ResumeLayout(false);
  }
}
