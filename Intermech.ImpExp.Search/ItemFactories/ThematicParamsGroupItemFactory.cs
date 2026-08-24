// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.ItemFactories.ThematicParamsGroupItemFactory
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.ImpExp.Interface;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Search.ItemFactories;

internal class ThematicParamsGroupItemFactory : PumpItemFactory
{
  public static string TableName = "PARAMSGROUP";
  public static string TableColumns = "GROUP_ID, G_LABEL";
  private static int idxGroupId = -1;
  private static int idxLabel = -1;

  public ThematicParamsGroupItemFactory(
    string tableName,
    IDataReader dataReader,
    IAppManager appManager)
    : base(tableName, dataReader, appManager)
  {
    string fieldName1 = "GROUP_ID";
    string fieldName2 = "G_LABEL";
    ThematicParamsGroupItemFactory.idxGroupId = this.getFieldIndex(fieldName1);
    ThematicParamsGroupItemFactory.idxLabel = this.getFieldIndex(fieldName2);
  }

  public IThematicParamsGroupItem NewItem(IDataReader idr)
  {
    return (IThematicParamsGroupItem) new ThematicParamsGroupItemFactory.ThematicParamsGroupItem()
    {
      groupId = this.getInt32(idr, ThematicParamsGroupItemFactory.idxGroupId),
      label = this.getString(idr, ThematicParamsGroupItemFactory.idxLabel),
      Guid = Guid.NewGuid()
    };
  }

  private class ThematicParamsGroupItem : IThematicParamsGroupItem
  {
    internal int groupId;
    internal string label;
    private Guid guid;
    private string note = string.Empty;

    public int GroupId => this.groupId;

    public string Label
    {
      get => this.label;
      set => this.label = value;
    }

    public string Note
    {
      get => this.note;
      set => this.note = value;
    }

    public Guid Guid
    {
      get => this.guid;
      set => this.guid = value;
    }
  }
}
