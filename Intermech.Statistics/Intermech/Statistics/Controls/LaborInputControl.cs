// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Controls.LaborInputControl
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Bars;
using Intermech.Expressions;
using Intermech.Interfaces;
using Intermech.Navigator.DBObjects;
using Intermech.Statistics.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics.Controls;

public class LaborInputControl : UserControl
{
  private List<int> _objTypes = new List<int>();
  private IContainer components;
  private GroupBox groupBox1;
  private RichTextBox rtbFormula;
  private ToolTip toolTip1;
  private Intermech.Bars.ToolBar toolBar1;
  private ButtonItem btnCorrectFormula;
  private ButtonItem btnDeleteFormula;

  public string Formula => this.rtbFormula.Text;

  private void Modify()
  {
    EventHandler onModified = this.OnModified;
    if (onModified == null)
      return;
    onModified((object) this, EventArgs.Empty);
  }

  public event EventHandler OnModified;

  public LaborInputControl() => this.InitializeComponent();

  public void Init(string formula, List<int> objTypes)
  {
    this.rtbFormula.Text = formula;
    this._objTypes = objTypes;
    this.SetDeleteButtonEnable();
  }

  public void ClearData()
  {
    this.rtbFormula.Text = string.Empty;
    this._objTypes.Clear();
    this.Modify();
  }

  private void btnCorrectFormula_Click(object sender, EventArgs e)
  {
    if (this._objTypes.Count <= 0)
    {
      int num1 = (int) MessageBox.Show("Для создания формулы выберите типы анализируемых объектов на вкладке Дополнительные настройки.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }
    else
    {
      List<int> typesCommonAttrIds = this.GetObjTypesCommonAttrIds();
      if (typesCommonAttrIds.Count == 0)
      {
        int num2 = (int) MessageBox.Show("У выбранных типов объектов отсутствуют общие атрибуты.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        List<Variable> variables = new List<Variable>();
        foreach (int attrTypeID in typesCommonAttrIds)
        {
          IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attrTypeID);
          if (attributeType != null)
          {
            Variable variable = new Variable(attributeType.Name, Helper.ConvertType(attributeType.FieldType), attributeType.FieldType);
            variables.Add(variable);
          }
        }
        string text = this.rtbFormula.Text;
        ExpressionEditor.EditExpression(ref text, (ICollection) variables, (CreateVariableEventHandler) null);
        this.rtbFormula.Text = text;
        this.SetDeleteButtonEnable();
        this.Modify();
      }
    }
  }

  private void btnDeleteFormula_Click(object sender, EventArgs e)
  {
    if (MessageBox.Show("Удалить формулу?", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
      return;
    this.rtbFormula.Text = string.Empty;
    this.SetDeleteButtonEnable();
    this.Modify();
  }

  private void SetDeleteButtonEnable()
  {
    if (string.IsNullOrWhiteSpace(this.rtbFormula.Text))
      this.btnDeleteFormula.Enabled = false;
    else
      this.btnDeleteFormula.Enabled = true;
  }

  private List<int> GetObjTypesCommonAttrIds()
  {
    List<int> typesCommonAttrIds = new List<int>();
    if (this._objTypes.Count <= 0)
      return typesCommonAttrIds;
    List<IMSAttribute4ObjectType> resultData = MetaDataHelper.GetAttribute4ObjectTypeList(this._objTypes[0]);
    if (resultData.Count <= 0)
      return typesCommonAttrIds;
    for (int index = 1; index < this._objTypes.Count; ++index)
    {
      List<IMSAttribute4ObjectType> attribute4ObjectTypeList = MetaDataHelper.GetAttribute4ObjectTypeList(this._objTypes[index]);
      if (attribute4ObjectTypeList.Count <= 0)
      {
        resultData.Clear();
        break;
      }
      GenericListHelper.GetDifference<IMSAttribute4ObjectType>((IList<IMSAttribute4ObjectType>) resultData, (IList<IMSAttribute4ObjectType>) attribute4ObjectTypeList, GenericListHelper.SearchMode.smExistInBoth, out resultData);
      if (resultData.Count == 0)
        break;
    }
    if (resultData.Count > 0)
      typesCommonAttrIds = resultData.Select<IMSAttribute4ObjectType, int>((Func<IMSAttribute4ObjectType, int>) (x => x.AttributeID)).ToList<int>().Distinct<int>().ToList<int>();
    return typesCommonAttrIds;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.groupBox1 = new GroupBox();
    this.toolBar1 = new Intermech.Bars.ToolBar();
    this.btnCorrectFormula = new ButtonItem();
    this.btnDeleteFormula = new ButtonItem();
    this.rtbFormula = new RichTextBox();
    this.toolTip1 = new ToolTip(this.components);
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.groupBox1.Controls.Add((Control) this.toolBar1);
    this.groupBox1.Controls.Add((Control) this.rtbFormula);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(538, 150);
    this.groupBox1.TabIndex = 0;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Формула для расчета статистики по трудоемкости";
    this.toolBar1.FullMenus = true;
    this.toolBar1.Guid = new Guid("a0190b1c-1506-4f72-a7ee-20a1588c7e8d");
    this.toolBar1.Hidden = false;
    this.toolBar1.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnCorrectFormula,
      (ToolbarItemBase) this.btnDeleteFormula
    });
    this.toolBar1.Location = new Point(3, 16 /*0x10*/);
    this.toolBar1.Name = "toolBar1";
    this.toolBar1.Size = new Size(532, 24);
    this.toolBar1.TabIndex = 20;
    this.toolBar1.Text = "toolBar1";
    this.btnCorrectFormula.CommandName = "btnCorrectFormula";
    this.btnCorrectFormula.Image = (Image) Resources.EditStandart;
    this.btnCorrectFormula.Text = "Редактировать формулу";
    this.btnCorrectFormula.ToolTipText = "Редактировать формулу";
    this.btnCorrectFormula.Click += new EventHandler(this.btnCorrectFormula_Click);
    this.btnDeleteFormula.CommandName = "btnDeleteFormula";
    this.btnDeleteFormula.Image = (Image) Resources.del;
    this.btnDeleteFormula.ToolTipText = "Удалить формулу";
    this.btnDeleteFormula.Click += new EventHandler(this.btnDeleteFormula_Click);
    this.rtbFormula.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.rtbFormula.Enabled = false;
    this.rtbFormula.Location = new Point(6, 49);
    this.rtbFormula.Name = "rtbFormula";
    this.rtbFormula.Size = new Size(529, 95);
    this.rtbFormula.TabIndex = 19;
    this.rtbFormula.Text = "";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.groupBox1);
    this.Name = nameof (LaborInputControl);
    this.Size = new Size(538, 150);
    this.groupBox1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
