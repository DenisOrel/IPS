// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Views.DocComplectScriptFormsView
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using Intermech.Client.Core;
using Intermech.Client.Core.FormDesigner;
using Intermech.Expert;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Expert;
using Intermech.Interfaces.ParamsStorage;
using Intermech.Interfaces.Reports;
using Intermech.Localization;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Reports.Views;

/// <summary>
/// Реализация закладки для назначения форм редактирования скриптам генерации комплектов
/// </summary>
public class DocComplectScriptFormsView : NavBaseView
{
  /// <summary>Имя контейнера параметров</summary>
  private string _storageName = string.Empty;
  /// <summary>Возможность редактирования свойств объекта</summary>
  private bool _canEdit;
  /// <summary>Признак наличия службы экспертной системы</summary>
  private bool _expertEditorAvailable;
  /// <summary>Контейнер параметров</summary>
  private IParamsStorageObject _storageObject;
  /// <summary>Список форм редактирования</summary>
  private List<long> _formIdList;
  /// <summary>Условие ЭС</summary>
  /// <remarks>Пока не используется</remarks>
  private TempFormula _curFormula;
  /// <summary>Формулы, присвоенные формам</summary>
  private readonly Dictionary<long, object> _conditions = new Dictionary<long, object>();
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private RichTextBox txtCondition;
  private TreeView tvForms;
  private Splitter splitter1;
  private ToolTip toolTipFE;
  private TableLayoutPanel _tlpSelecetdFields;
  private Button btnMoveTop;
  private Button btnMoveUp;
  private Button btnMoveBottom;
  private Button btnMoveDown;
  private ImageList _imgList;
  private ContextMenuStrip contextMenuStrip;
  private ToolStripMenuItem tsmiFormAdd;
  private ToolStripMenuItem tsmiFormView;
  private ToolStripMenuItem tsmiFormEdit;
  private ToolStripMenuItem tsmiFormRemove;
  private ToolStripSeparator tsmiSep1;
  private ToolStripMenuItem tsmiFormCond;
  private ToolStripMenuItem tsmiFormRemoveAll;
  private ToolStripSeparator tsmiFormSep2;

  /// <summary>
  /// 
  /// </summary>
  private void LoadContainerInfo()
  {
    this._conditions.Clear();
    this._formIdList = new List<long>();
    this._storageObject = (IParamsStorageObject) null;
    this._curFormula = (TempFormula) null;
    if (this._storageName != string.Empty)
    {
      IParamsStorageService service = ServiceUtils.GetService<IParamsStorageService>((object) ApplicationServices.Container, false);
      if (service != null)
      {
        this._storageObject = service.GetObject(this._storageName);
        if (this._storageObject != null)
          this._formIdList.AddRange((IEnumerable<long>) this._storageObject.GetFormDesignIDs());
      }
    }
    this.UpdateContainerInfo();
  }

  /// <summary>
  /// 
  /// </summary>
  private void SaveContainerInfo()
  {
    if (!this.Modified || !this.CanModify || this._storageName == string.Empty)
      return;
    List<long> longList = new List<long>();
    foreach (TreeNode node in this.tvForms.Nodes)
    {
      if (node?.Tag is FormInformation tag)
        longList.Add(tag.ID);
    }
    this._formIdList = longList;
    if (this._storageObject == null)
    {
      IParamsStorageService service = ServiceUtils.GetService<IParamsStorageService>((object) ApplicationServices.Container, false);
      if (service == null)
        return;
      this._storageObject = service.RegisterObject(this._storageName, false);
    }
    this._storageObject?.SetFormDesignIDs(this._formIdList.ToArray());
  }

  /// <summary>
  /// 
  /// </summary>
  private void UpdateContainerInfo()
  {
    ICollection<FormInformation> formInformations = (ICollection<FormInformation>) null;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (this._formIdList != null && this._formIdList.Count != 0 && sessionKeeper.Session.GetCustomService(typeof (IFormDesignerService)) is IFormDesignerService customService)
        formInformations = customService.GetForms(this._formIdList.ToArray(), AttributableElements.None, sessionKeeper.Session.SessionGUID, true);
      this.tvForms.BeginUpdate();
      try
      {
        this.tvForms.Nodes.Clear();
        if (formInformations != null)
        {
          foreach (FormInformation formInfo in (IEnumerable<FormInformation>) formInformations)
          {
            if (formInfo != null)
            {
              TreeNode node = this.BuildFormTreeNode(formInfo, sessionKeeper.Session);
              if (node != null && !this.tvForms.Nodes.Contains(node))
                this.tvForms.Nodes.Insert(this._formIdList.IndexOf(formInfo.ID), node);
            }
          }
        }
      }
      finally
      {
        this.tvForms.EndUpdate();
      }
    }
    this.ConditionFillInfo();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <returns></returns>
  private FormInformation GetSelectedItem()
  {
    TreeNode selectedNode = this.tvForms.SelectedNode;
    return selectedNode == null ? (FormInformation) null : selectedNode.Tag as FormInformation;
  }

  /// <summary>Move form's item up / first</summary>
  /// <param name="firstMode"></param>
  private void ItemsMove(DocComplectScriptFormsView.ItemMoveMode moveMode)
  {
    if (!this.CanModify)
      return;
    TreeNode selectedNode = this.tvForms.SelectedNode;
    if (selectedNode == null)
      return;
    int index1 = selectedNode.Index;
    int index2 = -1;
    switch (moveMode)
    {
      case DocComplectScriptFormsView.ItemMoveMode.First:
      case DocComplectScriptFormsView.ItemMoveMode.Up:
        if (index1 == -1 || index1 <= 0)
          return;
        index2 = moveMode == DocComplectScriptFormsView.ItemMoveMode.First ? 0 : index1 - 1;
        break;
      case DocComplectScriptFormsView.ItemMoveMode.Down:
      case DocComplectScriptFormsView.ItemMoveMode.Last:
        if (index1 == -1 || index1 >= this.tvForms.Nodes.Count - 1)
          return;
        index2 = moveMode == DocComplectScriptFormsView.ItemMoveMode.Last ? this.tvForms.Nodes.Count - 1 : index1 + 1;
        break;
    }
    if (index1 < 0 || index2 >= this.tvForms.Nodes.Count)
      return;
    this.tvForms.BeginUpdate();
    try
    {
      this.tvForms.Nodes.RemoveAt(index1);
      this.tvForms.Nodes.Insert(index2, selectedNode);
      this.tvForms.SelectedNode = selectedNode;
    }
    finally
    {
      this.tvForms.EndUpdate();
      this.Modified = true;
      this.UpdateControls();
    }
  }

  /// <summary>Добавление формы</summary>
  private void ItemAdd()
  {
    if (!this.CanModify)
      return;
    int objectTypeId = MetaDataHelper.GetObjectTypeID(GuidHolder.FormsTypeGuid);
    long[] objectIDs = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("Reports_54"), string.Empty, objectTypeId, SelectionOptions.Default);
    if (objectIDs == null || objectIDs.Length == 0)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ICollection<FormInformation> forms = ServiceUtils.GetService<IFormDesignerService>((object) sessionKeeper.Session, true).GetForms(objectIDs, AttributableElements.None, sessionKeeper.Session.SessionGUID, true);
      if (forms == null)
        return;
      bool flag = false;
      foreach (FormInformation formInfo in (IEnumerable<FormInformation>) forms)
      {
        if (formInfo != null)
        {
          TreeNode node = this.BuildFormTreeNode(formInfo, sessionKeeper.Session);
          if (node != null)
          {
            this.tvForms.Nodes.Add(node);
            flag = true;
          }
        }
      }
      if (!flag)
        return;
      this.Modified = true;
    }
  }

  /// <summary>Открыть в новом окне</summary>
  private void ItemView()
  {
    FormInformation selectedItem = this.GetSelectedItem();
    if (selectedItem == null)
      return;
    long id = selectedItem.ID;
    ServiceContainer viewServices = new ServiceContainer();
    viewServices.AddService(typeof (IViewState), (object) new ViewStateService(ViewStateFlags.ReadOnly));
    Intermech.Navigator.ContextMenu.Services.InvokeCommand("OpenInNewWindow", Intermech.Navigator.ContextMenu.Services.GetCommandsTable(Intermech.Navigator.ContextMenu.Services.GetItems(id), (IServiceProvider) viewServices, false), (IServiceProvider) viewServices);
  }

  /// <summary>Редактировать объект</summary>
  private void ItemEdit()
  {
    int num = this.CanModify ? 1 : 0;
  }

  private void ItemRemove()
  {
    if (!this.CanModify)
      return;
    TreeNode selectedNode = this.tvForms.SelectedNode;
    if (selectedNode == null)
      return;
    string caption = LocalizationHolder.rm.GetString("Reports_56");
    if (MessageBox.Show((IWin32Window) this, string.Format(LocalizationHolder.rm.GetString("Reports_55"), (object) selectedNode.Text), caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      return;
    selectedNode.Remove();
    this.tvForms.SelectedNode = (TreeNode) null;
    this.Modified = true;
  }

  private void ItemRemoveAll()
  {
    int num = this.CanModify ? 1 : 0;
  }

  /// <summary>
  /// 
  /// </summary>
  private void ItemCond()
  {
    int num = this.CanModify ? 1 : 0;
  }

  /// <summary>"Подсветка" синтаксиса условия</summary>
  /// <param name="t"></param>
  /// <param name="memoForm"></param>
  private void ConditionPaintCurToken(Token t, RichTextBox memoForm)
  {
    if (t == null || memoForm == null)
      return;
    if (t.type != Intermech.Expert.TokenType.FuncCall)
      memoForm.Select(t.StartPos, t.text.Length);
    switch (t.type)
    {
      case Intermech.Expert.TokenType.UnaryOper:
      case Intermech.Expert.TokenType.BinaryOper:
        memoForm.SelectionColor = Color.DarkRed;
        break;
      case Intermech.Expert.TokenType.OpeningBrace:
      case Intermech.Expert.TokenType.ClosingBrace:
        memoForm.SelectionColor = Color.Blue;
        break;
      case Intermech.Expert.TokenType.FuncCall:
        memoForm.Select(t.StartPos, t.text.Length - 1);
        memoForm.SelectionColor = Color.Black;
        memoForm.Select(t.StartPos + t.text.Length - 1, 1);
        memoForm.SelectionColor = Color.Blue;
        break;
      case Intermech.Expert.TokenType.Integer:
        memoForm.SelectionColor = Color.Indigo;
        break;
      case Intermech.Expert.TokenType.Float:
        memoForm.SelectionColor = Color.DarkOliveGreen;
        break;
      case Intermech.Expert.TokenType.String:
        memoForm.SelectionColor = Color.DarkMagenta;
        break;
      case Intermech.Expert.TokenType.Date:
        memoForm.SelectionColor = Color.DarkOrchid;
        break;
      case Intermech.Expert.TokenType.ObjectLink:
        memoForm.SelectionColor = Color.Red;
        break;
      default:
        memoForm.SelectionColor = Color.Black;
        break;
    }
  }

  /// <summary>
  /// 
  /// </summary>
  public void ConditionFillInfo()
  {
    if (this._curFormula == null)
    {
      this.txtCondition.Text = string.Empty;
    }
    else
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this._curFormula.Count; ++index)
        stringBuilder.Append(this._curFormula[index].text);
      this.txtCondition.Text = stringBuilder.ToString();
      for (int index = 0; index < this._curFormula.Count; ++index)
        this.ConditionPaintCurToken(this._curFormula[index], this.txtCondition);
    }
  }

  /// <summary>Создать узел для соотв формы</summary>
  /// <param name="formInfo">Описание формы</param>
  /// <param name="session">Сессия</param>
  /// <returns>Узел дерева</returns>
  private TreeNode BuildFormTreeNode(FormInformation formInfo, IUserSession session)
  {
    if (formInfo == null)
      return (TreeNode) null;
    string text = formInfo.Caption;
    if (string.IsNullOrEmpty(text.Trim()))
      text = string.Format(LocalizationHolder.rm.GetString("Reports_53"), (object) formInfo.ID);
    TreeNode treeNode = new TreeNode(text)
    {
      Tag = (object) formInfo
    };
    if (this.tvForms.ImageList != null)
      treeNode.SelectedImageIndex = treeNode.ImageIndex = Statics.IconSrv.IndexOf(4, MetaDataHelper.GetObjectTypeID(GuidHolder.FormsTypeGuid));
    return treeNode;
  }

  /// <summary>Установить доступность пунктов меню.</summary>
  private void UpdateContextMenuItems()
  {
    TreeNode selectedNode = this.tvForms.SelectedNode;
    this.tsmiFormAdd.Enabled = this.CanModify;
    this.tsmiFormView.Enabled = selectedNode != null;
    this.tsmiFormEdit.Enabled = this.tsmiFormRemove.Enabled = this.CanModify && selectedNode != null;
    this.tsmiFormCond.Enabled = this.CanModify && selectedNode != null && this._expertEditorAvailable;
  }

  /// <summary>Инициализация контролов</summary>
  protected override void InitializeCustomControls()
  {
    this.InitializeComponent();
    base.InitializeCustomControls();
    this.btnMoveTop.Tag = (object) DocComplectScriptFormsView.ItemMoveMode.First;
    this.btnMoveUp.Tag = (object) DocComplectScriptFormsView.ItemMoveMode.Up;
    this.btnMoveDown.Tag = (object) DocComplectScriptFormsView.ItemMoveMode.Down;
    this.btnMoveBottom.Tag = (object) DocComplectScriptFormsView.ItemMoveMode.Last;
    this.Dock = DockStyle.Fill;
    this.tvForms.ImageList = Statics.IconSrv != null ? Statics.IconSrv.ImageList : (ImageList) null;
    if (this.DesignMode)
      return;
    this._expertEditorAvailable = ServiceUtils.GetService<IExpertEditor>((object) ApplicationServices.Container, false) != null;
  }

  /// <summary>Инициализация кастом сообщений</summary>
  protected override void InitializeCustomMessages()
  {
    base.InitializeCustomMessages();
    this._caption = LocalizationHolder.rm.GetString("Reports_52");
  }

  /// <summary>Загрузить информацию в контрол</summary>
  protected override void LoadData()
  {
    this._canEdit = false;
    this._storageName = string.Empty;
    if (!this._expertEditorAvailable)
      this._expertEditorAvailable = ServiceUtils.GetService<IExpertEditor>((object) ApplicationServices.Container, false) != null;
    if (this._objID != 0L)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(this._objID, false);
        if (dbObject != null)
        {
          this._canEdit = dbObject.ObjectModifyMode == ObjectModifyModes.InBase || dbObject.CheckoutBy == sessionKeeper.Session.UserID;
          this._storageName = ReportUtils.GetContainerName(dbObject);
        }
      }
    }
    this.LoadContainerInfo();
    base.LoadData();
  }

  /// <summary>Сохранить информацию из контрола</summary>
  /// <param name="sendNotifications">Необходимость отправки уведомлений</param>
  protected override void SaveData(bool sendNotifications = true)
  {
    if (!this.Modified)
      return;
    this.SaveContainerInfo();
    base.SaveData(sendNotifications);
  }

  /// <summary>Can modifying flag</summary>
  public override bool CanModify
  {
    get
    {
      bool canModify = base.CanModify && this._canEdit;
      if (!canModify)
        return false;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObjectType objectType = sessionKeeper.Session.GetObjectType(ReportsConsts.ScriptPackageTypeID, false);
        if (objectType == null)
          return false;
        if (objectType is IDBSecurity dbSecurity)
          canModify = dbSecurity.CheckAccess(ActionType.CreateChildItem, true, false);
      }
      return canModify;
    }
  }

  /// <summary>Обновить состояние элементов управления закладки</summary>
  protected override void UpdateControls()
  {
    base.UpdateControls();
    this.UpdateContextMenuItems();
    TreeNode selectedNode = this.tvForms.SelectedNode;
    if (selectedNode != null)
    {
      this.btnMoveTop.Enabled = this.btnMoveUp.Enabled = this.btnMoveDown.Enabled = this.btnMoveBottom.Enabled = this.CanModify;
      if (selectedNode.Index <= 0)
        this.btnMoveTop.Enabled = this.btnMoveUp.Enabled = false;
      if (selectedNode.Index >= this.tvForms.Nodes.Count - 1)
        this.btnMoveDown.Enabled = this.btnMoveBottom.Enabled = false;
    }
    else
      this.btnMoveTop.Enabled = this.btnMoveUp.Enabled = this.btnMoveDown.Enabled = this.btnMoveBottom.Enabled = false;
    this.txtCondition.Enabled = this._expertEditorAvailable;
  }

  /// <summary>ImageIndex</summary>
  public override int ImageIndex => -1;

  /// <summary>OrderID</summary>
  public override int OrderID => 0;

  /// <summary>Клик по дереву.</summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void OntrvForms_MouseDown(object sender, MouseEventArgs e)
  {
  }

  /// <summary>Событие, которое возникает после выделения узела.</summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void OntrvForms_AfterSelect(object sender, TreeViewEventArgs e)
  {
    this._curFormula = (TempFormula) null;
    FormInformation selectedItem = this.GetSelectedItem();
    if (selectedItem != null && this._conditions.ContainsKey(selectedItem.ID))
      this._curFormula = (TempFormula) this._conditions[selectedItem.ID];
    this.ConditionFillInfo();
    this.UpdateControls();
  }

  /// <summary>Перемещение элементов по позициям в списке.</summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void On_btnUpDown_Click(object sender, EventArgs e)
  {
    if (!(sender is Button button))
      return;
    this.ItemsMove((DocComplectScriptFormsView.ItemMoveMode) button.Tag);
    button.Focus();
  }

  /// <summary>Открытие контекстного меню.</summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
  {
    this.UpdateContextMenuItems();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void tsmiFormAdd_Click(object sender, EventArgs e) => this.ItemAdd();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void tsmiFormView_Click(object sender, EventArgs e) => this.ItemView();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void tsmiFormEdit_Click(object sender, EventArgs e) => this.ItemEdit();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void tsmiFormRemove_Click(object sender, EventArgs e) => this.ItemRemove();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void tsmiFormCond_Click(object sender, EventArgs e) => this.ItemCond();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void tsmiFormRemoveAll_Click(object sender, EventArgs e) => this.ItemRemoveAll();

  /// <summary>Двойной клик мышкой по полю условие.</summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void OntxtCondition_DoubleClick(object sender, EventArgs e) => this.ItemCond();

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  private void OntxtCondition_MouseMove(object sender, MouseEventArgs e)
  {
    if (this._curFormula == null)
      return;
    int tokenByPos = this._curFormula.GetTokenByPos(this.txtCondition.GetCharIndexFromPosition(new Point(e.X, e.Y)));
    string caption = "";
    if (tokenByPos >= 0)
    {
      Token token = this._curFormula[tokenByPos];
      if (token.type == Intermech.Expert.TokenType.Integer && token.text != token.trueText)
        caption = token.trueText;
    }
    if (caption == this.toolTipFE.GetToolTip((Control) this.txtCondition))
      return;
    this.toolTipFE.SetToolTip((Control) this.txtCondition, caption);
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
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DocComplectScriptFormsView));
    this.txtCondition = new RichTextBox();
    this.tvForms = new TreeView();
    this.contextMenuStrip = new ContextMenuStrip(this.components);
    this.tsmiFormAdd = new ToolStripMenuItem();
    this.tsmiFormView = new ToolStripMenuItem();
    this.tsmiFormEdit = new ToolStripMenuItem();
    this.tsmiFormRemove = new ToolStripMenuItem();
    this.tsmiSep1 = new ToolStripSeparator();
    this.tsmiFormRemoveAll = new ToolStripMenuItem();
    this.tsmiFormSep2 = new ToolStripSeparator();
    this.tsmiFormCond = new ToolStripMenuItem();
    this.splitter1 = new Splitter();
    this.toolTipFE = new ToolTip(this.components);
    this.btnMoveTop = new Button();
    this._imgList = new ImageList(this.components);
    this.btnMoveUp = new Button();
    this.btnMoveBottom = new Button();
    this.btnMoveDown = new Button();
    this._tlpSelecetdFields = new TableLayoutPanel();
    this.pnButtons.SuspendLayout();
    this.contextMenuStrip.SuspendLayout();
    this._tlpSelecetdFields.SuspendLayout();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.pnButtons, "pnButtons");
    componentResourceManager.ApplyResources((object) this.btApply, "btApply");
    componentResourceManager.ApplyResources((object) this.btCancel, "btCancel");
    this.txtCondition.BackColor = SystemColors.Window;
    componentResourceManager.ApplyResources((object) this.txtCondition, "txtCondition");
    this.txtCondition.Name = "txtCondition";
    this.txtCondition.ReadOnly = true;
    this.txtCondition.DoubleClick += new EventHandler(this.OntxtCondition_DoubleClick);
    this.txtCondition.MouseMove += new MouseEventHandler(this.OntxtCondition_MouseMove);
    this.tvForms.ContextMenuStrip = this.contextMenuStrip;
    componentResourceManager.ApplyResources((object) this.tvForms, "tvForms");
    this.tvForms.HideSelection = false;
    this.tvForms.Name = "tvForms";
    this.tvForms.AfterSelect += new TreeViewEventHandler(this.OntrvForms_AfterSelect);
    this.tvForms.MouseDown += new MouseEventHandler(this.OntrvForms_MouseDown);
    this.contextMenuStrip.Items.AddRange(new ToolStripItem[8]
    {
      (ToolStripItem) this.tsmiFormAdd,
      (ToolStripItem) this.tsmiFormView,
      (ToolStripItem) this.tsmiFormEdit,
      (ToolStripItem) this.tsmiFormRemove,
      (ToolStripItem) this.tsmiSep1,
      (ToolStripItem) this.tsmiFormRemoveAll,
      (ToolStripItem) this.tsmiFormSep2,
      (ToolStripItem) this.tsmiFormCond
    });
    this.contextMenuStrip.Name = "contextMenuStrip1";
    componentResourceManager.ApplyResources((object) this.contextMenuStrip, "contextMenuStrip");
    this.contextMenuStrip.Opening += new CancelEventHandler(this.contextMenuStrip_Opening);
    this.tsmiFormAdd.Name = "tsmiFormAdd";
    componentResourceManager.ApplyResources((object) this.tsmiFormAdd, "tsmiFormAdd");
    this.tsmiFormAdd.Click += new EventHandler(this.tsmiFormAdd_Click);
    this.tsmiFormView.Name = "tsmiFormView";
    componentResourceManager.ApplyResources((object) this.tsmiFormView, "tsmiFormView");
    this.tsmiFormView.Click += new EventHandler(this.tsmiFormView_Click);
    this.tsmiFormEdit.Name = "tsmiFormEdit";
    componentResourceManager.ApplyResources((object) this.tsmiFormEdit, "tsmiFormEdit");
    this.tsmiFormEdit.Click += new EventHandler(this.tsmiFormEdit_Click);
    this.tsmiFormRemove.Name = "tsmiFormRemove";
    componentResourceManager.ApplyResources((object) this.tsmiFormRemove, "tsmiFormRemove");
    this.tsmiFormRemove.Click += new EventHandler(this.tsmiFormRemove_Click);
    this.tsmiSep1.Name = "tsmiSep1";
    componentResourceManager.ApplyResources((object) this.tsmiSep1, "tsmiSep1");
    this.tsmiFormRemoveAll.Name = "tsmiFormRemoveAll";
    componentResourceManager.ApplyResources((object) this.tsmiFormRemoveAll, "tsmiFormRemoveAll");
    this.tsmiFormRemoveAll.Click += new EventHandler(this.tsmiFormRemoveAll_Click);
    this.tsmiFormSep2.Name = "tsmiFormSep2";
    componentResourceManager.ApplyResources((object) this.tsmiFormSep2, "tsmiFormSep2");
    this.tsmiFormCond.Name = "tsmiFormCond";
    componentResourceManager.ApplyResources((object) this.tsmiFormCond, "tsmiFormCond");
    this.tsmiFormCond.Click += new EventHandler(this.tsmiFormCond_Click);
    componentResourceManager.ApplyResources((object) this.splitter1, "splitter1");
    this.splitter1.Name = "splitter1";
    this.splitter1.TabStop = false;
    componentResourceManager.ApplyResources((object) this.btnMoveTop, "btnMoveTop");
    this.btnMoveTop.ImageList = this._imgList;
    this.btnMoveTop.Name = "btnMoveTop";
    this.btnMoveTop.Tag = (object) "";
    this.toolTipFE.SetToolTip((Control) this.btnMoveTop, componentResourceManager.GetString("btnMoveTop.ToolTip"));
    this.btnMoveTop.UseVisualStyleBackColor = true;
    this.btnMoveTop.Click += new EventHandler(this.On_btnUpDown_Click);
    this._imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("_imgList.ImageStream");
    this._imgList.TransparentColor = Color.Transparent;
    this._imgList.Images.SetKeyName(0, "Top.ico");
    this._imgList.Images.SetKeyName(1, "Up.ico");
    this._imgList.Images.SetKeyName(2, "Down.ico");
    this._imgList.Images.SetKeyName(3, "Bottom.ico");
    componentResourceManager.ApplyResources((object) this.btnMoveUp, "btnMoveUp");
    this.btnMoveUp.ImageList = this._imgList;
    this.btnMoveUp.Name = "btnMoveUp";
    this.btnMoveUp.Tag = (object) "";
    this.toolTipFE.SetToolTip((Control) this.btnMoveUp, componentResourceManager.GetString("btnMoveUp.ToolTip"));
    this.btnMoveUp.UseVisualStyleBackColor = true;
    this.btnMoveUp.Click += new EventHandler(this.On_btnUpDown_Click);
    componentResourceManager.ApplyResources((object) this.btnMoveBottom, "btnMoveBottom");
    this.btnMoveBottom.ImageList = this._imgList;
    this.btnMoveBottom.Name = "btnMoveBottom";
    this.btnMoveBottom.Tag = (object) "";
    this.toolTipFE.SetToolTip((Control) this.btnMoveBottom, componentResourceManager.GetString("btnMoveBottom.ToolTip"));
    this.btnMoveBottom.UseVisualStyleBackColor = true;
    this.btnMoveBottom.Click += new EventHandler(this.On_btnUpDown_Click);
    componentResourceManager.ApplyResources((object) this.btnMoveDown, "btnMoveDown");
    this.btnMoveDown.ImageList = this._imgList;
    this.btnMoveDown.Name = "btnMoveDown";
    this.btnMoveDown.Tag = (object) "";
    this.toolTipFE.SetToolTip((Control) this.btnMoveDown, componentResourceManager.GetString("btnMoveDown.ToolTip"));
    this.btnMoveDown.UseVisualStyleBackColor = true;
    this.btnMoveDown.Click += new EventHandler(this.On_btnUpDown_Click);
    componentResourceManager.ApplyResources((object) this._tlpSelecetdFields, "_tlpSelecetdFields");
    this._tlpSelecetdFields.Controls.Add((Control) this.btnMoveTop, 0, 1);
    this._tlpSelecetdFields.Controls.Add((Control) this.btnMoveUp, 0, 2);
    this._tlpSelecetdFields.Controls.Add((Control) this.btnMoveBottom, 0, 4);
    this._tlpSelecetdFields.Controls.Add((Control) this.btnMoveDown, 0, 3);
    this._tlpSelecetdFields.Name = "_tlpSelecetdFields";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.tvForms);
    this.Controls.Add((Control) this.splitter1);
    this.Controls.Add((Control) this.txtCondition);
    this.Controls.Add((Control) this._tlpSelecetdFields);
    this.Name = nameof (DocComplectScriptFormsView);
    this.Tag = (object) "";
    this.Controls.SetChildIndex((Control) this.pnButtons, 0);
    this.Controls.SetChildIndex((Control) this._tlpSelecetdFields, 0);
    this.Controls.SetChildIndex((Control) this.txtCondition, 0);
    this.Controls.SetChildIndex((Control) this.splitter1, 0);
    this.Controls.SetChildIndex((Control) this.tvForms, 0);
    this.pnButtons.ResumeLayout(false);
    this.contextMenuStrip.ResumeLayout(false);
    this._tlpSelecetdFields.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  /// <summary>Режимы перемещения элементов</summary>
  internal enum ItemMoveMode
  {
    /// <summary>
    /// 
    /// </summary>
    First,
    /// <summary>
    /// 
    /// </summary>
    Up,
    /// <summary>
    /// 
    /// </summary>
    Down,
    /// <summary>
    /// 
    /// </summary>
    Last,
  }
}
