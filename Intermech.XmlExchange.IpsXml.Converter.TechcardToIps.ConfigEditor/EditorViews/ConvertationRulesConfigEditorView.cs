// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews.ConvertationRulesConfigEditorView
// Assembly: Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 0BD7AB18-9725-4F3A-95EA-AF9537E2626A
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.dll

using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.ConfigTypes;
using Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.Config.Configuration.Format;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.TechcardToIps.ConfigEditor.EditorViews;

public class ConvertationRulesConfigEditorView : UserControl
{
  private IContainer components;
  private CheckBox cbxRelation;
  private CheckBox cbxObjectParams;
  private CheckBox cbxObject;

  public ConvertationRulesConfigEditorView() => this.InitializeComponent();

  public ConvertationRulesConfig TargetConfig { get; set; }

  public BaseConvertableEntityWithParamsConfig TargetParentConfig { get; set; }

  public IServiceProvider GlobalServices { get; set; }

  public void PerformData() => this.ApplyConfigToControl(this.TargetConfig);

  public bool ApplyChanges()
  {
    this.TargetConfig.Rules = ConvertationRules.None;
    if (this.cbxObject.Checked)
      this.TargetConfig.Rules |= ConvertationRules.Object;
    if (this.cbxObjectParams.Checked)
      this.TargetConfig.Rules |= ConvertationRules.ObjectParams;
    if (this.cbxRelation.Checked)
      this.TargetConfig.Rules |= ConvertationRules.Relation;
    return true;
  }

  private void ApplyConfigToControl(ConvertationRulesConfig config)
  {
    this.SuspendLayout();
    this.cbxRelation.Visible = this.TargetParentConfig is ObjectConfig;
    this.cbxObject.Checked = (this.TargetConfig.Rules & ConvertationRules.Object) == ConvertationRules.Object;
    this.cbxObjectParams.Checked = (this.TargetConfig.Rules & ConvertationRules.ObjectParams) == ConvertationRules.ObjectParams;
    this.cbxRelation.Checked = (this.TargetConfig.Rules & ConvertationRules.Relation) == ConvertationRules.Relation;
    this.ResumeLayout();
  }

  public event EventHandler<bool> OnDataChanged;

  private void cbx_CheckedChanged(object sender, EventArgs e)
  {
    bool e1 = this.RuleChanged(this.cbxObject, ConvertationRules.Object) || this.RuleChanged(this.cbxObjectParams, ConvertationRules.ObjectParams) || this.RuleChanged(this.cbxRelation, ConvertationRules.Relation);
    EventHandler<bool> onDataChanged = this.OnDataChanged;
    if (onDataChanged == null)
      return;
    onDataChanged(sender, e1);
  }

  private bool RuleChanged(CheckBox checkBox, ConvertationRules ruleToCheck)
  {
    if (checkBox.Checked && (this.TargetConfig.Rules & ruleToCheck) != ruleToCheck)
      return true;
    return !checkBox.Checked && (this.TargetConfig.Rules & ruleToCheck) == ruleToCheck;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.cbxRelation = new CheckBox();
    this.cbxObjectParams = new CheckBox();
    this.cbxObject = new CheckBox();
    this.SuspendLayout();
    this.cbxRelation.AutoSize = true;
    this.cbxRelation.Dock = DockStyle.Top;
    this.cbxRelation.Location = new Point(0, 34);
    this.cbxRelation.Name = "cbxRelation";
    this.cbxRelation.Size = new Size(150, 17);
    this.cbxRelation.TabIndex = 5;
    this.cbxRelation.Text = "Связи сущности";
    this.cbxRelation.UseVisualStyleBackColor = true;
    this.cbxRelation.CheckedChanged += new EventHandler(this.cbx_CheckedChanged);
    this.cbxObjectParams.AutoSize = true;
    this.cbxObjectParams.Dock = DockStyle.Top;
    this.cbxObjectParams.Location = new Point(0, 17);
    this.cbxObjectParams.Name = "cbxObjectParams";
    this.cbxObjectParams.Size = new Size(150, 17);
    this.cbxObjectParams.TabIndex = 4;
    this.cbxObjectParams.Text = "Параметры сущности";
    this.cbxObjectParams.UseVisualStyleBackColor = true;
    this.cbxObjectParams.CheckedChanged += new EventHandler(this.cbx_CheckedChanged);
    this.cbxObject.AutoSize = true;
    this.cbxObject.Dock = DockStyle.Top;
    this.cbxObject.Location = new Point(0, 0);
    this.cbxObject.Name = "cbxObject";
    this.cbxObject.Size = new Size(150, 17);
    this.cbxObject.TabIndex = 3;
    this.cbxObject.Text = "Сущность";
    this.cbxObject.UseVisualStyleBackColor = true;
    this.cbxObject.CheckedChanged += new EventHandler(this.cbx_CheckedChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.cbxRelation);
    this.Controls.Add((Control) this.cbxObjectParams);
    this.Controls.Add((Control) this.cbxObject);
    this.Name = nameof (ConvertationRulesConfigEditorView);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
