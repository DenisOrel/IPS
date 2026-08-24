// Decompiled with JetBrains decompiler
// Type: DiffPlex.Log
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System.Diagnostics;

#nullable disable
namespace DiffPlex;

public static class Log
{
  [Conditional("LOG")]
  public static void WriteLine(string format, params object[] args)
  {
  }

  [Conditional("LOG")]
  public static void Write(string format, params object[] args)
  {
  }
}
