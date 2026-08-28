// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.DBProjectCollection
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Metadata;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Project.Server;

[DBObjectTypeHandler("cad00e91-306c-11d8-b4e9-00304f19f545", true)]
public class DBProjectCollection([NotNull] UserSession uSession, [NotEmpty] int objectType) : 
  DBProjectTaskCollection(uSession, objectType),
  IDBProjectCollection,
  IDBObjectCollection,
  IDBRecords,
  IDBSessionable,
  IDBAttributableCollection
{
  public new static Guid TypeGuid => Intermech.Project.ObjectTypes.Project.Guid;

  public new static int TypeID => (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project;

  [NotNull]
  internal DBProject CreateProject([NotEmpty] long id, [NotEmpty] int objectType, [CanBeNull] IDBObject prototype, Guid versionGuid)
  {
    IDBObject dbObject = this.CreateObject(id, objectType, prototype, versionGuid);
    Intermech.Diagnostics.Check.Result.NotNull<IDBObject>(dbObject);
    return dbObject.CastToClass<DBProject>();
  }

  [NotNull]
  protected override IDBObject CreateObject(
    [NotEmpty] long id,
    [NotEmpty] int objectType,
    [CanBeNull] IDBObject prototype,
    Guid versionGuid)
  {
    IDBObject dbObject1 = base.CreateObject(id, objectType, prototype, versionGuid);
    Intermech.Diagnostics.Check.Result.NotNull<IDBObject>(dbObject1);
    DBProject dbProject = dbObject1.CastToClass<DBProject>();
    long objectId1 = dbProject.ObjectID;
    long id1 = dbProject.ID;
    if (prototype == null)
      return (IDBObject) dbProject;
    long id2 = prototype.ID;
    long objectId2 = prototype.ObjectID;
    DBRelationCollectionTaskInProject collectionTaskInProject = this.Session.GetServerRelationCollectionTaskInProject();
    DataTable projectTasks = collectionTaskInProject.GetProjectTasks(objectId2, (IReadOnlyCollection<ColumnDescriptor>) DB.Columns(DB.ObjectAttr.ID, DB.ObjectAttr.VersionID, DB.ObjectAttr.TypeID, DB.RelationAttr.PrjLinkID, DB.RelationAttr.ProjID), (IReadOnlyCollection<ConditionStructure>) null, false);
    if (projectTasks == null || projectTasks.Rows.Count == 0)
      return (IDBObject) dbProject;
    DBProjectTaskCollection serverTaskCollection = this.Session.GetServerTaskCollection();
    DBProjectCollection projectCollection = (DBProjectCollection) null;
    Dictionary<long, (long, long)> dictionary1 = new Dictionary<long, (long, long)>(projectTasks.Rows.Count + 1);
    dictionary1.Add(Math.Abs(objectId2), (objectId1, id1));
    Dictionary<long, (long, long)> dictionary2 = new Dictionary<long, (long, long)>(projectTasks.Rows.Count + 1);
    dictionary2.Add(Math.Abs(id2), (objectId1, id1));
    DBDependencyCollection dependencyCollection1 = this.Session.GetServerDependencyCollection();
    DataTable dataTable = dependencyCollection1.Select(DB.Condition((ColumnDescriptor) Intermech.Project.Attributes.Project, DB.EqualTo((object) objectId2)), DB.Columns(DB.ObjectAttr.VersionID, DB.ObjectAttr.TypeID, DB.ObjectAttribute((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.FromTask), DB.ObjectAttribute((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ToTask)));
    int count1 = projectTasks.Rows.Count;
    int? count2 = dataTable?.Rows.Count;
    List<IDBObject> dbObjectList = new List<IDBObject>((count2.HasValue ? new int?(count1 + count2.GetValueOrDefault()) : new int?()) ?? 0);
    Dictionary<int, DBProjectTaskCollection> dictionary3 = (Dictionary<int, DBProjectTaskCollection>) null;
    int num1 = (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task;
    DBProjectTaskCollection projectTaskCollection = serverTaskCollection;
    foreach (DataRow row in (InternalDataCollectionBase) projectTasks.Rows)
    {
      int num2 = row.FieldAsObjectTypeID(2);
      long num3;
      long num4;
      long num5;
      long taskVersionID;
      if (Intermech.Project.Helper.IsProject(num2))
      {
        num4 = num3 = row.FieldAsObjectID(0);
        taskVersionID = num5 = row.FieldAsObjectID(1);
      }
      else
      {
        num4 = row.FieldAsObjectID(0);
        taskVersionID = row.FieldAsObjectID(1);
        DBProjectTask serverTask = this.Session.GetServerTask(taskVersionID);
        if (num2 != num1)
        {
          num1 = num2;
          if (num2 == (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task)
            projectTaskCollection = serverTaskCollection;
          else if (num2 == (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project)
          {
            if (projectCollection == null)
              projectCollection = this.Session.GetServerProjectCollection();
            projectTaskCollection = (DBProjectTaskCollection) projectCollection;
          }
          else
          {
            if (dictionary3 == null)
              dictionary3 = new Dictionary<int, DBProjectTaskCollection>(8);
            projectTaskCollection = dictionary3.GetOrAdd<int, DBProjectTaskCollection>(num2, (System.Func<int, DBProjectTaskCollection>) (objTypeID => this.Session.GetServerTaskCollection(objTypeID)));
          }
        }
        DBProjectTask taskCopy = projectTaskCollection.CreateTaskCopy((IDBObject) serverTask);
        dbObjectList.Add((IDBObject) taskCopy);
        num3 = taskCopy.ID;
        num5 = taskCopy.ObjectID;
      }
      if (taskVersionID != num5)
        dictionary1.Add(Math.Abs(taskVersionID), (num5, num3));
      if (num4 != num3)
        dictionary2.Add(Math.Abs(num4), (num5, num3));
    }
    foreach (DataRow row in (InternalDataCollectionBase) projectTasks.Rows)
    {
      DBRelationTaskInProject inProjectRelation = this.Session.GetServerTaskInProjectRelation(row.FieldAsObjectID(3));
      long num6 = row.FieldAsObjectID(4);
      (long, long) valueTuple1;
      long projectObjectID = dictionary1.TryGetValue(Math.Abs(num6), out valueTuple1) ? valueTuple1.Item1 : num6;
      long num7 = row.FieldAsObjectID(0);
      (long, long) valueTuple2;
      if (!dictionary2.TryGetValue(Math.Abs(num7), out valueTuple2))
        valueTuple2 = (num7, row.FieldAsObjectID(0));
      long relationId = collectionTaskInProject.CreateRelationCopy((IDBRelation) inProjectRelation, projectObjectID, valueTuple2.Item2, valueTuple2.Item1).CastClassToClass<DBRelationTaskInProject>().RelationID;
    }
    if (dataTable != null && dataTable.Rows.Count > 0)
    {
      Dictionary<int, DBDependencyCollection> dictionary4 = (Dictionary<int, DBDependencyCollection>) null;
      int num8 = (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency;
      DBDependencyCollection dependencyCollection2 = dependencyCollection1;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        int dependenceTypeID = row.FieldAsObjectTypeID(1);
        long dependencyVersionID = row.FieldAsObjectID(0);
        long num9 = row.FieldAsObjectID(2);
        long num10 = row.FieldAsObjectID(3);
        (long, long) valueTuple;
        long newValue1 = dictionary1.TryGetValue(Math.Abs(num9), out valueTuple) ? valueTuple.Item1 : num9;
        long newValue2 = dictionary1.TryGetValue(Math.Abs(num10), out valueTuple) ? valueTuple.Item1 : num10;
        DBDependency serverDependency = this.Session.GetServerDependency(dependencyVersionID);
        if (dependenceTypeID != num8)
        {
          num8 = dependenceTypeID;
          if (dependenceTypeID == (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency)
          {
            dependencyCollection2 = dependencyCollection1;
          }
          else
          {
            if (dictionary4 == null)
              dictionary4 = new Dictionary<int, DBDependencyCollection>(8);
            dependencyCollection2 = dictionary4.GetOrAdd<int, DBDependencyCollection>(dependenceTypeID, (System.Func<int, DBDependencyCollection>) (objTypeID => this.Session.GetServerDependencyCollection(dependenceTypeID)));
          }
        }
        DBDependency dependencyCopy = dependencyCollection2.CreateDependencyCopy((IDBObject) serverDependency);
        dbObjectList.Add((IDBObject) dependencyCopy);
        dependencyCopy.SetAttrObjLinkValue((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.Project, objectId1);
        if (newValue1 != num9)
          dependencyCopy.SetAttrObjLinkValue((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.FromTask, newValue1);
        if (newValue2 != num10)
          dependencyCopy.SetAttrObjLinkValue((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.ToTask, newValue2);
      }
    }
    foreach (IDBObject dbObject2 in dbObjectList)
      dbObject2.CommitCreation(true, true);
    return (IDBObject) dbProject;
  }

  IProject IDBProjectCollection.Create()
  {
    return ObjectExtensions.CastToInterface<IProject>(this.Create((long) DBProjectCollection.TypeID));
  }

  IProject IDBProjectCollection.Create([NotNull] IProject prototype)
  {
    return ObjectExtensions.CastToInterface<IProject>(this.Create((IDBProjectTask) prototype));
  }

  IProject IDBProjectCollection.Create([NotEmpty] long prototypeID)
  {
    return ObjectExtensions.CastToInterface<IProject>(this.Create(prototypeID));
  }
}
