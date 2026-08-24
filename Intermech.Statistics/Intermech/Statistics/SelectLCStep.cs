// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.SelectLCStep
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public class SelectLCStep : Form
{
  private List<IMSLifeCycleStep> _allLCSteps;
  public ListViewItem SelectedItem;
  private IContainer components;
  private ListView stepsListView;
  private Button btnOK;
  private Button btnCancel;

  public SelectLCStep(List<IMSLifeCycleStep> allLCSteps)
  {
    this.InitializeComponent();
    this._allLCSteps = allLCSteps;
  }

  private void SelectLCStep_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
    if (this._allLCSteps == null)
      return;
    this.stepsListView.Groups.Clear();
    this.stepsListView.Items.Clear();
    foreach (IMSLifeCycleScheme lcSchemes in MetaDataHelper.GetLCSchemesList())
      this.stepsListView.Groups.Add(lcSchemes.SchemaID.ToString(), lcSchemes.Name);
    foreach (IMSLifeCycleStep allLcStep in this._allLCSteps)
      this.stepsListView.Items.Add(new ListViewItem()
      {
        Text = allLcStep.Name,
        Tag = (object) allLcStep.Guid,
        Group = this.stepsListView.Groups[allLcStep.SchemaID.ToString()]
      });
  }

  private void btnOK_Click(object sender, EventArgs e)
  {
    this.SelectedItem = this.stepsListView.SelectedItems[0];
  }

  private void stepsListView_SelectedIndexChanged(object sender, EventArgs e)
  {
    this.btnOK.Enabled = this.stepsListView.SelectedItems.Count == 1;
  }

  private void SelectLCStep_FormClosed(object sender, FormClosedEventArgs e)
  {
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
    this.stepsListView = new ListView();
    this.btnOK = new Button();
    this.btnCancel = new Button();
    this.SuspendLayout();
    this.stepsListView.Alignment = ListViewAlignment.Default;
    this.stepsListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.stepsListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    this.stepsListView.Location = new Point(13, 13);
    this.stepsListView.MultiSelect = false;
    this.stepsListView.Name = "stepsListView";
    this.stepsListView.Size = new Size(559, 257);
    this.stepsListView.TabIndex = 0;
    this.stepsListView.TileSize = new Size(168, 30);
    this.stepsListView.UseCompatibleStateImageBehavior = false;
    this.stepsListView.View = View.SmallIcon;
    this.stepsListView.SelectedIndexChanged += new EventHandler(this.stepsListView_SelectedIndexChanged);
    this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOK.DialogResult = DialogResult.OK;
    this.btnOK.Enabled = false;
    this.btnOK.Location = new Point(416, 276);
    this.btnOK.Name = "btnOK";
    this.btnOK.Size = new Size(75, 23);
    this.btnOK.TabIndex = 1;
    this.btnOK.Text = "OK";
    this.btnOK.UseVisualStyleBackColor = true;
    this.btnOK.Click += new EventHandler(this.btnOK_Click);
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(497, 276);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(75, 23);
    this.btnCancel.TabIndex = 2;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(584, 311);
    this.Controls.Add((Control) this.btnCancel);
    this.Controls.Add((Control) this.btnOK);
    this.Controls.Add((Control) this.stepsListView);
    this.MinimumSize = new Size(600, 350);
    this.Name = nameof (SelectLCStep);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Выберите шаг ЖЦ";
    this.FormClosed += new FormClosedEventHandler(this.SelectLCStep_FormClosed);
    this.Load += new EventHandler(this.SelectLCStep_Load);
    this.ResumeLayout(false);
  }
}
