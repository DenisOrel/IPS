// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump.ImTableList
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.TechTypes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;

[Obsolete("Данный класс более не используется, он заменен классом ImTableInfoCache")]
[Serializable]
internal class ImTableList : Dictionary<int, ImTableInfo>
{
  public ImTableList()
  {
  }

  public ImTableList(SerializationInfo serializationInfo, StreamingContext streamingContext)
    : base(serializationInfo, streamingContext)
  {
  }

  public Guid GetIMBASEObjectGuid(TechcardConsts.imTablesConsts id)
  {
    ImTableInfo imTableInfo = (ImTableInfo) null;
    return this.TryGetValue(Convert.ToInt32((object) id), out imTableInfo) && imTableInfo != null ? imTableInfo.IpsObjectVersionGuid : Guid.Empty;
  }

  public virtual void Add(int tableKey, string tableName, int recordId, string recordName)
  {
    ImTableInfo imTableInfo = new ImTableInfo(tableKey, tableName, recordId, recordName);
    if (this.ContainsKey(recordId))
      return;
    this.Add(recordId, imTableInfo);
  }

  public string GetTableName(string typeCode, TechTypeList typelist)
  {
    int key = 0;
    foreach (TechTypeInfo techTypeInfo in typelist.Values)
    {
      if (techTypeInfo.Type == typeCode)
      {
        key = techTypeInfo.PredefID;
        break;
      }
    }
    ImTableInfo imTableInfo = (ImTableInfo) null;
    return this.TryGetValue(key, out imTableInfo) ? imTableInfo.TableName : string.Empty;
  }

  public string GetTableName(int tableId)
  {
    foreach (ImTableInfo imTableInfo in this.Values)
    {
      if (tableId == imTableInfo.TableKey)
        return imTableInfo.TableName;
    }
    string Message = $"Таблица с идентификатором {tableId.ToString()} не найдена в кэше imTableList";
    TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
    return string.Empty;
  }

  public string GetTableName(TechcardConsts.imTablesConsts recordId)
  {
    ImTableInfo imTableInfo = (ImTableInfo) null;
    if (this.TryGetValue(Convert.ToInt32((object) recordId), out imTableInfo))
      return imTableInfo.TableName;
    string Message = $"Таблица с идентификатором {recordId.ToString()} не найдена в кэше imTableList";
    TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
    return string.Empty;
  }

  public string GetName(string typeCode, TechTypeList typelist)
  {
    int key = 0;
    foreach (TechTypeInfo techTypeInfo in typelist.Values)
    {
      if (techTypeInfo.Type == typeCode)
      {
        key = techTypeInfo.PredefID;
        break;
      }
    }
    ImTableInfo imTableInfo = (ImTableInfo) null;
    return this.TryGetValue(key, out imTableInfo) ? imTableInfo.RecordName : string.Empty;
  }

  public int GetTableCod(string TypeCode, TechTypeList typelist)
  {
    int key = 0;
    foreach (TechTypeInfo techTypeInfo in typelist.Values)
    {
      if (techTypeInfo.Type == TypeCode)
      {
        key = techTypeInfo.PredefID;
        break;
      }
    }
    ImTableInfo imTableInfo = (ImTableInfo) null;
    return this.TryGetValue(key, out imTableInfo) ? imTableInfo.TableKey : -1;
  }
}
