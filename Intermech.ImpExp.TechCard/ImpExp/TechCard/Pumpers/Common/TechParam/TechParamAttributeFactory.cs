// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.TechParamAttributeFactory
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.TechProcPump.Common;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;

public class TechParamAttributeFactory
{
  private static readonly TechParamAttributeFactory _instance = new TechParamAttributeFactory();

  public ITechParamAttribute CreateAttribute(
    IAttributeTypeItem attributeTypeItem,
    object value,
    string caption = null,
    EntitySetting.AttributeBelongs attributeBelongs = EntitySetting.AttributeBelongs.ToObject)
  {
    return !string.IsNullOrEmpty(caption) ? (ITechParamAttribute) new TechParamAttributeCaption(attributeTypeItem, value, caption, attributeBelongs) : (ITechParamAttribute) new TechParamAttribute(attributeTypeItem, value, attributeBelongs);
  }

  public static TechParamAttributeFactory Instance => TechParamAttributeFactory._instance;
}
