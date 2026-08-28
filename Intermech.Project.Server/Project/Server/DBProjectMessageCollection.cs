// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.DBProjectMessageCollection
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Kernel;
using Intermech.Metadata;
using System;

#nullable disable
namespace Intermech.Project.Server;

[DBObjectTypeHandler("cadd91f6-306c-11d8-b4e9-00304f19f545", true)]
public class DBProjectMessageCollection([NotNull] UserSession uSession, [NotEmpty] int objectType) : 
  DBObjectCollection(uSession, objectType),
  IDBProjectMessageCollection,
  IDBObjectCollection,
  IDBRecords,
  IDBSessionable,
  IDBAttributableCollection
{
  public static Guid TypeGuid => Intermech.Project.ObjectTypes.ProjectMessage.Guid;

  [NotEmpty]
  public static int TypeID => (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.ProjectMessage;

  [NotNull]
  internal DBProjectMessage CreateMessage(
    [NotEmpty] long id,
    [NotEmpty] int objectType,
    [CanBeNull] IDBObject prototype,
    Guid versionGuid)
  {
    IDBObject dbObject = this.CreateObject(id, objectType, prototype, versionGuid);
    Intermech.Diagnostics.Check.Result.NotNull<IDBObject>(dbObject);
    return dbObject.CastToClass<DBProjectMessage>();
  }

  [NotNull]
  protected override IDBObject CreateObject(
    [NotEmpty] long id,
    [NotEmpty] int objectType,
    [CanBeNull] IDBObject prototype,
    Guid versionGuid)
  {
    IDBObject dbObject = base.CreateObject(id, objectType, prototype, versionGuid);
    Intermech.Diagnostics.Check.Result.NotNull<IDBObject>(dbObject);
    return (IDBObject) dbObject.CastToClass<DBProjectMessage>();
  }

  IDBProjectMessage IDBProjectMessageCollection.Create()
  {
    return ObjectExtensions.CastToInterface<IDBProjectMessage>(this.Create(DBProjectMessageCollection.TypeID));
  }

  IDBProjectMessage IDBProjectMessageCollection.Create([NotNull] IDBProjectMessage prototype)
  {
    return ObjectExtensions.CastToInterface<IDBProjectMessage>(this.Create((IDBObject) prototype));
  }

  IDBProjectMessage IDBProjectMessageCollection.Create([NotEmpty] long prototypeID)
  {
    return ObjectExtensions.CastToInterface<IDBProjectMessage>(this.Create(prototypeID));
  }
}
