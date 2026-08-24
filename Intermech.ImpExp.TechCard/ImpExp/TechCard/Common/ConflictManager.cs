// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.ConflictManager
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using DevExpress.IM.Utils;
using DevExpress.IM.XtraTreeList;
using DevExpress.IM.XtraTreeList.Columns;
using DevExpress.IM.XtraTreeList.Nodes;
using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

public class ConflictManager : Form, IDisposable
{
  private IContainer components;
  private Button button2;
  private Button button1;
  private TableLayoutPanel tableLayoutPanel1;
  private Button button3;
  private Button button4;
  private TreeList treeList1;
  private TreeListColumn treeListColumn1;
  private TreeListColumn treeListColumn2;
  private TreeListColumn treeListColumn3;
  private Label label1;
  private TreeListColumn treeListColumn4;

  public ConflictManager()
  {
    this.InitializeComponent();
    this.LoadTreeList();
  }

  public void LoadTreeList()
  {
    try
    {
      Dictionary<Guid, TreeListNode> dictionary = new Dictionary<Guid, TreeListNode>();
      this.treeList1.StateImageList = new ImageList();
      foreach (Conflict initializationConflict in TechCardPlugin.InitializationConflictList)
      {
        TreeListNode parentNode1 = (TreeListNode) null;
        if (dictionary.ContainsKey(initializationConflict.TP_type_Guid))
          parentNode1 = dictionary[initializationConflict.TP_type_Guid];
        int key;
        TreeListNode treeListNode;
        if (dictionary.ContainsKey(initializationConflict.TP_type_Guid))
        {
          TreeList treeList1 = this.treeList1;
          object[] nodeData = new object[4];
          key = initializationConflict.Key;
          nodeData[0] = (object) key.ToString();
          nodeData[1] = (object) initializationConflict.Comments;
          nodeData[2] = (object) initializationConflict.Caption;
          nodeData[3] = (object) initializationConflict.Description;
          TreeListNode parentNode2 = parentNode1;
          treeListNode = treeList1.AppendNode((object) nodeData, parentNode2);
          treeListNode.StateImageIndex = this.treeList1.StateImageList.Images.Count - 1;
        }
        else
        {
          IObjectTypeItem byGuid = TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(initializationConflict.TP_type_Guid);
          MemoryStream memoryStream = new MemoryStream();
          memoryStream.Write(byGuid.Icon, 0, byGuid.Icon.Length);
          this.treeList1.StateImageList.Images.Add(Image.FromStream((Stream) memoryStream));
          treeListNode = this.treeList1.AppendNode((object) new object[4]
          {
            (object) byGuid.Name,
            null,
            (object) string.Empty,
            null
          }, parentNode1);
          treeListNode.StateImageIndex = this.treeList1.StateImageList.Images.Count - 1;
          TreeList treeList1 = this.treeList1;
          object[] nodeData = new object[4];
          key = initializationConflict.Key;
          nodeData[0] = (object) key.ToString();
          nodeData[1] = (object) initializationConflict.Comments;
          nodeData[2] = (object) initializationConflict.Caption;
          nodeData[3] = (object) initializationConflict.Description;
          TreeListNode parentNode3 = treeListNode;
          treeList1.AppendNode((object) nodeData, parentNode3).StateImageIndex = this.treeList1.StateImageList.Images.Count - 1;
        }
        if (!dictionary.ContainsKey(initializationConflict.TP_type_Guid))
          dictionary.Add(initializationConflict.TP_type_Guid, treeListNode);
      }
      this.treeList1.FullExpand();
    }
    catch (Exception ex)
    {
      throw new Exception($"Невозможно обработать список конфликтов: {ex.Message}");
    }
  }

  public new void Dispose()
  {
    TechCardPlugin.InitializationConflictList.Clear();
    base.Dispose();
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ConflictManager));
    this.button2 = new Button();
    this.button1 = new Button();
    this.tableLayoutPanel1 = new TableLayoutPanel();
    this.button3 = new Button();
    this.button4 = new Button();
    this.treeList1 = new TreeList();
    this.treeListColumn3 = new TreeListColumn();
    this.treeListColumn1 = new TreeListColumn();
    this.treeListColumn2 = new TreeListColumn();
    this.label1 = new Label();
    this.treeListColumn4 = new TreeListColumn();
    this.tableLayoutPanel1.SuspendLayout();
    this.treeList1.BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.button2, "button2");
    this.button2.DialogResult = DialogResult.Cancel;
    this.button2.Name = "button2";
    this.button2.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.button1, "button1");
    this.button1.DialogResult = DialogResult.OK;
    this.button1.Name = "button1";
    this.button1.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
    this.tableLayoutPanel1.Controls.Add((Control) this.button3, 0, 1);
    this.tableLayoutPanel1.Name = "tableLayoutPanel1";
    this.button3.DialogResult = DialogResult.OK;
    componentResourceManager.ApplyResources((object) this.button3, "button3");
    this.button3.Name = "button3";
    this.button3.UseVisualStyleBackColor = true;
    this.button4.DialogResult = DialogResult.Cancel;
    componentResourceManager.ApplyResources((object) this.button4, "button4");
    this.button4.Name = "button4";
    this.button4.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.treeList1, "treeList1");
    this.treeList1.CausesValidation = false;
    this.treeList1.Columns.AddRange(new TreeListColumn[4]
    {
      this.treeListColumn4,
      this.treeListColumn3,
      this.treeListColumn1,
      this.treeListColumn2
    });
    this.treeList1.Name = "treeList1";
    this.treeList1.Styles.AddReplace("Row", (object) new ViewStyle("Row", "TreeList", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204), "", StyleOptions.StyleEnabled | StyleOptions.UseImage, true, false, false, HorzAlignment.Default, VertAlignment.Center, (Image) null, SystemColors.Window, SystemColors.WindowText));
    componentResourceManager.ApplyResources((object) this.treeListColumn3, "treeListColumn3");
    this.treeListColumn3.Name = "treeListColumn3";
    componentResourceManager.ApplyResources((object) this.treeListColumn1, "treeListColumn1");
    this.treeListColumn1.Name = "treeListColumn1";
    componentResourceManager.ApplyResources((object) this.treeListColumn2, "treeListColumn2");
    this.treeListColumn2.Name = "treeListColumn2";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    componentResourceManager.ApplyResources((object) this.treeListColumn4, "treeListColumn4");
    this.treeListColumn4.Name = "treeListColumn4";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.treeList1);
    this.Controls.Add((Control) this.button2);
    this.Controls.Add((Control) this.button1);
    this.Name = nameof (ConflictManager);
    this.Tag = (object) " ";
    this.tableLayoutPanel1.ResumeLayout(false);
    this.treeList1.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
