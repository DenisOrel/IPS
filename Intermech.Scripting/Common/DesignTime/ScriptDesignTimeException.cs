// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.ScriptDesignTimeException
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

[Serializable]
public class ScriptDesignTimeException : ScriptingException
{
  /// <summary>Создает объект.</summary>
  /// <param name="message">Сообщение исключения</param>
  public ScriptDesignTimeException(string message)
    : base(message)
  {
  }

  /// <summary>Создает объект.</summary>
  /// <param name="message">Сообщение исключения</param>
  /// <pparam name="innerException">Вложенное исключение</pparam>
  public ScriptDesignTimeException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  /// <summary>Создает объект.</summary>
  /// <param name="info">Сериализованное представление объекта</param>
  /// <param name="context">Контекст сериализации</param>
  protected ScriptDesignTimeException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
