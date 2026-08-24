// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptPad.IDESettingsService
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Scripting.Common.DesignTime;
using System;

#nullable disable
namespace Intermech.Scripting.ScriptPad;

internal abstract class IDESettingsService : ISettingsContainer
{
  public bool ReadBoolean(string sectionName, string parameterName, bool defaultValue)
  {
    if (sectionName == null)
      throw new ArgumentNullException(nameof (sectionName));
    if (parameterName == null)
      throw new ArgumentNullException(nameof (parameterName));
    return this.ReadInternal<bool>(sectionName, parameterName, defaultValue);
  }

  public int ReadInt32(string sectionName, string parameterName, int defaultValue)
  {
    if (sectionName == null)
      throw new ArgumentNullException(nameof (sectionName));
    if (parameterName == null)
      throw new ArgumentNullException(nameof (parameterName));
    return this.ReadInternal<int>(sectionName, parameterName, defaultValue);
  }

  public string ReadString(string sectionName, string parameterName, string defaultValue)
  {
    if (sectionName == null)
      throw new ArgumentNullException(nameof (sectionName));
    if (parameterName == null)
      throw new ArgumentNullException(nameof (parameterName));
    return this.ReadInternal<string>(sectionName, parameterName, defaultValue);
  }

  private T ReadInternal<T>(string sectionName, string parameterName, T defaultValue)
  {
    Tuple<Type, object> tuple = this.DoTryReadParameter(Tuple.Create<string, string>(sectionName, parameterName));
    return tuple != null && tuple.Item1 == typeof (T) && tuple.Item2 != null ? (T) tuple.Item2 : defaultValue;
  }

  public void WriteBoolean(string sectionName, string parameterName, bool value)
  {
    if (sectionName == null)
      throw new ArgumentNullException(nameof (sectionName));
    if (parameterName == null)
      throw new ArgumentNullException(nameof (parameterName));
    this.WriteInternal<bool>(sectionName, parameterName, value);
  }

  public void WriteInt32(string sectionName, string parameterName, int value)
  {
    if (sectionName == null)
      throw new ArgumentNullException(nameof (sectionName));
    if (parameterName == null)
      throw new ArgumentNullException(nameof (parameterName));
    this.WriteInternal<int>(sectionName, parameterName, value);
  }

  public void WriteString(string sectionName, string parameterName, string value)
  {
    if (sectionName == null)
      throw new ArgumentNullException(nameof (sectionName));
    if (parameterName == null)
      throw new ArgumentNullException(nameof (parameterName));
    this.WriteInternal<string>(sectionName, parameterName, value);
  }

  private void WriteInternal<T>(string sectionName, string parameterName, T value)
  {
    this.DoWriteParameter(Tuple.Create<string, string>(sectionName, parameterName), Tuple.Create<Type, object>(typeof (T), (object) value));
  }

  public void Flush() => this.DoFlush();

  protected abstract Tuple<Type, object> DoTryReadParameter(Tuple<string, string> key);

  protected abstract void DoWriteParameter(
    Tuple<string, string> key,
    Tuple<Type, object> typeAndValue);

  protected abstract void DoFlush();
}
