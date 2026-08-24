// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.AttributesHelper
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.CommonData;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interface;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class AttributesHelper
{
  /// <summary>Обязательные атрибуты для связей</summary>
  private static readonly IDictionary<Tuple<int, int[]>, List<TypeAttribute>> _obligatoryRelationAttribute = (IDictionary<Tuple<int, int[]>, List<TypeAttribute>>) new Dictionary<Tuple<int, int[]>, List<TypeAttribute>>();
  /// <summary>Обязательные атрибуты для связей</summary>
  private static readonly IDictionary<Tuple<int, int[]>, List<TypeAttribute>> _obligatoryObjectAttribute = (IDictionary<Tuple<int, int[]>, List<TypeAttribute>>) new Dictionary<Tuple<int, int[]>, List<TypeAttribute>>();
  /// <summary>
  /// Для дефолтных ссылок у атрибутов типов сохраним кэпшены объектов
  /// (эта ситуация очень маловероятна, поэтому по поводу запросов не беспокоиццо - пишеться на всякий случай)
  /// </summary>
  private static readonly Dictionary<long, string> _objectCaptions = new Dictionary<long, string>();

  /// <summary>Добавить к связи обязательные атрибуты</summary>
  public static void AddObligatoryRelationAttributes(
    IDataWriter dataWriter,
    IImportedRelationList iolm)
  {
    ImportingRelation attributable = iolm.Items[iolm.Items.Count - 1];
    Tuple<int, int[]> key = new Tuple<int, int[]>(attributable.Relation.RelationType, (int[]) null);
    List<TypeAttribute> obligatoryAttributes;
    if (!AttributesHelper._obligatoryRelationAttribute.TryGetValue(key, out obligatoryAttributes))
    {
      obligatoryAttributes = new Attributes4RelationReader(attributable.Relation.RelationType).Read();
      AttributesHelper._obligatoryRelationAttribute.Add(key, obligatoryAttributes);
    }
    AttributesHelper.AddObligatoryObjectAttributes(dataWriter.GetUserSession(), (IImportedAttributeList) iolm, (ImportingAttributable) attributable, obligatoryAttributes);
  }

  public static bool IsNumericType(Type type)
  {
    if (!type.IsEnum)
    {
      switch (Type.GetTypeCode(type))
      {
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          return true;
      }
    }
    return false;
  }

  public static void AddObligatoryObjectAttributes(
    IUserSession session,
    IImportedObjectList iolm,
    params int[] forbiddenAttributeIDs)
  {
    if (iolm == null || iolm.Items.Count == 0)
      return;
    AttributesHelper.AddObligatoryObjectAttributes(session, iolm, iolm.Items.CurrentIndex);
  }

  public static void CorrectObligatoryObjectAttributes(
    IUserSession session,
    IImportedObjectList iolm)
  {
    if (iolm == null || iolm.Items.Count == 0)
      return;
    ImportingObject attributable = iolm.Items[iolm.Items.CurrentIndex];
    List<TypeAttribute> objectAttributes = AttributesHelper.GetObjectAttributes(attributable.Object.ObjectType, true);
    HashSet<int> objectAttributeIDs = new HashSet<int>(objectAttributes.Select<TypeAttribute, int>((Func<TypeAttribute, int>) (item => item.AttributeID)));
    attributable.Attributes.RemoveAll((Predicate<AttributeRecord>) (x => !objectAttributeIDs.Contains(x.AttributeId)));
    AttributesHelper.AddObligatoryObjectAttributes(session, (IImportedAttributeList) iolm, (ImportingAttributable) attributable, objectAttributes);
  }

  /// <summary>
  /// Добавление обязательных атрибутов для импортируемого объекта
  /// </summary>
  /// <param name="session"></param>
  /// <param name="iolm"></param>
  /// <param name="objectIndex"></param>
  /// <remarks>Наличие objIndex'a требуется в ряде случаев, когда вызывается UseObject</remarks>
  /// &gt;
  public static void AddObligatoryObjectAttributes(
    IUserSession session,
    IImportedObjectList iolm,
    int objectIndex,
    params int[] forbiddenAttributeIDs)
  {
    if (iolm == null || iolm.Items.Count == 0 || objectIndex < 0 || objectIndex >= iolm.Items.Count)
      return;
    ImportingObject attributable = iolm.Items[objectIndex];
    int packetSize = iolm.PacketSize;
    ImportingObject importingObject = iolm.Items[iolm.Items.CurrentIndex];
    try
    {
      iolm.PacketSize = 0;
      if (attributable != importingObject)
        iolm.UseObject((Guid) attributable.Object.ObjectGuid, attributable.Object.Object_id);
      List<TypeAttribute> objectAttributes = AttributesHelper.GetObjectAttributes(attributable.Object.ObjectType, false, forbiddenAttributeIDs);
      AttributesHelper.AddObligatoryObjectAttributes(session, (IImportedAttributeList) iolm, (ImportingAttributable) attributable, objectAttributes);
    }
    finally
    {
      iolm.PacketSize = packetSize;
      if (iolm.Items.CurrentIndex == -1 || iolm.Items[iolm.Items.CurrentIndex] != importingObject)
        iolm.UseObject((Guid) importingObject.Object.ObjectGuid, importingObject.Object.Object_id);
    }
  }

  /// <summary>
  /// Сохраняет атрибут в кэш для создания его в списке при статре следующий раз
  /// </summary>
  public static void SaveAttributeToCreate(
    IAttributeTypeToCreate item,
    IMeasures measures,
    IImportingData cacheData)
  {
    if (item.FieldType == FieldTypes.ftUnknown)
    {
      item.FieldType = FieldTypes.ftString;
      item.Size = (long) Consts.MaxStringSize;
    }
    if (cacheData.GetNewKey(ImportingCategory.AttributeTypesToCreate, (object) item.Name) != 0L)
      return;
    cacheData.AddValue(ImportingCategory.AttributeTypesToCreate, (object) item.Name, long.MinValue, (ITagImportObject) new AttributeType(item.Name, item.ShortName, item.Alias, item.DefaultValue, item.GUID, item.Size, item.SystemId, item.FieldType, item.ValuesListIds, item.ValuesListMeasureIDs, item.MultiValueMode));
  }

  public static AttributeCheckResult CheckTypes(
    FieldTypes inFieldType,
    FieldTypes outFieldType,
    long inSize,
    long outSize,
    MultiValueModes inMVMode,
    MultiValueModes outMVMode)
  {
    AttributeCheckResult attributeCheckResult = AttributesHelper.CheckMultiValueModes(inMVMode, outMVMode);
    if (attributeCheckResult != AttributeCheckResult.cresOk)
      return attributeCheckResult;
    if (inFieldType == FieldTypes.ftUnknown)
      return AttributeCheckResult.cresLost;
    if (inFieldType != outFieldType)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      List<FieldTypes> convertList = new List<FieldTypes>();
      RelationalOperators[] enabledOperators = (RelationalOperators[]) null;
      bool computableAttribute = false;
      AttributeCacheHelper.GetAttributeTypeValues(inFieldType, -1, ref empty1, ref empty3, ref convertList, ref enabledOperators, ref computableAttribute, ref empty2);
      return convertList.Contains(outFieldType) ? AttributeCheckResult.cresConvert : AttributeCheckResult.cresLost;
    }
    return outFieldType == FieldTypes.ftString && outSize < inSize ? AttributeCheckResult.cresCut : AttributeCheckResult.cresOk;
  }

  private static AttributeCheckResult CheckMultiValueModes(
    MultiValueModes inMVMode,
    MultiValueModes outMVMode)
  {
    return inMVMode == MultiValueModes.SingleValue && (outMVMode == MultiValueModes.SingleValueFromList || outMVMode == MultiValueModes.MultiValuesFromList) || inMVMode == MultiValueModes.MultiValues && outMVMode != MultiValueModes.MultiValues || inMVMode == MultiValueModes.SingleValueFromList && (outMVMode == MultiValueModes.SingleValue || outMVMode == MultiValueModes.MultiValues) || inMVMode == MultiValueModes.MultiValuesFromList && outMVMode != MultiValueModes.MultiValuesFromList ? AttributeCheckResult.cresError : AttributeCheckResult.cresOk;
  }

  private static List<TypeAttribute> GetObjectAttributes(
    int type,
    bool includeManual,
    int[] forbiddenAttributeIDs = null)
  {
    Tuple<int, int[]> key = new Tuple<int, int[]>(type, forbiddenAttributeIDs);
    List<TypeAttribute> objectAttributes;
    if (!AttributesHelper._obligatoryObjectAttribute.TryGetValue(key, out objectAttributes))
    {
      objectAttributes = new Attributes4ObjectReader(type, includeManual, forbiddenAttributeIDs).Read();
      AttributesHelper._obligatoryObjectAttribute.Add(key, objectAttributes);
    }
    return objectAttributes;
  }

  private static void AddObligatoryObjectAttributes(
    IUserSession session,
    IImportedAttributeList importList,
    ImportingAttributable attributable,
    List<TypeAttribute> obligatoryAttributes)
  {
    HashSet<int> intSet = new HashSet<int>(attributable.Attributes.Select<AttributeRecord, int>((Func<AttributeRecord, int>) (item => item.AttributeId)));
    foreach (TypeAttribute obligatoryAttribute in obligatoryAttributes)
    {
      if (!intSet.Contains(obligatoryAttribute.AttributeID))
      {
        switch (obligatoryAttribute.FieldType)
        {
          case FieldTypes.ftString:
            importList.AddAttributeStr(obligatoryAttribute.AttributeID, obligatoryAttribute.DefaultValue);
            continue;
          case FieldTypes.ftInteger:
            if (!string.IsNullOrEmpty(obligatoryAttribute.DefaultValue))
            {
              long result;
              if (long.TryParse(obligatoryAttribute.DefaultValue, out result))
              {
                importList.AddAttributeInt(obligatoryAttribute.AttributeID, result);
                continue;
              }
              importList.AddAttributeNull(obligatoryAttribute.AttributeID);
              continue;
            }
            importList.AddAttributeNull(obligatoryAttribute.AttributeID);
            continue;
          case FieldTypes.ftDouble:
            if (!string.IsNullOrEmpty(obligatoryAttribute.DefaultValue))
            {
              double result;
              if (double.TryParse(obligatoryAttribute.DefaultValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
              {
                importList.AddAttributeDouble(obligatoryAttribute.AttributeID, result);
                continue;
              }
              importList.AddAttributeNull(obligatoryAttribute.AttributeID);
              continue;
            }
            importList.AddAttributeNull(obligatoryAttribute.AttributeID);
            continue;
          case FieldTypes.ftDateTime:
            if (!string.IsNullOrEmpty(obligatoryAttribute.DefaultValue))
            {
              if (obligatoryAttribute.DefaultValue == Consts.CurrentDateFunction)
              {
                importList.AddAttributeDate(obligatoryAttribute.AttributeID, DateTime.UtcNow);
                continue;
              }
              DateTime result;
              if (DateTime.TryParse(obligatoryAttribute.DefaultValue, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
              {
                importList.AddAttributeDate(obligatoryAttribute.AttributeID, result);
                continue;
              }
              importList.AddAttributeNull(obligatoryAttribute.AttributeID);
              continue;
            }
            importList.AddAttributeNull(obligatoryAttribute.AttributeID);
            continue;
          case FieldTypes.ftShortBlob:
          case FieldTypes.ftMemo:
          case FieldTypes.ftBlob:
            importList.AddAttributeNull(obligatoryAttribute.AttributeID);
            continue;
          case FieldTypes.ftObjectLink:
            if (!string.IsNullOrEmpty(obligatoryAttribute.DefaultValue))
            {
              long result;
              if (long.TryParse(obligatoryAttribute.DefaultValue, out result))
              {
                string caption;
                if (!AttributesHelper._objectCaptions.TryGetValue(result, out caption))
                {
                  QuickObjectInfo objectInfo = session.GetObjectInfo(result);
                  if (!objectInfo.Empty)
                  {
                    caption = objectInfo.Caption;
                    AttributesHelper._objectCaptions.Add(result, caption);
                  }
                }
                importList.AddAttributeLink(obligatoryAttribute.AttributeID, result, caption);
                continue;
              }
              importList.AddAttributeNull(obligatoryAttribute.AttributeID);
              continue;
            }
            importList.AddAttributeNull(obligatoryAttribute.AttributeID);
            continue;
          case FieldTypes.ftBoolean:
            if (!string.IsNullOrEmpty(obligatoryAttribute.DefaultValue))
            {
              bool result;
              if (bool.TryParse(obligatoryAttribute.DefaultValue, out result))
              {
                importList.AddAttributeInt(obligatoryAttribute.AttributeID, result ? 1L : 0L);
                continue;
              }
              importList.AddAttributeNull(obligatoryAttribute.AttributeID);
              continue;
            }
            importList.AddAttributeNull(obligatoryAttribute.AttributeID);
            continue;
          case FieldTypes.ftMeasured:
            if (!string.IsNullOrEmpty(obligatoryAttribute.DefaultValue))
            {
              MeasuredValue measuredValue = MeasureHelper.ConvertToMeasuredValue(obligatoryAttribute.DefaultValue);
              if (measuredValue != null)
              {
                MeasuredValue baseMeasure = MeasureHelper.ConvertToBaseMeasure(measuredValue);
                if (CompareValuesHelper.NormalizedValue((object) baseMeasure.Value) != null)
                {
                  importList.AddAttributeMeasure(obligatoryAttribute.AttributeID, baseMeasure.Value, baseMeasure.MeasureID, measuredValue.Caption);
                  continue;
                }
                importList.AddAttributeNull(obligatoryAttribute.AttributeID);
                continue;
              }
              importList.AddAttributeNull(obligatoryAttribute.AttributeID);
              continue;
            }
            importList.AddAttributeNull(obligatoryAttribute.AttributeID);
            continue;
          case FieldTypes.ftGuid:
            if (!string.IsNullOrEmpty(obligatoryAttribute.DefaultValue) && GuidHelper.IsGuid(obligatoryAttribute.DefaultValue))
            {
              importList.AddAttributeStr(obligatoryAttribute.AttributeID, obligatoryAttribute.DefaultValue);
              continue;
            }
            importList.AddAttributeNull(obligatoryAttribute.AttributeID);
            continue;
          default:
            continue;
        }
      }
    }
  }

  public static DateTime CorrectDbDateTimeValue(DateTime dateValue)
  {
    if (dateValue < SqlDateTime.MinValue.Value)
      return SqlDateTime.MinValue.Value;
    return dateValue > SqlDateTime.MaxValue.Value ? SqlDateTime.MaxValue.Value : dateValue;
  }
}
