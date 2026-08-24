// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionCopyComplectNumbersView
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Infralution.Controls;
using Infralution.Controls.VirtualTree;
using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Compositions.CompositionService;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

[ViewDescriptionProvider(typeof (ProductionCopyComplectNumbersView.ProductionCopyComplectNumbersViewDescriptionProvider))]
public class ProductionCopyComplectNumbersView : UserControl, IView, ICanDeactivateView
{
  private object _services;
  private long _relID;
  private ComplectNodeList _dataSource = new ComplectNodeList();
  private bool _modified;
  private NavigatorTreeNode _node;
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  protected Panel pnButtons;
  protected Button btCancel;
  protected Button btApply;
  protected Button add_button;
  protected Button del_button;
  protected Button change_button;
  private Intermech.VirtualTreeView.VirtualTreeView virtualTreeView1;
  private Column colCaption;
  private Column colStart;
  private Column colEnd;
  private CellEditor cellEditor1;
  private UniversalEditBox universalEditBox1;

  public ProductionCopyComplectNumbersView() => this.InitializeComponent();

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    this._services = (object) provider;
    this._node = items.GetItemData(0, typeof (NavigatorTreeNode)) as NavigatorTreeNode;
    this._relID = items.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData ? itemData.Value : 0L;
    this.virtualTreeView1.DataSource = (object) this._dataSource;
  }

  private void LoadData()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._dataSource.LoadData(sessionKeeper.Session, this._relID);
    this.Modified = false;
    this.virtualTreeView1.UpdateRows();
    this.virtualTreeView1.SelectedItem = (object) null;
  }

  private void SaveData()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._dataSource.SaveData(sessionKeeper.Session, this._relID);
    this.Modified = false;
  }

  public void Activate(IView previousView) => this.LoadData();

  public void Deactivate(IView nextView)
  {
    if (!this.Modified)
      return;
    if (MessageBox.Show("Вы изменили применяемость в комплектах. Сохранить сделанные изменения?", this.Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      this.SaveData();
    else
      this.LoadData();
  }

  public bool Modified
  {
    get => this._modified;
    set
    {
      this._modified = value;
      this.btApply.Enabled = this._modified;
      this.btCancel.Enabled = this._modified;
    }
  }

  public string Caption => "Применяемость в комплектах";

  public int ImageIndex => -1;

  public int OrderID => 21;

  private void btApply_Click(object sender, EventArgs e) => this.SaveData();

  private void btCancel_Click(object sender, EventArgs e) => this.LoadData();

  private void add_button_Click(object sender, EventArgs e)
  {
    long num = 0;
    long exitAsmID = 0;
    string max_count = "";
    if (this._node != null)
    {
      INodeID[] path = this._node.GetPath();
      if (path[0] is NodeID nodeId1 && MetaDataHelper.IsObjectTypeChildOf(nodeId1.ObjectTypeID, MRP2Consts.objtypeIdProductionLists))
        num = nodeId1.ObjectID;
      if (path.Length > 1 && path[1] is NodeID nodeId2 && MetaDataHelper.IsObjectTypeChildOf(nodeId2.ObjectTypeID, MRP2Consts.objtypeIdExitAssembly))
      {
        exitAsmID = nodeId2.ObjectID;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBAttribute attributeById = sessionKeeper.Session.GetRelation(nodeId2.PrjLinkID, false).GetAttributeByID(MRP2Consts.attrIdCount);
          if (attributeById != null)
            max_count = attributeById.AsString;
        }
      }
    }
    if (num == 0L || exitAsmID == 0L)
    {
      if (num == 0L)
      {
        IReadOnlyList<IDBObjectID> dbObjectIdList = SelectDialog.Objects((IReadOnlyCollection<int>) new int[1]
        {
          MRP2Consts.objtypeIdProductionLists
        }, "Выберите объект", options: SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect, operationName: "IndicateApplicability", disableGlobalContextMenuCommands: true);
        if (dbObjectIdList == null || dbObjectIdList.Count == 0)
          return;
        num = dbObjectIdList[0].Value;
      }
      if (exitAsmID == 0L)
      {
        List<ObjInfoItem> objects = new List<ObjInfoItem>();
        objects.Add(new ObjInfoItem(num));
        ColumnDescriptor[] columns = new ColumnDescriptor[2]
        {
          new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
          new ColumnDescriptor((object) MRP2Consts.attrIdCount, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
        };
        CompositionLoadingParams loadingParams = new CompositionLoadingParams((IEnumerable<ObjInfoItem>) objects, (IEnumerable<int>) new int[1]
        {
          MRP2Consts.objtypeIdExitAssembly
        }, (IEnumerable<int>) null, (IEnumerable<int>) new int[1]
        {
          MRP2Consts.reltypeIdProductComposition
        }, (IEnumerable<ColumnDescriptor>) columns, (IEnumerable<ConditionStructure>) null, true, false, 1, (VersionsRule) null, "cad00601-306c-11d8-b4e9-00304f19f545");
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          DataTable source = ServiceUtils.GetService<ICompositionLoadService>((object) sessionKeeper.Session, true).LoadComplexCompositions((object) sessionKeeper.Session, loadingParams);
          if (source == null || source.Rows.Count <= 0)
            throw new NotificationException("В выбранной ведомости отсутсвуют выходные сборочные единицы");
          object[] objArray = Intermech.Navigator.SelectionWindow.Select("Выберите выходную сборку", "", (IDescriptor) new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, MRP2Consts.objtypeIdExitAssembly, "", (IList) source.AsEnumerable().Select<DataRow, long>((System.Func<DataRow, long>) (row => Convert.ToInt64(row[0]))).ToList<long>()), typeof (IDBObjectID), SelectionOptions.HideTree | SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableMultiselect);
          if (objArray == null)
            return;
          exitAsmID = (objArray[0] as IDBObjectID).Value;
          max_count = source.Select($"([{-2}] = {exitAsmID})")[0][1].ToString();
        }
      }
    }
    string from = "";
    string to = "";
    if (DialogResult.OK != ComplectNumberDialog.Execute(ref from, ref to, max_count))
      return;
    int result1;
    int start = int.TryParse(from, out result1) ? result1 : -1;
    int result2 = int.TryParse(to, out result2) ? result2 : -1;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this._dataSource.AppendData(sessionKeeper.Session, num, exitAsmID, start, result2);
    this.virtualTreeView1.UpdateRows();
    this.Modified = true;
  }

  private void virtualTreeView1_GetChildren(object sender, GetChildrenEventArgs e)
  {
    if (e.Row.Item is ArrayList arrayList)
    {
      e.Children = (IList) arrayList;
    }
    else
    {
      if (!(e.Row.Item is ComplectNode complectNode))
        return;
      e.Children = complectNode.ChildNodes;
    }
  }

  private void virtualTreeView1_GetCellData(object sender, GetCellDataEventArgs e)
  {
    e.CellData.Value = (object) null;
    if (e.Row.Item is ComplectNode complectNode && e.Column == this.colCaption)
    {
      e.CellData.Value = (object) complectNode.Caption;
    }
    else
    {
      if (!(e.Row.Item is NumbersNode numbersNode))
        return;
      if (e.Column == this.colStart)
      {
        e.CellData.Value = (object) numbersNode.Start;
      }
      else
      {
        if (e.Column != this.colEnd)
          return;
        e.CellData.Value = (object) numbersNode.End;
      }
    }
  }

  private void virtualTreeView1_GetChildPolicy(object sender, GetChildPolicyEventArgs e)
  {
    e.ChildPolicy = RowChildPolicy.AutoExpand;
  }

  private void virtualTreeView1_GetParent(object sender, GetParentEventArgs e)
  {
    if (!(e.Item is ComplectNode complectNode))
      return;
    e.Parent = (object) complectNode.Parent;
    if (complectNode.Parent != null)
      return;
    e.Parent = this.virtualTreeView1.DataSource;
  }

  private void change_button_Click(object sender, EventArgs e)
  {
    if (!(this.virtualTreeView1.SelectedItem is NumbersNode selectedItem))
      return;
    string start = selectedItem.Start;
    string end = selectedItem.End;
    long objectId1 = (selectedItem.Parent as AssemblyNode).ObjectID;
    long objectId2 = selectedItem.Parent.Parent.ObjectID;
    string max_count = "";
    List<ObjInfoItem> objects = new List<ObjInfoItem>();
    objects.Add(new ObjInfoItem(objectId2));
    ColumnDescriptor[] columns = new ColumnDescriptor[2]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdCount, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    CompositionLoadingParams loadingParams = new CompositionLoadingParams((IEnumerable<ObjInfoItem>) objects, (IEnumerable<int>) new int[1]
    {
      MRP2Consts.objtypeIdExitAssembly
    }, (IEnumerable<int>) null, (IEnumerable<int>) new int[1]
    {
      MRP2Consts.reltypeIdProductComposition
    }, (IEnumerable<ColumnDescriptor>) columns, (IEnumerable<ConditionStructure>) null, true, false, 1, (VersionsRule) null, "cad00601-306c-11d8-b4e9-00304f19f545");
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable dataTable = ServiceUtils.GetService<ICompositionLoadService>((object) sessionKeeper.Session, true).LoadComplexCompositions((object) sessionKeeper.Session, loadingParams);
      if (dataTable != null)
      {
        if (dataTable.Rows.Count > 0)
        {
          DataRow[] dataRowArray = dataTable.Select($"([{-2}] = {objectId1})");
          if (dataRowArray != null)
          {
            if (dataRowArray.Length != 0)
              max_count = dataRowArray[0][1].ToString();
          }
        }
      }
    }
    if (DialogResult.OK != ComplectNumberDialog.Execute(ref start, ref end, max_count))
      return;
    selectedItem.Start = start;
    selectedItem.End = end;
    this.virtualTreeView1.UpdateRows();
    this.Modified = true;
  }

  private void del_button_Click(object sender, EventArgs e)
  {
    if (!(this.virtualTreeView1.SelectedItem is ComplectNode selectedItem))
      return;
    if (selectedItem.Parent == null)
      (this.virtualTreeView1.DataSource as ArrayList).Remove((object) selectedItem);
    else if (selectedItem.Parent.ChildNodes.Count == 1 && (selectedItem.Parent.Parent == null || selectedItem.Parent.Parent.ChildNodes.Count == 1))
      (this.virtualTreeView1.DataSource as ArrayList).Remove((object) selectedItem.Root);
    else
      selectedItem.Parent.ChildNodes.Remove((object) selectedItem);
    this.virtualTreeView1.UpdateRows();
    this.Modified = true;
  }

  public bool CanDeactivate(object sender)
  {
    if (!this.Modified)
      return true;
    int num = (int) MessageBox.Show("Подтверждение", "Данные не сохранены. Сохранить?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
    if (num == 6)
      this.SaveData();
    return num != 2;
  }

  private void virtualTreeView1_SelectionChanged(object sender, EventArgs e)
  {
    this.add_button.Enabled = this._relID != 0L;
    this.change_button.Enabled = this.add_button.Enabled && this.virtualTreeView1.SelectedItem is NumbersNode;
    this.del_button.Enabled = this.add_button.Enabled && this.virtualTreeView1.SelectedItem is ComplectNode;
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.colCaption = new Column();
    this.colStart = new Column();
    this.cellEditor1 = new CellEditor();
    this.universalEditBox1 = new UniversalEditBox();
    this.colEnd = new Column();
    this.pnButtons = new Panel();
    this.del_button = new Button();
    this.change_button = new Button();
    this.add_button = new Button();
    this.btCancel = new Button();
    this.btApply = new Button();
    this.virtualTreeView1 = new Intermech.VirtualTreeView.VirtualTreeView();
    this.pnButtons.SuspendLayout();
    this.virtualTreeView1.BeginInit();
    this.SuspendLayout();
    this.colCaption.Caption = "Ведомость/вых. сборка";
    this.colCaption.Name = "colCaption";
    this.colCaption.Width = 150;
    this.colStart.Caption = "с комплекта";
    this.colStart.Name = "colStart";
    this.cellEditor1.Control = (Control) this.universalEditBox1;
    this.universalEditBox1.Location = new Point(0, 0);
    this.universalEditBox1.Name = "universalEditBox1";
    this.universalEditBox1.Size = new Size(195, 20);
    this.universalEditBox1.TabIndex = 0;
    this.colEnd.Caption = "по комплект";
    this.colEnd.Name = "colEnd";
    this.pnButtons.Controls.Add((Control) this.del_button);
    this.pnButtons.Controls.Add((Control) this.change_button);
    this.pnButtons.Controls.Add((Control) this.add_button);
    this.pnButtons.Controls.Add((Control) this.btCancel);
    this.pnButtons.Controls.Add((Control) this.btApply);
    this.pnButtons.Dock = DockStyle.Bottom;
    this.pnButtons.Location = new Point(0, 467);
    this.pnButtons.Name = "pnButtons";
    this.pnButtons.Size = new Size(760, 40);
    this.pnButtons.TabIndex = 2;
    this.del_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.del_button.FlatStyle = FlatStyle.System;
    this.del_button.ImeMode = ImeMode.NoControl;
    this.del_button.Location = new Point(257, 6);
    this.del_button.Name = "del_button";
    this.del_button.Size = new Size(121, 27);
    this.del_button.TabIndex = 4;
    this.del_button.Text = "Удалить";
    this.del_button.Click += new EventHandler(this.del_button_Click);
    this.change_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.change_button.FlatStyle = FlatStyle.System;
    this.change_button.ImeMode = ImeMode.NoControl;
    this.change_button.Location = new Point(130, 6);
    this.change_button.Name = "change_button";
    this.change_button.Size = new Size(121, 27);
    this.change_button.TabIndex = 3;
    this.change_button.Text = "Изменить";
    this.change_button.Click += new EventHandler(this.change_button_Click);
    this.add_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.add_button.FlatStyle = FlatStyle.System;
    this.add_button.ImeMode = ImeMode.NoControl;
    this.add_button.Location = new Point(3, 6);
    this.add_button.Name = "add_button";
    this.add_button.Size = new Size(121, 27);
    this.add_button.TabIndex = 2;
    this.add_button.Text = "Добавить";
    this.add_button.Click += new EventHandler(this.add_button_Click);
    this.btCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btCancel.Enabled = false;
    this.btCancel.FlatStyle = FlatStyle.System;
    this.btCancel.ImeMode = ImeMode.NoControl;
    this.btCancel.Location = new Point(631, 6);
    this.btCancel.Name = "btCancel";
    this.btCancel.Size = new Size(121, 27);
    this.btCancel.TabIndex = 1;
    this.btCancel.Text = "Отмена";
    this.btCancel.Click += new EventHandler(this.btCancel_Click);
    this.btApply.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.btApply.Enabled = false;
    this.btApply.FlatStyle = FlatStyle.System;
    this.btApply.ImeMode = ImeMode.NoControl;
    this.btApply.Location = new Point(504, 6);
    this.btApply.Name = "btApply";
    this.btApply.Size = new Size(121, 27);
    this.btApply.TabIndex = 0;
    this.btApply.Text = "Применить";
    this.btApply.Click += new EventHandler(this.btApply_Click);
    this.virtualTreeView1.AllowDrop = true;
    this.virtualTreeView1.AllowMultiSelect = false;
    this.virtualTreeView1.AllowRowResize = false;
    this.virtualTreeView1.Columns.Add(this.colCaption);
    this.virtualTreeView1.Columns.Add(this.colStart);
    this.virtualTreeView1.Columns.Add(this.colEnd);
    this.virtualTreeView1.DisableHeaderContextMenu = false;
    this.virtualTreeView1.Dock = DockStyle.Fill;
    this.virtualTreeView1.Editors.Add(this.cellEditor1);
    this.virtualTreeView1.ImageList = (ImageList) null;
    this.virtualTreeView1.Location = new Point(0, 0);
    this.virtualTreeView1.Name = "virtualTreeView1";
    this.virtualTreeView1.ShowRootRow = false;
    this.virtualTreeView1.Size = new Size(760, 467);
    this.virtualTreeView1.TabIndex = 3;
    this.virtualTreeView1.GetCellData += new GetCellDataHandler(this.virtualTreeView1_GetCellData);
    this.virtualTreeView1.GetChildPolicy += new GetChildPolicyHandler(this.virtualTreeView1_GetChildPolicy);
    this.virtualTreeView1.GetChildren += new GetChildrenHandler(this.virtualTreeView1_GetChildren);
    this.virtualTreeView1.GetParent += new GetParentHandler(this.virtualTreeView1_GetParent);
    this.virtualTreeView1.SelectionChanged += new EventHandler(this.virtualTreeView1_SelectionChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.virtualTreeView1);
    this.Controls.Add((Control) this.pnButtons);
    this.Name = nameof (ProductionCopyComplectNumbersView);
    this.Size = new Size(760, 507);
    this.pnButtons.ResumeLayout(false);
    this.virtualTreeView1.EndInit();
    this.ResumeLayout(false);
  }

  private sealed class ProductionCopyComplectNumbersViewDescriptionProvider : 
    BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList))
        ServicesManager.GetService(typeof (INamedImageList));
      return new ViewDescription()
      {
        Caption = "Применяемость в комплектах",
        ImageIndex = -1,
        OrderID = 21
      };
    }
  }
}
