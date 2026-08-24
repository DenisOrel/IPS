// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.ImTableInfo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

[Serializable]
internal class ImTableInfo
{
  private readonly int _recordId;
  private readonly string _recordName;
  private readonly int _tableKey;
  private readonly string _tableName;
  private Guid _ipsObjectVersionGuid = Guid.Empty;
  private long _ipsObjectVersionId;

  public ImTableInfo(int tableKey, string tableName, int recordId, string recordName)
  {
    this._tableKey = tableKey;
    this._tableName = tableName;
    this._recordId = recordId;
    this._recordName = recordName;
  }

  public int RecordId => this._recordId;

  public string RecordName => this._recordName;

  public int TableKey => this._tableKey;

  public string TableName => this._tableName;

  public Guid IpsObjectVersionGuid
  {
    get => this._ipsObjectVersionGuid;
    internal set => this._ipsObjectVersionGuid = value;
  }

  public long IpsObjectVersionId
  {
    get => this._ipsObjectVersionId;
    internal set => this._ipsObjectVersionId = value;
  }
}
