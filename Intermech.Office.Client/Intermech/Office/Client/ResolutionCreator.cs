// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ResolutionCreator
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Client.Core.FormDesigner.Navigator;
using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Kernel.Search;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal class ResolutionCreator : IObjectCreatorRiderCustomService, IObjectCreatorCustomService
{
  [CanBeNull]
  private IDictionary<ObjectCreatePages, bool> _createPages;
  private const string ConfigSectionName = "ResolutionCreator";
  private const string ConfigPlannedDataProp = "PlannedData";
  [CanBeNull]
  private IList<long> _copyCommittedResolutionWithExecutorIDs;
  private bool _copyCommittedResponseRequires;
  private static long _committedBaseResolution;
  [CanBeNull]
  private static IList<QuickObjectInfo> _committedMultipleResolutionsInfo;

  public bool AcceptDialog(
    int objectType,
    long templateObject,
    int[] relationTypeIDs,
    long[] relatedObjectIDs,
    DateTime startDate,
    bool isVersion)
  {
    return false;
  }

  public bool AfterCreate(long newObjectID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBResolution resolution = sessionKeeper.Session.GetResolution(newObjectID);
      long userId = sessionKeeper.Session.UserID;
      resolution.AuthorID = sessionKeeper.Session.UserID;
      resolution.RegistrationDate = DateTime.Now;
      resolution.SetAttrObjLinkValue(OfficeConsts.AttrControllerID, userId, false);
      DateTime dateTime = DateTime.MinValue;
      DataTable dataTable = sessionKeeper.Session.GetRelationCollection(OfficeConsts.ReltypeOfficeCompositionID).EntersIn(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[1]
      {
        new ColumnDescriptor((object) OfficeConsts.AttrPlannedDateID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0)
      }), resolution.ID);
      if (dataTable.Rows.Count > 0 && dataTable.Rows[0][0] != DBNull.Value)
        dateTime = Convert.ToDateTime(dataTable.Rows[0][0]);
      if (dateTime != DateTime.MinValue)
        resolution.PlannedDate = dateTime;
      IDBObject officeDoc = OfficeHelper.GetOfficeDoc(sessionKeeper.Session, resolution.ObjectType, resolution.ObjectID, resolution.ID);
      IDBResolution dbResolution = resolution;
      string str;
      if (officeDoc == null)
        str = "Поручение без документа";
      else
        str = Localization.GetString("Office.Client_63", (object) officeDoc.Caption);
      dbResolution.Name = str;
      resolution.Attributes.AddAttribute(OfficeConsts.AttrTempCreateMultipleResolutionsID, false, new object[1]
      {
        (object) false
      });
      return true;
    }
  }

  private static void SaveConfigProps([NotNull] IUserSession session, long newObjectID)
  {
    int num1 = OfficeConsts.PlannedDataShift;
    DateTime result;
    if (session.GetResolution(newObjectID).TryGetAttrDateTimeValue(OfficeConsts.AttrPlannedDateID, out result))
      num1 = (result - DateTime.Now).Days;
    if (Holder.ConfigurationManager == null)
      return;
    IConfiguration configuration = Holder.ConfigurationManager.Open(nameof (ResolutionCreator)) ?? Holder.ConfigurationManager.Create(nameof (ResolutionCreator));
    int num2 = OfficeConsts.PlannedDataShift;
    string property = configuration.GetProperty("PlannedData");
    if (property != string.Empty)
      num2 = Convert.ToInt32(property);
    if (num2 == num1)
      return;
    configuration.SetProperty("PlannedData", num1.ToString());
  }

  [NotNull]
  public IDictionary<ObjectCreatePages, bool> VisiblePages
  {
    get
    {
      if (this._createPages == null)
      {
        this._createPages = (IDictionary<ObjectCreatePages, bool>) new Dictionary<ObjectCreatePages, bool>();
        this._createPages.Add(ObjectCreatePages.Properties, true);
        this._createPages.Add(ObjectCreatePages.Template, true);
      }
      return this._createPages;
    }
  }

  public bool OnBeforeCommitAction([NotNull] IUserSession session, [NotNull] IDBObject newObject)
  {
    IDBResolution iDbAttributable = newObject.As<IDBResolution>();
    bool flag = false;
    DataTable dataTable = session.GetRelationCollection(OfficeConsts.ReltypeOfficeCompositionID).EntersIn(new DBRecordSetParams((ConditionStructure[]) null, new ColumnDescriptor[1]
    {
      new ColumnDescriptor((object) OfficeConsts.AttrResponseRequiresID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0)
    }), iDbAttributable.ID);
    if (dataTable.Rows.Count > 0 && dataTable.Rows[0][0] != DBNull.Value)
      flag = Convert.ToBoolean(dataTable.Rows[0][0]);
    IDBAttribute dbAttribute = iDbAttributable.AttributeByID(OfficeConsts.AttrExecutorsID);
    ResolutionCreator._committedBaseResolution = 0L;
    ResolutionCreator._committedMultipleResolutionsInfo = (IList<QuickObjectInfo>) null;
    bool result;
    if (iDbAttributable.TryGetAttrBoolValue(OfficeConsts.AttrTempCreateMultipleResolutionsID, out result))
    {
      if (result)
      {
        long[] executorIds = iDbAttributable.ExecutorIDs;
        if (executorIds.Length > 1)
        {
          iDbAttributable.ExecutorIDs = new long[1]
          {
            executorIds[0]
          };
          this._copyCommittedResolutionWithExecutorIDs = (IList<long>) ((IList<long>) executorIds).FromIndexToRight<long>(1).ToList<long>(executorIds.Length - 1);
          this._copyCommittedResponseRequires = flag;
          iDbAttributable.Attributes.AddAttribute(OfficeConsts.AttrTempDelayedRunID, false, new object[1]
          {
            (object) true
          });
        }
      }
      iDbAttributable.DeleteAttribute(OfficeConsts.AttrTempCreateMultipleResolutionsID);
    }
    if (flag && dbAttribute.ValuesCount == 1)
    {
      if (iDbAttributable.ResponseUserID == 0L)
        iDbAttributable.ResponseUserID = dbAttribute.AsInteger;
      else
        this._copyCommittedResponseRequires = false;
    }
    return true;
  }

  public bool OnCommitAction(
    [NotNull] IUserSession session,
    long newObjectID,
    [NotNull] List<NotificationEventArgs> nea)
  {
    ResolutionCreator.SaveConfigProps(session, newObjectID);
    if (this._copyCommittedResolutionWithExecutorIDs != null && newObjectID != 0L)
    {
      IReadOnlyList<long> source = this._copyCommittedResolutionWithExecutorIDs.CloneStructsList<long>();
      this._copyCommittedResolutionWithExecutorIDs = (IList<long>) null;
      ResolutionCreator._committedMultipleResolutionsInfo = (IList<QuickObjectInfo>) new List<QuickObjectInfo>(source.Count);
      ResolutionCreator._committedBaseResolution = newObjectID;
      if (source.Count > 0)
      {
        IDBObjectCollection iDbObjectCollection = session.GetObjectCollection(session.GetObjectInfo(newObjectID).ObjectTypeID);
        ResolutionContextInfo contextInfo = OfficeHelper.GetResolutionContextInfo(session, newObjectID);
        IDBRelationCollection officeRelations = contextInfo != null ? session.GetRelationCollection(OfficeConsts.ReltypeOfficeCompositionID) : (IDBRelationCollection) null;
        ResolutionCreator._committedMultipleResolutionsInfo.AddRange<QuickObjectInfo>(source.Select<long, QuickObjectInfo?>((System.Func<long, QuickObjectInfo?>) (userID => this.CreateResolutionCopyWithExecutor(session, iDbObjectCollection, newObjectID, userID, contextInfo, officeRelations))).Where<QuickObjectInfo?>((System.Func<QuickObjectInfo?, bool>) (resolutionInfo => resolutionInfo.HasValue)).Select<QuickObjectInfo?, QuickObjectInfo>((System.Func<QuickObjectInfo?, QuickObjectInfo>) (resolutionInfo => resolutionInfo.Value)));
      }
    }
    return true;
  }

  private QuickObjectInfo? CreateResolutionCopyWithExecutor(
    [NotNull] IUserSession session,
    [NotNull] IDBObjectCollection iDbObjectCollection,
    long prototypeObjVersionID,
    long userID,
    [CanBeNull] ResolutionContextInfo contextInfo,
    [CanBeNull] IDBRelationCollection officeRelations)
  {
    long[] ex = iDbObjectCollection.CreateEx(prototypeObjVersionID);
    if (ex.Length == 0)
      return new QuickObjectInfo?();
    IDBResolution resolution = session.GetResolution(ex[0]);
    resolution.Attributes.AddAttribute(OfficeConsts.AttrTempDelayedRunID, false, new object[1]
    {
      (object) true
    });
    resolution.ExecutorIDs = new long[1]{ userID };
    if (this._copyCommittedResponseRequires)
      resolution.ResponseUserID = userID;
    IDBRelation dbRelation = (IDBRelation) null;
    if (contextInfo != null && officeRelations != null)
      dbRelation = officeRelations.Create(contextInfo.ParentObjectVersionID, resolution.ObjectID);
    resolution.CommitCreation(false);
    Holder.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBObjectsEventArgs("ObjectsCreated", resolution.ObjectID, resolution.TypeID));
    if (dbRelation != null)
      Holder.NotificationService.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsCreated", dbRelation.RelationID, contextInfo.ParentObjectVersionID, contextInfo.ParentObjType, OfficeConsts.ReltypeOfficeCompositionID));
    return new QuickObjectInfo?(session.GetObjectInfo(resolution.ObjectID));
  }

  internal static void OnCreatedNotificationFired(
    [NotNull] LazySession sk,
    long newResolutionID,
    int resolutionTypeID)
  {
    if (ResolutionCreator._committedMultipleResolutionsInfo == null || newResolutionID != ResolutionCreator._committedBaseResolution)
      return;
    Intermech.Navigator.SelectionWindow.OnSelectionWindowBeforeShow += new SelectionWindowBeforeShow(ResolutionCreator.CreatedMultipleResolutions_OnSelectionWindowBeforeShow);
    try
    {
      Intermech.Navigator.SelectionWindow.Select("Созданные поручения", "Созданные поручения", (IDescriptor) new ResolutionCopiesForMultipleUsersDescriptor(Enumeration.Create<long>(ResolutionCreator._committedBaseResolution).Concat<long>(ResolutionCreator._committedMultipleResolutionsInfo.Select<QuickObjectInfo, long>((System.Func<QuickObjectInfo, long>) (info => info.ObjectID)))), typeof (IDBObjectID), SelectionOptions.SelectObjects | SelectionOptions.SelectOtherNodes | SelectionOptions.DisableSelectFromViews | SelectionOptions.DisableMultiselect);
    }
    finally
    {
      Intermech.Navigator.SelectionWindow.OnSelectionWindowBeforeShow -= new SelectionWindowBeforeShow(ResolutionCreator.CreatedMultipleResolutions_OnSelectionWindowBeforeShow);
    }
  }

  private static void CreatedMultipleResolutions_OnSelectionWindowBeforeShow(
    [CanBeNull] object sender,
    [NotNull] EventArgs e)
  {
    Intermech.Navigator.Controls.SelectionWindow selectionWindow = sender as Intermech.Navigator.Controls.SelectionWindow;
    Intermech.Diagnostics.Check.NotNull<Intermech.Navigator.Controls.SelectionWindow>(selectionWindow, "selectionWindow");
    NavigatorTreeView navTreeView = selectionWindow.NavTreeView;
    navTreeView.PopulateNodeAndWaitForFull(navTreeView.RootNode);
    bool useDelay = selectionWindow.TreeViewsBridge.UseDelay;
    selectionWindow.TreeViewsBridge.UseDelay = false;
    try
    {
      if (navTreeView.RootNode.Children.Count > 0)
      {
        NavigatorTreeNode child = navTreeView.RootNode.Children[0];
        navTreeView.FocusedNode = child;
        if (OfficeConsts.FormResolutionID != 0L)
          selectionWindow.ViewsManager.ViewPages.FirstOrDefault<IViewPage>((System.Func<IViewPage, bool>) (page => page.Control is FormDesignerView control && control.FormID == OfficeConsts.FormResolutionID)).InvokeIfNotNull<IViewPage>((Action<IViewPage>) (page => selectionWindow.ViewsManager.ActiveViewPage = page));
        else if (selectionWindow.ViewsManager.ViewPages.Count > 0)
          selectionWindow.ViewsManager.ActiveViewPage = selectionWindow.ViewsManager.ViewPages[0];
      }
    }
    finally
    {
      if (selectionWindow.TreeViewsBridge.UseDelay != useDelay)
        selectionWindow.TreeViewsBridge.UseDelay = useDelay;
    }
    selectionWindow.btOK.Visible = false;
    selectionWindow.btCancel.Text = "Закрыть";
    selectionWindow.AcceptButton = (IButtonControl) selectionWindow.btCancel;
    selectionWindow.CancelButton = (IButtonControl) selectionWindow.btCancel;
    selectionWindow.FormClosed += new FormClosedEventHandler(ResolutionCreator.multipleResolutionCopyForUsers_FormClosed);
  }

  private static void multipleResolutionCopyForUsers_FormClosed(
    [CanBeNull] object sender,
    [NotNull] FormClosedEventArgs e)
  {
    if (sender is Intermech.Navigator.Controls.SelectionWindow selectionWindow)
      selectionWindow.FormClosed -= new FormClosedEventHandler(ResolutionCreator.multipleResolutionCopyForUsers_FormClosed);
    if (ResolutionCreator._committedBaseResolution != 0L && ResolutionCreator._committedMultipleResolutionsInfo != null)
    {
      using (SessionKeeper sk = new SessionKeeper())
      {
        foreach (IDBResolution dbResolution in Enumeration.Create<long>(ResolutionCreator._committedBaseResolution).Concat<long>(ResolutionCreator._committedMultipleResolutionsInfo.Select<QuickObjectInfo, long>((System.Func<QuickObjectInfo, long>) (info => info.ObjectID))).SelectNotNull<long, IDBResolution>((System.Func<long, IDBResolution>) (resolutionID => sk.Session.GetResolution(resolutionID, false))))
          dbResolution.Run();
      }
    }
    ResolutionCreator._committedBaseResolution = 0L;
    ResolutionCreator._committedMultipleResolutionsInfo = (IList<QuickObjectInfo>) null;
  }

  public bool OnCancelAction(
    IUserSession session,
    long newObjectID,
    List<NotificationEventArgs> nea)
  {
    return true;
  }

  [CanBeNull]
  public Dictionary<UserControl, int> AddPages(object createdObject, int propPageIndex)
  {
    return (Dictionary<UserControl, int>) null;
  }

  public long CreateObjectDialog(
    int objectType,
    long templateObject,
    int[] relationTypeIDs,
    long[] relatedObjectIDs,
    DateTime startDate,
    bool isVersion)
  {
    return -1;
  }
}
