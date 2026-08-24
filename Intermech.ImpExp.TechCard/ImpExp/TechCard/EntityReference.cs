// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.EntityReference
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace Intermech.ImpExp.TechCard;

[Serializable]
public class EntityReference
{
  private string _code = string.Empty;
  private int _production;
  private int _reference;
  private string _masterCode = string.Empty;
  private int _field;
  private int _root;
  private int _tag;
  public const string TableName = "TC_ENTITY_RF";
  private const string F_CODE = "F_CODE";
  private const string F_PRODUCTION = "F_PRODUCTION";
  private const string F_REF = "F_REFERENCE";
  private const string F_LINKCODE = "F_LINKCODE";
  private const string F_FIELD = "F_FIELD";
  private const string F_ROOT = "F_ROOT";
  private const string F_TAG = "F_TAG";
  private static int idx_F_CODE;
  private static int idx_F_PRODUCTION;
  private static int idx_F_REF;
  private static int idx_F_LINKCODE;
  private static int idx_F_FIELD;
  private static int idx_F_ROOT;
  private static int idx_F_TAG;

  public override string ToString() => $"{this.Code} -> ({this.MasterCode})";

  public string Code
  {
    [DebuggerStepThrough] get => this._code;
    set => this._code = value;
  }

  public int Production
  {
    [DebuggerStepThrough] get => this._production;
  }

  public int Reference
  {
    [DebuggerStepThrough] get => this._reference;
    set => this._reference = value;
  }

  public string MasterCode
  {
    [DebuggerStepThrough] get => this._masterCode;
    set => this._masterCode = value;
  }

  public int Field
  {
    [DebuggerStepThrough] get => this._field;
    set => this._field = value;
  }

  public int Root
  {
    [DebuggerStepThrough] get => this._root;
  }

  public int Tag
  {
    [DebuggerStepThrough] get => this._tag;
  }

  public override int GetHashCode() => this.Code.GetHashCode();

  public static void ParseSchema(Dictionary<string, int> schema)
  {
    EntityReference.idx_F_CODE = schema["F_CODE"];
    EntityReference.idx_F_PRODUCTION = schema["F_PRODUCTION"];
    EntityReference.idx_F_REF = schema["F_REFERENCE"];
    EntityReference.idx_F_LINKCODE = schema["F_LINKCODE"];
    EntityReference.idx_F_FIELD = schema["F_FIELD"];
    EntityReference.idx_F_ROOT = schema["F_ROOT"];
    EntityReference.idx_F_TAG = schema["F_TAG"];
  }

  public static EntityReference Parse(IDataReader idr)
  {
    return new EntityReference()
    {
      _code = idr.IsDBNull(EntityReference.idx_F_CODE) ? string.Empty : idr.GetString(EntityReference.idx_F_CODE),
      _production = idr.IsDBNull(EntityReference.idx_F_PRODUCTION) ? 0 : BasePumpHelper.ToInt32(idr[EntityReference.idx_F_PRODUCTION]),
      _reference = idr.IsDBNull(EntityReference.idx_F_REF) ? 0 : BasePumpHelper.ToInt32(idr[EntityReference.idx_F_REF]),
      _masterCode = idr.IsDBNull(EntityReference.idx_F_LINKCODE) ? string.Empty : idr.GetString(EntityReference.idx_F_LINKCODE),
      _field = idr.IsDBNull(EntityReference.idx_F_FIELD) ? 0 : BasePumpHelper.ToInt32(idr[EntityReference.idx_F_FIELD]),
      _root = idr.IsDBNull(EntityReference.idx_F_ROOT) ? 0 : BasePumpHelper.ToInt32(idr[EntityReference.idx_F_ROOT]),
      _tag = idr.IsDBNull(EntityReference.idx_F_TAG) ? 0 : BasePumpHelper.ToInt32(idr[EntityReference.idx_F_TAG])
    };
  }
}
