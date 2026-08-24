// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Condition4Type.Condition4Production
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;
using Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Condition4Type;

public static class Condition4Production
{
  internal static TempFormula GetCondition(
    TempFormula baseFormula,
    Scenario scen,
    IImportingData masterImport)
  {
    IpsProductionObj ipsProductionObj;
    if (scen == null || TechPumpData.Production.Productions == null || !TechPumpData.Production.Productions.TryGetValue(scen.Property.Catalog.Production, out ipsProductionObj))
      return baseFormula;
    Guid productionAttrTypeGuid = TechcardConsts.TypeConsts.atProductionAttrTypeGuid;
    if (baseFormula == null)
      baseFormula = new TempFormula(true);
    else
      baseFormula.AddToken(new Token(Intermech.Expert.TokenType.BinaryOper, "И"));
    baseFormula.AddToken(new Token(Intermech.Expert.TokenType.OpeningBrace, "("));
    baseFormula.AddToken(ExpTokenConverter.CreateTokenAttribute(productionAttrTypeGuid, Guid.Empty, ref baseFormula));
    baseFormula.AddToken(new Token(Intermech.Expert.TokenType.BinaryOper, " = "));
    baseFormula.AddToken(new Token(Intermech.Expert.TokenType.Integer, ipsProductionObj.ObjID.ToString())
    {
      iValue = ipsProductionObj.ObjID
    });
    baseFormula.AddToken(new Token(Intermech.Expert.TokenType.ClosingBrace, ")"));
    return baseFormula;
  }
}
