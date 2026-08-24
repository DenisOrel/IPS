// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Utils.TransparentProxyAdapter
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Castle.DynamicProxy;
using Intermech.Interfaces;
using System;
using System.Runtime.Remoting;

#nullable disable
namespace Intermech.Scripting.Utils;

internal sealed class TransparentProxyAdapter : LongLifeObject
{
  private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();
  private static readonly TransparentProxyAdapter.DefaultInterceptor tpInterceptor = new TransparentProxyAdapter.DefaultInterceptor();
  private static readonly IInterceptor[] defaultInterceptors = new IInterceptor[1]
  {
    (IInterceptor) TransparentProxyAdapter.tpInterceptor
  };

  public static object CreateAdapter(object obj, Type targetInterface)
  {
    if (obj == null)
      throw new ArgumentNullException(nameof (obj));
    if (targetInterface == (Type) null)
      throw new ArgumentNullException(nameof (targetInterface));
    return TransparentProxyAdapter.proxyGenerator.CreateInterfaceProxyWithTargetInterface(targetInterface, Type.EmptyTypes, obj, TransparentProxyAdapter.defaultInterceptors);
  }

  internal sealed class DefaultInterceptor : MarshalByRefObject, IInterceptor
  {
    public void Intercept(IInvocation invocation)
    {
      invocation.Proceed();
      if (invocation.ReturnValue == null)
        return;
      TransparentProxyAdapter.DefaultInterceptor.WrapReturnValue(invocation);
    }

    private static void WrapReturnValue(IInvocation invocation)
    {
      if (invocation.Proxy is IUserSession && invocation.Method.Name == "GetCustomService")
      {
        invocation.ReturnValue = TransparentProxyAdapter.CreateAdapter(invocation.ReturnValue, (Type) invocation.Arguments[0]);
      }
      else
      {
        if (!RemotingServices.IsTransparentProxy(invocation.ReturnValue) || !(invocation.Method.ReturnType != typeof (object)))
          return;
        invocation.ReturnValue = TransparentProxyAdapter.CreateAdapter(invocation.ReturnValue, invocation.Method.ReturnType);
      }
    }
  }
}
