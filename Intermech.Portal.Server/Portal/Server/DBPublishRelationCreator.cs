// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.DBPublishRelationCreator
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using System;
using System.Data;

#nullable disable
namespace Intermech.Portal.Server;

internal class DBPublishRelationCreator : IDBRelationCreator
{
  public IDBRelation CreateRelation(IUserSession uSession, Guid guid, DataTable relationParams)
  {
    return guid.Equals(PortalConsts.reltypePublish) ? (IDBRelation) new DBPublishRelation((UserSession) uSession, relationParams) : (IDBRelation) null;
  }
}
