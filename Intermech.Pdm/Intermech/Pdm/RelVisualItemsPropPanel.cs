// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelVisualItemsPropPanel
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Localization;
using Intermech.Map;
using Intermech.Pdm.RelationVisualizer;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm;

public class RelVisualItemsPropPanel : UserControl
{
  protected static readonly Type[] tabTypes = new Type[1]
  {
    typeof (ObjectAllAttributesGridTab)
  };
  private bool isModificated;
  private IContainer components;
  private ObjectPropertyGrid propertyGrid1;
  private TableLayoutPanel tableLayoutPanel1;
  private TableLayoutPanel tableLayoutPanel2;
  private Button buttonSettingCancel;
  private Button buttonSettingApply;

  public event EventHandler RelationAttrUpdated;

  public RelVisualItemsPropPanel() => this.InitializeComponent();

  public bool IsModificated
  {
    get => this.isModificated;
    set => this.isModificated = value;
  }

  public ObjectPropertyGrid PropertyGrid
  {
    [DebuggerStepThrough] get => this.propertyGrid1;
    set => this.propertyGrid1 = value;
  }

  public void LoadObject(MapObject node)
  {
    if (this.isModificated)
    {
      switch (MessageBox.Show(LocalizationHolder.rm.GetString("Pdm_rv_21"), LocalizationHolder.rm.GetString("Pdm_rv_22"), MessageBoxButtons.YesNo, MessageBoxIcon.Question))
      {
        case DialogResult.Yes:
          this.Apply();
          break;
        case DialogResult.No:
          this.Cancel();
          break;
        default:
          return;
      }
    }
    switch (node)
    {
      case RelMapLink _:
        RelMapLink relMapLink = node as RelMapLink;
        this.PropertyGrid.Visible = true;
        this.PropertyGrid.Load(relMapLink.RelId, AttributableElements.Relation, GetAttributeValuesModes.IncludeName | GetAttributeValuesModes.IncludeGroupName | GetAttributeValuesModes.CheckWriteAccess | GetAttributeValuesModes.IncludeDescriptions | GetAttributeValuesModes.CheckVisibility, false, RelVisualItemsPropPanel.tabTypes);
        break;
      case VisObjectNode _:
        VisObjectNode visObjectNode = node as VisObjectNode;
        this.PropertyGrid.Visible = true;
        this.PropertyGrid.Load(visObjectNode.ObjectVerId, AttributableElements.Object, GetAttributeValuesModes.IncludeName | GetAttributeValuesModes.IncludeGroupName | GetAttributeValuesModes.CheckWriteAccess | GetAttributeValuesModes.IncludeDescriptions | GetAttributeValuesModes.CheckVisibility, false, RelVisualItemsPropPanel.tabTypes);
        break;
      default:
        this.PropertyGrid.Visible = false;
        break;
    }
    this.LockButtons(true);
  }

  private void buttonSettingApply_Click(object sender, EventArgs e) => this.Apply();

  private void Apply()
  {
    if (this.propertyGrid1.Save() && this.RelationAttrUpdated != null)
      this.RelationAttrUpdated((object) null, EventArgs.Empty);
    this.isModificated = false;
    this.LockButtons(true);
  }

  private void Cancel()
  {
    this.propertyGrid1.Refresh();
    this.isModificated = false;
    this.LockButtons(true);
  }

  private void buttonSettingCancel_Click(object sender, EventArgs e) => this.Cancel();

  private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
  {
    this.isModificated = true;
    this.LockButtons(false);
  }

  private void LockButtons(bool flag)
  {
    this.buttonSettingApply.Enabled = !flag;
    this.buttonSettingCancel.Enabled = !flag;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RelVisualItemsPropPanel));
    this.propertyGrid1 = new ObjectPropertyGrid();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.tableLayoutPanel2 = new TableLayoutPanel();
    this.buttonSettingCancel = new Button();
    this.buttonSettingApply = new Button();
    this.tableLayoutPanel1.SuspendLayout();
    this.tableLayoutPanel2.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.propertyGrid1, "propertyGrid1");
    this.propertyGrid1.CommandsActiveLinkColor = SystemColors.ActiveCaption;
    this.propertyGrid1.CommandsDisabledLinkColor = SystemColors.ControlDark;
    this.propertyGrid1.CommandsLinkColor = SystemColors.ActiveCaption;
    this.propertyGrid1.InternalMenuEnabled = true;
    this.propertyGrid1.LockTypeChange = false;
    this.propertyGrid1.Name = "propertyGrid1";
    this.propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
    this.tableLayoutPanel1.Controls.Add((Control) this.propertyGrid1, 0, 0);
    this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel2, 0, 1);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel2, "tableLayoutPanel2");
    this.tableLayoutPanel2.Controls.Add((Control) this.buttonSettingCancel, 1, 0);
    this.tableLayoutPanel2.Controls.Add((Control) this.buttonSettingApply, 0, 0);
    this.tableLayoutPanel2.Name = "tableLayoutPanel2";
    componentResourceManager.ApplyResources((object) this.buttonSettingCancel, "buttonSettingCancel");
    this.buttonSettingCancel.Name = "buttonSettingCancel";
    this.buttonSettingCancel.UseVisualStyleBackColor = true;
    this.buttonSettingCancel.Click += new EventHandler(this.buttonSettingCancel_Click);
    componentResourceManager.ApplyResources((object) this.buttonSettingApply, "buttonSettingApply");
    this.buttonSettingApply.Name = "buttonSettingApply";
    this.buttonSettingApply.UseVisualStyleBackColor = true;
    this.buttonSettingApply.Click += new EventHandler(this.buttonSettingApply_Click);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this.tableLayoutPanel1);
    this.Name = nameof (RelVisualItemsPropPanel);
    this.tableLayoutPanel1.ResumeLayout(false);
    this.tableLayoutPanel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
