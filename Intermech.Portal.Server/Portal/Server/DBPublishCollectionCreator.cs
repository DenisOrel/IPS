// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.DBPublishCollectionCreator
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using Intermech.Portal.Server.Classes;
using System;

#nullable disable
namespace Intermech.Portal.Server;

internal class DBPublishCollectionCreator : IDBObjectCollectionCreator
{
  public IDBObjectCollection CreateObjectCollection(
    IUserSession uSession,
    Guid guid,
    int objectTypeID)
  {
    return (IDBObjectCollection) new DBPublishCollection(uSession as UserSession, objectTypeID);
  }
}
