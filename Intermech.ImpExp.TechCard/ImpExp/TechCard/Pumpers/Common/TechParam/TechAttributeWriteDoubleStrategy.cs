// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.TechAttributeWriteDoubleStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;

[AttributeFieldType(new FieldTypes[] {FieldTypes.ftDouble})]
internal class TechAttributeWriteDoubleStrategy : TechAttributeWriteStrategy
{
  public override bool Write(
    PumpClass pumper,
    IImportedAttributeList importedList,
    ITechParamAttribute techAttribute,
    out string errorMessage)
  {
    errorMessage = string.Empty;
    if (DataConvertor.IsEmptyValue(techAttribute.Value))
    {
      importedList.AddAttribute(techAttribute.AttributeType.ID, AttrValueType.doubleVal, techAttribute.AttributeType.DefaultValue, techAttribute.Index);
      return true;
    }
    double dblValue;
    if (DataConvertor.ConvertObjToDouble(techAttribute.Value, out dblValue))
    {
      double num = Math.Abs(dblValue);
      if (num > 0.0 && num < 1E-100 || num > 1E+100)
      {
        errorMessage = $"Значение \"{dblValue}\" вне допустимого диапазона {TechAttributeWriteStrategy.GetParamInfo4ErrorMsg((ITechParamBase) techAttribute)}";
        return false;
      }
      importedList.AddAttribute(techAttribute.AttributeType.ID, AttrValueType.doubleVal, (object) dblValue, techAttribute.Index);
      return true;
    }
    errorMessage = $"Ошибка преобразования \"{techAttribute.Value}\" в значение с плавающей запятой {TechAttributeWriteStrategy.GetParamInfo4ErrorMsg((ITechParamBase) techAttribute)}";
    return false;
  }
}
