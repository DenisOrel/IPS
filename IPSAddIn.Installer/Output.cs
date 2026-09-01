// Decompiled with JetBrains decompiler
// Type: IPSAddIn.Installer.Output
// Assembly: IPSAddIn.Installer, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: 0B42B756-5F54-4959-820D-851B2C3E0C84
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn.Installer.exe

using System;

#nullable disable
namespace IPSAddIn.Installer;

internal static class Output
{
  public static void WriteLine(string text) => Console.WriteLine(text);

  public static void Write(string text) => Console.Write(text);

  public static string ReadLine() => Console.ReadLine();

  public static void ReadKey() => Console.ReadKey();

  public static void WriteColored(string text, ConsoleColor color)
  {
    int foregroundColor = (int) Console.ForegroundColor;
    Console.ForegroundColor = color;
    Output.WriteLine(text);
    Console.ForegroundColor = (ConsoleColor) foregroundColor;
  }

  public static void WriteError(string text) => Output.WriteColored(text, ConsoleColor.DarkRed);

  public static void WriteException(Exception ex, string additionalText)
  {
    Output.WriteError(ex.Message);
    Output.WriteError(additionalText);
  }
}
