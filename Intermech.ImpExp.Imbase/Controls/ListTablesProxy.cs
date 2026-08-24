// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.ListTablesProxy
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

[Editor(typeof (ListTablesEditor), typeof (UITypeEditor))]
public class ListTablesProxy
{
  private List<string> _tableNames;

  public ListTablesProxy() => this._tableNames = new List<string>();

  public List<string> TableNames => this._tableNames;

  public ListTablesProxy(List<string> tableNames) => this._tableNames = tableNames;

  public override string ToString()
  {
    if (this._tableNames.Count == 1)
      return this._tableNames[0];
    return this._tableNames.Count > 1 ? $"{this._tableNames[0]}, ... " : "Не назначен";
  }
}
