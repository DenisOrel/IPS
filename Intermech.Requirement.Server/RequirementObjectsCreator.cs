// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.Server.RequirementObjectsCreator
// Assembly: Intermech.Requirement.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C85D341A-B4CB-4985-9EA3-68BB7F9530D7
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Requirement.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using System;
using System.Data;

#nullable disable
namespace Intermech.Requirement.Server;

public class RequirementObjectsCreator : IDBObjectCreator
{
  public IDBObject CreateObject(IUserSession uSession, Guid guid, DataTable objectParams)
  {
    return (IDBObject) new RequirementObjects((UserSession) uSession, objectParams);
  }
}
