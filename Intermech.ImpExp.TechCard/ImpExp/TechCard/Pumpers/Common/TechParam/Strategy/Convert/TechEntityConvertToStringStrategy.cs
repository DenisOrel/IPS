// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert.TechEntityConvertToStringStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;
using Intermech.ImpExp.TechCard.TechProcPump.Common;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.Strategy.Convert;

[AttributeFieldType(new FieldTypes[] {FieldTypes.ftString, FieldTypes.ftMemo})]
internal class TechEntityConvertToStringStrategy : TechEntityConvertStrategy
{
  public override bool Convert(
    PumpClass pumper,
    TechObjectRecordBase record,
    TechParamList recordParamList,
    ITechParamEntity techEntity,
    Entity entitySettings,
    out ITechParamAttribute techAttribute,
    out string errorMessage)
  {
    if (entitySettings == null)
      throw new ArgumentNullException(nameof (entitySettings));
    errorMessage = (string) null;
    techAttribute = (ITechParamAttribute) null;
    if (techEntity == null)
      return false;
    string str1 = System.Convert.ToString(techEntity.Value);
    string strValue;
    if (DataConvertor.ConvertRtfToStr(str1, out strValue))
      str1 = strValue;
    else if (entitySettings.Type == "K")
    {
      List<string> list = ((IEnumerable<string>) str1.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None)).ToList<string>();
      if (list.Count > 0)
      {
        string str2 = list[0];
        bool flag = false;
        if (str2.Length == 1)
        {
          list.RemoveAt(0);
          flag = true;
        }
        if (str2 == "1")
        {
          list.RemoveAt(0);
          flag = true;
        }
        if (flag)
          str1 = string.Join(Environment.NewLine, (IEnumerable<string>) list);
      }
    }
    if (!string.IsNullOrEmpty(str1))
    {
      string a = str1.Replace("«", "<<").Replace("»", ">>");
      if (!string.Equals(a, str1))
        str1 = a;
    }
    if (string.IsNullOrEmpty(str1))
      return false;
    techAttribute = (ITechParamAttribute) new TechParamAttribute(entitySettings.PumpToAttrType, (object) str1, entitySettings.Settings != null ? entitySettings.Settings.AttributeBelong : EntitySetting.AttributeBelongs.ToLinkAndObject);
    return true;
  }
}
