// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.OrderPointSelection.OrderPointSelectionForm
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

#nullable disable
namespace Intermech.Pdm.OrderPointSelection;

public class OrderPointSelectionForm : Form
{
  private readonly List<long> assemblyUnitPoints;
  private IContainer components;
  private Panel panel1;
  private Button btnCancel;
  private Button btnOk;
  private Panel panel2;
  private ObjectsViewBase objectsViewBase1;

  public long SelectedPointID { get; private set; }

  public OrderPointSelectionForm(List<long> assemblyUnitPoints)
  {
    this.InitializeComponent();
    this.objectsViewBase1.Grid.SelectionMode = iGSelectionMode.One;
    this.assemblyUnitPoints = assemblyUnitPoints;
    this.objectsViewBase1.Initialize((IDescriptor) new ListDescriptor(Intermech.Navigator.Consts.CategoryAllObjectTypes, 0, string.Empty, (IList) this.assemblyUnitPoints), (IServiceProvider) this.objectsViewBase1.Services);
    this.objectsViewBase1.Activate((IView) null);
  }

  private void btnOk_Click(object sender, EventArgs e)
  {
    if (this.objectsViewBase1.SelectedItems.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData)
      this.SelectedPointID = itemData.ObjectID;
    this.objectsViewBase1.Deactivate((IView) null);
    this.Close();
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    this.objectsViewBase1.Deactivate((IView) null);
    this.Close();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.panel1 = new Panel();
    this.btnCancel = new Button();
    this.btnOk = new Button();
    this.panel2 = new Panel();
    this.objectsViewBase1 = new ObjectsViewBase();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.btnCancel);
    this.panel1.Controls.Add((Control) this.btnOk);
    this.panel1.Dock = DockStyle.Bottom;
    this.panel1.Location = new Point(0, 350);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(747, 63 /*0x3F*/);
    this.panel1.TabIndex = 1;
    this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Location = new Point(607, 15);
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Size = new Size(128 /*0x80*/, 36);
    this.btnCancel.TabIndex = 1;
    this.btnCancel.Text = "Отмена";
    this.btnCancel.UseVisualStyleBackColor = true;
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btnOk.Location = new Point(462, 15);
    this.btnOk.Name = "btnOk";
    this.btnOk.Size = new Size(128 /*0x80*/, 36);
    this.btnOk.TabIndex = 0;
    this.btnOk.Text = "OK";
    this.btnOk.UseVisualStyleBackColor = true;
    this.btnOk.Click += new EventHandler(this.btnOk_Click);
    this.panel2.Controls.Add((Control) this.objectsViewBase1);
    this.panel2.Dock = DockStyle.Fill;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(747, 350);
    this.panel2.TabIndex = 2;
    this.objectsViewBase1.AllowCustomGroupValues = true;
    this.objectsViewBase1.AutoSize = true;
    this.objectsViewBase1.Control = (object) this.objectsViewBase1;
    this.objectsViewBase1.DisableDoubleClicks = true;
    this.objectsViewBase1.DisableFiltration = true;
    this.objectsViewBase1.DisableGroupBox = true;
    this.objectsViewBase1.DisableIMContextMenu = true;
    this.objectsViewBase1.DisableKeyDownEvents = false;
    this.objectsViewBase1.DisableStatusBar = true;
    this.objectsViewBase1.DisableToolBar = true;
    this.objectsViewBase1.Dock = DockStyle.Fill;
    this.objectsViewBase1.EmbeddedFocusAndSelection = (iFocusAndSelection) null;
    this.objectsViewBase1.Font = new Font("Tahoma", 8.25f);
    this.objectsViewBase1.Location = new Point(0, 0);
    this.objectsViewBase1.Name = "objectsViewBase1";
    this.objectsViewBase1.Size = new Size(747, 350);
    this.objectsViewBase1.TabIndex = 0;
    this.AcceptButton = (IButtonControl) this.btnOk;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.ClientSize = new Size(747, 413);
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Name = nameof (OrderPointSelectionForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Выберите точку, в которую надо добавить изделие";
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel2.PerformLayout();
    this.ResumeLayout(false);
  }
}
