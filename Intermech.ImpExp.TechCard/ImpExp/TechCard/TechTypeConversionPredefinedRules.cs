// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechTypeConversionPredefinedRules
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechTypes.Settings;
using Intermech.Interfaces.TechCard;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard;

internal class TechTypeConversionPredefinedRules
{
  public static Dictionary<int, TechTypeConvertionRule> ObjectTypeImportRules = new Dictionary<int, TechTypeConvertionRule>();

  public static void InitializeSettings()
  {
    TechTypeConversionPredefinedRules.ObjectTypeImportRules = TechTypeConversionPredefinedRules.LoadObjectTypeImportRules();
  }

  private static Dictionary<int, TechTypeConvertionRule> LoadObjectTypeImportRules()
  {
    Dictionary<int, TechTypeConvertionRule> dictionary = new Dictionary<int, TechTypeConvertionRule>();
    int num1 = 1;
    TechTypeConvertionRule typeConvertionRule1 = new TechTypeConvertionRule(num1, TechcardConsts.TypeConsts.otOperationObjTypeGuid);
    dictionary.Add(num1, typeConvertionRule1);
    int num2 = 2;
    TechTypeConvertionRule typeConvertionRule2 = new TechTypeConvertionRule(num2, TechcardConsts.TypeConsts.otInstrumentationModelObjTypeGuid);
    dictionary.Add(num2, typeConvertionRule2);
    int num3 = 3;
    TechTypeConvertionRule typeConvertionRule3 = new TechTypeConvertionRule(num3, TechcardConsts.TypeConsts.otRouteObjTypeGuid);
    dictionary.Add(num3, typeConvertionRule3);
    int num4 = 4;
    TechTypeConvertionRule typeConvertionRule4 = new TechTypeConvertionRule(num4, TechcardConsts.TypeConsts.otTechDocumentsObjTypeGuid);
    dictionary.Add(num4, typeConvertionRule4);
    int num5 = 5;
    TechTypeConvertionRule typeConvertionRule5 = new TechTypeConvertionRule(num5, TechcardConsts.TypeConsts.otDopPrGUID);
    dictionary.Add(num5, typeConvertionRule5);
    int num6 = 6;
    TechTypeConvertionRule typeConvertionRule6 = new TechTypeConvertionRule(num6, Guid.Empty)
    {
      Mode = TechTypeConvertionRuleMode.Hide | TechTypeConvertionRuleMode.ReadOnly
    };
    dictionary.Add(num6, typeConvertionRule6);
    int num7 = 7;
    TechTypeConvertionRule typeConvertionRule7 = new TechTypeConvertionRule(num7, TechcardConsts.TypeConsts.otRoutesTemplatesObjTypeGuid);
    dictionary.Add(num7, typeConvertionRule7);
    int num8 = 8;
    TechTypeConvertionRule typeConvertionRule8 = new TechTypeConvertionRule(num8, TechcardConsts.TypeConsts.otTechTPBaseObjTypeGuid)
    {
      Mode = TechTypeConvertionRuleMode.ReadOnly
    };
    dictionary.Add(num8, typeConvertionRule8);
    int num9 = 9;
    TechTypeConvertionRule typeConvertionRule9 = new TechTypeConvertionRule(num9, TechcardConsts.TypeConsts.otPersonalObjTypeGuid);
    dictionary.Add(num9, typeConvertionRule9);
    int num10 = 10;
    TechTypeConvertionRule typeConvertionRule10 = new TechTypeConvertionRule(num10, TechcardConsts.TypeConsts.otCommentsObjTypeGuid);
    dictionary.Add(num10, typeConvertionRule10);
    int num11 = 11;
    TechTypeConvertionRule typeConvertionRule11 = new TechTypeConvertionRule(num11, Guid.Empty)
    {
      Mode = TechTypeConvertionRuleMode.Hide | TechTypeConvertionRuleMode.ReadOnly
    };
    dictionary.Add(num11, typeConvertionRule11);
    int num12 = 12;
    TechTypeConvertionRule typeConvertionRule12 = new TechTypeConvertionRule(num12, TechcardConsts.TypeConsts.otMaterialsObjTypeGuid);
    dictionary.Add(num12, typeConvertionRule12);
    int num13 = 13;
    TechTypeConvertionRule typeConvertionRule13 = new TechTypeConvertionRule(num13, TechcardConsts.TypeConsts.otEdSostArt)
    {
      Mode = TechTypeConvertionRuleMode.ReadOnly
    };
    dictionary.Add(num13, typeConvertionRule13);
    int num14 = -2;
    TechTypeConvertionRule typeConvertionRule14 = new TechTypeConvertionRule(num14, TechcardConsts.TypeConsts.otInstrumentationObjTypeGuid)
    {
      Mode = TechTypeConvertionRuleMode.Hide
    };
    dictionary.Add(num14, typeConvertionRule14);
    int num15 = 14;
    TechTypeConvertionRule typeConvertionRule15 = new TechTypeConvertionRule(num15, TechcardConsts.TypeConsts.otTechPerehodObjTypeGuid);
    dictionary.Add(num15, typeConvertionRule15);
    int num16 = 15;
    TechTypeConvertionRule typeConvertionRule16 = new TechTypeConvertionRule(num16, TechcardConsts.TypeConsts.otTechTPBaseObjTypeGuid)
    {
      Mode = TechTypeConvertionRuleMode.ReadOnly
    };
    dictionary.Add(num16, typeConvertionRule16);
    int num17 = 16 /*0x10*/;
    TechTypeConvertionRule typeConvertionRule17 = new TechTypeConvertionRule(num17, TechcardConsts.TypeConsts.otConditionsObjTypeGuid);
    dictionary.Add(num17, typeConvertionRule17);
    int num18 = 18;
    TechTypeConvertionRule typeConvertionRule18 = new TechTypeConvertionRule(num18, TechcardConsts.TypeConsts.otRiggingObjTypeGuid);
    dictionary.Add(num18, typeConvertionRule18);
    int num19 = 20;
    TechTypeConvertionRule typeConvertionRule19 = new TechTypeConvertionRule(num19, TechcardConsts.TypeConsts.otTPModificationObjTypeGuid);
    dictionary.Add(num19, typeConvertionRule19);
    int num20 = 19;
    TechTypeConvertionRule typeConvertionRule20 = new TechTypeConvertionRule(num20, Guid.Empty)
    {
      Mode = TechTypeConvertionRuleMode.Hide | TechTypeConvertionRuleMode.ReadOnly
    };
    dictionary.Add(num20, typeConvertionRule20);
    int num21 = 21;
    TechTypeConvertionRule typeConvertionRule21 = new TechTypeConvertionRule(num21, TechcardConsts.TypeConsts.otTechTPTypeObjTypeGuid);
    dictionary.Add(num21, typeConvertionRule21);
    int num22 = 22;
    TechTypeConvertionRule typeConvertionRule22 = new TechTypeConvertionRule(num22, TechcardConsts.TypeConsts.otTechRouteElemObjTypeGuid);
    dictionary.Add(num22, typeConvertionRule22);
    int num23 = 23;
    TechTypeConvertionRule typeConvertionRule23 = new TechTypeConvertionRule(num23, TechcardConsts.TypeConsts.otZagotGUID);
    dictionary.Add(num23, typeConvertionRule23);
    int num24 = 24;
    TechTypeConvertionRule typeConvertionRule24 = new TechTypeConvertionRule(num24, TechCardConsts.ObjectTypes.MaterialGroupGUID);
    dictionary.Add(num24, typeConvertionRule24);
    int num25 = 25;
    TechTypeConvertionRule typeConvertionRule25 = new TechTypeConvertionRule(num25, Guid.Empty)
    {
      Mode = TechTypeConvertionRuleMode.Hide | TechTypeConvertionRuleMode.ReadOnly
    };
    dictionary.Add(num25, typeConvertionRule25);
    int num26 = -3;
    TechTypeConvertionRule typeConvertionRule26 = new TechTypeConvertionRule(num26, TechcardConsts.TypeConsts.otScenarioObjTypeGuid)
    {
      Mode = TechTypeConvertionRuleMode.ReadOnly
    };
    dictionary.Add(num26, typeConvertionRule26);
    int num27 = 36;
    TechTypeConvertionRule typeConvertionRule27 = new TechTypeConvertionRule(num27, TechcardConsts.TypeConsts.otDraftGuid);
    dictionary.Add(num27, typeConvertionRule27);
    return dictionary;
  }
}
