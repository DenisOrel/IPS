// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ImbaseImportHelper
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Globalization;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class ImbaseImportHelper
{
  /// <summary>Обработчик ключа Imbase</summary>
  /// <param name="iol">Коллекция импортируемых объектов. AddObject или UseObject для текущего объекта уже должны быть вызваны.</param>
  /// <param name="attributeImbaseLinkID">Идентификатор атрибута "Ссылка на объект IMBASE" "cad00209-306c-11d8-b4e9-00304f19f545"</param>
  /// <param name="attributeImbaseKeyID">Идентификатор атрибута "Код IMBASE" "cad0020f-306c-11d8-b4e9-00304f19f545"</param>
  /// <param name="imbaseKey">Ключ IMBASE вида I6CCCCCCRRRRRRTTTTTT</param>
  /// <param name="cacheData">Кэш</param>
  /// <returns></returns>
  public static bool ImbaseKeyHandler(
    IImportedObjectList iol,
    int attributeImbaseLinkID,
    int attributeImbaseKeyID,
    string imbaseKey,
    IImportingData cacheData)
  {
    long imbaseTableRefObjectId;
    long imbaseCode;
    string tableRefCaption;
    if (!ImbaseImportHelper.GetImbaseCodes(cacheData, imbaseKey, out imbaseTableRefObjectId, out imbaseCode, out tableRefCaption))
      return false;
    iol.AddAttributeLink(attributeImbaseLinkID, imbaseTableRefObjectId, tableRefCaption);
    iol.AddAttributeInt(attributeImbaseKeyID, imbaseCode);
    return true;
  }

  /// <summary>Анализирует ключ IMBASE</summary>
  /// <param name="cacheData"></param>
  /// <param name="imbaseKey">Ключ IMBASE</param>
  /// <param name="imbaseTableRefObjectId">Идентификатор ярлыка. Записать в атрибут "Ссылка на объект IMBASE" "cad00209-306c-11d8-b4e9-00304f19f545"</param>
  /// <param name="imbaseCode">Код Imbase.Записать в атрибут "Код IMBASE" "cad0020f-306c-11d8-b4e9-00304f19f545" </param>
  /// <param name="tableRefCaption"></param>
  /// <returns>Результат</returns>
  public static bool GetImbaseCodes(
    IImportingData cacheData,
    string imbaseKey,
    out long imbaseTableRefObjectId,
    out long imbaseCode,
    out string tableRefCaption)
  {
    imbaseTableRefObjectId = 0L;
    imbaseCode = 0L;
    tableRefCaption = (string) null;
    if (string.IsNullOrEmpty(imbaseKey) || imbaseKey.Length != 20 || imbaseKey.IndexOf("I6", StringComparison.InvariantCultureIgnoreCase) != 0)
      return false;
    int num1 = int.Parse(imbaseKey.Substring(2, 6), NumberStyles.HexNumber);
    int num2 = int.Parse(imbaseKey.Substring(8, 6), NumberStyles.HexNumber);
    imbaseCode = (long) int.Parse(imbaseKey.Substring(14, 6), NumberStyles.HexNumber);
    DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ImbaseTableLinksKeyToObjectID, (object) (((long) num1 << 32 /*0x20*/) + (long) num2));
    if (dictionaryValue == null)
      return false;
    imbaseTableRefObjectId = dictionaryValue.NewObjectID;
    tableRefCaption = dictionaryValue.Caption;
    return true;
  }
}
