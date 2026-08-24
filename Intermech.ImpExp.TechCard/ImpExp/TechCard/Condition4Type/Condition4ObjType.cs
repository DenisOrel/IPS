// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Condition4Type.Condition4ObjType
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;
using Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Condition4Type;

public class Condition4ObjType
{
  internal static TempFormula GetCondition(
    TempFormula baseFormula,
    Scenario scen,
    IImportingData masterImport)
  {
    if (scen == null)
      return baseFormula;
    Guid importingObjectType = ScenarioUtils.GetImportingObjectType(scen);
    if (importingObjectType == Guid.Empty)
      return baseFormula;
    IObjectTypeItem byGuid = TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(importingObjectType);
    int num = byGuid != null ? byGuid.ID : -1;
    if (num == -1)
      return baseFormula;
    Guid objectTypeAttrGuid = TechcardConsts.TypeConsts.atObjectTypeAttrGuid;
    if (baseFormula == null)
      baseFormula = new TempFormula(true);
    else
      baseFormula.AddToken(new Token(Intermech.Expert.TokenType.BinaryOper, "И"));
    try
    {
      baseFormula.AddToken(new Token(Intermech.Expert.TokenType.OpeningBrace, "("));
      baseFormula.AddToken(ExpTokenConverter.CreateTokenAttribute(objectTypeAttrGuid, Guid.Empty, ref baseFormula));
      baseFormula.AddToken(new Token(Intermech.Expert.TokenType.BinaryOper, " = "));
      baseFormula.AddToken(new Token(Intermech.Expert.TokenType.Integer, byGuid.Name)
      {
        iValue = (long) num
      });
      baseFormula.AddToken(new Token(Intermech.Expert.TokenType.ClosingBrace, ")"));
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Ошибка генерации условия сценария {scen.key}: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return (TempFormula) null;
      throw;
    }
    return baseFormula;
  }
}
