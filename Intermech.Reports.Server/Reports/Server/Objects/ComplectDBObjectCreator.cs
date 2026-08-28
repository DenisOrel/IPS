// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Server.Objects.ComplectDBObjectCreator
// Assembly: Intermech.Reports.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B97D7940-CE11-4EF0-80CD-76A0AE479D33
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Reports.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using System;
using System.Data;

#nullable disable
namespace Intermech.Reports.Server.Objects;

public class ComplectDBObjectCreator : IDBObjectCreator
{
  public IDBObject CreateObject(IUserSession uSession, Guid guid, DataTable objectParams)
  {
    return (IDBObject) new ComplectDBObject((UserSession) uSession, objectParams);
  }
}
