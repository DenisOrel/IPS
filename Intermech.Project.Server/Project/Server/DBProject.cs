// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.DBProject
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.Snapshots;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Kernel.Services.PortalServices;
using Intermech.Metadata;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Project.Server;

[DBObjectTypeHandler("cad00e91-306c-11d8-b4e9-00304f19f545", true)]
public class DBProject([NotNull] UserSession uSession, [NotNull] DataTable objectsTable) : 
  DBProjectTask(uSession, objectsTable),
  IProject,
  IDBProjectTask,
  IDBObject,
  IDBAttributable,
  IDBSessionable,
  IPluginsData,
  IDBLifecycleLevel,
  IDBSecurityCollection,
  IDBSecurity,
  IRuntimeFlags
{
  private const bool AutoStartPublishTasks = true;
  protected bool _pseudoCheckedOut;
  [NotNull]
  internal static Dictionary<long, DBProject> SessionPrototypes = new Dictionary<long, DBProject>();
  [CanBeNull]
  private DBProject _prototype;
  [CanBeNull]
  [NotNullAfter("InitPubService")]
  private ICustomPublisherService _pubService;

  public new static Guid TypeGuid => Intermech.Project.ObjectTypes.Project.Guid;

  public new static int TypeID => (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project;

  public void CheckOutChildren()
  {
    Intermech.Project.SiteID siteId = new Intermech.Project.SiteID(this.SiteID);
    if ((int) siteId.Owner != (int) siteId.CurrentSite)
      this.CheckAccess(ActionType.Edit, this.GetDefaultAccess(ActionType.Edit));
    this.PerformChildOperation(new DBProject.ChildOperationDelegate(DBProject.ChildCheckOut));
  }

  [NotNull]
  public override IDBObject DoCheckout()
  {
    if (this.RemoteStatus == RemoteProcessStatus.WaitingForPublish)
      throw new AbortException("Взятие на изменение невозможно до завершения операции передачи прав владения объекта через портал");
    return base.DoCheckout();
  }

  private void DeleteArcCopiesWithoutWorkOnes()
  {
    if (this.ObjectID >= 0L)
      return;
    List<long> list = ((IEnumerable<DataTable>) new DataTable[2]
    {
      this.GetChildren((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task, (ConditionStructure[]) null, -this.ObjectID),
      this.GetChildren((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency, (ConditionStructure[]) null, -this.ObjectID)
    }).SelectMany<DataTable, DataRow>((System.Func<DataTable, IEnumerable<DataRow>>) (tab => tab.Rows.Cast<DataRow>())).Select<DataRow, long>((System.Func<DataRow, long>) (row => Convert.ToInt64(row[0]))).ToList<long>();
    bool autoRollback = this.UserSession.AutoRollback;
    this.UserSession.AutoRollback = false;
    try
    {
      int count;
      int num;
      do
      {
        count = list.Count;
        num = 0;
        int index = 0;
        while (index < list.Count)
        {
          IDBObject dbObject = this.UserSession.GetObject(list[index], false);
          if (dbObject != null)
          {
            Intermech.Project.SiteID siteId = new Intermech.Project.SiteID(dbObject.SiteID);
            if ((int) siteId.Owner != (int) siteId.CurrentSite)
            {
              list.RemoveAt(index);
            }
            else
            {
              try
              {
                dbObject.Delete(0L);
                list.RemoveAt(index);
              }
              catch (KernelExceptionID ex)
              {
                if (ex.ErrorID != 138)
                  throw;
                ++index;
                ++num;
                if (num == count)
                  throw;
              }
            }
          }
        }
      }
      while (num != 0 && num < count);
    }
    finally
    {
      this.UserSession.AutoRollback = autoRollback;
    }
  }

  protected bool PseudoCheckedOut
  {
    [DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      return this._pseudoCheckedOut;
    }
  }

  protected void InternalCheckInChildren(bool pseudoCheckedOut = false)
  {
    if (pseudoCheckedOut)
      this._pseudoCheckedOut = true;
    try
    {
      this.DeleteArcCopiesWithoutWorkOnes();
      this.PerformChildOperation(new DBProject.ChildOperationDelegate(DBProject.ChildCheckIn));
    }
    finally
    {
      if (pseudoCheckedOut)
        this._pseudoCheckedOut = false;
    }
  }

  public void CheckInChildren() => this.InternalCheckInChildren(true);

  protected override void DoAfterCheckInCommited()
  {
    base.DoAfterCheckInCommited();
    this.InternalCheckInChildren();
  }

  protected override void DoBeforeCheckout()
  {
    this.CheckOutChildren();
    base.DoBeforeCheckout();
  }

  protected void DeleteLinkedObjects(bool cancelChanges, long deleteMode)
  {
    if (cancelChanges)
      this.PerformChildOperation(new DBProject.ChildOperationDelegate(DBProject.ChildPurge), (object) deleteMode);
    else
      this.PerformChildOperation(new DBProject.ChildOperationDelegate(DBProject.ChildDelete), (object) deleteMode);
  }

  public override int Delete(long deleteMode)
  {
    if (this.IsCreationMode)
      DBProject.SessionPrototypes.Remove(this.ObjectID);
    DataTable children = this.GetChildren();
    this.UserSession.StartTransaction();
    int num;
    try
    {
      this.GetProject(new bool?()).DeleteNotifications();
      num = base.Delete(deleteMode);
      IDBRelationCollection relationCollection = this.UserSession.GetRelationCollection((int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.TaskComposition);
      DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[1]
      {
        (object) -20
      }, (object[]) null, (SortOrders[]) null);
      foreach (DataRow row in (InternalDataCollectionBase) children.Rows)
      {
        relationCollection.ChildObjectTypes = (IList<int>) new int[2]
        {
          (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task,
          (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project
        };
        foreach (IDBRelation dbRelation in relationCollection.ConsistFrom(paramSet, Convert.ToInt64(row[0])).SelectNotNull<IDBRelation>((System.Func<DataRow, IDBRelation>) (r => this.UserSession.GetRelation(Convert.ToInt64(r[0]), false))))
          dbRelation.Delete((long) Intermech.Consts.PurgeMode);
      }
      foreach (IDBObject dbObject in children.SelectNotNull<IDBObject>((System.Func<DataRow, IDBObject>) (row => this.UserSession.GetObject(Convert.ToInt64(row[0]), false))))
        dbObject.Delete(deleteMode);
      this.UserSession.Commit();
    }
    catch
    {
      this.UserSession.Rollback();
      throw;
    }
    return num;
  }

  protected override void DoPurge(long deleteMode)
  {
    DateTime checkOutDate = this.GetCheckOutDate();
    foreach (DataRow row in (InternalDataCollectionBase) this.GetChildren().Rows)
    {
      long int64 = Convert.ToInt64(row[0]);
      if (int64 < 0L)
      {
        IDBObject dbObject = this.UserSession.GetObject(int64, false);
        if (dbObject != null && dbObject.GetCheckOutDate() > checkOutDate)
        {
          dbObject.Delete(deleteMode);
          this.UserSession.GetObject(-int64, false)?.Delete(deleteMode);
        }
      }
    }
    if (this.CheckoutBy != 0L)
      this.DeleteLinkedObjects(true, deleteMode);
    base.DoPurge(deleteMode);
  }

  [NotNull]
  private DataTable GetChildren(
    int objTypeID,
    [CanBeNull] ConditionStructure[] addConds,
    long project,
    int maxCount = 0)
  {
    IDBObjectCollection objectCollection = this.UserSession.GetObjectCollection(objTypeID);
    ConditionStructure conditionStructure = new ConditionStructure(-2, RelationalOperators.Less, (object) 0, LogicalOperators.AND, 0, false);
    if (project > 0L && !this.PseudoCheckedOut)
      conditionStructure.RelationalOperator = RelationalOperators.Greater;
    ConditionStructure[] array = new ConditionStructure[3]
    {
      new ConditionStructure((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.Project, RelationalOperators.Equal, (object) Math.Abs(project), LogicalOperators.AND, 0, true),
      new ConditionStructure(-7, RelationalOperators.NotEqual, (object) Intermech.Project.ObjectTypes.Project.ID, LogicalOperators.AND, 0, true),
      conditionStructure
    };
    if (addConds != null)
    {
      int length = array.Length;
      Array.Resize<ConditionStructure>(ref array, length + addConds.Length);
      for (int index = 0; index < addConds.Length; ++index)
        array[index + length] = addConds[index];
    }
    object[] columns = new object[1]{ (object) -2 };
    if (maxCount == 0)
      maxCount = -1;
    DBRecordSetParams paramSet = new DBRecordSetParams(array, columns, 0L, (object) null, maxCount);
    if (this.UserSession.IsSystemSession)
    {
      paramSet.Tags = new HybridDictionary();
      paramSet.Tags[(object) "ShowNotOwnedWorkCopies"] = (object) false;
    }
    return objectCollection.Select(paramSet);
  }

  [NotNull]
  internal DataTable GetChildren(int objTypeID, [CanBeNull] ConditionStructure[] addConds, int maxCount = 0)
  {
    return this.GetChildren(objTypeID, addConds, this.ObjectID, maxCount);
  }

  [NotNull]
  internal DataTable GetChildren(int objTypeID)
  {
    return this.GetChildren(objTypeID, (ConditionStructure[]) null);
  }

  [NotNull]
  protected DataTable GetChildren()
  {
    DataTable children = this.GetChildren((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task);
    children.Merge(this.GetChildren((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency));
    return children;
  }

  protected void PerformChildOperation([NotNull] DBProject.ChildOperationDelegate operation, [CanBeNull] object tag)
  {
    this.PerformTableOperation(this.GetChildren(), operation, tag);
  }

  protected void PerformTableOperation(
    [NotNull] DataTable tbl,
    [NotNull] DBProject.ChildOperationDelegate operation,
    [CanBeNull] object tag)
  {
    foreach (IDBObject dbObject in (IEnumerable<IDBObject>) tbl.Select<IDBObject>((System.Func<DataRow, IDBObject>) (row => this.UserSession.GetObject(Convert.ToInt64(row[0]), false))))
      operation(dbObject, tag);
  }

  protected void PerformChildOperation([NotNull] DBProject.ChildOperationDelegate operation)
  {
    this.PerformChildOperation(operation, (object) null);
  }

  private static void ChildCheckOut([NotNull] IDBObject obj, [CanBeNull] object tag)
  {
    Intermech.Project.SiteID siteId = new Intermech.Project.SiteID(obj.SiteID);
    if ((int) siteId.Owner != (int) siteId.CurrentSite)
      return;
    obj.CheckOut();
  }

  private static void ChildCheckIn([NotNull] IDBObject obj, [CanBeNull] object tag)
  {
    obj.CheckIn();
  }

  private static void ChildPurge([NotNull] IDBObject obj, [NotNull] object deleteMode)
  {
    if (obj.CheckoutBy == 0L)
      return;
    obj.CancelChanges((Convert.ToInt64(deleteMode) & 16L /*0x10*/) == 16L /*0x10*/);
  }

  private static void ChildDelete([NotNull] IDBObject obj, [NotNull] object deleteMode)
  {
    obj.Delete((long) deleteMode);
  }

  [CanBeNull]
  internal DBProject Prototype
  {
    get => this._prototype;
    set
    {
      this._prototype = value;
      if (this._prototype == null)
        return;
      DBProject.SessionPrototypes.Add(Math.Abs(this.ObjectID), this._prototype);
    }
  }

  protected override void DoCommitCreation()
  {
    if (this._prototype == null && DBProject.SessionPrototypes.TryGetValue(Math.Abs(this.ObjectID), out this._prototype))
      DBProject.SessionPrototypes.Remove(Math.Abs(this.ObjectID));
    this.RemoteStatus = RemoteProcessStatus.None;
    base.DoCommitCreation();
    if (this._prototype == null)
      return;
    ServerProject project = this._prototype.GetProject(new bool?(false));
    project.LoadProperties((IDBObject) this);
    project.CopyTo(this.ObjectID);
    this._prototype = (DBProject) null;
  }

  [NotNull]
  internal ServerProject GetProject(bool? editMode)
  {
    ServerProject project = new ServerProject(this.ObjectID, true, true);
    project._SessionProvider = (ISessionProvider) new SessionProvider((IUserSession) this.UserSession);
    if (editMode.HasValue)
      project.Load((IDBObject) this, new bool?(editMode.Value));
    return project;
  }

  public override long ChiefID
  {
    get
    {
      long chiefId = base.ChiefID;
      return chiefId == 0L ? this.OwnerID : chiefId;
    }
  }

  private static void ChildSaveChanges([NotNull] IDBObject obj, [CanBeNull] object tag)
  {
    obj.SaveChanges();
  }

  protected override void DoSaveChanges(bool flag)
  {
    base.DoSaveChanges(flag);
    this.PerformChildOperation(new DBProject.ChildOperationDelegate(DBProject.ChildSaveChanges));
  }

  protected override void DoAfterCreateSnapshot(
    [NotNull] IDBSnapshotCollection sender,
    long snapshotID,
    [NotNull] string snapshotName,
    [CanBeNull] string filtrationOwner,
    [NotNull] List<long> lst)
  {
    DataTable dataTable = this.UserSession.GetObjectCollection(Intermech.Project.ObjectTypes.Dependency.Guid).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(Intermech.Project.Attributes.Project.Guid, RelationalOperators.Equal, (object) Math.Abs(this.ObjectID), LogicalOperators.NONE, 0)
    }, new object[1]{ (object) -2 }));
    if (dataTable == null)
      return;
    for (int index = 0; index < dataTable.Rows.Count; ++index)
      sender.AddObjectToSnapshot(Convert.ToInt64(dataTable.Rows[index][0]), snapshotID, snapshotName, filtrationOwner, lst);
  }

  protected override void DoBeforeRestoreSnapshot([NotNull] IDBObjectSnapshot sender)
  {
    DataTable dataTable = this.UserSession.GetObjectCollection(Intermech.Project.ObjectTypes.Dependency.Guid).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(Intermech.Project.Attributes.Project.Guid, RelationalOperators.Equal, (object) Math.Abs(this.ObjectID), LogicalOperators.NONE, 0)
    }, new object[1]{ (object) -2 }));
    if (dataTable == null)
      return;
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      IDBObject dbObject = this.UserSession.GetObject(Convert.ToInt64(dataTable.Rows[index][0]), false);
      if (dbObject != null)
      {
        dbObject.Delete((long) Intermech.Consts.PurgeMode);
        if (Convert.ToInt64(dataTable.Rows[index][0]) < 0L)
          this.UserSession.GetObject(-Convert.ToInt64(dataTable.Rows[index][0]), false)?.Delete((long) Intermech.Consts.PurgeMode);
      }
    }
  }

  protected override void DoAfterRestoreSnapshot([NotNull] IDBObjectSnapshot sender)
  {
    if (this.CheckoutBy != this.UserSession.UserID)
      return;
    DataTable dataTable = this.UserSession.GetObjectCollection(Intermech.Project.ObjectTypes.Dependency.Guid).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(Intermech.Project.Attributes.Project.Guid, RelationalOperators.Equal, (object) Math.Abs(this.ObjectID), LogicalOperators.NONE, 0)
    }, new object[1]{ (object) -2 }));
    if (dataTable == null)
      return;
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      if (Convert.ToInt64(dataTable.Rows[index][0]) > 0L)
        this.UserSession.GetObject(Convert.ToInt64(dataTable.Rows[index][0]), false)?.CheckOut();
    }
  }

  public override void SetSiteID([NotNull] string siteID)
  {
    base.SetSiteID(siteID);
    if (siteID.Length != 3 || this.RemoteStatus != RemoteProcessStatus.WaitingForPublish)
      return;
    IDBAttribute attributeById = this.GetAttributeByID((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ProjectData);
    XmlIni xmlIni = new XmlIni();
    StreamHelper.LoadFromBlobStream(attributeById as IBlobReader, new ProcessStreamDelegate(xmlIni.Load));
    if (!(xmlIni.ReadString("Pending", "SiteID") == siteID))
      return;
    xmlIni.WriteString("Pending", "SiteID", string.Empty);
    StreamHelper.SaveToBlobStream(attributeById as IBlobWriter, new ProcessStreamDelegate(xmlIni.Save), string.Empty);
    this.RemoteStatus = RemoteProcessStatus.Published;
  }

  private void InitPubService()
  {
    this._pubService = this._pubService ?? (this._pubService = ApplicationServices.Container.GetService<ICustomPublisherService>());
  }

  private void LaunchRemoteProcess(
    [NotNull] SystemSessionKeeper sk,
    [NotNull] Intermech.Project.Project p,
    char siteCode,
    bool giveOwnership,
    bool giveCompositionOwnership,
    [NotNull] string command = "")
  {
    this.InitPubService();
    if (!RemoteSettings.Loaded)
      RemoteSettings.LoadSettings(this.Session);
    ISitesCacheService customService = this.UserSession.GetCustomService<ISitesCacheService>();
    SiteInfo site = customService.GetSite(siteCode);
    Intermech.Diagnostics.Check.Assert(site != null, $"Site with code \"{siteCode}\" not found!");
    string caption = site.Caption;
    Guid guid;
    if (!RemoteSettings.SiteSchemes.TryGetValue(siteCode, out guid))
    {
      if (caption == string.Empty)
        caption = siteCode.ToString();
      throw new Exception($"Шаблон публикации проектов для узла '{caption}' не задан!");
    }
    IDBObject dbObject = p.GetObject();
    try
    {
      StringList stringList1 = new StringList();
      stringList1.Values["Launch"] = "1";
      stringList1.Values["RTGuid"] = guid.ToString();
      stringList1.Values["GiveOwnership"] = giveOwnership ? "1" : "0";
      stringList1.Values["GiveCOwnership"] = giveCompositionOwnership ? "1" : "0";
      stringList1.Values["SrcSite"] = customService.Info.GUID.ToString();
      stringList1.Values["SrcSiteName"] = customService.Info.Caption;
      stringList1.Values["Src"] = "IPS";
      stringList1.Values["Kind"] = RemoteProcessKind.ImProject;
      stringList1.Values["ProjectGuid"] = dbObject.ObjectGUID.ToString();
      if (command != string.Empty)
        stringList1.Values["Command"] = command;
      List<long> longList = new List<long>();
      longList.Add(dbObject.ObjectID);
      stringList1.Values["Att.Count"] = "1";
      stringList1.Values["Att.0"] = dbObject.ObjectGUID.ToString();
      string format = "Права владения на проект \"{0}\" были переданы с узла \"{1}\"";
      if (command == "Execute")
        format = "Проект \"{0}\" был передан с узла \"{1}\" для выполнения";
      if (format != string.Empty)
      {
        string s = string.Format(format, (object) p.Name, (object) customService.Info.Caption);
        stringList1.Values["HistArray"] = "0";
        StringList stringList2 = new StringList();
        StringList stringList3 = new StringList();
        stringList3.Add("0");
        stringList3.Add(StringList.StringToCommaText(p.Name));
        stringList3.Add("@null@");
        stringList3.Add(StringList.StringToCommaText(s));
        stringList3.Add(Intermech.Project.ObjectTypes.Project.ToString());
        stringList3.Add(0.ToString());
        stringList3.Add(StringList.StringToCommaText(DateTime.UtcNow.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
        stringList3.Add("0");
        stringList2.Values["0"] = stringList3.CommaText;
        stringList1.Values["Messages"] = stringList2.CommaText;
      }
      string str = $"ImProject {command}: {p.Name} -> {site.Caption}";
      char? owner = new char?();
      if (giveOwnership)
        owner = new char?(site.Code);
      char? compositionOwner = new char?();
      if (giveCompositionOwnership)
        compositionOwner = new char?(site.Code);
      ExtendedPublishOptions options = new ExtendedPublishOptions(PublishCompositionOptions.WithLinkedObjects | PublishCompositionOptions.IncludeFreeChangeAttributes, -1, (List<int>) null, (List<int>) null, (FiltrationSettings) null, customService.Info.Code.ToString() + site.Code.ToString(), false, owner, compositionOwner);
      ProjectPublisher projectPublisher = new ProjectPublisher(new CustomPublishDataInfo(str, site.Code, longList, stringList1.CommaText, options, string.Empty), Publisher.Composition((IUserSession) this.UserSession, longList, options, PublishType.Simple), options);
      long taskID = this._pubService.CustomPublish(sk.Session.SessionGUID, (IPublisher) projectPublisher, str, TaskPriority.Normal);
      ApplicationServices.Container.GetService<IPortalTasksQueue>().StartTask(taskID);
    }
    finally
    {
      p.ReleaseObject();
    }
  }

  private void PublishToPortal(
    [NotNull] SystemSessionKeeper sk,
    [NotNull] Intermech.Project.Project p,
    char toSite,
    bool includeComposition = true)
  {
    this.InitPubService();
    ExtendedPublishOptions options = new ExtendedPublishOptions(PublishCompositionOptions.WithLinkedObjects | PublishCompositionOptions.IncludeFreeChangeAttributes, includeComposition ? -1 : 0, (List<int>) null, (List<int>) null, (FiltrationSettings) null, toSite.ToString(), false, new char?(), new char?());
    string str = $"ImProject sync: {p.Name} -> {toSite.ToString()}";
    IDBObject dbObject = p.GetObject();
    try
    {
      IDBAttribute attributeById = dbObject.GetAttributeByID((int) (IpsMetadataEntityBase<int>) Intermech.Workflow.Attributes.PublicationNecessary);
      if (attributeById != null)
        attributeById.AsInteger = 1L;
    }
    finally
    {
      p.ReleaseObject();
    }
    ProjectPublisher projectPublisher = new ProjectPublisher(new CustomPublishDataInfo(str, toSite, new List<long>(), string.Empty, options, string.Empty), Publisher.Composition((IUserSession) this.UserSession, ListFactory.Create<long>(Math.Abs(p.ObjectID)), options, PublishType.Simple), options);
    long taskID = this._pubService.CustomPublish(sk.Session.SessionGUID, (IPublisher) projectPublisher, str, TaskPriority.Normal);
    ApplicationServices.Container.GetService<IPortalTasksQueue>().StartTask(taskID);
  }

  protected void SyncProject([NotNull] SystemSessionKeeper sk, [NotNull] Intermech.Project.Project p)
  {
    Intermech.Project.SiteID siteId1 = new Intermech.Project.SiteID(p.SiteID);
    IDBObject dbObject = p.GetObject();
    try
    {
      if (p.SyncPending)
      {
        Intermech.Project.SiteID siteId2 = new Intermech.Project.SiteID(p.PendingSiteID);
        try
        {
          char? nullable1 = new char?();
          char? nullable2 = new char?();
          if ((int) siteId1.Owner == (int) siteId1.CurrentSite && (int) siteId2.Owner != (int) siteId1.Owner)
            nullable1 = new char?(siteId2.Owner);
          if ((int) siteId1.CompositionOwner == (int) siteId1.CurrentSite && (int) siteId2.CompositionOwner != (int) siteId1.CompositionOwner)
            nullable2 = new char?(siteId2.CompositionOwner);
          char? nullable3;
          if (nullable1.HasValue)
          {
            nullable3 = nullable1;
            int? nullable4 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            nullable3 = nullable2;
            int? nullable5 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
            if (nullable4.GetValueOrDefault() == nullable5.GetValueOrDefault() & nullable4.HasValue == nullable5.HasValue)
            {
              this.LaunchRemoteProcess(sk, p, nullable1.Value, true, true);
              goto label_14;
            }
          }
          if (nullable1.HasValue)
            this.LaunchRemoteProcess(sk, p, nullable1.Value, true, false);
          if (nullable2.HasValue)
            this.LaunchRemoteProcess(sk, p, nullable2.Value, false, true);
label_14:
          p.RemoteStatus = RemoteProcessStatus.WaitingForPublish;
          nullable3 = nullable1;
          int? nullable6 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
          nullable3 = nullable2;
          int? nullable7 = nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?();
          if (!(nullable6.GetValueOrDefault() == nullable7.GetValueOrDefault() & nullable6.HasValue == nullable7.HasValue))
          {
            dbObject.LCStep = (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Imported;
          }
          else
          {
            if (!nullable1.HasValue)
              return;
            dbObject.LCStep = (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Designing;
          }
        }
        catch
        {
          p.RemoteStatus = RemoteProcessStatus.PublishError;
          throw;
        }
      }
      else
      {
        HashSet<char> charSet = new HashSet<char>();
        charSet.Add(siteId1.Creator);
        charSet.Add(siteId1.Owner);
        charSet.Add(siteId1.CompositionOwner);
        charSet.Remove(siteId1.CurrentSite);
        foreach (char toSite in charSet)
          this.PublishToPortal(sk, p, toSite, (int) siteId1.CompositionOwner == (int) siteId1.CurrentSite || p.RemoteSiteCode == ' ');
      }
    }
    finally
    {
      p.ReleaseObject();
    }
  }

  public void Sync()
  {
    using (SystemSessionKeeper sk = new SystemSessionKeeper("ProjectServer.DBProject.Sync"))
    {
      ServerProject project = this.GetProject(new bool?(false));
      try
      {
        this.SyncProject(sk, (Intermech.Project.Project) project);
        foreach (Intermech.Project.Task task in (System.Collections.ObjectModel.Collection<Intermech.Project.Task>) project.Tasks)
        {
          if (task is Intermech.Project.Project p && !task.IsProjectSummaryTask)
            this.SyncProject(sk, p);
        }
      }
      finally
      {
        this._pubService = (ICustomPublisherService) null;
      }
    }
  }

  public RemoteProcessStatus RemoteStatus
  {
    get
    {
      long remoteStatus = 0;
      IDBAttribute attributeById = this.GetAttributeByID(Intermech.Workflow.Attributes.RemoteProcessStatus.ID);
      if (attributeById != null)
        remoteStatus = attributeById.AsInteger;
      return (RemoteProcessStatus) remoteStatus;
    }
    set
    {
      this.Attributes.AddAttribute((int) (IpsMetadataEntityBase<int>) Intermech.Workflow.Attributes.RemoteProcessStatus, false, new object[1]
      {
        (object) (long) value
      });
    }
  }

  public void Execute()
  {
    if (this.CheckoutBy != 0L)
      throw new Exception("Перед запуском проект должен быть сдан в архив!");
    using (SystemSessionKeeper sk = new SystemSessionKeeper("ProjectServer.DBProject.Execute"))
    {
      ServerProject project = this.GetProject(new bool?(false));
      char remoteSiteCode = project.RemoteSiteCode;
      if (remoteSiteCode != ' ')
      {
        project.Status = Intermech.Project.TaskStatus.Waiting;
        this.LaunchRemoteProcess(sk, (Intermech.Project.Project) project, remoteSiteCode, true, true, nameof (Execute));
      }
      else
      {
        if (!RemoteSettings.Loaded)
          RemoteSettings.LoadSettings(this.Session);
        project.Execute();
      }
    }
  }

  internal override void AfterSetLcStepInternal(bool portalEvent = false)
  {
    base.AfterSetLcStepInternal(portalEvent);
    if (!Portal.Enabled)
      return;
    if (portalEvent)
    {
      if (this.Status != Intermech.Project.TaskStatus.Executed || this.ParentTask == null || this.ParentTask.LCStep != (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Sent)
        return;
      this.ParentTask.LCStep = (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Executing;
    }
    else
    {
      if (this.Status != Intermech.Project.TaskStatus.Completed && this.Status != Intermech.Project.TaskStatus.Terminated)
        return;
      this.UserSession.EventLogHelper.CommitEvent += new TransactionHandler(this.EventLogHelper_CommitEvent);
    }
  }

  private void EventLogHelper_CommitEvent([NotNull] IUserSession session)
  {
    this.UserSession.EventLogHelper.CommitEvent -= new TransactionHandler(this.EventLogHelper_CommitEvent);
    this.Sync();
  }

  [CanBeNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DataTable GetProjectTasks(
    [NotNull] IReadOnlyCollection<ColumnDescriptor> columns,
    [CanBeNull] IReadOnlyCollection<ConditionStructure> conditions = null,
    bool recursiveSubProjects = false)
  {
    return this.Session.GetObjectComposition(this.ObjectID, (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project, columns, searchRelationTypes: (IReadOnlyCollection<int>) new int[1]
    {
      (int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.TaskComposition
    }, searchObjectTypes: (IReadOnlyCollection<int>) Intermech.Project.Helper.TasksTypeIDsArray, expandObjectTypes: (IReadOnlyCollection<int>) (recursiveSubProjects ? Intermech.Project.Helper.TasksTypeIDsArray : Intermech.Project.Helper.TasksNotProjectTypeIDsArray));
  }

  protected delegate void ChildOperationDelegate([NotNull] IDBObject obj, [CanBeNull] object tag);
}
