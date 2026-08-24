// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechParam.TechAttributeWriteStrategy
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.TechProcPump.Common;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechParam;

internal abstract class TechAttributeWriteStrategy
{
  public abstract bool Write(
    PumpClass pumper,
    IImportedAttributeList importedList,
    ITechParamAttribute techAttribute,
    out string errorMessage);

  protected static string GetParamInfo4ErrorMsg(ITechParamBase techParam)
  {
    string paramInfo4ErrorMsg = string.Empty;
    switch (techParam)
    {
      case ITechParamEntity techParamEntity:
        paramInfo4ErrorMsg = $" для понятия \"{techParamEntity.Code}\"";
        break;
      case ITechParamAttribute techParamAttribute:
        paramInfo4ErrorMsg = $" для атрибута \"{techParamAttribute.AttributeType.Name}\" ID = \"{techParamAttribute.AttributeType.ID}\"";
        break;
    }
    return paramInfo4ErrorMsg;
  }
}
