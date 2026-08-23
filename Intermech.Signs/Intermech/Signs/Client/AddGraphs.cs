// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.AddGraphs
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Client.Core;
using Intermech.Localization;
using Intermech.PropertyEditors;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

public class AddGraphs : Form
{
  private int step;
  private SelectorForm srForm;
  private SelectGraphs sgForm;
  private ICollection selectedList;
  private ArrayList idList;
  private IContainer components;
  private Button btnNext;
  private Button btnOK;
  private Button btnCancel;
  private Button btnUpdate;
  private Button btnBack;
  private Panel panel1;
  private Panel panel2;

  public ICollection SelectedList => this.selectedList;

  public ArrayList IDList => this.idList;

  public AddGraphs()
  {
    this.srForm = new SelectorForm(typeof (ObjectTypesFolder), LocalizationHolder.rm.GetString("Signs_48"), typeof (ObjectTypeFolder), true);
    this.sgForm = new SelectGraphs();
    this.sgForm.OnButtonApllyChange += new EventHandler(this.sgForm_OnButtonApllyChange);
    this.InitializeComponent();
  }

  public void LoadForm()
  {
    this.panel1.Controls.Clear();
    if (this.step == 0)
    {
      this.btnUpdate.Visible = false;
      this.btnNext.Enabled = true;
      this.btnBack.Enabled = false;
      this.btnOK.Enabled = false;
      this.Text = LocalizationHolder.rm.GetString("Signs_110");
      this.srForm.SelectorFilter = (ISelectorFilter) new FilterObjectType();
      this.srForm.InitSelectionAsCategory(this.idList, new ArrayList((ICollection) new int[1]
      {
        4
      }));
      this.srForm.SetParent((Control) this.panel1);
      this.srForm.MinimumSize = new Size(0, 150);
      this.srForm.Show();
    }
    else
    {
      this.btnUpdate.Visible = true;
      this.btnNext.Enabled = false;
      this.btnBack.Enabled = true;
      this.Text = LocalizationHolder.rm.GetString("Signs_111");
      this.btnOK.Enabled = this.sgForm.SelectedGraphs > 0;
      this.sgForm.MinimumSize = new Size(0, 150);
      this.sgForm.SetParent((Control) this.panel1);
      this.sgForm.Show();
    }
  }

  private void button2_Click(object sender, EventArgs e)
  {
    this.srForm.SelectNodes();
    if (this.srForm.IDList.Count > 0)
    {
      this.idList = this.srForm.IDList;
      ++this.step;
      this.LoadForm();
    }
    else
    {
      int num = (int) MessageBox.Show(LocalizationHolder.rm.GetString("Signs_110"), LocalizationHolder.rm.GetString("Signs_112"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
  }

  private void AddGraphs_Load(object sender, EventArgs e) => FormStorage.LoadLayout((Control) this);

  private void AddGraphs_FormClosed(object sender, FormClosedEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void btnBack_Click(object sender, EventArgs e)
  {
    --this.step;
    this.LoadForm();
  }

  private void btnUpdate_Click(object sender, EventArgs e) => this.sgForm.ListUpdate();

  private void btnOK_Click(object sender, EventArgs e)
  {
    this.selectedList = this.sgForm.SelectedList;
  }

  private void sgForm_OnButtonApllyChange(object sender, EventArgs e)
  {
    this.btnOK.Enabled = this.sgForm.SelectedGraphs > 0;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddGraphs));
    this.btnNext = new Button();
    this.btnOK = new Button();
    this.btnCancel = new Button();
    this.btnUpdate = new Button();
    this.btnBack = new Button();
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.btnNext, "btnNext");
    this.btnNext.Name = "btnNext";
    this.btnNext.UseVisualStyleBackColor = true;
    this.btnNext.Click += new EventHandler(this.button2_Click);
    componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Name = "btnOK";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.btnUpdate, "btnUpdate");
    this.btnUpdate.Name = "btnUpdate";
    this.btnUpdate.UseVisualStyleBackColor = true;
    this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
    componentResourceManager.ApplyResources((object) this.btnBack, "btnBack");
    this.btnBack.Name = "btnBack";
    this.btnBack.UseVisualStyleBackColor = true;
    this.btnBack.Click += new EventHandler(this.btnBack_Click);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    this.panel2.Controls.Add((Control) this.btnBack);
    this.panel2.Controls.Add((Control) this.btnNext);
    this.panel2.Controls.Add((Control) this.btnUpdate);
    this.panel2.Controls.Add((Control) this.btnOK);
    this.panel2.Controls.Add((Control) this.btnCancel);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this.AcceptButton = (IButtonControl) this.btnOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.panel2);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (AddGraphs);
    this.ShowInTaskbar = false;
    this.FormClosed += new FormClosedEventHandler(this.AddGraphs_FormClosed);
    this.Load += new EventHandler(this.AddGraphs_Load);
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
