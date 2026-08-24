// Decompiled with JetBrains decompiler
// Type: ntermech.ImpExp.Interface.Search.SearchHelper
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.ItemsToCreate;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using Intermech.Interfaces.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace ntermech.ImpExp.Interface.Search;

public class SearchHelper
{
  private const string _ruCultureName = "ru-RU";

  public static int PacketSize
  {
    get => ServicesManager.GetService<IConfigurationService>().Configuration.PacketSize;
  }

  /// <summary>Путь к иконкам типов в Search</summary>
  /// <returns></returns>
  public static string GetSearchImagesFolder()
  {
    RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Intermech");
    string searchImagesFolder = registryKey != null ? (string) registryKey.GetValue("IM_Dir") : string.Empty;
    if (searchImagesFolder != string.Empty)
      searchImagesFolder += "\\SEARCH\\images";
    return searchImagesFolder;
  }

  public static string NormalizeName(string name)
  {
    return Regex.Replace(name, "\\s+", string.Empty).Replace('ё', 'е').ToUpper();
  }

  public static void NormalizeObjTypeNames(DataRow row)
  {
    row["F_OBJ_TYPE_NAME"] = (object) SearchHelper.NormalizeName(Convert.ToString(row["F_OBJ_TYPE_NAME"]));
    row["F_OBJ_NAME"] = (object) SearchHelper.NormalizeName(Convert.ToString(row["F_OBJ_NAME"]));
  }

  /// <summary>
  /// Чтение иконки для типа объектов из файла в массив байт
  /// </summary>
  /// <param name="bitmapPath">Путь + имя файла к иконке</param>
  /// <returns></returns>
  public static byte[] GetIcon(string bitmapPath)
  {
    byte[] buffer = (byte[]) null;
    if (new FileInfo(bitmapPath).Exists)
    {
      FileStream fileStream = new FileStream(bitmapPath, FileMode.Open, FileAccess.Read, FileShare.Read);
      try
      {
        buffer = new byte[(int) fileStream.Length];
        fileStream.Read(buffer, 0, buffer.Length);
      }
      catch
      {
        buffer = (byte[]) null;
      }
      finally
      {
        fileStream.Close();
      }
    }
    return buffer;
  }

  public static double GetDoubleValue(string Value)
  {
    CultureInfo provider = CultureInfo.CurrentCulture;
    try
    {
      provider = CultureInfo.GetCultureInfo("ru-RU");
    }
    catch
    {
    }
    return Convert.ToDouble(Value, (IFormatProvider) provider);
  }

  public static DateTime GetDateValue(string Value)
  {
    CultureInfo provider = CultureInfo.CurrentCulture;
    try
    {
      provider = CultureInfo.GetCultureInfo("ru-RU");
    }
    catch
    {
    }
    return Convert.ToDateTime(Value, (IFormatProvider) provider);
  }

  /// <summary>
  /// Найти и если нужно создать атрибут в списке создаваемых атрибутов
  /// </summary>
  /// <param name="attcl">Список атрибутов</param>
  /// <param name="sattrItem"></param>
  /// <param name="attributeName">Имя атрибута</param>
  /// <param name="dBFieldName">Алиас</param>
  /// <param name="attributeType">Тип</param>
  /// <param name="attributeSize">Размер</param>
  /// <param name="attributeGuid">GUID</param>
  /// <param name="defaultValue">Значение по умолчанию</param>
  /// <param name="multiMode"><see cref="T:Intermech.MultiValueModes" /></param>
  /// <returns></returns>
  public static IAttributeTypeToCreate FindAttribute(
    IAttributeTypeToCreateList attcl,
    SettingsAttributeTypeItem sattrItem,
    string attributeName,
    string dBFieldName,
    FieldTypes attributeType,
    int attributeSize,
    Guid attributeGuid,
    object defaultValue,
    MultiValueModes multiMode)
  {
    IAttributeTypeToCreate outAttrType = attcl.GetByName(attributeName);
    if (outAttrType == null && dBFieldName != string.Empty)
      outAttrType = attcl.GetByAlias(dBFieldName);
    if (outAttrType != null)
    {
      ItemErrorType errorType = ItemErrorType.None;
      string[] strArray = AttributeTypeComparer.CompareAttributeType(outAttrType, attributeType, attributeSize, ref errorType);
      switch (errorType)
      {
        case ItemErrorType.None:
          sattrItem.AttrGuid = outAttrType.GUID;
          break;
        case ItemErrorType.Renamed:
          string name = $"{attributeName}({EnumDescConverter.GetEnumDescription((Enum) attributeType)}-{attributeSize})";
          outAttrType = attcl.GetByName(name);
          if (outAttrType == null)
          {
            sattrItem.AttrGuid = attributeGuid;
            string DefaultValue = string.Empty;
            try
            {
              DefaultValue = SearchHelper.FormingDefaultValue(attributeType, Convert.ToString(defaultValue));
            }
            catch (Exception ex)
            {
              strArray = new List<string>((IEnumerable<string>) strArray)
              {
                $"Не удалось привести значение по умолчанию '{defaultValue}' к типу {attributeType}: {ex.Message}"
              }.ToArray();
            }
            outAttrType = attcl.AddItem(true, name, string.Empty, string.Empty, attributeType, (long) attributeSize, sattrItem.AttrGuid, long.MaxValue, false, 0, DefaultValue, multiMode);
            StringBuilder stringBuilder = new StringBuilder("В базе назначения уже присутствует атрибут с таким наименованием. Атрибут был переименован по причине:");
            foreach (string str in strArray)
              stringBuilder.Append(str);
            sattrItem.Error = new ItemError(ItemErrorType.Renamed, stringBuilder.ToString());
            break;
          }
          sattrItem.AttrGuid = outAttrType.GUID;
          sattrItem.Error = new ItemError(ItemErrorType.Renamed, "В базе назначения уже присутствует атрибут с таким наименованием. Был найден аналогичный атрибут.");
          break;
        default:
          sattrItem.Error = new ItemError(errorType, strArray);
          sattrItem.AttrGuid = outAttrType.GUID;
          break;
      }
    }
    else
    {
      sattrItem.AttrGuid = attributeGuid;
      string DefaultValue = string.Empty;
      try
      {
        DefaultValue = SearchHelper.FormingDefaultValue(attributeType, Convert.ToString(defaultValue));
      }
      catch (Exception ex)
      {
        List<string> stringList = new List<string>(1)
        {
          $"Не удалось привести значение по умолчанию '{defaultValue}' к типу {attributeType}: {ex.Message}"
        };
        sattrItem.Error = new ItemError(ItemErrorType.Warning, stringList.ToArray());
      }
      outAttrType = attcl.AddItem(true, attributeName, string.Empty, string.Empty, attributeType, (long) attributeSize, sattrItem.AttrGuid, long.MaxValue, false, 0, DefaultValue, multiMode);
    }
    return outAttrType;
  }

  public static string FormingDefaultValue(FieldTypes fieldType, string DefValue)
  {
    if (DefValue != string.Empty)
    {
      switch (fieldType)
      {
        case FieldTypes.ftInteger:
          return DefValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        case FieldTypes.ftDouble:
          return SearchHelper.GetDoubleValue(DefValue).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        case FieldTypes.ftDateTime:
          return DefValue.Equals(Consts.CurrentDateFunction) ? DefValue : SearchHelper.GetDateValue(DefValue).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
    }
    return DefValue;
  }
}
