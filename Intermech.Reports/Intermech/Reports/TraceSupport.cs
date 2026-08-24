// Decompiled with JetBrains decompiler
// Type: Intermech.Reports.TraceSupport
// Assembly: Intermech.Reports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A20B4FCB-3CA6-4E39-8837-1BB71F87F99A
// Assembly location: D:\IPS\Client\Intermech.Reports.dll
// XML documentation location: D:\IPS\Client\Intermech.Reports.xml

using System.Diagnostics;

#nullable disable
namespace Intermech.Reports;

/// <summary>
/// 
/// </summary>
internal static class TraceSupport
{
  public static readonly BooleanSwitch DocumentRealign = new BooleanSwitch("Reports.DocumentRealign", string.Empty, "0");
}
