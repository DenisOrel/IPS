// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamListExtension
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

public static class TechParamListExtension
{
  public static ITechParamAttribute AddAttribute(
    this TechParamList paramList,
    IAttributeTypeItem attributeTypeItem,
    object value,
    EntitySetting.AttributeBelongs attributeBelongs)
  {
    return paramList.AddAttribute(attributeTypeItem, value, string.Empty, attributeBelongs);
  }

  public static ITechParamAttribute AddAttribute(
    this TechParamList paramList,
    IAttributeTypeItem attrTypeItem,
    object value,
    string caption = null,
    EntitySetting.AttributeBelongs attributeBelongs = EntitySetting.AttributeBelongs.ToObject)
  {
    ITechParamAttribute attribute = TechParamAttributeFactory.Instance.CreateAttribute(attrTypeItem, value, caption, attributeBelongs);
    paramList.Add((ITechParamBase) attribute);
    return attribute;
  }

  public static void AddEntity(
    this TechParamList paramList,
    string code,
    object value,
    bool isFixed = false,
    string caption = null)
  {
    paramList.Add((ITechParamBase) TechParamEntityFactory.Instance.CreateEntity(code, value, isFixed, caption));
  }

  public static void AddOrUpdateEntity(
    this TechParamList paramList,
    string code,
    object value,
    bool isFixed = false,
    string caption = null)
  {
    paramList.AddOrUpdate((ITechParamBase) TechParamEntityFactory.Instance.CreateEntity(code, value, isFixed, caption));
  }

  public static ITechParamEntity GetEntity(this TechParamList paramList, string code)
  {
    return paramList.FirstOrDefault<ITechParamBase>((Func<ITechParamBase, bool>) (item => item is ITechParamEntity && ((ITechParamEntity) item).Code.Equals(code))) as ITechParamEntity;
  }

  public static ITechParamAttribute GetAttribute(this TechParamList paramList, int attributeId)
  {
    foreach (ITechParamBase techParamBase in (List<ITechParamBase>) paramList)
    {
      if (techParamBase is ITechParamAttribute attribute && attribute.AttributeType != null && attribute.AttributeType.ID == attributeId)
        return attribute;
    }
    return (ITechParamAttribute) null;
  }

  public static object GetAttributeValue(this TechParamList paramList, int attributeId)
  {
    return paramList.GetAttribute(attributeId)?.Value;
  }

  public static object GetEntityValue(this TechParamList paramList, string entCode)
  {
    return paramList.GetEntity(entCode)?.Value;
  }
}
