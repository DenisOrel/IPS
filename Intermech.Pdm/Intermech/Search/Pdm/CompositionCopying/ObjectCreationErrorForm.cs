// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.CompositionCopying.ObjectCreationErrorForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Search.Utilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.CompositionCopying;

public sealed class ObjectCreationErrorForm : Form
{
  private ObjectCreationErrorForm.ObjectCreationErrorAction _disabledActions;
  private IContainer components;
  private TableLayoutPanel tableLayoutPanel1;
  private Label _label;
  private FlowLayoutPanel flowLayoutPanel1;
  private Button _useExistingObjectButton;
  private Button _excludeObjectFromCompositionButton;
  private Button _useExistingObjectForAllButton;
  private Button _excludeObjectFromCompositionForAllButton;
  private Button _abortButton;

  public ObjectCreationErrorForm()
  {
    this.InitializeComponent();
    this.DisabledActions = ObjectCreationErrorForm.ObjectCreationErrorAction.None;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public long PrototypeVersionId { get; set; }

  public string Error
  {
    get => this._label.Text;
    set => this._label.Text = value;
  }

  public ObjectCreationErrorForm.ObjectCreationErrorAction DisabledActions
  {
    get => this._disabledActions;
    set
    {
      if (this._disabledActions == value)
        return;
      this._disabledActions = value;
      this._useExistingObjectButton.Enabled = !this._disabledActions.HasFlag((Enum) ObjectCreationErrorForm.ObjectCreationErrorAction.UseExistingObject);
      this._useExistingObjectForAllButton.Enabled = !this._disabledActions.HasFlag((Enum) ObjectCreationErrorForm.ObjectCreationErrorAction.UseExistingObjectForAll);
      this._abortButton.Enabled = !this._disabledActions.HasFlag((Enum) ObjectCreationErrorForm.ObjectCreationErrorAction.Abort);
      this._excludeObjectFromCompositionButton.Enabled = !this._disabledActions.HasFlag((Enum) ObjectCreationErrorForm.ObjectCreationErrorAction.ExcludeFromComposition);
      this._excludeObjectFromCompositionForAllButton.Enabled = !this._disabledActions.HasFlag((Enum) ObjectCreationErrorForm.ObjectCreationErrorAction.ExcludeFromCompositionForAll);
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public ObjectCreationErrorForm.ObjectCreationErrorAction Action { get; private set; }

  private void ObjectCreationErrorForm_Load(object sender, EventArgs e)
  {
    this.Text = ObjectHelper.IsUnknownObjectVersionID(this.PrototypeVersionId) ? "Ошибка создания объекта по прототипу" : $"Ошибка создания объекта по прототипу #{this.PrototypeVersionId}";
  }

  private void UseExistingObjectButton_Click(object sender, EventArgs e)
  {
    this.SetActionAndClose(ObjectCreationErrorForm.ObjectCreationErrorAction.UseExistingObject);
  }

  private void ExcludeObjectFromCompositionButton_Click(object sender, EventArgs e)
  {
    this.SetActionAndClose(ObjectCreationErrorForm.ObjectCreationErrorAction.ExcludeFromComposition);
  }

  private void UseExistingObjectForAllButton_Click(object sender, EventArgs e)
  {
    this.SetActionAndClose(ObjectCreationErrorForm.ObjectCreationErrorAction.UseExistingObjectForAll);
  }

  private void ExcludeObjectFromCompositionForAllButton_Click(object sender, EventArgs e)
  {
    this.SetActionAndClose(ObjectCreationErrorForm.ObjectCreationErrorAction.ExcludeFromCompositionForAll);
  }

  private void AbortButton_Click(object sender, EventArgs e)
  {
    this.SetActionAndClose(ObjectCreationErrorForm.ObjectCreationErrorAction.Abort);
  }

  private void SetActionAndClose(
    ObjectCreationErrorForm.ObjectCreationErrorAction action)
  {
    this.Action = action;
    this.Close();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this._label = new Label();
    this.flowLayoutPanel1 = new FlowLayoutPanel();
    this._useExistingObjectButton = new Button();
    this._excludeObjectFromCompositionButton = new Button();
    this._useExistingObjectForAllButton = new Button();
    this._excludeObjectFromCompositionForAllButton = new Button();
    this._abortButton = new Button();
    this.tableLayoutPanel1.SuspendLayout();
    this.flowLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    this.tableLayoutPanel1.ColumnCount = 1;
    this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.Controls.Add((Control) this._label, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.flowLayoutPanel1, 0, 1);
    this.tableLayoutPanel1.Dock = DockStyle.Fill;
    this.tableLayoutPanel1.Location = new Point(0, 0);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.RowCount = 2;
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
    this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
    this.tableLayoutPanel1.Size = new Size(484, 211);
    this.tableLayoutPanel1.TabIndex = 0;
    this._label.AutoSize = true;
    this._label.Dock = DockStyle.Fill;
    this._label.Location = new Point(3, 0);
    this._label.Name = "_label";
    this._label.Size = new Size(478, 118);
    this._label.TabIndex = 0;
    this.flowLayoutPanel1.AutoSize = true;
    this.flowLayoutPanel1.Controls.Add((Control) this._useExistingObjectButton);
    this.flowLayoutPanel1.Controls.Add((Control) this._excludeObjectFromCompositionButton);
    this.flowLayoutPanel1.Controls.Add((Control) this._useExistingObjectForAllButton);
    this.flowLayoutPanel1.Controls.Add((Control) this._excludeObjectFromCompositionForAllButton);
    this.flowLayoutPanel1.Controls.Add((Control) this._abortButton);
    this.flowLayoutPanel1.Dock = DockStyle.Fill;
    this.flowLayoutPanel1.Location = new Point(3, 121);
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    this.flowLayoutPanel1.Size = new Size(478, 87);
    this.flowLayoutPanel1.TabIndex = 1;
    this._useExistingObjectButton.AutoSize = true;
    this._useExistingObjectButton.Location = new Point(3, 3);
    this._useExistingObjectButton.Name = "_useExistingObjectButton";
    this._useExistingObjectButton.Size = new Size(209, 23);
    this._useExistingObjectButton.TabIndex = 0;
    this._useExistingObjectButton.Text = "Использовать существующий объект";
    this._useExistingObjectButton.UseVisualStyleBackColor = true;
    this._useExistingObjectButton.Click += new EventHandler(this.UseExistingObjectButton_Click);
    this._excludeObjectFromCompositionButton.AutoSize = true;
    this._excludeObjectFromCompositionButton.Location = new Point(218, 3);
    this._excludeObjectFromCompositionButton.Name = "_excludeObjectFromCompositionButton";
    this._excludeObjectFromCompositionButton.Size = new Size(171, 23);
    this._excludeObjectFromCompositionButton.TabIndex = 0;
    this._excludeObjectFromCompositionButton.Text = "Исключить объект из состава";
    this._excludeObjectFromCompositionButton.UseVisualStyleBackColor = true;
    this._excludeObjectFromCompositionButton.Click += new EventHandler(this.ExcludeObjectFromCompositionButton_Click);
    this._useExistingObjectForAllButton.AutoSize = true;
    this._useExistingObjectForAllButton.Location = new Point(3, 32 /*0x20*/);
    this._useExistingObjectForAllButton.Name = "_useExistingObjectForAllButton";
    this._useExistingObjectForAllButton.Size = new Size(262, 23);
    this._useExistingObjectForAllButton.TabIndex = 0;
    this._useExistingObjectForAllButton.Text = "Использовать существующий объект (для всех)";
    this._useExistingObjectForAllButton.UseVisualStyleBackColor = true;
    this._useExistingObjectForAllButton.Click += new EventHandler(this.UseExistingObjectForAllButton_Click);
    this._excludeObjectFromCompositionForAllButton.AutoSize = true;
    this._excludeObjectFromCompositionForAllButton.Location = new Point(3, 61);
    this._excludeObjectFromCompositionForAllButton.Name = "_excludeObjectFromCompositionForAllButton";
    this._excludeObjectFromCompositionForAllButton.Size = new Size(224 /*0xE0*/, 23);
    this._excludeObjectFromCompositionForAllButton.TabIndex = 0;
    this._excludeObjectFromCompositionForAllButton.Text = "Исключить объект из состава (для всех)";
    this._excludeObjectFromCompositionForAllButton.UseVisualStyleBackColor = true;
    this._excludeObjectFromCompositionForAllButton.Click += new EventHandler(this.ExcludeObjectFromCompositionForAllButton_Click);
    this._abortButton.AutoSize = true;
    this._abortButton.Location = new Point(233, 61);
    this._abortButton.Name = "_abortButton";
    this._abortButton.Size = new Size(75, 23);
    this._abortButton.TabIndex = 1;
    this._abortButton.Text = "Прервать";
    this._abortButton.UseVisualStyleBackColor = true;
    this._abortButton.Click += new EventHandler(this.AbortButton_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(484, 211);
    this.ControlBox = false;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.MaximizeBox = false;
    this.Name = nameof (ObjectCreationErrorForm);
    this.ShowIcon = false;
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Ошибка создания объекта по прототипу";
    this.Load += new EventHandler(this.ObjectCreationErrorForm_Load);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.flowLayoutPanel1.ResumeLayout(false);
    this.flowLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }

  [Flags]
  public enum ObjectCreationErrorAction
  {
    None = 0,
    UseExistingObject = 1,
    ExcludeFromComposition = 16, // 0x00000010
    UseExistingObjectForAll = 256, // 0x00000100
    ExcludeFromCompositionForAll = 4096, // 0x00001000
    Abort = 65536, // 0x00010000
  }
}
