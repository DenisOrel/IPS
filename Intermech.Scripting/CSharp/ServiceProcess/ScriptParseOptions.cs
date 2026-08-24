// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ServiceProcess.ScriptParseOptions
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp.ServiceProcess;

/// <summary>
/// Хранит настройки, необходимые для разбора исходного текста сценария на C#
/// </summary>
[Serializable]
public class ScriptParseOptions
{
  private static readonly string AutoReferencedAssembliesOption = nameof (AutoReferencedAssemblies);
  private static readonly string SearchPathListOption = nameof (SearchPathList);

  /// <summary>Создает объект.</summary>
  public ScriptParseOptions()
  {
    this.AutoReferencedAssemblies = new List<string>();
    this.SearchPathList = new List<string>();
  }

  /// <summary>
  /// Возвращает коллекцию имен файлов сборок, которые всегда передаются компилятору сценариев,
  /// даже если они не указаны в самом сценарии.
  /// </summary>
  public List<string> AutoReferencedAssemblies { get; }

  /// <summary>
  /// Возвращает список путей для поиска сборок, на которые имеются ссылки из сценариев.
  /// </summary>
  public List<string> SearchPathList { get; }

  /// <summary>
  /// Преобразует параметры разбора типа <see cref="T:Intermech.Scripting.CSharp.ServiceProcess.ScriptParseOptions" /> к типу <see cref="T:System.Collections.Generic.Dictionary`2" />
  /// </summary>
  /// <returns>Полученные параметры разбора в виде словаря либо null, если исходные параметры были равны null</returns>
  public static Dictionary<string, string> ToDictionary(ScriptParseOptions parseOptions)
  {
    if (parseOptions == null)
      throw new ArgumentNullException(nameof (parseOptions));
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    if (parseOptions.AutoReferencedAssemblies.Count != 0)
      dictionary.Add(ScriptParseOptions.AutoReferencedAssembliesOption, string.Join(";", (IEnumerable<string>) parseOptions.AutoReferencedAssemblies));
    if (parseOptions.SearchPathList.Count != 0)
      dictionary.Add(ScriptParseOptions.SearchPathListOption, string.Join(";", (IEnumerable<string>) parseOptions.SearchPathList));
    return dictionary;
  }

  /// <summary>
  /// Преобразует параметры разбора типа <see cref="T:System.Collections.Generic.Dictionary`2" /> к типу <see cref="T:Intermech.Scripting.CSharp.ServiceProcess.ScriptParseOptions" />
  /// </summary>
  /// <returns>Полученные параметры разбора либо null, если исходные словарь был равен null</returns>
  /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">в словаре <paramref name="parseOptions" /> не найден ключ с именем <see cref="!:LanguageVersionOption" /></exception>
  public static ScriptParseOptions FromDictionary(Dictionary<string, string> parseOptions)
  {
    if (parseOptions == null)
      throw new ArgumentNullException(nameof (parseOptions));
    ScriptParseOptions scriptParseOptions = new ScriptParseOptions();
    string str1;
    if (parseOptions.TryGetValue(ScriptParseOptions.AutoReferencedAssembliesOption, out str1))
    {
      scriptParseOptions.AutoReferencedAssemblies.AddRange((IEnumerable<string>) str1.Split(';'));
      scriptParseOptions.AutoReferencedAssemblies.RemoveAll(new Predicate<string>(string.IsNullOrEmpty));
    }
    string str2;
    if (parseOptions.TryGetValue(ScriptParseOptions.SearchPathListOption, out str2))
    {
      scriptParseOptions.SearchPathList.AddRange((IEnumerable<string>) str2.Split(';'));
      scriptParseOptions.SearchPathList.RemoveAll(new Predicate<string>(string.IsNullOrEmpty));
    }
    return scriptParseOptions;
  }
}
