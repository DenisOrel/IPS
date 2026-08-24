// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ISettingsContainer
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

public interface ISettingsContainer
{
  bool ReadBoolean(string sectionName, string parameterName, bool defaultValue);

  int ReadInt32(string sectionName, string parameterName, int defaultValue);

  string ReadString(string sectionName, string parameterName, string defaultValue);

  void WriteBoolean(string sectionName, string parameterName, bool value);

  void WriteInt32(string sectionName, string parameterName, int value);

  void WriteString(string sectionName, string parameterName, string value);
}
