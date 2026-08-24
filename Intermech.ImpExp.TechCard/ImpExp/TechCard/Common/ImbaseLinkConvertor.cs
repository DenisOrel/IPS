// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.ImbaseLinkConvertor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers;
using Intermech.ImpExp.TechCard.Pumpers.MetaData.TablesPump;
using Intermech.ImpExp.TechCard.TechExpPump.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal class ImbaseLinkConvertor
{
  private readonly Dictionary<long, DictionaryValue> _imbaseHashCache = new Dictionary<long, DictionaryValue>();
  private readonly ImbaseInvalidLinkItems _imbaseInvalidItems = new ImbaseInvalidLinkItems();
  private Lazy<ImTableInfo> _cehTableInfo = new Lazy<ImTableInfo>((Func<ImTableInfo>) (() =>
  {
    ImTableInfo imTableInfo = TechPumpData.Tables.ImTablesData?.GetTableInfo(TechcardConsts.imTablesConsts.Ceh);
    if (imTableInfo == null)
    {
      TechcardConsts.Plugin.appManager.AddErrorMessage("Внимание! Таблица справочника \"Цеха\" не найдена!!");
      imTableInfo = new ImTableInfo(-1, string.Empty, 0, string.Empty);
    }
    return imTableInfo;
  }));
  private static ImbaseLinkConvertor _instance;

  private void AddWarningMessage(int catalogKey, int folderKey, string message)
  {
    if (this._imbaseInvalidItems.Contains(catalogKey, folderKey))
      return;
    this._imbaseInvalidItems.Add(catalogKey, folderKey);
    TechcardConsts.Plugin.appManager.AddNewWarningMessage(message);
  }

  private ImbaseLinkConvertor()
  {
  }

  public DictionaryValue ConvertValue(
    Entity entity,
    int value,
    IImportingData importingData,
    bool throwException = false)
  {
    if (entity == null || value == 0)
      return (DictionaryValue) null;
    if (importingData == null)
      throw new ArgumentNullException(nameof (importingData));
    if (entity.EntityReference == null)
    {
      string message = $"Не найден справочник IMBASE соответствующий понятию \"{entity.Code}\" ({entity.Name})";
      if (throwException)
        throw new ObjectLinkTypeConvertException(message);
      TechcardConsts.Plugin.appManager.AddNewWarningMessage(message);
      return (DictionaryValue) null;
    }
    int reference = entity.EntityReference.Reference;
    if (reference == 0)
    {
      string message = $"Для понятия \"{entity.Code}\" не найдена привязка к справочнику IMBASE";
      if (throwException)
        throw new ObjectLinkTypeConvertException(message);
      TechcardConsts.Plugin.appManager.AddNewWarningMessage(message);
      return (DictionaryValue) null;
    }
    if (TechCardPlugin.Configuration.ImbaseCeh2ObjectPumpMode && entity.EntityReference.Reference == this._cehTableInfo.Value.TableKey)
      return importingData.GetValue(ImportingCategory.TechCeh, (object) value);
    long num = TechcardConsts.Utils.CodeHashCode(reference, value);
    DictionaryValue dictionaryValue;
    if (this._imbaseHashCache.TryGetValue(num, out dictionaryValue))
      return dictionaryValue;
    dictionaryValue = importingData.GetValue(ImportingCategory.ImbaseFolders, (object) num);
    this._imbaseHashCache[num] = dictionaryValue;
    if (dictionaryValue != null)
      return dictionaryValue;
    string message1 = $"Для понятия \"{entity.Code}\" не найден объект IPS IMBASE соответствующий справочнику \"{reference}\" записи \"{value}\"";
    if (throwException)
      throw new ObjectLinkTypeConvertException(message1);
    this.AddWarningMessage(reference, value, message1);
    return new DictionaryValue(0L, string.Empty, (ITagImportObject) null);
  }

  public static ImbaseLinkConvertor Instance
  {
    get
    {
      if (ImbaseLinkConvertor._instance == null)
        ImbaseLinkConvertor._instance = new ImbaseLinkConvertor();
      return ImbaseLinkConvertor._instance;
    }
  }
}
