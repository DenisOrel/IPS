// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Condition4Type.Condition4Zag
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.ScenarioPump;
using Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.Condition4Type;

public class Condition4Zag
{
  internal static TempFormula GetCondition(
    TempFormula baseFormula,
    Scenario scen,
    IImportingData masterImport)
  {
    try
    {
      if (scen == null)
        return baseFormula;
      Guid partObjectAttrGuid = TechcardConsts.TypeConsts.atPartObjectAttrGuid;
      Guid vidZagAttrTypeGuid = TechcardConsts.TypeConsts.atVidZagAttrTypeGuid;
      int vidZag = scen.Property.VidZag;
      int vidDet = scen.Property.VidDet;
      long newKey1 = ImportingDataHelper.Instance.GetNewKey(masterImport, ImportingCategory.TechVidZagPump, (object) vidZag);
      long newKey2 = ImportingDataHelper.Instance.GetNewKey(masterImport, ImportingCategory.TechVidIzdPump, (object) vidDet);
      if (newKey1 == 0L && newKey2 == 0L)
        return (TempFormula) null;
      if (baseFormula == null)
        baseFormula = new TempFormula(true);
      else
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.BinaryOper, "И"));
      if (newKey1 != 0L)
      {
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.OpeningBrace, "("));
        baseFormula.AddToken(ExpTokenConverter.CreateTokenAttribute(vidZagAttrTypeGuid, Guid.Empty, ref baseFormula));
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.BinaryOper, " = "));
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.Integer, newKey1.ToString())
        {
          iValue = newKey1
        });
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.ClosingBrace, ")"));
      }
      if (newKey2 != 0L && newKey1 != 0L)
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.BinaryOper, "И"));
      if (newKey2 != 0L)
      {
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.OpeningBrace, "("));
        baseFormula.AddToken(ExpTokenConverter.CreateTokenAttribute(partObjectAttrGuid, Guid.Empty, ref baseFormula));
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.BinaryOper, " = "));
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.Integer, newKey2.ToString())
        {
          iValue = newKey2
        });
        baseFormula.AddToken(new Token(Intermech.Expert.TokenType.ClosingBrace, ")"));
      }
      return baseFormula;
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Ошибка генерации условия сценария {scen.key}: {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return (TempFormula) null;
      throw;
    }
  }
}
