// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.CommonParamsReader
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal sealed class CommonParamsReader
{
  private readonly string _sql;
  private readonly CacheCategory _commonParameters;

  public CommonParamsReader(CacheCategory commonParameters, string tableName, string idField)
  {
    if (commonParameters.Items.Count <= 0)
      return;
    this._commonParameters = commonParameters;
    this._sql = $"select {string.Join(",", commonParameters.Items.Select<KeyValuePair<object, DictionaryValue>, string>((System.Func<KeyValuePair<object, DictionaryValue>, string>) (x => (string) x.Key)))} from {tableName} where {idField} = @p1";
  }

  public void Read(S4Table commonParamsData, int id)
  {
    if (this._commonParameters == null)
      return;
    using (IDataReader dataReader = BasePumpHelper.S4Query(this._sql, (object) id))
    {
      while (dataReader.Read())
      {
        foreach (KeyValuePair<object, DictionaryValue> keyValuePair in this._commonParameters.Items)
        {
          int ordinal = dataReader.GetOrdinal((string) keyValuePair.Key);
          if (ordinal >= 0 && !dataReader.IsDBNull(ordinal))
            commonParamsData.Add(Convert.ToString(keyValuePair.Value.NewObjectID), dataReader.GetValue(ordinal));
        }
      }
    }
  }
}
