// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.SelectRootTaskActivity
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Statistics.Configurations;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics;

public class SelectRootTaskActivity : Form
{
  public List<RootActivityListBox> SelectedActivity = new List<RootActivityListBox>();
  private bool _checkIsOne;
  private DataRowCollection _findRows;
  private IContainer components;
  private Button okBtn;
  private Button cancelBtn;
  private TabControl ActivityTabPage;
  private TabPage taskAct;
  private TabPage ApproveTab;
  private Label label1;
  private TextBox textBox1;
  private ListView taskActivityListView;
  private ListView approveActivityListView;
  private ColumnHeader objectName;
  private ColumnHeader objectID;
  private ColumnHeader approveObjectName;
  private ColumnHeader approveObjectID;

  public SelectRootTaskActivity(DataRowCollection findRows, bool checkIsOne = false)
  {
    this.InitializeComponent();
    this._checkIsOne = checkIsOne;
    this._findRows = findRows;
    if (checkIsOne)
    {
      this.taskActivityListView.MultiSelect = false;
      this.taskActivityListView.CheckBoxes = false;
      this.approveActivityListView.MultiSelect = false;
      this.approveActivityListView.CheckBoxes = false;
      this.label1.Visible = true;
      this.textBox1.Visible = true;
      this.taskActivityListView.SelectedIndexChanged += new EventHandler(this.taskListBox_SelectedIndexChanged);
      this.approveActivityListView.SelectedIndexChanged += new EventHandler(this.taskListBox_SelectedIndexChanged);
    }
    else
    {
      this.label1.Visible = false;
      this.textBox1.Visible = false;
      this.taskActivityListView.ItemCheck += new ItemCheckEventHandler(this.taskActivityListBox_ItemCheck);
      this.approveActivityListView.ItemCheck += new ItemCheckEventHandler(this.taskActivityListBox_ItemCheck);
    }
  }

  private void SelectRootTaskActivity_Load(object sender, EventArgs e)
  {
    foreach (DataRow findRow in (InternalDataCollectionBase) this._findRows)
      this.AddObjectToList(Convert.ToInt32(findRow.ItemArray[3]), findRow);
    this.SetVisibleForTabs();
  }

  private void SetVisibleForTabs()
  {
    if (this.taskActivityListView.Items.Count == 0)
      this.HidePage(ref this.taskAct);
    if (this.approveActivityListView.Items.Count != 0)
      return;
    this.HidePage(ref this.ApproveTab);
  }

  private void AddObjectToList(int objType, DataRow row)
  {
    if (objType == wfConsts.ApproveTypeID)
    {
      ListViewItem listViewItem = new ListViewItem()
      {
        Text = row.ItemArray[1].ToString(),
        Tag = (object) new RootActivityListBox(row.ItemArray[1], row.ItemArray[0], row.ItemArray[2])
      };
      listViewItem.SubItems.Add(row.ItemArray[0].ToString());
      this.approveActivityListView.Items.Add(listViewItem);
    }
    else
    {
      if (objType != wfConsts.TaskTypeID)
        return;
      ListViewItem listViewItem = new ListViewItem()
      {
        Text = row.ItemArray[1].ToString(),
        Tag = (object) new RootActivityListBox(row.ItemArray[1], row.ItemArray[0], row.ItemArray[2])
      };
      listViewItem.SubItems.Add(row.ItemArray[0].ToString());
      this.taskActivityListView.Items.Add(listViewItem);
      ControlHelper.AutoResizeColumns(this.taskActivityListView);
    }
  }

  private void HidePage(ref TabPage page)
  {
    this.ActivityTabPage.TabPages.Remove(page);
    page = (TabPage) null;
  }

  private void taskListBox_SelectedIndexChanged(object sender, EventArgs e)
  {
    ListView listView = sender as ListView;
    this.SelectedActivity.Clear();
    ListView.SelectedListViewItemCollection selectedItems = listView.SelectedItems;
    if (selectedItems.Count <= 0)
      return;
    RootActivityListBox tag = selectedItems[0].Tag as RootActivityListBox;
    this.SelectedActivity.Add(tag);
    this.textBox1.Text = $"{tag.ActivityCaption} ID: {tag.ActivityObjID}";
  }

  private void taskActivityListBox_ItemCheck(object sender, ItemCheckEventArgs e)
  {
    RootActivityListBox tag = (sender as ListView).Items[e.Index].Tag as RootActivityListBox;
    if (e.NewValue == CheckState.Checked)
    {
      if (this.SelectedActivity.Contains(tag))
        return;
      this.SelectedActivity.Add(tag);
    }
    else
    {
      if (e.NewValue != CheckState.Unchecked || !this.SelectedActivity.Contains(tag))
        return;
      this.SelectedActivity.Remove(tag);
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
    this.okBtn = new Button();
    this.cancelBtn = new Button();
    this.ActivityTabPage = new TabControl();
    this.taskAct = new TabPage();
    this.taskActivityListView = new ListView();
    this.objectName = new ColumnHeader();
    this.objectID = new ColumnHeader();
    this.ApproveTab = new TabPage();
    this.approveActivityListView = new ListView();
    this.label1 = new Label();
    this.textBox1 = new TextBox();
    this.approveObjectName = new ColumnHeader();
    this.approveObjectID = new ColumnHeader();
    this.ActivityTabPage.SuspendLayout();
    this.taskAct.SuspendLayout();
    this.ApproveTab.SuspendLayout();
    this.SuspendLayout();
    this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.okBtn.DialogResult = DialogResult.OK;
    this.okBtn.Location = new Point(331, 260);
    this.okBtn.Name = "okBtn";
    this.okBtn.Size = new Size(75, 23);
    this.okBtn.TabIndex = 1;
    this.okBtn.Text = "ОК";
    this.okBtn.UseVisualStyleBackColor = true;
    this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.cancelBtn.DialogResult = DialogResult.Cancel;
    this.cancelBtn.Location = new Point(412, 260);
    this.cancelBtn.Name = "cancelBtn";
    this.cancelBtn.Size = new Size(75, 23);
    this.cancelBtn.TabIndex = 1;
    this.cancelBtn.Text = "Отмена";
    this.cancelBtn.UseVisualStyleBackColor = true;
    this.ActivityTabPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.ActivityTabPage.Controls.Add((Control) this.taskAct);
    this.ActivityTabPage.Controls.Add((Control) this.ApproveTab);
    this.ActivityTabPage.Location = new Point(12, 12);
    this.ActivityTabPage.Name = "ActivityTabPage";
    this.ActivityTabPage.SelectedIndex = 0;
    this.ActivityTabPage.Size = new Size(475, 206);
    this.ActivityTabPage.TabIndex = 2;
    this.taskAct.Controls.Add((Control) this.taskActivityListView);
    this.taskAct.Location = new Point(4, 22);
    this.taskAct.Name = "taskAct";
    this.taskAct.Padding = new Padding(3);
    this.taskAct.Size = new Size(467, 180);
    this.taskAct.TabIndex = 1;
    this.taskAct.Text = "Задача";
    this.taskAct.UseVisualStyleBackColor = true;
    this.taskActivityListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.taskActivityListView.CheckBoxes = true;
    this.taskActivityListView.Columns.AddRange(new ColumnHeader[2]
    {
      this.objectName,
      this.objectID
    });
    this.taskActivityListView.FullRowSelect = true;
    this.taskActivityListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    this.taskActivityListView.Location = new Point(7, 6);
    this.taskActivityListView.Name = "taskActivityListView";
    this.taskActivityListView.ShowGroups = false;
    this.taskActivityListView.Size = new Size(454, 161);
    this.taskActivityListView.TabIndex = 4;
    this.taskActivityListView.UseCompatibleStateImageBehavior = false;
    this.taskActivityListView.View = View.Details;
    this.objectName.Text = "Наименование";
    this.objectName.Width = 100;
    this.objectID.Text = "Идентификатор объекта";
    this.objectID.Width = 148;
    this.ApproveTab.Controls.Add((Control) this.approveActivityListView);
    this.ApproveTab.Location = new Point(4, 22);
    this.ApproveTab.Name = "ApproveTab";
    this.ApproveTab.Size = new Size(467, 180);
    this.ApproveTab.TabIndex = 3;
    this.ApproveTab.Text = "Утверждение";
    this.ApproveTab.UseVisualStyleBackColor = true;
    this.approveActivityListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.approveActivityListView.CheckBoxes = true;
    this.approveActivityListView.Columns.AddRange(new ColumnHeader[2]
    {
      this.approveObjectName,
      this.approveObjectID
    });
    this.approveActivityListView.FullRowSelect = true;
    this.approveActivityListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    this.approveActivityListView.Location = new Point(7, 3);
    this.approveActivityListView.Name = "approveActivityListView";
    this.approveActivityListView.Size = new Size(454, 164);
    this.approveActivityListView.TabIndex = 6;
    this.approveActivityListView.UseCompatibleStateImageBehavior = false;
    this.approveActivityListView.View = View.Details;
    this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(9, 227);
    this.label1.Name = "label1";
    this.label1.Size = new Size(117, 13);
    this.label1.TabIndex = 3;
    this.label1.Text = "Выбранное действие:";
    this.textBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.textBox1.Enabled = false;
    this.textBox1.Location = new Point(135, 224 /*0xE0*/);
    this.textBox1.Multiline = true;
    this.textBox1.Name = "textBox1";
    this.textBox1.Size = new Size(348, 30);
    this.textBox1.TabIndex = 4;
    this.approveObjectName.Text = "Наименование";
    this.approveObjectName.Width = 104;
    this.approveObjectID.Text = "Идентификатор объекта";
    this.approveObjectID.Width = 144 /*0x90*/;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.cancelBtn;
    this.ClientSize = new Size(499, 295);
    this.Controls.Add((Control) this.textBox1);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.ActivityTabPage);
    this.Controls.Add((Control) this.cancelBtn);
    this.Controls.Add((Control) this.okBtn);
    this.MinimumSize = new Size(515, 300);
    this.Name = nameof (SelectRootTaskActivity);
    this.Text = "Выберите задачу для анализа";
    this.Load += new EventHandler(this.SelectRootTaskActivity_Load);
    this.ActivityTabPage.ResumeLayout(false);
    this.taskAct.ResumeLayout(false);
    this.ApproveTab.ResumeLayout(false);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
