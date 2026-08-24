// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.TechAttributeWriteIntegerStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;
using Intermech.ImpExp.TechCard.TechProcPump.Common;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;

[AttributeFieldType(new FieldTypes[] {FieldTypes.ftInteger})]
internal class TechAttributeWriteIntegerStrategy : TechAttributeWriteStrategy
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
      importedList.AddAttribute(techAttribute.AttributeType.ID, AttrValueType.integerVal, techAttribute.AttributeType.DefaultValue, techAttribute.Index);
      return true;
    }
    long intValue;
    if (DataConvertor.ConvertObjToInt(techAttribute.Value, out intValue))
    {
      importedList.AddAttribute(techAttribute.AttributeType.ID, AttrValueType.integerVal, (object) intValue, techAttribute.Index);
      return true;
    }
    errorMessage = $"Ошибка преобразования \"{techAttribute.Value}\" в целочисленное значение {TechAttributeWriteStrategy.GetParamInfo4ErrorMsg((ITechParamBase) techAttribute)}";
    return false;
  }
}
