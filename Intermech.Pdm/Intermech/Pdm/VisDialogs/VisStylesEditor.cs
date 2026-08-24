// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.VisDialogs.VisStylesEditor
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using DevExpress.IM.Utils;
using DevExpress.IM.XtraEditors.Controls;
using Intermech.Bars;
using Intermech.Client.Core;
using Intermech.Controls;
using Intermech.Docking;
using Intermech.Docking.Rendering;
using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.PropertyEditors;
using OfficePickers.ColorPicker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.VisDialogs;

internal class VisStylesEditor : Form
{
  private VisParentMode _parentMode;
  public int EditorMode;
  public long StyleID;
  public bool IsChanged;
  private TreeNode _tNoPreview;
  private TreeNode _tWithPreview;
  private TreeNode _tRelation;
  public int ObjectTypeID = -1;
  private int _rolesTypeID = -1;
  private VisStyle _style = new VisStyle();
  internal bool IsChangedColumns;
  private AttributesSelectDlg ASF;
  private bool lockChanged;
  private TreeNode rootNode;
  private TreeNode subRoot;
  private TreeNode curNode;
  private int curAttrId = -1;
  private string curAttrName = "";
  private IContainer components;
  private Panel panel1;
  private Panel panel2;
  private Button btnCancel;
  private Button btnApply;
  private Panel panel3;
  private Label label1;
  private Intermech.Bars.ToolBar toolBarValues;
  private ButtonItem btnDeleteValue;
  private ImageList imagesToolabarImpact;
  private ButtonItem btnEditValue;
  private TextBox eName;
  private TreeView treeList1;
  private ToolTipController toolTipController1;
  private ImageList doubleIL;
  private Intermech.Bars.ToolBar tbFirst;
  private ButtonItem btnAddRegular;
  private ButtonItem btnAddPreview;
  private ButtonItem btnAddLink;
  private Panel panelToolBar;
  private Panel parmsPanel;
  private Splitter splitter1;
  private Panel panParmsRelation;
  private Panel panParmsPreview;
  private Panel panParmsSimpleObject;
  private Label label4;
  private LineDashStyleSetupComboBox lineStyleBox;
  private Label label5;
  private Panel panParmsClear;
  private Label label7;
  private ComboBoxColorPicker colorPickLine;
  private Label label6;
  private Label label8;
  private Label label9;
  private ComboBoxColorPicker colorPickHighlight;
  private DockManager dockManager1;
  private DockContainer leftDock;
  private DockContainer rightDock;
  private DockContainer bottomDock;
  private DockContainer topDock;
  private Button btnUpperStr;
  private TextBox tbUpperStr;
  private Label label10;
  private Button btnUpperHint;
  private Label label11;
  private Button btnLowerHint;
  private Button btnLowerStr;
  private TextBox tbLowerStr;
  private Label label12;
  private TextBox tbCentralHint;
  private Label label13;
  private Button btnCentralHint;
  private TextBox tbLowerHint;
  private TextBox tbUpperHint;
  private Label label14;
  private ButtonedEdit beLineAttr;
  private Label label15;
  private MeasureSpinEdit msePreview;
  private Panel panel5;
  private Panel panel4;
  private Label label3;
  private Panel panel6;
  private Label label17;
  private Button btnAddPreviewLower;
  private TextBox tbPreviewLower;
  private Label label16;
  private Button btnAddPreviewUpper;
  private TextBox tbPreviewUpper;
  private Label label19;
  private Label label20;

  public VisParentMode ParentMode
  {
    get => this._parentMode;
    set
    {
      if (value == VisParentMode.ObjCreator)
      {
        this.btnApply.Text = LocalizationHolder.rm.GetString("Pdm_64");
        this.Text = LocalizationHolder.rm.GetString("Pdm_726");
      }
      this._parentMode = value;
    }
  }

  public VisStylesEditor()
  {
    this.InitializeComponent();
    this.Text = VisSchemeConsts.SearchSchemeEditorName;
    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
    this.Size = new Size(workingArea.Width / 100 * 40, workingArea.Height / 100 * 50);
    this.Location = new Point((workingArea.Width - this.Size.Width) / 3, (workingArea.Height - this.Size.Height) / 3);
    this.RuntimeFillControls();
    this.UpdateControls();
    this.ASF = new AttributesSelectDlg(false);
  }

  public static long Execute(int ObjectTypeID, long TemplateObjectID)
  {
    if (ObjectTypeID == 0)
      return 0;
    using (VisStylesEditor visStylesEditor = new VisStylesEditor())
    {
      visStylesEditor.StyleID = TemplateObjectID >= 0L ? TemplateObjectID : 0L;
      visStylesEditor.ParentMode = VisParentMode.ObjCreator;
      visStylesEditor.ObjectTypeID = ObjectTypeID;
      visStylesEditor.LoadObjectData(0);
      visStylesEditor.StyleID = 0L;
      visStylesEditor._style.StyleID = 0L;
      return visStylesEditor.ExecuteForm();
    }
  }

  private long ExecuteForm()
  {
    this.DialogResult = DialogResult.None;
    int num = (int) this.ShowDialog();
    return this.DialogResult != DialogResult.OK ? 0L : this.StyleID;
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

  public void UpdateButtons()
  {
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
    this.FindRoots();
    this.btnEditValue.Enabled = this.curNode != this.rootNode;
    this.btnDeleteValue.Enabled = this.curNode != this.rootNode;
  }

  public void UpdateControls()
  {
    this.UpdateButtons();
    this.lockChanged = true;
    try
    {
      VisStyleNode tag = this.curNode?.Tag as VisStyleNode;
      if (this.subRoot == null || tag == null)
        this.panParmsClear.BringToFront();
      else if (this.rootNode == this._tNoPreview)
      {
        this.panParmsSimpleObject.BringToFront();
        if (!(tag?.Data is ObjNodeData data))
          return;
        this.tbUpperStr.Text = data.UpperStr;
        this.tbUpperHint.Text = data.UpperHint;
        this.tbCentralHint.Text = data.MainHint;
        this.tbLowerHint.Text = data.LowerHint;
        this.tbLowerStr.Text = data.LowerStr;
      }
      else if (this.rootNode == this._tWithPreview)
      {
        this.panParmsPreview.BringToFront();
        if (!(tag?.Data is PreviewNodeData data))
          return;
        this.msePreview.Value = (Decimal) data.PreviewScale;
        this.tbPreviewUpper.Text = data.UpperHint;
        this.tbPreviewLower.Text = data.LowerHint;
      }
      else
      {
        if (this.rootNode != this._tRelation)
          return;
        this.panParmsRelation.BringToFront();
        if (!(tag?.Data is LinkNodeData data))
          return;
        this.lineStyleBox.SelectedLineDashStyle = data.DStyle;
        this.beLineAttr.Value = data.AttrName;
        this.colorPickLine.Color = data.LineColor;
        this.colorPickHighlight.Color = data.HighlightColor;
      }
    }
    finally
    {
      this.lockChanged = false;
    }
  }

  private void FindRoots()
  {
    this.curNode = this.treeList1.SelectedNode;
    if (this.curNode == null)
      this.rootNode = this.subRoot = (TreeNode) null;
    else if (this.curNode.Parent == null)
    {
      this.rootNode = this.curNode;
      this.subRoot = (TreeNode) null;
    }
    else if (this.curNode.Tag is VisStyleNode)
    {
      this.subRoot = this.curNode;
      this.rootNode = this.curNode.Parent;
    }
    else
    {
      this.subRoot = this.curNode.Parent;
      this.rootNode = this.subRoot.Parent;
    }
  }

  public void LoadObjectData(int AEditorMode)
  {
    this._style = new VisStyle();
    this.EditorMode = AEditorMode;
    if (this.EditorMode < 0)
      this.EditorMode = 1;
    this.IsChanged = false;
    if (this.StyleID == 0L)
      return;
    this.ClearControls();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this._style.LoadFromObject(sessionKeeper.Session, this.StyleID);
      this._rolesTypeID = sessionKeeper.Session.IdentHelper.RolesTypeID;
      this.FillControls(sessionKeeper.Session, this._style);
    }
    this.IsChanged = false;
    this.UpdateControls();
  }

  public void SaveObjectData()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this.StyleID == 0L)
      {
        IDBObject dbObject = sessionKeeper.Session.GetObjectCollection(this.ObjectTypeID).Create();
        this._style.StyleID = dbObject.ObjectID;
        this._style.SaveToObject(sessionKeeper.Session);
        dbObject.CommitCreation(true);
        this._style.StyleID = this.StyleID = dbObject.ObjectID;
      }
      else
      {
        this._style.SaveToObject(sessionKeeper.Session);
        this.IsChanged = false;
      }
    }
    this.UpdateControls();
  }

  private void FillControls(IUserSession session, VisStyle scheme)
  {
    if (scheme == null)
    {
      this.ClearControls();
    }
    else
    {
      this.eName.Text = scheme.Name;
      this.FillTreeList(scheme);
    }
  }

  private void FillTreeList(VisStyle style)
  {
    this.treeList1.BeginUpdate();
    this._tNoPreview.Nodes.Clear();
    this._tWithPreview.Nodes.Clear();
    this._tRelation.Nodes.Clear();
    foreach (VisStyleNode styleNode in style.StyleNodes)
    {
      TreeNode parent = (TreeNode) null;
      int num = 4;
      switch (styleNode.Kind)
      {
        case StyleKind.CommonObject:
          parent = this.AddNode(this._tNoPreview, styleNode.Name, (object) styleNode, 4, 0);
          break;
        case StyleKind.ObjPreview:
          parent = this.AddNode(this._tWithPreview, styleNode.Name, (object) styleNode, 4, 0);
          break;
        case StyleKind.Relation:
          parent = this.AddNode(this._tRelation, styleNode.Name, (object) styleNode, 6, 0);
          num = 6;
          break;
      }
      foreach (GlobalType cat in styleNode.CatList)
        this.AddNode(parent, cat.TypeName, (object) new VisStylesEditor.NodeObject(num, cat.TypeID, (object) cat), num, cat.TypeID);
    }
    this.treeList1.EndUpdate();
    this._tNoPreview.Expand();
    this._tWithPreview.Expand();
    this._tRelation.Expand();
  }

  private TreeNode CreateRootNode(string text, int category)
  {
    return this.AddNode((TreeNode) null, text, (object) null, category, 0);
  }

  private void SetTreeListFirst()
  {
    this.treeList1.BeginUpdate();
    this.treeList1.Nodes.Clear();
    this._tNoPreview = this.CreateRootNode(VisSchemeConsts.StylesWithoutPreview, 4);
    this._tWithPreview = this.CreateRootNode(VisSchemeConsts.StylesWithPreview, 4);
    this._tRelation = this.CreateRootNode(VisSchemeConsts.StylesRelation, 6);
    this.treeList1.EndUpdate();
  }

  private void ClearControls()
  {
    this.eName.Text = string.Empty;
    this.SetTreeListFirst();
  }

  private void eName_TextChanged(object sender, EventArgs e)
  {
    this._style.Name = this.eName.Text;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void treeList1_AfterSelect(object sender, TreeViewEventArgs e) => this.UpdateControls();

  private bool ShowObjectTypesDialog(bool Multiselect)
  {
    VisStyleNode tag1 = this.subRoot.Tag as VisStyleNode;
    SelectorForm selectorForm = new SelectorForm(typeof (ObjectTypesFolder), VisSchemeConsts.VisObjectTypes, typeof (ObjectTypeFolder), Multiselect);
    selectorForm.SelectFocusedWhenNothingMultiselected = false;
    HashSet<int> prevTypes = new HashSet<int>();
    tag1.CatList.ForEach((Action<GlobalType>) (gt => prevTypes.Add(gt.TypeID)));
    ArrayList idList = new ArrayList();
    foreach (GlobalType cat in tag1.CatList)
      idList.Add((object) cat.TypeID);
    selectorForm.InitSelectionAsType(idList, (ArrayList) null);
    bool flag = false;
    if (selectorForm.ShowDialog() == DialogResult.OK)
    {
      HashSet<int> intSet = new HashSet<int>();
      foreach (int id in selectorForm.IDList)
        intSet.Add(id);
      if (!intSet.Equals((object) prevTypes))
      {
        this.treeList1.BeginUpdate();
        this.subRoot.Nodes.Clear();
        tag1.CatList.Clear();
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          int num = 4;
          foreach (int id in intSet)
          {
            GlobalType tag2 = new GlobalType(id, num, sessionKeeper.Session);
            tag1.CatList.Add(tag2);
            this.AddNode(this.subRoot, tag2.TypeName, (object) new VisStylesEditor.NodeObject(num, tag2.TypeID, (object) tag2), num, tag2.TypeID);
          }
        }
        this.treeList1.EndUpdate();
        flag = true;
      }
    }
    return flag;
  }

  private bool ShowRelationTypesDialog(bool Multiselect)
  {
    if (this.subRoot == null)
      return false;
    VisStyleNode tag1 = this.subRoot.Tag as VisStyleNode;
    SelectorForm selectorForm = new SelectorForm(typeof (RelationTypesFolder), VisSchemeConsts.VisRelationTypes, typeof (RelationTypeFolder), Multiselect);
    selectorForm.SelectFocusedWhenNothingMultiselected = false;
    HashSet<int> prevTypes = new HashSet<int>();
    tag1.CatList.ForEach((Action<GlobalType>) (gt => prevTypes.Add(gt.TypeID)));
    ArrayList idList = new ArrayList();
    foreach (GlobalType cat in tag1.CatList)
      idList.Add((object) cat.TypeID);
    selectorForm.InitSelectionAsType(idList, (ArrayList) null);
    bool flag = false;
    if (selectorForm.ShowDialog() == DialogResult.OK)
    {
      HashSet<int> intSet = new HashSet<int>();
      foreach (int id in selectorForm.IDList)
        intSet.Add(id);
      if (!intSet.Equals((object) prevTypes))
      {
        this.treeList1.BeginUpdate();
        this.subRoot.Nodes.Clear();
        tag1.CatList.Clear();
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          int num = 6;
          foreach (int id in intSet)
          {
            GlobalType tag2 = new GlobalType(id, num, sessionKeeper.Session);
            tag1.CatList.Add(tag2);
            this.AddNode(this.subRoot, tag2.TypeName, (object) new VisStylesEditor.NodeObject(num, tag2.TypeID, (object) tag2), num, tag2.TypeID);
          }
        }
        this.treeList1.EndUpdate();
        flag = true;
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

  private void btnAddRegular_Click(object sender, EventArgs e)
  {
    using (UserPrompt userPrompt = new UserPrompt())
    {
      string name = userPrompt.Execute(VisSchemeConsts.VisCreateStyle1, VisSchemeConsts.VisCreateStylePrompt);
      if (name == "")
        return;
      VisStyleNode tag = new VisStyleNode(StyleKind.CommonObject, name);
      this._style.StyleNodes.Add(tag);
      this.subRoot = this.AddNode(this._tNoPreview, tag.Name, (object) tag, 4, 0);
      this.IsChanged = true;
      this.treeList1.SelectedNode = this.subRoot;
    }
  }

  private void btnAddPreview_Click(object sender, EventArgs e)
  {
    using (UserPrompt userPrompt = new UserPrompt())
    {
      string name = userPrompt.Execute(VisSchemeConsts.VisCreateStyle2, VisSchemeConsts.VisCreateStylePrompt);
      if (name == "")
        return;
      VisStyleNode tag = new VisStyleNode(StyleKind.ObjPreview, name);
      this._style.StyleNodes.Add(tag);
      this.subRoot = this.AddNode(this._tWithPreview, tag.Name, (object) tag, 4, 0);
      this.IsChanged = true;
      this.treeList1.SelectedNode = this.subRoot;
    }
  }

  private void btnAddLink_Click(object sender, EventArgs e)
  {
    using (UserPrompt userPrompt = new UserPrompt())
    {
      string name = userPrompt.Execute(VisSchemeConsts.VisCreateStyle3, VisSchemeConsts.VisCreateStylePrompt);
      if (name == "")
        return;
      VisStyleNode tag = new VisStyleNode(StyleKind.Relation, name);
      this._style.StyleNodes.Add(tag);
      this.subRoot = this.AddNode(this._tRelation, tag.Name, (object) tag, 6, 0);
      this.IsChanged = true;
      this.treeList1.SelectedNode = this.subRoot;
    }
  }

  private void btnEditValue_Click(object sender, EventArgs e)
  {
    this.IsChanged = this.rootNode != this._tRelation ? this.ShowObjectTypesDialog(true) : this.ShowRelationTypesDialog(true);
    this.UpdateControls();
  }

  private void btnDeleteValue_Click(object sender, EventArgs e)
  {
    VisStyleNode tag1 = this.subRoot.Tag as VisStyleNode;
    if (this.curNode == this.subRoot)
    {
      this.treeList1.Nodes.Remove(this.curNode);
      this._style.StyleNodes.Remove(tag1);
      this.IsChanged = true;
      this.UpdateControls();
    }
    else
    {
      GlobalType tag2 = this.curNode.Tag as GlobalType;
      tag1.CatList.Remove(tag2);
      this.treeList1.Nodes.Remove(this.curNode);
      this.IsChanged = true;
      this.UpdateControls();
    }
  }

  private void measureSpinEdit1_EditValueChanged(object sender, EventArgs e)
  {
    if (this.lockChanged || !(this.subRoot?.Tag is VisStyleNode tag))
      return;
    PreviewNodeData data = (PreviewNodeData) tag.Data;
    data.PreviewScale = Convert.ToInt32(this.msePreview.Value);
    if (data.PreviewScale < 50)
      data.PreviewScale = 50;
    if (data.PreviewScale > 150)
      data.PreviewScale = 150;
    if ((Decimal) data.PreviewScale != this.msePreview.Value)
    {
      this.lockChanged = true;
      try
      {
        this.msePreview.Value = (Decimal) data.PreviewScale;
      }
      finally
      {
        this.lockChanged = false;
      }
    }
    this.IsChanged = true;
    this.UpdateControls();
  }

  private bool ChooseAttr()
  {
    if (this.ASF.ShowDialog() != DialogResult.OK || this.ASF.SelectedAttributesGuid.Count <= 0)
      return false;
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(this.ASF.SelectedAttributesGuid[0]);
    this.curAttrId = attributeType.AttributeID;
    this.curAttrName = attributeType.Name;
    return true;
  }

  private void btnEdAttr_ButtonClick(object sender, EventArgs e)
  {
    if (this.lockChanged || !(sender is ButtonedEdit buttonedEdit) || this.ASF.ShowDialog() != DialogResult.OK || this.ASF.SelectedAttributesGuid.Count <= 0)
      return;
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(this.ASF.SelectedAttributesGuid[0]);
    this.curAttrId = attributeType.AttributeID;
    this.curAttrName = attributeType.Name;
    buttonedEdit.Value = this.curAttrName;
    if (buttonedEdit != this.beLineAttr || this.subRoot?.Tag == null || !(this.subRoot.Tag is VisStyleNode tag))
      return;
    ((LinkNodeData) tag.Data).AttrName = buttonedEdit.Value;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void InsertCurAttr(TextBox tb)
  {
    if (this.lockChanged || tb.SelectionLength > 0)
      return;
    int selectionStart = tb.SelectionStart;
    string str = tb.Text;
    int startIndex1 = 0;
    bool flag = true;
    while (true)
    {
      int startIndex2 = str.IndexOf('{', startIndex1);
      if (startIndex2 != -1)
      {
        int num = str.IndexOf('}', startIndex2);
        if (num == -1)
        {
          str += "}";
          num = str.IndexOf('}', startIndex2);
        }
        if (selectionStart <= startIndex2 || selectionStart >= num)
          startIndex1 = num;
        else
          break;
      }
      else
        goto label_8;
    }
    flag = false;
label_8:
    if (flag)
      str = str.Insert(selectionStart, $"{{{this.curAttrName}}}");
    if (tb.Text != str)
      tb.Text = str;
    this.UpdateSubRoot(tb);
    this.IsChanged = true;
    this.UpdateButtons();
  }

  private void UpdateSubRoot(TextBox tb)
  {
    if (this.lockChanged || tb == null || this.subRoot == null || this.subRoot.Tag == null || !(this.subRoot.Tag is VisStyleNode tag))
      return;
    if (tb == this.tbUpperStr)
    {
      if (((ObjNodeData) tag.Data).UpperStr != tb.Text)
      {
        ((ObjNodeData) tag.Data).UpperStr = tb.Text;
        this.IsChanged = true;
      }
    }
    else if (tb == this.tbUpperHint)
    {
      if (((ObjNodeData) tag.Data).UpperHint != tb.Text)
      {
        ((ObjNodeData) tag.Data).UpperHint = tb.Text;
        this.IsChanged = true;
      }
    }
    else if (tb == this.tbCentralHint)
    {
      if (((ObjNodeData) tag.Data).MainHint != tb.Text)
      {
        ((ObjNodeData) tag.Data).MainHint = tb.Text;
        this.IsChanged = true;
      }
    }
    else if (tb == this.tbLowerHint)
    {
      if (((ObjNodeData) tag.Data).LowerHint != tb.Text)
      {
        ((ObjNodeData) tag.Data).LowerHint = tb.Text;
        this.IsChanged = true;
      }
    }
    else if (tb == this.tbLowerStr)
    {
      if (((ObjNodeData) tag.Data).LowerStr != tb.Text)
      {
        ((ObjNodeData) tag.Data).LowerStr = tb.Text;
        this.IsChanged = true;
      }
    }
    else if (tb == this.tbPreviewUpper)
    {
      if (((PreviewNodeData) tag.Data).UpperHint != tb.Text)
      {
        ((PreviewNodeData) tag.Data).UpperHint = tb.Text;
        this.IsChanged = true;
      }
    }
    else if (tb == this.tbPreviewLower && ((PreviewNodeData) tag.Data).LowerHint != tb.Text)
    {
      ((PreviewNodeData) tag.Data).LowerHint = tb.Text;
      this.IsChanged = true;
    }
    if (!this.IsChanged)
      return;
    this.UpdateControls();
  }

  private void btnUpperStr_Click(object sender, EventArgs e)
  {
    if (!this.ChooseAttr())
      return;
    this.InsertCurAttr(this.tbUpperStr);
  }

  private void btnUpperHint_Click(object sender, EventArgs e)
  {
    if (!this.ChooseAttr())
      return;
    this.InsertCurAttr(this.tbUpperHint);
  }

  private void btnCentralHint_Click(object sender, EventArgs e)
  {
    if (!this.ChooseAttr())
      return;
    this.InsertCurAttr(this.tbCentralHint);
  }

  private void btnLowerStr_Click(object sender, EventArgs e)
  {
    if (!this.ChooseAttr())
      return;
    this.InsertCurAttr(this.tbLowerStr);
  }

  private void btnLowerHint_Click(object sender, EventArgs e)
  {
    if (!this.ChooseAttr())
      return;
    this.InsertCurAttr(this.tbLowerHint);
  }

  private void btnAddPreviewUpper_Click(object sender, EventArgs e)
  {
    if (!this.ChooseAttr())
      return;
    this.InsertCurAttr(this.tbPreviewUpper);
  }

  private void btnAddPreviewLower_Click(object sender, EventArgs e)
  {
    if (!this.ChooseAttr())
      return;
    this.InsertCurAttr(this.tbPreviewLower);
  }

  private void tbPreviewUpper_TextChanged(object sender, EventArgs e)
  {
    this.UpdateSubRoot(sender as TextBox);
  }

  private void colorPickLine_SelectedColorChanged(object sender, EventArgs e)
  {
    if (this.lockChanged || !(this.subRoot?.Tag is VisStyleNode tag))
      return;
    ((LinkNodeData) tag.Data).LineColor = this.colorPickLine.Color;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void colorPickHighlight_SelectedColorChanged(object sender, EventArgs e)
  {
    if (this.lockChanged || !(this.subRoot?.Tag is VisStyleNode tag))
      return;
    ((LinkNodeData) tag.Data).HighlightColor = this.colorPickHighlight.Color;
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void msePreview_Properties_EditValueChanged(object sender, EventArgs e)
  {
    if (this.lockChanged || !(this.subRoot?.Tag is VisStyleNode tag))
      return;
    ((PreviewNodeData) tag.Data).PreviewScale = Convert.ToInt32(this.msePreview.Value);
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void msePreview_TextChanged(object sender, EventArgs e)
  {
    if (this.lockChanged || !(this.subRoot?.Tag is VisStyleNode tag))
      return;
    PreviewNodeData data = (PreviewNodeData) tag.Data;
    data.PreviewScale = Convert.ToInt32(this.msePreview.Value);
    if (data.PreviewScale < 50)
      data.PreviewScale = 50;
    if (data.PreviewScale > 150)
      data.PreviewScale = 150;
    if ((Decimal) data.PreviewScale != this.msePreview.Value)
    {
      this.lockChanged = true;
      try
      {
        this.msePreview.Value = (Decimal) data.PreviewScale;
      }
      finally
      {
        this.lockChanged = false;
      }
    }
    this.IsChanged = true;
    this.UpdateButtons();
  }

  private void lineStyleBox_OnLineDashStyleSelected(
    LineDashStyleSetupComboBox sender,
    DashStyle selectedDashStyle)
  {
    if (this.lockChanged || !(this.subRoot?.Tag is VisStyleNode tag))
      return;
    ((LinkNodeData) tag.Data).DStyle = this.lineStyleBox.SelectedLineDashStyle;
    this.IsChanged = true;
    this.UpdateControls();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    if (this.ASF != null)
    {
      this.ASF.Dispose();
      this.ASF = (AttributesSelectDlg) null;
    }
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (VisStylesEditor));
    this.panel1 = new Panel();
    this.btnCancel = new Button();
    this.btnApply = new Button();
    this.panel2 = new Panel();
    this.splitter1 = new Splitter();
    this.parmsPanel = new Panel();
    this.panParmsSimpleObject = new Panel();
    this.panel6 = new Panel();
    this.label17 = new Label();
    this.label14 = new Label();
    this.tbCentralHint = new TextBox();
    this.label13 = new Label();
    this.btnCentralHint = new Button();
    this.imagesToolabarImpact = new ImageList(this.components);
    this.tbLowerHint = new TextBox();
    this.tbUpperHint = new TextBox();
    this.label11 = new Label();
    this.btnLowerHint = new Button();
    this.btnLowerStr = new Button();
    this.tbLowerStr = new TextBox();
    this.label12 = new Label();
    this.label10 = new Label();
    this.btnUpperHint = new Button();
    this.btnUpperStr = new Button();
    this.tbUpperStr = new TextBox();
    this.label6 = new Label();
    this.panParmsPreview = new Panel();
    this.label20 = new Label();
    this.btnAddPreviewLower = new Button();
    this.tbPreviewLower = new TextBox();
    this.label16 = new Label();
    this.btnAddPreviewUpper = new Button();
    this.tbPreviewUpper = new TextBox();
    this.label19 = new Label();
    this.panel4 = new Panel();
    this.label3 = new Label();
    this.label15 = new Label();
    this.msePreview = new MeasureSpinEdit();
    this.panParmsRelation = new Panel();
    this.panel5 = new Panel();
    this.label4 = new Label();
    this.beLineAttr = new ButtonedEdit();
    this.label9 = new Label();
    this.colorPickHighlight = new ComboBoxColorPicker();
    this.label8 = new Label();
    this.label7 = new Label();
    this.colorPickLine = new ComboBoxColorPicker();
    this.label5 = new Label();
    this.lineStyleBox = new LineDashStyleSetupComboBox();
    this.panParmsClear = new Panel();
    this.treeList1 = new TreeView();
    this.panelToolBar = new Panel();
    this.tbFirst = new Intermech.Bars.ToolBar();
    this.doubleIL = new ImageList(this.components);
    this.btnAddRegular = new ButtonItem();
    this.btnAddPreview = new ButtonItem();
    this.btnAddLink = new ButtonItem();
    this.toolBarValues = new Intermech.Bars.ToolBar();
    this.btnEditValue = new ButtonItem();
    this.btnDeleteValue = new ButtonItem();
    this.panel3 = new Panel();
    this.eName = new TextBox();
    this.label1 = new Label();
    this.toolTipController1 = new ToolTipController(this.components);
    this.dockManager1 = new DockManager();
    this.leftDock = new DockContainer();
    this.rightDock = new DockContainer();
    this.bottomDock = new DockContainer();
    this.topDock = new DockContainer();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.parmsPanel.SuspendLayout();
    this.panParmsSimpleObject.SuspendLayout();
    this.panel6.SuspendLayout();
    this.panParmsPreview.SuspendLayout();
    this.panel4.SuspendLayout();
    this.msePreview.Properties.BeginInit();
    this.panParmsRelation.SuspendLayout();
    this.panel5.SuspendLayout();
    this.panelToolBar.SuspendLayout();
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
    this.panel2.Controls.Add((Control) this.splitter1);
    this.panel2.Controls.Add((Control) this.parmsPanel);
    this.panel2.Controls.Add((Control) this.treeList1);
    this.panel2.Controls.Add((Control) this.panelToolBar);
    this.panel2.Controls.Add((Control) this.panel3);
    componentResourceManager.ApplyResources((object) this.panel2, "panel2");
    this.panel2.Name = "panel2";
    componentResourceManager.ApplyResources((object) this.splitter1, "splitter1");
    this.splitter1.Name = "splitter1";
    this.splitter1.TabStop = false;
    this.parmsPanel.Controls.Add((Control) this.panParmsSimpleObject);
    this.parmsPanel.Controls.Add((Control) this.panParmsPreview);
    this.parmsPanel.Controls.Add((Control) this.panParmsRelation);
    this.parmsPanel.Controls.Add((Control) this.panParmsClear);
    componentResourceManager.ApplyResources((object) this.parmsPanel, "parmsPanel");
    this.parmsPanel.Name = "parmsPanel";
    this.panParmsSimpleObject.Controls.Add((Control) this.panel6);
    this.panParmsSimpleObject.Controls.Add((Control) this.label14);
    this.panParmsSimpleObject.Controls.Add((Control) this.tbCentralHint);
    this.panParmsSimpleObject.Controls.Add((Control) this.label13);
    this.panParmsSimpleObject.Controls.Add((Control) this.btnCentralHint);
    this.panParmsSimpleObject.Controls.Add((Control) this.tbLowerHint);
    this.panParmsSimpleObject.Controls.Add((Control) this.tbUpperHint);
    this.panParmsSimpleObject.Controls.Add((Control) this.label11);
    this.panParmsSimpleObject.Controls.Add((Control) this.btnLowerHint);
    this.panParmsSimpleObject.Controls.Add((Control) this.btnLowerStr);
    this.panParmsSimpleObject.Controls.Add((Control) this.tbLowerStr);
    this.panParmsSimpleObject.Controls.Add((Control) this.label12);
    this.panParmsSimpleObject.Controls.Add((Control) this.label10);
    this.panParmsSimpleObject.Controls.Add((Control) this.btnUpperHint);
    this.panParmsSimpleObject.Controls.Add((Control) this.btnUpperStr);
    this.panParmsSimpleObject.Controls.Add((Control) this.tbUpperStr);
    this.panParmsSimpleObject.Controls.Add((Control) this.label6);
    componentResourceManager.ApplyResources((object) this.panParmsSimpleObject, "panParmsSimpleObject");
    this.panParmsSimpleObject.Name = "panParmsSimpleObject";
    this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
    this.panel6.Controls.Add((Control) this.label17);
    componentResourceManager.ApplyResources((object) this.panel6, "panel6");
    this.panel6.Name = "panel6";
    componentResourceManager.ApplyResources((object) this.label17, "label17");
    this.label17.Name = "label17";
    componentResourceManager.ApplyResources((object) this.label14, "label14");
    this.label14.Name = "label14";
    this.tbCentralHint.AcceptsReturn = true;
    componentResourceManager.ApplyResources((object) this.tbCentralHint, "tbCentralHint");
    this.tbCentralHint.Name = "tbCentralHint";
    this.tbCentralHint.TextChanged += new EventHandler(this.tbPreviewUpper_TextChanged);
    componentResourceManager.ApplyResources((object) this.label13, "label13");
    this.label13.Name = "label13";
    componentResourceManager.ApplyResources((object) this.btnCentralHint, "btnCentralHint");
    this.btnCentralHint.ImageList = this.imagesToolabarImpact;
    this.btnCentralHint.Name = "btnCentralHint";
    this.toolTipController1.SetToolTip((Control) this.btnCentralHint, "Вставить атрибут в центральную подсказку");
    this.btnCentralHint.UseVisualStyleBackColor = true;
    this.btnCentralHint.Click += new EventHandler(this.btnCentralHint_Click);
    this.imagesToolabarImpact.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imagesToolabarImpact.ImageStream");
    this.imagesToolabarImpact.TransparentColor = Color.Transparent;
    this.imagesToolabarImpact.Images.SetKeyName(0, "add.png");
    this.imagesToolabarImpact.Images.SetKeyName(1, "delete.png");
    this.imagesToolabarImpact.Images.SetKeyName(2, "edit.png");
    this.imagesToolabarImpact.Images.SetKeyName(3, "attr.png");
    this.tbLowerHint.AcceptsReturn = true;
    componentResourceManager.ApplyResources((object) this.tbLowerHint, "tbLowerHint");
    this.tbLowerHint.Name = "tbLowerHint";
    this.tbLowerHint.TextChanged += new EventHandler(this.tbPreviewUpper_TextChanged);
    this.tbUpperHint.AcceptsReturn = true;
    componentResourceManager.ApplyResources((object) this.tbUpperHint, "tbUpperHint");
    this.tbUpperHint.Name = "tbUpperHint";
    this.tbUpperHint.TextChanged += new EventHandler(this.tbPreviewUpper_TextChanged);
    componentResourceManager.ApplyResources((object) this.label11, "label11");
    this.label11.Name = "label11";
    componentResourceManager.ApplyResources((object) this.btnLowerHint, "btnLowerHint");
    this.btnLowerHint.ImageList = this.imagesToolabarImpact;
    this.btnLowerHint.Name = "btnLowerHint";
    this.toolTipController1.SetToolTip((Control) this.btnLowerHint, "Вставить атрибут в нижнюю подсказку");
    this.btnLowerHint.UseVisualStyleBackColor = true;
    this.btnLowerHint.Click += new EventHandler(this.btnLowerHint_Click);
    componentResourceManager.ApplyResources((object) this.btnLowerStr, "btnLowerStr");
    this.btnLowerStr.ImageList = this.imagesToolabarImpact;
    this.btnLowerStr.Name = "btnLowerStr";
    this.toolTipController1.SetToolTip((Control) this.btnLowerStr, "Вставить атрибут в нижнюю строку");
    this.btnLowerStr.UseVisualStyleBackColor = true;
    this.btnLowerStr.Click += new EventHandler(this.btnLowerStr_Click);
    componentResourceManager.ApplyResources((object) this.tbLowerStr, "tbLowerStr");
    this.tbLowerStr.Name = "tbLowerStr";
    this.tbLowerStr.TextChanged += new EventHandler(this.tbPreviewUpper_TextChanged);
    componentResourceManager.ApplyResources((object) this.label12, "label12");
    this.label12.Name = "label12";
    componentResourceManager.ApplyResources((object) this.label10, "label10");
    this.label10.Name = "label10";
    componentResourceManager.ApplyResources((object) this.btnUpperHint, "btnUpperHint");
    this.btnUpperHint.ImageList = this.imagesToolabarImpact;
    this.btnUpperHint.Name = "btnUpperHint";
    this.toolTipController1.SetToolTip((Control) this.btnUpperHint, "Вставить атрибут в верхнюю подсказку");
    this.btnUpperHint.UseVisualStyleBackColor = true;
    this.btnUpperHint.Click += new EventHandler(this.btnUpperHint_Click);
    componentResourceManager.ApplyResources((object) this.btnUpperStr, "btnUpperStr");
    this.btnUpperStr.ImageList = this.imagesToolabarImpact;
    this.btnUpperStr.Name = "btnUpperStr";
    this.toolTipController1.SetToolTip((Control) this.btnUpperStr, "Вставить атрибут в верхнюю строку");
    this.btnUpperStr.UseVisualStyleBackColor = true;
    this.btnUpperStr.Click += new EventHandler(this.btnUpperStr_Click);
    componentResourceManager.ApplyResources((object) this.tbUpperStr, "tbUpperStr");
    this.tbUpperStr.Name = "tbUpperStr";
    this.tbUpperStr.TextChanged += new EventHandler(this.tbPreviewUpper_TextChanged);
    componentResourceManager.ApplyResources((object) this.label6, "label6");
    this.label6.Name = "label6";
    this.panParmsPreview.Controls.Add((Control) this.label20);
    this.panParmsPreview.Controls.Add((Control) this.btnAddPreviewLower);
    this.panParmsPreview.Controls.Add((Control) this.tbPreviewLower);
    this.panParmsPreview.Controls.Add((Control) this.label16);
    this.panParmsPreview.Controls.Add((Control) this.btnAddPreviewUpper);
    this.panParmsPreview.Controls.Add((Control) this.tbPreviewUpper);
    this.panParmsPreview.Controls.Add((Control) this.label19);
    this.panParmsPreview.Controls.Add((Control) this.panel4);
    this.panParmsPreview.Controls.Add((Control) this.label15);
    this.panParmsPreview.Controls.Add((Control) this.msePreview);
    componentResourceManager.ApplyResources((object) this.panParmsPreview, "panParmsPreview");
    this.panParmsPreview.Name = "panParmsPreview";
    componentResourceManager.ApplyResources((object) this.label20, "label20");
    this.label20.Name = "label20";
    componentResourceManager.ApplyResources((object) this.btnAddPreviewLower, "btnAddPreviewLower");
    this.btnAddPreviewLower.ImageList = this.imagesToolabarImpact;
    this.btnAddPreviewLower.Name = "btnAddPreviewLower";
    this.toolTipController1.SetToolTip((Control) this.btnAddPreviewLower, "Вставить атрибут в нижнюю строку");
    this.btnAddPreviewLower.UseVisualStyleBackColor = true;
    this.btnAddPreviewLower.Click += new EventHandler(this.btnAddPreviewLower_Click);
    componentResourceManager.ApplyResources((object) this.tbPreviewLower, "tbPreviewLower");
    this.tbPreviewLower.Name = "tbPreviewLower";
    this.tbPreviewLower.TextChanged += new EventHandler(this.tbPreviewUpper_TextChanged);
    componentResourceManager.ApplyResources((object) this.label16, "label16");
    this.label16.Name = "label16";
    componentResourceManager.ApplyResources((object) this.btnAddPreviewUpper, "btnAddPreviewUpper");
    this.btnAddPreviewUpper.ImageList = this.imagesToolabarImpact;
    this.btnAddPreviewUpper.Name = "btnAddPreviewUpper";
    this.toolTipController1.SetToolTip((Control) this.btnAddPreviewUpper, "Вставить атрибут в верхнюю строку");
    this.btnAddPreviewUpper.UseVisualStyleBackColor = true;
    this.btnAddPreviewUpper.Click += new EventHandler(this.btnAddPreviewUpper_Click);
    componentResourceManager.ApplyResources((object) this.tbPreviewUpper, "tbPreviewUpper");
    this.tbPreviewUpper.Name = "tbPreviewUpper";
    this.tbPreviewUpper.TextChanged += new EventHandler(this.tbPreviewUpper_TextChanged);
    componentResourceManager.ApplyResources((object) this.label19, "label19");
    this.label19.Name = "label19";
    this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
    this.panel4.Controls.Add((Control) this.label3);
    componentResourceManager.ApplyResources((object) this.panel4, "panel4");
    this.panel4.Name = "panel4";
    componentResourceManager.ApplyResources((object) this.label3, "label3");
    this.label3.Name = "label3";
    componentResourceManager.ApplyResources((object) this.label15, "label15");
    this.label15.Name = "label15";
    componentResourceManager.ApplyResources((object) this.msePreview, "msePreview");
    this.msePreview.LastValue = 0.0;
    this.msePreview.Name = "msePreview";
    this.msePreview.Properties.Buttons.AddRange(new EditorButton[1]
    {
      new EditorButton()
    });
    this.msePreview.Properties.Increment = new Decimal(new int[4]
    {
      5,
      0,
      0,
      0
    });
    this.msePreview.Properties.MaxValue = new Decimal(new int[4]
    {
      100,
      0,
      0,
      0
    });
    this.msePreview.Properties.MinValue = new Decimal(new int[4]
    {
      50,
      0,
      0,
      0
    });
    this.msePreview.Properties.UseCtrlIncrement = false;
    this.msePreview.Properties.EditValueChanged += new EventHandler(this.msePreview_Properties_EditValueChanged);
    this.msePreview.EditValueChanged += new EventHandler(this.measureSpinEdit1_EditValueChanged);
    this.msePreview.TextChanged += new EventHandler(this.msePreview_TextChanged);
    this.panParmsRelation.Controls.Add((Control) this.panel5);
    this.panParmsRelation.Controls.Add((Control) this.beLineAttr);
    this.panParmsRelation.Controls.Add((Control) this.label9);
    this.panParmsRelation.Controls.Add((Control) this.colorPickHighlight);
    this.panParmsRelation.Controls.Add((Control) this.label8);
    this.panParmsRelation.Controls.Add((Control) this.label7);
    this.panParmsRelation.Controls.Add((Control) this.colorPickLine);
    this.panParmsRelation.Controls.Add((Control) this.label5);
    this.panParmsRelation.Controls.Add((Control) this.lineStyleBox);
    componentResourceManager.ApplyResources((object) this.panParmsRelation, "panParmsRelation");
    this.panParmsRelation.Name = "panParmsRelation";
    this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
    this.panel5.Controls.Add((Control) this.label4);
    componentResourceManager.ApplyResources((object) this.panel5, "panel5");
    this.panel5.Name = "panel5";
    componentResourceManager.ApplyResources((object) this.label4, "label4");
    this.label4.Name = "label4";
    componentResourceManager.ApplyResources((object) this.beLineAttr, "beLineAttr");
    this.beLineAttr.ButtonImage = (Image) Intermech.Pdm.Properties.Resources.Folder_Search;
    this.beLineAttr.ButtonText = (string) null;
    this.beLineAttr.Caption = "";
    this.beLineAttr.CaptionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.beLineAttr.Hint = "Выбор атрибута";
    this.beLineAttr.Image = (Image) null;
    this.beLineAttr.Name = "beLineAttr";
    this.beLineAttr.ReadOnly = true;
    this.toolTipController1.SetToolTip((Control) this.beLineAttr, "Нажмите на кнопку справа для выбора атрибута");
    this.beLineAttr.Value = "Количество";
    this.beLineAttr.ButtonClick += new EventHandler(this.btnEdAttr_ButtonClick);
    componentResourceManager.ApplyResources((object) this.label9, "label9");
    this.label9.Name = "label9";
    componentResourceManager.ApplyResources((object) this.colorPickHighlight, "colorPickHighlight");
    this.colorPickHighlight.Color = Color.Gold;
    this.colorPickHighlight.DrawMode = DrawMode.OwnerDrawFixed;
    this.colorPickHighlight.DropDownHeight = 1;
    this.colorPickHighlight.DropDownStyle = ComboBoxStyle.DropDownList;
    this.colorPickHighlight.DropDownWidth = 1;
    this.colorPickHighlight.FormattingEnabled = true;
    this.colorPickHighlight.Items.AddRange(new object[25]
    {
      (object) componentResourceManager.GetString("colorPickHighlight.Items"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items1"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items2"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items3"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items4"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items5"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items6"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items7"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items8"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items9"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items10"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items11"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items12"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items13"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items14"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items15"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items16"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items17"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items18"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items19"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items20"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items21"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items22"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items23"),
      (object) componentResourceManager.GetString("colorPickHighlight.Items24")
    });
    this.colorPickHighlight.Name = "colorPickHighlight";
    this.colorPickHighlight.SelectedColorChanged += new EventHandler(this.colorPickHighlight_SelectedColorChanged);
    componentResourceManager.ApplyResources((object) this.label8, "label8");
    this.label8.Name = "label8";
    componentResourceManager.ApplyResources((object) this.label7, "label7");
    this.label7.Name = "label7";
    componentResourceManager.ApplyResources((object) this.colorPickLine, "colorPickLine");
    this.colorPickLine.Color = Color.Gray;
    this.colorPickLine.DrawMode = DrawMode.OwnerDrawFixed;
    this.colorPickLine.DropDownHeight = 1;
    this.colorPickLine.DropDownStyle = ComboBoxStyle.DropDownList;
    this.colorPickLine.DropDownWidth = 1;
    this.colorPickLine.FormattingEnabled = true;
    this.colorPickLine.Items.AddRange(new object[24]
    {
      (object) componentResourceManager.GetString("colorPickLine.Items"),
      (object) componentResourceManager.GetString("colorPickLine.Items1"),
      (object) componentResourceManager.GetString("colorPickLine.Items2"),
      (object) componentResourceManager.GetString("colorPickLine.Items3"),
      (object) componentResourceManager.GetString("colorPickLine.Items4"),
      (object) componentResourceManager.GetString("colorPickLine.Items5"),
      (object) componentResourceManager.GetString("colorPickLine.Items6"),
      (object) componentResourceManager.GetString("colorPickLine.Items7"),
      (object) componentResourceManager.GetString("colorPickLine.Items8"),
      (object) componentResourceManager.GetString("colorPickLine.Items9"),
      (object) componentResourceManager.GetString("colorPickLine.Items10"),
      (object) componentResourceManager.GetString("colorPickLine.Items11"),
      (object) componentResourceManager.GetString("colorPickLine.Items12"),
      (object) componentResourceManager.GetString("colorPickLine.Items13"),
      (object) componentResourceManager.GetString("colorPickLine.Items14"),
      (object) componentResourceManager.GetString("colorPickLine.Items15"),
      (object) componentResourceManager.GetString("colorPickLine.Items16"),
      (object) componentResourceManager.GetString("colorPickLine.Items17"),
      (object) componentResourceManager.GetString("colorPickLine.Items18"),
      (object) componentResourceManager.GetString("colorPickLine.Items19"),
      (object) componentResourceManager.GetString("colorPickLine.Items20"),
      (object) componentResourceManager.GetString("colorPickLine.Items21"),
      (object) componentResourceManager.GetString("colorPickLine.Items22"),
      (object) componentResourceManager.GetString("colorPickLine.Items23")
    });
    this.colorPickLine.Name = "colorPickLine";
    this.colorPickLine.SelectedColorChanged += new EventHandler(this.colorPickLine_SelectedColorChanged);
    componentResourceManager.ApplyResources((object) this.label5, "label5");
    this.label5.Name = "label5";
    componentResourceManager.ApplyResources((object) this.lineStyleBox, "lineStyleBox");
    this.lineStyleBox.DrawMode = DrawMode.OwnerDrawFixed;
    this.lineStyleBox.DropDownStyle = ComboBoxStyle.DropDownList;
    this.lineStyleBox.ImageList = (ImageList) null;
    this.lineStyleBox.Name = "lineStyleBox";
    this.lineStyleBox.RemarksColor = SystemColors.GrayText;
    this.lineStyleBox.ShowItemRemarks = false;
    this.lineStyleBox.OnLineDashStyleSelected += new LineDashStyleSetupComboBox.LineDashStyleSelectedDelegate(this.lineStyleBox_OnLineDashStyleSelected);
    componentResourceManager.ApplyResources((object) this.panParmsClear, "panParmsClear");
    this.panParmsClear.Name = "panParmsClear";
    componentResourceManager.ApplyResources((object) this.treeList1, "treeList1");
    this.treeList1.HideSelection = false;
    this.treeList1.Name = "treeList1";
    this.treeList1.ShowRootLines = false;
    this.treeList1.AfterSelect += new TreeViewEventHandler(this.treeList1_AfterSelect);
    this.panelToolBar.Controls.Add((Control) this.tbFirst);
    this.panelToolBar.Controls.Add((Control) this.toolBarValues);
    componentResourceManager.ApplyResources((object) this.panelToolBar, "panelToolBar");
    this.panelToolBar.Name = "panelToolBar";
    this.tbFirst.AllowVerticalDock = false;
    componentResourceManager.ApplyResources((object) this.tbFirst, "tbFirst");
    this.tbFirst.DockLine = 3;
    this.tbFirst.FullMenus = true;
    this.tbFirst.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.tbFirst.Hidden = false;
    this.tbFirst.ImageList = this.doubleIL;
    this.tbFirst.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.btnAddRegular,
      (ToolbarItemBase) this.btnAddPreview,
      (ToolbarItemBase) this.btnAddLink
    });
    this.tbFirst.MinimumFloatingSize = new Size(250, 30);
    this.tbFirst.Movable = false;
    this.tbFirst.Name = "tbFirst";
    this.tbFirst.Overflow = ToolBarOverflow.Wrap;
    this.tbFirst.Resizable = false;
    this.tbFirst.Stretch = true;
    this.doubleIL.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("doubleIL.ImageStream");
    this.doubleIL.TransparentColor = Color.Transparent;
    this.doubleIL.Images.SetKeyName(0, "CreatePreview.png");
    this.doubleIL.Images.SetKeyName(1, "CreateRegular.png");
    this.doubleIL.Images.SetKeyName(2, "CreateLink.png");
    componentResourceManager.ApplyResources((object) this.btnAddRegular, "btnAddRegular");
    this.btnAddRegular.ImageIndex = 1;
    this.btnAddRegular.ShowText = true;
    this.btnAddRegular.Click += new EventHandler(this.btnAddRegular_Click);
    componentResourceManager.ApplyResources((object) this.btnAddPreview, "btnAddPreview");
    this.btnAddPreview.ImageIndex = 0;
    this.btnAddPreview.ShowText = true;
    this.btnAddPreview.Click += new EventHandler(this.btnAddPreview_Click);
    componentResourceManager.ApplyResources((object) this.btnAddLink, "btnAddLink");
    this.btnAddLink.ImageIndex = 2;
    this.btnAddLink.ShowText = true;
    this.btnAddLink.Click += new EventHandler(this.btnAddLink_Click);
    this.toolBarValues.AllowVerticalDock = false;
    componentResourceManager.ApplyResources((object) this.toolBarValues, "toolBarValues");
    this.toolBarValues.DockLine = 3;
    this.toolBarValues.FullMenus = true;
    this.toolBarValues.Guid = new Guid("ba855ba6-35ae-4775-b979-b76ac70a54e0");
    this.toolBarValues.Hidden = false;
    this.toolBarValues.ImageList = this.imagesToolabarImpact;
    this.toolBarValues.Items.AddRange(new ToolbarItemBase[2]
    {
      (ToolbarItemBase) this.btnEditValue,
      (ToolbarItemBase) this.btnDeleteValue
    });
    this.toolBarValues.MinimumFloatingSize = new Size(250, 30);
    this.toolBarValues.Name = "toolBarValues";
    this.toolBarValues.Overflow = ToolBarOverflow.Wrap;
    this.toolBarValues.Stretch = true;
    componentResourceManager.ApplyResources((object) this.btnEditValue, "btnEditValue");
    this.btnEditValue.ImageIndex = 2;
    this.btnEditValue.ShowText = true;
    this.btnEditValue.Click += new EventHandler(this.btnEditValue_Click);
    componentResourceManager.ApplyResources((object) this.btnDeleteValue, "btnDeleteValue");
    this.btnDeleteValue.ImageIndex = 1;
    this.btnDeleteValue.ShowText = true;
    this.btnDeleteValue.Click += new EventHandler(this.btnDeleteValue_Click);
    this.panel3.Controls.Add((Control) this.eName);
    this.panel3.Controls.Add((Control) this.label1);
    componentResourceManager.ApplyResources((object) this.panel3, "panel3");
    this.panel3.Name = "panel3";
    componentResourceManager.ApplyResources((object) this.eName, "eName");
    this.eName.Name = "eName";
    this.eName.TextChanged += new EventHandler(this.eName_TextChanged);
    componentResourceManager.ApplyResources((object) this.label1, "label1");
    this.label1.Name = "label1";
    this.toolTipController1.Style = new ViewStyle("ToolTip style");
    this.toolTipController1.ToolTipLocation = ToolTipLocation.LeftBottom;
    this.dockManager1.DocumentContainer = (DocumentContainer) null;
    this.dockManager1.OwnerForm = (Form) this;
    componentResourceManager.ApplyResources((object) this.leftDock, "leftDock");
    this.leftDock.Guid = new Guid("ef255558-3167-4217-8d36-f0e80f1715f1");
    this.leftDock.LayoutSystem = new SplitLayoutSystem(250, 400);
    this.leftDock.Manager = this.dockManager1;
    this.leftDock.Name = "leftDock";
    this.leftDock.Renderer = (RendererBase) null;
    componentResourceManager.ApplyResources((object) this.rightDock, "rightDock");
    this.rightDock.Guid = new Guid("1d42240a-b3da-445c-a56f-ef52a6ba1f3c");
    this.rightDock.LayoutSystem = new SplitLayoutSystem(250, 400);
    this.rightDock.Manager = this.dockManager1;
    this.rightDock.Name = "rightDock";
    this.rightDock.Renderer = (RendererBase) null;
    componentResourceManager.ApplyResources((object) this.bottomDock, "bottomDock");
    this.bottomDock.Guid = new Guid("d380b446-b7a4-4aa8-9ce4-0359bf38810b");
    this.bottomDock.LayoutSystem = new SplitLayoutSystem(250, 400);
    this.bottomDock.Manager = this.dockManager1;
    this.bottomDock.Name = "bottomDock";
    this.bottomDock.Renderer = (RendererBase) null;
    componentResourceManager.ApplyResources((object) this.topDock, "topDock");
    this.topDock.Guid = new Guid("d5ac7b0a-2b1c-4d0b-9d26-7af19fd165d0");
    this.topDock.LayoutSystem = new SplitLayoutSystem(250, 400);
    this.topDock.Manager = this.dockManager1;
    this.topDock.Name = "topDock";
    this.topDock.Renderer = (RendererBase) null;
    this.AcceptButton = (IButtonControl) this.btnApply;
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.btnCancel;
    this.Controls.Add((Control) this.panel2);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.leftDock);
    this.Controls.Add((Control) this.rightDock);
    this.Controls.Add((Control) this.bottomDock);
    this.Controls.Add((Control) this.topDock);
    this.HelpButton = true;
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (VisStylesEditor);
    this.ShowInTaskbar = false;
    this.Tag = (object) " ";
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.parmsPanel.ResumeLayout(false);
    this.panParmsSimpleObject.ResumeLayout(false);
    this.panParmsSimpleObject.PerformLayout();
    this.panel6.ResumeLayout(false);
    this.panel6.PerformLayout();
    this.panParmsPreview.ResumeLayout(false);
    this.panParmsPreview.PerformLayout();
    this.panel4.ResumeLayout(false);
    this.panel4.PerformLayout();
    this.msePreview.Properties.EndInit();
    this.panParmsRelation.ResumeLayout(false);
    this.panParmsRelation.PerformLayout();
    this.panel5.ResumeLayout(false);
    this.panel5.PerformLayout();
    this.panelToolBar.ResumeLayout(false);
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
}
