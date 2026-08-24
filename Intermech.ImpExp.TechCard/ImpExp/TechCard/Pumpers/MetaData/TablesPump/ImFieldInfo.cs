// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.ImFieldInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

[Serializable]
internal class ImFieldInfo
{
  protected int _key;
  protected int _tableId;
  protected string _field;
  protected string _units;
  protected int _sort;
  protected long _width;
  protected int _flags;
  protected ImDataMode _dataMode;
  protected int _required;
  protected ImDataTypeEx _dataType;
  protected ImEnterMode _enterMode;
  protected string _data;
  protected Guid _ipsAttrTypeGuid = Guid.Empty;

  public ImFieldInfo(string longName, string shortName, string alias, FieldTypes fieldType)
  {
  }

  public ImFieldInfo()
    : this("", "", "", FieldTypes.ftUnknown)
  {
  }

  public ImFieldInfo(
    int key,
    int tableId,
    string field,
    string longName,
    string shortName,
    string units,
    int sort,
    int flags,
    ImDataMode dataMode,
    int required,
    ImDataTypeEx dataType,
    long width,
    ImEnterMode enterMode,
    string data)
  {
    this._key = key;
    this._tableId = tableId;
    this._field = field;
    this._units = units;
    this._sort = sort;
    this._flags = flags;
    this._dataMode = dataMode;
    this._required = required;
    this._dataType = dataType;
    this._width = width;
    this._enterMode = enterMode;
    this._data = data;
  }

  public string UniqueKey(string tableName) => $"{tableName}.{this._field}";

  public int Key => this._key;

  public int TableId => this._tableId;

  public string Field => this._field;

  public string Units => this._units;

  public int Sort => this._sort;

  public long Width
  {
    get => this._width;
    set
    {
      if (this._width == value)
        return;
      this._width = value;
    }
  }

  public int Flags => this._flags;

  public ImDataMode DataMode => this._dataMode;

  public int Required => this._required;

  public ImDataTypeEx DataType
  {
    get => this._dataType;
    set
    {
      if (this._dataType == value)
        return;
      this._dataType = value;
    }
  }

  public ImEnterMode EnterMode
  {
    get => this._enterMode;
    set => this._enterMode = value;
  }

  public string Data => this._data;

  public Guid IpsAttrTypeGuid
  {
    get => this._ipsAttrTypeGuid;
    set
    {
      if (!(this._ipsAttrTypeGuid != value))
        return;
      this._ipsAttrTypeGuid = value;
    }
  }
}
