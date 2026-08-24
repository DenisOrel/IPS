// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Condition4Type.Condition4Imbase
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;
using Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Condition4Type;

public static class Condition4Imbase
{
  internal static TempFormula GetCondition(TempFormula baseFormula, Scenario scenario)
  {
    if (scenario == null)
      return baseFormula;
    Guid zsearchObjectAttrGuid = TechcardConsts.TypeConsts.atZSearchObjectAttrGuid;
    try
    {
      if (baseFormula == null)
        baseFormula = new TempFormula(true);
      else
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.BinaryOper, "И"));
      baseFormula.AddToken(ExpTokenConverter.CreateFunctionToken(FormulaFunc.def));
      baseFormula.AddToken(new Token(Intermech.Expert.TokenType.OpeningBrace, "("));
      baseFormula.AddToken(ExpTokenConverter.CreateTokenAttribute(zsearchObjectAttrGuid, Guid.Empty, ref baseFormula));
      baseFormula.AddToken(new Token(Intermech.Expert.TokenType.ClosingBrace, ")"));
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Ошибка генерации условия сценария {scenario.key}: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return (TempFormula) null;
      throw;
    }
    return baseFormula;
  }
}
