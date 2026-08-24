// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ICodeNavigationService
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Сервис кодовой модели для получения списка типов и их дочерних элементов, содержащихся в тексте сценария.
/// Полученная информация размещается в выпадающих списках и используется для навигации по сценарию.
/// Реализация не должна быть thread safe.
/// </summary>
public interface ICodeNavigationService : ICodeModel
{
  /// <summary>
  /// Возвращает список описателей типов с полным именем, расположением и
  /// списком дочерних элементов, содержащихся в сценарии.
  /// </summary>
  /// <returns>Список описателей типов</returns>
  IList<NavigationItem> GetNavigationItems();
}
