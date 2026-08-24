// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseMeasureDefine
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal sealed class ImbaseMeasureDefine : MeasureDefine
{
  private IImportingData _cacheData;
  private IMeasures _measures;
  private IPhysicalValues _physicalValues;

  public ImbaseMeasureDefine(
    IImportingData cacheData,
    IMeasures measures,
    IPhysicalValues physicalValues)
  {
    this._cacheData = cacheData;
    this._measures = measures;
    this._physicalValues = physicalValues;
  }

  private Guid FindMeasureGuid(long measureID) => this._measures.GetMeasure(measureID).GUID;

  protected override Guid FindMeasureGuid(string unit)
  {
    long newKey = this._cacheData.GetNewKey(ImportingCategory.ImbaseBindedMeasures, (object) unit);
    return newKey != 0L ? this.FindMeasureGuid(newKey) : base.FindMeasureGuid(unit);
  }

  protected override Guid FindDefaultMeasureGuid(long physicalValueID)
  {
    IPhysicalValueItem physicalValue = this._physicalValues.GetPhysicalValue(physicalValueID);
    if (physicalValue != null)
    {
      long measureID = 0;
      if (physicalValue.DefaultMeasureID != 0L)
        measureID = physicalValue.DefaultMeasureID;
      else if (physicalValue.Measures != null && physicalValue.Measures.Count > 0)
      {
        // ISSUE: variable of a boxed type
        __Boxed<Dictionary<long, IMeasureItem>.Enumerator> enumerator = (ValueType) physicalValue.Measures.GetEnumerator();
        ((IEnumerator) enumerator).MoveNext();
        measureID = (long) ((IDictionaryEnumerator) enumerator).Key;
      }
      if (measureID != 0L)
        return this.FindMeasureGuid(measureID);
    }
    return base.FindDefaultMeasureGuid(physicalValueID);
  }
}
