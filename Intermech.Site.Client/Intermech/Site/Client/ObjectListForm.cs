// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ObjectListForm
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Client.Core;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Site.Client;

internal class ObjectListForm : Form
{
  private IContainer components;
  protected ObjectsListControl viewObjectsList;
  private Button button1;
  private Label label1;
  private Label lCount;

  public ObjectListForm(string caption)
  {
    this.InitializeComponent();
    this.Text = caption;
  }

  public void Initialize(IServiceProvider viewServices, List<long> objectIDs)
  {
    this.Initialize(viewServices, objectIDs, (List<int>) null);
  }

  public void Initialize(IServiceProvider viewServices, List<long> objectIDs, List<int> types)
  {
    new ObjectListView(this.viewObjectsList).InitView(viewServices, objectIDs, types);
    this.lCount.Text = objectIDs.Count.ToString();
  }

  private void ObjectListForm_Load(object sender, EventArgs e)
  {
    FormStorage.LoadLayout((Control) this);
  }

  private void ObjectListForm_FormClosing(object sender, FormClosingEventArgs e)
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
    this.button1 = new Button();
    this.label1 = new Label();
    this.lCount = new Label();
    this.viewObjectsList = new ObjectsListControl();
    this.SuspendLayout();
    this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.button1.DialogResult = DialogResult.Cancel;
    this.button1.Location = new Point(472, 298);
    this.button1.Name = "button1";
    this.button1.Size = new Size(121, 27);
    this.button1.TabIndex = 16 /*0x10*/;
    this.button1.Text = "Закрыть";
    this.button1.UseVisualStyleBackColor = true;
    this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(22, 298);
    this.label1.Margin = new Padding(0);
    this.label1.Name = "label1";
    this.label1.Size = new Size(40, 13);
    this.label1.TabIndex = 17;
    this.label1.Text = "Всего:";
    this.lCount.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.lCount.AutoSize = true;
    this.lCount.Location = new Point(57, 298);
    this.lCount.Margin = new Padding(0);
    this.lCount.Name = "lCount";
    this.lCount.Size = new Size(19, 13);
    this.lCount.TabIndex = 18;
    this.lCount.Text = "10";
    this.viewObjectsList.AllowCustomGroupValues = true;
    this.viewObjectsList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.viewObjectsList.Control = (object) this.viewObjectsList;
    this.viewObjectsList.DataLoaded = false;
    this.viewObjectsList.DisableColumnsGrouping = true;
    this.viewObjectsList.DisableGroupBox = true;
    this.viewObjectsList.DisableIMContextMenu = true;
    this.viewObjectsList.DisableKeyDownEvents = false;
    this.viewObjectsList.DisableStatusBar = true;
    this.viewObjectsList.DisableToolBar = true;
    this.viewObjectsList.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.viewObjectsList.Font = new Font("Tahoma", 8.25f);
    this.viewObjectsList.Location = new Point(12, 12);
    this.viewObjectsList.Name = "viewObjectsList";
    this.viewObjectsList.Size = new Size(581, 276);
    this.viewObjectsList.TabIndex = 15;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.button1;
    this.ClientSize = new Size(605, 337);
    this.Controls.Add((Control) this.lCount);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.button1);
    this.Controls.Add((Control) this.viewObjectsList);
    this.Name = nameof (ObjectListForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.FormClosing += new FormClosingEventHandler(this.ObjectListForm_FormClosing);
    this.Load += new EventHandler(this.ObjectListForm_Load);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
