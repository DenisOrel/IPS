// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ExecutionOrderEditor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Client.Core;
using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal class ExecutionOrderEditor : Form
{
  private long _resolutionID;
  private bool _changed;
  private bool _selfChange;
  private IContainer components;
  private ListView listView1;
  private ColumnHeader columnHeader1;
  private ColumnHeader columnHeader2;
  private Label label1;
  private NumericUpDown nudIndex;
  private Button bOK;
  private Button bCancel;

  public ExecutionOrderEditor()
  {
    this.InitializeComponent();
    FormStorage.LoadLayout((Control) this);
  }

  public void Init([NotNull] DesForm dForm)
  {
    this._resolutionID = dForm.Info.ElementIdentifier;
    dForm.GetLinkedControls();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      Dictionary<long, string> executors = ExecutionOrderEditor.GetExecutors(sessionKeeper.Session, dForm);
      if (executors != null)
      {
        if (executors.Count > 0)
        {
          IDBAttribute attributeById = sessionKeeper.Session.GetObject(this._resolutionID).GetAttributeByID(OfficeConsts.AttrExecutionOrderID);
          int num1 = 1;
          foreach (KeyValuePair<long, string> keyValuePair in executors)
          {
            int num2 = num1;
            if (attributeById != null && attributeById.ValuesCount >= num1 && attributeById.Values[num1 - 1] != DBNull.Value)
              num2 = Convert.ToInt32(attributeById.Values[num1 - 1]);
            else
              this._changed = true;
            this.listView1.Items.Add(new ListViewItem(new string[2]
            {
              keyValuePair.Value,
              num2.ToString()
            })
            {
              Tag = (object) num2
            });
            ++num1;
          }
        }
      }
    }
    this.nudIndex.Enabled = this.listView1.Items.Count != 0;
    if (this.listView1.Items.Count > 0)
    {
      this.listView1.Items[0].Selected = true;
      this.listView1_SelectedIndexChanged((object) this, new EventArgs());
    }
    this.RefreshButtonsState();
  }

  [CanBeNull]
  public List<int> ExecutionOrders
  {
    get
    {
      if (this.listView1.Items.Count == 0)
        return (List<int>) null;
      List<int> executionOrders = new List<int>(this.listView1.Items.Count);
      for (int index = 0; index < this.listView1.Items.Count; ++index)
        executionOrders.Add((int) this.listView1.Items[index].Tag);
      return executionOrders;
    }
  }

  [CanBeNull]
  private static Dictionary<long, string> GetExecutors([NotNull] IUserSession session, [NotNull] DesForm dForm)
  {
    foreach (IAttributeEditor linkedControl in dForm.GetLinkedControls())
    {
      if (OfficeConsts.AttrExecutorsGuid.Equals(linkedControl.AttributeInfo.AttributeGuid))
      {
        if (linkedControl.Values != null)
        {
          if (linkedControl.Values.Values.Length != 0)
          {
            Dictionary<long, string> executors = new Dictionary<long, string>(linkedControl.Values.Values.Length);
            foreach (object obj in linkedControl.Values.Values)
            {
              if (obj != DBNull.Value)
              {
                long int64 = Convert.ToInt64(obj);
                QuickObjectInfo objectInfo = session.GetObjectInfo(int64);
                executors.Add(int64, objectInfo.Caption);
              }
            }
            return executors;
          }
          break;
        }
        break;
      }
    }
    return (Dictionary<long, string>) null;
  }

  private void RefreshButtonsState() => this.bOK.Enabled = this._changed;

  private void ExecutionOrderEditor_FormClosing([CanBeNull] object sender, [NotNull] FormClosingEventArgs e)
  {
    FormStorage.SaveLayout((Control) this);
  }

  private void listView1_SelectedIndexChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this.listView1.SelectedItems.Count == 0)
      return;
    try
    {
      this._selfChange = true;
      this.nudIndex.Value = (Decimal) (int) this.listView1.SelectedItems[0].Tag;
    }
    finally
    {
      this._selfChange = false;
    }
  }

  private void nudIndex_ValueChanged([CanBeNull] object sender, [NotNull] EventArgs e)
  {
    if (this._selfChange)
      return;
    this.listView1.SelectedItems[0].Tag = (object) (int) this.nudIndex.Value;
    this.listView1.SelectedItems[0].SubItems[1].Text = this.nudIndex.Value.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    this._changed = true;
    this.RefreshButtonsState();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.listView1 = new ListView();
    this.columnHeader1 = new ColumnHeader();
    this.columnHeader2 = new ColumnHeader();
    this.label1 = new Label();
    this.nudIndex = new NumericUpDown();
    this.bOK = new Button();
    this.bCancel = new Button();
    this.nudIndex.BeginInit();
    this.SuspendLayout();
    this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.listView1.Columns.AddRange(new ColumnHeader[2]
    {
      this.columnHeader1,
      this.columnHeader2
    });
    this.listView1.FullRowSelect = true;
    this.listView1.GridLines = true;
    this.listView1.Location = new Point(12, 12);
    this.listView1.MultiSelect = false;
    this.listView1.Name = "listView1";
    this.listView1.Size = new Size(475, 189);
    this.listView1.TabIndex = 0;
    this.listView1.UseCompatibleStateImageBehavior = false;
    this.listView1.View = View.Details;
    this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
    this.columnHeader1.Text = "Исполнитель";
    this.columnHeader1.Width = 336;
    this.columnHeader2.Text = "Этап исполнения";
    this.columnHeader2.Width = 129;
    this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 221);
    this.label1.Name = "label1";
    this.label1.Size = new Size(97, 13);
    this.label1.TabIndex = 1;
    this.label1.Text = "Этап исполнения:";
    this.nudIndex.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.nudIndex.Location = new Point(115, 217);
    this.nudIndex.Minimum = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this.nudIndex.Name = "nudIndex";
    this.nudIndex.Size = new Size(55, 20);
    this.nudIndex.TabIndex = 2;
    this.nudIndex.Value = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this.nudIndex.ValueChanged += new EventHandler(this.nudIndex_ValueChanged);
    this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bOK.DialogResult = DialogResult.OK;
    this.bOK.Location = new Point(239, 214);
    this.bOK.Name = "bOK";
    this.bOK.Size = new Size(121, 27);
    this.bOK.TabIndex = 3;
    this.bOK.Text = "Применить";
    this.bOK.UseVisualStyleBackColor = true;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(366, 214);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(121, 27);
    this.bCancel.TabIndex = 4;
    this.bCancel.Text = "Закрыть";
    this.bCancel.UseVisualStyleBackColor = true;
    this.AcceptButton = (IButtonControl) this.bOK;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.bCancel;
    this.ClientSize = new Size(496, (int) byte.MaxValue);
    this.Controls.Add((Control) this.bCancel);
    this.Controls.Add((Control) this.bOK);
    this.Controls.Add((Control) this.nudIndex);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.listView1);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ExecutionOrderEditor);
    this.StartPosition = FormStartPosition.Manual;
    this.Text = "Редактор порядка исполнения";
    this.FormClosing += new FormClosingEventHandler(this.ExecutionOrderEditor_FormClosing);
    this.nudIndex.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
