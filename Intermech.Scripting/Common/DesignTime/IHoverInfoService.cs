// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IHoverInfoService
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Сервис кодовой модели для получения всплывающих подсказок для текста под курсором.
/// Как правило, используется для отображения xml-документации при наведении курсора
/// на имена классов, методов, свойств и т.д.
/// Реализация не должна быть thread safe.
/// </summary>
public interface IHoverInfoService : ICodeModel
{
  /// <summary>
  /// Возвращает всплывающую подсказку для текста под курсором.
  /// Предварительно требуется вызвать метод <see cref="M:Intermech.Scripting.Common.DesignTime.ICodeModel.CheckSynchronizationStatus" /> и убедиться в наличии полной синхронизации с редактором.
  /// </summary>
  /// <param name="offset">Смещение курсора в символах от начала текста сценария</param>
  /// <returns>Текст всплывающей подсказки (может быть пустой)</returns>
  /// <exception cref="T:System.ArgumentOutOfRangeException">параметр <paramref name="offset" /> содержит отрицательное значение</exception>
  /// <exception cref="T:System.InvalidOperationException">Метод проверки синхронизации не был вызван</exception>
  HoverInfo GetHoverInfo(int offset);
}
