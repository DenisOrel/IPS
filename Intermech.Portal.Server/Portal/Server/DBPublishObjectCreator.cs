// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.DBPublishObjectCreator
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using System;
using System.Data;

#nullable disable
namespace Intermech.Portal.Server;

internal class DBPublishObjectCreator : IDBObjectCreator
{
  public IDBObject CreateObject(IUserSession uSession, Guid guid, DataTable objectParams)
  {
    return (IDBObject) new DBPublishObject((UserSession) uSession, objectParams);
  }
}
