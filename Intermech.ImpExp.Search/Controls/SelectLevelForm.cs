// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.Controls.SelectLevelForm
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Search.Controls;

internal class SelectLevelForm : Form
{
  private IContainer components;
  private ListView listView1;
  private Button bOK;
  private Button bCancel;

  public SelectLevelForm(List<Tuple<string, int>> levels, ImageList levelsImageList)
  {
    this.InitializeComponent();
    if (levels.Count > 0)
    {
      this.listView1.SmallImageList = levelsImageList;
      this.SetLevels(levels);
    }
    this.RefreshButtons();
  }

  public Tuple<string, int> SelectedLevel
  {
    get
    {
      return this.listView1.SelectedItems.Count == 1 ? new Tuple<string, int>(this.listView1.SelectedItems[0].Text, (int) this.listView1.SelectedItems[0].Tag) : (Tuple<string, int>) null;
    }
  }

  private void SetLevels(List<Tuple<string, int>> levels)
  {
    (ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter).GetUserSession();
    int num1 = 0;
    foreach (Tuple<string, int> level in levels)
    {
      ListViewItem listViewItem = this.listView1.Items.Add(level.Item1);
      listViewItem.Tag = (object) level.Item2;
      int num2;
      num1 = num2 = num1 + 1;
      listViewItem.ImageIndex = num2;
    }
    this.listView1.Items[0].Selected = true;
  }

  private void RefreshButtons()
  {
    this.bOK.Enabled = this.listView1.Items.Count > 0 && this.listView1.SelectedItems.Count > 0;
  }

  private void listView1_SelectedIndexChanged(object sender, EventArgs e) => this.RefreshButtons();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectLevelForm));
    this.listView1 = new ListView();
    this.bOK = new Button();
    this.bCancel = new Button();
    this.SuspendLayout();
    this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.listView1.HideSelection = false;
    this.listView1.Location = new Point(1, 1);
    this.listView1.MultiSelect = false;
    this.listView1.Name = "listView1";
    this.listView1.Size = new Size(226, 212);
    this.listView1.TabIndex = 0;
    this.listView1.UseCompatibleStateImageBehavior = false;
    this.listView1.View = View.List;
    this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Location = new Point(59, 217);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(75, 23);
    this.bOK.TabIndex = 1;
    this.bOK.Text = "OK";
    this.bOK.UseVisualStyleBackColor = true;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(140, 217);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(75, 23);
    this.bCancel.TabIndex = 2;
    this.bCancel.Text = "Отмена";
    this.bCancel.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(228, 248);
    this.Controls.Add((Control) this.bCancel);
    this.Controls.Add((Control) this.bOK);
    this.Controls.Add((Control) this.listView1);
    this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.MinimumSize = new Size(230, 275);
    this.Name = nameof (SelectLevelForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Уровни продвижения";
    this.ResumeLayout(false);
  }
}
