// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.Server.ReportsServerServiceCache
// Assembly: Intermech.Reports.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B97D7940-CE11-4EF0-80CD-76A0AE479D33
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Reports.Server.dll

using Intermech.Interfaces.Server;
using System;

#nullable disable
namespace Intermech.Reports.Server;

public static class ReportsServerServiceCache
{
  public static IServiceProvider ServiceProvider;
  public static IEventLogHelper EventLogHelper;
}
