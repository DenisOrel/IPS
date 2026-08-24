// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.Controls.SelectSchemeForm
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Search.Controls;

public class SelectSchemeForm : Form
{
  private Dictionary<int, List<LCStep>> _steps;
  private IContainer components;
  private Panel panel1;
  private Button bCancel;
  private Button bOK;
  private Panel panel2;
  private GroupBox groupBox2;
  private ListView lwSchemes;
  private GroupBox groupBox1;
  private ColumnHeader columnHeader1;
  private ComboBox cbSteps;
  private Button bRefresh;

  public SelectSchemeForm() => this.InitializeComponent();

  public void FillControls(IUserSession session)
  {
    this.lwSchemes.Items.Clear();
    DataTable dataTable = (session.GetLCSchemaCollection() as IDBCollection).Select(string.Empty);
    if (dataTable.Rows.Count > 0)
    {
      this._steps = new Dictionary<int, List<LCStep>>(dataTable.Rows.Count);
      this.lwSchemes.BeginUpdate();
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        ListViewItem listViewItem = new ListViewItem(Convert.ToString(dataTable.Rows[index]["F_NAME"]));
        int int32 = Convert.ToInt32(dataTable.Rows[index]["F_SCHEMA_ID"]);
        listViewItem.Tag = (object) int32;
        this.lwSchemes.Items.Add(listViewItem);
        this._steps.Add(int32, (List<LCStep>) null);
        DataSet schema = session.GetLCSchema(int32).GetStepsCollection().GetSchema();
        if (schema != null && schema.Tables["IMS_LC_STEPS"] != null && schema.Tables["IMS_LC_STEPS"].Rows != null)
        {
          List<LCStep> lcStepList = new List<LCStep>(schema.Tables["IMS_LC_STEPS"].Rows.Count);
          foreach (DataRow row in (InternalDataCollectionBase) schema.Tables["IMS_LC_STEPS"].Rows)
            lcStepList.Add(new LCStep(Convert.ToString(row["F_LC_NAME"]), Convert.ToInt32(row["F_LC_STEP"])));
          this._steps[int32] = lcStepList;
        }
      }
      this.lwSchemes.EndUpdate();
    }
    if (this.lwSchemes.Items.Count <= 0)
      return;
    this.lwSchemes.Items[0].Selected = true;
  }

  private void SelectScheme(int id)
  {
    if (!this._steps.ContainsKey(id))
      return;
    this.cbSteps.Items.Clear();
    if (this._steps[id] == null)
      return;
    this.cbSteps.BeginUpdate();
    foreach (LCStep lcStep in this._steps[id])
      this.cbSteps.Items.Add((object) lcStep.Name);
    if (this.cbSteps.Items.Count > 0)
      this.cbSteps.SelectedIndex = 0;
    this.cbSteps.EndUpdate();
  }

  public int SchemeID
  {
    get
    {
      return this.lwSchemes.SelectedItems != null && this.lwSchemes.SelectedItems.Count > 0 ? (int) this.lwSchemes.SelectedItems[0].Tag : 0;
    }
  }

  public string SchemeName
  {
    get
    {
      return this.lwSchemes.SelectedItems != null && this.lwSchemes.SelectedItems.Count > 0 ? this.lwSchemes.SelectedItems[0].Text : string.Empty;
    }
  }

  public LCStep DefaultStep
  {
    get
    {
      if (this.cbSteps.Items.Count > 0)
      {
        List<LCStep> lcStepList = (List<LCStep>) null;
        if (this._steps.TryGetValue(this.SchemeID, out lcStepList) && this.cbSteps.SelectedIndex >= 0)
          return lcStepList[this.cbSteps.SelectedIndex];
      }
      return (LCStep) null;
    }
  }

  private void lwSchemes_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (this.lwSchemes.SelectedItems == null || this.lwSchemes.SelectedItems.Count <= 0)
      return;
    this.SelectScheme((int) this.lwSchemes.SelectedItems[0].Tag);
  }

  private void bRefresh_Click(object sender, EventArgs e)
  {
    this.FillControls((ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter).GetUserSession());
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectSchemeForm));
    this.panel1 = new Panel();
    this.bRefresh = new Button();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.panel2 = new Panel();
    this.groupBox2 = new GroupBox();
    this.lwSchemes = new ListView();
    this.columnHeader1 = new ColumnHeader();
    this.groupBox1 = new GroupBox();
    this.cbSteps = new ComboBox();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.panel1.AccessibleDescription = (string) null;
    this.panel1.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.BackgroundImage = (Image) null;
    this.panel1.Controls.Add((Control) this.bRefresh);
    this.panel1.Controls.Add((Control) this.bCancel);
    this.panel1.Controls.Add((Control) this.bOK);
    this.panel1.Font = (Font) null;
    this.panel1.Name = "panel1";
    this.bRefresh.AccessibleDescription = (string) null;
    this.bRefresh.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.bRefresh, "bRefresh");
    this.bRefresh.BackgroundImage = (Image) null;
    this.bRefresh.Font = (Font) null;
    this.bRefresh.Name = "bRefresh";
    this.bRefresh.UseVisualStyleBackColor = true;
    this.bRefresh.Click += new EventHandler(this.bRefresh_Click);
    this.bCancel.AccessibleDescription = (string) null;
    this.bCancel.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.BackgroundImage = (Image) null;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Font = (Font) null;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bOK.AccessibleDescription = (string) null;
    this.bOK.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.BackgroundImage = (Image) null;
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Font = (Font) null;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.panel2.AccessibleDescription = (string) null;
    this.panel2.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.BackgroundImage = (Image) null;
    this.panel2.Controls.Add((Control) this.groupBox2);
    this.panel2.Controls.Add((Control) this.groupBox1);
    this.panel2.Font = (Font) null;
    this.panel2.Name = "panel2";
    this.groupBox2.AccessibleDescription = (string) null;
    this.groupBox2.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.groupBox2, "groupBox2");
    this.groupBox2.BackgroundImage = (Image) null;
    this.groupBox2.Controls.Add((Control) this.lwSchemes);
    this.groupBox2.Font = (Font) null;
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.TabStop = false;
    this.lwSchemes.AccessibleDescription = (string) null;
    this.lwSchemes.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.lwSchemes, "lwSchemes");
    this.lwSchemes.BackgroundImage = (Image) null;
    this.lwSchemes.Columns.AddRange(new ColumnHeader[1]
    {
      this.columnHeader1
    });
    this.lwSchemes.Font = (Font) null;
    this.lwSchemes.MultiSelect = false;
    this.lwSchemes.Name = "lwSchemes";
    this.lwSchemes.UseCompatibleStateImageBehavior = false;
    this.lwSchemes.View = View.Details;
    this.lwSchemes.SelectedIndexChanged += new EventHandler(this.lwSchemes_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this.columnHeader1, "columnHeader1");
    this.groupBox1.AccessibleDescription = (string) null;
    this.groupBox1.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.groupBox1, "groupBox1");
    this.groupBox1.BackgroundImage = (Image) null;
    this.groupBox1.Controls.Add((Control) this.cbSteps);
    this.groupBox1.Font = (Font) null;
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.TabStop = false;
    this.cbSteps.AccessibleDescription = (string) null;
    this.cbSteps.AccessibleName = (string) null;
    componentResourceManager.ApplyResources((object) this.cbSteps, "cbSteps");
    this.cbSteps.BackgroundImage = (Image) null;
    this.cbSteps.DropDownStyle = ComboBoxStyle.DropDownList;
    this.cbSteps.Font = (Font) null;
    this.cbSteps.FormattingEnabled = true;
    this.cbSteps.Name = "cbSteps";
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AccessibleDescription = (string) null;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackgroundImage = (Image) null;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Font = (Font) null;
    this.Icon = (Icon) null;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SelectSchemeForm);
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.groupBox2.ResumeLayout(false);
    this.groupBox1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
