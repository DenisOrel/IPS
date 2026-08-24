// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.AttributeTypeSelectorForm
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Advanced;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard;

internal class AttributeTypeSelectorForm : Form, ISelectorForm, IDisposable
{
  private const string Ftc = "Все типы атрибутов";
  private const string Fftc = "Все допустимые типы атрибутов";
  private readonly List<ListBoxItem> _items = new List<ListBoxItem>();
  private readonly FieldTypes _fieldType;
  private readonly Entity _entity;
  private ListBoxItem _standartItem;
  private IContainer components;
  private Label label1;
  private TableLayoutPanel tableLayoutPanel1;
  private ComboBox comboBox1;
  private Label label2;
  private Button button1;
  private Button button2;
  private ListBox listBox1;
  private TextBox textBox1;

  public AttributeTypeSelectorForm(string caption, Entity ent)
    : this(caption, ent.Settings.Properties.FieldType)
  {
    if (ent != null)
      this._entity = ent;
    this.LoadAttrs();
  }

  public AttributeTypeSelectorForm(string caption, FieldTypes fieldType)
  {
    this.InitializeComponent();
    this.Text = caption;
    this._fieldType = fieldType;
    this.comboBox1.Sorted = false;
    this.listBox1.Sorted = true;
    this.listBox1.SelectedIndex = -1;
    this.button1.Enabled = false;
  }

  private void LoadAttrs()
  {
    Guid guid = Guid.Empty;
    bool flag = this._entity != null && this._entity.Settings != null && this._entity.Settings.PumpTo is Guid && !((Guid) this._entity.Settings.PumpTo).Equals(Guid.Empty);
    if (flag)
      guid = (Guid) this._entity.Settings.PumpTo;
    if (TechcardConsts.Plugin == null || TechcardConsts.Plugin.Imdi == null)
      return;
    foreach (IAttributeTypeItem attributeType in (IEnumerable<IAttributeTypeItem>) TechcardConsts.Plugin.Imdi.AttributeTypes)
    {
      ListBoxItem listBoxItem = new ListBoxItem((object) attributeType.GUID, attributeType.Name, (FieldTypes) attributeType.AttrValueType);
      this._items.Add(listBoxItem);
      if (flag && guid.Equals(attributeType.GUID))
        this._standartItem = listBoxItem;
    }
  }

  public AttributeTypeSelectorForm(string caption, FieldTypes fieldType, Entity[] items)
    : this(caption, fieldType)
  {
    this._items.Clear();
    foreach (Entity entity in items)
    {
      if (entity != null)
        this._items.Add(new ListBoxItem((object) entity, entity.ToString(), entity.Settings.Properties.FieldType));
    }
  }

  public object SelectedItem
  {
    get
    {
      return !(this.listBox1.SelectedItem is ListBoxItem selectedItem) ? (object) null : selectedItem.Item;
    }
  }

  private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
  {
    bool flag1 = this.comboBox1.SelectedItem.Equals((object) "Все типы атрибутов");
    bool flag2 = this.comboBox1.SelectedItem.Equals((object) "Все допустимые типы атрибутов");
    FieldTypes fieldTypes = FieldTypes.ftUnknown;
    if (!(flag1 | flag2))
      fieldTypes = (FieldTypes) EnumTypeHelper.GetEnumValue(typeof (FieldTypes), this.comboBox1.SelectedItem.ToString());
    this.listBox1.BeginUpdate();
    try
    {
      List<FieldTypes> fieldTypesList = new List<FieldTypes>();
      if (flag2)
        fieldTypesList = EntityHelper.GetPosibleTypes(this._entity);
      this.listBox1.Items.Clear();
      foreach (ListBoxItem listBoxItem in this._items)
      {
        if ((flag1 || flag2 || listBoxItem.FieldType.Equals((object) fieldTypes)) && (!flag2 || flag1 || fieldTypesList.Contains(listBoxItem.FieldType)))
          this.listBox1.Items.Add((object) listBoxItem);
      }
    }
    finally
    {
      this.listBox1.EndUpdate();
    }
  }

  private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.textBox1.TextChanged -= new EventHandler(this.textBox1_TextChanged);
    try
    {
      this.textBox1.Text = (this.button1.Enabled = this.listBox1.SelectedItem != null) ? this.listBox1.SelectedItem.ToString() : string.Empty;
    }
    finally
    {
      this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
    }
  }

  private void textBox1_TextChanged(object sender, EventArgs e)
  {
    string text = this.textBox1.Text;
    this.listBox1.SelectedIndexChanged -= new EventHandler(this.listBox1_SelectedIndexChanged);
    try
    {
      foreach (ListBoxItem listBoxItem in this.listBox1.Items)
      {
        if (listBoxItem.ToString().StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
        {
          this.listBox1.SelectedItem = (object) listBoxItem;
          this.button1.Enabled = true;
          return;
        }
      }
      this.listBox1.SelectedItem = (object) null;
      this.button1.Enabled = false;
    }
    finally
    {
      this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
    }
  }

  private void AttributeTypeSelectorForm_Load(object sender, EventArgs e)
  {
    FormStorageEx.LoadSettings((Control) this);
    FieldTypes fieldTypes1 = FieldTypes.ftUnknown;
    FieldInfo[] fields = typeof (FieldTypes).GetFields();
    List<FieldTypes> fieldTypesList = new List<FieldTypes>();
    if (this._entity != null)
      fieldTypesList = EntityHelper.GetPosibleTypes(this._entity);
    foreach (FieldInfo fieldInfo in fields)
    {
      FieldTypes fieldTypes2 = (FieldTypes) fieldInfo.GetValue((object) fieldTypes1);
      if (fieldTypesList.Count == 0 || fieldTypesList.Contains(fieldTypes2))
      {
        string str = fieldTypes2.Equals((object) FieldTypes.ftUnknown) ? "Все типы атрибутов" : EnumTypeHelper.GetCaption((Enum) fieldTypes2);
        if (!this.comboBox1.Items.Contains((object) str))
          this.comboBox1.Items.Add((object) str);
      }
    }
    this.comboBox1.SelectedItem = (object) "Все типы атрибутов";
    if (fieldTypesList.Count > 1)
      this.comboBox1.Items.Add((object) "Все допустимые типы атрибутов");
    this.comboBox1.SelectedItem = (object) EnumTypeHelper.GetCaption((Enum) this._fieldType);
    if (this._standartItem == null)
      return;
    this.listBox1.SelectedItem = (object) this._standartItem;
  }

  private void AttributeTypeSelectorForm_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorageEx.SaveSettings((Control) this);
  }

  private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    if (this.listBox1.SelectedItem == null)
      return;
    this.DialogResult = DialogResult.OK;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AttributeTypeSelectorForm));
    this.label1 = new Label();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.comboBox1 = new ComboBox();
    this.label2 = new Label();
    this.listBox1 = new ListBox();
    this.textBox1 = new TextBox();
    this.button2 = new Button();
    this.button1 = new Button();
    this.tableLayoutPanel1.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
    this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.comboBox1, 1, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.label2, 0, 1);
    this.tableLayoutPanel1.Controls.Add((Control) this.listBox1, 0, 3);
    this.tableLayoutPanel1.Controls.Add((Control) this.textBox1, 0, 2);
    this.tableLayoutPanel1.Controls.Add((Control) this.button2, 3, 4);
    this.tableLayoutPanel1.Controls.Add((Control) this.button1, 2, 4);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.tableLayoutPanel1.SetColumnSpan((Control) this.comboBox1, 3);
    componentResourceManager.ApplyResources((object) this.comboBox1, "comboBox1");
    this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
    this.comboBox1.FormattingEnabled = true;
    this.comboBox1.Name = "comboBox1";
    this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
    this.tableLayoutPanel1.SetColumnSpan((Control) this.label2, 4);
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.Name = "label2";
    this.tableLayoutPanel1.SetColumnSpan((Control) this.listBox1, 4);
    componentResourceManager.ApplyResources((object) this.listBox1, "listBox1");
    this.listBox1.FormattingEnabled = true;
    this.listBox1.Name = "listBox1";
    this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
    this.listBox1.MouseDoubleClick += new MouseEventHandler(this.listBox1_MouseDoubleClick);
    this.tableLayoutPanel1.SetColumnSpan((Control) this.textBox1, 4);
    componentResourceManager.ApplyResources((object) this.textBox1, "textBox1");
    this.textBox1.Name = "textBox1";
    this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
    this.button2.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.button2, "button2");
    this.button2.Name = "button2";
    this.button2.UseVisualStyleBackColor = true;
    this.button1.DialogResult = DialogResult.OK;
    componentResourceManager.ApplyResources((object) this.button1, "button1");
    this.button1.Name = "button1";
    this.button1.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (AttributeTypeSelectorForm);
    this.Tag = (object) " ";
    this.FormClosed += new FormClosedEventHandler(this.AttributeTypeSelectorForm_FormClosed);
    this.Load += new EventHandler(this.AttributeTypeSelectorForm_Load);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel1.PerformLayout();
    this.ResumeLayout(false);
  }

  DialogResult ISelectorForm.ShowDialog() => this.ShowDialog();
}
