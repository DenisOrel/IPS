// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHView
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase.Selection;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MaterialsHandbook;

[ToolboxItem(false)]
[ViewDescriptionProvider(typeof (IMHView.IMHViewDescriptionProvider))]
public class IMHView : UserControl, IView, ICanCloseViews, ICanDeactivateView, ISelectedItemsHost
{
  private IMHViewCtrlBase _ctrl;
  private IMHView.IMHSelectedItems _selectedItems;
  private IMHSelector _selectorService;
  private ImbaseSelector _imbaseSelectorService;
  private IContainer components;

  internal IMHViewCtrlBase iMHViewCtrlBase => this._ctrl;

  public IMHView()
  {
    this.InitializeComponent();
    this._selectorService = ServiceUtils.GetService<IIMHSelector>((object) ApplicationServices.Container, false) as IMHSelector;
    this._imbaseSelectorService = ServiceUtils.GetService<IImbaseSelector>((object) ApplicationServices.Container, false) as ImbaseSelector;
  }

  private void On_ctrl_IMHMaterialChanged(object sender, IMHMaterialChangedEventArgs e)
  {
    if (e != null)
      this._selectedItems.SetData(e.TebleRefID, e.RecordID, e.Selectable, e.Designation);
    this.OnSelectedItemsChanged();
  }

  public bool CanClose(object sender) => true;

  public bool CanDeactivate(object sender) => this.CanClose(sender);

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    NavigatorTreeNode parentNode = this.GetParentNode(items);
    NodeIDPath parentPath = items.GetParentPath(0);
    INodeID itemId = items.GetItemID(0);
    INodeID nodeID = itemId;
    NodeIDPath handlerPath = new NodeIDPath(parentPath, nodeID);
    INode itemData1 = (INode) items.GetItemData(0, typeof (INode));
    INode handler = itemData1.GetChild(itemId) ?? itemData1.GetData(itemId, typeof (INode)) as INode;
    if (handler is IContextAware contextAware)
      contextAware.Services = provider;
    this._selectedItems = new IMHView.IMHSelectedItems(handlerPath, handler, this);
    INamedImageList service = ServiceUtils.GetService<INamedImageList>((object) ApplicationServices.Container, false);
    this.SuspendLayout();
    try
    {
      if (items.GetItemData(0, typeof (IIMHNode)) is IIMHNode itemData2)
      {
        Type type1 = this._ctrl?.GetType();
        Type type2 = (Type) null;
        string name = string.Empty;
        if (itemData2.ParentCategoryID == Consts.IMHGluesHandbookNodeCategoryID)
        {
          name = "imgGlue";
          this.Caption = LocalizationHolder.rm.GetString("IMH_Glues");
          type2 = typeof (IMHGluesViewCtrl);
        }
        else if (itemData2.ParentCategoryID == Consts.IMHCoatingsVarietiesNodeCategoryID)
        {
          name = "icoCoating";
          this.Caption = LocalizationHolder.rm.GetString("IMH_CoatingsVarieties");
          type2 = typeof (IMHCoatingsVarietiesViewCtrl);
        }
        else if (itemData2.ParentCategoryID == Consts.IMHDetailsMaterialNodeCategoryID)
        {
          name = "icoMaterialCoating";
          this.Caption = LocalizationHolder.rm.GetString("IMH_DetailsMaterial");
          type2 = typeof (IMHDetailsMaterialViewCtrl);
        }
        else if (itemData2.ParentCategoryID == Consts.IMHMaterialsNodeCategoryID)
        {
          name = "icoHandbookMaterials";
          this.Caption = LocalizationHolder.rm.GetString("IMH_MaterialsNode_Caption");
          type2 = typeof (IMHMaterialsViewCtrl);
        }
        else if (itemData2.ParentCategoryID == Consts.IMHProfilesNodeCategoryID)
        {
          name = "icoProfiles";
          this.Caption = LocalizationHolder.rm.GetString("IMH_ProfilesNode_Caption");
          type2 = typeof (IMHProfilesViewCtrl);
        }
        else if (itemData2.ParentCategoryID == Consts.IMHAssortmentNodeCategoryID)
        {
          name = "imgAssortment";
          this.Caption = LocalizationHolder.rm.GetString("IMH_AssortmentsNode_Caption");
          type2 = typeof (IMHAssortmentViewCtrl);
        }
        else if (itemData2.ParentCategoryID == Consts.IMHOilHandbookNodeCategoryID)
        {
          name = "icoOils";
          this.Caption = LocalizationHolder.rm.GetString("IMH_OilHandbookNode_Caption");
          type2 = typeof (IMHOilViewCtrl);
        }
        else if (itemData2.ParentCategoryID == Consts.IMHVarnishHandbookNodeCategoryID)
        {
          name = "icoVarnish";
          this.Caption = LocalizationHolder.rm.GetString("IMH_VarnishHandbookNode_Caption");
          type2 = typeof (IMHVarnishViewCtrl);
        }
        if (service != null)
          this.ImageIndex = service.ImageIndex(name);
        if (type1 != (Type) null && type1 != type2)
        {
          this._ctrl.IMHMaterialChanged -= new EventHandler<IMHMaterialChangedEventArgs>(this.On_ctrl_IMHMaterialChanged);
          this.Controls.Remove((Control) this._ctrl);
        }
        if (type1 != type2)
        {
          this._ctrl = (IMHViewCtrlBase) Activator.CreateInstance(type2 ?? throw new InvalidOperationException());
          this._ctrl.IMHMaterialChanged += new EventHandler<IMHMaterialChangedEventArgs>(this.On_ctrl_IMHMaterialChanged);
          this._ctrl.Dock = DockStyle.Fill;
          this.Controls.Add((Control) this._ctrl);
        }
      }
      else if (this._ctrl != null)
      {
        this._ctrl.IMHMaterialChanged -= new EventHandler<IMHMaterialChangedEventArgs>(this.On_ctrl_IMHMaterialChanged);
        this.Controls.Remove((Control) this._ctrl);
      }
    }
    finally
    {
      this.ResumeLayout();
    }
    this._ctrl?.Initialize(items, provider, parentNode);
  }

  public void Activate(IView previousView)
  {
    if (this._ctrl == null || previousView == PageViewsManager.BlackHoleView)
      return;
    this._ctrl.Activate(previousView);
  }

  public void Deactivate(IView nextView)
  {
    this._ctrl?.Deactivate(nextView);
    if (this._selectedItems == null || nextView == PageViewsManager.BlackHoleView)
      return;
    this._selectedItems.SetData(0L, -1L, false);
  }

  public string Caption { get; private set; }

  public int ImageIndex { get; private set; }

  public int OrderID => 0;

  private NavigatorTreeNode GetParentNode(ISelectedItems items)
  {
    NavigatorTreeNode parentNode = (NavigatorTreeNode) null;
    if (items is NavigatorTreeViewSelectedItems viewSelectedItems && viewSelectedItems.Nodes.Length != 0)
    {
      parentNode = viewSelectedItems.Nodes[0];
      if (parentNode != null)
      {
        while (parentNode != null && parentNode.Level > 1 && parentNode.NodeID.TypeID == Intermech.Imbase.Consts.ImbaseFolderTypeID)
          parentNode = parentNode.Parent;
      }
    }
    return parentNode;
  }

  private void OnSelectedItemsChanged()
  {
    EventHandler selectedItemsChanged = this.SelectedItemsChanged;
    if (selectedItemsChanged == null)
      return;
    selectedItemsChanged((object) this, EventArgs.Empty);
  }

  public ISelectedItems SelectedItems => (ISelectedItems) this._selectedItems;

  public event EventHandler SelectedItemsChanged;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IMHView));
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.DoubleBuffered = true;
    this.Name = nameof (IMHView);
    this.ResumeLayout(false);
  }

  public class IMHSelectedItems : ISelectedItems, ISimpleSelectedItems
  {
    private NodeItems _nodeItems;
    private IMHView _view;
    private bool _valid;
    private long _tableRefID;
    private long _recID = -1;
    private string _designation = string.Empty;

    public bool Selectable { get; private set; }

    public IMHSelectedItems(NodeIDPath handlerPath, INode handler, IMHView owner)
    {
      this._nodeItems = new NodeItems(handlerPath, handler, new NodeIDCollection(), (IServiceProvider) null);
      this._view = owner;
      this.Selectable = true;
    }

    internal void SetData(long tableRefID, long recID, bool selectable, string designation = "")
    {
      this._view._selectorService.TableRefID = tableRefID;
      this._view._selectorService.Designation = designation;
      this._view._imbaseSelectorService.ContextObjectId = tableRefID;
      this._tableRefID = tableRefID;
      this._recID = recID;
      this.Selectable = selectable;
      this._designation = designation;
      this._valid = false;
      this.Validate();
    }

    public bool IsCollage
    {
      get
      {
        this.Validate();
        return this._nodeItems.IsCollage;
      }
    }

    public int Count
    {
      get
      {
        this.Validate();
        return this._nodeItems.Count;
      }
    }

    public object GetItemData(int index, Type dataFormat)
    {
      this.Validate();
      return (object) new IMHMaterialRecordID(this._tableRefID, this._recID, this._designation);
    }

    public INodeID GetItemID(int index)
    {
      this.Validate();
      return this._nodeItems.GetItemID(index);
    }

    public object GetParentData(int index, Type dataFormat)
    {
      this.Validate();
      return this._nodeItems.GetParentData(index, dataFormat);
    }

    public NodeIDPath GetParentPath(int index)
    {
      this.Validate();
      return this._nodeItems.GetParentPath(index);
    }

    private void Validate()
    {
      if (this._valid)
        return;
      NodeIDCollection nodeIds = this._nodeItems.NodeIDs;
      nodeIds.Clear();
      try
      {
        this._valid = true;
        if (this._recID <= -1L)
          return;
        nodeIds.Add((INodeID) new IMHMaterialRecordNodeID(new IMHMaterialRecordID(this._tableRefID, this._recID, this._designation)));
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
        this._valid = false;
        nodeIds.Clear();
      }
    }
  }

  private sealed class IMHViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList service))
        service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      INamedImageList namedImageList = service;
      string empty = string.Empty;
      string name = string.Empty;
      if (selectedItems.GetItemData(0, typeof (IIMHNode)) is IIMHNode itemData)
      {
        if (itemData.ParentCategoryID == Consts.IMHGluesHandbookNodeCategoryID)
        {
          name = "imgGlue";
          empty = LocalizationHolder.rm.GetString("IMH_Glues");
        }
        else if (itemData.ParentCategoryID == Consts.IMHCoatingsVarietiesNodeCategoryID)
        {
          name = "icoCoating";
          empty = LocalizationHolder.rm.GetString("IMH_CoatingsVarieties");
        }
        else if (itemData.ParentCategoryID == Consts.IMHDetailsMaterialNodeCategoryID)
        {
          name = "icoMaterialCoating";
          empty = LocalizationHolder.rm.GetString("IMH_DetailsMaterial");
        }
        else if (itemData.ParentCategoryID == Consts.IMHMaterialsNodeCategoryID)
        {
          name = "icoHandbookMaterials";
          empty = LocalizationHolder.rm.GetString("IMH_MaterialsNode_Caption");
        }
        else if (itemData.ParentCategoryID == Consts.IMHProfilesNodeCategoryID)
        {
          name = "icoProfiles";
          empty = LocalizationHolder.rm.GetString("IMH_ProfilesNode_Caption");
        }
        else if (itemData.ParentCategoryID == Consts.IMHAssortmentNodeCategoryID)
        {
          name = "imgAssortment";
          empty = LocalizationHolder.rm.GetString("IMH_AssortmentsNode_Caption");
        }
        else if (itemData.ParentCategoryID == Consts.IMHOilHandbookNodeCategoryID)
        {
          name = "icoOils";
          empty = LocalizationHolder.rm.GetString("IMH_OilHandbookNode_Caption");
        }
        else if (itemData.ParentCategoryID == Consts.IMHVarnishHandbookNodeCategoryID)
        {
          name = "icoVarnish";
          empty = LocalizationHolder.rm.GetString("IMH_VarnishHandbookNode_Caption");
        }
      }
      return new ViewDescription()
      {
        Caption = empty,
        ImageIndex = namedImageList.ImageIndex(name),
        OrderID = 0
      };
    }
  }
}
