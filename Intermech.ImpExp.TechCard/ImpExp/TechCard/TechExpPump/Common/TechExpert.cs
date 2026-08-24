// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.Common.TechExpert
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.Expert.Table;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.TechTypes;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.Common;

public static class TechExpert
{
  public static class Const
  {
    public static string cnt_ImportGuid_Prefix = "caee";
    public static int cnt_FileVersion = 250;
    public const int cnt_tcfHeaderSize = 40;
    public const int cnt_tcfKeySize = 4;
    public const string cnt_tcfKey = "TCFF";
    public const int cnt_tcfmaxOutlineDepth = 256 /*0x0100*/;
    public const int cnt_tcfFileVers = 103;
    public const int cnt_tcfODelim = 560685328;
    public const int cnt_tcfEDelim = 558579984;
    public const int cnt_tblMaxCols = 5000;
    public const int cnt_tblMaxRows = 20000;
    public const int cnt_tblMaxConds = 32 /*0x20*/;
    public const int cnt_tblMaxLayers = 20;
    public const string cnt_exp_bool_True = "ДА";
    public const string con_exp_bool_False = "НЕТ";
    public const string cnt_exp_oper_OR = "ИЛИ";
    public const string cnt_exp_oper_AND = "И";
    public const string cnt_exp_oper_NOT = "НЕ";
  }

  public static class Tokens
  {
    public const byte cnt_rgLoID = 1;
    public const byte cnt_rgHiID = 19;
    public const byte cnt_rgLoFun = 20;
    public const byte cnt_rgHiFun = 64 /*0x40*/;
    public const byte cnt_tcNothing = 0;
    public const byte cnt_tcInteger = 1;
    public const byte cnt_tcFloat = 2;
    public const byte cnt_tcString = 3;
    public const byte cnt_tcBoolean = 4;
    public const byte cnt_tcID = 15;
    public const byte cnt_tcSin = 20;
    public const byte cnt_tcCos = 21;
    public const byte cnt_tcTg = 22;
    public const byte cnt_tcArctg = 23;
    public const byte cnt_tcCh = 25;
    public const byte cnt_tcExp = 26;
    public const byte cnt_tcLn = 27;
    public const byte cnt_tcLg = 28;
    public const byte cnt_tcInt = 29;
    public const byte cnt_tcFrac = 30;
    public const byte cnt_tcAbs = 31 /*0x1F*/;
    public const byte cnt_tcSqrt = 32 /*0x20*/;
    public const byte cnt_tcNom = 33;
    public const byte cnt_tcHi = 34;
    public const byte cnt_tcLo = 35;
    public const byte cnt_tcKv = 36;
    public const byte cnt_tcKt = 37;
    public const byte cnt_tcStep = 38;
    public const byte cnt_tcR10 = 39;
    public const byte cnt_tcR1 = 40;
    public const byte cnt_tcR01 = 41;
    public const byte cnt_tcR001 = 42;
    public const byte cnt_tcRa = 43;
    public const byte cnt_tcRs_old = 44;
    public const byte cnt_tcCtn = 45;
    public const byte cnt_tcDef = 46;
    public const byte cnt_tcR0001 = 47;
    public const byte cnt_tcS1 = 48 /*0x30*/;
    public const byte cnt_tcS2 = 49;
    public const byte cnt_tcS3 = 50;
    public const byte cnt_tcS4 = 51;
    public const byte cnt_tcSum = 52;
    public const byte cnt_tcMin = 53;
    public const byte cnt_tcMax = 54;
    public const byte cnt_tcPar = 55;
    public const byte cnt_tcBrO = 65;
    public const byte cnt_tcBrC = 70;
    public const byte cnt_tcConc = 75;
    public const byte cnt_tcPlus = 85;
    public const byte cnt_tcMinus = 86;
    public const byte cnt_tcMulti = 87;
    public const byte cnt_tcDivide = 88;
    public const byte cnt_tcSt = 89;
    public const byte cnt_tcEDl = 90;
    public const byte cnt_tcRDl = 91;
    public const byte cnt_tcEq = 110;
    public const byte cnt_tcGrt = 111;
    public const byte cnt_tcLes = 112 /*0x70*/;
    public const byte cnt_tcNEq = 113;
    public const byte cnt_tcGEq = 114;
    public const byte cnt_tcLEq = 115;
    public const byte cnt_tcEn = 116;
    public const byte cnt_tcOr = 140;
    public const byte cnt_tcAnd = 141;
    public const byte cnt_tcNot = 142;
    public const byte cnt_tcPi = 160 /*0xA0*/;
    public const byte cnt_tcFootnote = 180;
    public const byte cnt_tcOrJump = 181;
    public const byte cnt_tcAndJump = 182;
    public const byte cnt_tcToInteger = 252;
    public const byte cnt_tcToFloat = 253;
    public const byte cnt_tcToString = 254;
    public const byte cnt_tcToBoolean = 255 /*0xFF*/;

    public static string Token2String(byte tokenId)
    {
      string str = string.Empty;
      switch (tokenId)
      {
        case 43:
          str = "Ra()";
          break;
        case 52:
          str = "Sum()";
          break;
        case 53:
          str = "Min()";
          break;
        case 54:
          str = "Max()";
          break;
        case 55:
          str = "Par()";
          break;
      }
      return str;
    }
  }

  public static class Sql
  {
    public const string Expert = " SELECT * FROM TC_EXPERT WHERE F_TYPE   IN ({0}) {1} ORDER BY F_KEY";
    public const string ExpertCount = "SELECT COUNT(*) FROM TC_EXPERT WHERE F_TYPE IN ({0}) {1} ";
  }

  public static class Utils
  {
    public static byte[] END_OF_LIST = new byte[4]
    {
      (byte) 1,
      (byte) 3,
      (byte) 5,
      (byte) 7
    };

    public static void TechLoadStringList(ref List<string> list, BinaryReader reader)
    {
      if (list == null || reader == null)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      string str1 = new ASCIIEncoding().GetString(TechExpert.Utils.END_OF_LIST);
      long position;
      string str2;
      int num;
      while (true)
      {
        long count = reader.BaseStream.Length - reader.BaseStream.Position;
        if (count > 200L)
          count = 200L;
        position = reader.BaseStream.Position;
        str2 = new string(reader.ReadChars((int) count));
        if (str2.Length != 0)
        {
          num = str2.IndexOf(str1, StringComparison.Ordinal);
          if (num == -1)
            stringBuilder.Append(str2);
          else
            break;
        }
        else
          goto label_8;
      }
      stringBuilder.Append(str2.Substring(0, num - 1));
      reader.BaseStream.Position = position + (long) num + (long) TechExpert.Utils.END_OF_LIST.Length;
label_8:
      string[] collection = stringBuilder.ToString().Split('\n');
      list.AddRange((IEnumerable<string>) collection);
    }

    public static string TechReadString(BinaryReader reader)
    {
      return TechExpert.Utils.TechReadString(reader, TechExpert.Const.cnt_FileVersion);
    }

    public static string TechReadString(BinaryReader reader, int version)
    {
      string empty = string.Empty;
      int count = version >= TechExpert.Const.cnt_FileVersion ? (int) reader.ReadUInt16() : (int) reader.ReadByte();
      if (count > 5000 || (count | (int) byte.MaxValue) == 0 && version < 250)
        count &= (int) byte.MaxValue;
      return count != 0 ? new string(reader.ReadChars(count)) : empty;
    }

    public static string TechReadByteString(BinaryReader reader)
    {
      return TechExpert.Utils.TechReadString(reader, TechExpert.Const.cnt_FileVersion - 1);
    }

    public static string TechReadIntString(BinaryReader reader)
    {
      return TechExpert.Utils.TechReadString(reader, TechExpert.Const.cnt_FileVersion);
    }

    public static string ReadString(BinaryReader br, ushort version)
    {
      return TechExpert.Utils.TechReadString(br, (int) version);
    }

    public static Guid GetGuid4Import()
    {
      string str = Guid.NewGuid().ToString();
      return new Guid(TechExpert.Const.cnt_ImportGuid_Prefix + str.Remove(0, TechExpert.Const.cnt_ImportGuid_Prefix.Length));
    }
  }

  public static class DataConverter
  {
    public static eCellSymbol ConvertSymbol(string oldSymbol)
    {
      switch (oldSymbol)
      {
        case "+":
          return eCellSymbol.Other;
        case "<":
          return eCellSymbol.Less;
        case "<=":
          return eCellSymbol.LessOrEqual;
        case "<>":
          return eCellSymbol.NotEqual;
        case "=":
          return eCellSymbol.Equal;
        case ">":
          return eCellSymbol.More;
        case ">=":
          return eCellSymbol.MoreOrEqual;
        case "{}":
          return eCellSymbol.Set;
        default:
          return eCellSymbol.None;
      }
    }

    public static eCellSymbol ConvertSymbol(byte oldSymbolCode)
    {
      switch (oldSymbolCode)
      {
        case 0:
          return eCellSymbol.Less;
        case 1:
          return eCellSymbol.LessOrEqual;
        case 2:
          return eCellSymbol.Equal;
        case 3:
          return eCellSymbol.NotEqual;
        case 4:
          return eCellSymbol.MoreOrEqual;
        case 5:
          return eCellSymbol.More;
        case 6:
          return eCellSymbol.Set;
        case 7:
          return eCellSymbol.Other;
        default:
          return eCellSymbol.None;
      }
    }

    internal static long ConvertValue2ObjectLink(
      Entity entity,
      int value,
      IImportingData importingData)
    {
      if (importingData == null)
        throw new ObjectLinkTypeConvertException("Importing data not defined");
      DictionaryValue dictionaryValue = ImbaseLinkConvertor.Instance.ConvertValue(entity, value, importingData, true);
      return dictionaryValue == null ? 0L : dictionaryValue.NewObjectID;
    }

    internal static long ConvertImbaseCode2ObjectLink(
      Entity entity,
      string imbaseCode,
      IImportingData importingData)
    {
      DictionaryValue dictionaryValue = ImbaseKeyConvertor.Instance.ConvertValue(entity, imbaseCode, importingData);
      return dictionaryValue == null ? 0L : dictionaryValue.NewObjectID;
    }

    internal static bool ConvertValue2Measured(
      Entity entity,
      double value,
      int prodId,
      out MeasuredValue measuredValue,
      bool throwException)
    {
      measuredValue = (MeasuredValue) null;
      if (entity == null)
        return false;
      if (entity.Settings.MeasProdSettings.PhysicalValueId == -1L)
      {
        if (throwException)
          throw new EntitySettNotExistException($"Не найдены настройки единиц измерения для понятия \"{entity.Name}\"");
        return false;
      }
      measuredValue = new MeasuredValue(value, entity.Settings.MeasProdSettings[prodId]);
      return true;
    }
  }

  public static class TypeConverter
  {
    private static readonly Dictionary<string, IAttributeTypeItem> ParamCache = new Dictionary<string, IAttributeTypeItem>();

    public static void CheckDataType(
      string paramCode,
      IAttributeTypeItem attrItem,
      DataType dataType)
    {
      switch (dataType)
      {
        case DataType.ObjectLink:
          Entity entityByCode = TechExpert.TypeConverter.GetEntityByCode(paramCode);
          if (entityByCode != null && entityByCode.EntityReference != null && entityByCode.EntityReference.MasterCode != paramCode && entityByCode.EntityReference.Field != -2)
            throw new ObjectLinkTypeConvertException($"Ошибка конвертации понятия \"{paramCode}\" к типу данных ЭС \"{EnumTypeHelper.GetCaption((Enum) dataType)}\"");
          if (entityByCode == null || entityByCode.EntityReference != null && entityByCode.EntityReference.Reference != 0)
            break;
          throw new ObjectLinkTypeConvertException($"Ошибка! Для понятия \"{paramCode}\" не найдена привязка к справочнику IMBASE");
        case DataType.Packet:
        case DataType.Diap:
        case DataType.Attribute:
        case DataType.ObjType:
        case DataType.RelType:
          throw new ObjectLinkTypeConvertException($"Ошибка конвертации атрибута \"{attrItem}\" к типу данных ЭС \"{EnumTypeHelper.GetCaption((Enum) dataType)}\"");
      }
    }

    public static DataType ConvertFormulaResultType(
      short resType,
      string paramCode,
      IAttributeTypeItem attrTypeItem)
    {
      if (attrTypeItem != null)
      {
        FieldTypes attrValueType = (FieldTypes) attrTypeItem.AttrValueType;
        if (attrValueType != FieldTypes.ftUnknown)
        {
          DataType dataType = DataTypeConvertor.AttrType2DataType(attrValueType);
          TechExpert.TypeConverter.CheckDataType(paramCode, attrTypeItem, dataType);
          return dataType;
        }
      }
      switch (resType)
      {
        case 0:
          return DataType.Integer;
        case 1:
          return DataType.Float;
        case 2:
          return DataType.String;
        case 3:
          return DataType.Boolean;
        default:
          return DataType.RelType;
      }
    }

    internal static Entity GetEntityByCode(string paramCode)
    {
      if (paramCode.Equals(string.Empty))
        throw new AttributeNotExistsException("Неверный аргумент. Имя понятия не определено.");
      Entity entityByCode;
      if (!TechPumpData.Entities.EntitiesList.TryGetValue(paramCode, out entityByCode))
        throw new EntitySettNotExistException($"Неверный аргумент. Понятие ({paramCode}) не найдено в настройках.");
      return entityByCode;
    }

    internal static TechTypeSett GetObjectTypeSettByEntity(Entity entity)
    {
      if (entity.RecordID == 0)
        return (TechTypeSett) null;
      TechTypeInfo techTypeInfo;
      TechPumpData.TechType.TechTypeList.TryGetValue(entity.RecordID, out techTypeInfo);
      return techTypeInfo?.TypeSett ?? throw new EntitySettNotExistException($"Неверный аргумент. Для типа записи TechCard ID = {entity.RecordID} не настроен тип объекта в IPS.");
    }

    public static IAttributeTypeItem GetAttributeItemByCode(
      string paramCode,
      PluginClass plugin,
      out string errorMsg)
    {
      errorMsg = "Невозможно получить IAttributeType для понятия \"{0}\".  Ошибка: {1}";
      if (plugin == null)
      {
        errorMsg = string.Format(errorMsg, (object) paramCode, (object) "Plugin reference is empty");
        return (IAttributeTypeItem) null;
      }
      if (paramCode == string.Empty)
      {
        errorMsg = string.Format(errorMsg, (object) paramCode, (object) "Имя понятия не определено");
        return (IAttributeTypeItem) null;
      }
      IAttributeTypeItem byGuid;
      if (TechExpert.TypeConverter.ParamCache.TryGetValue(paramCode, out byGuid))
      {
        errorMsg = "";
        return byGuid;
      }
      Guid guid;
      if (!TechPumpData.Entities.Code2AttributeGuid.TryGetValue(paramCode, out guid))
      {
        errorMsg = string.Format(errorMsg, (object) paramCode, (object) "Неверный аргумент. Не найден атрибут для понятия ( понятие отсутствует в настройках )");
        return (IAttributeTypeItem) null;
      }
      if (guid == Guid.Empty)
      {
        errorMsg = string.Format(errorMsg, (object) paramCode, (object) "Неверный аргумент. Атрибут для понятия ({0}) не определен");
        return (IAttributeTypeItem) null;
      }
      byGuid = plugin.Imdi.AttributeTypes.GetByGuid(guid);
      if (byGuid == null)
      {
        errorMsg = string.Format(errorMsg, (object) paramCode, (object) $"Атрибут \"{(object) guid}\" не найден в кэше типов атрибутов IAttributeTypeItemList");
        return (IAttributeTypeItem) null;
      }
      errorMsg = "";
      TechExpert.TypeConverter.ParamCache.Add(paramCode, byGuid);
      return byGuid;
    }
  }
}
