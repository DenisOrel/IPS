// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.DBProjectTaskCollection
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Kernel;
using Intermech.Metadata;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Project.Server;

[DBObjectTypeHandler("cad00e92-306c-11d8-b4e9-00304f19f545", true)]
public class DBProjectTaskCollection([NotNull] UserSession uSession, [NotEmpty] int objectType) : 
  DBObjectCollection(uSession, objectType),
  IDBProjectTaskCollection,
  IDBObjectCollection,
  IDBRecords,
  IDBSessionable,
  IDBAttributableCollection
{
  public static Guid TypeGuid => Intermech.Project.ObjectTypes.Task.Guid;

  [NotEmpty]
  public static int TypeID => (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task;

  public DBProjectTaskCollection([NotNull] UserSession uSession)
    : this(uSession, DBProjectTaskCollection.TypeID)
  {
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal DBProjectTask CreateTask([CanBeNull] IDBObject prototype, [NotEmpty] Guid versionGuid)
  {
    return this.CreateObject(0L, DBProjectTaskCollection.TypeID, prototype, versionGuid).CastToClass<DBProjectTask>();
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal DBProjectTask CreateTask([NotEmpty] int objectType, [CanBeNull] IDBObject prototype, [NotEmpty] Guid versionGuid)
  {
    return this.CreateObject(0L, objectType, prototype, versionGuid).CastToClass<DBProjectTask>();
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal DBProjectTask CreateTaskVersion(
    [NotEmpty] int taskID,
    [NotEmpty] int objectType,
    [CanBeNull] IDBObject prototype,
    [NotEmpty] Guid versionGuid)
  {
    return this.CreateObject((long) taskID, objectType, prototype, versionGuid).CastToClass<DBProjectTask>();
  }

  [NotNull]
  internal DBProjectTask CreateTaskCopy([NotNull] IDBObject prototype)
  {
    return this.Create(prototype).CastToClass<DBProjectTask>();
  }

  [NotNull]
  protected override IDBObject CreateObject(
    [NotEmpty] long id,
    [NotEmpty] int objectType,
    [CanBeNull] IDBObject prototype,
    [NotEmpty] Guid versionGuid)
  {
    return (IDBObject) base.CreateObject(id, objectType, prototype, versionGuid).CastToClass<DBProjectTask>();
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DBProjectTask Create() => base.Create().CastToClass<DBProjectTask>();

  IDBProjectTask IDBProjectTaskCollection.Create()
  {
    return ObjectExtensions.CastToInterface<IDBProjectTask>(base.Create());
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DBProjectTask Create([NotNull] IDBProjectTask prototype)
  {
    return this.Create((IDBObject) prototype).CastToClass<DBProjectTask>();
  }

  IDBProjectTask IDBProjectTaskCollection.Create([NotNull] IDBProjectTask prototype)
  {
    return ObjectExtensions.CastToInterface<IDBProjectTask>(this.Create((IDBObject) prototype));
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public DBProjectTask Create([NotEmpty] long prototypeID)
  {
    return base.Create(prototypeID).CastToClass<DBProjectTask>();
  }

  IDBProjectTask IDBProjectTaskCollection.Create([NotEmpty] long prototypeID)
  {
    return ObjectExtensions.CastToInterface<IDBProjectTask>(base.Create(prototypeID));
  }
}
