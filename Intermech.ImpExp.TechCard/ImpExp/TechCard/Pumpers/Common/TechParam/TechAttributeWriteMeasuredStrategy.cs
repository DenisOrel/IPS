// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.TechAttributeWriteMeasuredStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using Intermech.Interfaces;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;

[AttributeFieldType(new FieldTypes[] {FieldTypes.ftMeasured})]
internal class TechAttributeWriteMeasuredStrategy : TechAttributeWriteStrategy
{
  public override bool Write(
    PumpClass pumper,
    IImportedAttributeList importedList,
    ITechParamAttribute techAttribute,
    out string errorMessage)
  {
    errorMessage = string.Empty;
    if (techAttribute.Value == null)
      return true;
    if (techAttribute.Value is MeasuredValue measuredValue)
    {
      importedList.AddAttributeMeasure(techAttribute.AttributeType.ID, measuredValue.Value, measuredValue.MeasureID, measuredValue.Caption, techAttribute.Index);
      return true;
    }
    errorMessage = $"Ошибка преобразования \"{techAttribute.Value}\" в значение с плавающей точкой {TechAttributeWriteStrategy.GetParamInfo4ErrorMsg((ITechParamBase) techAttribute)}";
    return false;
  }
}
