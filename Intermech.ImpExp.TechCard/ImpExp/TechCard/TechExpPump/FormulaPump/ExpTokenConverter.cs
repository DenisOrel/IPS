// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.ExpTokenConverter
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump;
using Intermech.ImpExp.TechCard.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using Intermech.Interfaces.SelectionService;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

internal class ExpTokenConverter
{
  private bool _rangeConverted;
  private readonly TechExpBasePump _pumper;
  private string _entCode = string.Empty;
  private readonly Dictionary<TokenRecord, List<Token>> _tokenList;
  private readonly List<string> _entityIdList;

  private Entity GetEntityByIndex(int index)
  {
    if (this._entityIdList.Count < index)
      throw new TokenConvertException($"Неверный аргумент. Unknown entity index = {index}.");
    string entityId = this._entityIdList[index];
    return !entityId.Equals(string.Empty) ? TechExpert.TypeConverter.GetEntityByCode(entityId) : throw new TokenConvertException($"Неверный аргумент. Empty entity code for index = {index}.");
  }

  public ExpTokenConverter(TechExpBasePump pumper)
  {
    this._pumper = pumper;
    this._tokenList = new Dictionary<TokenRecord, List<Token>>();
    this._entityIdList = new List<string>();
  }

  internal static Token CreateFunctionToken(FormulaFunc ff)
  {
    int funcIndex = ExpertFunc.GetFuncIndex(ff);
    return funcIndex != -1 ? new Token(Intermech.Expert.TokenType.FuncCall, ExpertFunc.funcs(funcIndex).text + "(")
    {
      info = funcIndex
    } : throw new TokenConvertException($"Неизвестная функция экспертной системы. Name =\"{EnumTypeHelper.GetCaption((Enum) ff)}\" index = {ff}");
  }

  internal bool ConvertTokens(
    string entCode,
    FormulaList tcFormulaList,
    List<string> tcIdList,
    ref TempFormula ipsFormula)
  {
    this._entCode = entCode;
    this._tokenList.Clear();
    this._entityIdList.Clear();
    this._entityIdList.AddRange((IEnumerable<string>) tcIdList);
    foreach (TokenRecord tcFormula in (List<TokenRecord>) tcFormulaList)
    {
      if (!this.ConvertToken(tcFormula, ref ipsFormula))
        throw new TokenConvertException("Can't convert TokenRec to IPS Token");
    }
    foreach (List<Token> tokenList in this._tokenList.Values)
    {
      foreach (Token t in tokenList)
      {
        if (t != null)
          ipsFormula.AddToken(t);
      }
    }
    return true;
  }

  private bool CanChangeIntToken2ObjLink(Entity entity)
  {
    if (entity == null || entity.EntityReference == null || entity.EntityReference.MasterCode != entity.Code)
      return false;
    IAttributeTypeItem attributeItemByCode = TechExpert.TypeConverter.GetAttributeItemByCode(entity.Code, TechcardConsts.Plugin, out string _);
    return attributeItemByCode != null && attributeItemByCode.AttrValueType == 8;
  }

  private bool CanChangeStrToken2ObjLink(Entity entity)
  {
    if (entity == null || entity.EntityReference == null || entity.EntityReference.Field != -2)
      return false;
    IAttributeTypeItem attributeItemByCode = TechExpert.TypeConverter.GetAttributeItemByCode(entity.Code, TechcardConsts.Plugin, out string _);
    return attributeItemByCode != null && attributeItemByCode.AttrValueType == 8;
  }

  private bool ConvertToken(TokenRecord token, ref TempFormula ipsFormula)
  {
    if (token == null)
      throw new TokenConvertException("Неверный аргумент. Элемент формулы не определен.");
    List<Token> ipsTokens = new List<Token>();
    this._tokenList.Add(token, ipsTokens);
    if (ipsFormula.resType == DataType.ObjectLink)
    {
      Entity entityByCode = TechExpert.TypeConverter.GetEntityByCode(this._entCode);
      string message = $"Неверный тип элемента формулы. Can't add to object's link formula the token with ID = {token.Kind}";
      switch (token.Kind)
      {
        case 1:
          if (!this.CanChangeIntToken2ObjLink(entityByCode))
            throw new TokenConvertException(message);
          break;
        case 3:
          if (!this.CanChangeStrToken2ObjLink(entityByCode))
            throw new TokenConvertException(message);
          break;
        default:
          throw new TokenConvertException(message);
      }
    }
    switch (token.Kind)
    {
      case 1:
        Token ipsToken1 = new Token(Intermech.Expert.TokenType.Integer, token.SValue);
        int intValue1;
        if (!DataConvertor.ConvertStrToInt(token.SValue, out intValue1))
          throw new CommonDataTypeConvertException($"Невозможно конвертировать строковое значение  = \"{token.SValue}\" в целое число");
        ipsToken1.iValue = (long) intValue1;
        if (!this._rangeConverted)
        {
          this.CheckIntegerToken(ref ipsToken1, ipsTokens, ipsFormula);
        }
        else
        {
          List<TokenRecord> tokenRecordList = new List<TokenRecord>((IEnumerable<TokenRecord>) this._tokenList.Keys);
          TokenRecord tokenRecord1 = (TokenRecord) null;
          if (this._tokenList.Count > 2)
            tokenRecord1 = tokenRecordList[this._tokenList.Count - 3];
          int intValue2;
          if (tokenRecord1 != null && DataConvertor.ConvertStrToInt(tokenRecord1.SValue, out intValue2))
          {
            TokenRecord tokenRecord2 = (TokenRecord) null;
            for (int index = tokenRecordList.Count - 3; index >= 0; --index)
            {
              if (tokenRecordList[index] != null && tokenRecordList[index].Kind == (byte) 15)
              {
                tokenRecord2 = token;
                break;
              }
            }
            if (tokenRecord2 != null)
            {
              Entity entityByIndex = this.GetEntityByIndex((int) tokenRecord2.Index);
              if (entityByIndex != null)
              {
                for (int index = intValue2 + 1; index < intValue1; ++index)
                {
                  Token ipsToken2 = new Token(Intermech.Expert.TokenType.Integer, index.ToString())
                  {
                    iValue = (long) index
                  };
                  if (this.ChangeIntToken2ObjLink(entityByIndex, ipsToken2) && ipsToken2.iValue != 0L)
                  {
                    ipsTokens.Add(ipsToken2);
                    ipsTokens.Add(new Token(Intermech.Expert.TokenType.Divider, ", "));
                  }
                }
              }
              this.ChangeIntToken2ObjLink(entityByIndex, ipsToken1);
            }
          }
          this._rangeConverted = false;
        }
        ipsTokens.Add(ipsToken1);
        break;
      case 2:
        Token ipsToken3 = new Token(Intermech.Expert.TokenType.Float, token.SValue);
        double dblValue;
        if (!DataConvertor.ConvertStrToDouble(token.SValue, out dblValue))
          throw new CommonDataTypeConvertException($"Невозможно конвертировать строковое значение = \"{token.SValue}\" в вещественное число");
        ipsToken3.fValue = dblValue;
        this.CheckDoubleToken(ref ipsToken3, ipsTokens, ipsFormula);
        ipsTokens.Add(ipsToken3);
        break;
      case 3:
        Token ipsToken4 = new Token(Intermech.Expert.TokenType.String, token.SValue);
        ipsTokens.Add(ipsToken4);
        this.CheckStringToken(ref ipsToken4, ipsTokens, ipsFormula);
        break;
      case 4:
        Token token1 = new Token(Intermech.Expert.TokenType.Boolean, token.SValue)
        {
          iValue = token.SValue == "ДА" ? 1L : 0L
        };
        ipsTokens.Add(token1);
        break;
      case 15:
        Entity entityByIndex1 = this.GetEntityByIndex((int) token.Index);
        Guid attrGuid1;
        if (!TechPumpData.Entities.Code2AttributeGuid.TryGetValue(entityByIndex1.Code, out attrGuid1))
          throw new TokenConvertException($"Неверный аргумент. Не найден атрибут для понятия ({entityByIndex1.Code}) ( понятие отсутствует в настройках ).");
        if (attrGuid1.Equals(Guid.Empty))
          throw new TokenConvertException($"Неверный аргумент. Атрибут для понятия ({entityByIndex1.Code}) не определен.");
        Guid objTypeGuid1;
        if (entityByIndex1.Settings.ObjectType != Guid.Empty)
        {
          objTypeGuid1 = entityByIndex1.Settings.ObjectType;
        }
        else
        {
          TechTypeSett typeSettByEntity = TechExpert.TypeConverter.GetObjectTypeSettByEntity(entityByIndex1);
          objTypeGuid1 = typeSettByEntity != null ? typeSettByEntity.ObjType : Guid.Empty;
        }
        Token tokenAttribute = ExpTokenConverter.CreateTokenAttribute(attrGuid1, objTypeGuid1, ref ipsFormula);
        if (tokenAttribute == null)
          throw new TokenConvertException($"Ошибка создания элемента формулы. Понятие = {entityByIndex1.Code}.");
        IAttributeTypeItem attributeItemByCode1 = TechExpert.TypeConverter.GetAttributeItemByCode(entityByIndex1.Code, TechcardConsts.Plugin, out string _);
        if (attributeItemByCode1 != null)
          this.CheckEntityToken(entityByIndex1, (FieldTypes) attributeItemByCode1.AttrValueType, ref tokenAttribute, ipsFormula);
        ipsTokens.Add(tokenAttribute);
        break;
      case 20:
        Token functionToken1 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.sin);
        ipsTokens.Add(functionToken1);
        break;
      case 21:
        Token functionToken2 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.cos);
        ipsTokens.Add(functionToken2);
        break;
      case 22:
        Token functionToken3 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.tg);
        ipsTokens.Add(functionToken3);
        break;
      case 23:
        Token functionToken4 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.atg);
        ipsTokens.Add(functionToken4);
        break;
      case 26:
        Token functionToken5 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.exp);
        ipsTokens.Add(functionToken5);
        break;
      case 27:
        Token functionToken6 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.ln);
        ipsTokens.Add(functionToken6);
        break;
      case 28:
        Token functionToken7 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.lg);
        ipsTokens.Add(functionToken7);
        break;
      case 29:
        Token functionToken8 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.Int);
        ipsTokens.Add(functionToken8);
        break;
      case 30:
        Token functionToken9 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.frac);
        ipsTokens.Add(functionToken9);
        break;
      case 31 /*0x1F*/:
        Token functionToken10 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.abs);
        ipsTokens.Add(functionToken10);
        break;
      case 32 /*0x20*/:
        Token functionToken11 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.sqrt);
        ipsTokens.Add(functionToken11);
        break;
      case 33:
        Token functionToken12 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.nom);
        ipsTokens.Add(functionToken12);
        break;
      case 34:
        Token functionToken13 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.hi);
        ipsTokens.Add(functionToken13);
        break;
      case 35:
        Token functionToken14 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.low);
        ipsTokens.Add(functionToken14);
        break;
      case 36:
        Token functionToken15 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.kv);
        ipsTokens.Add(functionToken15);
        break;
      case 37:
        Token functionToken16 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.kt);
        ipsTokens.Add(functionToken16);
        break;
      case 38:
        Token functionToken17 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.st);
        ipsTokens.Add(functionToken17);
        break;
      case 39:
        if (ipsFormula.resType == DataType.Measured)
        {
          Token functionToken18 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnd_m);
          ipsTokens.Add(functionToken18);
          Token functionToken19 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnde_m);
          ipsTokens.Add(functionToken19);
          break;
        }
        Token functionToken20 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnd);
        ipsTokens.Add(functionToken20);
        Token functionToken21 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnde);
        ipsTokens.Add(functionToken21);
        break;
      case 40:
        if (ipsFormula.resType == DataType.Measured)
        {
          Token functionToken22 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnd_m);
          ipsTokens.Add(functionToken22);
          break;
        }
        Token functionToken23 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnd);
        ipsTokens.Add(functionToken23);
        break;
      case 41:
        if (ipsFormula.resType == DataType.Measured)
        {
          Token functionToken24 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnde_m);
          ipsTokens.Add(functionToken24);
          break;
        }
        Token functionToken25 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnde);
        ipsTokens.Add(functionToken25);
        break;
      case 42:
        if (ipsFormula.resType == DataType.Measured)
        {
          Token functionToken26 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnde_m);
          ipsTokens.Add(functionToken26);
          break;
        }
        Token functionToken27 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnde);
        ipsTokens.Add(functionToken27);
        break;
      case 43:
        Token functionToken28 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.ra);
        ipsTokens.Add(functionToken28);
        Entity entityByCode1 = TechExpert.TypeConverter.GetEntityByCode(this._entCode);
        Guid attrGuid2;
        if (!TechPumpData.Entities.Code2AttributeGuid.TryGetValue(this._entCode, out attrGuid2))
          throw new TokenConvertException($"Неверный аргумент. Не найден атрибут для понятия ({entityByCode1.Code}) ( понятие отсутствует в настройках ).");
        if (attrGuid2.Equals(Guid.Empty))
          throw new TokenConvertException($"Неверный аргумент. Атрибут для понятия ({entityByCode1.Code}) не определен.");
        Guid objTypeGuid2;
        if (entityByCode1.Settings.ObjectType != Guid.Empty)
        {
          objTypeGuid2 = entityByCode1.Settings.ObjectType;
        }
        else
        {
          TechTypeSett typeSettByEntity = TechExpert.TypeConverter.GetObjectTypeSettByEntity(entityByCode1);
          objTypeGuid2 = typeSettByEntity != null ? typeSettByEntity.ObjType : Guid.Empty;
        }
        ipsTokens.Add(ExpTokenConverter.CreateTokenAttribute(attrGuid2, objTypeGuid2, ref ipsFormula) ?? throw new TokenConvertException($"Ошибка создания элемента формулы. Понятие = {entityByCode1.Code}."));
        Token token2 = new Token(Intermech.Expert.TokenType.Divider, ",");
        ipsTokens.Add(token2);
        break;
      case 45:
        Token functionToken29 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.ctn);
        ipsTokens.Add(functionToken29);
        break;
      case 46:
        Token functionToken30 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.def);
        ipsTokens.Add(functionToken30);
        break;
      case 47:
        if (ipsFormula.resType == DataType.Measured)
        {
          Token functionToken31 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnde_m);
          ipsTokens.Add(functionToken31);
          break;
        }
        Token functionToken32 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rnde);
        ipsTokens.Add(functionToken32);
        break;
      case 48 /*0x30*/:
        if (ipsFormula.resType == DataType.Measured)
        {
          Token functionToken33 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rndg_m);
          ipsTokens.Add(functionToken33);
          break;
        }
        Token functionToken34 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rndg);
        ipsTokens.Add(functionToken34);
        break;
      case 49:
        if (ipsFormula.resType == DataType.Measured)
        {
          Token functionToken35 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rndg_m);
          ipsTokens.Add(functionToken35);
          break;
        }
        Token functionToken36 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rndg);
        ipsTokens.Add(functionToken36);
        break;
      case 50:
        if (ipsFormula.resType == DataType.Measured)
        {
          Token functionToken37 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rndg_m);
          ipsTokens.Add(functionToken37);
          break;
        }
        Token functionToken38 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rndg);
        ipsTokens.Add(functionToken38);
        break;
      case 51:
        if (ipsFormula.resType == DataType.Measured)
        {
          Token functionToken39 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rndg_m);
          ipsTokens.Add(functionToken39);
          break;
        }
        Token functionToken40 = ExpTokenConverter.CreateFunctionToken(FormulaFunc.rndg);
        ipsTokens.Add(functionToken40);
        break;
      case 52:
      case 53:
      case 54:
      case 55:
        string str = TechExpert.Tokens.Token2String(token.Kind);
        throw new TokenConvertException($"Неверный аргумент. Ошибка конвертации TokenRec = {token.Kind} ( функция \"{str}\" не поддерживает автоматическую конвертацию )");
      case 65:
        bool flag = true;
        Token token3 = new Token(Intermech.Expert.TokenType.OpeningBrace, "(");
        if (this._tokenList.Count > 1)
        {
          List<TokenRecord> tokenRecordList = new List<TokenRecord>((IEnumerable<TokenRecord>) this._tokenList.Keys);
          TokenRecord tokenRecord = tokenRecordList[tokenRecordList.Count - 2];
          if (tokenRecord != null)
          {
            if (tokenRecord.Kind == (byte) 116)
              token3.text = "{";
            else if (tokenRecord.Kind >= (byte) 20 && tokenRecord.Kind <= (byte) 64 /*0x40*/)
              flag = false;
          }
        }
        if (flag)
        {
          ipsTokens.Add(token3);
          break;
        }
        break;
      case 70:
        Token ipsToken5 = new Token(Intermech.Expert.TokenType.ClosingBrace, ")");
        this.CheckClosingBracket(ref ipsToken5, ipsTokens);
        ipsTokens.Add(ipsToken5);
        break;
      case 75:
        Token token4 = new Token(Intermech.Expert.TokenType.BinaryOper, "+");
        ipsTokens.Add(token4);
        break;
      case 85:
        Token token5 = new Token(Intermech.Expert.TokenType.BinaryOper, "+");
        ipsTokens.Add(token5);
        break;
      case 86:
        Token token6 = new Token(Intermech.Expert.TokenType.BinaryOper, "-");
        ipsTokens.Add(token6);
        break;
      case 87:
        Token token7 = new Token(Intermech.Expert.TokenType.BinaryOper, "*");
        ipsTokens.Add(token7);
        break;
      case 88:
        Token token8 = new Token(Intermech.Expert.TokenType.BinaryOper, "/");
        ipsTokens.Add(token8);
        break;
      case 89:
        Token token9 = new Token(Intermech.Expert.TokenType.BinaryOper, "^");
        ipsTokens.Add(token9);
        break;
      case 90:
        Token token10 = new Token(Intermech.Expert.TokenType.Divider, ", ");
        ipsTokens.Add(token10);
        break;
      case 91:
        this._rangeConverted = false;
        if (this._tokenList.Count > 1)
        {
          List<TokenRecord> tokenRecordList = new List<TokenRecord>((IEnumerable<TokenRecord>) this._tokenList.Keys);
          TokenRecord tokenRecord = (TokenRecord) null;
          for (int index = tokenRecordList.Count - 2; index >= 0; --index)
          {
            if (tokenRecordList[index] != null && tokenRecordList[index].Kind == (byte) 15)
            {
              tokenRecord = tokenRecordList[index];
              break;
            }
          }
          if (tokenRecord != null)
          {
            IAttributeTypeItem attributeItemByCode2 = TechExpert.TypeConverter.GetAttributeItemByCode(this.GetEntityByIndex((int) tokenRecord.Index).Code, TechcardConsts.Plugin, out string _);
            if (attributeItemByCode2 != null && attributeItemByCode2.AttrValueType == 8)
              this._rangeConverted = true;
          }
        }
        Token token11 = this._rangeConverted ? new Token(Intermech.Expert.TokenType.Divider, ", ") : new Token(Intermech.Expert.TokenType.Divider, ":");
        ipsTokens.Add(token11);
        break;
      case 110:
        Token token12 = new Token(Intermech.Expert.TokenType.BinaryOper, "=");
        ipsTokens.Add(token12);
        break;
      case 111:
        Token token13 = new Token(Intermech.Expert.TokenType.BinaryOper, ">");
        ipsTokens.Add(token13);
        break;
      case 112 /*0x70*/:
        Token token14 = new Token(Intermech.Expert.TokenType.BinaryOper, "<");
        ipsTokens.Add(token14);
        break;
      case 113:
        Token token15 = new Token(Intermech.Expert.TokenType.BinaryOper, "<>");
        ipsTokens.Add(token15);
        break;
      case 114:
        Token token16 = new Token(Intermech.Expert.TokenType.BinaryOper, ">=");
        ipsTokens.Add(token16);
        break;
      case 115:
        Token token17 = new Token(Intermech.Expert.TokenType.BinaryOper, "<=");
        ipsTokens.Add(token17);
        break;
      case 116:
        Token token18 = new Token(Intermech.Expert.TokenType.BinaryOper, "?");
        ipsTokens.Add(token18);
        break;
      case 140:
        Token token19 = new Token(Intermech.Expert.TokenType.BinaryOper, "ИЛИ");
        ipsTokens.Add(token19);
        break;
      case 141:
        Token token20 = new Token(Intermech.Expert.TokenType.BinaryOper, "И");
        ipsTokens.Add(token20);
        break;
      case 142:
        Token token21 = new Token(Intermech.Expert.TokenType.UnaryOper, "НЕ");
        ipsTokens.Add(token21);
        break;
      case 160 /*0xA0*/:
        Token token22 = new Token(Intermech.Expert.TokenType.Float, "pi")
        {
          fValue = Math.PI
        };
        ipsTokens.Add(token22);
        break;
      case 252:
      case 253:
      case 254:
      case byte.MaxValue:
        throw new TokenConvertException($"Неверный аргумент. Ошибка конвертации TokenRec = {token.Kind}.");
      default:
        throw new TokenConvertException($"Неверный аргумент. Неизвестный тип TokenRec = {token.Kind}.");
    }
    return true;
  }

  public static Token CreateTokenAttribute(
    Guid attrGuid,
    Guid objTypeGuid,
    ref TempFormula ipsFormula)
  {
    if (attrGuid.Equals(Guid.Empty) || ipsFormula == null)
      return (Token) null;
    IAttributeTypeItem byGuid1 = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByGuid(attrGuid);
    if (byGuid1 == null)
      return (Token) null;
    IObjectTypeItem byGuid2 = objTypeGuid != Guid.Empty ? TechcardConsts.Plugin.Imdi.ObjectTypes.GetByGuid(objTypeGuid) : (IObjectTypeItem) null;
    return ExpTokenConverter.CreateTokenAttribute(byGuid1, byGuid2, ref ipsFormula);
  }

  public static Token CreateTokenAttribute(
    int attrTypeId,
    int objTypeId,
    ref TempFormula ipsFormula)
  {
    if (attrTypeId == -1 || ipsFormula == null)
      return (Token) null;
    IAttributeTypeItem byId1 = TechcardConsts.Plugin.Imdi.AttributeTypes.GetByID(attrTypeId);
    if (byId1 == null)
      return (Token) null;
    IObjectTypeItem byId2 = objTypeId != -1 ? TechcardConsts.Plugin.Imdi.ObjectTypes.GetByID(objTypeId) : (IObjectTypeItem) null;
    return ExpTokenConverter.CreateTokenAttribute(byId1, byId2, ref ipsFormula);
  }

  private static Token CreateTokenAttribute(
    IAttributeTypeItem attrTypeItem,
    IObjectTypeItem objTypeItem,
    ref TempFormula ipsFormula)
  {
    if (attrTypeItem == null || ipsFormula == null)
      return (Token) null;
    int id = attrTypeItem.ID;
    int num = -1;
    string shortName = attrTypeItem.ShortName;
    string name = attrTypeItem.Name;
    FieldTypes attrValueType = (FieldTypes) attrTypeItem.AttrValueType;
    bool multis = attrTypeItem.MultiValueMode == MultiValueModes.MultiValues || attrTypeItem.MultiValueMode == MultiValueModes.MultiValuesFromList;
    string oShortName = "";
    string oLongName = "";
    if (objTypeItem != null)
    {
      num = objTypeItem.ID;
      oShortName = objTypeItem.ShortName;
      oLongName = objTypeItem.Name;
    }
    int index1 = -1;
    for (int index2 = 0; index2 < ipsFormula.usedAttrs.Count; ++index2)
    {
      AttribPair usedAttr = ipsFormula.usedAttrs[index2];
      if (usedAttr.attribID == id && (num == -1 && usedAttr.objTypeID == 0 || num != -1 && usedAttr.objTypeID == num))
      {
        index1 = index2;
        break;
      }
    }
    if (index1 < 0)
    {
      AttribPair attribPair;
      PairName pairName;
      if (num != -1)
      {
        attribPair = new AttribPair(id, num);
        pairName = new PairName(shortName, name, oShortName, oLongName, attrValueType, multis);
      }
      else
      {
        attribPair = new AttribPair(id);
        pairName = new PairName(shortName, name, "", "", attrValueType, multis);
      }
      ipsFormula.usedAttrs.Add(attribPair);
      ipsFormula.pairNames.Add(pairName);
      ipsFormula.attrGUIDs.Add(attrTypeItem.GUID.ToString());
      ipsFormula.objTypeGUIDs.Add(num == -1 || objTypeItem == null ? "" : Convert.ToString((object) objTypeItem.GUID));
      index1 = ipsFormula.usedAttrs.Count - 1;
    }
    Token tokenAttribute = new Token(Intermech.Expert.TokenType.Attribute, ipsFormula.pairNames[index1].ShortName);
    tokenAttribute.info = index1;
    ipsFormula.UpdateTokenBegs();
    return tokenAttribute;
  }

  private bool FindTokenForBracket(out TokenRecord openToken, out TokenRecord prevToken)
  {
    openToken = prevToken = (TokenRecord) null;
    int num = 0;
    List<TokenRecord> tokenRecordList = new List<TokenRecord>((IEnumerable<TokenRecord>) this._tokenList.Keys);
    for (int index = tokenRecordList.Count - 2; index >= 0; --index)
    {
      TokenRecord tokenRecord = tokenRecordList[index];
      if (tokenRecord != null)
      {
        switch (tokenRecord.Kind)
        {
          case 65:
            if (num == 0)
            {
              openToken = tokenRecord;
              prevToken = index > 0 ? tokenRecordList[index - 1] : (TokenRecord) null;
              return true;
            }
            --num;
            continue;
          case 70:
            ++num;
            continue;
          default:
            continue;
        }
      }
    }
    return false;
  }

  private void CheckIntegerToken(ref Token ipsToken, List<Token> ipsTokens, TempFormula ipsFormula)
  {
    if (ipsToken == null || ipsToken.type != Intermech.Expert.TokenType.Integer || ipsFormula == null)
      return;
    bool flag1 = false;
    if (ipsFormula.resType == DataType.ObjectLink && this._entCode != string.Empty)
    {
      Entity entityByCode = TechExpert.TypeConverter.GetEntityByCode(this._entCode);
      if (entityByCode != null && this.ChangeIntToken2ObjLink(entityByCode, ipsToken))
        return;
    }
    if (ipsFormula.resType == DataType.Boolean && this._tokenList.Count > 0)
    {
      bool flag2 = false;
      TokenRecord tokenRecord = (TokenRecord) null;
      List<TokenRecord> tokenRecordList = new List<TokenRecord>((IEnumerable<TokenRecord>) this._tokenList.Keys);
      for (int index = tokenRecordList.Count - 2; index >= 0 && !flag2; --index)
      {
        TokenRecord key = tokenRecordList[index];
        switch (key.Kind)
        {
          case 1:
            bool flag3 = false;
            List<Token> token = this._tokenList[key];
            if (token != null && token.Any<Token>((Func<Token, bool>) (item => item.type == Intermech.Expert.TokenType.ObjectLink || item.type == Intermech.Expert.TokenType.Integer)))
              flag3 = true;
            if (!flag3)
            {
              flag2 = true;
              continue;
            }
            continue;
          case 15:
            tokenRecord = key;
            flag2 = true;
            continue;
          case 65:
          case 90:
          case 91:
          case 110:
          case 113:
          case 116:
            continue;
          default:
            flag2 = true;
            continue;
        }
      }
      if (tokenRecord != null)
      {
        Entity entityByIndex = this.GetEntityByIndex((int) tokenRecord.Index);
        if (entityByIndex != null)
          flag1 = this.ChangeIntToken2ObjLink(entityByIndex, ipsToken);
      }
    }
    if (flag1)
      return;
    this.CheckDoubleToken(ref ipsToken, ipsTokens, ipsFormula);
  }

  private void CheckStringToken(ref Token ipsToken, List<Token> ipsTokens, TempFormula ipsFormula)
  {
    if (ipsToken == null || ipsToken.type != Intermech.Expert.TokenType.String || ipsFormula == null)
      return;
    bool flag1 = false;
    if (ipsFormula.resType == DataType.ObjectLink && this._entCode != string.Empty)
    {
      Entity entityByCode = TechExpert.TypeConverter.GetEntityByCode(this._entCode);
      if (entityByCode != null && this.ChangeStrToken2ObjLink(entityByCode, ipsToken))
        return;
    }
    if (ipsFormula.resType == DataType.Boolean && this._tokenList.Count > 0)
    {
      bool flag2 = false;
      TokenRecord tokenRecord = (TokenRecord) null;
      List<TokenRecord> tokenRecordList = new List<TokenRecord>((IEnumerable<TokenRecord>) this._tokenList.Keys);
      for (int index = tokenRecordList.Count - 2; index >= 0 && !flag2; --index)
      {
        TokenRecord key = tokenRecordList[index];
        switch (key.Kind)
        {
          case 3:
            bool flag3 = false;
            List<Token> token = this._tokenList[key];
            if (token != null && token.Any<Token>((Func<Token, bool>) (item => item.type == Intermech.Expert.TokenType.ObjectLink || item.type == Intermech.Expert.TokenType.Integer)))
              flag3 = true;
            if (!flag3)
            {
              flag2 = true;
              continue;
            }
            continue;
          case 15:
            tokenRecord = key;
            flag2 = true;
            continue;
          case 65:
          case 90:
          case 91:
          case 110:
          case 113:
          case 116:
            continue;
          default:
            flag2 = true;
            continue;
        }
      }
      if (tokenRecord != null)
      {
        Entity entityByIndex = this.GetEntityByIndex((int) tokenRecord.Index);
        if (entityByIndex != null)
          flag1 = this.ChangeStrToken2ObjLink(entityByIndex, ipsToken);
      }
    }
    if (flag1)
      return;
    this.CheckDoubleToken(ref ipsToken, ipsTokens, ipsFormula);
  }

  private void CheckDoubleToken(ref Token ipsToken, List<Token> ipsTokens, TempFormula ipsFormula)
  {
    if (ipsToken == null || ipsFormula == null || ipsToken.type != Intermech.Expert.TokenType.Integer && ipsToken.type != Intermech.Expert.TokenType.Float)
      return;
    if (ipsFormula.resType == DataType.Measured && this._entCode != string.Empty)
    {
      List<TokenRecord> tokenRecordList = new List<TokenRecord>((IEnumerable<TokenRecord>) this._tokenList.Keys);
      Entity entityByCode = TechExpert.TypeConverter.GetEntityByCode(this._entCode);
      if (entityByCode != null)
      {
        bool flag1 = false;
        bool flag2 = false;
        for (int index = tokenRecordList.Count - 2; index >= 0 && !flag2; --index)
        {
          TokenRecord tokenRecord = tokenRecordList[index];
          if (tokenRecord != null)
          {
            switch (tokenRecord.Kind)
            {
              case 85:
              case 86:
                flag2 = true;
                continue;
              case 87:
              case 88:
              case 89:
                flag2 = true;
                flag1 = true;
                continue;
              default:
                continue;
            }
          }
        }
        if (!flag1 && this.ChangeToken2Measured(entityByCode, ipsToken))
          return;
      }
    }
    if (ipsFormula.resType != DataType.Boolean || this._tokenList.Count <= 0)
      return;
    List<TokenRecord> tokenRecordList1 = new List<TokenRecord>((IEnumerable<TokenRecord>) this._tokenList.Keys);
    bool flag3 = false;
    TokenRecord tokenRecord1 = (TokenRecord) null;
    for (int index = tokenRecordList1.Count - 2; index >= 0 && !flag3; --index)
    {
      TokenRecord key = tokenRecordList1[index];
      switch (key.Kind)
      {
        case 1:
        case 2:
          bool flag4 = false;
          List<Token> token = this._tokenList[key];
          if (token != null && token.Any<Token>((Func<Token, bool>) (item => item.type == Intermech.Expert.TokenType.Measured)))
            flag4 = true;
          if (!flag4)
          {
            flag3 = true;
            continue;
          }
          continue;
        case 15:
          tokenRecord1 = key;
          flag3 = true;
          continue;
        case 90:
        case 91:
        case 110:
        case 113:
        case 116:
          continue;
        default:
          flag3 = true;
          continue;
      }
    }
    if (tokenRecord1 == null)
      return;
    Entity entityByIndex = this.GetEntityByIndex((int) tokenRecord1.Index);
    if (entityByIndex == null)
      return;
    this.ChangeToken2Measured(entityByIndex, ipsToken);
  }

  private void CheckClosingBracket(ref Token ipsToken, List<Token> ipsTokens)
  {
    TokenRecord openToken;
    TokenRecord prevToken;
    this.FindTokenForBracket(out openToken, out prevToken);
    if (openToken == null)
      throw new TokenConvertException("Ошибка в структуре формулы. Not found opened token for closing bracket");
    if (openToken.Kind == (byte) 65 && prevToken != null)
      openToken = prevToken;
    switch (openToken.Kind)
    {
      case 39:
      case 41:
      case 42:
      case 47:
        int num1 = openToken.Kind == (byte) 47 ? 3 : (int) openToken.Kind - 40;
        ipsTokens.Add(new Token(Intermech.Expert.TokenType.Divider, ", "));
        Token token1 = new Token(Intermech.Expert.TokenType.Integer, num1.ToString())
        {
          iValue = (long) num1
        };
        ipsTokens.Add(token1);
        if (openToken.Kind != (byte) 39)
          break;
        ipsTokens.Add(new Token(Intermech.Expert.TokenType.ClosingBrace, ")"));
        break;
      case 48 /*0x30*/:
      case 49:
      case 50:
      case 51:
        int num2 = (int) openToken.Kind - 48 /*0x30*/ + 1;
        ipsTokens.Add(new Token(Intermech.Expert.TokenType.Divider, ", "));
        Token token2 = new Token(Intermech.Expert.TokenType.Integer, num2.ToString())
        {
          iValue = (long) num2
        };
        ipsTokens.Add(token2);
        break;
      case 116:
        ipsToken.text = "}";
        break;
    }
  }

  private void CheckEntityToken(
    Entity entity,
    FieldTypes attrType,
    ref Token ipsToken,
    TempFormula ipsFormula)
  {
    if (attrType != FieldTypes.ftObjectLink)
    {
      if (attrType != FieldTypes.ftMeasured)
        return;
      this.CheckEntityToken_Measure(entity, ref ipsToken, ipsFormula);
    }
    else
      this.CheckEntityToken_Link(entity, ref ipsToken, ipsFormula);
  }

  private void CheckEntityToken_Link(Entity entity, ref Token ipsToken, TempFormula ipsFormula)
  {
    if (entity == null || ipsToken == null || ipsFormula == null || ipsFormula.resType != DataType.Boolean || this._tokenList.Count <= 0)
      return;
    List<TokenRecord> tokenRecordList = new List<TokenRecord>((IEnumerable<TokenRecord>) this._tokenList.Keys);
    for (int index = tokenRecordList.Count - 2; index >= 0; --index)
    {
      TokenRecord token = tokenRecordList[index];
      if (token == null)
        break;
      switch (token.Kind)
      {
        case 1:
          this.ChangeIntToken2ObjLink(entity, token);
          return;
        case 3:
          this.ChangeStrToken2ObjLink(entity, token);
          return;
        case 90:
        case 91:
        case 110:
        case 113:
        case 116:
          continue;
        default:
          return;
      }
    }
  }

  private void CheckEntityToken_Measure(Entity entity, ref Token ipsToken, TempFormula ipsFormula)
  {
    if (entity == null || ipsToken == null || ipsFormula == null || ipsFormula.resType != DataType.Boolean || this._tokenList.Count <= 0)
      return;
    List<TokenRecord> tokenRecordList = new List<TokenRecord>((IEnumerable<TokenRecord>) this._tokenList.Keys);
    for (int index = tokenRecordList.Count - 2; index >= 0; --index)
    {
      TokenRecord token = tokenRecordList[index];
      if (token == null)
        break;
      switch (token.Kind)
      {
        case 1:
        case 2:
          this.ChangeToken2Measured(entity, token);
          return;
        case 90:
        case 91:
        case 110:
        case 113:
        case 116:
          continue;
        default:
          return;
      }
    }
  }

  private void ChangeIntToken2ObjLink(Entity entity, TokenRecord token)
  {
    if (token == null || token.Kind != (byte) 1)
      return;
    foreach (Token ipsToken in this._tokenList[token])
      this.ChangeIntToken2ObjLink(entity, ipsToken);
  }

  private bool ChangeIntToken2ObjLink(Entity entity, Token ipsToken)
  {
    if (ipsToken == null || ipsToken.type != Intermech.Expert.TokenType.Integer || !this.CanChangeIntToken2ObjLink(entity))
      return false;
    int iValue = (int) ipsToken.iValue;
    if (iValue == 0)
      return false;
    try
    {
      ipsToken.iValue = TechExpert.DataConverter.ConvertValue2ObjectLink(entity, iValue, this._pumper._importingData);
      ipsToken.text = ipsToken.iValue.ToString();
      ipsToken.spt = SelectionParameterTypes.sptObject;
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case ObjLinkTypeCheckFailException _:
        case ObjectLinkTypeConvertException _:
          ipsToken.iValue = 0L;
          string message = ex.Message;
          TechcardConsts.Plugin.appManager.AddWarningMessage(message);
          break;
        default:
          throw;
      }
    }
    return true;
  }

  private void ChangeStrToken2ObjLink(Entity entity, TokenRecord token)
  {
    if (token == null || token.Kind != (byte) 3)
      return;
    foreach (Token ipsToken in this._tokenList[token])
      this.ChangeStrToken2ObjLink(entity, ipsToken);
  }

  private bool ChangeStrToken2ObjLink(Entity entity, Token ipsToken)
  {
    if (ipsToken == null || ipsToken.type != Intermech.Expert.TokenType.String || !this.CanChangeStrToken2ObjLink(entity))
      return false;
    string trueText = ipsToken.trueText;
    ipsToken.type = Intermech.Expert.TokenType.Integer;
    if (string.IsNullOrEmpty(trueText))
      return false;
    try
    {
      ipsToken.iValue = TechExpert.DataConverter.ConvertImbaseCode2ObjectLink(entity, trueText, this._pumper._importingData);
      ipsToken.text = ipsToken.iValue.ToString();
      ipsToken.spt = SelectionParameterTypes.sptObject;
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case ObjLinkTypeCheckFailException _:
        case ObjectLinkTypeConvertException _:
          ipsToken.iValue = 0L;
          string message = ex.Message;
          TechcardConsts.Plugin.appManager.AddWarningMessage(message);
          break;
        default:
          throw;
      }
    }
    return true;
  }

  private void ChangeToken2Measured(Entity entity, TokenRecord token)
  {
    if (entity == null || token == null)
      return;
    foreach (Token ipsToken in this._tokenList[token])
      this.ChangeToken2Measured(entity, ipsToken);
  }

  private bool ChangeToken2Measured(Entity entity, Token ipsToken)
  {
    if (entity == null || ipsToken == null || ipsToken.type != Intermech.Expert.TokenType.Integer && ipsToken.type != Intermech.Expert.TokenType.Float)
      return false;
    double num = ipsToken.type == Intermech.Expert.TokenType.Integer ? (double) ipsToken.iValue : ipsToken.fValue;
    MeasuredValue measuredValue;
    if (!TechExpert.DataConverter.ConvertValue2Measured(entity, num, 0, out measuredValue, false))
      return false;
    ipsToken.fValue = measuredValue.Value;
    ipsToken.iValue = measuredValue.MeasureID;
    ipsToken.type = Intermech.Expert.TokenType.Measured;
    return true;
  }

  public Dictionary<TokenRecord, List<Token>> TokenList => this._tokenList;
}
