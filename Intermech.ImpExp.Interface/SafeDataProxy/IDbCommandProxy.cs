// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.SafeDataProxy.IDbCommandProxy
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Interface.SafeDataProxy;

/// <summary>Обертка над стандартным IDbCommand</summary>
public interface IDbCommandProxy : IDbCommand, IDisposable
{
  /// <summary>
  /// 
  /// </summary>
  IDbCommand Target { get; }
}
