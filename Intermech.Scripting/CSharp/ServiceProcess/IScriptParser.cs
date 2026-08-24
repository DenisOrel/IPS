// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ServiceProcess.IScriptParser
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common.DesignTime;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.CSharp.ServiceProcess;

/// <summary>
/// Интерфейс парсера C#-сценариев.
/// Реализация должна быть thread safe.
/// </summary>
public interface IScriptParser
{
  /// <summary>
  /// Возвращает список описателей типов с полным именем, расположением и списком дочерних элементов, содержащихся в сценарии с данным <paramref name="documentUri" />.
  /// </summary>
  /// <param name="documentUri">Uri документа</param>
  /// <returns>Список описателей типов сценария</returns>
  IList<NavigationItem> GetNavigationItems(Uri documentUri);

  /// <summary>Возвращает список регионов содержащихся в сценарии</summary>
  /// <param name="documentUri">Uri документа</param>
  /// <returns></returns>
  IList<FoldingRegionItem> GetFoldingRegions(Uri documentUri);

  /// <summary>
  /// Возвращает документацию на указанный элемент в коде сценария
  /// </summary>
  /// <param name="documentUri">Uri документа</param>
  /// <param name="offset">Смещение в символах от начала документа</param>
  /// <returns>Документация на элемент в коде сценария</returns>
  string GetDocumentation(Uri documentUri, int offset);
}
