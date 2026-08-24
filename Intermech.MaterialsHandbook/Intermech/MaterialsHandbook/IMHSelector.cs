// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHSelector
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.DataFormats;
using Intermech.Imbase;
using Intermech.Imbase.Views;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.Imbase.Params;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Localization;
using Intermech.Navigator;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHSelector : IIMHSelector
{
  private long _tableRefID;
  private long _recID = -1;
  private bool _preferImbase;
  private string _designation = string.Empty;
  private bool _lock;

  public long TableRefID
  {
    get => this._tableRefID;
    set
    {
      if (this._lock)
        return;
      this._tableRefID = value;
    }
  }

  public string Designation
  {
    get => this._designation;
    set
    {
      if (this._lock)
        return;
      this._designation = value;
    }
  }

  public bool PreferImbase
  {
    get => this._preferImbase;
    set => this._preferImbase = value;
  }

  private void SelectionWindow_OnSelectionWindowBeforeShow(object sender, EventArgs e)
  {
    this._lock = false;
    if (this._tableRefID == 0L)
      return;
    NavigatorTreeView navTreeView = ((Intermech.Navigator.Controls.SelectionWindow) sender).NavTreeView;
    if (navTreeView == null || !this._preferImbase && this.SearchAndSelectMaterial((object) navTreeView.RootNode, this._tableRefID, this._recID))
      return;
    NavigatorTreeNode node;
    if ((node = FindHelper.SearchNodeByNodeID(navTreeView.RootNode, this._tableRefID)) != null)
    {
      SelectedRecords.Clear();
      SelectedRecords.Add(this._tableRefID, new long[1]
      {
        this._recID
      });
      NodeIDPath nodeIdPath = navTreeView.GetNodeIDPath(node);
      navTreeView.TryBrowse(nodeIdPath);
    }
    this._tableRefID = 0L;
  }

  public IDescriptor GetMaterialsHandbookDescriptor()
  {
    string caption = LocalizationHolder.rm.GetString("IMH_RootNode_Caption");
    return (IDescriptor) new VirtualNodeDescriptor(Consts.IMHRootNodeCategoryID, Consts.IMHRootNodeCategoryID, caption);
  }

  public long SelectMaterial(
    string caption,
    string description,
    object descriptorCollection,
    int needType,
    long contextObjsID)
  {
    long num = contextObjsID;
    this._tableRefID = 0L;
    IDescriptor rootDescriptor;
    if (descriptorCollection is DescriptorCollection descriptors)
    {
      descriptors.Add(this.GetMaterialsHandbookDescriptor());
      rootDescriptor = (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor(LocalizationHolder.rm.GetString("IMH_SelectaMaterial"), descriptors);
    }
    else
      rootDescriptor = this.GetMaterialsHandbookDescriptor();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ImbaseHelper.GetImbaseDataFromObject(sessionKeeper.Session, contextObjsID, ref this._tableRefID, ref this._recID);
      this._preferImbase = !ServiceUtils.GetService<IImbaseParamsService>((object) sessionKeeper.Session, true).GetUserParams(sessionKeeper.Session.SessionGUID).UseIMHSelector;
    }
    try
    {
      this._lock = this._tableRefID != 0L && this._recID != -1L;
      if (this._lock)
        Intermech.Navigator.SelectionWindow.OnSelectionWindowBeforeShow += new SelectionWindowBeforeShow(this.SelectionWindow_OnSelectionWindowBeforeShow);
      Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new SelectedMaterialAnalizer(), true);
      SelectionOptions options = SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect;
      long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(caption, description, rootDescriptor, options);
      this._lock = false;
      if (numArray != null && numArray.Length != 0)
      {
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IImbaseServer customService = sessionKeeper.Session.GetCustomService(typeof (IImbaseServer)) as IImbaseServer;
          if (this._tableRefID != 0L)
          {
            if (customService != null)
              num = customService.CreateObject(sessionKeeper.Session.SessionGUID, 0L, this._tableRefID, numArray[0], true, needType);
          }
          else
          {
            IImbaseSelector service;
            if ((service = ServiceUtils.GetService<IImbaseSelector>((object) ApplicationServices.Container, false)) != null && service.ContextObjectId != 0L && service.ContextObjectId != -1L)
            {
              if (customService != null)
                num = customService.CreateObject(sessionKeeper.Session.SessionGUID, 0L, service.ContextObjectId, numArray[0], true, needType);
            }
            else
              num = numArray[0];
          }
        }
      }
      else
        num = 0L;
    }
    finally
    {
      Intermech.Navigator.SelectionWindow.OnSelectionWindowBeforeShow -= new SelectionWindowBeforeShow(this.SelectionWindow_OnSelectionWindowBeforeShow);
    }
    return num;
  }

  public List<string> SelectMaterial(bool useGuid, bool multiSelect)
  {
    List<string> stringList = new List<string>(1);
    this._tableRefID = 0L;
    Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new SelectedMaterialAnalizer(), true);
    string caption = LocalizationHolder.rm.GetString("IMH_MaterialsNode_Caption");
    IDescriptor rootDescriptor = (IDescriptor) new VirtualNodeDescriptor(Consts.IMHMaterialsHandbookNodeCategoryID, Consts.IMHMaterialsNodeCategoryID, caption);
    SelectionOptions options = SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableSelectAbstractTypes;
    if (!multiSelect)
      options |= SelectionOptions.DisableMultiselect;
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("IMH_SelectMaterial"), LocalizationHolder.rm.GetString("IMH_SelectSubstitute"), rootDescriptor, options);
    if (numArray != null && numArray.Length != 0)
    {
      foreach (long recordId in numArray)
      {
        string keyValue = ImbaseHelper.MakeInternalImbaseKey(this._tableRefID, recordId);
        if (useGuid)
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
            keyValue = ImbaseHelper.ConvertImbaseKey(sessionKeeper.Session, keyValue);
        }
        if (!stringList.Contains(keyValue))
          stringList.Add(keyValue);
      }
    }
    return stringList;
  }

  public Tuple<long, long> SelectMaterial(
    string caption,
    string description,
    object descriptorCollection,
    long contextObjsID)
  {
    Tuple<long, long> tuple = (Tuple<long, long>) null;
    this._tableRefID = 0L;
    string caption1 = LocalizationHolder.rm.GetString("IMH_RootNode_Caption");
    IDescriptor rootDescriptor;
    if (descriptorCollection is DescriptorCollection descriptors)
    {
      descriptors.Add((IDescriptor) new VirtualNodeDescriptor(Consts.IMHMaterialsHandbookNodeCategoryID, Consts.IMHMaterialsHandbookNodeCategoryID, caption1));
      rootDescriptor = (IDescriptor) new Intermech.Navigator.CustomNode.Descriptor(LocalizationHolder.rm.GetString("IMH_RootNode_CatalogsMaterials"), descriptors);
    }
    else
      rootDescriptor = (IDescriptor) new VirtualNodeDescriptor(Consts.IMHMaterialsHandbookNodeCategoryID, Consts.IMHMaterialsHandbookNodeCategoryID, caption1);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      ImbaseHelper.GetImbaseDataFromObject(sessionKeeper.Session, contextObjsID, ref this._tableRefID, ref this._recID);
    bool flag = false;
    if (this._tableRefID != 0L && this._recID != -1L)
    {
      flag = true;
      Intermech.Navigator.SelectionWindow.OnSelectionWindowBeforeShow += new SelectionWindowBeforeShow(this.SelectionWindow_OnSelectionWindowBeforeShow);
    }
    Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new SelectedMaterialAnalizer(), true);
    SelectionOptions options = SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableSelectAbstractTypes | SelectionOptions.DisableMultiselect;
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(caption, description, rootDescriptor, options);
    if (flag)
      Intermech.Navigator.SelectionWindow.OnSelectionWindowBeforeShow -= new SelectionWindowBeforeShow(this.SelectionWindow_OnSelectionWindowBeforeShow);
    if (numArray != null && numArray.Length != 0)
    {
      IImbaseSelector service;
      tuple = this._tableRefID == 0L ? ((service = ServiceUtils.GetService<IImbaseSelector>((object) ApplicationServices.Container, false)) == null || service.ContextObjectId == 0L ? new Tuple<long, long>(numArray[0], -1L) : new Tuple<long, long>(service.ContextObjectId, numArray[0])) : new Tuple<long, long>(this._tableRefID, numArray[0]);
    }
    return tuple;
  }

  public bool IsMaterialsHandbookItem(ISelectedItems selectedItems, out bool selected)
  {
    selected = false;
    IMHView.IMHSelectedItems imhSelectedItems = selectedItems as IMHView.IMHSelectedItems;
    int num = imhSelectedItems != null ? 1 : 0;
    if (imhSelectedItems == null)
      return num != 0;
    if (!imhSelectedItems.Selectable)
      return num != 0;
    selected = imhSelectedItems.GetItemData(0, (Type) null) is IMHMaterialRecordID itemData && itemData.ID != 0L && itemData.Value > -1L;
    return num != 0;
  }

  public string SelectCoatingDesignation() => this.SelectCoatingOrGlueDesignation(true);

  public string SelectGlueDesignation() => this.SelectCoatingOrGlueDesignation(false);

  public bool SearchAndSelectMaterial(object node, long tableRefID, long recID)
  {
    bool flag = false;
    this._tableRefID = tableRefID;
    this._recID = recID;
    if (this.SearchMaterial(node, tableRefID, recID) is NavigatorTreeNode node1)
    {
      NavigatorTreeView tree = node is NavigatorTreeNode navigatorTreeNode ? navigatorTreeNode.Tree : (NavigatorTreeView) null;
      NodeIDPath nodeIdPath = tree?.GetNodeIDPath(node1);
      flag = nodeIdPath != null;
      if (nodeIdPath != null)
        tree.TryBrowse(nodeIdPath);
    }
    return flag;
  }

  public object SearchMaterial(object node, long tableRefID, long recID)
  {
    this._tableRefID = tableRefID;
    this._recID = recID;
    if (!(node is NavigatorTreeNode parentNode) || tableRefID == 0L)
      return (object) null;
    NavigatorTreeView tree = parentNode.Tree;
    if (tree == null || tree.IsDisposed)
      return (object) null;
    long nodeCategoryID = -1;
    NavigatorTreeNode navigatorTreeNode;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IIMHSystemSettingsService service = ServiceUtils.GetService<IIMHSystemSettingsService>((object) sessionKeeper.Session, true);
      DataTable imbaseTableTree = IMHHelper.GetImbaseTableTree(tableRefID);
      if (imbaseTableTree != null && imbaseTableTree.Rows.Count > 0 && imbaseTableTree.Columns.Contains("F_OBJECT_ID"))
      {
        IIMHSystemSettingsService systemSettingsService = service;
        foreach (KeyValuePair<string, Guid> objectGuidsByName in systemSettingsService.GetObjectGuidsByNames(new List<string>()
        {
          "GLUE_FOLDER_NAME",
          "VARNISH_FOLDER_NAME",
          "OIL_FOLDER_NAME",
          "COATING_FOLDER_NAME",
          "BASE_MATERIALS_CTL",
          "ASSORTMENT_FOLDER_NAME"
        }))
        {
          QuickObjectInfo info = sessionKeeper.Session.GetObjectInfo(objectGuidsByName.Value);
          if (!info.Empty && imbaseTableTree.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToInt64(x["F_OBJECT_ID"]) == info.ObjectID)) != null)
          {
            switch (objectGuidsByName.Key)
            {
              case "BASE_MATERIALS_CTL":
                nodeCategoryID = (long) Consts.IMHMaterialsNodeCategoryID;
                goto label_19;
              case "ASSORTMENT_FOLDER_NAME":
                nodeCategoryID = (long) Consts.IMHAssortmentNodeCategoryID;
                goto label_19;
              case "GLUE_FOLDER_NAME":
                nodeCategoryID = (long) Consts.IMHGluesHandbookNodeCategoryID;
                goto label_19;
              case "VARNISH_FOLDER_NAME":
                nodeCategoryID = (long) Consts.IMHVarnishHandbookNodeCategoryID;
                goto label_19;
              case "OIL_FOLDER_NAME":
                nodeCategoryID = (long) Consts.IMHOilHandbookNodeCategoryID;
                goto label_19;
              case "COATING_FOLDER_NAME":
                nodeCategoryID = (long) Consts.IMHCoatingsVarietiesNodeCategoryID;
                goto label_19;
              default:
                nodeCategoryID = -1L;
                goto label_19;
            }
          }
        }
      }
label_19:
      if (nodeCategoryID == -1L)
        return (object) null;
      NavigatorTreeNode materialNode = this.GetMaterialNode(parentNode, nodeCategoryID);
      if (materialNode == null)
        return (object) null;
      tree.FocusedNode = materialNode;
      tree.SetNodeExpanded(materialNode, true);
      while (!materialNode.Full)
        Thread.Sleep(50);
      long parentId = IMHHelper.GetParentID(imbaseTableTree, tableRefID);
      if (parentId == 0L)
        return (object) null;
      navigatorTreeNode = this.SearchFolderNode(materialNode, parentId, imbaseTableTree);
      if (!(navigatorTreeNode?.Handler is FolderNode handler))
        return (object) navigatorTreeNode;
      if (nodeCategoryID == (long) Consts.IMHAssortmentNodeCategoryID)
      {
        long linkId = 0;
        long recordId = -1;
        IDBObject objectActualCopy = sessionKeeper.Session.GetObjectActualCopy(this._tableRefID, false);
        if (objectActualCopy != null)
        {
          Guid objectGuidByName = service.GetObjectGuidByName("BASE_MATERIAL_ATTR");
          if (objectGuidByName != Guid.Empty)
          {
            IDBAttribute attributeByGuid = objectActualCopy.GetAttributeByGuid(objectGuidByName);
            if (attributeByGuid != null)
              ImbaseHelper.TryParseRecordReference(sessionKeeper.Session, attributeByGuid.AsString, out linkId, out recordId);
          }
        }
        handler.SelectedMaterialTableRefID = linkId;
        handler.SelectedMaterialRecID = recordId;
        handler.SelectedAssortmentTableRefID = this._tableRefID;
        handler.SelectedAssortmentRecID = this._recID;
      }
      else
      {
        handler.SelectedMaterialTableRefID = this._tableRefID;
        handler.SelectedMaterialRecID = this._recID;
      }
    }
    return (object) navigatorTreeNode;
  }

  private NavigatorTreeNode GetMaterialNode(NavigatorTreeNode parentNode, long nodeCategoryID)
  {
    if ((long) parentNode.NodeID.CategoryID == nodeCategoryID)
      return parentNode;
    NavigatorTreeNode node = (NavigatorTreeNode) null;
    if (parentNode.NodeID.CategoryID == Consts.IMHRootNodeCategoryID)
    {
      node = parentNode;
    }
    else
    {
      foreach (NavigatorTreeNode child in (List<NavigatorTreeNode>) parentNode.Children)
      {
        NavigatorTreeNode navigatorTreeNode;
        if ((navigatorTreeNode = child) != null && navigatorTreeNode.NodeID.CategoryID == Consts.IMHRootNodeCategoryID)
        {
          node = navigatorTreeNode;
          navigatorTreeNode.Tree.FocusedNode = node;
          navigatorTreeNode.Tree.SetNodeExpanded(node, true);
          while (!node.Full)
            Thread.Sleep(50);
          break;
        }
      }
    }
    if (node != null)
    {
      foreach (NavigatorTreeNode child1 in (List<NavigatorTreeNode>) node.Children)
      {
        NavigatorTreeNode materialNode;
        if ((materialNode = child1) != null)
        {
          if (materialNode.NodeID.CategoryID == Consts.IMHMaterialsHandbookNodeCategoryID)
          {
            materialNode.Tree.FocusedNode = child1;
            materialNode.Tree.SetNodeExpanded(child1, true);
            foreach (NavigatorTreeNode child2 in (List<NavigatorTreeNode>) child1.Children)
            {
              NavigatorTreeNode navigatorTreeNode;
              if ((navigatorTreeNode = child2) != null && (long) navigatorTreeNode.NodeID.CategoryID == nodeCategoryID)
                return child2;
            }
          }
          else if (materialNode.NodeID.CategoryID == Consts.IMHCoatingsHandbookNodeCategoryID)
          {
            materialNode.Tree.FocusedNode = child1;
            materialNode.Tree.SetNodeExpanded(child1, true);
            foreach (NavigatorTreeNode child3 in (List<NavigatorTreeNode>) child1.Children)
            {
              NavigatorTreeNode navigatorTreeNode;
              if ((navigatorTreeNode = child3) != null && (long) navigatorTreeNode.NodeID.CategoryID == nodeCategoryID)
                return child3;
            }
          }
          else if ((long) materialNode.NodeID.CategoryID == nodeCategoryID)
            return materialNode;
        }
      }
    }
    return (NavigatorTreeNode) null;
  }

  private NavigatorTreeNode SearchFolderNode(
    NavigatorTreeNode parentNode,
    long folderID,
    DataTable dt)
  {
    NavigatorTreeNode navigatorTreeNode1 = (NavigatorTreeNode) null;
    if (parentNode?.Handler != null && parentNode.Children != null)
    {
      foreach (NavigatorTreeNode child in (List<NavigatorTreeNode>) parentNode.Children)
      {
        NavigatorTreeNode navigatorTreeNode2 = child;
        IDBObjectID objID;
        if (child != null && (objID = parentNode.Handler.GetData(navigatorTreeNode2.NodeID, typeof (IDBObjectID)) as IDBObjectID) != null && objID.Value != 0L)
        {
          if (objID.Value == folderID)
          {
            navigatorTreeNode2.Tree.PopulateNode(navigatorTreeNode2);
            navigatorTreeNode2.Tree.FocusedNode = navigatorTreeNode2;
            navigatorTreeNode2.Tree.SetNodeExpanded(navigatorTreeNode2, true);
            navigatorTreeNode1 = navigatorTreeNode2;
          }
          else if (dt.AsEnumerable().FirstOrDefault<DataRow>((System.Func<DataRow, bool>) (x => Convert.ToInt64(x["F_OBJECT_ID"]) == objID.Value)) != null)
          {
            navigatorTreeNode2.Tree.PopulateNode(navigatorTreeNode2);
            navigatorTreeNode2.Tree.FocusedNode = navigatorTreeNode2;
            navigatorTreeNode2.Tree.SetNodeExpanded(navigatorTreeNode2, true);
            navigatorTreeNode1 = this.SearchFolderNode(navigatorTreeNode2, folderID, dt);
          }
          else
            continue;
          if (navigatorTreeNode1 != null)
            break;
        }
      }
    }
    return navigatorTreeNode1;
  }

  private string SelectCoatingOrGlueDesignation(bool isCoating)
  {
    Intermech.Navigator.SelectionWindow.RegisterAnalyze((ISelectedItemsAnalyzer) new SelectedCoatingAndGlueAnalizer(), true);
    SelectionOptions options = SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableMultiselect;
    IDescriptor rootDescriptor;
    string caption;
    if (isCoating)
    {
      rootDescriptor = (IDescriptor) new VirtualNodeDescriptor(Consts.IMHMaterialsHandbookNodeCategoryID, Consts.IMHCoatingsHandbookNodeCategoryID, LocalizationHolder.rm.GetString("IMH_CoatingsHandbookNode_Caption"));
      caption = LocalizationHolder.rm.GetString("IMH_SelectaCoating");
    }
    else
    {
      rootDescriptor = (IDescriptor) new VirtualNodeDescriptor(Consts.IMHMaterialsHandbookNodeCategoryID, Consts.IMHGluesHandbookNodeCategoryID, LocalizationHolder.rm.GetString("IMH_GluesHandbookNode_Caption"));
      caption = LocalizationHolder.rm.GetString("IMH_SelectaGlue");
    }
    long[] numArray = Intermech.Navigator.SelectionWindow.SelectObjects(caption, "", rootDescriptor, options);
    return numArray == null || numArray.Length == 0 || numArray[0] <= -1L ? string.Empty : this._designation;
  }
}
