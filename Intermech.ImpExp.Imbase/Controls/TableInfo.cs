// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Controls.TableInfo
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;

#nullable disable
namespace Intermech.ImpExp.Imbase.Controls;

internal class TableInfo
{
  public int TableID;
  public ImTablesType TableType;

  public TableInfo(int tableID, ImTablesType tableType)
  {
    this.TableID = tableID;
    this.TableType = tableType;
  }
}
