// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignDBRelationCreator
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using Intermech.Signs.Interfaces;
using System;
using System.Data;

#nullable disable
namespace Intermech.Signs.Server;

internal class SignDBRelationCreator : IDBRelationCreator
{
  public IDBRelation CreateRelation(IUserSession uSession, Guid guid, DataTable relationParams)
  {
    return guid.Equals(SignsHolder.SignRelationTypeGuid) ? (IDBRelation) new SignDBRelation((UserSession) uSession, relationParams) : (IDBRelation) null;
  }
}
