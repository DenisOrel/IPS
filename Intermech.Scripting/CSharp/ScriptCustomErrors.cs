// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ScriptCustomErrors
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Ошибки компиляции в C#-сценариях, специфических для IPS.
/// </summary>
public static class ScriptCustomErrors
{
  public const string MissingFilenameInCssRef = "IPS001";
  public const string MissingSemicolonInCssRef = "IPS002";
  public const string InvalidFilenameInCssRef = "IPS003";
  public const string InvalidCharactersInCssRef = "IPS004";
}
