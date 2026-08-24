// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.ScriptAssemblyChecker
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Scripting.CSharp;

internal sealed class ScriptAssemblyChecker
{
  public void CheckAssembly(Assembly scriptAssembly)
  {
    if (scriptAssembly == (Assembly) null)
      throw new ArgumentNullException(nameof (scriptAssembly));
    if (!this.IsInstanceScript(scriptAssembly))
      throw new ScriptStructureException("В коде C#-сценария не должно быть статических полей данных и статических свойств, доступных для записи.");
  }

  private bool IsInstanceScript(Assembly scriptAssembly)
  {
    foreach (Type type in scriptAssembly.GetTypes())
    {
      if (!type.IsDefined(typeof (CompilerGeneratedAttribute), true))
      {
        foreach (MemberInfo member in type.GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
        {
          if ((object) (member as FieldInfo) != null && member.DeclaringType.Assembly == scriptAssembly && !this.IsStaticFieldAllowedInInstanceScript((FieldInfo) member) || (object) (member as PropertyInfo) != null && member.DeclaringType.Assembly == scriptAssembly && !this.IsStaticPropertyAllowedInInstanceScript((PropertyInfo) member))
            return false;
        }
      }
    }
    return true;
  }

  private bool IsStaticFieldAllowedInInstanceScript(FieldInfo staticField)
  {
    return staticField.IsLiteral || staticField.IsDefined(typeof (CompilerGeneratedAttribute), true);
  }

  private bool IsStaticPropertyAllowedInInstanceScript(PropertyInfo staticProperty)
  {
    return !staticProperty.CanWrite;
  }
}
