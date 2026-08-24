// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.GlueSearchForm
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class GlueSearchForm : Form
{
  private bool _lock;
  private Func<string, string, Dictionary<string, string>> _searchDelegate;
  private IContainer components;
  private ListView _lv1;
  private System.Windows.Forms.ColumnHeader _colName1;
  private Splitter splitter1;
  private ListView _lv2;
  private System.Windows.Forms.ColumnHeader _colName2;
  private Splitter splitter2;
  private ListView _lvResult;
  private System.Windows.Forms.ColumnHeader _colNameResult;
  private Button _btnCancel;

  internal string ImbaseKey { get; private set; }

  public GlueSearchForm(
    Dictionary<string, string> dict1,
    Dictionary<string, string> dict2,
    Func<string, string, Dictionary<string, string>> searchDelegate)
  {
    this.InitializeComponent();
    this._btnCancel.Location = new Point(0, -30);
    this._searchDelegate = searchDelegate;
    this.FillListView(this._lv1, dict1);
    this.FillListView(this._lv2, dict2);
  }

  private void On_lv_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.ClearResultListView();
    if (this._lv1.SelectedItems.Count <= 0 || this._lv2.SelectedItems.Count <= 0)
      return;
    this.FillListView(this._lvResult, this._searchDelegate(this._lv1.SelectedItems[0].Name, this._lv2.SelectedItems[0].Name));
  }

  private void On_lv_SizeChanged(object sender, EventArgs e)
  {
    if (sender == null)
      return;
    ListView listView = sender as ListView;
    if (this._lock || listView == null || listView.Columns.Count <= 0 || listView.Columns[0] == null)
      return;
    this._lock = true;
    listView.Columns[0].Width = -2;
    this._lock = false;
  }

  private void On_lvGlue_DoubleClick(object sender, EventArgs e)
  {
    if (this._lvResult.SelectedItems.Count > 0)
    {
      this.ImbaseKey = this._lvResult.SelectedItems[0].Name;
      this.Close();
    }
    else
      this.ImbaseKey = string.Empty;
  }

  internal void SetCaptions(
    string captionForm,
    string caption1,
    string caption2,
    string captionResult)
  {
    this.Text = captionForm;
    this._colName1.Text = caption1;
    this._colName2.Text = caption2;
    this._colNameResult.Text = captionResult;
  }

  private void FillListView(ListView lv, Dictionary<string, string> items)
  {
    if (items == null || items.Count <= 0)
      return;
    lv.SuspendLayout();
    try
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      foreach (KeyValuePair<string, string> keyValuePair in items)
      {
        string key = keyValuePair.Key;
        ListViewItem listViewItem = new ListViewItem(keyValuePair.Value)
        {
          Name = key
        };
        lv.Items.Add(listViewItem);
      }
    }
    finally
    {
      lv.ResumeLayout();
    }
  }

  private void ClearResultListView()
  {
    this._lvResult.SuspendLayout();
    try
    {
      this._lvResult.Items.Clear();
    }
    finally
    {
      this._lvResult.ResumeLayout();
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (GlueSearchForm));
    this._lv1 = new ListView();
    this._colName1 = new System.Windows.Forms.ColumnHeader();
    this.splitter1 = new Splitter();
    this._lv2 = new ListView();
    this._colName2 = new System.Windows.Forms.ColumnHeader();
    this.splitter2 = new Splitter();
    this._lvResult = new ListView();
    this._colNameResult = new System.Windows.Forms.ColumnHeader();
    this._btnCancel = new Button();
    this.SuspendLayout();
    this._lv1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._colName1
    });
    componentResourceManager.ApplyResources((object) this._lv1, "_lv1");
    this._lv1.FullRowSelect = true;
    this._lv1.HideSelection = false;
    this._lv1.MultiSelect = false;
    this._lv1.Name = "_lv1";
    this._lv1.Sorting = SortOrder.Ascending;
    this._lv1.UseCompatibleStateImageBehavior = false;
    this._lv1.View = View.Details;
    this._lv1.SelectedIndexChanged += new EventHandler(this.On_lv_SelectedIndexChanged);
    this._lv1.SizeChanged += new EventHandler(this.On_lv_SizeChanged);
    componentResourceManager.ApplyResources((object) this._colName1, "_colName1");
    componentResourceManager.ApplyResources((object) this.splitter1, "splitter1");
    this.splitter1.Name = "splitter1";
    this.splitter1.TabStop = false;
    this._lv2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._colName2
    });
    componentResourceManager.ApplyResources((object) this._lv2, "_lv2");
    this._lv2.FullRowSelect = true;
    this._lv2.HideSelection = false;
    this._lv2.MultiSelect = false;
    this._lv2.Name = "_lv2";
    this._lv2.Sorting = SortOrder.Ascending;
    this._lv2.UseCompatibleStateImageBehavior = false;
    this._lv2.View = View.Details;
    this._lv2.SelectedIndexChanged += new EventHandler(this.On_lv_SelectedIndexChanged);
    this._lv2.SizeChanged += new EventHandler(this.On_lv_SizeChanged);
    componentResourceManager.ApplyResources((object) this._colName2, "_colName2");
    componentResourceManager.ApplyResources((object) this.splitter2, "splitter2");
    this.splitter2.Name = "splitter2";
    this.splitter2.TabStop = false;
    this._lvResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
    {
      this._colNameResult
    });
    componentResourceManager.ApplyResources((object) this._lvResult, "_lvResult");
    this._lvResult.FullRowSelect = true;
    this._lvResult.HideSelection = false;
    this._lvResult.MultiSelect = false;
    this._lvResult.Name = "_lvResult";
    this._lvResult.Sorting = SortOrder.Ascending;
    this._lvResult.UseCompatibleStateImageBehavior = false;
    this._lvResult.View = View.Details;
    this._lvResult.SizeChanged += new EventHandler(this.On_lv_SizeChanged);
    this._lvResult.DoubleClick += new EventHandler(this.On_lvGlue_DoubleClick);
    componentResourceManager.ApplyResources((object) this._colNameResult, "_colNameResult");
    this._btnCancel.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this._btnCancel, "_btnCancel");
    this._btnCancel.Name = "_btnCancel";
    this._btnCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnCancel;
    this.Controls.Add((Control) this._btnCancel);
    this.Controls.Add((Control) this._lvResult);
    this.Controls.Add((Control) this.splitter2);
    this.Controls.Add((Control) this._lv2);
    this.Controls.Add((Control) this.splitter1);
    this.Controls.Add((Control) this._lv1);
    this.DoubleBuffered = true;
    this.Name = nameof (GlueSearchForm);
    this.ShowInTaskbar = false;
    this.ResumeLayout(false);
  }
}
