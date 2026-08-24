// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.ImTableInfoCache
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

[Serializable]
internal class ImTableInfoCache
{
  private readonly Dictionary<int, ImTableInfo> _recordId2TableInfo = new Dictionary<int, ImTableInfo>();
  private readonly Dictionary<int, ImTableInfo> _tableKey2TableInfo = new Dictionary<int, ImTableInfo>();
  private readonly List<ImTableInfo> _tableInfoList = new List<ImTableInfo>();

  public ImTableInfoCache()
  {
  }

  public ImTableInfoCache(SerializationInfo serializationInfo, StreamingContext streamingContext)
  {
  }

  public virtual void Add(ImTableInfo tableInfo)
  {
    if (tableInfo == null)
      throw new ArgumentNullException(nameof (tableInfo));
    this._tableInfoList.Add(tableInfo);
    if (tableInfo.RecordId != 0)
      this._recordId2TableInfo[tableInfo.RecordId] = tableInfo;
    this._tableKey2TableInfo[tableInfo.TableKey] = tableInfo;
  }

  public IEnumerable<ImTableInfo> GetAllTableInfo()
  {
    return (IEnumerable<ImTableInfo>) this._tableInfoList;
  }

  public ImTableInfo GetTableInfo(int tableKey)
  {
    ImTableInfo tableInfo;
    if (this._tableKey2TableInfo.TryGetValue(tableKey, out tableInfo))
      return tableInfo;
    string Message = $"Таблица с идентификатором {tableKey.ToString()} не найдена в кэше ImTableInfoCache";
    TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
    return (ImTableInfo) null;
  }

  public ImTableInfo GetTableInfo(TechcardConsts.imTablesConsts recordId)
  {
    ImTableInfo tableInfo;
    if (this._recordId2TableInfo.TryGetValue(Convert.ToInt32((object) recordId), out tableInfo))
      return tableInfo;
    if (recordId != TechcardConsts.imTablesConsts.Unknown)
    {
      string Message = $"Таблица с идентификатором {recordId} не найдена в кэше ImTableInfoCache";
      TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
    }
    return (ImTableInfo) null;
  }

  public string GetTableName(int tableKey)
  {
    ImTableInfo tableInfo = this.GetTableInfo(tableKey);
    return tableInfo != null ? tableInfo.TableName : string.Empty;
  }

  public string GetTableName(TechcardConsts.imTablesConsts recordId)
  {
    ImTableInfo tableInfo = this.GetTableInfo(recordId);
    return tableInfo != null ? tableInfo.TableName : string.Empty;
  }

  public Guid GetIpsImObjectGuid(TechcardConsts.imTablesConsts recordId)
  {
    ImTableInfo tableInfo = this.GetTableInfo(recordId);
    return tableInfo != null ? tableInfo.IpsObjectVersionGuid : Guid.Empty;
  }
}
