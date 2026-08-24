// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.Hosting.AppDomainSearchPathListProvider
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#nullable disable
namespace Intermech.Scripting.CSharp.Hosting;

/// <summary>
/// Провайдер путей поиска сборок для компиляции сценария, создаваемый на основе списка уже загруженных сборок в текущий AppDomain.
/// Реализация должна быть thread safe и доступной через remoting.
/// </summary>
public sealed class AppDomainSearchPathListProvider : SearchPathListProvider
{
  private AppDomain currentAppDomain;
  private Lazy<ICollection<string>> cachedResult;

  /// <summary>Создает объект.</summary>
  public AppDomainSearchPathListProvider()
  {
    this.currentAppDomain = AppDomain.CurrentDomain;
    this.cachedResult = new Lazy<ICollection<string>>(new Func<ICollection<string>>(this.GetSearchPathListSlow));
  }

  /// <summary>
  /// Возвращает коллекцию путей поиска сборок для компиляции C#-сценария.
  /// </summary>
  /// <returns>Коллекция путей к каталогам</returns>
  public override ICollection<string> GetSearchPathList() => this.cachedResult.Value;

  private ICollection<string> GetSearchPathListSlow()
  {
    Assembly[] assemblies = this.currentAppDomain.GetAssemblies();
    List<string> searchPathListSlow = new List<string>();
    foreach (Assembly assembly in assemblies)
    {
      if (!assembly.IsDynamic && !assembly.GlobalAssemblyCache && !string.IsNullOrEmpty(assembly.Location))
      {
        string directoryName = Path.GetDirectoryName(assembly.Location);
        if (!searchPathListSlow.Contains(directoryName))
          searchPathListSlow.Add(directoryName);
      }
    }
    return (ICollection<string>) searchPathListSlow;
  }
}
