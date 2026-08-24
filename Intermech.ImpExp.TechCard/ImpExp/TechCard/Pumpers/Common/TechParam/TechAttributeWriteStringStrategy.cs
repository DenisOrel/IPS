// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.TechAttributeWriteStringStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;

[AttributeFieldType(new FieldTypes[] {FieldTypes.ftString})]
internal class TechAttributeWriteStringStrategy : TechAttributeWriteStrategy
{
  public override bool Write(
    PumpClass pumper,
    IImportedAttributeList importedList,
    ITechParamAttribute techAttribute,
    out string errorMessage)
  {
    errorMessage = string.Empty;
    string attrVal = Convert.ToString(techAttribute.Value);
    int length = techAttribute.AttributeType.MaxSize > 0 ? techAttribute.AttributeType.MaxSize : Consts.MaxStringSize;
    if (length < attrVal.Length)
    {
      errorMessage = $"Значение атрибута '{techAttribute.AttributeType.Name}' ID={techAttribute.AttributeType.ID} '{attrVal}' было урезано до {length} символа(ов)";
      attrVal = attrVal.Substring(0, length);
    }
    importedList.AddAttribute(techAttribute.AttributeType.ID, AttrValueType.stringVal, (object) attrVal, techAttribute.Index);
    return string.IsNullOrEmpty(errorMessage);
  }
}
