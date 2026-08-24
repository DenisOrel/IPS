// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.ControlWithEditor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

public class ControlWithEditor : UserControl
{
  private TypeConverter _typeConverter;
  private UITypeEditor _typeEditor;
  private object _editableValue;
  private IContainer components;
  private SelectablePanel _ControlSurface;
  protected PropertyValueEditor _Editor;

  public ControlWithEditor() => this.InitializeComponent();

  public event EventHandler EditorLeave;

  public event EventHandler EditorEnter;

  public event EventHandler ValueChanged;

  protected virtual void OnEditorLeave(object sender, EventArgs e)
  {
    EventHandler editorLeave = this.EditorLeave;
    if (editorLeave == null)
      return;
    editorLeave(sender, e);
  }

  protected virtual void OnEditorEnter(object sender, EventArgs e)
  {
    EventHandler editorEnter = this.EditorEnter;
    if (editorEnter == null)
      return;
    editorEnter(sender, e);
  }

  protected virtual void OnValueChanged()
  {
    EventHandler valueChanged = this.ValueChanged;
    if (valueChanged == null)
      return;
    valueChanged((object) this, EventArgs.Empty);
  }

  protected virtual Rectangle CalcEditorBounds() => Rectangle.Empty;

  protected virtual Color EditorColor() => this.BackColor;

  protected virtual void OnBeginEdit(object oldValue, bool startEdit = false)
  {
    this._editableValue = oldValue;
    this._Editor.Bounds = this.CalcEditorBounds();
    this._Editor.BackColor = this.EditorColor();
    this._Editor.Value = oldValue;
    this._Editor.Visible = true;
    this._Editor.Focus();
    if (!startEdit)
      return;
    this._Editor.EditValue();
  }

  protected virtual void OnCompleteEdit(object value)
  {
    if (this._editableValue == value)
      return;
    this.OnValueChanged();
  }

  protected override void OnBackColorChanged(EventArgs e)
  {
    base.OnBackColorChanged(e);
    this._ControlSurface.BackColor = this.BackColor;
  }

  private void _Editor_Leave(object sender, EventArgs e)
  {
    this._Editor.Visible = false;
    this.OnCompleteEdit(this._Editor.Value);
    this.OnEditorLeave(sender, e);
  }

  private void _Editor_Enter(object sender, EventArgs e) => this.OnEditorEnter(sender, e);

  private void _ControlSurface_Paint(object sender, PaintEventArgs e) => this.OnPaint(e);

  private void ControlWithEditor_BackColorChanged(object sender, EventArgs e)
  {
    this._ControlSurface.BackColor = this.BackColor;
    this._Editor.BackColor = this.BackColor;
  }

  private void _Editor_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyCode != Keys.Escape)
      return;
    this._ControlSurface.Focus();
  }

  private void _ControlSurface_MouseMove(object sender, MouseEventArgs e) => this.OnMouseMove(e);

  private void _ControlSurface_MouseClick(object sender, MouseEventArgs e) => this.OnMouseClick(e);

  private void _ControlSurface_MouseLeave(object sender, EventArgs e) => this.OnMouseLeave(e);

  private void _ControlSurface_MouseEnter(object sender, EventArgs e) => this.OnMouseEnter(e);

  private void _ControlSurface_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    this.OnMouseDoubleClick(e);
  }

  private void _ControlSurface_MouseDown(object sender, MouseEventArgs e) => this.OnMouseDown(e);

  private void _ControlSurface_MouseHover(object sender, EventArgs e) => this.OnMouseHover(e);

  private void _ControlSurface_Leave(object sender, EventArgs e) => this.OnLeave(e);

  private void _ControlSurface_Enter(object sender, EventArgs e) => this.OnEnter(e);

  private void _ControlSurface_Click(object sender, EventArgs e) => this.OnClick(e);

  private void _ControlSurface_DoubleClick(object sender, EventArgs e) => this.OnDoubleClick(e);

  private void _ControlSurface_MouseUp(object sender, MouseEventArgs e) => this.OnMouseUp(e);

  private void _ControlSurface_ControlAdded(object sender, ControlEventArgs e)
  {
    this.OnControlAdded(e);
  }

  private void _ControlSurface_ControlRemoved(object sender, ControlEventArgs e)
  {
    this.OnControlRemoved(e);
  }

  public Control.ControlCollection ChildControls => this._ControlSurface.Controls;

  public Rectangle SurfaceBounds => this._ControlSurface.Bounds;

  public TypeConverter TypeConverter
  {
    get => this._typeConverter;
    set
    {
      this._typeConverter = value;
      this._Editor.Converter = this._typeConverter;
    }
  }

  public UITypeEditor TypeEditor
  {
    get => this._typeEditor;
    set
    {
      this._typeEditor = value;
      this._Editor.Editor = this._typeEditor;
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
    this._ControlSurface = new SelectablePanel();
    this._Editor = new PropertyValueEditor();
    this.SuspendLayout();
    this._ControlSurface.Dock = DockStyle.Fill;
    this._ControlSurface.Location = new Point(0, 0);
    this._ControlSurface.Name = "_ControlSurface";
    this._ControlSurface.Size = new Size(379, 150);
    this._ControlSurface.TabIndex = 0;
    this._ControlSurface.TabStop = true;
    this._ControlSurface.Click += new EventHandler(this._ControlSurface_Click);
    this._ControlSurface.ControlAdded += new ControlEventHandler(this._ControlSurface_ControlAdded);
    this._ControlSurface.ControlRemoved += new ControlEventHandler(this._ControlSurface_ControlRemoved);
    this._ControlSurface.Paint += new PaintEventHandler(this._ControlSurface_Paint);
    this._ControlSurface.DoubleClick += new EventHandler(this._ControlSurface_DoubleClick);
    this._ControlSurface.Enter += new EventHandler(this._ControlSurface_Enter);
    this._ControlSurface.Leave += new EventHandler(this._ControlSurface_Leave);
    this._ControlSurface.MouseClick += new MouseEventHandler(this._ControlSurface_MouseClick);
    this._ControlSurface.MouseDoubleClick += new MouseEventHandler(this._ControlSurface_MouseDoubleClick);
    this._ControlSurface.MouseDown += new MouseEventHandler(this._ControlSurface_MouseDown);
    this._ControlSurface.MouseEnter += new EventHandler(this._ControlSurface_MouseEnter);
    this._ControlSurface.MouseLeave += new EventHandler(this._ControlSurface_MouseLeave);
    this._ControlSurface.MouseHover += new EventHandler(this._ControlSurface_MouseHover);
    this._ControlSurface.MouseMove += new MouseEventHandler(this._ControlSurface_MouseMove);
    this._ControlSurface.MouseUp += new MouseEventHandler(this._ControlSurface_MouseUp);
    this._Editor.BackColor = SystemColors.Control;
    this._Editor.ForeColor = SystemColors.ControlText;
    this._Editor.Location = new Point(13, 14);
    this._Editor.Margin = new Padding(0);
    this._Editor.Name = "_Editor";
    this._Editor.Size = new Size(170, 20);
    this._Editor.TabIndex = 0;
    this._Editor.Visible = false;
    this._Editor.Enter += new EventHandler(this._Editor_Enter);
    this._Editor.KeyDown += new KeyEventHandler(this._Editor_KeyDown);
    this._Editor.Leave += new EventHandler(this._Editor_Leave);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this._Editor);
    this.Controls.Add((Control) this._ControlSurface);
    this.Name = nameof (ControlWithEditor);
    this.Size = new Size(379, 150);
    this.BackColorChanged += new EventHandler(this.ControlWithEditor_BackColorChanged);
    this.ResumeLayout(false);
  }
}
