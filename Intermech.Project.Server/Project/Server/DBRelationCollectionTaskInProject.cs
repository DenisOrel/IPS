// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.DBRelationCollectionTaskInProject
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Localization;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Project.Server;

[DBRelationTypeHandler("cad00e93-306c-11d8-b4e9-00304f19f545")]
public class DBRelationCollectionTaskInProject : 
  DBRelationCollection,
  IDBRelationCollectionTaskInProject,
  IDBRelationCollection,
  IDBRecords,
  IDBSessionable,
  IDBAttributableCollection,
  IServerDBRelationCollection,
  IDBJoinField,
  IDBLocalizable,
  IDBLastAccessInfo
{
  public static Guid TypeGuid => Intermech.Project.RelationTypes.TaskComposition.Guid;

  [NotEmpty]
  public static int TypeID => (int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.TaskComposition;

  public DBRelationCollectionTaskInProject([NotNull] UserSession userSession, [NotEmpty] int relationTypeID)
    : base(userSession, relationTypeID)
  {
  }

  public DBRelationCollectionTaskInProject(
    [NotNull] UserSession userSession,
    [NotEmpty] int relationTypeID,
    [CanBeNull, CanBeEmpty] string filtrationOwnerID)
    : base(userSession, relationTypeID, filtrationOwnerID)
  {
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DBRelationTaskInProject Create([NotEmpty] long projectID, [NotEmpty] long taskID, [CanBeEmpty] DateTime beginDate)
  {
    return base.Create(projectID, taskID, beginDate).CastInterfaceToClass<IDBRelation, DBRelationTaskInProject>();
  }

  IDBRelationTaskInProject IDBRelationCollectionTaskInProject.Create(
    [NotEmpty] long projectID,
    [NotEmpty] long partObjectID,
    [CanBeEmpty] DateTime beginDate)
  {
    return base.Create(projectID, partObjectID, beginDate).CastInterfaceToOtherInterface<IDBRelation, IDBRelationTaskInProject>();
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DBRelationTaskInProject Create([NotEmpty] long projectID, [NotEmpty] long taskID, [CanBeNull] AttributeValues[] vals = null)
  {
    return base.Create(projectID, taskID, vals).CastInterfaceToClass<IDBRelation, DBRelationTaskInProject>();
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  IDBRelationTaskInProject IDBRelationCollectionTaskInProject.Create(
    [NotEmpty] long projectID,
    [NotEmpty] long taskID,
    [CanBeNull] AttributeValues[] vals)
  {
    return base.Create(projectID, taskID, vals).CastInterfaceToOtherInterface<IDBRelation, IDBRelationTaskInProject>();
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DBRelationTaskInProject Create([NotEmpty] in NewRelationProperties properties)
  {
    return this.Create(properties).CastInterfaceToClass<IDBRelation, DBRelationTaskInProject>();
  }

  IDBRelationTaskInProject IDBRelationCollectionTaskInProject.Create(
    [NotEmpty] in NewRelationProperties properties)
  {
    return this.Create(properties).CastInterfaceToOtherInterface<IDBRelation, IDBRelationTaskInProject>();
  }

  [CanBeNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DataTable GetProjectTasks(
    [NotEmpty] long projectVersionID,
    [NotNull] IReadOnlyCollection<ColumnDescriptor> columns,
    [CanBeNull] IReadOnlyCollection<ConditionStructure> conditions = null,
    bool recursiveSubProjects = false)
  {
    return this.Session.GetObjectComposition(projectVersionID, (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project, columns, searchRelationTypes: (IReadOnlyCollection<int>) new int[1]
    {
      (int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.TaskComposition
    }, searchObjectTypes: (IReadOnlyCollection<int>) Intermech.Project.Helper.TasksTypeIDsArray, expandObjectTypes: (IReadOnlyCollection<int>) (recursiveSubProjects ? Intermech.Project.Helper.TasksTypeIDsArray : Intermech.Project.Helper.TasksNotProjectTypeIDsArray));
  }
}
