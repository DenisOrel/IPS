// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.ImportingDataHelper
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal sealed class ImportingDataHelper
{
  private readonly InvalidKeyItems _invalidItems = new InvalidKeyItems();
  private static ImportingDataHelper _instance;

  private void AddWarningMessage(ImportingCategory category, object key, string message)
  {
    if (this._invalidItems.Contains(category, key))
      return;
    this._invalidItems.Add(category, key);
    TechcardConsts.Plugin.appManager.AddNewWarningMessage(message);
  }

  public long GetNewKey(
    IImportingData importingData,
    ImportingCategory category,
    object key,
    bool addWarningMessage = true)
  {
    DictionaryValue dictionaryValue = this.GetValue(importingData, category, key, addWarningMessage);
    return dictionaryValue == null ? 0L : dictionaryValue.NewObjectID;
  }

  public ITagImportObject GetTag(
    IImportingData importingData,
    ImportingCategory category,
    object key,
    bool addWarningMessage = true)
  {
    return this.GetValue(importingData, category, key, addWarningMessage)?.Tag;
  }

  public DictionaryValue GetValue(
    IImportingData importingData,
    ImportingCategory category,
    object key,
    bool addWarningMessage = true)
  {
    if (category == ImportingCategory.None || key == null)
      return (DictionaryValue) null;
    DictionaryValue dictionaryValue = importingData.GetValue(category, key);
    if (dictionaryValue != null || !addWarningMessage)
      return dictionaryValue;
    string message = $"Не найден объект соответствующий категории \"{category}\" код записи \"{key}\"";
    this.AddWarningMessage(category, key, message);
    return (DictionaryValue) null;
  }

  public static ImportingDataHelper Instance
  {
    get
    {
      if (ImportingDataHelper._instance == null)
        ImportingDataHelper._instance = new ImportingDataHelper();
      return ImportingDataHelper._instance;
    }
  }
}
