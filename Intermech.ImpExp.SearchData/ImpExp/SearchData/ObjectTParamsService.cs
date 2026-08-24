// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ObjectTParamsService
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal abstract class ObjectTParamsService
{
  private Hashtable _data;
  private IDbConnection _connection;
  private HashSet<int> _importedObjects;
  private SimpleLogger _logger;
  private CacheCategory _themeParams;
  private string _keyName;
  private int _packetSize = 1000;

  public ObjectTParamsService(
    IDbConnection connection,
    SimpleLogger logger,
    CacheCategory importedObjects,
    CacheCategory themeParams,
    string keyName)
  {
    this._importedObjects = new HashSet<int>();
    foreach (KeyValuePair<object, DictionaryValue> keyValuePair in importedObjects.Items)
      this._importedObjects.Add(Convert.ToInt32(keyValuePair.Key));
    this._data = new Hashtable();
    this._connection = connection;
    this._logger = logger;
    this._themeParams = themeParams;
    this._keyName = keyName;
  }

  protected abstract string sql4TableParams { get; }

  public void Read()
  {
    try
    {
      List<int> intList1 = new List<int>();
      using (IDataReader dataReader = BasePumpHelper.S4Query(this._connection, this.sql4TableParams))
      {
        while (dataReader.Read())
        {
          int int32 = BasePumpHelper.ToInt32(dataReader[0]);
          if (!this._importedObjects.Contains(int32))
          {
            List<TParamValue> tparamValueList;
            if (!this._data.ContainsKey((object) int32))
            {
              tparamValueList = new List<TParamValue>();
              this._data.Add((object) int32, (object) tparamValueList);
              intList1.Add(int32);
            }
            else
              tparamValueList = (List<TParamValue>) this._data[(object) int32];
            tparamValueList.Add(new TParamValue(BasePumpHelper.ToInt32(dataReader[1])));
          }
        }
      }
      this._importedObjects = (HashSet<int>) null;
      while (intList1.Count > 0)
      {
        int count = this._packetSize <= intList1.Count ? this._packetSize : intList1.Count;
        List<int> range = intList1.GetRange(0, count);
        List<Tuple<int, List<int>>> tupleList = new List<Tuple<int, List<int>>>();
        foreach (int key in range)
        {
          foreach (TParamValue tparamValue in (List<TParamValue>) this._data[(object) key])
          {
            TParamValue param = tparamValue;
            Tuple<int, List<int>> tuple = tupleList.Find((Predicate<Tuple<int, List<int>>>) (x => x.Item1.Equals(param.ParameterID)));
            if (tuple == null)
            {
              tuple = new Tuple<int, List<int>>(param.ParameterID, new List<int>());
              tupleList.Add(tuple);
            }
            tuple.Item2.Add(key);
          }
        }
        foreach (Tuple<int, List<int>> tuple in tupleList)
        {
          Tuple<int, List<int>> param4Objects = tuple;
          string tableForParameter = this.GetTableForParameter(this._themeParams, param4Objects.Item1);
          if (tableForParameter == null)
          {
            this._logger.Write($"Не указана таблица для тематического параметра {param4Objects.Item1}");
          }
          else
          {
            List<int> intList2 = param4Objects.Item2;
            string cmdtext;
            if (intList2.Count == 1)
            {
              cmdtext = string.Format("select t.{2}, t.p_value from {0} t where t.{2}={1}", (object) tableForParameter, (object) intList2[0], (object) this._keyName);
            }
            else
            {
              StringBuilder stringBuilder = new StringBuilder();
              foreach (int num in intList2)
              {
                if (stringBuilder.Length > 0)
                  stringBuilder.Append(',');
                stringBuilder.Append(num.ToString());
              }
              cmdtext = string.Format("select t.{2}, t.p_value from {0} t where t.{2} in({1})", (object) tableForParameter, (object) stringBuilder.ToString(), (object) this._keyName);
            }
            using (IDataReader dataReader = BasePumpHelper.S4Query(cmdtext))
            {
              while (dataReader.Read())
                ((List<TParamValue>) this._data[(object) BasePumpHelper.ToInt32(dataReader[0])]).Find((Predicate<TParamValue>) (x => x.ParameterID.Equals(param4Objects.Item1))).Value = dataReader[1];
            }
          }
        }
        intList1.RemoveRange(0, count);
      }
    }
    catch (Exception ex)
    {
      this._logger.Write(ex.Message);
    }
  }

  private string GetTableForParameter(CacheCategory themeParams, int parameterID)
  {
    DictionaryValue dictionaryValue = themeParams.GetValue((object) parameterID);
    if (dictionaryValue == null)
      return (string) null;
    return this.GetParamTableName(dictionaryValue.Caption.Split(','));
  }

  public List<TParamValue> GetParams(int id)
  {
    return this._data.ContainsKey((object) id) ? (List<TParamValue>) this._data[(object) id] : (List<TParamValue>) null;
  }

  public void ClearValues(int id) => this._data.Remove((object) id);

  protected abstract string GetParamTableName(string[] tabs);
}
