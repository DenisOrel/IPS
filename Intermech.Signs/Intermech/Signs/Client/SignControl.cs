// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignControl
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using DevExpress.IM.XtraEditors;
using DevExpress.IM.XtraEditors.Controls;
using DevExpress.IM.XtraEditors.Repository;
using DevExpress.IM.XtraTreeList;
using DevExpress.IM.XtraTreeList.Columns;
using DevExpress.IM.XtraTreeList.Nodes;
using ImSSP;
using Intermech.Bars;
using Intermech.Localization;
using Intermech.Signs.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>
/// Контрол для настройки подписей (на архива и на типы объектов)
/// + уровни продвижения
/// </summary>
public class SignControl : UserControl
{
  private IContainer components;
  private bool _modified;
  private bool _readonly;
  private bool _isLayout;
  private GraphsSet _originalSet = new GraphsSet();
  private TreeList _tree;
  private Intermech.Bars.ToolBar toolBarSign;
  private ImageList imageListSign;
  private ButtonItem buttonItemAddGroup;
  private ButtonItem buttonItemAddGraph;
  private ButtonItem buttonItemEdit;
  private ButtonItem buttonItemDelete;
  private ButtonItem buttonItemClearGroup;
  private ButtonItem buttonItemClearList;
  private TreeListColumn treeListColumnSign;
  private TreeListColumn treeListColumnStrikt;
  private RepositoryItemTextEdit repositoryItemTextEdit1;
  private RepositoryItemCheckEdit repositoryItemCheckEdit1;
  private Panel panel1;
  private CheckBox _cbII;
  private TreeListNode _current;
  /// <summary>
  /// флаг для события FocusedColumnChanged
  /// если true, обрабатывать событие не надо,
  /// </summary>
  private bool isEdit;
  private MenuBarItem _menu;
  private MenuButtonItem _AddGroup;
  private MenuButtonItem _AddItem;
  private MenuButtonItem _RenameGroup;
  private MenuButtonItem _Delete;
  private MenuButtonItem _ClearGroup;
  private MenuButtonItem _ClearAll;

  private event EventHandler _onModified;

  private TreeListNode CurrentObject
  {
    get => this._current;
    set
    {
      this._current = value;
      this._isLayout = true;
      try
      {
        bool flag = false;
        if (value != null && value.Tag is GraphClass)
        {
          flag = true;
          this._cbII.Checked = (value.Tag as GraphClass).II;
        }
        if (flag)
          return;
        this._cbII.Checked = false;
      }
      finally
      {
        this._isLayout = false;
        this._tree.FullExpand();
      }
    }
  }

  /// <summary>Конструктор</summary>
  public SignControl()
  {
    this.InitializeComponent();
    this.InitMenu();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="disposing"></param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>Произоводились ли изменения</summary>
  public bool Modified
  {
    get => this._modified;
    set
    {
      this._modified = value;
      if (this._onModified == null)
        return;
      this._onModified.DynamicInvoke((object) this, (object) new EventArgs());
    }
  }

  /// <summary>Только для просмотра</summary>
  public bool ReadOnly
  {
    set
    {
      this._readonly = value;
      if (!this._readonly)
        this._tree.BehaviorOptions |= BehaviorOptionsFlags.Editable;
      else
        this._tree.BehaviorOptions &= ~BehaviorOptionsFlags.Editable;
      this._cbII.Enabled = !value;
    }
    get => this._readonly;
  }

  /// <summary>Событые на изменение содержимого, можно подписываться</summary>
  public event EventHandler OnModified
  {
    add => this._onModified += value;
    remove => this._onModified -= value;
  }

  /// <summary>Набор подписей</summary>
  public GraphsSet Set
  {
    get
    {
      GraphsSet set = GraphsSet.Clone(this._originalSet);
      set.Clear();
      foreach (TreeListNode node in this._tree.Nodes)
      {
        string key = (string) node.GetValue((object) 0);
        if (set.Contains(key))
          throw new Exception(string.Format(LocalizationHolder.rm.GetString(sc_18405.ssp_signs_18406()), (object) (string) node.GetValue((object) 0)));
        GraphsCollection tag = node.Tag as GraphsCollection;
        set.Add(key, tag);
      }
      return set;
    }
    set
    {
      this._originalSet = value;
      this._tree.BeginUpdate();
      try
      {
        this._tree.Nodes.Clear();
        this.CurrentObject = (TreeListNode) null;
        this._tree.Tag = (object) value;
        if (value != null)
        {
          foreach (string key in value)
          {
            GraphsCollection graphsCollection = value[key];
            if (graphsCollection != null)
            {
              TreeListNode parentNode = this._tree.AppendNode((object) new object[1]
              {
                (object) key
              }, (TreeListNode) null);
              parentNode.SetValue((object) 1, (object) true);
              parentNode.Tag = (object) graphsCollection;
              bool flag = true;
              foreach (GraphClass graphClass in graphsCollection)
              {
                if (SignsCache.PossibleGraphs.ContainsKey(graphClass.Value))
                {
                  TreeListNode treeListNode = this._tree.AppendNode((object) new object[1]
                  {
                    (object) SignsCache.PossibleGraphs[graphClass.Value]
                  }, parentNode);
                  treeListNode.SetValue((object) 1, (object) graphClass.StrongCheck);
                  treeListNode.Tag = (object) graphClass;
                  if (flag && !graphClass.StrongCheck)
                    parentNode[(object) 1] = (object) (flag = false);
                }
              }
            }
          }
        }
        this._tree.FocusedColumn = this.treeListColumnStrikt;
      }
      finally
      {
        this._tree.EndUpdate();
      }
      this._tool_Enabled();
    }
  }

  /// <summary>Оригинальный набор данных (не измененный)</summary>
  public GraphsSet OriginalSet => this._originalSet;

  private void InitMenu()
  {
    this._menu = (SignsHolder.Bar as BarManager).MenuBar.AddMenuBar("Intermech.Signs.SignControl");
    this._menu.Visible = false;
    int count = this._menu.ImageList.Images.Count;
    foreach (Image image in this.imageListSign.Images)
      this._menu.ImageList.Images.Add(image);
    this._menu.BeforePopup += new MenuItemBase.BeforePopupEventHandler(this._menu_BeforePopup);
    this._AddGroup = new MenuButtonItem(LocalizationHolder.rm.GetString("Signs_4"), new EventHandler(this._menu_Click), count);
    this._AddItem = new MenuButtonItem(LocalizationHolder.rm.GetString(sc_18405.ssp_signs_18407()), new EventHandler(this._menu_Click), count + 1);
    this._RenameGroup = new MenuButtonItem(LocalizationHolder.rm.GetString("Signs_6"), new EventHandler(this._menu_Click), count + 2);
    this._RenameGroup.BeginGroup = true;
    this._Delete = new MenuButtonItem(LocalizationHolder.rm.GetString("Signs_7"), new EventHandler(this._menu_Click), count + 3);
    this._ClearGroup = new MenuButtonItem(LocalizationHolder.rm.GetString("Signs_8"), new EventHandler(this._menu_Click), count + 4);
    this._ClearGroup.BeginGroup = true;
    this._ClearAll = new MenuButtonItem(LocalizationHolder.rm.GetString(sc_18405.ssp_signs_18408()), new EventHandler(this._menu_Click), count + 5);
    this._menu.Items.AddRange((ToolbarItemBase[]) new MenuButtonItem[6]
    {
      this._AddGroup,
      this._AddItem,
      this._RenameGroup,
      this._Delete,
      this._ClearGroup,
      this._ClearAll
    });
  }

  private void _tool_Enabled()
  {
    this.buttonItemAddGroup.Visible = true;
    this.buttonItemAddGraph.Visible = false;
    this.buttonItemEdit.Visible = false;
    this.buttonItemDelete.Visible = true;
    this.buttonItemClearGroup.Visible = false;
    this.buttonItemClearList.Visible = true;
    if (this.CurrentObject != null)
    {
      if (this.CurrentObject.Tag is GraphsCollection)
      {
        this.buttonItemAddGraph.Visible = true;
        this.buttonItemEdit.Visible = !this.ReadOnly;
        this.buttonItemDelete.Enabled = true;
        this.buttonItemClearGroup.Visible = true;
        this.buttonItemClearList.Enabled = true;
      }
      if (!(this.CurrentObject.Tag is GraphClass))
        return;
      this.buttonItemAddGraph.Visible = true;
      this.buttonItemDelete.Enabled = true;
      this.buttonItemClearGroup.Visible = true;
      this.buttonItemClearList.Enabled = true;
    }
    else
    {
      this.buttonItemDelete.Enabled = false;
      this.buttonItemClearList.Enabled = this._tree.Nodes.Count > 0;
    }
  }

  private void _menu_BeforePopup(object sender, MenuPopupEventArgs e)
  {
    this._AddGroup.Visible = true;
    this._AddItem.Visible = false;
    this._RenameGroup.Visible = false;
    this._Delete.Visible = true;
    this._ClearGroup.Visible = false;
    this._ClearAll.Visible = true;
    if (this.CurrentObject != null)
    {
      if (this.CurrentObject.Tag is GraphsCollection)
      {
        this._AddItem.Visible = true;
        this._RenameGroup.Visible = !this.ReadOnly;
        this._Delete.Enabled = true;
        this._ClearGroup.Visible = true;
        this._ClearAll.Enabled = true;
      }
      if (!(this.CurrentObject.Tag is GraphClass))
        return;
      this._AddItem.Visible = true;
      this._Delete.Enabled = true;
      this._ClearGroup.Visible = true;
      this._ClearAll.Enabled = true;
    }
    else
    {
      this._Delete.Enabled = false;
      this._ClearAll.Enabled = this._tree.Nodes.Count > 0;
    }
  }

  private void _menu_Click(object sender, EventArgs e)
  {
    string str1 = LocalizationHolder.rm.GetString(sc_18405.ssp_signs_18409());
    this._tree.BeginUpdate();
    try
    {
      if (sender.Equals((object) this._AddGroup) || sender.Equals((object) this.buttonItemAddGroup))
      {
        int num1 = 1;
        foreach (TreeListNode node in this._tree.Nodes)
        {
          string str2 = (string) node.GetValue((object) 0);
          if (str2.StartsWith(str1))
          {
            string str3 = str2.Remove(0, str1.Length).TrimStart(' ');
            try
            {
              int num2 = Convert.ToInt32(str3) + 1;
              num1 = num2 > num1 ? num2 : num1;
            }
            catch
            {
            }
          }
        }
        TreeListNode node1 = this._tree.AppendNode((object) new object[1]
        {
          (object) $"{str1} {num1.ToString()}"
        }, (TreeListNode) null);
        node1.SetValue((object) 1, (object) false);
        node1.Tag = (object) new GraphsCollection();
        this._tree.FocusedNode = node1;
        this._menu_Click((object) this._AddItem, (EventArgs) null);
        if (!this._tree.FocusedNode.HasChildren)
          this._tree.Nodes.Remove(node1);
        if (this._tree.Nodes.Count != 0)
          this._tree.SetFocusedNode(this._tree.Nodes[0]);
      }
      else if (sender.Equals((object) this._AddItem) || sender.Equals((object) this.buttonItemAddGraph))
      {
        if (this.CurrentObject.Tag is GraphClass)
        {
          this.CurrentObject = this.CurrentObject.ParentNode;
          this._tree.FocusedNode = this.CurrentObject;
        }
        GraphsCollection tag = this.CurrentObject.Tag as GraphsCollection;
        using (SelectGraphs selectGraphs = new SelectGraphs())
        {
          if (selectGraphs.ShowDialog().Equals((object) DialogResult.OK))
          {
            if (selectGraphs.SelectedList.Count > 0)
            {
              foreach (string selected in (IEnumerable) selectGraphs.SelectedList)
              {
                if (!tag.Contains(selected))
                {
                  GraphClass graphClass = new GraphClass(selected);
                  tag.Add(graphClass);
                  if (SignsCache.PossibleGraphs.ContainsKey(graphClass.Value))
                  {
                    TreeListNode treeListNode = this._tree.AppendNode((object) new object[1]
                    {
                      (object) SignsCache.PossibleGraphs[graphClass.Value].ToString()
                    }, this.CurrentObject);
                    if (!(treeListNode.Tag is GraphsCollection))
                    {
                      if (treeListNode.ParentNode != null && (bool) treeListNode.ParentNode.GetValue((object) 1))
                      {
                        treeListNode.SetValue((object) 1, (object) true);
                        graphClass.StrongCheck = true;
                      }
                      treeListNode.Tag = (object) graphClass;
                    }
                    this.Modified = true;
                  }
                }
              }
            }
          }
        }
      }
      else if (sender.Equals((object) this._Delete) || sender.Equals((object) this.buttonItemDelete))
      {
        if (MessageBox.Show(string.Format(LocalizationHolder.rm.GetString(sc_18405.ssp_signs_18410()), this.CurrentObject.GetValue((object) 0)), LocalizationHolder.rm.GetString("Signs_12"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals((object) DialogResult.Yes))
        {
          if (this.CurrentObject.Tag is GraphsCollection)
          {
            this._tree.Nodes.Remove(this.CurrentObject);
            this.Modified = true;
          }
          else if (this.CurrentObject.Tag is GraphClass)
          {
            TreeListNode parentNode = this.CurrentObject.ParentNode;
            bool flag = false;
            if (parentNode.Nodes.Count == 1)
            {
              DialogResult dialogResult = MessageBox.Show("Производится удаление последней графы в группе подписей.\nУдалять также группу?\n\nПримечание: настройка групп без граф не рекомендуется.", "Внимание", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
              if (dialogResult == DialogResult.Cancel)
                return;
              flag = dialogResult == DialogResult.Yes;
            }
            (this.CurrentObject.ParentNode.Tag as GraphsCollection).Remove(this.CurrentObject.Tag as GraphClass);
            if (flag)
              this._tree.Nodes.Remove(parentNode);
            else
              this._tree.Nodes.Remove(this.CurrentObject);
            this.Modified = true;
          }
        }
      }
      else if (sender.Equals((object) this._ClearGroup) || sender.Equals((object) this.buttonItemClearGroup))
      {
        if (this.CurrentObject.Tag is GraphClass)
        {
          this.CurrentObject = this.CurrentObject.ParentNode;
          this._tree.FocusedNode = this.CurrentObject;
        }
        if (MessageBox.Show(string.Format(LocalizationHolder.rm.GetString(sc_18405.ssp_signs_18411()), this.CurrentObject.GetValue((object) 0)), LocalizationHolder.rm.GetString("Signs_14"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals((object) DialogResult.Yes))
        {
          (this.CurrentObject.Tag as GraphsCollection).Clear();
          this.CurrentObject.Nodes.Clear();
          this.Modified = true;
        }
      }
      else
      {
        if (!sender.Equals((object) this._ClearAll))
        {
          if (!sender.Equals((object) this.buttonItemClearList))
            goto label_62;
        }
        if (MessageBox.Show(string.Format(LocalizationHolder.rm.GetString(sc_18405.ssp_signs_18412())), LocalizationHolder.rm.GetString("Signs_16"), MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals((object) DialogResult.Yes))
        {
          this._tree.Nodes.Clear();
          this.CurrentObject = (TreeListNode) null;
          this.Modified = true;
        }
      }
    }
    finally
    {
      this._tree.EndUpdate();
    }
label_62:
    if (!sender.Equals((object) this._RenameGroup) && !sender.Equals((object) this.buttonItemEdit))
      return;
    this.isEdit = true;
    if (this._tree.FocusedColumn.AbsoluteIndex == 1)
    {
      this.treeListColumnSign.Options |= ColumnOptions.CanFocused;
      this._tree.FocusedColumn = this.treeListColumnSign;
    }
    this.treeListColumnSign.Options &= ~ColumnOptions.ReadOnly;
    this._tree.ShowEditor();
    this.Modified = true;
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SignControl));
    this._tree = new TreeList();
    this.treeListColumnSign = new TreeListColumn();
    this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
    this.treeListColumnStrikt = new TreeListColumn();
    this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
    this.imageListSign = new ImageList(this.components);
    this.toolBarSign = new Intermech.Bars.ToolBar();
    this.buttonItemAddGroup = new ButtonItem();
    this.buttonItemAddGraph = new ButtonItem();
    this.buttonItemEdit = new ButtonItem();
    this.buttonItemDelete = new ButtonItem();
    this.buttonItemClearGroup = new ButtonItem();
    this.buttonItemClearList = new ButtonItem();
    this.panel1 = new Panel();
    this._cbII = new CheckBox();
    this._tree.BeginInit();
    this.repositoryItemTextEdit1.BeginInit();
    this.repositoryItemCheckEdit1.BeginInit();
    this.panel1.SuspendLayout();
    this.SuspendLayout();
    this._tree.Columns.AddRange(new TreeListColumn[2]
    {
      this.treeListColumnSign,
      this.treeListColumnStrikt
    });
    componentResourceManager.ApplyResources((object) this._tree, "_tree");
    this._tree.Name = "_tree";
    this._tree.RepositoryItems.AddRange(new RepositoryItem[2]
    {
      (RepositoryItem) this.repositoryItemTextEdit1,
      (RepositoryItem) this.repositoryItemCheckEdit1
    });
    this._tree.FocusedColumnChanged += new FocusedColumnChangedEventHandler(this._tree_FocusedColumnChanged);
    this._tree.ShownEditor += new EventHandler(this._tree_ShownEditor);
    this._tree.FocusedNodeChanged += new FocusedNodeChangedEventHandler(this._tree_FocusedNodeChanged);
    this._tree.MouseUp += new MouseEventHandler(this._tree_MouseUp);
    componentResourceManager.ApplyResources((object) this.treeListColumnSign, "treeListColumnSign");
    this.treeListColumnSign.ColumnEdit = (RepositoryItem) this.repositoryItemTextEdit1;
    this.treeListColumnSign.Name = "treeListColumnSign";
    this.repositoryItemTextEdit1.AutoHeight = false;
    this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
    this.repositoryItemTextEdit1.Validating += new CancelEventHandler(this.repositoryItemTextEdit1_Validating);
    componentResourceManager.ApplyResources((object) this.treeListColumnStrikt, "treeListColumnStrikt");
    this.treeListColumnStrikt.ColumnEdit = (RepositoryItem) this.repositoryItemCheckEdit1;
    this.treeListColumnStrikt.Name = "treeListColumnStrikt";
    this.repositoryItemCheckEdit1.AutoHeight = false;
    this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
    this.repositoryItemCheckEdit1.NullStyle = StyleIndeterminate.Unchecked;
    this.repositoryItemCheckEdit1.CheckedChanged += new EventHandler(this.repositoryItemCheckEdit1_CheckedChanged);
    this.imageListSign.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageListSign.ImageStream");
    this.imageListSign.TransparentColor = Color.Transparent;
    this.imageListSign.Images.SetKeyName(0, "add_gr.ico");
    this.imageListSign.Images.SetKeyName(1, "add_line.ico");
    this.imageListSign.Images.SetKeyName(2, "edit_gr.ico");
    this.imageListSign.Images.SetKeyName(3, "delete.ico");
    this.imageListSign.Images.SetKeyName(4, "clear_gr.ico");
    this.imageListSign.Images.SetKeyName(5, "clear_all.ico");
    this.toolBarSign.FullMenus = true;
    this.toolBarSign.Guid = new Guid("e48b7452-4f60-4309-8e9d-9b28efb59fb0");
    this.toolBarSign.Hidden = false;
    this.toolBarSign.ImageList = this.imageListSign;
    this.toolBarSign.Items.AddRange(new ToolbarItemBase[6]
    {
      (ToolbarItemBase) this.buttonItemAddGroup,
      (ToolbarItemBase) this.buttonItemAddGraph,
      (ToolbarItemBase) this.buttonItemEdit,
      (ToolbarItemBase) this.buttonItemDelete,
      (ToolbarItemBase) this.buttonItemClearGroup,
      (ToolbarItemBase) this.buttonItemClearList
    });
    componentResourceManager.ApplyResources((object) this.toolBarSign, "toolBarSign");
    this.toolBarSign.Name = "toolBarSign";
    this.buttonItemAddGroup.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.buttonItemAddGroup, "buttonItemAddGroup");
    this.buttonItemAddGroup.ImageIndex = 0;
    this.buttonItemAddGroup.Click += new EventHandler(this._menu_Click);
    componentResourceManager.ApplyResources((object) this.buttonItemAddGraph, "buttonItemAddGraph");
    this.buttonItemAddGraph.ImageIndex = 1;
    this.buttonItemAddGraph.Click += new EventHandler(this._menu_Click);
    this.buttonItemEdit.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.buttonItemEdit, "buttonItemEdit");
    this.buttonItemEdit.ImageIndex = 2;
    this.buttonItemEdit.Click += new EventHandler(this._menu_Click);
    this.buttonItemDelete.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.buttonItemDelete, "buttonItemDelete");
    this.buttonItemDelete.ImageIndex = 3;
    this.buttonItemDelete.Click += new EventHandler(this._menu_Click);
    componentResourceManager.ApplyResources((object) this.buttonItemClearGroup, "buttonItemClearGroup");
    this.buttonItemClearGroup.ImageIndex = 4;
    this.buttonItemClearGroup.Click += new EventHandler(this._menu_Click);
    componentResourceManager.ApplyResources((object) this.buttonItemClearList, "buttonItemClearList");
    this.buttonItemClearList.ImageIndex = 5;
    this.buttonItemClearList.Click += new EventHandler(this._menu_Click);
    this.panel1.Controls.Add((Control) this._cbII);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this._cbII, "_cbII");
    this._cbII.Name = "_cbII";
    this._cbII.CheckedChanged += new EventHandler(this.repositoryItemCheckEdit1_CheckedChanged);
    this.Controls.Add((Control) this._tree);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.toolBarSign);
    this.Name = nameof (SignControl);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.Tag = (object) "  ";
    this._tree.EndInit();
    this.repositoryItemTextEdit1.EndInit();
    this.repositoryItemCheckEdit1.EndInit();
    this.panel1.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  private void _tree_MouseUp(object sender, MouseEventArgs e)
  {
    if (!e.Button.Equals((object) MouseButtons.Right))
      return;
    this.CurrentObject = this._tree.FocusedNode;
    if (this._readonly)
      return;
    this._menu.Show(sender as Control, new Point(e.X, e.Y));
  }

  private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
  {
    if (this._isLayout || this.CurrentObject == null)
      return;
    CheckEdit checkEdit = sender as CheckEdit;
    if (this.CurrentObject.Tag is GraphClass)
    {
      if (sender.Equals((object) this._cbII))
      {
        (this.CurrentObject.Tag as GraphClass).II = this._cbII.Checked;
        this.Modified = true;
      }
      else
      {
        if (this.CurrentObject.Tag is GraphClass tag)
        {
          tag.StrongCheck = checkEdit.Checked;
          if (!checkEdit.Checked)
          {
            TreeListNode parentNode = this.CurrentObject.ParentNode;
            if (parentNode != null && Convert.ToBoolean(parentNode[(object) 1]))
              parentNode[(object) 1] = (object) false;
          }
        }
        this.Modified = true;
      }
    }
    else if (!sender.Equals((object) this._cbII))
    {
      for (int index = 0; index < this._tree.FocusedNode.Nodes.Count; ++index)
        this._tree.FocusedNode.Nodes[index].SetValue((object) 1, (object) checkEdit.Checked);
      foreach (GraphClass graphClass in this._tree.FocusedNode.Tag as GraphsCollection)
        graphClass.StrongCheck = checkEdit.Checked;
      this.Modified = true;
    }
    if (Convert.ToBoolean(this.CurrentObject[(object) 1]) == checkEdit.Checked)
      return;
    this.CurrentObject[(object) 1] = (object) checkEdit.Checked;
    this._tree.EndCurrentEdit();
  }

  private void _tree_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
  {
    this.CurrentObject = this._tree.FocusedNode;
    this.treeListColumnSign.Options |= ColumnOptions.ReadOnly;
    this.treeListColumnSign.Options &= ~ColumnOptions.CanFocused;
    this._tool_Enabled();
  }

  private void _tree_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
  {
    if (!this.isEdit)
    {
      this.treeListColumnSign.Options |= ColumnOptions.ReadOnly;
      this.treeListColumnSign.Options &= ~ColumnOptions.CanFocused;
    }
    else
      this.isEdit = false;
  }

  private void repositoryItemTextEdit1_Validating(object sender, CancelEventArgs e)
  {
    BaseEdit baseEdit = sender as BaseEdit;
    if (!(baseEdit.Text == string.Empty))
      return;
    baseEdit.Text = "Группа граф для подписи";
  }

  private void _tree_ShownEditor(object sender, EventArgs e) => this._tree.ActiveEditor.SelectAll();
}
