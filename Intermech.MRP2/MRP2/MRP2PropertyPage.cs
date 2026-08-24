// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.MRP2PropertyPage
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using DevExpress.IM.XtraEditors.Controls;
using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.Controls.Grid;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

public class MRP2PropertyPage : UserControl, IPropertyPage, IPropertyPageSearchOptionEvents
{
  public static long cfg_maxPLNumber = 5000;
  public static long cfg_scriptID = 0;
  public static long cfg_calcScriptID = 0;
  public static List<Tuple<int, int, AttributableElements>> cfg_compareAttrs = new List<Tuple<int, int, AttributableElements>>();
  public static List<(int CopyAttributeID, int ArticleAttributeID)> cfg_ObjectAttributes = (List<(int, int)>) null;
  public static List<(int CopyAttributeID, int ArticleAttributeID)> cfg_SostavAttributes = (List<(int, int)>) null;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private Label label1;
  private Label label2;
  private MeasureSpinEdit maxPLNumber;
  private Button button1;
  private TextBox scriptName;
  private GroupBox groupBox1;
  private Button button3;
  private Button button2;
  private ListGrid attrGrid;
  private Button button4;
  private TextBox scriptName2;
  private Button button5;
  private Label label3;

  public MRP2PropertyPage()
  {
    this.InitializeComponent();
    this.LoadSettings();
  }

  private void LoadSettings()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      MRP2PropertyPage.cfg_maxPLNumber = sessionKeeper.Session.Configurations.ReadInteger("MRP2", "MRP2", "maxPLNumber", 5000L, DBConfigMode.GlobalOnly);
      MRP2PropertyPage.cfg_scriptID = sessionKeeper.Session.Configurations.ReadInteger("MRP2", "MRP2", "scriptID", 0L, DBConfigMode.GlobalOnly);
      MRP2PropertyPage.cfg_calcScriptID = sessionKeeper.Session.Configurations.ReadInteger("MRP2", "MRP2", "calc_scriptID", 0L, DBConfigMode.GlobalOnly);
      MRP2PropertyPage.cfg_compareAttrs.Clear();
      string str1 = sessionKeeper.Session.Configurations.ReadString("MRP2", "MRP2", "CompareAttributes", "", DBConfigMode.GlobalOnly);
      char[] chArray1 = new char[1]{ ',' };
      foreach (string str2 in str1.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ ':' };
        string[] strArray = str2.Split(chArray2);
        if (strArray.Length == 3)
          MRP2PropertyPage.cfg_compareAttrs.Add(new Tuple<int, int, AttributableElements>(Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), (AttributableElements) Enum.Parse(typeof (AttributableElements), strArray[0])));
      }
      this.maxPLNumber.Value = (Decimal) MRP2PropertyPage.cfg_maxPLNumber;
      this.scriptName.Text = "";
      this.scriptName.Tag = (object) 0;
      if (MRP2PropertyPage.cfg_scriptID == 0L)
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(new Guid("cadd9b5c-306c-11d8-b4e9-00304f19f545"), false);
        if (dbObject != null)
        {
          MRP2PropertyPage.cfg_scriptID = dbObject.ObjectID;
          sessionKeeper.Session.Configurations.WriteInteger("MRP2", "MRP2", "scriptID", MRP2PropertyPage.cfg_scriptID, 0L);
        }
      }
      if (MRP2PropertyPage.cfg_scriptID != 0L)
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(MRP2PropertyPage.cfg_scriptID, false);
        if (dbObject != null)
        {
          this.scriptName.Text = dbObject.Caption;
          this.scriptName.Tag = (object) MRP2PropertyPage.cfg_scriptID;
        }
      }
      this.scriptName2.Text = "";
      this.scriptName2.Tag = (object) 0;
      if (MRP2PropertyPage.cfg_calcScriptID != 0L)
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(MRP2PropertyPage.cfg_calcScriptID, false);
        if (dbObject != null)
        {
          this.scriptName2.Text = dbObject.Caption;
          this.scriptName2.Tag = (object) MRP2PropertyPage.cfg_calcScriptID;
        }
      }
      this.attrGrid.Items.Clear();
      foreach (Tuple<int, int, AttributableElements> cfgCompareAttr in MRP2PropertyPage.cfg_compareAttrs)
        this.attrGrid.Items.Add(new ListItem(this.attrGrid)
        {
          Text = MetaDataHelper.GetAttributeType(cfgCompareAttr.Item1).Text,
          SubItems = {
            MetaDataHelper.GetAttributeType(cfgCompareAttr.Item2).Text
          },
          Tag = (object) cfgCompareAttr
        });
    }
  }

  private void SaveSettings()
  {
    MRP2PropertyPage.cfg_maxPLNumber = (long) Convert.ToInt32(this.maxPLNumber.Value);
    MRP2PropertyPage.cfg_scriptID = (long) Convert.ToInt32(this.scriptName.Tag);
    MRP2PropertyPage.cfg_calcScriptID = (long) Convert.ToInt32(this.scriptName2.Tag);
    MRP2PropertyPage.cfg_compareAttrs.Clear();
    List<string> values = new List<string>();
    foreach (ListItem listItem in (CollectionBase) this.attrGrid.Items)
    {
      Tuple<int, int, AttributableElements> tag = listItem.Tag as Tuple<int, int, AttributableElements>;
      MRP2PropertyPage.cfg_compareAttrs.Add(tag);
      if (tag != null)
        values.Add($"{tag.Item3}:{tag.Item1}:{tag.Item2}");
    }
    string str = string.Join(",", (IEnumerable<string>) values);
    MRP2PropertyPage.cfg_ObjectAttributes = (List<(int, int)>) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.Configurations.WriteInteger("MRP2", "MRP2", "maxPLNumber", MRP2PropertyPage.cfg_maxPLNumber, 0L);
      sessionKeeper.Session.Configurations.WriteInteger("MRP2", "MRP2", "scriptID", MRP2PropertyPage.cfg_scriptID, 0L);
      sessionKeeper.Session.Configurations.WriteInteger("MRP2", "MRP2", "calc_scriptID", MRP2PropertyPage.cfg_calcScriptID, 0L);
      sessionKeeper.Session.Configurations.WriteString("MRP2", "MRP2", "CompareAttributes", str, 0L);
    }
  }

  public PropertyPageType Type => PropertyPageType.Control;

  public object Control => (object) this;

  public string PageName => "Настройки производственных ведомостей";

  public string HelpTopicID => "0";

  public string HeaderText => "";

  public event EventHandler Changed;

  public void Apply() => this.SaveSettings();

  public void Cancel() => this.LoadSettings();

  public List<string> GetOptionNames()
  {
    return !(this.Control is System.Windows.Forms.Control control) ? new List<string>() : IPropertyPageHelper.GetOptionNames(control);
  }

  private void button1_Click(object sender, EventArgs e)
  {
    IReadOnlyList<IDBObjectID> dbObjectIdList = SelectDialog.Objects((IReadOnlyCollection<int>) new int[1]
    {
      MetaDataHelper.GetObjectTypeID("cadd9962-306c-11d8-b4e9-00304f19f545")
    }, "Выберите объект", options: SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect, operationName: "SelectScript", disableGlobalContextMenuCommands: true);
    if (dbObjectIdList == null || dbObjectIdList.Count <= 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(dbObjectIdList[0].Value);
      this.scriptName.Text = dbObject.Caption;
      this.scriptName.Tag = (object) dbObject.ObjectID;
      this.OnChanged();
    }
  }

  private void OnChanged()
  {
    if (this.Changed == null)
      return;
    this.Changed((object) this, new EventArgs());
  }

  private void MaxPLNumber_TextChanged(object sender, EventArgs e) => this.OnChanged();

  private void groupBox1_Resize(object sender, EventArgs e)
  {
    this.attrGrid.Columns[0].Width = (this.attrGrid.Width - 1) / 2;
    this.attrGrid.Columns[1].Width = this.attrGrid.Columns[0].Width;
  }

  private void button2_Click(object sender, EventArgs e)
  {
    int attrTypeID1 = 0;
    int attrTypeID2 = 0;
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false))
    {
      attributesSelectDlg.AllowedAttributesSourceTypes = AllowedAttrsSourceTypesEnum.Objects;
      attributesSelectDlg.ForbiddenAttrsTypesFilter = ((IEnumerable<FieldTypes>) new FieldTypes[1]
      {
        FieldTypes.ftSystem
      }).ToList<FieldTypes>();
      attributesSelectDlg.LoadAttrDialogForObjectsTypes(new Guid("cadd9a5d-306c-11d8-b4e9-00304f19f545"));
      attributesSelectDlg.Text = "Выберите атрибут производственной копии";
      if (!attributesSelectDlg.ShowDialog().Equals((object) DialogResult.OK) || attributesSelectDlg.SelectedAttributesID.Count <= 0)
        return;
      attrTypeID1 = attributesSelectDlg.SelectedAttributesID[0];
    }
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false))
    {
      attributesSelectDlg.AllowedAttributesSourceTypes = AllowedAttrsSourceTypesEnum.Objects;
      attributesSelectDlg.ForbiddenAttrsTypesFilter = ((IEnumerable<FieldTypes>) new FieldTypes[1]
      {
        FieldTypes.ftSystem
      }).ToList<FieldTypes>();
      attributesSelectDlg.LoadAttrDialogForObjectsTypes(new Guid("cad00268-306c-11d8-b4e9-00304f19f545"));
      attributesSelectDlg.Text = "Выберите атрибут изделия";
      if (!attributesSelectDlg.ShowDialog().Equals((object) DialogResult.OK) || attributesSelectDlg.SelectedAttributesID.Count <= 0)
        return;
      attrTypeID2 = attributesSelectDlg.SelectedAttributesID[0];
    }
    foreach (ListItem listItem in (CollectionBase) this.attrGrid.Items)
    {
      Tuple<int, int, AttributableElements> tag = listItem.Tag as Tuple<int, int, AttributableElements>;
      if (tag.Item3 == AttributableElements.Object && (tag.Item1 == attrTypeID1 || tag.Item2 == attrTypeID2))
        throw new NotificationException("Нельзя добавлять один и тот же тип атрибута дважды");
    }
    ListItem listItem1 = new ListItem(this.attrGrid);
    IMSAttributeType attributeType1 = MetaDataHelper.GetAttributeType(attrTypeID1);
    listItem1.Text = attributeType1.Text;
    IMSAttributeType attributeType2 = MetaDataHelper.GetAttributeType(attrTypeID2);
    listItem1.SubItems.Add(attributeType2.Text);
    listItem1.Tag = (object) new Tuple<int, int, AttributableElements>(attrTypeID1, attrTypeID2, AttributableElements.Object);
    this.attrGrid.Items.Add(listItem1);
    this.OnChanged();
  }

  private void button4_Click(object sender, EventArgs e)
  {
    int attrTypeID1 = 0;
    int attrTypeID2 = 0;
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false))
    {
      attributesSelectDlg.LoadAttrDialogForRelationsTypes(new Guid("cadd9a57-306c-11d8-b4e9-00304f19f545"));
      attributesSelectDlg.ForbiddenAttrsTypesFilter = ((IEnumerable<FieldTypes>) new FieldTypes[1]
      {
        FieldTypes.ftSystem
      }).ToList<FieldTypes>();
      attributesSelectDlg.Text = "Выберите атрибут состава производственной копии";
      if (!attributesSelectDlg.ShowDialog().Equals((object) DialogResult.OK) || attributesSelectDlg.SelectedAttributesID.Count <= 0)
        return;
      attrTypeID1 = attributesSelectDlg.SelectedAttributesID[0];
    }
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(false))
    {
      attributesSelectDlg.LoadAttrDialogForRelationsTypes(new Guid("cad00023-306c-11d8-b4e9-00304f19f545"));
      attributesSelectDlg.ForbiddenAttrsTypesFilter = ((IEnumerable<FieldTypes>) new FieldTypes[1]
      {
        FieldTypes.ftSystem
      }).ToList<FieldTypes>();
      attributesSelectDlg.Text = "Выберите атрибут состава изделия";
      if (!attributesSelectDlg.ShowDialog().Equals((object) DialogResult.OK) || attributesSelectDlg.SelectedAttributesID.Count <= 0)
        return;
      attrTypeID2 = attributesSelectDlg.SelectedAttributesID[0];
    }
    foreach (ListItem listItem in (CollectionBase) this.attrGrid.Items)
    {
      Tuple<int, int, AttributableElements> tag = listItem.Tag as Tuple<int, int, AttributableElements>;
      if (tag.Item3 == AttributableElements.Relation && (tag.Item1 == attrTypeID1 || tag.Item2 == attrTypeID2))
        throw new NotificationException("Нельзя добавлять один и тот же тип атрибута дважды");
    }
    this.attrGrid.Items.Add(new ListItem(this.attrGrid)
    {
      Text = MetaDataHelper.GetAttributeType(attrTypeID1).Text,
      SubItems = {
        MetaDataHelper.GetAttributeType(attrTypeID2).Text
      },
      Tag = (object) new Tuple<int, int, AttributableElements>(attrTypeID1, attrTypeID2, AttributableElements.Relation)
    });
    this.OnChanged();
  }

  private void button3_Click(object sender, EventArgs e)
  {
    for (int index = this.attrGrid.Items.Count - 1; index >= 0; --index)
    {
      if (this.attrGrid.Items[index].Selected)
        this.attrGrid.Items.RemoveAt(index);
    }
    this.attrGrid.Invalidate();
    this.OnChanged();
  }

  private void button5_Click(object sender, EventArgs e)
  {
    IReadOnlyList<IDBObjectID> dbObjectIdList = SelectDialog.Objects((IReadOnlyCollection<int>) new int[1]
    {
      MetaDataHelper.GetObjectTypeID("cadd9962-306c-11d8-b4e9-00304f19f545")
    }, "Выберите объект", options: SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect, operationName: "SelectScript", disableGlobalContextMenuCommands: true);
    if (dbObjectIdList == null || dbObjectIdList.Count <= 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(dbObjectIdList[0].Value);
      this.scriptName2.Text = dbObject.Caption;
      this.scriptName2.Tag = (object) dbObject.ObjectID;
      this.OnChanged();
    }
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
    ListColumn listColumn1 = new ListColumn();
    ListColumn listColumn2 = new ListColumn();
    this.label1 = new Label();
    this.label2 = new Label();
    this.button1 = new Button();
    this.scriptName = new TextBox();
    this.groupBox1 = new GroupBox();
    this.button4 = new Button();
    this.attrGrid = new ListGrid();
    this.button3 = new Button();
    this.button2 = new Button();
    this.maxPLNumber = new MeasureSpinEdit();
    this.scriptName2 = new TextBox();
    this.button5 = new Button();
    this.label3 = new Label();
    this.groupBox1.SuspendLayout();
    this.maxPLNumber.Properties.BeginInit();
    this.SuspendLayout();
    this.label1.AutoSize = true;
    this.label1.Location = new Point(3, 10);
    this.label1.Name = "label1";
    this.label1.Size = new Size(287, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Сценарий для проверки производственной ведомости:";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(3, 63 /*0x3F*/);
    this.label2.Name = "label2";
    this.label2.Size = new Size(182, 13);
    this.label2.TabIndex = 1;
    this.label2.Text = "Максимальный номер ведомости:";
    this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.button1.Location = new Point(607, 6);
    this.button1.Name = "button1";
    this.button1.Size = new Size(27, 20);
    this.button1.TabIndex = 3;
    this.button1.Text = "...";
    this.button1.UseVisualStyleBackColor = true;
    this.button1.Click += new EventHandler(this.button1_Click);
    this.scriptName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.scriptName.Location = new Point(296, 6);
    this.scriptName.Name = "scriptName";
    this.scriptName.ReadOnly = true;
    this.scriptName.Size = new Size(305, 20);
    this.scriptName.TabIndex = 4;
    this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.button4);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.attrGrid);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.button3);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.button2);
    this.groupBox1.Location = new Point(6, 106);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(628, 274);
    this.groupBox1.TabIndex = 5;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Настройка сравнения атрибутов копий и изделий";
    this.groupBox1.Resize += new EventHandler(this.groupBox1_Resize);
    this.button4.Location = new Point(185, 21);
    this.button4.Name = "button4";
    this.button4.Size = new Size(173, 22);
    this.button4.TabIndex = 4;
    this.button4.Text = "Добавить атрибут связи";
    this.button4.UseVisualStyleBackColor = true;
    this.button4.Click += new EventHandler(this.button4_Click);
    this.attrGrid.AlternateBackground = Color.DarkGreen;
    this.attrGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.attrGrid.BackColor = SystemColors.ControlLightLight;
    listColumn1.Name = "Column1";
    listColumn1.Text = "Атрибут копии";
    listColumn1.Width = 200;
    listColumn2.Name = "Column2";
    listColumn2.Text = "Атрибут изделия";
    listColumn2.Width = 200;
    this.attrGrid.Columns.AddRange(new ListColumn[2]
    {
      listColumn1,
      listColumn2
    });
    this.attrGrid.GridColor = Color.LightGray;
    this.attrGrid.HeaderHeight = 22;
    this.attrGrid.HotTrackingColor = Color.LightGray;
    this.attrGrid.ImageList = (ImageList) null;
    this.attrGrid.ItemHeight = 17;
    this.attrGrid.Location = new Point(6, 49);
    this.attrGrid.Name = "attrGrid";
    this.attrGrid.SelectedTextColor = Color.White;
    this.attrGrid.SelectionColor = Color.DarkBlue;
    this.attrGrid.Size = new Size(616, 222);
    this.attrGrid.SuperFlatHeaderColor = Color.White;
    this.attrGrid.TabIndex = 3;
    this.attrGrid.Text = "listGrid1";
    this.button3.Location = new Point(364, 21);
    this.button3.Name = "button3";
    this.button3.Size = new Size(69, 22);
    this.button3.TabIndex = 2;
    this.button3.Text = "Удалить";
    this.button3.UseVisualStyleBackColor = true;
    this.button3.Click += new EventHandler(this.button3_Click);
    this.button2.Location = new Point(6, 21);
    this.button2.Name = "button2";
    this.button2.Size = new Size(173, 22);
    this.button2.TabIndex = 1;
    this.button2.Text = "Добавить атрибут объекта";
    this.button2.UseVisualStyleBackColor = true;
    this.button2.Click += new EventHandler(this.button2_Click);
    this.maxPLNumber.EditValue = (object) "0";
    this.maxPLNumber.LastValue = 0.0;
    this.maxPLNumber.Location = new Point(296, 58);
    this.maxPLNumber.Name = "maxPLNumber";
    this.maxPLNumber.Properties.Buttons.AddRange(new EditorButton[1]
    {
      new EditorButton()
    });
    this.maxPLNumber.Properties.UseCtrlIncrement = false;
    this.maxPLNumber.Size = new Size(90, 20);
    this.maxPLNumber.TabIndex = 2;
    this.maxPLNumber.TextChanged += new EventHandler(this.MaxPLNumber_TextChanged);
    this.scriptName2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.scriptName2.Location = new Point(296, 32 /*0x20*/);
    this.scriptName2.Name = "scriptName2";
    this.scriptName2.ReadOnly = true;
    this.scriptName2.Size = new Size(305, 20);
    this.scriptName2.TabIndex = 8;
    this.button5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.button5.Location = new Point(607, 32 /*0x20*/);
    this.button5.Name = "button5";
    this.button5.Size = new Size(27, 20);
    this.button5.TabIndex = 7;
    this.button5.Text = "...";
    this.button5.UseVisualStyleBackColor = true;
    this.button5.Click += new EventHandler(this.button5_Click);
    this.label3.AutoSize = true;
    this.label3.Location = new Point(3, 36);
    this.label3.Name = "label3";
    this.label3.Size = new Size(251, 13);
    this.label3.TabIndex = 6;
    this.label3.Text = "Сценарий для расчета количества в ведомости:";
    this.Controls.Add((System.Windows.Forms.Control) this.scriptName2);
    this.Controls.Add((System.Windows.Forms.Control) this.button5);
    this.Controls.Add((System.Windows.Forms.Control) this.label3);
    this.Controls.Add((System.Windows.Forms.Control) this.groupBox1);
    this.Controls.Add((System.Windows.Forms.Control) this.scriptName);
    this.Controls.Add((System.Windows.Forms.Control) this.button1);
    this.Controls.Add((System.Windows.Forms.Control) this.maxPLNumber);
    this.Controls.Add((System.Windows.Forms.Control) this.label2);
    this.Controls.Add((System.Windows.Forms.Control) this.label1);
    this.Name = nameof (MRP2PropertyPage);
    this.Size = new Size(641, 383);
    this.groupBox1.ResumeLayout(false);
    this.maxPLNumber.Properties.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
