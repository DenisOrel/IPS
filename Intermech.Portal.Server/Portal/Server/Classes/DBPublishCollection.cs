// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.Classes.DBPublishCollection
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Kernel;

#nullable disable
namespace Intermech.Portal.Server.Classes;

internal class DBPublishCollection(UserSession uSession, int objectType) : DBObjectCollection(uSession, objectType)
{
  protected override void CreateObject_CopyAttributes(
    IDBObject prototype,
    IDBObject newobject,
    long id)
  {
  }

  protected override IDBRelation CreateObject_CopyVersionRelations(
    IDBObject newObject,
    IDBObject prototype,
    DBRelationCollection rels,
    NewRelationProperties props)
  {
    return (IDBRelation) null;
  }
}
