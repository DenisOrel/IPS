// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SearchScheme.SearchSchemeEditor
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using DevExpress.IM.Utils;
using DevExpress.IM.XtraEditors.Controls;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.PropertyEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.SearchScheme;

internal class SearchSchemeEditor : Form
{
  public static SchemeEditorParams EditorParams;
  private int _parentMode;
  public int EditorMode;
  public long SchemeID;
  public bool IsChanged;
  private TreeNode _tlnObjType;
  private TreeNode _typesToDisableExpand;
  private TreeNode _typesToExpand;
  private TreeNode _tlnRelType;
  private TreeNode _tlnAttributes;
  private TreeNode _tlnRoles;
  public int ObjectTypeID = -1;
  private int _rolesTypeID = -1;
  private SearchSheme _searchScheme = new SearchSheme();
  private List<ColumnSchemeAttProxy> _oldViewColumns;
  internal bool IsChangedColumns;
  private string _currentRuleString = "";
  private IContainer components;
  private Panel panel1;
  private Panel panel2;
  private Button btnCancel;
  private Button btnApply;
  private Panel panel3;
  private Label label1;
  private ComboBox eSearchDirection;
  private Label label3;
  private Label label2;
  private Intermech.Bars.ToolBar toolBarValues;
  private ButtonItem btnAddValue;
  private ButtonItem btnDeleteValue;
  private ImageList imagesToolabarImpact;
  private ButtonItem btnEditValue;
  private TextBox eName;
  private TreeView treeList1;
  private TextBox eSelection;
  private Button bSelectionOpen;
  private Button bSelectionAdd;
  private ContextMenuStrip contextMenuStripAdd;
  private ToolStripMenuItem tsmiAdd;
  private ToolStripMenuItem tsmiEdit;
  private ToolStripMenuItem tsmiDelete;
  private ToolTipController toolTipController1;
  private Button bSelectionView;
  private CheckBox cbGrouping;
  private CheckBox cbInSelectProd;
  private ButtonItem biAttibuteUp;
  private ButtonItem biAttibuteDown;
  private ToolStripSeparator tsmiSeparator;
  private ToolStripMenuItem tsmiUp;
  private ToolStripMenuItem tsmiDown;
  private Label label4;
  private Button bVersionRuleOpen;
  private TextBox eVersionRule;
  private CheckBox cbActual;

  public int ParentMode
  {
    get => this._parentMode;
    set
    {
      if (value == 1)
      {
        this.btnApply.Text = LocalizationHolder.rm.GetString("Pdm_64");
        this.Text = LocalizationHolder.rm.GetString("Pdm_65");
      }
      this._parentMode = value;
    }
  }

  public int RolesRelationType
  {
    get
    {
      if (this._rolesTypeID == -1)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
          this._rolesTypeID = sessionKeeper.Session.IdentHelper.RolesTypeID;
      }
      return this._rolesTypeID;
    }
  }

  public SearchSchemeEditor()
  {
    this.InitializeComponent();
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1086);
    this.Text = SearchShemeConsts.SearchSchemeEditorName;
    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
    this.Size = new Size(workingArea.Width / 100 * 80 /*0x50*/, workingArea.Height / 100 * 70);
    this.Location = new Point((workingArea.Width - this.Size.Width) / 2, (workingArea.Height - this.Size.Height) / 2);
    this.RuntimeFillControls();
    this.UpdateControls();
  }

  public static long Execute(int ObjectTypeID, long TemplateObjectID)
  {
    if (ObjectTypeID == 0)
      return 0;
    using (SearchSchemeEditor searchSchemeEditor = new SearchSchemeEditor())
    {
      searchSchemeEditor.SchemeID = TemplateObjectID >= 0L ? TemplateObjectID : 0L;
      searchSchemeEditor.ParentMode = 1;
      searchSchemeEditor.ObjectTypeID = ObjectTypeID;
      searchSchemeEditor.LoadObjectData(0);
      searchSchemeEditor.SchemeID = 0L;
      searchSchemeEditor._searchScheme.SchemeID = 0L;
      return searchSchemeEditor.ExecuteForm();
    }
  }

  private long ExecuteForm()
  {
    this.DialogResult = DialogResult.None;
    int num = (int) this.ShowDialog();
    return this.DialogResult != DialogResult.OK ? 0L : this.SchemeID;
  }

  public void SetParent(Control aParent)
  {
    if (aParent == null)
    {
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.TopLevel = true;
      this.Dock = DockStyle.None;
      this.FormBorderStyle = FormBorderStyle.Sizable;
      this.Visible = false;
    }
    else
    {
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.TopLevel = false;
      this.Dock = DockStyle.Fill;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Visible = true;
    }
    this.Parent = aParent;
  }

  private void RuntimeFillControls()
  {
    this.eSearchDirection.Items.Clear();
    bool flag = false;
    if (SearchSchemeEditor.EditorParams != null)
    {
      if (SearchSchemeEditor.EditorParams.Mode == ContainsMode.Applicability)
      {
        this.eSearchDirection.Items.Add((object) new SearchSchemeEditor.SearchDirectionItem(SearchDirection.EntersTo));
        this.eSearchDirection.Items.Add((object) new SearchSchemeEditor.SearchDirectionItem(SearchDirection.RecursiveEntersTo));
        flag = true;
      }
      else if (SearchSchemeEditor.EditorParams.Mode == ContainsMode.Contains)
      {
        this.eSearchDirection.Items.Add((object) new SearchSchemeEditor.SearchDirectionItem(SearchDirection.Contains));
        this.eSearchDirection.Items.Add((object) new SearchSchemeEditor.SearchDirectionItem(SearchDirection.RecursiveContains));
        flag = true;
      }
    }
    if (!flag)
    {
      foreach (int direction in Enum.GetValues(typeof (SearchDirection)))
        this.eSearchDirection.Items.Add((object) new SearchSchemeEditor.SearchDirectionItem((SearchDirection) direction));
    }
    this.eSearchDirection.SelectedIndexChanged -= new EventHandler(this.eSearchDirection_SelectedIndexChanged);
    try
    {
      if (this.eSearchDirection.Items.Count > 0)
        this.eSearchDirection.SelectedIndex = 0;
    }
    finally
    {
      this.eSearchDirection.SelectedIndexChanged += new EventHandler(this.eSearchDirection_SelectedIndexChanged);
    }
    this.treeList1.ImageList = Statics.IconSrv?.ImageList;
    this.eName.MaxLength = (int) MetaDataHelper.GetAttributeType(new Guid("cad00047-306c-11d8-b4e9-00304f19f545")).SizeType;
    this.SetTreeListFirst();
  }

  public void UpdateControls()
  {
    this.btnAddValue.Enabled = this.btnEditValue.Enabled = this.btnDeleteValue.Enabled = this.tsmiAdd.Enabled = this.tsmiEdit.Enabled = this.tsmiDelete.Enabled = this.tsmiUp.Visible = this.tsmiDown.Visible = this.tsmiSeparator.Visible = this.biAttibuteDown.Enabled = this.biAttibuteUp.Enabled = false;
    if (this.treeList1.SelectedNode != null && this.treeList1.SelectedNode.Tag is SearchSchemeEditor.NodeObject tag1)
    {
      bool flag = this.treeList1.SelectedNode == this._tlnRoles && this._searchScheme.IsPersonal;
      this.btnAddValue.Enabled = this.tsmiAdd.Enabled = !flag;
      if (tag1.TypeID != -1)
        this.btnEditValue.Enabled = this.tsmiEdit.Enabled = this.tsmiDelete.Enabled = this.btnDeleteValue.Enabled = !flag;
      if (tag1.Tag is ColumnSchemeAttProxy)
      {
        object tag = tag1.Tag;
        if (this.treeList1.SelectedNode.Index > 0)
          this.tsmiSeparator.Visible = this.tsmiUp.Visible = this.biAttibuteUp.Enabled = true;
        if (this.treeList1.SelectedNode.Parent.Nodes.Count - 1 > this.treeList1.SelectedNode.Index)
          this.tsmiSeparator.Visible = this.tsmiDown.Visible = this.biAttibuteDown.Enabled = true;
      }
    }
    if (this.ParentMode == 1)
    {
      this.btnApply.Enabled = this.btnCancel.Enabled = true;
    }
    else
    {
      this.btnApply.Enabled = this.IsChanged;
      if (this.ParentMode == 0)
        this.btnCancel.Enabled = true;
      else
        this.btnCancel.Enabled = this.IsChanged;
    }
    this.bSelectionView.Enabled = this._searchScheme.SelectionID != -1L;
  }

  public void LoadObjectData(int AEditorMode)
  {
    this._searchScheme = new SearchSheme()
    {
      Direction = (this.eSearchDirection.SelectedItem as SearchSchemeEditor.SearchDirectionItem).Direction
    };
    this.EditorMode = AEditorMode;
    if (this.EditorMode < 0)
      this.EditorMode = 1;
    if (this.EditorMode >= SearchShemeConsts.Headers.Length)
      this.EditorMode = 1;
    this.IsChanged = false;
    if (this.SchemeID == 0L)
    {
      this._searchScheme.IsPersonal = this.ObjectTypeID == MetaDataHelper.GetObjectTypeID("cad0012b-306c-11d8-b4e9-00304f19f545");
    }
    else
    {
      this.ClearControls();
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        this._searchScheme.LoadFromObject(sessionKeeper.Session, this.SchemeID);
        this._rolesTypeID = sessionKeeper.Session.IdentHelper.RolesTypeID;
        this.FillControls(sessionKeeper.Session, this._searchScheme);
      }
      this.IsChanged = false;
      this.UpdateControls();
    }
  }

  public void SaveObjectData()
  {
    bool flag = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this.SchemeID == 0L)
      {
        IDBObject dbObject = sessionKeeper.Session.GetObjectCollection(this.ObjectTypeID).Create();
        flag = true;
        this._searchScheme.SchemeID = dbObject.ObjectID;
        this._searchScheme.SaveToObject(sessionKeeper.Session);
        dbObject.CommitCreation(true);
        this._searchScheme.SchemeID = this.SchemeID = dbObject.ObjectID;
      }
      else
      {
        this._searchScheme.SaveToObject(sessionKeeper.Session);
        this.IsChanged = false;
      }
    }
    if (ServicesManager.GetService(typeof (INotificationService)) is INotificationService service)
    {
      DBObjectsEventArgs e = flag ? new DBObjectsEventArgs("ObjectsCreated", this.SchemeID) : new DBObjectsEventArgs("ObjectsChanged", this.SchemeID);
      service.FireEvent((object) null, (NotificationEventArgs) e);
    }
    this.UpdateControls();
  }

  private void FillControls(IUserSession session, SearchSheme scheme)
  {
    if (scheme == null)
    {
      this.ClearControls();
    }
    else
    {
      this.eName.Text = scheme.Name;
      this.eSelection.Text = scheme.SelectionName;
      this.cbGrouping.Checked = (scheme.Options & SearchOptions.ObjectGrouping) == SearchOptions.ObjectGrouping;
      this.cbInSelectProd.Checked = (scheme.Options & SearchOptions.InSelectionProd) == SearchOptions.InSelectionProd;
      this.cbActual.Checked = (scheme.Options & SearchOptions.ActualSubstitutesOnly) == SearchOptions.ActualSubstitutesOnly;
      for (int index = 0; index < this.eSearchDirection.Items.Count; ++index)
      {
        if ((this.eSearchDirection.Items[index] as SearchSchemeEditor.SearchDirectionItem).Direction == scheme.Direction)
        {
          this.eSearchDirection.SelectedIndexChanged -= new EventHandler(this.eSearchDirection_SelectedIndexChanged);
          try
          {
            this.eSearchDirection.SelectedIndex = index;
            break;
          }
          finally
          {
            this.eSearchDirection.SelectedIndexChanged += new EventHandler(this.eSearchDirection_SelectedIndexChanged);
          }
        }
      }
      if (scheme.VersionRule == Guid.Empty)
      {
        this.eVersionRule.Text = this._currentRuleString;
      }
      else
      {
        bool flag = false;
        if (session.GetCustomService(typeof (IVersionRulesCacheService)) is IVersionRulesCacheService customService && customService.Count > 0)
        {
          for (int Index = 0; Index < customService.Count; ++Index)
          {
            VersionsRule versionsRule = customService[Index];
            if (new Guid(versionsRule.RuleObjectGuid) == scheme.VersionRule)
            {
              this.eVersionRule.Text = versionsRule.RuleObjectCaption;
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          this.eVersionRule.Text = scheme.VersionRule.ToString();
      }
      this.FillTreeList(scheme);
    }
  }

  private void AddNodesFromCollection(
    TreeNode parentNode,
    List<GlobalType> collection,
    int category)
  {
    foreach (GlobalType tag in collection)
      this.AddNode(parentNode, tag.TypeName, (object) new SearchSchemeEditor.NodeObject(category, tag.TypeID, (object) tag), category, tag.TypeID);
  }

  private void FillTreeList(SearchSheme scheme)
  {
    this.treeList1.BeginUpdate();
    this.AddNodesFromCollection(this._tlnObjType, scheme.ObjectTypes, 4);
    this.AddNodesFromCollection(this._typesToExpand, scheme.TypesToExpand, 4);
    this.AddNodesFromCollection(this._typesToDisableExpand, scheme.TypesToDisableExpand, 4);
    this.AddNodesFromCollection(this._tlnRelType, scheme.RelationTypes, 6);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (ColumnSchemeAttProxy viewColumn in scheme.ViewColumns)
      {
        IDBAttributeType attributeType = sessionKeeper.Session.GetAttributeType(viewColumn.AttributeGuid, false);
        int attributeId = attributeType != null ? attributeType.AttributeID : 0;
        this.AddNode(this._tlnAttributes, viewColumn.ToString(), (object) new SearchSchemeEditor.NodeObject(3, attributeId, (object) viewColumn), 3, -1, (object) (FieldTypes) (attributeType != null ? (int) attributeType.AttributeType : 0));
      }
    }
    this._oldViewColumns = new List<ColumnSchemeAttProxy>(scheme.ViewColumns.Count);
    for (int index = 0; index < scheme.ViewColumns.Count; ++index)
      this._oldViewColumns.Add(new ColumnSchemeAttProxy(scheme.ViewColumns[index].AttributeGuid, scheme.ViewColumns[index].AttributeSource, scheme.ViewColumns[index].ColumnWidth));
    foreach (SearchSchemeRole role in scheme.Roles)
      this.AddNode(this._tlnRoles, role.ToString(), (object) new SearchSchemeEditor.NodeObject(2, this.RolesRelationType, (object) role), 4, this.RolesRelationType);
    this.treeList1.EndUpdate();
    this._tlnObjType.Expand();
    this._typesToExpand.Expand();
    this._typesToDisableExpand.Expand();
    this._tlnRelType.Expand();
    this._tlnAttributes.Expand();
    this._tlnRoles.Expand();
  }

  private TreeNode CreateRootNode(string text, int category)
  {
    return this.AddNode((TreeNode) null, text, (object) new SearchSchemeEditor.NodeObject(category, -1, (object) null), category, 0);
  }

  private void SetTreeListFirst()
  {
    this.treeList1.BeginUpdate();
    this.treeList1.Nodes.Clear();
    this._tlnObjType = this.CreateRootNode(SearchShemeConsts.SearchObjectTypes, 4);
    this._typesToExpand = this.CreateRootNode(SearchShemeConsts.TypesToExpand, 4);
    this._typesToDisableExpand = this.CreateRootNode(SearchShemeConsts.TypesToDisableExpand, 4);
    this._tlnRelType = this.CreateRootNode(SearchShemeConsts.SearchRelationTypes, 6);
    this._tlnAttributes = this.CreateRootNode(SearchShemeConsts.SearchAddedColumns, 3);
    this._tlnRoles = this.AddNode((TreeNode) null, SearchShemeConsts.SearchRoles, (object) new SearchSchemeEditor.NodeObject(2, -1, (object) null), 4, this.RolesRelationType);
    this.treeList1.EndUpdate();
  }

  private void ClearControls()
  {
    this.eName.Text = string.Empty;
    this.eSelection.Text = string.Empty;
    this.eSearchDirection.SelectedIndex = 0;
    this.SetTreeListFirst();
  }

  private void bSelectionOpen_Click(object sender, EventArgs e)
  {
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("Pdm_66"), string.Empty, ObjectTypesHelper.GetObjTypeID("cad00156-306c-11d8-b4e9-00304f19f545"), SelectionOptions.Default);
    if (numArray == null || this._searchScheme.SelectionID == numArray[0])
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(numArray[0], false);
      if (dbObject == null)
        return;
      this._searchScheme.SelectionID = dbObject.ObjectID;
      this._searchScheme.SelectionName = dbObject.Caption != string.Empty ? dbObject.Caption : $"<{dbObject.ObjectID}>";
      this.eSelection.Text = this._searchScheme.SelectionName;
      this.IsChanged = true;
      this.UpdateControls();
      this.CheckColumnsAfterAddSelection(sessionKeeper.Session);
    }
  }

  private void eSearchDirection_SelectedIndexChanged(object sender, EventArgs e)
  {
    this._searchScheme.Direction = (this.eSearchDirection.SelectedItem as SearchSchemeEditor.SearchDirectionItem).Direction;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void eName_TextChanged(object sender, EventArgs e)
  {
    this._searchScheme.Name = this.eName.Text;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void treeList1_AfterSelect(object sender, TreeViewEventArgs e) => this.UpdateControls();

  private void toolBarValues_ButtonClick(object sender, ToolBarItemEventArgs e)
  {
    this.SetChanges(e.Item.CommandName);
  }

  private void tsmiAdd_Click(object sender, EventArgs e) => this.SetChanges("btnAddValue");

  private void tsmiEdit_Click(object sender, EventArgs e) => this.SetChanges("btnEditValue");

  private void tsmiDelete_Click(object sender, EventArgs e) => this.SetChanges("btnDeleteValue");

  private void SetChanges(string CommandName)
  {
    bool flag = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      switch (CommandName)
      {
        case "btnAddValue":
          flag = this.AddTreeListValue(sessionKeeper.Session);
          break;
        case "btnEditValue":
          flag = this.EditTreeListValue(sessionKeeper.Session);
          break;
        case "btnDeleteValue":
          flag = this.DeleteTreeListValue();
          break;
      }
      if (!flag)
        return;
      this.IsChanged = true;
      this.UpdateControls();
    }
  }

  private bool DeleteTreeListValue()
  {
    bool flag = false;
    if (this.treeList1.SelectedNode != null && this.treeList1.SelectedNode.Tag is SearchSchemeEditor.NodeObject tag && tag.TypeID != -1)
    {
      switch (tag.CategoryID)
      {
        case 2:
          this._searchScheme.Roles.Remove((SearchSchemeRole) tag.Tag);
          this.RefreshTreeList(true);
          flag = true;
          break;
        case 3:
          this._searchScheme.ViewColumns.Remove((ColumnSchemeAttProxy) tag.Tag);
          this.RefreshTreeList(true);
          flag = true;
          this.IsChangedColumns = true;
          break;
        case 4:
          TreeNode parent = this.treeList1.SelectedNode.Parent;
          if (parent == this._tlnObjType)
            this._searchScheme.ObjectTypes.Remove((GlobalType) tag.Tag);
          else if (parent == this._typesToExpand)
            this._searchScheme.TypesToExpand.Remove((GlobalType) tag.Tag);
          else if (parent == this._typesToDisableExpand)
            this._searchScheme.TypesToDisableExpand.Remove((GlobalType) tag.Tag);
          this.RefreshTreeList(true);
          flag = true;
          break;
        case 6:
          this._searchScheme.RelationTypes.Remove((GlobalType) tag.Tag);
          this.RefreshTreeList(true);
          flag = true;
          break;
      }
    }
    return flag;
  }

  private void RefreshTreeList() => this.RefreshTreeList(false);

  private void RefreshTreeList(bool DeleteMode)
  {
    this.treeList1.BeginUpdate();
    if (!DeleteMode)
    {
      if (this.treeList1.SelectedNode.Tag is SearchSchemeEditor.NodeObject tag)
      {
        this.treeList1.SelectedNode.Text = tag.Tag.ToString();
        int num = !(tag.Tag is SearchSchemeRole) ? (Statics.IconSrv != null ? Statics.IconSrv.IndexOf(tag.CategoryID, tag.TypeID) : -1) : (Statics.IconSrv != null ? Statics.IconSrv.IndexOf(4, this.RolesRelationType) : -1);
        this.treeList1.SelectedNode.ImageIndex = num;
        this.treeList1.SelectedNode.SelectedImageIndex = num;
      }
    }
    else
      this.treeList1.Nodes.Remove(this.treeList1.SelectedNode);
    this.treeList1.EndUpdate();
  }

  private ArrayList ShowObjectTypesDialog(
    bool Multiselect,
    List<GlobalType> presentIDs,
    string caption)
  {
    ArrayList arrayList = new ArrayList();
    SelectorForm selectorForm = new SelectorForm(typeof (ObjectTypesFolder), caption, typeof (ObjectTypeFolder), Multiselect);
    if (selectorForm.ShowDialog() == DialogResult.OK && selectorForm.IDList.Count > 0)
    {
      foreach (int id in selectorForm.IDList)
      {
        int objType = id;
        if (!presentIDs.Exists((Predicate<GlobalType>) (x => x.TypeID == objType)))
          arrayList.Add((object) objType);
      }
    }
    return arrayList;
  }

  private long[] ShowRolesDialog()
  {
    return Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("Pdm_67"), LocalizationHolder.rm.GetString("Pdm_68"), this._rolesTypeID, SelectionOptions.Default);
  }

  private ArrayList ShowRelationTypesDialog(bool Multiselect)
  {
    ArrayList arrayList = new ArrayList();
    SelectorForm selectorForm = new SelectorForm(typeof (RelationTypesFolder), SearchShemeConsts.SearchRelationTypes, typeof (RelationTypeFolder), Multiselect);
    if (selectorForm.ShowDialog() == DialogResult.OK && selectorForm.IDList.Count > 0)
    {
      foreach (int id in selectorForm.IDList)
      {
        bool flag = false;
        foreach (GlobalType relationType in this._searchScheme.RelationTypes)
        {
          if (relationType.TypeID == id)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          arrayList.Add((object) id);
      }
    }
    return arrayList;
  }

  private ColumnSchemeAttProxy ShowAttributeDialog(
    Guid attributeGuid,
    AttributeSourceTypes attributeSource,
    int columnWidth)
  {
    List<int> enabledObjTypes = (List<int>) null;
    if (this._searchScheme.ObjectTypes != null && this._searchScheme.ObjectTypes.Count > 0)
    {
      enabledObjTypes = new List<int>(this._searchScheme.ObjectTypes.Count);
      foreach (GlobalType objectType in this._searchScheme.ObjectTypes)
        enabledObjTypes.Add(objectType.TypeID);
    }
    ColumnSchemeAttrEditForm schemeAttrEditForm = new ColumnSchemeAttrEditForm(enabledObjTypes, attributeGuid, columnWidth, attributeSource);
    if (schemeAttrEditForm.ShowDialog() == DialogResult.OK)
    {
      ColumnSchemeAttProxy columnSchemeAttProxy = new ColumnSchemeAttProxy(schemeAttrEditForm.Attribute, schemeAttrEditForm.AttributeSource, schemeAttrEditForm.ColumnWidth);
      bool flag = false;
      foreach (object viewColumn in this._searchScheme.ViewColumns)
      {
        if (viewColumn.Equals((object) columnSchemeAttProxy))
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return columnSchemeAttProxy;
    }
    return (ColumnSchemeAttProxy) null;
  }

  private bool AddTreeListValue(IUserSession session)
  {
    bool flag = false;
    if (this.treeList1.SelectedNode != null && this.treeList1.SelectedNode.Tag is SearchSchemeEditor.NodeObject tag)
    {
      switch (tag.CategoryID)
      {
        case 2:
          long[] numArray = this.ShowRolesDialog();
          if (numArray != null && numArray.Length != 0)
          {
            foreach (long role in numArray)
            {
              if (this.AddRole(role, session))
                flag = true;
            }
            break;
          }
          break;
        case 3:
          ColumnSchemeAttProxy csap = this.ShowAttributeDialog(Guid.Empty, AttributeSourceTypes.Object, 150);
          if (csap != null)
          {
            this.AddAttribute(csap, session);
            flag = true;
            break;
          }
          break;
        case 4:
          TreeNode parent = this.treeList1.SelectedNode.Parent ?? this.treeList1.SelectedNode;
          List<GlobalType> collectionForNode = this.GetCollectionForNode(parent);
          ArrayList arrayList1 = this.ShowObjectTypesDialog(true, collectionForNode, this.GetSelectDialogCaption(parent));
          if (arrayList1 != null && arrayList1.Count > 0)
          {
            foreach (int objType in arrayList1)
              this.AddObjectType(parent, collectionForNode, objType, session);
            flag = true;
            break;
          }
          break;
        case 6:
          ArrayList arrayList2 = this.ShowRelationTypesDialog(true);
          if (arrayList2 != null && arrayList2.Count > 0)
          {
            foreach (int relType in arrayList2)
              this.AddRelationType(relType, session);
            flag = true;
            break;
          }
          break;
      }
    }
    return flag;
  }

  private bool AddRole(long role, IUserSession session)
  {
    SearchSchemeRole tag = new SearchSchemeRole(role, session);
    if (!tag.ValidRole)
      return false;
    foreach (SearchSchemeRole role1 in this._searchScheme.Roles)
    {
      if (tag.RoleID == role1.RoleID)
        return false;
    }
    this._searchScheme.Roles.Add(tag);
    this.AddNode(this._tlnRoles, tag.ToString(), (object) new SearchSchemeEditor.NodeObject(2, this.RolesRelationType, (object) tag), 4, this.RolesRelationType);
    return true;
  }

  private string GetSelectDialogCaption(TreeNode parent)
  {
    if (parent == this._tlnObjType)
      return SearchShemeConsts.SearchObjectTypes;
    if (parent == this._typesToExpand)
      return SearchShemeConsts.TypesToExpand;
    return parent == this._typesToDisableExpand ? SearchShemeConsts.TypesToDisableExpand : string.Empty;
  }

  private List<GlobalType> GetCollectionForNode(TreeNode parent)
  {
    if (parent == this._tlnObjType)
      return this._searchScheme.ObjectTypes;
    if (parent == this._typesToExpand)
      return this._searchScheme.TypesToExpand;
    if (parent == this._typesToDisableExpand)
      return this._searchScheme.TypesToDisableExpand;
    throw new ArgumentOutOfRangeException(nameof (parent), $"Узел {parent.Text} не поддерживается методом!");
  }

  private void AddObjectType(
    TreeNode parent,
    List<GlobalType> collection,
    int objType,
    IUserSession session)
  {
    GlobalType tag = new GlobalType(objType, 4, session);
    collection.Add(tag);
    this.AddNode(parent, tag.ToString(), (object) new SearchSchemeEditor.NodeObject(4, tag.TypeID, (object) tag), 4, tag.TypeID);
  }

  private void AddRelationType(int relType, IUserSession session)
  {
    GlobalType tag = new GlobalType(relType, 6, session);
    this._searchScheme.RelationTypes.Add(tag);
    this.AddNode(this._tlnRelType, tag.ToString(), (object) new SearchSchemeEditor.NodeObject(6, tag.TypeID, (object) tag), 6, tag.TypeID);
  }

  private void AddAttribute(ColumnSchemeAttProxy csap, IUserSession session)
  {
    this._searchScheme.ViewColumns.Add(csap);
    IDBAttributeType attributeType = session.GetAttributeType(csap.AttributeGuid, false);
    int attributeId = attributeType != null ? attributeType.AttributeID : 0;
    this.IsChangedColumns = true;
    this.AddNode(this._tlnAttributes, csap.ToString(), (object) new SearchSchemeEditor.NodeObject(3, attributeId, (object) csap), 3, -1, (object) attributeType.AttributeType);
  }

  private bool EditTreeListValue(IUserSession session)
  {
    bool flag = false;
    if (this.treeList1.SelectedNode != null && this.treeList1.SelectedNode.Tag is SearchSchemeEditor.NodeObject tag2 && tag2.TypeID != -1)
    {
      switch (tag2.CategoryID)
      {
        case 2:
          long[] numArray = this.ShowRolesDialog();
          if (numArray != null && numArray.Length == 1)
          {
            foreach (SearchSchemeRole role in this._searchScheme.Roles)
            {
              if (role.RoleID == numArray[0])
                return false;
            }
            int index = this._searchScheme.Roles.IndexOf((SearchSchemeRole) tag2.Tag);
            SearchSchemeRole searchSchemeRole = new SearchSchemeRole(numArray[0], session);
            if (searchSchemeRole.ValidRole)
            {
              this._searchScheme.Roles[index] = searchSchemeRole;
              tag2.Tag = (object) searchSchemeRole;
              this.RefreshTreeList();
              flag = true;
              break;
            }
            break;
          }
          break;
        case 3:
          ColumnSchemeAttProxy tag1 = tag2.Tag as ColumnSchemeAttProxy;
          List<int> enabledObjTypes = (List<int>) null;
          if (this._searchScheme.ObjectTypes != null && this._searchScheme.ObjectTypes.Count > 0)
          {
            enabledObjTypes = new List<int>(this._searchScheme.ObjectTypes.Count);
            foreach (GlobalType objectType in this._searchScheme.ObjectTypes)
              enabledObjTypes.Add(objectType.TypeID);
          }
          ColumnSchemeAttrEditForm schemeAttrEditForm = new ColumnSchemeAttrEditForm(enabledObjTypes, tag1.AttributeGuid, tag1.ColumnWidth, tag1.AttributeSource);
          if (schemeAttrEditForm.ShowDialog() == DialogResult.OK)
          {
            ColumnSchemeAttProxy columnSchemeAttProxy = new ColumnSchemeAttProxy(schemeAttrEditForm.Attribute, schemeAttrEditForm.AttributeSource, schemeAttrEditForm.ColumnWidth);
            if (!columnSchemeAttProxy.Equals((object) tag1))
            {
              foreach (object viewColumn in this._searchScheme.ViewColumns)
              {
                if (viewColumn.Equals((object) columnSchemeAttProxy))
                  return false;
              }
            }
            int index = this._searchScheme.ViewColumns.IndexOf((ColumnSchemeAttProxy) tag2.Tag);
            if (index >= 0)
            {
              this._searchScheme.ViewColumns[index] = columnSchemeAttProxy;
              IDBAttributeType attributeType = session.GetAttributeType(columnSchemeAttProxy.AttributeGuid);
              tag2.TypeID = attributeType.AttributeID;
              tag2.Tag = (object) columnSchemeAttProxy;
              this.RefreshTreeList();
              flag = true;
              this.IsChangedColumns = true;
              break;
            }
            break;
          }
          break;
        case 4:
          TreeNode parent = this.treeList1.SelectedNode.Parent;
          List<GlobalType> collectionForNode = this.GetCollectionForNode(parent);
          ArrayList arrayList1 = this.ShowObjectTypesDialog(false, collectionForNode, this.GetSelectDialogCaption(parent));
          if (arrayList1 != null && arrayList1.Count == 1)
          {
            GlobalType globalType = new GlobalType((int) arrayList1[0], 4, session);
            int index = collectionForNode.IndexOf((GlobalType) tag2.Tag);
            collectionForNode[index] = globalType;
            tag2.TypeID = globalType.TypeID;
            tag2.Tag = (object) globalType;
            this.RefreshTreeList();
            flag = true;
            break;
          }
          break;
        case 6:
          ArrayList arrayList2 = this.ShowRelationTypesDialog(false);
          if (arrayList2 != null && arrayList2.Count == 1)
          {
            int index = this._searchScheme.RelationTypes.IndexOf((GlobalType) tag2.Tag);
            if (index >= 0)
            {
              GlobalType globalType = new GlobalType((int) arrayList2[0], 6, session);
              this._searchScheme.RelationTypes[index] = globalType;
              tag2.TypeID = globalType.TypeID;
              tag2.Tag = (object) globalType;
              this.RefreshTreeList();
              flag = true;
              break;
            }
            break;
          }
          break;
      }
    }
    return flag;
  }

  private TreeNode AddNode(TreeNode parent, string text, object tag, int category, int typeID)
  {
    return this.AddNode(parent, text, tag, category, typeID, (object) null);
  }

  private TreeNode AddNode(
    TreeNode parent,
    string text,
    object tag,
    int category,
    int typeID,
    object data)
  {
    TreeNode treeNode = parent == null ? this.treeList1.Nodes.Add(text) : parent.Nodes.Add(text);
    if (Statics.IconSrv != null)
    {
      int num = data != null ? Statics.IconSrv.IndexOf(category, typeID, data) : Statics.IconSrv.IndexOf(category, typeID);
      treeNode.ImageIndex = num;
      treeNode.SelectedImageIndex = num;
    }
    treeNode.Tag = tag;
    if (parent == null)
      this.treeList1.ExpandAll();
    else
      parent.Expand();
    return treeNode;
  }

  private void btnApply_Click(object sender, EventArgs e)
  {
    this.SaveObjectData();
    if (this.ParentMode == 2)
      return;
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    if (this.ParentMode != 2)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
    else
      this.LoadObjectData(this.EditorMode);
  }

  private void repositoryItemButtonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
  {
    bool flag = false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      flag = this.EditTreeListValue(sessionKeeper.Session);
    if (!flag)
      return;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void eSelection_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyCode != Keys.Delete)
      return;
    this._searchScheme.SelectionID = -1L;
    this._searchScheme.SelectionName = string.Empty;
    this.eSelection.Text = string.Empty;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void bSelectionAdd_Click(object sender, EventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long objectByTypeDialog = (ServicesManager.GetService(typeof (IObjectCreatorService)) as IObjectCreatorService).CreateObjectByTypeDialog(new Guid[2]
      {
        new Guid("cad00122-306c-11d8-b4e9-00304f19f545"),
        new Guid("cad00123-306c-11d8-b4e9-00304f19f545")
      });
      if (objectByTypeDialog == -1L)
        return;
      IDBObject dbObject = sessionKeeper.Session.GetObject(objectByTypeDialog, false);
      if (dbObject == null)
        return;
      this._searchScheme.SelectionID = dbObject.ObjectID;
      this._searchScheme.SelectionName = this.GetSelectionName(dbObject.ObjectID, dbObject.Caption);
      this.eSelection.Text = this._searchScheme.SelectionName;
      this.IsChanged = true;
      this.UpdateControls();
      this.CheckColumnsAfterAddSelection(sessionKeeper.Session);
    }
  }

  private string GetSelectionName(long selectionID, string caption)
  {
    return !(caption != string.Empty) ? $"<{selectionID}>" : caption;
  }

  private void CheckColumnsAfterAddSelection(IUserSession session)
  {
    if (!(session.GetCustomService(typeof (ISelectionsService)) is ISelectionsService customService))
      return;
    ConditionStructure[] conditionStructures = customService.GetConditionStructures((object) session.SessionGUID, this._searchScheme.SelectionID);
    if (conditionStructures == null || conditionStructures.Length == 0)
      return;
    foreach (ConditionStructure conditionStructure in conditionStructures)
    {
      Guid guid = Guid.Empty;
      if (conditionStructure.Attribute != null)
      {
        if (conditionStructure.Attribute is string)
        {
          IDBAttributeType attributeType = session.GetAttributeType((string) conditionStructure.Attribute, false);
          if (attributeType != null)
            guid = (attributeType as IDBGuid).GUID;
        }
        else if (conditionStructure.Attribute is Guid)
          guid = (Guid) conditionStructure.Attribute;
        else if (conditionStructure.Attribute is int)
        {
          IDBAttributeType attributeType = session.GetAttributeType((int) conditionStructure.Attribute, false);
          if (attributeType != null)
            guid = (attributeType as IDBGuid).GUID;
        }
      }
      if (guid != Guid.Empty)
      {
        AttributeSourceTypes attributeSource = conditionStructure.AttributeSource == AttributeSourceTypes.Relation ? AttributeSourceTypes.Relation : AttributeSourceTypes.Object;
        bool flag = true;
        if (this._searchScheme.ViewColumns != null && this._searchScheme.ViewColumns.Count > 0)
        {
          foreach (ColumnSchemeAttProxy viewColumn in this._searchScheme.ViewColumns)
          {
            if (viewColumn.AttributeGuid.Equals(guid) && viewColumn.AttributeSource == attributeSource)
            {
              flag = false;
              break;
            }
          }
        }
        if (flag)
          this.AddAttribute(new ColumnSchemeAttProxy(guid, attributeSource, 150), session);
      }
    }
  }

  private void bSelectionView_Click(object sender, EventArgs e)
  {
    if (this._searchScheme.SelectionID == -1L)
      return;
    int num = (int) PropertiesWindow.Execute(string.Empty, string.Empty, this._searchScheme.SelectionID, "SelectionViewObject");
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._searchScheme.SelectionID, false);
      if (dbObject != null)
      {
        string selectionName = this.GetSelectionName(dbObject.ObjectID, dbObject.Caption);
        if (this._searchScheme.SelectionName != selectionName)
        {
          this._searchScheme.SelectionName = selectionName;
          this.eSelection.Text = this._searchScheme.SelectionName;
        }
        this.IsChanged = true;
        this.UpdateControls();
      }
      this.CheckColumnsAfterAddSelection(sessionKeeper.Session);
    }
  }

  private void cbGrouping_CheckedChanged(object sender, EventArgs e)
  {
    if (this.cbGrouping.Checked)
      this._searchScheme.Options |= SearchOptions.ObjectGrouping;
    else
      this._searchScheme.Options &= ~SearchOptions.ObjectGrouping;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void cbInSelectProd_CheckedChanged(object sender, EventArgs e)
  {
    if (this.cbInSelectProd.Checked)
      this._searchScheme.Options |= SearchOptions.InSelectionProd;
    else
      this._searchScheme.Options &= ~SearchOptions.InSelectionProd;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void cbActual_CheckedChanged(object sender, EventArgs e)
  {
    if (this.cbActual.Checked)
      this._searchScheme.Options |= SearchOptions.ActualSubstitutesOnly;
    else
      this._searchScheme.Options &= ~SearchOptions.ActualSubstitutesOnly;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void biAttibuteUp_Click(object sender, EventArgs e)
  {
    if (((SearchSchemeEditor.NodeObject) this.treeList1.SelectedNode.Tag).Tag is ColumnSchemeAttProxy tag1)
    {
      int index1 = this._searchScheme.ViewColumns.IndexOf(tag1);
      if (index1 > 0)
      {
        List<ColumnSchemeAttProxy> columnSchemeAttProxyList = new List<ColumnSchemeAttProxy>(this._searchScheme.ViewColumns.Count);
        for (int index2 = 0; index2 < this._searchScheme.ViewColumns.Count; ++index2)
        {
          if (index2 == index1 - 1)
            columnSchemeAttProxyList.Add(this._searchScheme.ViewColumns[index1]);
          else if (index2 == index1)
            columnSchemeAttProxyList.Add(this._searchScheme.ViewColumns[index1 - 1]);
          else
            columnSchemeAttProxyList.Add(this._searchScheme.ViewColumns[index2]);
        }
        this._searchScheme.ViewColumns = columnSchemeAttProxyList;
      }
    }
    this.IsChanged = true;
    this.IsChangedColumns = true;
    TreeNode prevNode = this.treeList1.SelectedNode.PrevNode;
    SearchSchemeEditor.NodeObject tag2 = (SearchSchemeEditor.NodeObject) prevNode.Tag;
    ColumnSchemeAttProxy tag3 = tag2.Tag as ColumnSchemeAttProxy;
    SearchSchemeEditor.NodeObject nodeObject = new SearchSchemeEditor.NodeObject(tag2.CategoryID, tag2.TypeID, (object) new ColumnSchemeAttProxy(tag3.AttributeGuid, tag3.AttributeSource, tag3.ColumnWidth));
    string text = prevNode.Text;
    this.treeList1.AfterSelect -= new TreeViewEventHandler(this.treeList1_AfterSelect);
    try
    {
      prevNode.Tag = this.treeList1.SelectedNode.Tag;
      prevNode.Text = this.treeList1.SelectedNode.Text;
      this.treeList1.SelectedNode.Tag = (object) nodeObject;
      this.treeList1.SelectedNode.Text = text;
      this.treeList1.SelectedNode = prevNode;
    }
    finally
    {
      this.treeList1.AfterSelect += new TreeViewEventHandler(this.treeList1_AfterSelect);
    }
    this.UpdateControls();
  }

  private void biAttibuteDown_Click(object sender, EventArgs e)
  {
    if (((SearchSchemeEditor.NodeObject) this.treeList1.SelectedNode.Tag).Tag is ColumnSchemeAttProxy tag1)
    {
      int index1 = this._searchScheme.ViewColumns.IndexOf(tag1);
      if (index1 < this._searchScheme.ViewColumns.Count)
      {
        List<ColumnSchemeAttProxy> columnSchemeAttProxyList = new List<ColumnSchemeAttProxy>(this._searchScheme.ViewColumns.Count);
        for (int index2 = 0; index2 < this._searchScheme.ViewColumns.Count; ++index2)
        {
          if (index2 == index1)
            columnSchemeAttProxyList.Add(this._searchScheme.ViewColumns[index1 + 1]);
          else if (index2 == index1 + 1)
            columnSchemeAttProxyList.Add(this._searchScheme.ViewColumns[index1]);
          else
            columnSchemeAttProxyList.Add(this._searchScheme.ViewColumns[index2]);
        }
        this._searchScheme.ViewColumns = columnSchemeAttProxyList;
      }
    }
    this.IsChanged = true;
    this.IsChangedColumns = true;
    TreeNode nextNode = this.treeList1.SelectedNode.NextNode;
    SearchSchemeEditor.NodeObject tag2 = (SearchSchemeEditor.NodeObject) nextNode.Tag;
    ColumnSchemeAttProxy tag3 = tag2.Tag as ColumnSchemeAttProxy;
    SearchSchemeEditor.NodeObject nodeObject = new SearchSchemeEditor.NodeObject(tag2.CategoryID, tag2.TypeID, (object) new ColumnSchemeAttProxy(tag3.AttributeGuid, tag3.AttributeSource, tag3.ColumnWidth));
    string text = nextNode.Text;
    this.treeList1.AfterSelect -= new TreeViewEventHandler(this.treeList1_AfterSelect);
    try
    {
      nextNode.Tag = this.treeList1.SelectedNode.Tag;
      nextNode.Text = this.treeList1.SelectedNode.Text;
      this.treeList1.SelectedNode.Tag = (object) nodeObject;
      this.treeList1.SelectedNode.Text = text;
      this.treeList1.SelectedNode = nextNode;
    }
    finally
    {
      this.treeList1.AfterSelect += new TreeViewEventHandler(this.treeList1_AfterSelect);
    }
    this.UpdateControls();
  }

  private void bVersionRuleOpen_Click(object sender, EventArgs e)
  {
    using (VersionRulesSelectionForm rulesSelectionForm = new VersionRulesSelectionForm(VersionRulesSelectFilter.vrfNone, false, "", this._searchScheme.VersionRule))
    {
      if (rulesSelectionForm.ShowDialog() != DialogResult.OK)
        return;
      VersionsRule[] selectedRules = rulesSelectionForm.SelectedRules;
      if (selectedRules.Length != 0)
      {
        Guid guid = new Guid(selectedRules[0].RuleObjectGuid);
        if (this._searchScheme.VersionRule != guid)
        {
          this._searchScheme.VersionRule = guid;
          this.eVersionRule.Text = selectedRules[0].RuleObjectCaption;
        }
      }
      this.IsChanged = true;
      this.UpdateControls();
    }
  }

  private void eVersionRule_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyCode != Keys.Delete || !(this._searchScheme.VersionRule != Guid.Empty))
      return;
    this._searchScheme.VersionRule = Guid.Empty;
    this.eVersionRule.Text = this._currentRuleString;
    this.IsChanged = true;
    this.UpdateControls();
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SearchSchemeEditor));
    this.panel1 = new Panel();
    this.btnCancel = new Button();
    this.btnApply = new Button();
    this.panel2 = new Panel();
    this.treeList1 = new TreeView();
    this.contextMenuStripAdd = new ContextMenuStrip(this.components);
    this.tsmiAdd = new ToolStripMenuItem();
    this.tsmiEdit = new ToolStripMenuItem();
    this.tsmiDelete = new ToolStripMenuItem();
    this.tsmiSeparator = new ToolStripSeparator();
    this.tsmiUp = new ToolStripMenuItem();
    this.tsmiDown = new ToolStripMenuItem();
    this.toolBarValues = new Intermech.Bars.ToolBar();
    this.imagesToolabarImpact = new ImageList(this.components);
    this.btnAddValue = new ButtonItem();
    this.btnEditValue = new ButtonItem();
    this.btnDeleteValue = new ButtonItem();
    this.biAttibuteUp = new ButtonItem();
    this.biAttibuteDown = new ButtonItem();
    this.panel3 = new Panel();
    this.cbActual = new CheckBox();
    this.label4 = new Label();
    this.bVersionRuleOpen = new Button();
    this.eVersionRule = new TextBox();
    this.cbGrouping = new CheckBox();
    this.cbInSelectProd = new CheckBox();
    this.bSelectionView = new Button();
    this.bSelectionAdd = new Button();
    this.bSelectionOpen = new Button();
    this.eSelection = new TextBox();
    this.eSearchDirection = new ComboBox();
    this.eName = new TextBox();
    this.label3 = new Label();
    this.label2 = new Label();
    this.label1 = new Label();
    this.toolTipController1 = new ToolTipController(this.components);
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.contextMenuStripAdd.SuspendLayout();
    this.panel3.SuspendLayout();
    this.SuspendLayout();
    this.panel1.Controls.Add((Control) this.btnCancel);
    this.panel1.Controls.Add((Control) this.btnApply);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
    this.btnCancel.Cursor = Cursors.Default;
    this.btnCancel.DialogResult = DialogResult.Cancel;
    this.btnCancel.Name = "btnCancel";
    this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
    componentResourceManager.ApplyResources((object) this.btnApply, "btnApply");
    this.btnApply.Cursor = Cursors.Default;
    this.btnApply.Name = "btnApply";
    this.btnApply.Click += new EventHandler(this.btnApply_Click);
    this.panel2.Controls.Add((Control) this.treeList1);
    this.panel2.Controls.Add((Control) this.toolBarValues);
    this.panel2.Controls.Add((Control) this.panel3);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    this.treeList1.ContextMenuStrip = this.contextMenuStripAdd;
    componentResourceManager.ApplyResources((object) this.treeList1, "treeList1");
    this.treeList1.Name = "treeList1";
    this.treeList1.AfterSelect += new TreeViewEventHandler(this.treeList1_AfterSelect);
    this.contextMenuStripAdd.Items.AddRange(new ToolStripItem[6]
    {
      (ToolStripItem) this.tsmiAdd,
      (ToolStripItem) this.tsmiEdit,
      (ToolStripItem) this.tsmiDelete,
      (ToolStripItem) this.tsmiSeparator,
      (ToolStripItem) this.tsmiUp,
      (ToolStripItem) this.tsmiDown
    });
    this.contextMenuStripAdd.Name = "contextMenuStripAdd";
    componentResourceManager.ApplyResources((object) this.contextMenuStripAdd, "contextMenuStripAdd");
    this.tsmiAdd.Name = "tsmiAdd";
    componentResourceManager.ApplyResources((object) this.tsmiAdd, "tsmiAdd");
    this.tsmiAdd.Click += new EventHandler(this.tsmiAdd_Click);
    this.tsmiEdit.Name = "tsmiEdit";
    componentResourceManager.ApplyResources((object) this.tsmiEdit, "tsmiEdit");
    this.tsmiEdit.Click += new EventHandler(this.tsmiEdit_Click);
    this.tsmiDelete.Name = "tsmiDelete";
    componentResourceManager.ApplyResources((object) this.tsmiDelete, "tsmiDelete");
    this.tsmiDelete.Click += new EventHandler(this.tsmiDelete_Click);
    this.tsmiSeparator.Name = "tsmiSeparator";
    componentResourceManager.ApplyResources((object) this.tsmiSeparator, "tsmiSeparator");
    this.tsmiUp.Name = "tsmiUp";
    componentResourceManager.ApplyResources((object) this.tsmiUp, "tsmiUp");
    this.tsmiUp.Click += new EventHandler(this.biAttibuteUp_Click);
    this.tsmiDown.Name = "tsmiDown";
    componentResourceManager.ApplyResources((object) this.tsmiDown, "tsmiDown");
    this.tsmiDown.Click += new EventHandler(this.biAttibuteDown_Click);
    this.toolBarValues.AllowVerticalDock = false;
    this.toolBarValues.DockLine = 3;
    this.toolBarValues.FullMenus = true;
    this.toolBarValues.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarValues.Hidden = false;
    this.toolBarValues.ImageList = this.imagesToolabarImpact;
    this.toolBarValues.Items.AddRange(new ToolbarItemBase[5]
    {
      (ToolbarItemBase) this.btnAddValue,
      (ToolbarItemBase) this.btnEditValue,
      (ToolbarItemBase) this.btnDeleteValue,
      (ToolbarItemBase) this.biAttibuteUp,
      (ToolbarItemBase) this.biAttibuteDown
    });
    componentResourceManager.ApplyResources((object) this.toolBarValues, "toolBarValues");
    this.toolBarValues.MinimumFloatingSize = new Size(250, 30);
    this.toolBarValues.Name = "toolBarValues";
    this.toolBarValues.Overflow = ToolBarOverflow.Wrap;
    this.toolBarValues.Stretch = true;
    this.toolBarValues.ButtonClick += new Intermech.Bars.ToolBar.ButtonClickEventHandler(this.toolBarValues_ButtonClick);
    this.imagesToolabarImpact.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolabarImpact.ImageStream");
    this.imagesToolabarImpact.TransparentColor = Color.Transparent;
    this.imagesToolabarImpact.Images.SetKeyName(0, "add.png");
    this.imagesToolabarImpact.Images.SetKeyName(1, "delete.png");
    this.imagesToolabarImpact.Images.SetKeyName(2, "edit.png");
    componentResourceManager.ApplyResources((object) this.btnAddValue, "btnAddValue");
    this.btnAddValue.ImageIndex = 0;
    this.btnAddValue.ShowText = true;
    componentResourceManager.ApplyResources((object) this.btnEditValue, "btnEditValue");
    this.btnEditValue.ImageIndex = 2;
    this.btnEditValue.ShowText = true;
    componentResourceManager.ApplyResources((object) this.btnDeleteValue, "btnDeleteValue");
    this.btnDeleteValue.ImageIndex = 1;
    this.btnDeleteValue.ShowText = true;
    componentResourceManager.ApplyResources((object) this.biAttibuteUp, "biAttibuteUp");
    this.biAttibuteUp.Image = (Image) Intermech.Pdm.Properties.Resources.Outline_Move_Up;
    this.biAttibuteUp.Click += new EventHandler(this.biAttibuteUp_Click);
    componentResourceManager.ApplyResources((object) this.biAttibuteDown, "biAttibuteDown");
    this.biAttibuteDown.Image = (Image) Intermech.Pdm.Properties.Resources.Outline_Move_Down;
    this.biAttibuteDown.Click += new EventHandler(this.biAttibuteDown_Click);
    this.panel3.Controls.Add((Control) this.cbActual);
    this.panel3.Controls.Add((Control) this.label4);
    this.panel3.Controls.Add((Control) this.bVersionRuleOpen);
    this.panel3.Controls.Add((Control) this.eVersionRule);
    this.panel3.Controls.Add((Control) this.cbGrouping);
    this.panel3.Controls.Add((Control) this.cbInSelectProd);
    this.panel3.Controls.Add((Control) this.bSelectionView);
    this.panel3.Controls.Add((Control) this.bSelectionAdd);
    this.panel3.Controls.Add((Control) this.bSelectionOpen);
    this.panel3.Controls.Add((Control) this.eSelection);
    this.panel3.Controls.Add((Control) this.eSearchDirection);
    this.panel3.Controls.Add((Control) this.eName);
    this.panel3.Controls.Add((Control) this.label3);
    this.panel3.Controls.Add((Control) this.label2);
    this.panel3.Controls.Add((Control) this.label1);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    componentResourceManager.ApplyResources((object) this.cbActual, "cbActual");
    this.cbActual.Name = "cbActual";
    this.cbActual.UseVisualStyleBackColor = true;
    this.cbActual.CheckedChanged += new EventHandler(this.cbActual_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.label4, "label4");
    this.label4.Name = "label4";
    componentResourceManager.ApplyResources((object) this.bVersionRuleOpen, "bVersionRuleOpen");
    this.bVersionRuleOpen.Image = (Image) Intermech.Pdm.Properties.Resources.Open;
    this.bVersionRuleOpen.Name = "bVersionRuleOpen";
    this.toolTipController1.SetToolTip((Control) this.bVersionRuleOpen, "Добавить правило подбора версий");
    this.bVersionRuleOpen.UseVisualStyleBackColor = true;
    this.bVersionRuleOpen.Click += new EventHandler(this.bVersionRuleOpen_Click);
    componentResourceManager.ApplyResources((object) this.eVersionRule, "eVersionRule");
    this.eVersionRule.BackColor = SystemColors.Window;
    this.eVersionRule.Name = "eVersionRule";
    this.eVersionRule.ReadOnly = true;
    this.eVersionRule.KeyDown += new KeyEventHandler(this.eVersionRule_KeyDown);
    componentResourceManager.ApplyResources((object) this.cbGrouping, "cbGrouping");
    this.cbGrouping.Name = "cbGrouping";
    this.cbGrouping.UseVisualStyleBackColor = true;
    this.cbGrouping.CheckedChanged += new EventHandler(this.cbGrouping_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.cbInSelectProd, "cbInSelectProd");
    this.cbInSelectProd.Name = "cbInSelectProd";
    this.cbInSelectProd.UseVisualStyleBackColor = true;
    this.cbInSelectProd.CheckedChanged += new EventHandler(this.cbInSelectProd_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.bSelectionView, "bSelectionView");
    this.bSelectionView.Image = (Image) Intermech.Pdm.Properties.Resources.Note_Edit;
    this.bSelectionView.Name = "bSelectionView";
    this.toolTipController1.SetToolTip((Control) this.bSelectionView, "Свойства (карточка) выборки");
    this.bSelectionView.UseVisualStyleBackColor = true;
    this.bSelectionView.Click += new EventHandler(this.bSelectionView_Click);
    componentResourceManager.ApplyResources((object) this.bSelectionAdd, "bSelectionAdd");
    this.bSelectionAdd.Image = (Image) Intermech.Pdm.Properties.Resources.Note_Add;
    this.bSelectionAdd.Name = "bSelectionAdd";
    this.toolTipController1.SetToolTip((Control) this.bSelectionAdd, "Создать и добавить условие выборки");
    this.bSelectionAdd.UseVisualStyleBackColor = true;
    this.bSelectionAdd.Click += new EventHandler(this.bSelectionAdd_Click);
    componentResourceManager.ApplyResources((object) this.bSelectionOpen, "bSelectionOpen");
    this.bSelectionOpen.Image = (Image) Intermech.Pdm.Properties.Resources.Open;
    this.bSelectionOpen.Name = "bSelectionOpen";
    this.toolTipController1.SetToolTip((Control) this.bSelectionOpen, "Добавить условие существующей выборки");
    this.bSelectionOpen.UseVisualStyleBackColor = true;
    this.bSelectionOpen.Click += new EventHandler(this.bSelectionOpen_Click);
    componentResourceManager.ApplyResources((object) this.eSelection, "eSelection");
    this.eSelection.BackColor = SystemColors.Window;
    this.eSelection.Name = "eSelection";
    this.eSelection.ReadOnly = true;
    this.eSelection.KeyDown += new KeyEventHandler(this.eSelection_KeyDown);
    componentResourceManager.ApplyResources((object) this.eSearchDirection, "eSearchDirection");
    this.eSearchDirection.DropDownStyle = ComboBoxStyle.DropDownList;
    this.eSearchDirection.FormattingEnabled = true;
    this.eSearchDirection.Name = "eSearchDirection";
    this.eSearchDirection.SelectedIndexChanged += new EventHandler(this.eSearchDirection_SelectedIndexChanged);
    componentResourceManager.ApplyResources((object) this.eName, "eName");
    this.eName.Name = "eName";
    this.eName.TextChanged += new EventHandler(this.eName_TextChanged);
    componentResourceManager.ApplyResources((object) this.label3, "label3");
    this.label3.Name = "label3";
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.Name = "label2";
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    this.toolTipController1.Style = new ViewStyle("ToolTip style");
    this.toolTipController1.ToolTipLocation = ToolTipLocation.LeftBottom;
    this.AcceptButton = (IButtonControl) this.btnApply;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.HelpButton = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (SearchSchemeEditor);
    this.ShowInTaskbar = false;
    this.Tag = (object) " ";
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.contextMenuStripAdd.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.panel3.PerformLayout();
    this.ResumeLayout(false);
  }

  private class NodeObject
  {
    public int CategoryID;
    public int TypeID;
    public object Tag;

    public NodeObject(int categoryID, int typeId, object tag)
    {
      this.CategoryID = categoryID;
      this.TypeID = typeId;
      this.Tag = tag;
    }
  }

  private class SearchDirectionItem
  {
    public SearchDirectionItem(SearchDirection direction) => this.Direction = direction;

    public SearchDirection Direction { get; }

    public override string ToString() => EnumTypeHelper.GetCaption((Enum) this.Direction);
  }
}
