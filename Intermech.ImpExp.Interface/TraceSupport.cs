// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TraceSupport
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System.Diagnostics;

#nullable disable
namespace Intermech.ImpExp.Interface;

public static class TraceSupport
{
  public static readonly BooleanSwitch PluginConnections = new BooleanSwitch(nameof (PluginConnections), string.Empty, "0");
  public static readonly BooleanSwitch ImpExpTrace = new BooleanSwitch("ImpExp.Trace", string.Empty, "0");
}
