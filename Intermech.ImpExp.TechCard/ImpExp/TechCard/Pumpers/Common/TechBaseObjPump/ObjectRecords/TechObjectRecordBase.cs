// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords.TechObjectRecordBase
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Collections;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;

public class TechObjectRecordBase : IAssignable, IEquatable<TechObjectRecordBase>
{
  private const string FldFKey = "F_KEY";
  private static readonly IDictionary<string, int> IdxFieldIndexCache = (IDictionary<string, int>) new Dictionary<string, int>();
  protected readonly Dictionary<string, object> _fields;
  public int diff_ArtTcKey;
  public int baseKey;
  public int Key;
  public string TableName = string.Empty;
  public IDictionary<string, object> ExFields;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private string GetFieldKey(string fieldName) => $"{this.GetType()}_{fieldName}";

  protected virtual int GetFieldsCapacity() => 1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected int GetFieldIndex(string fieldName)
  {
    int fieldIndex;
    if (!TechObjectRecordBase.IdxFieldIndexCache.TryGetValue(this.GetFieldKey(fieldName), out fieldIndex))
      fieldIndex = -1;
    return fieldIndex;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  protected void SetFieldIndex(string fieldName, int fieldIndex)
  {
    TechObjectRecordBase.IdxFieldIndexCache[this.GetFieldKey(fieldName)] = fieldIndex;
  }

  protected int Idx_F_KEY
  {
    get => this.GetFieldIndex("F_KEY");
    set => this.SetFieldIndex("F_KEY", value);
  }

  public TechObjectRecordBase()
  {
    this._fields = new Dictionary<string, object>(this.GetFieldsCapacity());
  }

  public virtual void ParseSchema(IDictionary<string, int> schema)
  {
    this.Idx_F_KEY = schema["F_KEY"];
  }

  public virtual void Parse(IDataReader dataReader)
  {
    this.Key = dataReader.IsDBNull(this.Idx_F_KEY) ? 0 : BasePumpHelper.ToInt32(dataReader[this.Idx_F_KEY]);
    this.baseKey = this.Key;
  }

  public virtual IDictionary<string, object> Fields => (IDictionary<string, object>) this._fields;

  public virtual bool FieldExist(string fieldName) => this._fields.ContainsKey(fieldName);

  public virtual object GetFieldValue(string fieldName)
  {
    object fieldValue;
    this._fields.TryGetValue(fieldName, out fieldValue);
    return fieldValue;
  }

  public virtual void SetFieldValue(string fieldName, object fieldValue)
  {
    this._fields[fieldName] = fieldValue;
  }

  public virtual void AddFieldValue(string fieldName, object fieldValue)
  {
    this._fields.Add(fieldName, fieldValue);
  }

  public virtual void Clear()
  {
    this.Fields.Clear();
    this.diff_ArtTcKey = -1;
    this.baseKey = -1;
    this.Key = -1;
    this.TableName = string.Empty;
    this.ExFields = (IDictionary<string, object>) null;
  }

  public virtual void Assign(object source)
  {
    if (!(source is TechObjectRecordBase objectRecordBase))
      return;
    this.diff_ArtTcKey = objectRecordBase.diff_ArtTcKey;
    this.baseKey = objectRecordBase.baseKey;
    this.Key = objectRecordBase.Key;
    this.TableName = objectRecordBase.TableName;
    this.Fields.AddRange<KeyValuePair<string, object>>((IEnumerable<KeyValuePair<string, object>>) objectRecordBase.Fields);
    if (objectRecordBase.ExFields == null)
      return;
    if (this.ExFields == null)
      this.ExFields = (IDictionary<string, object>) new Dictionary<string, object>(objectRecordBase.ExFields);
    else
      this.ExFields.AddRange<KeyValuePair<string, object>>((IEnumerable<KeyValuePair<string, object>>) objectRecordBase.ExFields);
  }

  public virtual bool Equals(TechObjectRecordBase other)
  {
    if (other == null)
      return false;
    return this == other || this.Key == other.Key;
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    if (this == obj)
      return true;
    return obj.GetType() == this.GetType() && this.Equals((TechObjectRecordBase) obj);
  }

  public override int GetHashCode() => this.Key;
}
