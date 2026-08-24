// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ILanguageExtension
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Интерфейс расширения для скриптовой IDE, которое добавляет поддержку нового языка сценариев.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface ILanguageExtension
{
  /// <summary>Предоставляет базовую информацию о языке.</summary>
  LanguageInfo LanguageInfo { get; }

  /// <summary>Создает сервис языковых сессий.</summary>
  /// <returns>Объект сервиса</returns>
  ILanguageSessionService CreateLanguageSessionService();

  /// <summary>Создает шаблон для административного сценария.</summary>
  /// <returns>Шаблон административного сценария</returns>
  string CreateAdministrativeScriptTemplate();
}
