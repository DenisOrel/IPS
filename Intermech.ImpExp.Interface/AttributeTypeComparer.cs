// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.AttributeTypeComparer
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Функции для проверки совместимости атрибутов</summary>
public static class AttributeTypeComparer
{
  /// <summary>Функция сравнения типов атрибутов</summary>
  /// <returns></returns>
  public static string[] CompareAttributeType(
    IAttributeTypeToCreate outAttrType,
    FieldTypes inFieldType,
    int inAttributeSize,
    ref ItemErrorType errorType)
  {
    List<string> stringList = new List<string>();
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    List<FieldTypes> convertList = new List<FieldTypes>();
    RelationalOperators[] enabledOperators = (RelationalOperators[]) null;
    bool computableAttribute = false;
    AttributeCacheHelper.GetAttributeTypeValues(inFieldType, -1, ref empty1, ref empty3, ref convertList, ref enabledOperators, ref computableAttribute, ref empty2);
    FieldTypes fieldType = outAttrType.FieldType;
    if (inFieldType == FieldTypes.ftObjectLink)
    {
      switch (fieldType)
      {
        case FieldTypes.ftString:
          errorType = ItemErrorType.Renamed;
          stringList.Add("Возможна потеря данных");
          stringList.Add($"Импортируемый тип: \"{EnumDescConverter.GetEnumDescription((Enum) inFieldType)}\" и тип в базе назначения: \"{EnumDescConverter.GetEnumDescription((Enum) fieldType)}\".");
          break;
        case FieldTypes.ftObjectLink:
          errorType = ItemErrorType.Warning;
          stringList.Add("Проверьте атрибут \"Тип создаваемого по ссылке объекта\"");
          break;
        default:
          errorType = ItemErrorType.Renamed;
          stringList.Add("Несовместимы типы данных.");
          stringList.Add($"Импортируемый тип: \"{EnumDescConverter.GetEnumDescription((Enum) inFieldType)}\" и тип в базе назначения: \"{EnumDescConverter.GetEnumDescription((Enum) fieldType)}\".");
          break;
      }
    }
    else if (fieldType != inFieldType)
    {
      if (!convertList.Contains(fieldType))
      {
        errorType = ItemErrorType.Renamed;
        stringList.Add("Несовместимы типы данных.");
        stringList.Add($"Импортируемый тип: \"{EnumDescConverter.GetEnumDescription((Enum) inFieldType)}\" и тип в базе назначения: \"{EnumDescConverter.GetEnumDescription((Enum) fieldType)}\".");
      }
      else
      {
        errorType = ItemErrorType.Renamed;
        stringList.Add("Возможна потеря данных при конвертации типов данных.");
        stringList.Add($"Импортируемый тип: \"{EnumDescConverter.GetEnumDescription((Enum) inFieldType)}\" и тип в базе назначения: \"{EnumDescConverter.GetEnumDescription((Enum) fieldType)}\".");
      }
    }
    else if (fieldType == inFieldType && inFieldType == FieldTypes.ftString && (long) inAttributeSize > outAttrType.Size)
    {
      errorType = ItemErrorType.Renamed;
      stringList.Add("Возможна потеря данных");
      stringList.Add($"Длина значения импортируемого типа: {inAttributeSize} и длина значения типа в базе назначения: {outAttrType.Size}");
    }
    if (stringList.Count != 0)
      return stringList.ToArray();
    errorType = ItemErrorType.None;
    return (string[]) null;
  }

  public static int GetMaxAttributeSize(FieldTypes fieldType)
  {
    int maxAttributeSize = 0;
    switch (fieldType)
    {
      case FieldTypes.ftString:
        maxAttributeSize = Consts.MaxStringSize;
        break;
      case FieldTypes.ftInteger:
      case FieldTypes.ftDouble:
        maxAttributeSize = Consts.MaxNumericSize;
        break;
      case FieldTypes.ftMemo:
        maxAttributeSize = Consts.MaxShortBlobSize;
        break;
    }
    return maxAttributeSize;
  }
}
