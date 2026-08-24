// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ObjectTypeAttProxy
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.ImpExp.Interface;

[Editor(typeof (ObjectTypeEditor), typeof (UITypeEditor))]
public class ObjectTypeAttProxy
{
  protected Guid id;
  protected string typeName;

  public ObjectTypeAttProxy()
  {
    this.id = Guid.Empty;
    this.typeName = string.Empty;
  }

  public ObjectTypeAttProxy(Guid id, string name)
  {
    this.id = id;
    this.typeName = name;
  }

  public override string ToString()
  {
    if (this.id == Guid.Empty)
      return "Не назначен";
    return this.typeName.Length <= 0 ? this.id.ToString() : this.typeName;
  }

  public Guid ObjectType => this.id;
}
