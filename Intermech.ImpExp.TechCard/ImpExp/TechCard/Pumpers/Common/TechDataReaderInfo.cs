// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechDataReaderInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common;

public class TechDataReaderInfo : IDisposable
{
  private readonly string _dataType;
  private readonly string _dopType;
  private readonly string _tableName;
  private int _recordCount;
  private IDataReader _dataReader;

  public TechDataReaderInfo(
    string dataType,
    string dopType,
    IDataReader dataReader,
    string tableName)
    : this(dataType, dopType, dataReader, tableName, -1)
  {
  }

  public TechDataReaderInfo(
    string dataType,
    string dopType,
    IDataReader dataReader,
    string tableName,
    int recordCount)
  {
    this._dataType = dataType;
    this._dopType = dopType;
    this._dataReader = dataReader;
    this._tableName = tableName;
    this._recordCount = recordCount;
  }

  public string DataType => this._dataType;

  public string DopType => this._dopType;

  public string TableName => this._tableName;

  public int RecordCount
  {
    get => this._recordCount;
    set => this._recordCount = value;
  }

  public IDataReader DataReader => this._dataReader;

  public override int GetHashCode() => this._dataType.GetHashCode() ^ this._dopType.GetHashCode();

  public override bool Equals(object obj)
  {
    if (!(obj is TechDataReaderInfo techDataReaderInfo))
      return base.Equals(obj);
    return this._dataType.Equals(techDataReaderInfo._dataType) && this._dopType.Equals(techDataReaderInfo._dopType);
  }

  public void Dispose()
  {
    if (this._dataReader == null)
      return;
    this._dataReader.Close();
    this._dataReader = (IDataReader) null;
  }

  ~TechDataReaderInfo() => this.Dispose();
}
