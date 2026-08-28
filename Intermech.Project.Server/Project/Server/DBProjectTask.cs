// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.DBProjectTask
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Metadata;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Project.Server;

[DBObjectTypeHandler("cad00e92-306c-11d8-b4e9-00304f19f545", true)]
public class DBProjectTask([NotNull] UserSession uSession, [NotNull] DataTable objectsTable) : 
  DBObject(uSession, objectsTable),
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
  private long _projectID = -1;
  [CanBeNull]
  private DBObjectTask _task;
  [CanBeNull]
  private DBProjectTask _parentTask;
  private bool _parentTaskLoaded;
  private RuntimeFlags _runtimeFlags;

  public static Guid TypeGuid => Intermech.Project.ObjectTypes.Task.Guid;

  [NotEmpty]
  public new static int TypeID => (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task;

  public DateTime? PlanStartDateTime
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      DateTime result;
      return !this.TryGetAttrDateTimeValue((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.PlanStart, out result) ? new DateTime?() : new DateTime?(result);
    }
  }

  [CanBeNull]
  public MeasuredValue PlanDuration
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      MeasuredValue result;
      return !this.TryGetAttrMeasuredValue((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.PlanDuration, MeasureUnit.Days.Descriptor, out result) ? (MeasuredValue) null : result;
    }
  }

  public DateTime? PlanFinishDateTime
  {
    get
    {
      DateTime? planStartDateTime = this.PlanStartDateTime;
      if (!planStartDateTime.HasValue)
        return new DateTime?();
      MeasuredValue planDuration = this.PlanDuration;
      if (planDuration == null)
        return new DateTime?();
      MeasuredValue measuredValue = MeasureHelper.Instance.ConvertToMeasuredValue(planDuration, MeasureUnit.Days.ID);
      return new DateTime?(planStartDateTime.Value + TimeSpan.FromDays(measuredValue.Value));
    }
  }

  [CanBeNull]
  [NotWhitespace]
  public string ManagerAnswer
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      return this.GetAttrStrValueOrEmpty((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ManagerAnswer);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] set
    {
      if (string.IsNullOrWhiteSpace(value))
        this.DeleteAttribute((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ManagerAnswer);
      else
        this.SetAttrStrValue((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ManagerAnswer, value);
    }
  }

  public DateTime? DueDateID
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      DateTime result;
      return !this.TryGetAttrDateTimeValue((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.PlanFinish, out result) ? new DateTime?() : new DateTime?(result);
    }
  }

  [NotNull]
  [CanBeEmpty]
  public string Description
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      return this.GetAttrStrValueOrEmpty((int) (IpsMetadataEntityBase<int>) Intermech.Metadata.Attributes.Description);
    }
  }

  [NotNull]
  public IDBObjectCollection DBObjectCollection
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)] get
    {
      return this.Session.GetObjectCollection(DBProjectTask.TypeID);
    }
  }

  [NotNull]
  public IReadOnlyList<(long PrjLinkID, long ObjectID, int ObjectTypeID, string Caption, double Units, bool IsChief, long CalendarID)> Assignments
  {
    get
    {
      DataTable dataTable = this.Session.GetRelationCollection((int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.Resources).ConsistOf(this.ObjectID, DB.Columns(DB.RelationAttr.PrjLinkID, DB.ObjectAttr.VersionID, DB.ObjectAttr.TypeID, DB.ObjectAttr.Caption, DB.Attribute((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ResourceUnits), DB.Attribute((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ResourceIsChief), DB.Attribute((int) (IpsMetadataEntityBase<int>) Intermech.Metadata.Attributes.Calendar)));
      return dataTable.Rows.Count == 0 ? (IReadOnlyList<(long, long, int, string, double, bool, long)>) Array.Empty<(long, long, int, string, double, bool, long)>() : (IReadOnlyList<(long, long, int, string, double, bool, long)>) dataTable.Rows.Select<(long, long, int, string, double, bool, long)>((System.Func<DataRow, (long, long, int, string, double, bool, long)>) (row => (row.FieldAsLong(0), row.FieldAsLong(1), row.FieldAsInt(2), row.FieldAsNotNullString(3), row.FieldAsDouble(4), row.FieldAsBool(5), row.FieldAsLong(6)))).AsArray<(long, long, int, string, double, bool, long)>(dataTable.Rows.Count);
    }
  }

  [CanBeEmpty]
  public virtual long ChiefID
  {
    get
    {
      DataTable dataTable = this.Session.GetRelationCollection((int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.Resources).ConsistOf(this.ObjectID, DB.Columns(-2), DB.Condition(DB.RelationAttribute((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ResourceIsChief), DB.EqualTo((object) true)));
      long result;
      return dataTable.Rows.Count == 0 || !dataTable.Rows.Select<long>((System.Func<DataRow, long>) (row => row.FieldAsLongDef(0))).TryGetFirst<long>((System.Func<long, bool>) (objID => objID != 0L), out result) ? 0L : result;
    }
  }

  [NotNull]
  [ItemNotNull]
  public override AttributeValues[] GetAttributesValues(GetAttributeValuesModes modes)
  {
    AttributeValues[] attributesValues = base.GetAttributesValues(modes);
    if (this.IsCreationMode)
      return attributesValues;
    foreach (AttributeValues attributeValues in attributesValues)
    {
      if (!attributeValues.ReadOnly)
        attributeValues.ReadOnly = Intermech.Project.Attributes.IsProtected(attributeValues.AttributeID);
    }
    return attributesValues;
  }

  [NotNull]
  protected DataTable LoadDependencies()
  {
    return this.UserSession.GetObjectCollection((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ToTask, RelationalOperators.Equal, (object) Math.Abs(this.ObjectID), (object) null, LogicalOperators.AND, 0, false, AttributeSourceTypes.Auto, ColumnContents.ID)
    }, new ColumnDescriptor[3]
    {
      new ColumnDescriptor((object) -2),
      new ColumnDescriptor((object) Intermech.Project.Attributes.FromTask.ID, ColumnContents.ID, ColumnNameMapping.Default, SortOrders.NONE, 0),
      new ColumnDescriptor((object) Intermech.Project.Attributes.DependencyType.ID)
    }));
  }

  [NotNull]
  protected DataTable LoadAssignments()
  {
    return this.UserSession.GetRelationCollection((int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.Resources).ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[4]
    {
      (object) -2,
      (object) -20,
      (object) -50,
      (object) Intermech.Project.Attributes.ResourceUnits.ID
    }, 0L, (object) null, -1), this.ObjectID);
  }

  public new long ProjectID
  {
    get
    {
      if (this._projectID == -1L)
      {
        IDBAttribute attributeById = this.GetAttributeByID((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.Project);
        this._projectID = attributeById != null ? attributeById.AsInteger : 0L;
      }
      return this._projectID;
    }
    set
    {
      this.AttributeByID((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.Project).AsInteger = value;
    }
  }

  long IDBProjectTask.ProjectID => this.ProjectID;

  long IDBObject.ProjectID
  {
    get => base.ProjectID;
    set => base.ProjectID = value;
  }

  [CanBeNull]
  public DBProject GetDbProject()
  {
    return this.ProjectID == -1L ? (DBProject) null : this.Session.GetObject(this.ProjectID, false) as DBProject;
  }

  IProject IDBProjectTask.GetDbProject() => (IProject) this.GetDbProject();

  protected override void DoNextLCStep([NotNull] IDBLifecycleStep nextStep)
  {
    int lcStep = nextStep.LCStep;
    if (!this.Task.Milestone && this.LCStep == (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Executing && (lcStep == (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Validating || lcStep == (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Completed))
      this.Task.CheckResults();
    if (lcStep == (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Executing)
    {
      if (this.ParentTask != null)
        this.ParentTask.LCStep = (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Executing;
      string str = string.Empty;
      foreach (KeyValuePair<DBProjectTask, DependencyType> depTask in this.DepTasks)
      {
        DBProjectTask key;
        DependencyType dependencyType1;
        depTask.Deconstruct<DBProjectTask, DependencyType>(out key, out dependencyType1);
        DBProjectTask dbProjectTask = key;
        DependencyType dependencyType2 = dependencyType1;
        bool flag = false;
        switch (dependencyType2)
        {
          case DependencyType.FinishStart:
            flag = dbProjectTask.Status != TaskStatus.Completed;
            break;
          case DependencyType.StartStart:
            flag = dbProjectTask.Status < TaskStatus.Executed;
            break;
        }
        if (flag)
        {
          if (str != string.Empty)
            str += ", ";
          str = $"{str}\"{dbProjectTask.Caption}\"";
        }
      }
      if (str != string.Empty)
        throw new NotificationException($"Выполнение задачи \"{this.Caption}\" не может быть начато, пока не выполнены задачи, от которых она зависит ({str})!");
    }
    else if (lcStep == (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Completed)
    {
      if (!this.Task.Milestone && !this.FlagSet(RuntimeFlags.AutoComplete) && !this.FlagSet(RuntimeFlags.Summary))
      {
        this.Task.ProjectNeeded();
        if (this.Task.Project != null)
        {
          if (this.Task.Project._Properties.RequireTaskVerification)
          {
            if (this.UserSession.UserID != this.Task.ChiefID && this.UserSession.UserID != this.Task.Project.ChiefID)
              throw new NotificationException($"В соответствии с настройками проекта \"{this.Task.Project.Name}\", только руководитель ({this.Task.ChiefName}) может подтвердить выполнение задачи \"{this.Task.Name}\"!");
          }
          else if (!this.Task.Assignments.Any<Assignment>((System.Func<Assignment, bool>) (a => a.Resource != null && a.IsUser && a.Resource.ObjectID == this.UserSession.UserID)))
            throw new NotificationException("Изменение статуса задачи разрешено только её исполнителям!");
        }
      }
      this.Attributes.AddAttribute((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.FactFinish, false, new object[1]
      {
        (object) DateTime.Now
      });
    }
    base.DoNextLCStep(nextStep);
  }

  [NotNull]
  private Dictionary<DBProjectTask, DependencyType> GetDepTasks(int linkAttrID, int depAttrID)
  {
    IDBObjectCollection objectCollection = this.Session.GetObjectCollection((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency);
    ConditionStructure conditionStructure = new ConditionStructure(-2, RelationalOperators.Less, (object) 0, LogicalOperators.AND, 0, false);
    if (this.ObjectID > 0L)
      conditionStructure.RelationalOperator = RelationalOperators.Greater;
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[2]
    {
      new ConditionStructure(linkAttrID, RelationalOperators.Equal, (object) this.ObjectID, LogicalOperators.AND, 0, false),
      conditionStructure
    }, new ColumnDescriptor[2]
    {
      new ColumnDescriptor((object) depAttrID, ColumnContents.ID, ColumnNameMapping.Default, SortOrders.NONE, 0),
      new ColumnDescriptor((object) Intermech.Project.Attributes.DependencyType.ID)
    });
    DataTable dataTable = objectCollection.Select(paramSet);
    Dictionary<DBProjectTask, DependencyType> depTasks = new Dictionary<DBProjectTask, DependencyType>();
    if (dataTable != null)
    {
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        if (this.Session.GetObject(Convert.ToInt64(row[0]), false) is DBProjectTask key)
          depTasks.Add(key, (DependencyType) Convert.ToInt32(row[1]));
      }
    }
    return depTasks;
  }

  [NotNull]
  private Dictionary<DBProjectTask, DependencyType> DepTasks
  {
    get
    {
      return this.GetDepTasks((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ToTask, (int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.FromTask);
    }
  }

  internal virtual void AfterSetLcStepInternal(bool portalEvent = false)
  {
    switch (this.Status)
    {
      case TaskStatus.Executed:
        this.Attributes.AddAttribute((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.FactStart, false, new object[1]
        {
          (object) DateTime.Now
        });
        break;
      case TaskStatus.Completed:
        DBProject dbProject = this.GetDbProject();
        if (dbProject == null)
          break;
        Intermech.Project.Task byObjectId = dbProject.GetProject(new bool?(false)).Tasks.FindByObjectID(this.ObjectID);
        if (byObjectId == null)
          throw new Exception($"Задача \"{this.Caption}\" не входит в состав проекта \"{dbProject.Caption}\", выполнение вне контекста проекта невозможно!");
        List<Intermech.Project.Task> list = byObjectId.RelatedDependencies.Select<Dependency, Intermech.Project.Task>((System.Func<Dependency, Intermech.Project.Task>) (d => d.Task)).ToList<Intermech.Project.Task>(byObjectId.RelatedDependencies.Count);
        if (byObjectId.PropagateResults)
        {
          AttachmentList src = (AttachmentList) null;
          foreach (Intermech.Project.Task task in list)
          {
            if (src == null)
            {
              src = (AttachmentList) byObjectId.Attachments.Filter(PrjAttachKind.Result);
              for (int index = src.Count - 1; index >= 0; --index)
              {
                if (src[index] is PrjAttachment prjAttachment)
                  prjAttachment.Kind = PrjAttachKind.SrcData;
              }
            }
            if (src.Count != 0)
            {
              task.Attachments.AddList(src);
              IDBObject dbObject = task.GetObject();
              try
              {
                task.Attachments.Save(dbObject);
              }
              finally
              {
                task.ReleaseObject();
              }
            }
            else
              break;
          }
        }
        foreach (Intermech.Project.Task task in list)
        {
          if (task.Status == TaskStatus.Waiting && task.DependenciesCompleted)
            task.Execute();
          else if (task.Status == TaskStatus.Sent && task.DependenciesCompleted)
            task.SendCanStartNotification();
        }
        ConditionStructure[] addConds = new ConditionStructure[1]
        {
          new ConditionStructure(-4, RelationalOperators.In, (object) new int[4]
          {
            (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Waiting,
            (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Sent,
            (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Executing,
            (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Validating
          }, LogicalOperators.AND, 0, false)
        };
        if (dbProject.GetChildren((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task, addConds, 1).Rows.Count > 0 || Intermech.Project.Helper.ProjectsIsLocalType && dbProject.GetChildren((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project, addConds, 1).Rows.Count > 0)
          break;
        if (dbProject.LCStep == (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Sent)
          dbProject.LCStep = (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Executing;
        dbProject.LCStep = (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Completed;
        break;
    }
  }

  protected override void AfterSetLCStep()
  {
    base.AfterSetLCStep();
    this.AfterSetLcStepInternal();
  }

  [NotNull]
  public DBObjectTask Task
  {
    get
    {
      DBObjectTask task1 = this._task;
      if (task1 != null)
        return task1;
      DBObjectTask dbObjectTask = new DBObjectTask((IDBProjectTask) this);
      dbObjectTask._SessionProvider = (ISessionProvider) new SessionProvider((IUserSession) this.UserSession);
      DBObjectTask task2 = dbObjectTask;
      this._task = dbObjectTask;
      return task2;
    }
  }

  [CanBeNull]
  public DBProjectTask ParentTask
  {
    get
    {
      if (!this._parentTaskLoaded)
      {
        this._parentTaskLoaded = true;
        IDBRelationCollection relationCollection = this.Session.GetRelationCollection((int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.TaskComposition);
        DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[1]
        {
          (object) -2
        }, (object[]) null, (SortOrders[]) null);
        relationCollection.ChildObjectTypes = (IList<int>) new int[2]
        {
          (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task,
          (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project
        };
        DataTable dataTable = relationCollection.EntersInVersion(paramSet, this.ObjectID);
        if (dataTable.Rows.Count > 0)
          this._parentTask = this.Session.GetObject(Convert.ToInt64(dataTable.Rows[0][0]), false) as DBProjectTask;
      }
      return this._parentTask;
    }
  }

  IDBProjectTask IDBProjectTask.ParentTask => (IDBProjectTask) this.ParentTask;

  public TaskStatus Status => Intermech.Project.Helper.LCStepToTaskStatus(this.LCStep);

  [NotNull]
  public ParcipiantInfo[] Parcipiants
  {
    get
    {
      DataTable dataTable = this.ConsistOf(this.ObjectID, (int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.Resources, DB.Columns(DB.ObjectAttr.ID, DB.ObjectAttr.ObjectID, DB.ObjectAttr.Caption, (ColumnDescriptor) Intermech.Project.Attributes.ResourceIsChief));
      int count = dataTable.Rows.Count;
      if (count == 0)
        return Array.Empty<ParcipiantInfo>();
      ParcipiantInfo[] parcipiants = new ParcipiantInfo[count];
      for (int index = 0; index < count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        parcipiants[index] = new ParcipiantInfo(row.FieldAsObjectID(0), row.FieldAsObjectID(1), row.FieldAsNotNullStringDef(2, string.Empty), row.FieldAsBool(3));
      }
      return parcipiants;
    }
  }

  public void Set(RuntimeFlags flag) => this._runtimeFlags |= flag;

  public void Unset(RuntimeFlags flag) => this._runtimeFlags &= ~flag;

  public bool FlagSet(RuntimeFlags flag) => (this._runtimeFlags & flag) == flag;

  [CanBeNull]
  public DataTable Messages
  {
    get
    {
      return this.Session.GetObjectCollection((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.ProjectMessage).Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(wfConsts.AttrActivityID, RelationalOperators.Equal, (object) this.ObjectID, LogicalOperators.NONE, 0, false)
      }, new ColumnDescriptor[6]
      {
        new ColumnDescriptor((object) ObligatoryObjectAttributes.F_ID),
        new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID),
        new ColumnDescriptor((object) SystemGUIDs.attributeRecipient),
        new ColumnDescriptor((object) SystemGUIDs.attributeStart),
        new ColumnDescriptor((object) SystemGUIDs.attributeFinish),
        new ColumnDescriptor((object) wfConsts.AttrActivityStatusID)
      }));
    }
  }
}
