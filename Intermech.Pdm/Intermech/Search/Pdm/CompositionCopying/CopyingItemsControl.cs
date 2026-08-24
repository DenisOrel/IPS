// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionCopying.CopyingItemsControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Search.GroupAttributesChanging;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionCopying;

public class CopyingItemsControl : UserControl
{
  private IContainer components;
  private GroupBox groupBox4;
  private TableLayoutPanel tableLayoutPanel12;
  private Label label3;
  private Label label4;
  private TableLayoutPanel tableLayoutPanel13;
  private TextBox _findWhatTextBox;
  private Button _selectButton;
  private Button _selectAllButton;
  private TableLayoutPanel tableLayoutPanel14;
  private Button _checkUncheckButton;
  private Label label5;

  public CopyingItemsControl()
  {
    this.InitializeComponent();
    this.UpdateControls();
  }

  public event EventHandler SelectButtonClicked;

  public event EventHandler SelectAllButtonClicked;

  public event EventHandler CheckUncheckButtonClicked;

  public Regex FindWhat { get; private set; }

  public bool CheckUncheckButtonEnabled
  {
    get => this._checkUncheckButton.Enabled;
    set => this._checkUncheckButton.Enabled = value;
  }

  private void FindWhatTextBox_TextChanged(object sender, EventArgs e)
  {
    if (!string.IsNullOrEmpty(this._findWhatTextBox.Text))
      this.FindWhat = new FindWhatBuilder()
      {
        Text = this._findWhatTextBox.Text
      }.GetResult();
    else
      this.FindWhat = (Regex) null;
    this.UpdateControls();
  }

  private void SelectButton_Click(object sender, EventArgs e)
  {
    EventHandler selectButtonClicked = this.SelectButtonClicked;
    if (selectButtonClicked == null)
      return;
    selectButtonClicked((object) this, EventArgs.Empty);
  }

  private void SelectAllButton_Click(object sender, EventArgs e)
  {
    EventHandler allButtonClicked = this.SelectAllButtonClicked;
    if (allButtonClicked == null)
      return;
    allButtonClicked((object) this, EventArgs.Empty);
  }

  private void CheckUncheckButton_Click(object sender, EventArgs e)
  {
    if (this.CheckUncheckButtonClicked == null)
      return;
    this.CheckUncheckButtonClicked((object) this, EventArgs.Empty);
  }

  private void UpdateControls()
  {
    this._selectButton.Enabled = this._selectAllButton.Enabled = this.FindWhat != null;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.groupBox4 = new GroupBox();
    this.tableLayoutPanel12 = new TableLayoutPanel();
    this.label3 = new Label();
    this.label4 = new Label();
    this.tableLayoutPanel13 = new TableLayoutPanel();
    this._findWhatTextBox = new TextBox();
    this._selectButton = new Button();
    this._selectAllButton = new Button();
    this.tableLayoutPanel14 = new TableLayoutPanel();
    this._checkUncheckButton = new Button();
    this.label5 = new Label();
    this.groupBox4.SuspendLayout();
    this.tableLayoutPanel12.SuspendLayout();
    this.tableLayoutPanel13.SuspendLayout();
    this.tableLayoutPanel14.SuspendLayout();
    this.SuspendLayout();
    this.groupBox4.AutoSize = true;
    this.groupBox4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.groupBox4.Controls.Add((Control) this.tableLayoutPanel12);
    this.groupBox4.Dock = DockStyle.Fill;
    this.groupBox4.Location = new Point(0, 0);
    this.groupBox4.Name = "groupBox4";
    this.groupBox4.Size = new Size(461, 371);
    this.groupBox4.TabIndex = 2;
    this.groupBox4.TabStop = false;
    this.groupBox4.Text = "Копирование элементов";
    this.tableLayoutPanel12.AutoSize = true;
    this.tableLayoutPanel12.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.tableLayoutPanel12.ColumnCount = 1;
    this.tableLayoutPanel12.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel12.Controls.Add((Control) this.label3, 0, 0);
    this.tableLayoutPanel12.Controls.Add((Control) this.label4, 0, 1);
    this.tableLayoutPanel12.Controls.Add((Control) this.tableLayoutPanel13, 0, 2);
    this.tableLayoutPanel12.Controls.Add((Control) this.tableLayoutPanel14, 0, 3);
    this.tableLayoutPanel12.Dock = DockStyle.Fill;
    this.tableLayoutPanel12.Location = new Point(3, 16 /*0x10*/);
    this.tableLayoutPanel12.Name = "tableLayoutPanel12";
    this.tableLayoutPanel12.RowCount = 5;
    this.tableLayoutPanel12.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel12.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel12.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel12.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel12.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel12.Size = new Size(455, 352);
    this.tableLayoutPanel12.TabIndex = 0;
    this.label3.AutoSize = true;
    this.label3.Dock = DockStyle.Fill;
    this.label3.Location = new Point(3, 0);
    this.label3.Name = "label3";
    this.label3.Size = new Size(449, 26);
    this.label3.TabIndex = 0;
    this.label3.Text = "Для отмеченных элементов будут созданы копии и включены в состав вышестоящих элементов";
    this.label4.AutoSize = true;
    this.label4.Dock = DockStyle.Fill;
    this.label4.Location = new Point(3, 26);
    this.label4.Name = "label4";
    this.label4.Size = new Size(449, 13);
    this.label4.TabIndex = 1;
    this.label4.Text = "При копировании нижестоящего элемента вышестоящий копируется автоматически";
    this.tableLayoutPanel13.AutoSize = true;
    this.tableLayoutPanel13.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.tableLayoutPanel13.ColumnCount = 3;
    this.tableLayoutPanel13.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel13.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel13.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel13.Controls.Add((Control) this._findWhatTextBox, 0, 0);
    this.tableLayoutPanel13.Controls.Add((Control) this._selectButton, 1, 0);
    this.tableLayoutPanel13.Controls.Add((Control) this._selectAllButton, 2, 0);
    this.tableLayoutPanel13.Dock = DockStyle.Fill;
    this.tableLayoutPanel13.Location = new Point(3, 42);
    this.tableLayoutPanel13.Name = "tableLayoutPanel13";
    this.tableLayoutPanel13.RowCount = 1;
    this.tableLayoutPanel13.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel13.Size = new Size(449, 29);
    this.tableLayoutPanel13.TabIndex = 2;
    this._findWhatTextBox.Dock = DockStyle.Fill;
    this._findWhatTextBox.Location = new Point(3, 3);
    this._findWhatTextBox.Name = "_findWhatTextBox";
    this._findWhatTextBox.Size = new Size(288, 20);
    this._findWhatTextBox.TabIndex = 0;
    this._findWhatTextBox.TextChanged += new EventHandler(this.FindWhatTextBox_TextChanged);
    this._selectButton.AutoSize = true;
    this._selectButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this._selectButton.Location = new Point(297, 3);
    this._selectButton.Name = "_selectButton";
    this._selectButton.Size = new Size(61, 23);
    this._selectButton.TabIndex = 1;
    this._selectButton.Text = "Выбрать";
    this._selectButton.UseVisualStyleBackColor = true;
    this._selectButton.Click += new EventHandler(this.SelectButton_Click);
    this._selectAllButton.AutoSize = true;
    this._selectAllButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this._selectAllButton.Location = new Point(364, 3);
    this._selectAllButton.Name = "_selectAllButton";
    this._selectAllButton.Size = new Size(82, 23);
    this._selectAllButton.TabIndex = 2;
    this._selectAllButton.Text = "Выбрать все";
    this._selectAllButton.UseVisualStyleBackColor = true;
    this._selectAllButton.Click += new EventHandler(this.SelectAllButton_Click);
    this.tableLayoutPanel14.AutoSize = true;
    this.tableLayoutPanel14.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.tableLayoutPanel14.ColumnCount = 2;
    this.tableLayoutPanel14.ColumnStyles.Add(new ColumnStyle());
    this.tableLayoutPanel14.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel14.Controls.Add((Control) this._checkUncheckButton, 0, 0);
    this.tableLayoutPanel14.Controls.Add((Control) this.label5, 1, 0);
    this.tableLayoutPanel14.Dock = DockStyle.Fill;
    this.tableLayoutPanel14.Location = new Point(3, 77);
    this.tableLayoutPanel14.Name = "tableLayoutPanel14";
    this.tableLayoutPanel14.RowCount = 1;
    this.tableLayoutPanel14.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel14.Size = new Size(449, 29);
    this.tableLayoutPanel14.TabIndex = 3;
    this._checkUncheckButton.AutoSize = true;
    this._checkUncheckButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this._checkUncheckButton.Location = new Point(3, 3);
    this._checkUncheckButton.Name = "_checkUncheckButton";
    this._checkUncheckButton.Size = new Size(24, 23);
    this._checkUncheckButton.TabIndex = 0;
    this._checkUncheckButton.Text = "X";
    this._checkUncheckButton.UseVisualStyleBackColor = true;
    this._checkUncheckButton.Click += new EventHandler(this.CheckUncheckButton_Click);
    this.label5.AutoSize = true;
    this.label5.Dock = DockStyle.Fill;
    this.label5.Location = new Point(33, 0);
    this.label5.Name = "label5";
    this.label5.Size = new Size(413, 29);
    this.label5.TabIndex = 1;
    this.label5.Text = "Отметить\\ Снять отметки";
    this.label5.TextAlign = ContentAlignment.MiddleLeft;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.groupBox4);
    this.Name = nameof (CopyingItemsControl);
    this.Size = new Size(461, 371);
    this.groupBox4.ResumeLayout(false);
    this.groupBox4.PerformLayout();
    this.tableLayoutPanel12.ResumeLayout(false);
    this.tableLayoutPanel12.PerformLayout();
    this.tableLayoutPanel13.ResumeLayout(false);
    this.tableLayoutPanel13.PerformLayout();
    this.tableLayoutPanel14.ResumeLayout(false);
    this.tableLayoutPanel14.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
