// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.CSharp.MainDomainScriptRuntimeHelper
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using Intermech.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Intermech.Scripting.CSharp;

/// <summary>Реализация является thread safe и immutable.</summary>
internal sealed class MainDomainScriptRuntimeHelper
{
  public MainDomainScriptRuntimeHelper(
    Type scriptType,
    ConstructorInfo ctorMethod,
    MethodInfo executeMethod,
    PropertyInfo scriptContextProperty,
    PropertyInfo[] serviceProperties)
  {
    this.ScriptType = scriptType;
    this.CtorMethod = ctorMethod;
    this.HasExecuteMethod = executeMethod != (MethodInfo) null;
    this.ExecuteMethod = executeMethod;
    this.ScriptContextProperty = scriptContextProperty;
    this.HasServiceProperties = serviceProperties.Length != 0;
    this.ServiceProperties = serviceProperties;
    this.ServicePropertyTypes = this.HasServiceProperties ? CollectionUtils.ConvertAsArray<PropertyInfo, string>((ICollection<PropertyInfo>) serviceProperties, (Converter<PropertyInfo, string>) (item => item.PropertyType.AssemblyQualifiedName)) : new string[0];
  }

  public Type ScriptType { get; private set; }

  public ConstructorInfo CtorMethod { get; private set; }

  public bool HasExecuteMethod { get; private set; }

  public MethodInfo ExecuteMethod { get; private set; }

  public PropertyInfo ScriptContextProperty { get; private set; }

  public bool HasServiceProperties { get; private set; }

  public PropertyInfo[] ServiceProperties { get; private set; }

  public string[] ServicePropertyTypes { get; private set; }
}
