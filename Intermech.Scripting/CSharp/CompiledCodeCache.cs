// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.CompiledCodeCache
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Scripting.Common;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>
/// Сервис для кэширования результатов компиляции C#-сценариев.
/// Реализация класса является thread safe и доступна для обращений из изолированных AppDomain.
/// </summary>
internal sealed class CompiledCodeCache : ScriptCompiledCodeCache<CompiledCodeInfo>
{
}
