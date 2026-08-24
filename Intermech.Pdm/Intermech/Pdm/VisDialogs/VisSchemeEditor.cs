// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.VisSchemeEditor
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using DevExpress.IM.Utils;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Expert;
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
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

internal class VisSchemeEditor : Form
{
  public static SchemeEditorParams EditorParams;
  private VisParentMode _parentMode;
  public int EditorMode;
  public long SchemeID;
  public bool IsChanged;
  private TreeNode _tlnObjType;
  private TreeNode _typesToDisableExpand;
  private TreeNode _typesToExpand;
  private TreeNode _tlnRelType;
  private TreeNode _tPreviewObjs;
  private TreeNode _tObjAttrs;
  private TreeNode _tRelAttrs;
  public int ObjectTypeID = -1;
  private int _rolesTypeID = -1;
  private VisSchemeParms _searchScheme = new VisSchemeParms();
  private string _currentRuleString = "";
  private bool lockChanged;
  private IContainer components;
  private Panel panel1;
  private Panel panel2;
  private Button btnCancel;
  private Button btnApply;
  private Panel panel3;
  private Label label1;
  private Label label3;
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
  private ToolStripSeparator tsmiSeparator;
  private Label label4;
  private Button bVersionRuleOpen;
  private TextBox eVersionRule;
  private GroupBox gbHiddenSostav;
  private RadioButton rbHiddenStru;
  private RadioButton rbHiddenHide;
  private RadioButton rbHiddenShow;
  private TextBox tbMaxLevels;
  private Label label2;
  private GroupBox gbSubst;
  private RadioButton rbSubstClient;
  private RadioButton rbSubstAll;
  private RadioButton rbSubstActual;
  private RadioButton rbHiddenClient;

  public static Icon CombineIcons(Icon ico1, Icon ico2)
  {
    ico1.ToBitmap();
    ico2.ToBitmap();
    using (Bitmap bmp = new Bitmap(32 /*0x20*/, 16 /*0x10*/, PixelFormat.Format24bppRgb))
    {
      using (Graphics graphics = Graphics.FromImage((Image) bmp))
      {
        using (Icon icon = new Icon(ico1, 16 /*0x10*/, 16 /*0x10*/))
          graphics.DrawIconUnstretched(icon, new Rectangle(0, 0, 16 /*0x10*/, 16 /*0x10*/));
        using (Icon icon = new Icon(ico2, 16 /*0x10*/, 16 /*0x10*/))
          graphics.DrawIconUnstretched(icon, new Rectangle(16 /*0x10*/, 0, 16 /*0x10*/, 16 /*0x10*/));
      }
      return ImageHelper.BitmapToIcon(bmp);
    }
  }

  public VisParentMode ParentMode
  {
    get => this._parentMode;
    set
    {
      if (value == VisParentMode.ObjCreator)
      {
        this.btnApply.Text = LocalizationHolder.rm.GetString("Pdm_64");
        this.Text = LocalizationHolder.rm.GetString("Pdm_725");
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

  public VisSchemeEditor()
  {
    this.InitializeComponent();
    this.Text = VisSchemeConsts.SearchSchemeEditorName;
    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
    this.Size = new Size(workingArea.Width / 100 * 40, workingArea.Height / 100 * 50);
    this.Location = new Point((workingArea.Width - this.Size.Width) / 3, (workingArea.Height - this.Size.Height) / 3);
    this.RuntimeFillControls();
    this.UpdateControls();
  }

  public static long Execute(int ObjectTypeID, long TemplateObjectID)
  {
    if (ObjectTypeID == 0)
      return 0;
    using (VisSchemeEditor visSchemeEditor = new VisSchemeEditor())
    {
      visSchemeEditor.SchemeID = TemplateObjectID >= 0L ? TemplateObjectID : 0L;
      visSchemeEditor.ParentMode = VisParentMode.ObjCreator;
      visSchemeEditor.ObjectTypeID = ObjectTypeID;
      visSchemeEditor.LoadObjectData(0);
      visSchemeEditor.SchemeID = 0L;
      visSchemeEditor._searchScheme.SchemeId = 0L;
      return visSchemeEditor.ExecuteForm();
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
    this.treeList1.ImageList = Statics.IconSrv?.ImageList;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.eName.MaxLength = (int) sessionKeeper.Session.GetAttributeType(new Guid("cad00047-306c-11d8-b4e9-00304f19f545")).SizeType;
    this.SetTreeListFirst();
  }

  public void UpdateControls()
  {
    this.btnAddValue.Enabled = this.btnEditValue.Enabled = this.btnDeleteValue.Enabled = this.tsmiAdd.Enabled = this.tsmiEdit.Enabled = this.tsmiDelete.Enabled = true;
    if (this.ParentMode == VisParentMode.ObjCreator)
    {
      this.btnApply.Enabled = this.btnCancel.Enabled = true;
    }
    else
    {
      this.btnApply.Enabled = this.IsChanged;
      if (this.ParentMode == VisParentMode.Standalone)
        this.btnCancel.Enabled = true;
      else
        this.btnCancel.Enabled = this.IsChanged;
    }
    this.bSelectionView.Enabled = this._searchScheme.SelectionId != -1L;
  }

  public void LoadObjectData(int AEditorMode)
  {
    this._searchScheme = new VisSchemeParms();
    this.EditorMode = AEditorMode;
    if (this.EditorMode < 0)
      this.EditorMode = 1;
    if (this.EditorMode >= VisSchemeConsts.Headers.Length)
      this.EditorMode = 1;
    this.IsChanged = false;
    if (this.SchemeID == 0L)
    {
      this.ObjectTypeID = ObjectTypesHelper.GetObjTypeID("cadd9aa6-306c-11d8-b4e9-00304f19f545");
    }
    else
    {
      this.ClearControls();
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        if (sessionKeeper.Session.GetObject(this.SchemeID) is ISchemeSaveLoad schemeSaveLoad)
          this._searchScheme = schemeSaveLoad.LoadScheme();
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
        this._searchScheme.SchemeId = dbObject.ObjectID;
        if (dbObject is ISchemeSaveLoad schemeSaveLoad)
          schemeSaveLoad.SaveScheme(this._searchScheme);
        dbObject.CommitCreation(true);
        this._searchScheme.SchemeId = this.SchemeID = dbObject.ObjectID;
      }
      else
      {
        if (sessionKeeper.Session.GetObject(this.SchemeID, false) is ISchemeSaveLoad schemeSaveLoad)
          schemeSaveLoad.SaveScheme(this._searchScheme);
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

  private void FillControls(IUserSession session, VisSchemeParms scheme)
  {
    if (scheme == null)
    {
      this.ClearControls();
    }
    else
    {
      this.eName.Text = scheme.Name;
      this.eSelection.Text = scheme.SelectionName;
      this.tbMaxLevels.Text = scheme.maxLevels > 0 ? scheme.maxLevels.ToString() : "";
      switch (scheme.useZamens)
      {
        case UseZamens.AsClient:
          this.rbSubstClient.Checked = true;
          break;
        case UseZamens.MainVariant:
          this.rbSubstActual.Checked = true;
          break;
        case UseZamens.AllVariants:
          this.rbSubstAll.Checked = true;
          break;
      }
      switch (scheme.hiddenMode)
      {
        case HiddenContentsMode.ShowAllHidden:
          this.rbHiddenShow.Checked = true;
          break;
        case HiddenContentsMode.HideOnlyHidden:
          this.rbHiddenStru.Checked = true;
          break;
        case HiddenContentsMode.HideHiddenAndRoots:
          this.rbHiddenHide.Checked = true;
          break;
        case HiddenContentsMode.HiddenAsClient:
          this.rbHiddenClient.Checked = true;
          break;
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
      this.AddNode(parentNode, tag.TypeName, (object) new VisSchemeEditor.NodeObject(category, tag.TypeID, (object) tag), category, tag.TypeID);
  }

  private void FillTreeList(VisSchemeParms scheme)
  {
    this.treeList1.BeginUpdate();
    this.AddNodesFromCollection(this._tlnObjType, scheme.ObjectTypes, 4);
    this.AddNodesFromCollection(this._typesToExpand, scheme.TypesToExpand, 4);
    this.AddNodesFromCollection(this._typesToDisableExpand, scheme.TypesToDisableExpand, 4);
    this.AddNodesFromCollection(this._tlnRelType, scheme.RelationTypes, 6);
    this.AddNodesFromCollection(this._tPreviewObjs, scheme.PreviewTypes, 4);
    this.AddNodesFromCollection(this._tObjAttrs, scheme.ObjectAttrs, 3);
    this.AddNodesFromCollection(this._tRelAttrs, scheme.RelationAttrs, 3);
    this.treeList1.EndUpdate();
    this._tlnObjType.Expand();
    this._typesToExpand.Expand();
    this._typesToDisableExpand.Expand();
    this._tlnRelType.Expand();
    this._tPreviewObjs.Expand();
    this._tObjAttrs.Expand();
    this._tRelAttrs.Expand();
  }

  private TreeNode CreateRootNode(string text, int category)
  {
    return this.AddNode((TreeNode) null, text, (object) new VisSchemeEditor.NodeObject(category, 0, (object) null), category, 0);
  }

  private void SetTreeListFirst()
  {
    this.treeList1.BeginUpdate();
    this.treeList1.Nodes.Clear();
    this._tlnObjType = this.CreateRootNode(VisSchemeConsts.SearchObjectTypes, 4);
    this._typesToExpand = this.CreateRootNode(VisSchemeConsts.TypesToExpand, 4);
    this._typesToDisableExpand = this.CreateRootNode(VisSchemeConsts.TypesToDisableExpand, 4);
    this._tlnRelType = this.CreateRootNode(VisSchemeConsts.SearchRelationTypes, 6);
    this._tPreviewObjs = this.CreateRootNode(VisSchemeConsts.VisPreviewObjTypes, 4);
    this._tObjAttrs = this.CreateRootNode(VisSchemeConsts.VisObjectAttrs, 3);
    this._tRelAttrs = this.CreateRootNode(VisSchemeConsts.VisRelationAttrs, 3);
    this.treeList1.EndUpdate();
  }

  private void ClearControls()
  {
    this.eName.Text = string.Empty;
    this.eSelection.Text = string.Empty;
    this.SetTreeListFirst();
  }

  private void bSelectionOpen_Click(object sender, EventArgs e)
  {
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("Pdm_66"), string.Empty, ObjectTypesHelper.GetObjTypeID("cad00156-306c-11d8-b4e9-00304f19f545"), SelectionOptions.Default);
    if (numArray == null || this._searchScheme.SelectionId == numArray[0])
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(numArray[0], false);
      if (dbObject == null)
        return;
      this._searchScheme.SelectionId = dbObject.ObjectID;
      this._searchScheme.SelectionName = dbObject.Caption != string.Empty ? dbObject.Caption : $"<{dbObject.ObjectID}>";
      this.eSelection.Text = this._searchScheme.SelectionName;
      this.IsChanged = true;
      this.UpdateControls();
    }
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
    if (this.treeList1.SelectedNode != null && this.treeList1.SelectedNode.Parent != null && this.treeList1.SelectedNode.Tag is VisSchemeEditor.NodeObject tag && tag.TypeID != -1)
    {
      TreeNode parent = this.treeList1.SelectedNode.Parent;
      switch (tag.CategoryID)
      {
        case 3:
          (parent == this._tObjAttrs ? this._searchScheme.ObjectAttrs : this._searchScheme.RelationAttrs).Remove((GlobalType) tag.Tag);
          this.RefreshTreeList(true);
          flag = true;
          break;
        case 4:
          if (parent == this._tlnObjType)
            this._searchScheme.ObjectTypes.Remove((GlobalType) tag.Tag);
          else if (parent == this._typesToExpand)
            this._searchScheme.TypesToExpand.Remove((GlobalType) tag.Tag);
          else if (parent == this._typesToDisableExpand)
            this._searchScheme.TypesToDisableExpand.Remove((GlobalType) tag.Tag);
          else if (parent == this._tPreviewObjs)
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
      if (this.treeList1.SelectedNode.Tag is VisSchemeEditor.NodeObject tag)
      {
        this.treeList1.SelectedNode.Text = tag.Tag.ToString();
        int imageIndex = tag.GetImageIndex();
        this.treeList1.SelectedNode.ImageIndex = imageIndex;
        this.treeList1.SelectedNode.SelectedImageIndex = imageIndex;
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

  private ArrayList ShowRelationTypesDialog(bool Multiselect)
  {
    ArrayList arrayList = new ArrayList();
    SelectorForm selectorForm = new SelectorForm(typeof (RelationTypesFolder), VisSchemeConsts.SearchRelationTypes, typeof (RelationTypeFolder), Multiselect);
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

  private bool AddTreeListValue(IUserSession session)
  {
    bool flag = false;
    if (this.treeList1.SelectedNode != null && this.treeList1.SelectedNode.Tag is VisSchemeEditor.NodeObject tag)
    {
      switch (tag.CategoryID)
      {
        case 3:
          List<int> attrIds;
          if (this.ShowAttributesDialog(this._searchScheme.ObjectTypes.ConvertAll<int>((Converter<GlobalType, int>) (gt => gt.TypeID)), true, out attrIds))
          {
            List<GlobalType> globList = this.treeList1.SelectedNode == this._tObjAttrs ? this._searchScheme.ObjectAttrs : this._searchScheme.RelationAttrs;
            for (int index = 0; index < attrIds.Count; ++index)
            {
              GlobalType globalType = new GlobalType(attrIds[index], 3);
              globList.Add(globalType);
            }
            this.UpdateAttrNodes(globList, this.treeList1.SelectedNode);
            flag = true;
            break;
          }
          break;
        case 4:
          TreeNode parent = this.treeList1.SelectedNode.Parent ?? this.treeList1.SelectedNode;
          List<GlobalType> collectionForNode = this.GetCollectionForNode(parent);
          if (collectionForNode == null)
            return false;
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

  private void UpdateAttrNodes(List<GlobalType> globList, TreeNode root)
  {
    this._searchScheme.SortByName(globList);
    this.treeList1.BeginUpdate();
    root.Nodes.Clear();
    foreach (GlobalType glob in globList)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(glob.TypeID);
      this.AddNode(root, glob.TypeName, (object) new VisSchemeEditor.NodeObject(3, glob.TypeID, (object) glob), 3, -1, (object) attributeType.FieldType);
    }
    this.treeList1.EndUpdate();
  }

  private bool ShowAttributesDialog(List<int> enabledObjTypes, bool multi, out List<int> attrIds)
  {
    attrIds = new List<int>();
    if (Statics.IconSrv == null)
      return false;
    AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(multi);
    if (enabledObjTypes != null && enabledObjTypes.Count > 0)
      attributesSelectDlg.LoadAttrDialogForObjectsTypes(enabledObjTypes);
    attributesSelectDlg.SelectorFilter = (ISelectorFilter) new WithoutObligatoryFilter(new AttributeSourceTypes[2]
    {
      AttributeSourceTypes.Object,
      AttributeSourceTypes.Relation
    });
    if (attributesSelectDlg.ShowDialog() != DialogResult.OK)
      return false;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IUserSession session = sessionKeeper.Session;
      foreach (int anAttributeType in attributesSelectDlg.SelectedAttributesID)
      {
        if (session.GetAttributeType(anAttributeType, false) != null)
          attrIds.Add(anAttributeType);
      }
      return true;
    }
  }

  private string GetSelectDialogCaption(TreeNode parent)
  {
    if (parent == this._tlnObjType)
      return VisSchemeConsts.SearchObjectTypes;
    if (parent == this._typesToExpand)
      return VisSchemeConsts.TypesToExpand;
    if (parent == this._typesToDisableExpand)
      return VisSchemeConsts.TypesToDisableExpand;
    return parent == this._tPreviewObjs ? VisSchemeConsts.TypesToPreview : string.Empty;
  }

  private List<GlobalType> GetCollectionForNode(TreeNode parent)
  {
    if (parent == this._tlnObjType)
      return this._searchScheme.ObjectTypes;
    if (parent == this._typesToExpand)
      return this._searchScheme.TypesToExpand;
    if (parent == this._typesToDisableExpand)
      return this._searchScheme.TypesToDisableExpand;
    return parent == this._tPreviewObjs ? this._searchScheme.PreviewTypes : (List<GlobalType>) null;
  }

  private void AddObjectType(
    TreeNode parent,
    List<GlobalType> collection,
    int objType,
    IUserSession session)
  {
    GlobalType tag = new GlobalType(objType, 4, session);
    collection.Add(tag);
    this.AddNode(parent, tag.ToString(), (object) new VisSchemeEditor.NodeObject(4, tag.TypeID, (object) tag), 4, tag.TypeID);
    if (collection == this._searchScheme.TypesToExpand && this._searchScheme.TypesToDisableExpand.Any<GlobalType>((Func<GlobalType, bool>) (x => x.TypeID == objType)))
    {
      this._searchScheme.TypesToDisableExpand.RemoveAll((Predicate<GlobalType>) (x => x.TypeID == objType));
      this.DelObjectType(this._typesToDisableExpand, objType);
    }
    if (collection != this._searchScheme.TypesToDisableExpand || !this._searchScheme.TypesToExpand.Any<GlobalType>((Func<GlobalType, bool>) (x => x.TypeID == objType)))
      return;
    this._searchScheme.TypesToExpand.RemoveAll((Predicate<GlobalType>) (x => x.TypeID == objType));
    this.DelObjectType(this._typesToExpand, objType);
  }

  private void DelObjectType(TreeNode root, int objType)
  {
    foreach (TreeNode node in root.Nodes)
    {
      if (node.Tag is VisSchemeEditor.NodeObject tag && tag.TypeID == objType)
      {
        root.Nodes.Remove(node);
        break;
      }
    }
  }

  private void AddRelationType(int relType, IUserSession session)
  {
    GlobalType tag = new GlobalType(relType, 6, session);
    this._searchScheme.RelationTypes.Add(tag);
    this.AddNode(this._tlnRelType, tag.ToString(), (object) new VisSchemeEditor.NodeObject(6, tag.TypeID, (object) tag), 6, tag.TypeID);
  }

  private bool EditTreeListValue(IUserSession session)
  {
    bool flag = false;
    if (this.treeList1.SelectedNode != null && this.treeList1.SelectedNode.Tag is VisSchemeEditor.NodeObject tag2 && tag2.TypeID != -1)
    {
      switch (tag2.CategoryID)
      {
        case 3:
          List<int> enabledObjTypes = this._searchScheme.ObjectTypes.ConvertAll<int>((Converter<GlobalType, int>) (gt => gt.TypeID));
          TreeNode selectedNode = this.treeList1.SelectedNode;
          List<int> attrIds;
          if (selectedNode.Tag is VisSchemeEditor.NodeObject tag1 && this.ShowAttributesDialog(enabledObjTypes, false, out attrIds))
          {
            List<GlobalType> globList = selectedNode.Parent == this._tObjAttrs ? this._searchScheme.ObjectAttrs : this._searchScheme.RelationAttrs;
            GlobalType globalType = new GlobalType(attrIds[0], 3);
            globList.Remove((GlobalType) tag1.Tag);
            globList.Add(globalType);
            this.UpdateAttrNodes(globList, selectedNode.Parent);
            break;
          }
          break;
        case 4:
          TreeNode parent = this.treeList1.SelectedNode.Parent;
          List<GlobalType> collectionForNode = this.GetCollectionForNode(parent);
          if (collectionForNode == null)
            return false;
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
      int num = !(tag is VisSchemeEditor.NodeObject) ? (data != null ? Statics.IconSrv.IndexOf(category, typeID, data) : Statics.IconSrv.IndexOf(category, typeID)) : (tag as VisSchemeEditor.NodeObject).GetImageIndex();
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
    if (this.ParentMode == VisParentMode.NavigatorView)
      return;
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  private void btnCancel_Click(object sender, EventArgs e)
  {
    if (this.ParentMode != VisParentMode.NavigatorView)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
    else
      this.LoadObjectData(this.EditorMode);
  }

  private void eSelection_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyCode != Keys.Delete)
      return;
    this._searchScheme.SelectionId = -1L;
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
      this._searchScheme.SelectionId = dbObject.ObjectID;
      this._searchScheme.SelectionName = this.GetSelectionName(dbObject.ObjectID, dbObject.Caption);
      this.eSelection.Text = this._searchScheme.SelectionName;
      this.IsChanged = true;
      this.UpdateControls();
    }
  }

  private string GetSelectionName(long selectionID, string caption)
  {
    return !(caption != string.Empty) ? $"<{selectionID}>" : caption;
  }

  private void bSelectionView_Click(object sender, EventArgs e)
  {
    if (this._searchScheme.SelectionId == -1L)
      return;
    int num = (int) PropertiesWindow.Execute(string.Empty, string.Empty, this._searchScheme.SelectionId, "SelectionViewObject");
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject = sessionKeeper.Session.GetObject(this._searchScheme.SelectionId, false);
      if (dbObject == null)
        return;
      string selectionName = this.GetSelectionName(dbObject.ObjectID, dbObject.Caption);
      if (this._searchScheme.SelectionName != selectionName)
      {
        this._searchScheme.SelectionName = selectionName;
        this.eSelection.Text = this._searchScheme.SelectionName;
      }
      this.IsChanged = true;
      this.UpdateControls();
    }
  }

  private void cbGrouping_CheckedChanged(object sender, EventArgs e)
  {
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void cbActual_CheckedChanged(object sender, EventArgs e)
  {
    this.IsChanged = true;
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

  private void tbMaxLevels_TextChanged(object sender, EventArgs e)
  {
    if (this.lockChanged)
      return;
    int num1 = 0;
    string str = this.tbMaxLevels.Text.Trim();
    if (str != "")
    {
      try
      {
        num1 = Convert.ToInt32(str);
      }
      catch (Exception ex)
      {
        switch (ex)
        {
          case FormatException _:
          case OverflowException _:
            num1 = -1;
            break;
          default:
            throw;
        }
      }
    }
    if (num1 < 0)
    {
      int num2 = (int) MessageBox.Show(VisSchemeConsts.WrongLevels, "Ошибка", MessageBoxButtons.OK);
      this.lockChanged = true;
      try
      {
        this.tbMaxLevels.Text = this._searchScheme.maxLevels.ToString();
        this.tbMaxLevels.Focus();
      }
      finally
      {
        this.lockChanged = false;
      }
    }
    this._searchScheme.maxLevels = num1;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void rbSubstActual_CheckedChanged(object sender, EventArgs e)
  {
    if (sender == this.rbSubstActual)
      this._searchScheme.useZamens = UseZamens.MainVariant;
    if (sender == this.rbSubstAll)
      this._searchScheme.useZamens = UseZamens.AllVariants;
    if (sender == this.rbSubstClient)
      this._searchScheme.useZamens = UseZamens.AsClient;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void rbHiddenShow_CheckedChanged(object sender, EventArgs e)
  {
    if (sender == this.rbHiddenShow)
      this._searchScheme.hiddenMode = HiddenContentsMode.ShowAllHidden;
    if (sender == this.rbHiddenHide)
      this._searchScheme.hiddenMode = HiddenContentsMode.HideHiddenAndRoots;
    if (sender == this.rbHiddenStru)
      this._searchScheme.hiddenMode = HiddenContentsMode.HideOnlyHidden;
    if (sender == this.rbHiddenClient)
      this._searchScheme.hiddenMode = HiddenContentsMode.HiddenAsClient;
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (VisSchemeEditor));
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
    this.toolBarValues = new Intermech.Bars.ToolBar();
    this.imagesToolabarImpact = new ImageList(this.components);
    this.btnAddValue = new ButtonItem();
    this.btnEditValue = new ButtonItem();
    this.btnDeleteValue = new ButtonItem();
    this.panel3 = new Panel();
    this.gbSubst = new GroupBox();
    this.rbSubstClient = new RadioButton();
    this.rbSubstAll = new RadioButton();
    this.rbSubstActual = new RadioButton();
    this.gbHiddenSostav = new GroupBox();
    this.rbHiddenClient = new RadioButton();
    this.rbHiddenStru = new RadioButton();
    this.rbHiddenHide = new RadioButton();
    this.rbHiddenShow = new RadioButton();
    this.tbMaxLevels = new TextBox();
    this.label2 = new Label();
    this.label4 = new Label();
    this.bVersionRuleOpen = new Button();
    this.eVersionRule = new TextBox();
    this.bSelectionView = new Button();
    this.bSelectionAdd = new Button();
    this.bSelectionOpen = new Button();
    this.eSelection = new TextBox();
    this.eName = new TextBox();
    this.label3 = new Label();
    this.label1 = new Label();
    this.toolTipController1 = new ToolTipController(this.components);
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.contextMenuStripAdd.SuspendLayout();
    this.panel3.SuspendLayout();
    this.gbSubst.SuspendLayout();
    this.gbHiddenSostav.SuspendLayout();
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
    this.contextMenuStripAdd.Items.AddRange(new ToolStripItem[4]
    {
      (ToolStripItem) this.tsmiAdd,
      (ToolStripItem) this.tsmiEdit,
      (ToolStripItem) this.tsmiDelete,
      (ToolStripItem) this.tsmiSeparator
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
    this.toolBarValues.AllowVerticalDock = false;
    this.toolBarValues.DockLine = 3;
    this.toolBarValues.FullMenus = true;
    this.toolBarValues.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarValues.Hidden = false;
    this.toolBarValues.ImageList = this.imagesToolabarImpact;
    this.toolBarValues.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.btnAddValue,
      (ToolbarItemBase) this.btnEditValue,
      (ToolbarItemBase) this.btnDeleteValue
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
    this.panel3.Controls.Add((Control) this.gbSubst);
    this.panel3.Controls.Add((Control) this.gbHiddenSostav);
    this.panel3.Controls.Add((Control) this.tbMaxLevels);
    this.panel3.Controls.Add((Control) this.label2);
    this.panel3.Controls.Add((Control) this.label4);
    this.panel3.Controls.Add((Control) this.bVersionRuleOpen);
    this.panel3.Controls.Add((Control) this.eVersionRule);
    this.panel3.Controls.Add((Control) this.bSelectionView);
    this.panel3.Controls.Add((Control) this.bSelectionAdd);
    this.panel3.Controls.Add((Control) this.bSelectionOpen);
    this.panel3.Controls.Add((Control) this.eSelection);
    this.panel3.Controls.Add((Control) this.eName);
    this.panel3.Controls.Add((Control) this.label3);
    this.panel3.Controls.Add((Control) this.label1);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    componentResourceManager.ApplyResources((object) this.gbSubst, "gbSubst");
    this.gbSubst.Controls.Add((Control) this.rbSubstClient);
    this.gbSubst.Controls.Add((Control) this.rbSubstAll);
    this.gbSubst.Controls.Add((Control) this.rbSubstActual);
    this.gbSubst.Name = "gbSubst";
    this.gbSubst.TabStop = false;
    componentResourceManager.ApplyResources((object) this.rbSubstClient, "rbSubstClient");
    this.rbSubstClient.Checked = true;
    this.rbSubstClient.Name = "rbSubstClient";
    this.rbSubstClient.TabStop = true;
    this.rbSubstClient.UseVisualStyleBackColor = true;
    this.rbSubstClient.CheckedChanged += new EventHandler(this.rbSubstActual_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.rbSubstAll, "rbSubstAll");
    this.rbSubstAll.Name = "rbSubstAll";
    this.rbSubstAll.UseVisualStyleBackColor = true;
    this.rbSubstAll.CheckedChanged += new EventHandler(this.rbSubstActual_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.rbSubstActual, "rbSubstActual");
    this.rbSubstActual.Name = "rbSubstActual";
    this.rbSubstActual.UseVisualStyleBackColor = true;
    this.rbSubstActual.CheckedChanged += new EventHandler(this.rbSubstActual_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.gbHiddenSostav, "gbHiddenSostav");
    this.gbHiddenSostav.Controls.Add((Control) this.rbHiddenClient);
    this.gbHiddenSostav.Controls.Add((Control) this.rbHiddenStru);
    this.gbHiddenSostav.Controls.Add((Control) this.rbHiddenHide);
    this.gbHiddenSostav.Controls.Add((Control) this.rbHiddenShow);
    this.gbHiddenSostav.Name = "gbHiddenSostav";
    this.gbHiddenSostav.TabStop = false;
    componentResourceManager.ApplyResources((object) this.rbHiddenClient, "rbHiddenClient");
    this.rbHiddenClient.Checked = true;
    this.rbHiddenClient.Name = "rbHiddenClient";
    this.rbHiddenClient.TabStop = true;
    this.rbHiddenClient.UseVisualStyleBackColor = true;
    this.rbHiddenClient.CheckedChanged += new EventHandler(this.rbHiddenShow_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.rbHiddenStru, "rbHiddenStru");
    this.rbHiddenStru.Name = "rbHiddenStru";
    this.rbHiddenStru.UseVisualStyleBackColor = true;
    this.rbHiddenStru.CheckedChanged += new EventHandler(this.rbHiddenShow_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.rbHiddenHide, "rbHiddenHide");
    this.rbHiddenHide.Name = "rbHiddenHide";
    this.rbHiddenHide.UseVisualStyleBackColor = true;
    this.rbHiddenHide.CheckedChanged += new EventHandler(this.rbHiddenShow_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.rbHiddenShow, "rbHiddenShow");
    this.rbHiddenShow.Name = "rbHiddenShow";
    this.rbHiddenShow.UseVisualStyleBackColor = true;
    this.rbHiddenShow.CheckedChanged += new EventHandler(this.rbHiddenShow_CheckedChanged);
    componentResourceManager.ApplyResources((object) this.tbMaxLevels, "tbMaxLevels");
    this.tbMaxLevels.Name = "tbMaxLevels";
    this.tbMaxLevels.TextChanged += new EventHandler(this.tbMaxLevels_TextChanged);
    componentResourceManager.ApplyResources((object) this.label2, "label2");
    this.label2.Name = "label2";
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
    componentResourceManager.ApplyResources((object) this.eName, "eName");
    this.eName.Name = "eName";
    this.eName.TextChanged += new EventHandler(this.eName_TextChanged);
    componentResourceManager.ApplyResources((object) this.label3, "label3");
    this.label3.Name = "label3";
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
    this.Name = nameof (VisSchemeEditor);
    this.ShowInTaskbar = false;
    this.Tag = (object) " ";
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.contextMenuStripAdd.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.panel3.PerformLayout();
    this.gbSubst.ResumeLayout(false);
    this.gbSubst.PerformLayout();
    this.gbHiddenSostav.ResumeLayout(false);
    this.gbHiddenSostav.PerformLayout();
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

    public int GetImageIndex()
    {
      int imageIndex = -1;
      if (Statics.IconSrv != null)
        imageIndex = this.CategoryID != 3 || this.TypeID == 0 || this.TypeID == -1 ? Statics.IconSrv.IndexOf(this.CategoryID, this.TypeID) : Statics.IconSrv.IndexOf(this.CategoryID, -1, (object) MetaDataHelper.GetAttributeType(this.TypeID).FieldType);
      return imageIndex;
    }
  }

  private class SearchDirectionItem
  {
    public SearchDirectionItem(SearchDirection direction) => this.Direction = direction;

    public SearchDirection Direction { get; }

    public override string ToString() => EnumTypeHelper.GetCaption((Enum) this.Direction);
  }
}
