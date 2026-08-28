// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.TraceLog
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces.Server;
using System;
using System.Diagnostics;

#nullable disable
namespace Intermech.Portal.Server;

internal static class TraceLog
{
  private static BooleanSwitch _traceLog = new BooleanSwitch("Portal.TraceLog", string.Empty, "0");

  public static bool Enabled => TraceLog._traceLog.Enabled;

  public static void Write(string message)
  {
    (ServerServices.GetService(typeof (IEventLogHelper)) as IEventLogHelper).AddToTrace(message, Consts.traceAlways, string.Empty);
  }

  public static void Write(string message, Exception ex)
  {
    (ServerServices.GetService(typeof (IEventLogHelper)) as IEventLogHelper).TraceExeption(message, ex, string.Empty);
  }
}
