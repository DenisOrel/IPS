// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.Options.ObjectOptions.LinkedOptionsEditor
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using DevExpress.IM.XtraEditors.Controls;
using DevExpress.IM.XtraEditors.Repository;
using DevExpress.IM.XtraTreeList;
using DevExpress.IM.XtraTreeList.Columns;
using DevExpress.IM.XtraTreeList.Nodes;
using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator.Options.ObjectOptions;

public sealed class LinkedOptionsEditor : UserControl
{
  private IncompatibilityEditor.PathPart _selectedItem;
  private RepositoryItemComboBox cbVisibleValues = new RepositoryItemComboBox();
  private RepositoryItemComboBox cbAvailableOptions = new RepositoryItemComboBox();
  private ObjectOptionsHolder _options;
  private OptionHolder _selectedOption;
  private OptionValue _selectedValue;
  private OptionValuePair selectedOptionValuePair;
  private OptionAccessRights _accessRights;
  private LinkedOptions _linkedOptions;
  private LinkedOptions _linkedOptionsBack;
  private LinkedOptions _fullLinkedOptions;
  private bool localChange;
  private bool _isChanged;
  private List<OptionValuePair> currentLinkedList = new List<OptionValuePair>();
  private static Dictionary<string, int> _colWidths = new Dictionary<string, int>();
  private IContainer components;
  private Intermech.Bars.ToolBar tbOptionWork;
  private ButtonItem btnAddOption;
  private ButtonItem btnDeleteOption;
  private ToolTip toolTip;
  private ImageList ilLinked;
  private TreeList tlLinkedOptions;
  private ButtonItem btnClear;
  private MenuBar cmsOptionWork;
  private ContextMenuBarItem contextMenuBarItem;
  private MenuButtonItem tsmAddOption;
  private MenuButtonItem tsmDeleteOption;
  private MenuButtonItem tsmClear;
  private Panel panel1;
  private PictureBox pbError;
  private Label lbErrorState;
  private TreeListColumn treeListColumn1;
  private TreeListColumn treeListColumn2;
  private TreeListColumn treeListColumn3;
  private ImageList ilError;
  private Button btnMore;
  private TextBox tbMoreInfo;
  private Splitter splitter1;
  internal Panel panelHint;
  internal Label labelWarning;
  internal PictureBox pictureHint;

  public LinkedOptionsEditor()
  {
    this.InitializeComponent();
    if (ServicesManager.GetService(typeof (IGuidMapper)) is IGuidMapper)
    {
      this.InitAvailableOptions();
      this.InitVisibleValues();
    }
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.tbOptionWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.cmsOptionWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged += new EventHandler(this.ToolbarRendererChanged);
      this.ToolbarRendererChanged((object) service, EventArgs.Empty);
    }
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1833);
  }

  public event LinkedOptionsEditor.ObjectOptionsChangedEventHandler OnChanged;

  public bool IsChanged
  {
    get => this._isChanged;
    set
    {
      this._isChanged = this.localChange = value;
      this.RaiseOnChanged();
    }
  }

  [Browsable(false)]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public IncompatibilityEditor.PathPart SelectedItem
  {
    get => this._selectedItem;
    set
    {
      if (this._selectedItem == value)
        return;
      this._selectedItem = value;
      foreach (TreeListNode node in this.tlLinkedOptions.Nodes)
      {
        if (object.Equals((object) this._selectedItem, (object) this.GetPathPart(node)))
        {
          this.tlLinkedOptions.FocusedNode = node;
          break;
        }
      }
    }
  }

  public IncompatibilityEditor.PathPart GetLiveSelectedItem()
  {
    return this.tlLinkedOptions.FocusedNode != null ? this.GetPathPart(this.tlLinkedOptions.FocusedNode) : (IncompatibilityEditor.PathPart) null;
  }

  private IncompatibilityEditor.PathPart GetPathPart(TreeListNode node)
  {
    return new IncompatibilityEditor.PathPart(node[(object) "LINKED_OPTION"], node[(object) "OPTION_VALUE"]);
  }

  public void LoadLinkedOptions(
    ObjectOptionsHolder options,
    OptionHolder selectedOption,
    OptionValue selectedValue,
    OptionAccessRights accessRights)
  {
    this._accessRights = accessRights;
    this.Save();
    this.tlLinkedOptions.ClearNodes();
    this._options = options;
    this._selectedOption = selectedOption;
    this._selectedValue = selectedValue;
    this.currentLinkedList.Clear();
    this.lbErrorState.Text = string.Empty;
    this.pbError.Visible = false;
    if (this._options != null && this._selectedOption != null && this._selectedValue != null)
    {
      this.selectedOptionValuePair = new OptionValuePair(this._selectedOption.OptionGuid, this._selectedValue.ID);
      this._linkedOptions = this._options.Incompatibilities.LinkedOptions;
      this._linkedOptionsBack = this._linkedOptions.Clone() as LinkedOptions;
      this.currentLinkedList = this._linkedOptions.GetLinkedOptions(this._selectedOption.OptionGuid, this._selectedValue.ID);
      this.tlLinkedOptions.BeginUpdate();
      try
      {
        this.FillLinkedOptionsTree();
      }
      finally
      {
        this.tlLinkedOptions.EndUpdate();
      }
    }
    this.UpdateControls();
  }

  public void Save()
  {
    if (!this.localChange || this._selectedOption == null || this._selectedValue == null)
      return;
    List<OptionValuePair> optionValuePairList = new List<OptionValuePair>();
    foreach (TreeListNode node in this.tlLinkedOptions.Nodes)
    {
      Guid option = !(node[(object) "LINKED_OPTION"] is MyElement myElement1) ? Guid.Empty : (Guid) myElement1.Value;
      string id = !(node[(object) "OPTION_VALUE"] is MyElement myElement2) ? string.Empty : myElement2.Value.ToString();
      optionValuePairList.Add(new OptionValuePair(option, id));
    }
    this._fullLinkedOptions = (LinkedOptions) null;
    this.localChange = false;
    this._linkedOptions.Items[new OptionValuePair(this._selectedOption.OptionGuid, this._selectedValue.ID)] = optionValuePairList;
  }

  public void Undo()
  {
    this.IsChanged = false;
    this.tlLinkedOptions.ClearNodes();
    this.currentLinkedList.Clear();
  }

  private void btnMore_Click(object sender, EventArgs e)
  {
    this.tbMoreInfo.Visible = !this.tbMoreInfo.Visible;
  }

  private void btnAddOption_Click(object sender, EventArgs e)
  {
    this.tbOptionWork.Focus();
    TreeListNode treeListNode = this.tlLinkedOptions.AppendNode((object) new object[3]
    {
      (object) LocalizationHolder.rm.GetString("PdmConfigurator_29"),
      (object) LocalizationHolder.rm.GetString("PdmConfigurator_30"),
      (object) ErrorState.None
    }, (TreeListNode) null);
    if (treeListNode == null)
      return;
    this.tlLinkedOptions.FocusedNode = treeListNode;
    this.tlLinkedOptions.Focus();
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void btnDeleteOption_Click(object sender, EventArgs e)
  {
    this.tbOptionWork.Focus();
    TreeListNode focusedNode = this.tlLinkedOptions.FocusedNode;
    if (focusedNode != null)
      this.tlLinkedOptions.DeleteNode(focusedNode);
    if (this.IsIncompConflictExists())
    {
      foreach (TreeListNode node in this.tlLinkedOptions.Nodes)
      {
        if ((ErrorState) node[(object) "OPTION_ERROR"] == ErrorState.None)
        {
          node[(object) "OPTION_ERROR"] = (object) ErrorState.IncompConflict;
          node.StateImageIndex = 0;
        }
      }
    }
    else
    {
      foreach (TreeListNode node in this.tlLinkedOptions.Nodes)
      {
        if ((ErrorState) node[(object) "OPTION_ERROR"] == ErrorState.IncompConflict)
        {
          node[(object) "OPTION_ERROR"] = (object) ErrorState.None;
          node.StateImageIndex = -1;
        }
      }
    }
    this.tlLinkedOptions.Focus();
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void btnClear_Click(object sender, EventArgs e)
  {
    this.tbOptionWork.Focus();
    this.tlLinkedOptions.ClearNodes();
    this.tlLinkedOptions.Focus();
    this.IsChanged = true;
    this.UpdateControls();
  }

  private void ToolbarRendererChanged(object sender, EventArgs e)
  {
    IToolBarRenderer renderer = (sender as BarManager).Renderer;
    this.tbOptionWork.Renderer = renderer;
    this.cmsOptionWork.Renderer = renderer;
  }

  private void tlLinkedOptions_ShowingEditor(object sender, CancelEventArgs e)
  {
    TreeListNode focusedNode = this.tlLinkedOptions.FocusedNode;
    TreeListColumn focusedColumn = this.tlLinkedOptions.FocusedColumn;
    if (focusedNode == null || focusedColumn == null)
      return;
    if (focusedColumn.FieldName == "LINKED_OPTION")
    {
      this.cbAvailableOptions.BeginUpdate();
      try
      {
        this.cbAvailableOptions.Items.Clear();
        foreach (long option1 in this._options.Options)
        {
          OptionHolder option2 = PdmConfiguratorCache.CacheFindOption(option1);
          if (option2 == null)
          {
            using (SessionKeeper sessionKeeper = new SessionKeeper())
            {
              PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, option1);
              option2 = PdmConfiguratorCache.CacheFindOption(option1);
            }
          }
          if (option2 != null && option1 != this._selectedOption.OptionObjectID && !this.IsOptionExists(option2.OptionGuid, focusedNode))
            this.cbAvailableOptions.Items.Add((object) new MyElement((object) option2.OptionGuid, option2.OptionCaption, (object) option2));
        }
      }
      finally
      {
        this.cbAvailableOptions.EndUpdate();
      }
    }
    else
    {
      this.cbVisibleValues.BeginUpdate();
      try
      {
        this.cbVisibleValues.Items.Clear();
        if (focusedNode[(object) "LINKED_OPTION"] != null)
        {
          if (!(focusedNode.GetValue((object) "LINKED_OPTION") is MyElement myElement1) || myElement1.Value.Equals((object) Guid.Empty))
            e.Cancel = true;
          else if ((ErrorState) focusedNode.GetValue((object) "OPTION_ERROR") == ErrorState.Option)
          {
            e.Cancel = true;
          }
          else
          {
            OptionHolder tag = myElement1.Tag as OptionHolder;
            OptionValuesCollection optionValues = tag.OptionValues;
            List<string> list = (this._options.VisibleOptionValues.Items.ContainsKey(tag.OptionGuid) ? (IEnumerable<string>) this._options.VisibleOptionValues.Items[tag.OptionGuid] : (IEnumerable<string>) new List<string>(0)).OrderBy<string, int>((Func<string, int>) (o => optionValues.IndexOf(optionValues.FindValue(o)))).ToList<string>();
            if (list.Count == 0)
            {
              foreach (OptionValue optionValue in optionValues.ToArray())
              {
                MyElement myElement = new MyElement();
                string displayValue = optionValue.GetDisplayValue(tag);
                myElement.Caption = string.IsNullOrEmpty(optionValue.Code) ? displayValue : $"[{optionValue.Code}] {displayValue}";
                myElement.Tag = (object) optionValue;
                myElement.Value = (object) optionValue.ID;
                this.cbVisibleValues.Items.Add((object) myElement);
              }
            }
            else
            {
              foreach (string str in list)
              {
                OptionValue optionValue = optionValues.FindValue(str);
                MyElement myElement = new MyElement();
                string displayValue = optionValue.GetDisplayValue(tag);
                myElement.Caption = string.IsNullOrEmpty(optionValue.Code) ? displayValue : $"[{optionValue.Code}] {displayValue}";
                myElement.Tag = (object) optionValue;
                myElement.Value = (object) optionValue.ID;
                this.cbVisibleValues.Items.Add((object) myElement);
              }
            }
          }
        }
        else
          e.Cancel = true;
      }
      finally
      {
        this.cbVisibleValues.EndUpdate();
      }
    }
  }

  private void tlLinkedOptions_CellValueChanging(object sender, CellValueChangedEventArgs e)
  {
    TreeListNode node1 = e.Node;
    this.IsChanged = true;
    OptionValuePair linkedOptionValuePair = (OptionValuePair) null;
    OptionHolder linkedOption = (OptionHolder) null;
    OptionValue linkedOptionValue = (OptionValue) null;
    if (node1 != null)
    {
      if (e.Column.FieldName == "LINKED_OPTION")
      {
        if (!node1[(object) "LINKED_OPTION"].Equals(e.Value))
        {
          node1[(object) "OPTION_VALUE"] = (object) LocalizationHolder.rm.GetString("PdmConfigurator_30");
          this.cbVisibleValues.Items.Clear();
          linkedOption = (e.Value as MyElement).Tag as OptionHolder;
          linkedOptionValuePair = new OptionValuePair(linkedOption.OptionGuid, string.Empty);
        }
      }
      else
      {
        linkedOption = (e.Node.GetValue((object) "LINKED_OPTION") as MyElement).Tag as OptionHolder;
        linkedOptionValue = (e.Value as MyElement).Tag as OptionValue;
        linkedOptionValuePair = new OptionValuePair(linkedOption.OptionGuid, linkedOptionValue.ID);
      }
      if (this.IsIncompConflictExists(linkedOptionValuePair))
      {
        foreach (TreeListNode node2 in this.tlLinkedOptions.Nodes)
        {
          if ((ErrorState) node2[(object) "OPTION_ERROR"] == ErrorState.None)
          {
            node2[(object) "OPTION_ERROR"] = (object) ErrorState.IncompConflict;
            node2.StateImageIndex = 0;
          }
        }
      }
      else
      {
        foreach (TreeListNode node3 in this.tlLinkedOptions.Nodes)
        {
          if ((ErrorState) node3[(object) "OPTION_ERROR"] == ErrorState.IncompConflict)
          {
            node3[(object) "OPTION_ERROR"] = (object) ErrorState.None;
            node3.StateImageIndex = -1;
          }
        }
        ErrorState errorState = this.CheckOptionConflict(linkedOption, linkedOptionValue);
        e.Node[(object) "OPTION_ERROR"] = (object) errorState;
        switch (errorState)
        {
          case ErrorState.None:
            e.Node.StateImageIndex = -1;
            break;
          case ErrorState.Value:
          case ErrorState.ObsoleteOption:
            e.Node.StateImageIndex = 5;
            break;
          case ErrorState.ObsoleteOptionValue:
            e.Node.StateImageIndex = 4;
            break;
          default:
            e.Node.StateImageIndex = 0;
            break;
        }
      }
    }
    this.UpdateControls();
  }

  private void tlLinkedOptions_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
  {
    this._selectedItem = this.GetLiveSelectedItem();
    this.UpdateControls();
  }

  private void RaiseOnChanged()
  {
    if (this.OnChanged == null)
      return;
    this.OnChanged((object) this, new EventArgs());
  }

  private void InitAvailableOptions()
  {
    this.cbAvailableOptions.Buttons[0].Width = 14;
    this.cbAvailableOptions.TextEditStyle = TextEditStyles.DisableTextEditor;
    this.cbAvailableOptions.DropDownRows = 15;
    this.tlLinkedOptions.Columns["LINKED_OPTION"].ColumnEdit = (RepositoryItem) this.cbAvailableOptions;
  }

  private void InitVisibleValues()
  {
    this.cbVisibleValues.Buttons[0].Width = 14;
    this.cbVisibleValues.TextEditStyle = TextEditStyles.DisableTextEditor;
    this.cbVisibleValues.DropDownRows = 15;
    this.tlLinkedOptions.Columns["OPTION_VALUE"].ColumnEdit = (RepositoryItem) this.cbVisibleValues;
  }

  private void FillLinkedOptionsTree()
  {
    ErrorState errorState1 = ErrorState.None;
    if (LinkedOptions.IsIncompConflictExists(this._options, this.selectedOptionValuePair, this.currentLinkedList))
      errorState1 = ErrorState.IncompConflict;
    for (int index = 0; index < this.currentLinkedList.Count; ++index)
    {
      OptionValuePair currentLinked = this.currentLinkedList[index];
      Guid option1 = currentLinked.Option;
      OptionHolder option2 = PdmConfiguratorCache.CacheFindOption(option1);
      if (option2 == null)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, option1);
          option2 = PdmConfiguratorCache.CacheFindOption(option1);
        }
      }
      MyElement myElement1 = new MyElement();
      myElement1.Caption = option2 != null ? option2.OptionCaption : LocalizationHolder.rm.GetString("PdmConfigurator_29");
      myElement1.Value = (object) (option2 != null ? option2.OptionGuid : Guid.Empty);
      myElement1.Tag = (object) option2;
      string id = currentLinked.ID;
      MyElement myElement2 = new MyElement();
      OptionValue linkedOptionValue = option2?.OptionValues.FindValue(id);
      if (linkedOptionValue == null)
      {
        myElement2.Caption = LocalizationHolder.rm.GetString("PdmConfigurator_30");
      }
      else
      {
        string displayValue = linkedOptionValue.GetDisplayValue(option2);
        myElement2.Caption = string.IsNullOrEmpty(linkedOptionValue.Code) ? displayValue : $"[{linkedOptionValue.Code}] {displayValue}";
      }
      myElement2.Value = linkedOptionValue != null ? (object) linkedOptionValue.ID : (object) string.Empty;
      myElement2.Tag = (object) linkedOptionValue;
      ErrorState errorState2 = this.CheckOptionConflict(option2, linkedOptionValue, true, index == 0);
      if (errorState2 == ErrorState.None && errorState1 == ErrorState.IncompConflict)
        errorState2 = ErrorState.IncompConflict;
      TreeListNode treeListNode = this.tlLinkedOptions.AppendNode((object) new object[3]
      {
        (object) myElement1,
        (object) myElement2,
        (object) errorState2
      }, (TreeListNode) null);
      if (errorState2 != ErrorState.None)
        treeListNode.StateImageIndex = errorState2 == ErrorState.Value || errorState2 == ErrorState.ObsoleteOption ? 5 : (errorState2 != ErrorState.ObsoleteOptionValue ? 0 : 4);
    }
  }

  private ErrorState CheckOptionConflict(OptionHolder linkedOption, OptionValue linkedOptionValue)
  {
    return this.CheckOptionConflict(linkedOption, linkedOptionValue, false, true);
  }

  private ErrorState CheckOptionConflict(
    OptionHolder linkedOption,
    OptionValue linkedOptionValue,
    bool isLoad,
    bool reload)
  {
    if (linkedOption == null)
      return ErrorState.None;
    OptionValuesCollection optionValues = linkedOption.OptionValues;
    if (!this._options.Options.Contains(linkedOption.OptionObjectID))
      return ErrorState.Option;
    if (linkedOptionValue == null)
      return ErrorState.None;
    OptionValuePair optionValuePair = new OptionValuePair(linkedOption.OptionGuid, linkedOptionValue.ID);
    if (!this._options.VisibleOptionValues.GetVisibleOptionValue(linkedOption.OptionGuid, linkedOptionValue.ID))
      return ErrorState.Value;
    if (this._fullLinkedOptions == null | reload)
      this._fullLinkedOptions = this._linkedOptions.Clone() as LinkedOptions;
    if (!isLoad)
      this._fullLinkedOptions.Items[new OptionValuePair(this._selectedOption.OptionGuid, this._selectedValue.ID)] = this.currentLinkedList;
    List<OptionValuePair> optionValuePairList1 = new List<OptionValuePair>();
    optionValuePairList1.Add(optionValuePair);
    List<OptionValuePair> optionValuePairList2 = new List<OptionValuePair>();
    if (!this._fullLinkedOptions.CheckLinkedConflictExists(this.selectedOptionValuePair, optionValuePairList1, optionValuePairList2))
      return ErrorState.None;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.tbMoreInfo.Text = LinkedOptions.FormingPathString(sessionKeeper.Session, optionValuePairList1) + Environment.NewLine + LinkedOptions.FormingPathString(sessionKeeper.Session, optionValuePairList2);
    return ErrorState.LinkedConflict;
  }

  private bool IsIncompConflictExists(OptionValuePair linkedOptionValuePair)
  {
    this.currentLinkedList.Clear();
    foreach (TreeListNode node in this.tlLinkedOptions.Nodes)
    {
      if (node[(object) "LINKED_OPTION"] is MyElement myElement1 && !myElement1.Value.Equals((object) Guid.Empty))
      {
        Guid option = (Guid) myElement1.Value;
        MyElement myElement = node[(object) "OPTION_VALUE"] as MyElement;
        string id;
        if (linkedOptionValuePair != null && option == linkedOptionValuePair.Option)
          id = linkedOptionValuePair.ID;
        else if (myElement != null && !string.IsNullOrEmpty(myElement.Value.ToString()))
          id = myElement.Value.ToString();
        else
          continue;
        this.currentLinkedList.Add(new OptionValuePair(option, id));
      }
    }
    return LinkedOptions.IsIncompConflictExists(this._options, this.selectedOptionValuePair, this.currentLinkedList);
  }

  private bool IsIncompConflictExists() => this.IsIncompConflictExists((OptionValuePair) null);

  private bool IsOptionExists(Guid option, TreeListNode selNode)
  {
    foreach (TreeListNode node in this.tlLinkedOptions.Nodes)
    {
      if (selNode != node && node[(object) "LINKED_OPTION"] != null && node[(object) "LINKED_OPTION"] is MyElement myElement && !myElement.Value.Equals((object) Guid.Empty) && (Guid) myElement.Value == option)
        return true;
    }
    return false;
  }

  private void UpdateControls()
  {
    this.panelHint.Visible = this._accessRights != OptionAccessRights.FullAccess;
    if (this._options == null || this._selectedOption == null || this._selectedValue == null)
    {
      this.tbOptionWork.Enabled = this.cmsOptionWork.Enabled = this.tlLinkedOptions.Enabled = false;
      this.pbError.Visible = this.btnMore.Visible = this.tbMoreInfo.Visible = false;
    }
    else
    {
      TreeListNode focusedNode = this.tlLinkedOptions.FocusedNode;
      if (this._accessRights == OptionAccessRights.FullAccess)
      {
        this.tbMoreInfo.Visible = false;
        this.tbOptionWork.Enabled = this.cmsOptionWork.Enabled = this.tlLinkedOptions.Enabled = true;
        this.tsmDeleteOption.Enabled = this.btnDeleteOption.Enabled = focusedNode != null;
        this.tsmClear.Enabled = this.btnClear.Enabled = this.tlLinkedOptions.Nodes.Count > 0;
        this.tsmAddOption.Enabled = this.btnAddOption.Enabled = this.tlLinkedOptions.Nodes.Count < this._options.Options.Count - 1;
      }
      else
        this.tbOptionWork.Enabled = this.cmsOptionWork.Enabled = this.tlLinkedOptions.Enabled = false;
      if (focusedNode != null)
      {
        if (focusedNode[(object) "OPTION_ERROR"] == null)
          return;
        ErrorState errorState = (ErrorState) focusedNode[(object) "OPTION_ERROR"];
        this.lbErrorState.Text = EnumDescConverter.GetEnumDescription((Enum) errorState);
        this.pbError.Visible = errorState != ErrorState.None;
        this.btnMore.Visible = errorState == ErrorState.LinkedConflict;
        switch (errorState)
        {
          case ErrorState.Value:
          case ErrorState.ObsoleteOption:
            this.pbError.Image = this.ilError.Images[0];
            break;
          case ErrorState.ObsoleteOptionValue:
            this.pbError.Image = this.ilError.Images[2];
            break;
          default:
            this.pbError.Image = this.ilError.Images[1];
            break;
        }
      }
      else
      {
        this.lbErrorState.Text = string.Empty;
        this.pbError.Visible = this.btnMore.Visible = this.tbMoreInfo.Visible = false;
      }
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (ServicesManager.GetService(typeof (BarManager)) is BarManager service)
    {
      this.tbOptionWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      this.cmsOptionWork.Renderer = (IToolBarRenderer) new EmptyToolbarRenderer();
      service.RendererChanged -= new EventHandler(this.ToolbarRendererChanged);
    }
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LinkedOptionsEditor));
    this.tbOptionWork = new Intermech.Bars.ToolBar();
    this.ilLinked = new ImageList(this.components);
    this.btnAddOption = new ButtonItem();
    this.btnDeleteOption = new ButtonItem();
    this.btnClear = new ButtonItem();
    this.toolTip = new ToolTip(this.components);
    this.tlLinkedOptions = new TreeList();
    this.treeListColumn1 = new TreeListColumn();
    this.treeListColumn2 = new TreeListColumn();
    this.treeListColumn3 = new TreeListColumn();
    this.cmsOptionWork = new MenuBar();
    this.contextMenuBarItem = new ContextMenuBarItem();
    this.tsmAddOption = new MenuButtonItem();
    this.tsmDeleteOption = new MenuButtonItem();
    this.tsmClear = new MenuButtonItem();
    this.panel1 = new Panel();
    this.btnMore = new Button();
    this.pbError = new PictureBox();
    this.lbErrorState = new Label();
    this.tbMoreInfo = new TextBox();
    this.splitter1 = new Splitter();
    this.panelHint = new Panel();
    this.labelWarning = new Label();
    this.pictureHint = new PictureBox();
    this.ilError = new ImageList(this.components);
    this.tlLinkedOptions.BeginInit();
    this.panel1.SuspendLayout();
    ((ISupportInitialize) this.pbError).BeginInit();
    this.panelHint.SuspendLayout();
    ((ISupportInitialize) this.pictureHint).BeginInit();
    this.SuspendLayout();
    this.tbOptionWork.FullMenus = true;
    this.tbOptionWork.Guid = new Guid("37056402-c6d1-47d4-be0f-e941c1a06e55");
    this.tbOptionWork.Hidden = false;
    this.tbOptionWork.ImageList = this.ilLinked;
    this.tbOptionWork.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.btnAddOption,
      (ToolbarItemBase) this.btnDeleteOption,
      (ToolbarItemBase) this.btnClear
    });
    componentResourceManager.ApplyResources((object) this.tbOptionWork, "tbOptionWork");
    this.tbOptionWork.Name = "tbOptionWork";
    this.tbOptionWork.Tag = (object) "";
    this.ilLinked.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilLinked.ImageStream");
    this.ilLinked.TransparentColor = Color.Transparent;
    this.ilLinked.Images.SetKeyName(0, "error.gif");
    this.ilLinked.Images.SetKeyName(1, "add.png");
    this.ilLinked.Images.SetKeyName(2, "delete.png");
    this.ilLinked.Images.SetKeyName(3, "document.png");
    this.ilLinked.Images.SetKeyName(4, "garbage.png");
    this.ilLinked.Images.SetKeyName(5, "gear_warning.png");
    this.btnAddOption.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnAddOption, "btnAddOption");
    this.btnAddOption.Enabled = false;
    this.btnAddOption.ImageIndex = 1;
    this.btnAddOption.Click += new EventHandler(this.btnAddOption_Click);
    componentResourceManager.ApplyResources((object) this.btnDeleteOption, "btnDeleteOption");
    this.btnDeleteOption.Enabled = false;
    this.btnDeleteOption.ImageIndex = 2;
    this.btnDeleteOption.Click += new EventHandler(this.btnDeleteOption_Click);
    this.btnClear.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.btnClear, "btnClear");
    this.btnClear.Enabled = false;
    this.btnClear.ImageIndex = 3;
    this.btnClear.Click += new EventHandler(this.btnClear_Click);
    this.tlLinkedOptions.Columns.AddRange(new TreeListColumn[3]
    {
      this.treeListColumn1,
      this.treeListColumn2,
      this.treeListColumn3
    });
    componentResourceManager.ApplyResources((object) this.tlLinkedOptions, "tlLinkedOptions");
    this.tlLinkedOptions.IndicatorWidth = 4;
    this.tlLinkedOptions.Name = "tlLinkedOptions";
    this.cmsOptionWork.SetPopupMenu((Control) this.tlLinkedOptions, (MenuBarItem) this.contextMenuBarItem);
    this.tlLinkedOptions.StateImageList = this.ilLinked;
    this.tlLinkedOptions.TreeLineStyle = LineStyle.None;
    this.tlLinkedOptions.FocusedNodeChanged += new FocusedNodeChangedEventHandler(this.tlLinkedOptions_FocusedNodeChanged);
    this.tlLinkedOptions.CellValueChanging += new CellValueChangedEventHandler(this.tlLinkedOptions_CellValueChanging);
    this.tlLinkedOptions.ShowingEditor += new CancelEventHandler(this.tlLinkedOptions_ShowingEditor);
    componentResourceManager.ApplyResources((object) this.treeListColumn1, "treeListColumn1");
    this.treeListColumn1.Name = "treeListColumn1";
    componentResourceManager.ApplyResources((object) this.treeListColumn2, "treeListColumn2");
    this.treeListColumn2.Name = "treeListColumn2";
    componentResourceManager.ApplyResources((object) this.treeListColumn3, "treeListColumn3");
    this.treeListColumn3.Name = "treeListColumn3";
    componentResourceManager.ApplyResources((object) this.cmsOptionWork, "cmsOptionWork");
    this.cmsOptionWork.Guid = new Guid("0909a734-928b-4c5d-9a6d-05be64690c06");
    this.cmsOptionWork.Hidden = false;
    this.cmsOptionWork.ImageList = this.ilLinked;
    this.cmsOptionWork.Items.AddRange(new ToolbarItemBase[1]
    {
      (ToolbarItemBase) this.contextMenuBarItem
    });
    this.cmsOptionWork.Name = "cmsOptionWork";
    this.cmsOptionWork.OwnerForm = (Form) null;
    componentResourceManager.ApplyResources((object) this.contextMenuBarItem, "contextMenuBarItem");
    this.contextMenuBarItem.Items.AddRange(new ToolbarItemBase[3]
    {
      (ToolbarItemBase) this.tsmAddOption,
      (ToolbarItemBase) this.tsmDeleteOption,
      (ToolbarItemBase) this.tsmClear
    });
    this.contextMenuBarItem.ShowText = true;
    componentResourceManager.ApplyResources((object) this.tsmAddOption, "tsmAddOption");
    this.tsmAddOption.ImageIndex = 1;
    this.tsmAddOption.ShowText = true;
    this.tsmAddOption.Click += new EventHandler(this.btnAddOption_Click);
    componentResourceManager.ApplyResources((object) this.tsmDeleteOption, "tsmDeleteOption");
    this.tsmDeleteOption.ImageIndex = 2;
    this.tsmDeleteOption.ShowText = true;
    this.tsmDeleteOption.Click += new EventHandler(this.btnDeleteOption_Click);
    this.tsmClear.BeginGroup = true;
    componentResourceManager.ApplyResources((object) this.tsmClear, "tsmClear");
    this.tsmClear.ImageIndex = 3;
    this.tsmClear.ShowText = true;
    this.tsmClear.Click += new EventHandler(this.btnClear_Click);
    this.panel1.Controls.Add((Control) this.btnMore);
    this.panel1.Controls.Add((Control) this.pbError);
    this.panel1.Controls.Add((Control) this.lbErrorState);
    componentResourceManager.ApplyResources((object) this.panel1, "panel1");
    this.panel1.Name = "panel1";
    componentResourceManager.ApplyResources((object) this.btnMore, "btnMore");
    this.btnMore.Name = "btnMore";
    this.btnMore.UseVisualStyleBackColor = true;
    this.btnMore.Click += new EventHandler(this.btnMore_Click);
    componentResourceManager.ApplyResources((object) this.pbError, "pbError");
    this.pbError.Name = "pbError";
    this.pbError.TabStop = false;
    componentResourceManager.ApplyResources((object) this.lbErrorState, "lbErrorState");
    this.lbErrorState.Name = "lbErrorState";
    componentResourceManager.ApplyResources((object) this.tbMoreInfo, "tbMoreInfo");
    this.tbMoreInfo.Name = "tbMoreInfo";
    this.tbMoreInfo.ReadOnly = true;
    componentResourceManager.ApplyResources((object) this.splitter1, "splitter1");
    this.splitter1.Name = "splitter1";
    this.splitter1.TabStop = false;
    this.panelHint.BorderStyle = BorderStyle.Fixed3D;
    this.panelHint.Controls.Add((Control) this.labelWarning);
    this.panelHint.Controls.Add((Control) this.pictureHint);
    componentResourceManager.ApplyResources((object) this.panelHint, "panelHint");
    this.panelHint.Name = "panelHint";
    componentResourceManager.ApplyResources((object) this.labelWarning, "labelWarning");
    this.labelWarning.Name = "labelWarning";
    componentResourceManager.ApplyResources((object) this.pictureHint, "pictureHint");
    this.pictureHint.Name = "pictureHint";
    this.pictureHint.TabStop = false;
    this.ilError.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("ilError.ImageStream");
    this.ilError.TransparentColor = Color.Transparent;
    this.ilError.Images.SetKeyName(0, "gear_warning.png");
    this.ilError.Images.SetKeyName(1, "delete.png");
    this.ilError.Images.SetKeyName(2, "garbage.png");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.splitter1);
    this.Controls.Add((Control) this.tlLinkedOptions);
    this.Controls.Add((Control) this.tbOptionWork);
    this.Controls.Add((Control) this.tbMoreInfo);
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.panelHint);
    this.Controls.Add((Control) this.cmsOptionWork);
    this.Name = nameof (LinkedOptionsEditor);
    this.tlLinkedOptions.EndInit();
    this.panel1.ResumeLayout(false);
    this.panel1.PerformLayout();
    ((ISupportInitialize) this.pbError).EndInit();
    this.panelHint.ResumeLayout(false);
    ((ISupportInitialize) this.pictureHint).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  public delegate void ObjectOptionsChangedEventHandler(object sender, EventArgs e);
}
