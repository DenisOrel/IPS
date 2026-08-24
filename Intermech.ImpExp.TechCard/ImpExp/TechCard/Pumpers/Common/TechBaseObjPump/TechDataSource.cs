// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.TechDataSource
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

public class TechDataSource : IDisposable
{
  private readonly ITechDataBuilder _dataBuilder;
  private readonly IDictionary<string, TechDataReaderInfo> _dataReaderCache = (IDictionary<string, TechDataReaderInfo>) new ConcurrentDictionary<string, TechDataReaderInfo>();

  public TechDataSource(ITechDataBuilder dataBuilder)
  {
    this._dataBuilder = dataBuilder ?? throw new ArgumentNullException(nameof (dataBuilder));
  }

  public TechDataReaderInfo GetDataReaderInfo(string dopType = "")
  {
    TechDataReaderInfo dataReaderInfo;
    if (this._dataReaderCache.TryGetValue(dopType, out dataReaderInfo))
      return dataReaderInfo;
    TechDataReaderInfo dataReader = this._dataBuilder.CreateDataReader(dopType);
    if (!this._dataReaderCache.ContainsKey(dopType))
      this._dataReaderCache[dopType] = dataReader;
    return dataReader;
  }

  public void Close()
  {
    foreach (TechDataReaderInfo techDataReaderInfo in (IEnumerable<TechDataReaderInfo>) this._dataReaderCache.Values)
      techDataReaderInfo?.Dispose();
    this._dataReaderCache.Clear();
  }

  public void Dispose() => this.Close();
}
