// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ILanguageSessionParameters
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс объекта с параметрами создания языковой сессии.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface ILanguageSessionParameters : ICloneable
{
  /// <summary>
  /// Позволяет задать объект для перехвата потока stdout у исполнителя сценариев.
  /// Свойство може быть не задано, если перехват не требуется. Свойство не является обязательным, и
  /// конкретные реализации исполнителей сценариев могут его игнорировать.
  /// </summary>
  IScriptOutputStream Stdout { get; set; }
}
