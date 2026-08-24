// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ContextComposition.SelectAssembliesTCEForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.ContextComposition;

public class SelectAssembliesTCEForm : Form
{
  private IContainer components;
  private Button okBtn;
  private Button cancelBtn;
  private ListView assembliesList;
  private ColumnHeader objectID;
  private ColumnHeader objectCaption;

  public SelectAssembliesTCEForm(
    List<(long assembliesID, string assembliesCaption, long relationID)> assembliesTuples)
  {
    this.InitializeComponent();
    foreach ((long assembliesID, string assembliesCaption, long relationID) assembliesTuple in assembliesTuples)
    {
      ListViewItem listViewItem = new ListViewItem()
      {
        Text = assembliesTuple.assembliesID.ToString(),
        Tag = (object) assembliesTuple.relationID.ToString()
      };
      listViewItem.SubItems.Add(assembliesTuple.assembliesCaption);
      this.assembliesList.Items.Add(listViewItem);
    }
  }

  public long SelectedAssemblies
  {
    get
    {
      long result;
      return this.SelectedItem != null && long.TryParse(this.SelectedItem.Text, out result) ? result : -1L;
    }
  }

  public long SelectedAssemliesRelationID
  {
    get
    {
      long result;
      return this.SelectedItem != null && long.TryParse(this.SelectedItem.Tag.ToString(), out result) ? result : -1L;
    }
  }

  private void SelectAssembliesTCEForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void SelectAssembliesTCEForm_FormClosing(object sender, FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private ListViewItem SelectedItem
  {
    get
    {
      return this.assembliesList.SelectedItems.Count <= 0 ? (ListViewItem) null : this.assembliesList.SelectedItems[0];
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
    this.assembliesList = new ListView();
    this.objectID = new ColumnHeader();
    this.objectCaption = new ColumnHeader();
    this.SuspendLayout();
    this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.okBtn.DialogResult = DialogResult.OK;
    this.okBtn.Location = new Point(331, 339);
    this.okBtn.Name = "okBtn";
    this.okBtn.Size = new Size(75, 23);
    this.okBtn.TabIndex = 1;
    this.okBtn.Text = "ОК";
    this.okBtn.UseVisualStyleBackColor = true;
    this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.cancelBtn.DialogResult = DialogResult.Cancel;
    this.cancelBtn.Location = new Point(412, 339);
    this.cancelBtn.Name = "cancelBtn";
    this.cancelBtn.Size = new Size(75, 23);
    this.cancelBtn.TabIndex = 1;
    this.cancelBtn.Text = "Отмена";
    this.cancelBtn.UseVisualStyleBackColor = true;
    this.assembliesList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.assembliesList.Columns.AddRange(new ColumnHeader[2]
    {
      this.objectID,
      this.objectCaption
    });
    this.assembliesList.FullRowSelect = true;
    this.assembliesList.HideSelection = false;
    this.assembliesList.Location = new Point(12, 12);
    this.assembliesList.MultiSelect = false;
    this.assembliesList.Name = "assembliesList";
    this.assembliesList.Size = new Size(475, 321);
    this.assembliesList.TabIndex = 2;
    this.assembliesList.UseCompatibleStateImageBehavior = false;
    this.assembliesList.View = View.Details;
    this.objectID.Text = "Идентификатор версии объекта";
    this.objectID.Width = 180;
    this.objectCaption.Text = "Заголовок объекта";
    this.objectCaption.Width = 288;
    this.AcceptButton = (IButtonControl) this.okBtn;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.cancelBtn;
    this.ClientSize = new Size(499, 374);
    this.Controls.Add((Control) this.assembliesList);
    this.Controls.Add((Control) this.cancelBtn);
    this.Controls.Add((Control) this.okBtn);
    this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
    this.MinimumSize = new Size(515, 180);
    this.Name = nameof (SelectAssembliesTCEForm);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "Выберите сборочную единицу в составе которой откроется ТСЕ";
    this.FormClosing += new FormClosingEventHandler(this.SelectAssembliesTCEForm_FormClosing);
    this.Load += new EventHandler(this.SelectAssembliesTCEForm_Load);
    this.ResumeLayout(false);
  }
}
