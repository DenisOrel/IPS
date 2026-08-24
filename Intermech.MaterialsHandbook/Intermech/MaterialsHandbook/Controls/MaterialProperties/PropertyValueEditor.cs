// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.PropertyValueEditor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Infralution.Controls;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

public class PropertyValueEditor : UserControl, ITypeDescriptorContext, IServiceProvider
{
  private TypeConverter _converter;
  private Type _valueType;
  private UITypeEditor _editor;
  private object _value;
  private IContainer components;
  private TextBox _TextBox;
  private Button _EditButton;

  public PropertyValueEditor() => this.InitializeComponent();

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  [Browsable(false)]
  public TypeConverter Converter
  {
    get => this._converter;
    set
    {
      if (this._converter == value)
        return;
      this.UseDefaultConverter = false;
      this._converter = value;
      if (this.UseDefaultEditor)
        this.SetDefaultEditor();
      this.UpdateStandardValues();
      this.PerformLayout();
    }
  }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  [Browsable(false)]
  public UITypeEditor Editor
  {
    get => this._editor;
    set
    {
      this.UseDefaultEditor = false;
      this._editor = value;
      this.PerformLayout();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public override string Text
  {
    get => base.Text;
    set => base.Text = value;
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool UseDefaultConverter { get; set; } = true;

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public bool UseDefaultEditor { get; set; } = true;

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  [Browsable(false)]
  public virtual string ValueText { get; set; }

  [Category("Data")]
  [DefaultValue(null)]
  [System.ComponentModel.Editor("Infralution.Controls.Design.ObjectTypeEditor, Infralution.Controls.Design, Version=3.1.4.0, Culture=neutral, PublicKeyToken=3e7e8e3744a5c13f", typeof (UITypeEditor))]
  [TypeConverter("Infralution.Controls.Design.ObjectTypeConverter, Infralution.Controls.Design, Version=3.1.4.0, Culture=neutral, PublicKeyToken=3e7e8e3744a5c13f")]
  [Description("The type of the value to be edited")]
  public virtual Type ValueType
  {
    get => this._valueType;
    set
    {
      this._valueType = value;
      if (this.UseDefaultConverter)
        this.SetDefaultConverter();
      if (!this.UseDefaultEditor)
        return;
      this.SetDefaultEditor();
    }
  }

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  [Browsable(false)]
  public virtual object Value
  {
    get => this._value;
    set
    {
      object obj = this._value;
      if ((obj != null ? (obj.Equals(value) ? 1 : 0) : (value == null ? 1 : 0)) == 0)
      {
        this._value = value;
        if (value != null && !Convert.IsDBNull(value))
          this.ValueType = value.GetType();
        this.OnValueChanged();
      }
      this.UpdateText();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public virtual object ValueOwner { get; set; }

  [DefaultValue(true)]
  [Category("Appearance")]
  [Description("Set/Get whether control should display the text representation of the current value")]
  public virtual bool ShowText { get; set; } = true;

  protected virtual bool EditValueSupported
  {
    get
    {
      return this._editor != null && this._editor.GetEditStyle((ITypeDescriptorContext) this) != UITypeEditorEditStyle.None;
    }
  }

  protected object[] StandardValues { get; private set; }

  protected virtual bool TextEditable
  {
    get
    {
      return this.ShowText && this.Converter != null && this.Converter.CanConvertFrom((ITypeDescriptorContext) this, typeof (string)) && !this.Converter.GetStandardValuesExclusive((ITypeDescriptorContext) this);
    }
  }

  private void SetDefaultEditor()
  {
    UITypeEditor uiTypeEditor = (UITypeEditor) null;
    if (this._valueType != (Type) null)
      uiTypeEditor = (UITypeEditor) TypeDescriptor.GetEditor(this._valueType, typeof (UITypeEditor));
    if (uiTypeEditor == null && this.Converter != null && this.Converter.GetStandardValuesSupported((ITypeDescriptorContext) this))
      uiTypeEditor = (UITypeEditor) new StandardValueEditor(this.Converter);
    this.Editor = uiTypeEditor;
  }

  private void UpdateStandardValues()
  {
    if (this.Converter != null && this.Converter.GetStandardValuesSupported((ITypeDescriptorContext) this))
    {
      ICollection standardValues = this.Converter.GetStandardValues();
      if (standardValues == null)
        return;
      this.StandardValues = new object[standardValues.Count];
      standardValues.CopyTo((Array) this.StandardValues, 0);
    }
    else
      this.StandardValues = (object[]) null;
  }

  private void OnTextKeyDown(object sender, KeyEventArgs e) => this.OnKeyDown(e);

  protected override void OnForeColorChanged(EventArgs e)
  {
    base.OnForeColorChanged(e);
    this._TextBox.ForeColor = this.ForeColor;
  }

  protected override void OnBackColorChanged(EventArgs e)
  {
    base.OnBackColorChanged(e);
    this._TextBox.BackColor = this.BackColor;
  }

  protected override void OnLeave(EventArgs e)
  {
    if (this.TextEditable && this.ValueText != this._TextBox.Text)
      this.ValidateText();
    base.OnLeave(e);
  }

  protected override void OnRightToLeftChanged(EventArgs e)
  {
    base.OnRightToLeftChanged(e);
    this.PerformLayout();
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    e.Handled = true;
    switch (e.KeyCode)
    {
      case Keys.Return:
        this.ValidateText();
        break;
      case Keys.Escape:
        this.CancelTextEntry();
        break;
      case Keys.Next:
        this.EditValue();
        break;
      case Keys.Up:
        this.StepStandardValue(-1, false);
        break;
      case Keys.Down:
        if (this.StandardValues != null)
        {
          this.StepStandardValue(1, false);
          break;
        }
        this.EditValue();
        break;
      default:
        e.Handled = false;
        break;
    }
    base.OnKeyDown(e);
  }

  protected override void OnLayout(LayoutEventArgs levent)
  {
    this._EditButton.Dock = this.RightToLeft == RightToLeft.No ? DockStyle.Right : DockStyle.Left;
    this._TextBox.Visible = this.ShowText;
    this._TextBox.ReadOnly = !this.TextEditable;
    this.UpdateButtonVisibility();
    base.OnLayout(levent);
  }

  protected virtual void CancelTextEntry() => this.UpdateText();

  protected virtual bool ValidateText()
  {
    try
    {
      this.Value = this.Converter.ConvertFromString((ITypeDescriptorContext) this, CultureInfo.CurrentCulture, this._TextBox.Text);
    }
    catch (Exception ex)
    {
    }
    return true;
  }

  protected virtual void UpdateButtonVisibility()
  {
    UITypeEditor editor = this._editor;
    this._EditButton.Visible = (editor != null ? (int) editor.GetEditStyle((ITypeDescriptorContext) this) : 1) == 2;
  }

  protected virtual void SetDefaultConverter()
  {
    this.Converter = this._valueType != (Type) null ? TypeDescriptor.GetConverter(this._valueType) : (TypeConverter) null;
  }

  protected virtual string GetTextForValue(object value)
  {
    try
    {
      if (this.Converter != null)
      {
        if (this.Converter.CanConvertTo((ITypeDescriptorContext) this, typeof (string)))
          return this.Converter.ConvertToString((ITypeDescriptorContext) this, value);
      }
    }
    catch
    {
    }
    return value != null ? value.ToString() : string.Empty;
  }

  protected virtual void UpdateText()
  {
    this.ValueText = this.GetTextForValue(this.Value);
    this._TextBox.Text = this.ValueText;
  }

  protected virtual void OnValueChanged()
  {
    EventHandler valueChanged = this.ValueChanged;
    if (valueChanged == null)
      return;
    valueChanged((object) this, new EventArgs());
  }

  protected virtual void OnEditButtonClick(object sender, EventArgs e) => this.EditValue();

  protected virtual void ClearValue() => this.Value = (object) string.Empty;

  public virtual void EditValue()
  {
    if (this._editor == null)
      return;
    this.Value = this._editor.EditValue((ITypeDescriptorContext) this, (IServiceProvider) this, this._value);
  }

  protected virtual void StepStandardValue(int step, bool circular)
  {
    object[] standardValues = this.StandardValues;
    if (standardValues == null)
      return;
    int num = Array.IndexOf<object>(standardValues, this.Value);
    int index;
    if (num < 0)
    {
      index = 0;
    }
    else
    {
      index = num + step;
      if (index < 0)
        index = circular ? standardValues.Length - 1 : 0;
      else if (index >= standardValues.Length)
        index = circular ? 0 : standardValues.Length - 1;
    }
    this.Value = standardValues[index];
    this._TextBox.SelectAll();
  }

  [Category("Property Changed")]
  [Description("Event fired when the Value property is changed")]
  public event EventHandler ValueChanged;

  object IServiceProvider.GetService(Type serviceType)
  {
    return serviceType == typeof (IWindowsFormsEditorService) ? (object) this : (object) null;
  }

  bool ITypeDescriptorContext.OnComponentChanging() => true;

  void ITypeDescriptorContext.OnComponentChanged()
  {
  }

  object ITypeDescriptorContext.Instance => this.ValueOwner;

  PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => (PropertyDescriptor) null;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this._TextBox = new TextBox();
    this._EditButton = new Button();
    this.SuspendLayout();
    this._TextBox.BackColor = SystemColors.Window;
    this._TextBox.BorderStyle = BorderStyle.None;
    this._TextBox.Dock = DockStyle.Fill;
    this._TextBox.Location = new Point(0, 0);
    this._TextBox.Margin = new Padding(0);
    this._TextBox.Multiline = true;
    this._TextBox.Name = "_TextBox";
    this._TextBox.Size = new Size(178, 16 /*0x10*/);
    this._TextBox.TabIndex = 0;
    this._TextBox.KeyDown += new KeyEventHandler(this.OnTextKeyDown);
    this._EditButton.Dock = DockStyle.Right;
    this._EditButton.Font = new Font("Microsoft Sans Serif", 6f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this._EditButton.Location = new Point(178, 0);
    this._EditButton.Margin = new Padding(0);
    this._EditButton.Name = "_EditButton";
    this._EditButton.Size = new Size(20, 16 /*0x10*/);
    this._EditButton.TabIndex = 1;
    this._EditButton.TabStop = false;
    this._EditButton.Text = "...";
    this._EditButton.UseVisualStyleBackColor = true;
    this._EditButton.Click += new EventHandler(this.OnEditButtonClick);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackColor = SystemColors.Control;
    this.Controls.Add((Control) this._TextBox);
    this.Controls.Add((Control) this._EditButton);
    this.Margin = new Padding(0);
    this.Name = nameof (PropertyValueEditor);
    this.Size = new Size(198, 16 /*0x10*/);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  [SpecialName]
  IContainer ITypeDescriptorContext.get_Container() => this.Container;
}
