// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Utils.OpenEventHandlerBuilder`1
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Intermech.Scripting.Utils;

internal sealed class OpenEventHandlerBuilder<T> where T : EventArgs
{
  private ConcurrentDictionary<MethodInfo, OpenEventHandler<T>> cache;
  private Func<MethodInfo, OpenEventHandler<T>> createOpenHandlerFunc;

  public OpenEventHandlerBuilder()
  {
    this.cache = new ConcurrentDictionary<MethodInfo, OpenEventHandler<T>>();
    this.createOpenHandlerFunc = new Func<MethodInfo, OpenEventHandler<T>>(this.CreateOpenHandlerSlow);
  }

  public OpenEventHandler<T> GetOpenEventHandler(MethodInfo method)
  {
    if (method == (MethodInfo) null)
      throw new ArgumentNullException(nameof (method));
    return this.cache.GetOrAdd(method, this.createOpenHandlerFunc);
  }

  private OpenEventHandler<T> CreateOpenHandlerSlow(MethodInfo method)
  {
    ParameterExpression parameterExpression4;
    ParameterExpression parameterExpression5;
    ParameterExpression parameterExpression6;
    return method.IsStatic ? Expression.Lambda<OpenEventHandler<T>>((Expression) Expression.Call(method, (Expression) parameterExpression5, (Expression) parameterExpression6), parameterExpression4, parameterExpression5, parameterExpression6).Compile() : ((Expression<OpenEventHandler<T>>) ((parameterExpression1, parameterExpression2, parameterExpression3) => Expression.Call((Expression) Expression.Convert(parameterExpression1, method.DeclaringType), method, parameterExpression2, parameterExpression3))).Compile();
  }
}
