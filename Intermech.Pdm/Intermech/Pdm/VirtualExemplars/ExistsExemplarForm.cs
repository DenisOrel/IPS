// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VirtualExemplars.ExistsExemplarForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.VirtualExemplars;

internal class ExistsExemplarForm : Form
{
  private IContainer components;
  private Panel panel1;
  private PictureBox pictureBox1;
  private Label label1;
  private Panel panel2;
  private ListView listView1;
  private ColumnHeader columnHeader1;
  private ColumnHeader columnHeader2;
  private ColumnHeader columnHeader3;
  private ColumnHeader columnHeader4;
  private Button bCancel;
  private Button bOK;

  public ExistsExemplar SelectedExistsExemplar
  {
    get
    {
      return this.listView1.SelectedItems != null ? (ExistsExemplar) this.listView1.SelectedItems[0].Tag : (ExistsExemplar) null;
    }
  }

  public ExistsExemplarForm() => this.InitializeComponent();

  public void SetFormData(List<ExistsExemplar> eExs, VirtualExemplar exemplar)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(exemplar.ArticleID);
      string str = sessionKeeper.Session.GetObjectType(dbObject.ObjectType).ObjectInstanceName + " " + (dbObject.Caption != string.Empty ? $"\"{dbObject.Caption}\"" : string.Format(LocalizationHolder.rm.GetString("Pdm_408"), (object) exemplar.ArticleID));
      this.Text = exemplar.ArticlesInManufacture == ArticlesInManufacture.Instances ? LocalizationHolder.rm.GetString("Pdm_409") : LocalizationHolder.rm.GetString("Pdm_410");
      this.label1.Text = exemplar.ArticlesInManufacture == ArticlesInManufacture.Instances ? LocalizationHolder.rm.GetString("Pdm_411") : LocalizationHolder.rm.GetString("Pdm_412");
      this.label1.Text += str;
      if (exemplar.ArticlesInManufacture == ArticlesInManufacture.Instances)
        this.label1.Text += LocalizationHolder.rm.GetString("Pdm_413");
      else
        this.label1.Text += LocalizationHolder.rm.GetString("Pdm_414");
      this.listView1.BeginUpdate();
      this.listView1.Items.Clear();
      ICategoryTypeIconService service = (ICategoryTypeIconService) ServicesManager.GetService(typeof (ICategoryTypeIconService));
      if (service != null)
        this.listView1.SmallImageList = service.ImageList;
      foreach (ExistsExemplar existsExemplar in eExs)
      {
        ListViewItem listViewItem = new ListViewItem(new string[4]
        {
          existsExemplar.InstanceID.ToString(),
          existsExemplar.Name,
          existsExemplar.Designation,
          existsExemplar.SerialNo
        });
        listViewItem.Tag = (object) existsExemplar;
        if (service != null)
          listViewItem.ImageIndex = service.IndexOf(4, existsExemplar.InstanceTypeID);
        this.listView1.Items.Add(listViewItem);
      }
      this.listView1.EndUpdate();
    }
  }

  private void listView1_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.bOK.Enabled = this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ExistsExemplarForm));
    this.panel1 = new Panel();
    this.pictureBox1 = new PictureBox();
    this.label1 = new Label();
    this.panel2 = new Panel();
    this.bCancel = new Button();
    this.bOK = new Button();
    this.listView1 = new ListView();
    this.columnHeader1 = new ColumnHeader();
    this.columnHeader2 = new ColumnHeader();
    this.columnHeader3 = new ColumnHeader();
    this.columnHeader4 = new ColumnHeader();
    this.panel1.SuspendLayout();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.pictureBox1);
    this.panel1.Controls.Add((Control) this.label1);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    this.pictureBox1.Image = (Image) Intermech.Pdm.Properties.Resources.button_info;
    componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.TabStop = false;
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    this.panel2.Controls.Add((Control) this.bCancel);
    this.panel2.Controls.Add((Control) this.bOK);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.bCancel, "bCancel");
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Name = "bCancel";
    this.bCancel.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.bOK, "bOK");
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Name = "bOK";
    this.bOK.UseVisualStyleBackColor = true;
    this.listView1.Columns.AddRange(new ColumnHeader[4]
    {
      this.columnHeader1,
      this.columnHeader2,
      this.columnHeader3,
      this.columnHeader4
    });
    componentResourceManager.ApplyResources((object) this.listView1, "listView1");
    this.listView1.FullRowSelect = true;
    this.listView1.GridLines = true;
    this.listView1.MultiSelect = false;
    this.listView1.Name = "listView1";
    this.listView1.UseCompatibleStateImageBehavior = false;
    this.listView1.View = View.Details;
    this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this.columnHeader1, "columnHeader1");
    componentResourceManager.ApplyResources((object) this.columnHeader2, "columnHeader2");
    componentResourceManager.ApplyResources((object) this.columnHeader3, "columnHeader3");
    componentResourceManager.ApplyResources((object) this.columnHeader4, "columnHeader4");
    this.AcceptButton = (IButtonControl) this.bOK;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.Controls.Add((Control) this.listView1);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ExistsExemplarForm);
    this.ShowInTaskbar = false;
    this.Tag = (object) " ";
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.panel2.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
