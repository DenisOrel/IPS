// Decompiled with JetBrains decompiler
// Type: Intermech.Interfaces.Pdm.SubstitutesSettingsPropertiesPage
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Interfaces.Pdm;

public class SubstitutesSettingsPropertiesPage : 
  UserControl,
  IPropertyPage,
  IPropertyPageSearchOptionEvents
{
  private ISubstitutesSettings _substs;
  private bool _inEvent;
  private IServiceProvider _provider;
  private IContainer components;
  private CheckBox cbQuantity;
  private Label lbSubstituteText;
  private ComboBox edSubstituteText;
  private GroupBox gbActual;
  private TextBox edActualText3;
  private Label lbActualText3;
  private TextBox edActualText2;
  private Label lbActualText2;
  private TextBox edActualText;
  private Label lbActualText;
  private GroupBox gbSubstitute;
  private TextBox edSubstituteText4;
  private Label lbSubstituteText4;
  private TextBox edSubstituteText3;
  private Label SubstituteText3;
  private TextBox edSubstituteText2;
  private Label SubstituteText2;
  private CheckBox cbBrackets;
  private CheckBox cbNbspInQuantity;
  private CheckBox _includePositionalDesignationInNoteCheckBox;
  private GroupBox gbMaterial;
  private TextBox edMaterialText3;
  private Label lbMaterialText3;
  private TextBox edMaterialText2;
  private Label lbMaterialText2;
  private TextBox edMaterialText;
  private Label lbMaterialText;

  public SubstitutesSettingsPropertiesPage(IServiceProvider provider)
  {
    this.InitializeComponent();
    this._substs = ServicesManager.GetService(typeof (ISubstitutesSettings)) as ISubstitutesSettings;
    this._provider = provider;
    this.FillEditors();
    this.UpdateControls();
    if (!(this._provider.GetService(typeof (IPropertyPagesService)) is IPropertyPagesService service))
      return;
    service.AddPage(LocalizationHolder.rm.GetString("Pdm_242"), (IPropertyPage) this);
  }

  protected virtual void UpdateControls()
  {
  }

  protected virtual void FillEditors()
  {
    bool inEvent = this._inEvent;
    try
    {
      this._inEvent = true;
      this.cbQuantity.Checked = this._substs.QuantityInSubstitutes;
      this.cbBrackets.Checked = this._substs.QuantityInBrackets;
      this.edActualText.Text = this._substs.ActualSubstitute;
      this.edActualText2.Text = this._substs.ActualSubstitute2;
      this.edActualText3.Text = this._substs.ActualSubstitute3;
      this.edSubstituteText.Text = this._substs.PositionsSeparator;
      this.edSubstituteText2.Text = this._substs.Substitute;
      this.edSubstituteText3.Text = this._substs.Substitute2;
      this.edSubstituteText4.Text = this._substs.Substitute3;
      this.edMaterialText.Text = this._substs.MaterialSubstitute;
      this.edMaterialText2.Text = this._substs.MaterialSubstitute2;
      this.edMaterialText3.Text = this._substs.MaterialSubstitute3;
      this.cbNbspInQuantity.Checked = this._substs.NonbreakingSpace;
      this._includePositionalDesignationInNoteCheckBox.Checked = this._substs.IncludePositionalDesignationInNote;
    }
    finally
    {
      this._inEvent = inEvent;
    }
  }

  protected virtual void LoadFromEditors()
  {
    this._substs.QuantityInSubstitutes = this.cbQuantity.Checked;
    this._substs.QuantityInBrackets = this.cbBrackets.Checked;
    this._substs.ActualSubstitute = this.edActualText.Text;
    this._substs.ActualSubstitute2 = this.edActualText2.Text;
    this._substs.ActualSubstitute3 = this.edActualText3.Text;
    this._substs.PositionsSeparator = this.edSubstituteText.Text;
    this._substs.MaterialSubstitute = this.edMaterialText.Text;
    this._substs.MaterialSubstitute2 = this.edMaterialText2.Text;
    this._substs.MaterialSubstitute3 = this.edMaterialText3.Text;
    this._substs.Substitute = this.edSubstituteText2.Text;
    this._substs.Substitute2 = this.edSubstituteText3.Text;
    this._substs.Substitute3 = this.edSubstituteText4.Text;
    this._substs.NonbreakingSpace = this.cbNbspInQuantity.Checked;
    this._substs.IncludePositionalDesignationInNote = this._includePositionalDesignationInNoteCheckBox.Checked;
  }

  private void OnChanged()
  {
    if (this.Changed == null)
      return;
    this.Changed((object) this, new EventArgs());
  }

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Control;

  public object Control => (object) this;

  public string PageName => LocalizationHolder.rm.GetString("Pdm_243");

  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  public void Apply()
  {
    this.LoadFromEditors();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._substs.SaveSettings(sessionKeeper.Session);
    this.FillEditors();
  }

  public void Cancel() => this.FillEditors();

  public string HelpTopicID => "1081";

  public List<string> GetOptionNames()
  {
    return !(this.Control is System.Windows.Forms.Control control) ? new List<string>() : IPropertyPageHelper.GetOptionNames(control);
  }

  private void DoTextChanged(object sender, EventArgs e)
  {
    if (this._inEvent)
      return;
    this.OnChanged();
  }

  private void cbQuantity_CheckedChanged(object sender, EventArgs e)
  {
    if (this._inEvent)
      return;
    this.OnChanged();
  }

  private void IncludePositionalDesignationInNoteCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    if (this._inEvent)
      return;
    this.OnChanged();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SubstitutesSettingsPropertiesPage));
    this.cbQuantity = new CheckBox();
    this.lbSubstituteText = new Label();
    this.edSubstituteText = new ComboBox();
    this.gbActual = new GroupBox();
    this.edActualText3 = new TextBox();
    this.lbActualText3 = new Label();
    this.edActualText2 = new TextBox();
    this.lbActualText2 = new Label();
    this.edActualText = new TextBox();
    this.lbActualText = new Label();
    this.gbSubstitute = new GroupBox();
    this.edSubstituteText4 = new TextBox();
    this.lbSubstituteText4 = new Label();
    this.edSubstituteText3 = new TextBox();
    this.SubstituteText3 = new Label();
    this.edSubstituteText2 = new TextBox();
    this.SubstituteText2 = new Label();
    this.cbBrackets = new CheckBox();
    this.cbNbspInQuantity = new CheckBox();
    this._includePositionalDesignationInNoteCheckBox = new CheckBox();
    this.gbMaterial = new GroupBox();
    this.edMaterialText3 = new TextBox();
    this.lbMaterialText3 = new Label();
    this.edMaterialText2 = new TextBox();
    this.lbMaterialText2 = new Label();
    this.edMaterialText = new TextBox();
    this.lbMaterialText = new Label();
    this.gbActual.SuspendLayout();
    this.gbSubstitute.SuspendLayout();
    this.gbMaterial.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.cbQuantity, "cbQuantity");
    this.cbQuantity.Name = "cbQuantity";
    this.cbQuantity.UseVisualStyleBackColor = true;
    this.cbQuantity.CheckedChanged += new EventHandler(this.cbQuantity_CheckedChanged);
    this.lbSubstituteText.FlatStyle = FlatStyle.System;
    componentResourceManager.ApplyResources((object) this.lbSubstituteText, "lbSubstituteText");
    this.lbSubstituteText.Name = "lbSubstituteText";
    componentResourceManager.ApplyResources((object) this.edSubstituteText, "edSubstituteText");
    this.edSubstituteText.FormattingEnabled = true;
    this.edSubstituteText.Items.AddRange(new object[3]
    {
      (object) componentResourceManager.GetString("edSubstituteText.Items"),
      (object) componentResourceManager.GetString("edSubstituteText.Items1"),
      (object) componentResourceManager.GetString("edSubstituteText.Items2")
    });
    this.edSubstituteText.Name = "edSubstituteText";
    this.edSubstituteText.TextChanged += new EventHandler(this.DoTextChanged);
    componentResourceManager.ApplyResources((object) this.gbActual, "gbActual");
    this.gbActual.Controls.Add((System.Windows.Forms.Control) this.edActualText3);
    this.gbActual.Controls.Add((System.Windows.Forms.Control) this.lbActualText3);
    this.gbActual.Controls.Add((System.Windows.Forms.Control) this.edActualText2);
    this.gbActual.Controls.Add((System.Windows.Forms.Control) this.lbActualText2);
    this.gbActual.Controls.Add((System.Windows.Forms.Control) this.edActualText);
    this.gbActual.Controls.Add((System.Windows.Forms.Control) this.lbActualText);
    this.gbActual.Name = "gbActual";
    this.gbActual.TabStop = false;
    componentResourceManager.ApplyResources((object) this.edActualText3, "edActualText3");
    this.edActualText3.Name = "edActualText3";
    this.edActualText3.TextChanged += new EventHandler(this.DoTextChanged);
    this.lbActualText3.FlatStyle = FlatStyle.System;
    componentResourceManager.ApplyResources((object) this.lbActualText3, "lbActualText3");
    this.lbActualText3.Name = "lbActualText3";
    componentResourceManager.ApplyResources((object) this.edActualText2, "edActualText2");
    this.edActualText2.Name = "edActualText2";
    this.edActualText2.TextChanged += new EventHandler(this.DoTextChanged);
    this.lbActualText2.FlatStyle = FlatStyle.System;
    componentResourceManager.ApplyResources((object) this.lbActualText2, "lbActualText2");
    this.lbActualText2.Name = "lbActualText2";
    componentResourceManager.ApplyResources((object) this.edActualText, "edActualText");
    this.edActualText.Name = "edActualText";
    this.edActualText.TextChanged += new EventHandler(this.DoTextChanged);
    this.lbActualText.FlatStyle = FlatStyle.System;
    componentResourceManager.ApplyResources((object) this.lbActualText, "lbActualText");
    this.lbActualText.Name = "lbActualText";
    componentResourceManager.ApplyResources((object) this.gbSubstitute, "gbSubstitute");
    this.gbSubstitute.Controls.Add((System.Windows.Forms.Control) this.edSubstituteText4);
    this.gbSubstitute.Controls.Add((System.Windows.Forms.Control) this.lbSubstituteText4);
    this.gbSubstitute.Controls.Add((System.Windows.Forms.Control) this.edSubstituteText3);
    this.gbSubstitute.Controls.Add((System.Windows.Forms.Control) this.SubstituteText3);
    this.gbSubstitute.Controls.Add((System.Windows.Forms.Control) this.edSubstituteText2);
    this.gbSubstitute.Controls.Add((System.Windows.Forms.Control) this.SubstituteText2);
    this.gbSubstitute.Name = "gbSubstitute";
    this.gbSubstitute.TabStop = false;
    componentResourceManager.ApplyResources((object) this.edSubstituteText4, "edSubstituteText4");
    this.edSubstituteText4.Name = "edSubstituteText4";
    this.edSubstituteText4.TextChanged += new EventHandler(this.DoTextChanged);
    this.lbSubstituteText4.FlatStyle = FlatStyle.System;
    componentResourceManager.ApplyResources((object) this.lbSubstituteText4, "lbSubstituteText4");
    this.lbSubstituteText4.Name = "lbSubstituteText4";
    componentResourceManager.ApplyResources((object) this.edSubstituteText3, "edSubstituteText3");
    this.edSubstituteText3.Name = "edSubstituteText3";
    this.edSubstituteText3.TextChanged += new EventHandler(this.DoTextChanged);
    this.SubstituteText3.FlatStyle = FlatStyle.System;
    componentResourceManager.ApplyResources((object) this.SubstituteText3, "SubstituteText3");
    this.SubstituteText3.Name = "SubstituteText3";
    componentResourceManager.ApplyResources((object) this.edSubstituteText2, "edSubstituteText2");
    this.edSubstituteText2.Name = "edSubstituteText2";
    this.edSubstituteText2.TextChanged += new EventHandler(this.DoTextChanged);
    this.SubstituteText2.FlatStyle = FlatStyle.System;
    componentResourceManager.ApplyResources((object) this.SubstituteText2, "SubstituteText2");
    this.SubstituteText2.Name = "SubstituteText2";
    componentResourceManager.ApplyResources((object) this.cbBrackets, "cbBrackets");
    this.cbBrackets.Name = "cbBrackets";
    this.cbBrackets.UseVisualStyleBackColor = true;
    this.cbBrackets.CheckedChanged += new EventHandler(this.cbQuantity_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.cbNbspInQuantity, "cbNbspInQuantity");
    this.cbNbspInQuantity.Name = "cbNbspInQuantity";
    this.cbNbspInQuantity.UseVisualStyleBackColor = true;
    this.cbNbspInQuantity.CheckedChanged += new EventHandler(this.cbQuantity_CheckedChanged);
    componentResourceManager.ApplyResources((object) this._includePositionalDesignationInNoteCheckBox, "_includePositionalDesignationInNoteCheckBox");
    this._includePositionalDesignationInNoteCheckBox.Name = "_includePositionalDesignationInNoteCheckBox";
    this._includePositionalDesignationInNoteCheckBox.UseVisualStyleBackColor = true;
    this._includePositionalDesignationInNoteCheckBox.CheckedChanged += new EventHandler(this.IncludePositionalDesignationInNoteCheckBox_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.gbMaterial, "gbMaterial");
    this.gbMaterial.Controls.Add((System.Windows.Forms.Control) this.edMaterialText3);
    this.gbMaterial.Controls.Add((System.Windows.Forms.Control) this.lbMaterialText3);
    this.gbMaterial.Controls.Add((System.Windows.Forms.Control) this.edMaterialText2);
    this.gbMaterial.Controls.Add((System.Windows.Forms.Control) this.lbMaterialText2);
    this.gbMaterial.Controls.Add((System.Windows.Forms.Control) this.edMaterialText);
    this.gbMaterial.Controls.Add((System.Windows.Forms.Control) this.lbMaterialText);
    this.gbMaterial.Name = "gbMaterial";
    this.gbMaterial.TabStop = false;
    componentResourceManager.ApplyResources((object) this.edMaterialText3, "edMaterialText3");
    this.edMaterialText3.Name = "edMaterialText3";
    this.edMaterialText3.TextChanged += new EventHandler(this.DoTextChanged);
    this.lbMaterialText3.FlatStyle = FlatStyle.System;
    componentResourceManager.ApplyResources((object) this.lbMaterialText3, "lbMaterialText3");
    this.lbMaterialText3.Name = "lbMaterialText3";
    componentResourceManager.ApplyResources((object) this.edMaterialText2, "edMaterialText2");
    this.edMaterialText2.Name = "edMaterialText2";
    this.edMaterialText2.TextChanged += new EventHandler(this.DoTextChanged);
    this.lbMaterialText2.FlatStyle = FlatStyle.System;
    componentResourceManager.ApplyResources((object) this.lbMaterialText2, "lbMaterialText2");
    this.lbMaterialText2.Name = "lbMaterialText2";
    componentResourceManager.ApplyResources((object) this.edMaterialText, "edMaterialText");
    this.edMaterialText.Name = "edMaterialText";
    this.edMaterialText.TextChanged += new EventHandler(this.DoTextChanged);
    this.lbMaterialText.FlatStyle = FlatStyle.System;
    componentResourceManager.ApplyResources((object) this.lbMaterialText, "lbMaterialText");
    this.lbMaterialText.Name = "lbMaterialText";
    this.AutoScaleMode = AutoScaleMode.Inherit;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((System.Windows.Forms.Control) this.gbMaterial);
    this.Controls.Add((System.Windows.Forms.Control) this._includePositionalDesignationInNoteCheckBox);
    this.Controls.Add((System.Windows.Forms.Control) this.cbNbspInQuantity);
    this.Controls.Add((System.Windows.Forms.Control) this.cbBrackets);
    this.Controls.Add((System.Windows.Forms.Control) this.gbSubstitute);
    this.Controls.Add((System.Windows.Forms.Control) this.gbActual);
    this.Controls.Add((System.Windows.Forms.Control) this.edSubstituteText);
    this.Controls.Add((System.Windows.Forms.Control) this.lbSubstituteText);
    this.Controls.Add((System.Windows.Forms.Control) this.cbQuantity);
    this.Name = nameof (SubstitutesSettingsPropertiesPage);
    this.Tag = (object) " ";
    this.gbActual.ResumeLayout(false);
    this.gbActual.PerformLayout();
    this.gbSubstitute.ResumeLayout(false);
    this.gbSubstitute.PerformLayout();
    this.gbMaterial.ResumeLayout(false);
    this.gbMaterial.PerformLayout();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
