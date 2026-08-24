// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.LiteraSettingsControl
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.Controls;
using Intermech.Interfaces;
using Intermech.Workflow.Design;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class LiteraSettingsControl : StepControl
{
  public StringList LiteraValues = new StringList();
  private IContainer components;
  private GroupBox groupBox1;
  private Label label2;
  private Label label1;
  private AutoSizeLabel HintLabel;
  private TextBox LiteraBox;

  public LiteraSettingsControl() => this.InitializeComponent();

  public override bool isMetadataSettingsStep => false;

  public static LiteraSettingsControl InitControl()
  {
    StringList stringList = new StringList();
    int num = 0;
    IDBAttributeType attributeType = BasePumpHelper.Session.GetAttributeType(PumpHelper.AttrTypeLiteraID, false);
    if (attributeType != null && attributeType.MultipleValued != MultiValueModes.SingleValue)
    {
      foreach (DataRow row in (InternalDataCollectionBase) attributeType.GetPossibleValues().Rows)
        stringList.Add(row[1].ToString());
      num = stringList.Count;
      using (IDataReader dataReader = BasePumpHelper.S4Query("select distinct(litera) from articles"))
      {
        while (dataReader.Read())
        {
          if (!dataReader.IsDBNull(0))
          {
            string str = PumpHelper.LiteraToString((object) dataReader.GetString(0));
            if (str != "" && stringList.IndexOf(str) == -1)
              stringList.Add(str);
          }
        }
      }
    }
    if (stringList.Count == num)
      return (LiteraSettingsControl) null;
    LiteraSettingsControl literaSettingsControl = new LiteraSettingsControl();
    literaSettingsControl.LiteraValues.Text = stringList.Text;
    string str1 = "";
    for (int index = num; index < stringList.Count; ++index)
    {
      if (str1 != "")
        str1 += "\r\n";
      str1 += stringList[index];
    }
    literaSettingsControl.LiteraBox.Text = str1;
    return literaSettingsControl;
  }

  public override SaveSettingsResult SaveSettings()
  {
    IDBAttributeType attributeType = BasePumpHelper.Session.GetAttributeType(PumpHelper.AttrTypeLiteraID, false);
    DataTable possibleValues = attributeType.GetPossibleValues();
    possibleValues.Rows.Clear();
    for (int index = 0; index < this.LiteraValues.Count; ++index)
      possibleValues.Rows.Add((object) index, (object) this.LiteraValues[index], (object) "");
    try
    {
      attributeType.SetPossibleValues(possibleValues);
    }
    catch (Exception ex)
    {
      return MessageBox.Show($"Не удалось установить допустимые значения для атрибута {attributeType.Name} по причине:\n{ex.Message}\nПропустить?", "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation) != DialogResult.Yes ? SaveSettingsResult.ssrRetry : SaveSettingsResult.ssrOk;
    }
    return base.SaveSettings();
  }

  protected override string getCaption() => "Литера";

  public override void RefreshControl()
  {
    base.RefreshControl();
    this.HintLabel.StatusImage = (Image) SearchDataPlugin.WarningImage;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LiteraSettingsControl));
    this.groupBox1 = new GroupBox();
    this.HintLabel = new AutoSizeLabel();
    this.LiteraBox = new TextBox();
    this.label2 = new Label();
    this.label1 = new Label();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.groupBox1.Controls.Add((Control) this.HintLabel);
    this.groupBox1.Controls.Add((Control) this.LiteraBox);
    this.groupBox1.Controls.Add((Control) this.label2);
    this.groupBox1.Controls.Add((Control) this.label1);
    this.groupBox1.Dock = DockStyle.Top;
    this.groupBox1.Location = new Point(10, 10);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Padding = new Padding(10);
    this.groupBox1.Size = new Size(632, 223);
    this.groupBox1.TabIndex = 0;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Параметр \"Литера\"";
    this.HintLabel.Dock = DockStyle.Top;
    this.HintLabel.Location = new Point(10, 162);
    this.HintLabel.Name = "HintLabel";
    this.HintLabel.Padding = new Padding(0, 10, 0, 0);
    this.HintLabel.Size = new Size(612, 49);
    this.HintLabel.TabIndex = 3;
    this.HintLabel.Text = componentResourceManager.GetString("HintLabel.Text");
    this.LiteraBox.BackColor = SystemColors.Window;
    this.LiteraBox.Dock = DockStyle.Top;
    this.LiteraBox.Location = new Point(10, 69);
    this.LiteraBox.Multiline = true;
    this.LiteraBox.Name = "LiteraBox";
    this.LiteraBox.ReadOnly = true;
    this.LiteraBox.ScrollBars = ScrollBars.Vertical;
    this.LiteraBox.Size = new Size(612, 93);
    this.LiteraBox.TabIndex = 1;
    this.label2.AutoSize = true;
    this.label2.Dock = DockStyle.Top;
    this.label2.Location = new Point(10, 36);
    this.label2.Name = "label2";
    this.label2.Padding = new Padding(0, 10, 0, 10);
    this.label2.Size = new Size(427, 33);
    this.label2.TabIndex = 1;
    this.label2.Text = "Нестандартные значения параметра \"Литера\", отсутствующие в базе-приёмнике:";
    this.label1.AutoSize = true;
    this.label1.Dock = DockStyle.Top;
    this.label1.ForeColor = Color.Red;
    this.label1.Location = new Point(10, 23);
    this.label1.Name = "label1";
    this.label1.Size = new Size(592, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "В базе-источнике обнаружены объекты Search со значениями параметра \"Литера\", не соответствующими ГОСТу.";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.groupBox1);
    this.Name = nameof (LiteraSettingsControl);
    this.Padding = new Padding(10);
    this.Size = new Size(652, 343);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.ResumeLayout(false);
  }
}
