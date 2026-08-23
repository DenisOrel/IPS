// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignDBRelationCollectionCreator
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using System;

#nullable disable
namespace Intermech.Signs.Server;

public class SignDBRelationCollectionCreator : IDBRelationCollectionCreator
{
  public IDBRelationCollection CreateRelationCollection(
    IUserSession uSession,
    Guid guid,
    int relationTypeID)
  {
    return (IDBRelationCollection) new SignDBRelationCollection(uSession as UserSession, relationTypeID);
  }
}
