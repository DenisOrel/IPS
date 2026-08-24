// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.EntryPointInfo`1
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System;
using System.Reflection;

#nullable disable
namespace Intermech.Scripting.Common;

public sealed class EntryPointInfo<T>
{
  private Lazy<MethodInfo> methodInfoCache;
  private Lazy<EntryPointInfo<T>.InvocationInfo> invocationInfoCache;

  public EntryPointInfo()
  {
    this.methodInfoCache = new Lazy<MethodInfo>(new Func<MethodInfo>(this.GetInvokeMethodSlow));
    this.invocationInfoCache = new Lazy<EntryPointInfo<T>.InvocationInfo>(new Func<EntryPointInfo<T>.InvocationInfo>(this.GetInvocationInfoSlow));
  }

  public Type[] ParameterTypes => this.invocationInfoCache.Value.ParameterTypes;

  public Type ReturnType => this.invocationInfoCache.Value.ReturnType;

  public bool HasReturnValue => this.invocationInfoCache.Value.HasReturnValue;

  public bool IsCompatible(object[] arguments)
  {
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    Type[] parameterTypes = this.ParameterTypes;
    if (arguments.Length != parameterTypes.Length)
      return false;
    for (int index = 0; index < arguments.Length; ++index)
    {
      if (!parameterTypes[index].IsAssignableFrom(arguments[index].GetType()))
        return false;
    }
    return true;
  }

  private MethodInfo GetInvokeMethodSlow()
  {
    Type c = typeof (T);
    MethodInfo methodInfo = typeof (Delegate).IsAssignableFrom(c) ? c.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public) : throw new Exception();
    return !(methodInfo == (MethodInfo) null) ? methodInfo : throw new Exception();
  }

  private EntryPointInfo<T>.InvocationInfo GetInvocationInfoSlow()
  {
    MethodInfo methodInfo = this.methodInfoCache.Value;
    ParameterInfo[] parameters = methodInfo.GetParameters();
    Type[] parameterTypes = new Type[parameters.Length];
    for (int index = 0; index < parameters.Length; ++index)
      parameterTypes[index] = parameters[index].ParameterType;
    Type returnType = methodInfo.ReturnType;
    return new EntryPointInfo<T>.InvocationInfo(parameterTypes, returnType);
  }

  private sealed class InvocationInfo
  {
    public InvocationInfo(Type[] parameterTypes, Type returnType)
    {
      this.ParameterTypes = parameterTypes;
      this.ReturnType = returnType;
      this.HasReturnValue = returnType != typeof (void);
    }

    public Type[] ParameterTypes { get; private set; }

    public Type ReturnType { get; private set; }

    public bool HasReturnValue { get; private set; }
  }
}
