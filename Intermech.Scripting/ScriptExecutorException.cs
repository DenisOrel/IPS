// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.ScriptExecutorException
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Scripting;

/// <summary>
/// Базовый класс для собственных исключений исполнителя сценариев.
/// </summary>
[Serializable]
public class ScriptExecutorException : ScriptingException
{
  /// <summary>Создает объект.</summary>
  /// <param name="message">Сообщение исключения</param>
  public ScriptExecutorException(string message)
    : base(message)
  {
  }

  /// <summary>Создает объект.</summary>
  /// <param name="message">Сообщение исключения</param>
  /// <pparam name="innerException">Вложенное исключение</pparam>
  public ScriptExecutorException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  /// <summary>Создает объект.</summary>
  /// <param name="info">Сериализованное представление объекта</param>
  /// <param name="context">Контекст сериализации</param>
  protected ScriptExecutorException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
