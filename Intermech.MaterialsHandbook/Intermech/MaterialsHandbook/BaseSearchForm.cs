// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.BaseSearchForm
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Client.Core;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class BaseSearchForm : Form
{
  private IContainer components;
  private Panel _pnlBottom;
  private Button _btnClose;
  private Label _lbText;
  private Label _lbSearchIn;
  private GroupBox _gbSearchMode;
  private Button _btnSearch;
  private System.Windows.Forms.ColumnHeader _lvColName;
  private System.Windows.Forms.ColumnHeader _lvColPath;
  protected SplitContainer splitContainer1;
  protected ComboBox _cmbSearchIn;
  protected RadioButton _rbTemplate;
  protected RadioButton _rbEnd;
  protected RadioButton _rbEntry;
  protected RadioButton _rbBeg;
  protected RadioButton _rbExactly;
  protected TextBox _txtSearch;
  protected ListView _lvResult;
  protected Panel _pnlMaterialSearch;
  private Splitter _splt;
  protected Panel _pnl;

  public BaseSearchForm() => this.InitializeComponent();

  protected virtual void On_btnSearch_Click(object sender, EventArgs e)
  {
  }

  protected virtual void On_cmbSearchIn_SelectedIndexChanged(object sender, EventArgs e)
  {
  }

  protected virtual void On_lvResult_DoubleClick(object sender, EventArgs e)
  {
  }

  protected override void OnLoad(EventArgs e)
  {
    base.OnLoad(e);
    FormStorage.LoadLayout((Control) this);
  }

  protected override void OnClosed(EventArgs e)
  {
    base.OnClosed(e);
    FormStorage.SaveLayout((Control) this);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BaseSearchForm));
    this._pnlBottom = new Panel();
    this._btnClose = new Button();
    this.splitContainer1 = new SplitContainer();
    this._splt = new Splitter();
    this._pnl = new Panel();
    this._cmbSearchIn = new ComboBox();
    this._btnSearch = new Button();
    this._lbSearchIn = new Label();
    this._pnlMaterialSearch = new Panel();
    this._gbSearchMode = new GroupBox();
    this._rbTemplate = new RadioButton();
    this._rbEnd = new RadioButton();
    this._rbEntry = new RadioButton();
    this._rbBeg = new RadioButton();
    this._rbExactly = new RadioButton();
    this._txtSearch = new TextBox();
    this._lbText = new Label();
    this._lvResult = new ListView();
    this._lvColName = new System.Windows.Forms.ColumnHeader();
    this._lvColPath = new System.Windows.Forms.ColumnHeader();
    this._pnlBottom.SuspendLayout();
    this.splitContainer1.BeginInit();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.Panel2.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this._pnl.SuspendLayout();
    this._pnlMaterialSearch.SuspendLayout();
    this._gbSearchMode.SuspendLayout();
    this.SuspendLayout();
    this._pnlBottom.Controls.Add((Control) this._btnClose);
    componentResourceManager.ApplyResources((object) this._pnlBottom, "_pnlBottom");
    this._pnlBottom.Name = "_pnlBottom";
    componentResourceManager.ApplyResources((object) this._btnClose, "_btnClose");
    this._btnClose.DialogResult = DialogResult.Cancel;
    this._btnClose.Name = "_btnClose";
    this._btnClose.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this._splt);
    this.splitContainer1.Panel1.Controls.Add((Control) this._pnl);
    this.splitContainer1.Panel2.Controls.Add((Control) this._lvResult);
    componentResourceManager.ApplyResources((object) this._splt, "_splt");
    this._splt.Name = "_splt";
    this._splt.TabStop = false;
    this._pnl.Controls.Add((Control) this._cmbSearchIn);
    this._pnl.Controls.Add((Control) this._btnSearch);
    this._pnl.Controls.Add((Control) this._lbSearchIn);
    this._pnl.Controls.Add((Control) this._pnlMaterialSearch);
    componentResourceManager.ApplyResources((object) this._pnl, "_pnl");
    this._pnl.Name = "_pnl";
    componentResourceManager.ApplyResources((object) this._cmbSearchIn, "_cmbSearchIn");
    this._cmbSearchIn.DropDownStyle = ComboBoxStyle.DropDownList;
    this._cmbSearchIn.FormattingEnabled = true;
    this._cmbSearchIn.Name = "_cmbSearchIn";
    this._cmbSearchIn.SelectedIndexChanged += new EventHandler(this.On_cmbSearchIn_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this._btnSearch, "_btnSearch");
    this._btnSearch.Name = "_btnSearch";
    this._btnSearch.UseVisualStyleBackColor = true;
    this._btnSearch.Click += new EventHandler(this.On_btnSearch_Click);
    componentResourceManager.ApplyResources((object) this._lbSearchIn, "_lbSearchIn");
    this._lbSearchIn.Name = "_lbSearchIn";
    componentResourceManager.ApplyResources((object) this._pnlMaterialSearch, "_pnlMaterialSearch");
    this._pnlMaterialSearch.Controls.Add((Control) this._gbSearchMode);
    this._pnlMaterialSearch.Controls.Add((Control) this._txtSearch);
    this._pnlMaterialSearch.Controls.Add((Control) this._lbText);
    this._pnlMaterialSearch.Name = "_pnlMaterialSearch";
    componentResourceManager.ApplyResources((object) this._gbSearchMode, "_gbSearchMode");
    this._gbSearchMode.Controls.Add((Control) this._rbTemplate);
    this._gbSearchMode.Controls.Add((Control) this._rbEnd);
    this._gbSearchMode.Controls.Add((Control) this._rbEntry);
    this._gbSearchMode.Controls.Add((Control) this._rbBeg);
    this._gbSearchMode.Controls.Add((Control) this._rbExactly);
    this._gbSearchMode.Name = "_gbSearchMode";
    this._gbSearchMode.TabStop = false;
    componentResourceManager.ApplyResources((object) this._rbTemplate, "_rbTemplate");
    this._rbTemplate.Name = "_rbTemplate";
    this._rbTemplate.Tag = (object) "4";
    this._rbTemplate.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._rbEnd, "_rbEnd");
    this._rbEnd.Name = "_rbEnd";
    this._rbEnd.Tag = (object) "3";
    this._rbEnd.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._rbEntry, "_rbEntry");
    this._rbEntry.Name = "_rbEntry";
    this._rbEntry.Tag = (object) "2";
    this._rbEntry.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._rbBeg, "_rbBeg");
    this._rbBeg.Name = "_rbBeg";
    this._rbBeg.Tag = (object) "1";
    this._rbBeg.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._rbExactly, "_rbExactly");
    this._rbExactly.Checked = true;
    this._rbExactly.Name = "_rbExactly";
    this._rbExactly.TabStop = true;
    this._rbExactly.Tag = (object) "0";
    this._rbExactly.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this._txtSearch, "_txtSearch");
    this._txtSearch.Name = "_txtSearch";
    componentResourceManager.ApplyResources((object) this._lbText, "_lbText");
    this._lbText.Name = "_lbText";
    this._lvResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2]
    {
      this._lvColName,
      this._lvColPath
    });
    componentResourceManager.ApplyResources((object) this._lvResult, "_lvResult");
    this._lvResult.FullRowSelect = true;
    this._lvResult.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    this._lvResult.MultiSelect = false;
    this._lvResult.Name = "_lvResult";
    this._lvResult.UseCompatibleStateImageBehavior = false;
    this._lvResult.View = View.Details;
    this._lvResult.DoubleClick += new EventHandler(this.On_lvResult_DoubleClick);
    componentResourceManager.ApplyResources((object) this._lvColName, "_lvColName");
    componentResourceManager.ApplyResources((object) this._lvColPath, "_lvColPath");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this._btnClose;
    this.Controls.Add((Control) this.splitContainer1);
    this.Controls.Add((Control) this._pnlBottom);
    this.DoubleBuffered = true;
    this.Name = nameof (BaseSearchForm);
    this.ShowInTaskbar = false;
    this._pnlBottom.ResumeLayout(false);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.Panel2.ResumeLayout(false);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    this._pnl.ResumeLayout(false);
    this._pnl.PerformLayout();
    this._pnlMaterialSearch.ResumeLayout(false);
    this._pnlMaterialSearch.PerformLayout();
    this._gbSearchMode.ResumeLayout(false);
    this._gbSearchMode.PerformLayout();
    this.ResumeLayout(false);
  }

  private class ComboBoxItem
  {
    internal int Num = -1;
    internal string Text = string.Empty;

    public ComboBoxItem(int num, string text)
    {
      this.Num = num;
      this.Text = text;
    }

    public override string ToString() => this.Text;
  }
}
