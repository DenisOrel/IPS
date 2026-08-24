// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ExceptionLogger
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Interface;

public static class ExceptionLogger
{
  private static void Log(Exception e, StreamWriter sw)
  {
    sw.WriteLine("{0:G}\r\n", (object) DateTime.Now);
    sw.WriteLine(e.Message);
    sw.WriteLine(e.StackTrace);
    sw.WriteLine("");
    if (e.InnerException == null)
      return;
    ExceptionLogger.Log(e.InnerException, sw);
  }

  public static string GetExceptionInfo(Exception e)
  {
    string exceptionInfo = $"{(object) e.GetType()} {e.Message}";
    if (e.InnerException != null)
      exceptionInfo = $"{exceptionInfo} - {ExceptionLogger.GetExceptionInfo(e.InnerException)}";
    return exceptionInfo;
  }
}
