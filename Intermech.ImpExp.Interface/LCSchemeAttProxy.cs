// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.LCSchemeAttProxy
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.ImpExp.Interface;

[Editor(typeof (LCSchemeEditor), typeof (UITypeEditor))]
public class LCSchemeAttProxy
{
  private Guid _id;
  private string _schemeName;

  public LCSchemeAttProxy()
  {
    this._id = Guid.Empty;
    this._schemeName = string.Empty;
  }

  public LCSchemeAttProxy(Guid id, string name)
  {
    this._id = id;
    this._schemeName = name;
  }

  public override string ToString()
  {
    if (this._id == Guid.Empty)
      return "Не назначена";
    return this._schemeName.Length <= 0 ? this._id.ToString() : this._schemeName;
  }

  public Guid LCScheme => this._id;
}
