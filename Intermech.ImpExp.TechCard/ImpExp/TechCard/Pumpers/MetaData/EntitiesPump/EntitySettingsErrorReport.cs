// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump.EntitySettingsErrorReport
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using DevExpress.IM.XtraTreeList;
using DevExpress.IM.XtraTreeList.Columns;
using DevExpress.IM.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.EntitiesPump;

public class EntitySettingsErrorReport : Form
{
  private IContainer components;
  private TreeList tlErrors;
  private Button bCancel;
  private TreeListColumn Entity;
  private TreeListColumn Note;
  private Label label1;

  public EntitySettingsErrorReport() => this.InitializeComponent();

  public void LoadErrors(IEnumerable<EntityErrorRecord> errorRecords)
  {
    if (this.tlErrors == null)
      throw new ArgumentNullException(nameof (errorRecords));
    this.tlErrors.BeginUpdate();
    try
    {
      this.tlErrors.Nodes.Clear();
      Dictionary<string, TreeListNode> dictionary = new Dictionary<string, TreeListNode>();
      foreach (EntityErrorRecord errorRecord in errorRecords)
      {
        if (!dictionary.ContainsKey(errorRecord.Message))
        {
          TreeListNode treeListNode = this.tlErrors.AppendNode((object) new object[2]
          {
            (object) string.Empty,
            (object) errorRecord.Message
          }, (TreeListNode) null);
          dictionary.Add(errorRecord.Message, treeListNode);
        }
      }
      foreach (EntityErrorRecord errorRecord in errorRecords)
      {
        TreeListNode parentNode = dictionary[errorRecord.Message];
        this.tlErrors.AppendNode((object) new object[2]
        {
          (object) errorRecord.Entity.ToString(),
          (object) errorRecord.Message
        }, parentNode);
      }
      this.tlErrors.FullExpand();
    }
    finally
    {
      this.tlErrors.EndUpdate();
    }
  }

  public string EntityCode { get; private set; }

  private void Errors_MouseDoubleClick(object sender, MouseEventArgs e)
  {
    if (this.tlErrors.Selection[0].Nodes != null && this.tlErrors.Selection[0].Nodes.Count != 0)
      return;
    this.EntityCode = Convert.ToString(this.tlErrors.Selection[0].GetValue((object) 0));
    this.DialogResult = DialogResult.Abort;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.tlErrors = new TreeList();
    this.Entity = new TreeListColumn();
    this.Note = new TreeListColumn();
    this.bCancel = new Button();
    this.label1 = new Label();
    this.tlErrors.BeginInit();
    this.SuspendLayout();
    this.tlErrors.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.tlErrors.BehaviorOptions = BehaviorOptionsFlags.MoveOnEdit | BehaviorOptionsFlags.ExpandNodeOnDrag | BehaviorOptionsFlags.ShowToolTips | BehaviorOptionsFlags.ResizeNodes | BehaviorOptionsFlags.AutoSelectAllInEditor | BehaviorOptionsFlags.AutoNodeHeight | BehaviorOptionsFlags.AutoChangeParent | BehaviorOptionsFlags.CloseEditorOnLostFocus | BehaviorOptionsFlags.KeepSelectedOnClick | BehaviorOptionsFlags.SmartMouseHover;
    this.tlErrors.Columns.AddRange(new TreeListColumn[2]
    {
      this.Entity,
      this.Note
    });
    this.tlErrors.Location = new Point(12, 40);
    this.tlErrors.Name = "tlErrors";
    this.tlErrors.Size = new Size(433, 304);
    this.tlErrors.TabIndex = 0;
    this.tlErrors.Text = "treeList1";
    this.tlErrors.MouseDoubleClick += new MouseEventHandler(this.Errors_MouseDoubleClick);
    this.Entity.Caption = "Понятие";
    this.Entity.FieldName = "treeListColumn1";
    this.Entity.Name = "Entity";
    this.Entity.Options = ColumnOptions.CanMoved | ColumnOptions.CanResized | ColumnOptions.CanSorted | ColumnOptions.ReadOnly | ColumnOptions.CanFocused | ColumnOptions.ShowInCustomizationForm | ColumnOptions.CanMovedToCustomizationForm;
    this.Entity.VisibleIndex = 0;
    this.Note.Caption = "Описание ошибки";
    this.Note.FieldName = "treeListColumn1";
    this.Note.Name = "Note";
    this.Note.Options = ColumnOptions.CanMoved | ColumnOptions.CanResized | ColumnOptions.CanSorted | ColumnOptions.ReadOnly | ColumnOptions.CanFocused | ColumnOptions.ShowInCustomizationForm | ColumnOptions.CanMovedToCustomizationForm;
    this.Note.VisibleIndex = 1;
    this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.bCancel.DialogResult = DialogResult.Cancel;
    this.bCancel.Location = new Point(334, 350);
    this.bCancel.Name = "bCancel";
    this.bCancel.Size = new Size(111, 23);
    this.bCancel.TabIndex = 1;
    this.bCancel.Text = "Cancel";
    this.bCancel.UseVisualStyleBackColor = true;
    this.label1.AutoSize = true;
    this.label1.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
    this.label1.Location = new Point(13, 13);
    this.label1.Name = "label1";
    this.label1.Size = new Size(438, 15);
    this.label1.TabIndex = 2;
    this.label1.Text = "Следующие понятия не настроены либо настроены с ошибками:";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(457, 380);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.bCancel);
    this.Controls.Add((Control) this.tlErrors);
    this.Name = nameof (EntitySettingsErrorReport);
    this.Text = "Ошибки настройки понятий";
    this.tlErrors.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
