// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.IReadOnlyTextDocument
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

#nullable disable
namespace Intermech.Scripting.Common.DesignTime;

/// <summary>
/// Абстракция для текстового документа, доступного только для чтения,
/// позволяющая работать с ним без преобразования всего документа в одну строку.
/// Реализации этого интерфейса используются для интеграции языковых сервисов и документов IDE.
/// Реализация не обязана быть thread safe.
/// </summary>
public interface IReadOnlyTextDocument : ITextDocument
{
}
