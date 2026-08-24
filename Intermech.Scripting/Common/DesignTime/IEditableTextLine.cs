// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IEditableTextLine
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Абстракция для строки редактируемого текстового документа.
/// Реализации этого интерфейса используются для интеграции языковых сервисов и документов IDE.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface IEditableTextLine
{
  int Offset { get; }

  int Length { get; }

  IEditableTextLine TryGetPreviousLine();

  IEditableTextLine TryGetNextLine();
}
