// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.TechParamEntityFactory
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechProcPump.Common;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;

public class TechParamEntityFactory
{
  private static readonly TechParamEntityFactory _instance = new TechParamEntityFactory();

  public ITechParamEntity CreateEntity(string code, object value, bool isFixed, string caption)
  {
    return isFixed || !string.IsNullOrEmpty(caption) ? (ITechParamEntity) new TechParamEntityFixed(code, value, isFixed, caption) : (ITechParamEntity) new TechParamEntity(code, value);
  }

  public static TechParamEntityFactory Instance => TechParamEntityFactory._instance;
}
