// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.TechDataBuilder`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.TechCardSettings;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump;

internal abstract class TechDataBuilder<T> : ITechDataBuilder where T : PumpClass
{
  public const string NoDataCondition = " 1 = 2 ";
  protected readonly T _pumper;
  public Func<string, string, string> PumpModeCondFunc;

  protected virtual string GetPumpModeCond(string condField, string dopType)
  {
    if (TechSettingsHelper.PumpMode == TechPumpMode.tpmAll)
      return string.Empty;
    if (this.PumpModeCondFunc != null)
      return this.PumpModeCondFunc(condField, dopType);
    throw new Exception("Not implemented.");
  }

  protected TechDataBuilder(T pumper)
  {
    this._pumper = (object) pumper != null ? pumper : throw new ArgumentNullException(nameof (pumper));
  }

  public abstract TechDataReaderInfo CreateDataReader(string dopType);

  public static string GetPumpModeCond(string condField, int recTypeId)
  {
    switch (recTypeId)
    {
      case -2:
      case -1:
        if (!TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.TechProc))
          return " 1 = 2 ";
        break;
      case 2:
        if (!TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.Route))
          return " 1 = 2 ";
        break;
      case 3:
        if (!TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.Zagot))
          return " 1 = 2 ";
        break;
      case 4:
        if (!TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.MatGroup))
          return " 1 = 2 ";
        break;
      case 107:
        if (!TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.Route))
          return " 1 = 2 ";
        break;
      case 122:
        if (!TechSettingsHelper.PumpDataType.HasFlag((Enum) TechPumpDataType.Route))
          return " 1 = 2 ";
        break;
    }
    if (TechSettingsHelper.PumpMode == TechPumpMode.tpmAll || condField == string.Empty)
      return string.Empty;
    return $" ( {condField}  IN (        SELECT DISTINCT        F_OBJ_KEY       FROM        TP_PUMP_DATA       WHERE         F_OBJ_TYPE = {(object) recTypeId}     )  ) ";
  }
}
