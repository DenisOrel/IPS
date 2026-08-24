// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common.TechExpObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common;

[Serializable]
public class TechExpObject
{
  public readonly int Key;
  public readonly TechExpObjType Type;
  public readonly int Used;
  public readonly byte[] Body;
  public readonly int Size;
  public readonly DateTime Date;
  public readonly string Name;
  public readonly int Flags;
  public readonly int Production;
  private static int _idxFldKey;
  private static int _idxFldType;
  private static int _idxFldUsed;
  private static int _idxFldBody;
  private static int _idxFldSize;
  private static int _idxFldDate;
  private static int _idxFldName;
  private static int _idxFldFlags;
  private static int _idxFldProduction;

  public TechExpObject(IDataReader dataReader)
  {
    this.Key = dataReader.IsDBNull(TechExpObject._idxFldKey) ? 0 : BasePumpHelper.ToInt32(dataReader[TechExpObject._idxFldKey]);
    switch (dataReader.IsDBNull(TechExpObject._idxFldType) ? ' ' : dataReader.GetString(TechExpObject._idxFldType)[0])
    {
      case 'A':
        this.Type = TechExpObjType.AutoSelection;
        break;
      case 'F':
        this.Type = TechExpObjType.Formula;
        break;
      case 'T':
        this.Type = TechExpObjType.Table;
        break;
    }
    this.Used = dataReader.IsDBNull(TechExpObject._idxFldUsed) ? 0 : BasePumpHelper.ToInt32(dataReader[TechExpObject._idxFldUsed]);
    this.Size = dataReader.IsDBNull(TechExpObject._idxFldSize) ? 0 : BasePumpHelper.ToInt32(dataReader[TechExpObject._idxFldSize]);
    this.Date = dataReader.IsDBNull(TechExpObject._idxFldDate) ? DateTime.Now : dataReader.GetDateTime(TechExpObject._idxFldDate);
    this.Name = dataReader.IsDBNull(TechExpObject._idxFldName) ? string.Empty : dataReader.GetString(TechExpObject._idxFldName);
    this.Flags = dataReader.IsDBNull(TechExpObject._idxFldFlags) ? 0 : BasePumpHelper.ToInt32(dataReader[TechExpObject._idxFldFlags]);
    this.Production = dataReader.IsDBNull(TechExpObject._idxFldProduction) ? 0 : BasePumpHelper.ToInt32(dataReader[TechExpObject._idxFldProduction]);
    this.Body = new byte[this.Size];
    dataReader.GetBytes(TechExpObject._idxFldBody, 0L, this.Body, 0, this.Size);
  }

  public static void ParseSchema(Dictionary<string, int> schema)
  {
    TechExpObject._idxFldKey = schema["F_KEY"];
    TechExpObject._idxFldType = schema["F_TYPE"];
    TechExpObject._idxFldUsed = schema["F_USED"];
    TechExpObject._idxFldBody = schema["F_BODY"];
    TechExpObject._idxFldSize = schema["F_SIZE"];
    TechExpObject._idxFldDate = schema["F_DATE"];
    TechExpObject._idxFldName = schema["F_NAME"];
    TechExpObject._idxFldFlags = schema["F_FLAGS"];
    TechExpObject._idxFldProduction = schema["F_PRODUCTION"];
  }
}
