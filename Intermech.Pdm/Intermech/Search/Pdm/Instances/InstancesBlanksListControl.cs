// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Instances.InstancesBlanksListControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.Instances;

public sealed class InstancesBlanksListControl : UserControl
{
  private InstanceBlank[] _blanks;
  private IContainer components;
  private ListView _listView;
  private Label label1;
  private ColumnHeader _designationColumnHeader;
  private ColumnHeader _numberColumnHeader;

  public InstancesBlanksListControl() => this.InitializeComponent();

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public InstanceBlank[] Blanks
  {
    get => this._blanks;
    set
    {
      if (value == this._blanks)
        return;
      this._blanks = value;
      this._listView.BeginUpdate();
      try
      {
        this._listView.Items.Clear();
        if (this._blanks == null)
          return;
        foreach (InstanceBlank blank in this._blanks)
          this._listView.Items.Add(new ListViewItem(new string[2]
          {
            blank.Designation,
            blank.Number
          }));
      }
      finally
      {
        this._listView.EndUpdate();
      }
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
    this._listView = new ListView();
    this._designationColumnHeader = new ColumnHeader();
    this._numberColumnHeader = new ColumnHeader();
    this.label1 = new Label();
    this.SuspendLayout();
    this._listView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this._listView.Columns.AddRange(new ColumnHeader[2]
    {
      this._designationColumnHeader,
      this._numberColumnHeader
    });
    this._listView.GridLines = true;
    this._listView.Location = new Point(6, 27);
    this._listView.Name = "_listView";
    this._listView.Size = new Size(811, 224 /*0xE0*/);
    this._listView.TabIndex = 0;
    this._listView.UseCompatibleStateImageBehavior = false;
    this._listView.View = View.Details;
    this._designationColumnHeader.Text = "Обозначение исполнения";
    this._designationColumnHeader.Width = 300;
    this._numberColumnHeader.Text = "Номер исполнения";
    this._numberColumnHeader.Width = 150;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(3, 0);
    this.label1.Name = "label1";
    this.label1.Size = new Size(72, 13);
    this.label1.TabIndex = 1;
    this.label1.Text = "Исполнения:";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this._listView);
    this.Name = nameof (InstancesBlanksListControl);
    this.Size = new Size(820, 254);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
