// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.CatalogImbaseAttProxy
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

[Editor(typeof (CatalogImbaseEditor), typeof (UITypeEditor))]
public class CatalogImbaseAttProxy
{
  private Guid _id;
  private string _name;

  public CatalogImbaseAttProxy()
  {
    this._id = Guid.Empty;
    this._name = string.Empty;
  }

  public CatalogImbaseAttProxy(Guid id, string name)
  {
    this._id = id;
    this._name = name;
  }

  public override string ToString()
  {
    if (this._id == Guid.Empty)
      return "Не назначен";
    return this._name.Length <= 0 ? this._id.ToString() : this._name;
  }

  public Guid CatalogID => this._id;
}
