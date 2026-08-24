// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareRulesForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

public class CompareRulesForm : Form
{
  private CompareRulesTabList _tabsList;
  private CompoitionSettings _settings;
  private bool _selfChanged;
  private int _parentMode;
  private IContainer components;
  private Label label1;
  private TextBox tbName;
  private Button bCancel;
  private Button bApply;
  private TabControl tcSettingTabs;

  public Guid RuleGuid { get; set; } = Guid.Empty;

  public long RuleID { get; private set; }

  public CompareRulesForm()
  {
    this.InitializeComponent();
    this._tabsList = new CompareRulesTabList();
    this._tabsList.DataChanged += new EventHandler(this.TabsDataChanged);
    this._tabsList.AppendTabs(this.tcSettingTabs);
    FormStorage.LoadLayout((Control) this);
  }

  public bool IsChanged { get; private set; }

  public int ObjectType { get; set; } = -1;

  public int ParentMode
  {
    get => this._parentMode;
    set
    {
      switch (value)
      {
        case 1:
          this.bApply.Text = "Готово";
          break;
        case 2:
          this.bApply.Text = "Применить";
          break;
      }
      this._parentMode = value;
    }
  }

  public int EditorMode { get; private set; }

  private void TabsDataChanged(object sender, EventArgs e)
  {
    this.IsChanged = true;
    this.RefreshButtons();
  }

  public void SaveObjectData()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject ruleObject = !(this.RuleGuid == Guid.Empty) ? sessionKeeper.Session.GetObject(this.RuleGuid) : sessionKeeper.Session.GetObjectCollection(this.ObjectType).Create();
      ruleObject.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AsString = this.tbName.Text;
      ((ICompareTreeSettingsService) ServicesManager.GetService(typeof (ICompareTreeSettingsService))).SetCompoitionSettings(ruleObject, (ICompoitionSettings) this._settings);
      if (this.RuleGuid == Guid.Empty)
      {
        ruleObject.CommitCreation(true);
        this.RuleGuid = ruleObject.ObjectGUID;
        this.RuleID = ruleObject.ObjectID;
      }
    }
    this.IsChanged = false;
    this.RefreshButtons();
  }

  public void SetParent(Control parent)
  {
    if (parent == null)
    {
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.TopLevel = true;
      this.Dock = DockStyle.None;
      this.FormBorderStyle = FormBorderStyle.Sizable;
      this.Visible = false;
    }
    else
    {
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.TopLevel = false;
      this.Dock = DockStyle.Fill;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Visible = true;
    }
    this.Parent = parent;
  }

  private void RefreshButtons()
  {
    this.bCancel.Enabled = this._parentMode != 2 || this.IsChanged;
    this.bApply.Enabled = this._parentMode == 1 || this.IsChanged;
  }

  public void LoadObjectData(Guid ruleGuid, int editorMode)
  {
    this.IsChanged = false;
    this.EditorMode = editorMode;
    this._selfChanged = true;
    try
    {
      ICompareTreeSettingsService service = (ICompareTreeSettingsService) ServicesManager.GetService(typeof (ICompareTreeSettingsService));
      this._settings = ruleGuid != Guid.Empty ? (CompoitionSettings) service.GetCompoitionSettings(ruleGuid).Clone() : CompoitionSettings.CreateNew();
      this._tabsList.LoadSettingsToControls(this._settings);
      if (ruleGuid != Guid.Empty)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBObject dbObject = sessionKeeper.Session.GetObject(ruleGuid);
          this.tbName.Text = dbObject.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AsString;
          this.Name = dbObject.NameInMessages;
          this.RuleID = dbObject.ObjectID;
        }
      }
      else
      {
        this.tbName.Text = string.Empty;
        if (this._parentMode == 1)
          this.Text = "Создание правила сравнения составов";
        this.RuleID = 0L;
      }
      this.RuleGuid = ruleGuid;
      this.RefreshButtons();
    }
    finally
    {
      this._selfChanged = false;
    }
  }

  private void bApply_Click(object sender, EventArgs e)
  {
    this.SaveObjectData();
    if (this.ParentMode == 2)
      return;
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  private void bCancel_Click(object sender, EventArgs e)
  {
    if (this._parentMode != 2)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
    else
      this.LoadObjectData(this.RuleGuid, this.EditorMode);
  }

  private void CompareRulesForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void tbName_TextChanged(object sender, EventArgs e)
  {
    if (this._selfChanged)
      return;
    this.IsChanged = true;
    this.RefreshButtons();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.label1 = new Label();
    this.tbName = new TextBox();
    this.bCancel = new Button();
    this.bApply = new Button();
    this.tcSettingTabs = new TabControl();
    this.SuspendLayout();
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 24);
    this.label1.Name = "label1";
    this.label1.Size = new Size(83, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Наименование";
    this.tbName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.tbName.Location = new Point(101, 21);
    this.tbName.Name = "tbName";
    this.tbName.Size = new Size(671, 20);
    this.tbName.TabIndex = 1;
    this.tbName.TextChanged += new EventHandler(this.tbName_TextChanged);
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(647, 365);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(121, 27);
    this.bCancel.TabIndex = 3;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.bCancel.Click += new EventHandler(this.bCancel_Click);
    this.bApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bApply.Enabled = false;
    this.bApply.Location = new Point(520, 365);
    this.bApply.Name = "bApply";
    this.bApply.Size = new Size(121, 27);
    this.bApply.TabIndex = 4;
    this.bApply.Text = "OK";
    this.bApply.UseVisualStyleBackColor = true;
    this.bApply.Click += new EventHandler(this.bApply_Click);
    this.tcSettingTabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tcSettingTabs.Location = new Point(15, 65);
    this.tcSettingTabs.Name = "tcSettingTabs";
    this.tcSettingTabs.SelectedIndex = 0;
    this.tcSettingTabs.Size = new Size(757, 294);
    this.tcSettingTabs.TabIndex = 1;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(784, 404);
    this.Controls.Add((Control) this.tcSettingTabs);
    this.Controls.Add((Control) this.bApply);
    this.Controls.Add((Control) this.bCancel);
    this.Controls.Add((Control) this.tbName);
    this.Controls.Add((Control) this.label1);
    this.MinimumSize = new Size(800, 400);
    this.Name = nameof (CompareRulesForm);
    this.Text = "Настройка правила сравнения составов";
    this.FormClosing += new FormClosingEventHandler(this.CompareRulesForm_FormClosing);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
