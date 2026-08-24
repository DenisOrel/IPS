// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechTypes.TechTypeList
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechTypes;

[Serializable]
public class TechTypeList : Dictionary<int, TechTypeInfo>
{
  [NonSerialized]
  private readonly Dictionary<Guid, int> _objTypeGuid2Id = new Dictionary<Guid, int>();

  private int ObjTypeGuid2Id(Guid objTypeGuid)
  {
    int num1 = -1;
    if (objTypeGuid == Guid.Empty || this._objTypeGuid2Id.TryGetValue(objTypeGuid, out num1))
      return num1;
    int num2;
    if (TechcardConsts.Plugin != null && TechcardConsts.Plugin.Imdi != null)
    {
      IObjectTypeItem byGuid = TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(objTypeGuid);
      num2 = byGuid != null ? byGuid.ID : num1;
    }
    else
      num2 = MetaDataHelper.GetObjectTypeID(objTypeGuid);
    if (num2 != -1)
      this._objTypeGuid2Id.Add(objTypeGuid, num2);
    return num2;
  }

  public TechTypeList()
  {
  }

  public TechTypeList(SerializationInfo serializationInfo, StreamingContext streamingContext)
    : base(serializationInfo, streamingContext)
  {
    this._objTypeGuid2Id = new Dictionary<Guid, int>();
  }

  public TechTypeInfo GetTypeRecByName(string typeName)
  {
    TechTypeInfo typeRecByName = (TechTypeInfo) null;
    foreach (TechTypeInfo techTypeInfo in this.Values)
    {
      if (techTypeInfo.Name == typeName)
      {
        typeRecByName = techTypeInfo;
        break;
      }
    }
    return typeRecByName;
  }

  public TechTypeInfo GetTypeRecByRecord(string typeCode)
  {
    TechTypeInfo typeRecByRecord = (TechTypeInfo) null;
    foreach (TechTypeInfo techTypeInfo in this.Values)
    {
      if (techTypeInfo.Type == typeCode)
      {
        typeRecByRecord = techTypeInfo;
        break;
      }
    }
    return typeRecByRecord;
  }

  public TechTypeInfo GetTypeRecByRecordId(int recordId)
  {
    TechTypeInfo typeRecByRecordId = (TechTypeInfo) null;
    if (recordId == 0)
      return (TechTypeInfo) null;
    foreach (TechTypeInfo techTypeInfo in this.Values)
    {
      if (techTypeInfo.RecordID == recordId)
      {
        typeRecByRecordId = techTypeInfo;
        break;
      }
    }
    return typeRecByRecordId;
  }

  public void AddType(TechTypeInfo typeInfo)
  {
    if (typeInfo == null || this.ContainsKey(typeInfo.RecordID))
      return;
    this.Add(typeInfo.RecordID, typeInfo);
  }

  public int GetObjTypeId(TechcardConsts.TpRecordType techType)
  {
    return this.GetObjTypeId((int) techType);
  }

  public Guid GetObjTypeGuid(TechcardConsts.TpRecordType techType)
  {
    return this.GetObjTypeGuid((int) techType);
  }

  public int GetObjTypeId(int recId) => this.ObjTypeGuid2Id(this.GetObjTypeGuid(recId));

  public Guid GetObjTypeGuid(int recId)
  {
    Guid objTypeGuid = Guid.Empty;
    TechTypeInfo techTypeInfo;
    this.TryGetValue(recId, out techTypeInfo);
    if (techTypeInfo == null || techTypeInfo.TypeSett == null)
      return objTypeGuid;
    switch (techTypeInfo.TypeSett.Mode)
    {
      case TechTypePumpMode.NewObjType:
      case TechTypePumpMode.ExistObjType:
      case TechTypePumpMode.LockedType:
        objTypeGuid = techTypeInfo.TypeSett.ObjType;
        break;
    }
    return objTypeGuid;
  }
}
