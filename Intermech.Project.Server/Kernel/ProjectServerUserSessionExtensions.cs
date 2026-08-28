// Decompiled with JetBrains decompiler
// Type: Intermech.Kernel.ProjectServerUserSessionExtensions
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Metadata;
using Intermech.Project;
using Intermech.Project.Server;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Kernel;

public static class ProjectServerUserSessionExtensions
{
  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectCollection GetServerProjectCollection([NotNull] this IUserSession userSession)
  {
    return userSession.GetServerObjectsCollection<DBProjectCollection>((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project);
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectTaskCollection GetServerTaskCollection([NotNull] this IUserSession userSession)
  {
    return userSession.GetServerObjectsCollection<DBProjectTaskCollection>((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task);
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectTaskCollection GetServerTaskCollection(
    [NotNull] this IUserSession userSession,
    [NotEmpty] int taskSubTypeID)
  {
    if (taskSubTypeID == (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task)
      return userSession.GetServerObjectsCollection<DBProjectTaskCollection>((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task);
    Intermech.Project.Helper.CheckTypeIsTask(taskSubTypeID);
    return userSession.GetServerObjectsCollection<DBProjectTaskCollection>(taskSubTypeID);
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectMessageCollection GetServerProjectMessageCollection(
    [NotNull] this IUserSession userSession)
  {
    return userSession.GetServerObjectsCollection<DBProjectMessageCollection>((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.ProjectMessage);
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBDependencyCollection GetServerDependencyCollection([NotNull] this IUserSession userSession)
  {
    return userSession.GetServerObjectsCollection<DBDependencyCollection>((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency);
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBDependencyCollection GetServerDependencyCollection(
    [NotNull] this IUserSession userSession,
    [NotEmpty] int dependenceSubTypeID)
  {
    if (dependenceSubTypeID == (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency)
      return userSession.GetServerObjectsCollection<DBDependencyCollection>((int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task);
    Intermech.Project.Helper.CheckTypeIsDependence(dependenceSubTypeID);
    return userSession.GetServerObjectsCollection<DBDependencyCollection>(dependenceSubTypeID);
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBRelationCollectionTaskInProject GetServerRelationCollectionTaskInProject(
    [NotNull] this IUserSession userSession,
    [CanBeNull, CanBeEmpty] string filtrationOwnerID = null)
  {
    return userSession.GetRelationCollection<DBRelationCollectionTaskInProject>((int) (IpsMetadataEntityBase<int>) Intermech.Project.RelationTypes.TaskComposition, filtrationOwnerID);
  }

  [ContractAnnotation("failIfNotFound:false => CanBeNull; => NotNull")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProject GetServerProject(
    [NotNull] this IUserSession userSession,
    [NotEmpty] long projectVersionID,
    bool failIfNotFound = true)
  {
    return userSession.GetServerObject<DBProject, ProjectNotFoundException>(projectVersionID, failIfNotFound);
  }

  [ContractAnnotation("failIfNotFound:false => CanBeNull; => NotNull")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProject GetServerProject(
    [NotNull] this IUserSession userSession,
    [NotEmpty] Guid projectVersionGuid,
    bool failIfNotFound = true)
  {
    return userSession.GetServerObject<DBProject, ProjectNotFoundException>(projectVersionGuid, failIfNotFound);
  }

  [ContractAnnotation("=> true, result: notnull; => false, result: null")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetServerProject(
    [NotNull] this IUserSession userSession,
    [NotEmpty] long projectVersionID,
    out DBProject result)
  {
    return userSession.TryGetServerObject<DBProject>(projectVersionID, out result);
  }

  [ContractAnnotation("=> true, result: notnull; => false, result: null")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetServerProject(
    [NotNull] this IUserSession userSession,
    [NotEmpty] Guid projectVersionGuid,
    out DBProject result)
  {
    return userSession.TryGetServerObject<DBProject>(projectVersionGuid, out result);
  }

  [ContractAnnotation("failIfNotFound:false => CanBeNull; => NotNull")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectTask GetServerTask(
    [NotNull] this IUserSession userSession,
    [NotEmpty] long taskVersionID,
    bool failIfNotFound = true)
  {
    return userSession.GetServerObject<DBProjectTask, TaskNotFoundException>(taskVersionID, failIfNotFound);
  }

  [ContractAnnotation("failIfNotFound:false => CanBeNull; => NotNull")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectTask GetServerTask(
    [NotNull] this IUserSession userSession,
    [NotEmpty] Guid taskVersionGuid,
    bool failIfNotFound = true)
  {
    return userSession.GetServerObject<DBProjectTask, TaskNotFoundException>(taskVersionGuid, failIfNotFound);
  }

  [ContractAnnotation("=> true, result: notnull; => false, result: null")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetServerTask(
    [NotNull] this IUserSession userSession,
    [NotEmpty] long taskVersionID,
    out DBProjectTask result)
  {
    return userSession.TryGetServerObject<DBProjectTask>(taskVersionID, out result);
  }

  [ContractAnnotation("=> true, result: notnull; => false, result: null")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetServerTask(
    [NotNull] this IUserSession userSession,
    [NotEmpty] Guid taskVersionGuid,
    out DBProjectTask result)
  {
    return userSession.TryGetServerObject<DBProjectTask>(taskVersionGuid, out result);
  }

  [ContractAnnotation("failIfNotFound:false => CanBeNull; => NotNull")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectMessage GetServerProjectMessage(
    [NotNull] this IUserSession userSession,
    [NotEmpty] long projectMessageVersionID,
    bool failIfNotFound = true)
  {
    return userSession.GetServerObject<DBProjectMessage, ProjectMessageNotFoundException>(projectMessageVersionID, failIfNotFound);
  }

  [ContractAnnotation("failIfNotFound:false => CanBeNull; => NotNull")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBProjectMessage GetServerProjectMessage(
    [NotNull] this IUserSession userSession,
    [NotEmpty] Guid projectMessageVersionGuid,
    bool failIfNotFound = true)
  {
    return userSession.GetServerObject<DBProjectMessage, TaskNotFoundException>(projectMessageVersionGuid, failIfNotFound);
  }

  [ContractAnnotation("=> true, result: notnull; => false, result: null")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetServerProjectMessage(
    [NotNull] this IUserSession userSession,
    [NotEmpty] long projectMessageVersionID,
    out DBProjectMessage result)
  {
    return userSession.TryGetServerObject<DBProjectMessage>(projectMessageVersionID, out result);
  }

  [ContractAnnotation("=> true, result: notnull; => false, result: null")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetServerProjectMessage(
    [NotNull] this IUserSession userSession,
    [NotEmpty] Guid projectMessageVersionGuid,
    out DBProjectMessage result)
  {
    return userSession.TryGetServerObject<DBProjectMessage>(projectMessageVersionGuid, out result);
  }

  [ContractAnnotation("failIfNotFound:false => CanBeNull; => NotNull")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBDependency GetServerDependency(
    [NotNull] this IUserSession userSession,
    [NotEmpty] long dependencyVersionID,
    bool failIfNotFound = true)
  {
    return userSession.GetServerObject<DBDependency, DependencyNotFoundException>(dependencyVersionID, failIfNotFound);
  }

  [ContractAnnotation("failIfNotFound:false => CanBeNull; => NotNull")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBDependency GetServerDependency(
    [NotNull] this IUserSession userSession,
    [NotEmpty] Guid dependencyVersionGuid,
    bool failIfNotFound = true)
  {
    return userSession.GetServerObject<DBDependency, TaskNotFoundException>(dependencyVersionGuid, failIfNotFound);
  }

  [ContractAnnotation("=> true, result: notnull; => false, result: null")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetServerDependency(
    [NotNull] this IUserSession userSession,
    [NotEmpty] long dependencyVersionID,
    out DBDependency result)
  {
    return userSession.TryGetServerObject<DBDependency>(dependencyVersionID, out result);
  }

  [ContractAnnotation("=> true, result: notnull; => false, result: null")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetServerDependency(
    [NotNull] this IUserSession userSession,
    [NotEmpty] Guid dependencyVersionGuid,
    out DBDependency result)
  {
    return userSession.TryGetServerObject<DBDependency>(dependencyVersionGuid, out result);
  }

  [ContractAnnotation("failIfNotFound:false => CanBeNull; => NotNull")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBRelationTaskInProject GetServerTaskInProjectRelation(
    [NotNull] this IUserSession userSession,
    [NotEmpty] long relationID,
    bool failIfNotFound = true)
  {
    return userSession.GetServerRelation<DBRelationTaskInProject, RelationTaskInProjectNotFoundException>(relationID, failIfNotFound);
  }

  [ContractAnnotation("failIfNotFound:false => CanBeNull; => NotNull")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static DBRelationTaskInProject GetServerTaskInProjectRelation(
    [NotNull] this IUserSession userSession,
    [NotEmpty] Guid relationGuid,
    bool failIfNotFound = true)
  {
    return userSession.GetServerRelation<DBRelationTaskInProject, RelationTaskInProjectNotFoundException>(relationGuid, failIfNotFound);
  }

  [ContractAnnotation("=> true, result: notnull; => false, result: null")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetServerTaskInProjectRelation(
    [NotNull] this IUserSession userSession,
    [NotEmpty] long relationID,
    out DBRelationTaskInProject result)
  {
    return userSession.TryGetServerRelation<DBRelationTaskInProject>(relationID, out result);
  }

  [ContractAnnotation("=> true, result: notnull; => false, result: null")]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static bool TryGetServerTaskInProjectRelation(
    [NotNull] this IUserSession userSession,
    [NotEmpty] Guid relationGuid,
    out DBRelationTaskInProject result)
  {
    return userSession.TryGetServerRelation<DBRelationTaskInProject>(relationGuid, out result);
  }
}
