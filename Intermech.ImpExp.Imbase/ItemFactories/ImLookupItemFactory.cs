// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.ImLookupItemFactory
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal class ImLookupItemFactory : PumpItemFactory
{
  public static string TableName = "IM_LOOKUP";
  private static int idxKey = -1;
  private static int idxOwner = -1;
  private static int idxType = -1;
  private static int idxName = -1;
  private static int idxStr = -1;
  private static int idxInt = -1;
  private static int idxDbl = -1;

  public ImLookupItemFactory(IDataReader idr, IAppManager appMgr)
    : base(ImLookupItemFactory.TableName, idr, appMgr)
  {
    string fieldName1 = "F_KEY";
    string fieldName2 = "F_OWNER";
    string fieldName3 = "F_TYPE";
    string fieldName4 = "F_NAME";
    string fieldName5 = "F_STR";
    string fieldName6 = "F_INT";
    string fieldName7 = "F_DBL";
    ImLookupItemFactory.idxKey = this.getFieldIndex(fieldName1);
    ImLookupItemFactory.idxOwner = this.getFieldIndex(fieldName2);
    ImLookupItemFactory.idxType = this.getFieldIndex(fieldName3);
    ImLookupItemFactory.idxName = this.getFieldIndex(fieldName4);
    ImLookupItemFactory.idxStr = this.getFieldIndex(fieldName5);
    ImLookupItemFactory.idxInt = this.getFieldIndex(fieldName6);
    ImLookupItemFactory.idxDbl = this.getFieldIndex(fieldName7);
  }

  public IImLookupItem NewItem(IDataReader idr)
  {
    return (IImLookupItem) new ImLookupItemFactory.ImLookupItem()
    {
      key = this.getInt32(idr, ImLookupItemFactory.idxKey),
      owner = this.getInt32(idr, ImLookupItemFactory.idxOwner),
      dataType = (ImLookupRecordType) this.getInt32(idr, ImLookupItemFactory.idxType),
      name = this.getString(idr, ImLookupItemFactory.idxName).Trim(),
      valueStr = this.getString(idr, ImLookupItemFactory.idxStr).Trim(),
      valueInt = this.getInt32(idr, ImLookupItemFactory.idxInt),
      valueDbl = this.getDouble(idr, ImLookupItemFactory.idxDbl)
    };
  }

  protected class ImLookupItem : IImLookupItem
  {
    internal int key;
    internal int owner;
    internal ImLookupRecordType dataType;
    internal string name = "";
    internal string valueStr = "";
    internal int valueInt;
    internal double valueDbl;

    public int Key => this.key;

    public int Owner => this.owner;

    public ImLookupRecordType DataType => this.dataType;

    public string Name => this.name;

    public string ValueStr => this.valueStr;

    public int ValueInt => this.valueInt;

    public double ValueDbl => this.valueDbl;
  }
}
