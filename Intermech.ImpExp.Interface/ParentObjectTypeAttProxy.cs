// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ParentObjectTypeAttProxy
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.ImpExp.Interface;

[Editor(typeof (ParentObjectTypeEditor), typeof (UITypeEditor))]
public class ParentObjectTypeAttProxy : ObjectTypeAttProxy
{
  public ParentObjectTypeAttProxy()
  {
  }

  public ParentObjectTypeAttProxy(Guid id, string name)
    : base(id, name)
  {
  }
}
