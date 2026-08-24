// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpImbaseLinks
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("", "Восстановление внутренних ссылок Imbase")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpImbaseLinks(ImbasePlugin plugin) : PumpImbaseClass(plugin)
{
  internal static Guid _guid = new Guid("{716AFAAF-6CDC-4f89-B649-1F26AE2A42B3}");

  protected override Guid GUID => PumpImbaseLinks._guid;

  public override void Pump()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service.GetCache(ImportingCategory.ImbaseLinksFolder, ImportingCategory.ImbaseFolderKeyToLevel, ImportingCategory.ImbaseLinksTableLinks, ImportingCategory.ImbaseLinksCompleted, ImportingCategory.ImbaseFolders, ImportingCategory.ImbaseTableLinks, ImportingCategory.ImbaseFoldersGuids);
    Dictionary<object, DictionaryValue> category1 = cache.GetCategory(ImportingCategory.ImbaseLinksFolder);
    Dictionary<object, DictionaryValue> category2 = cache.GetCategory(ImportingCategory.ImbaseLinksTableLinks);
    try
    {
      int rowCount = (category1 != null ? category1.Count : 0) + (category2 != null ? category2.Count : 0);
      int rowIndex = 0;
      string message = "Закачка внутренних ссылок IMBASE ({0} из {1})";
      this.PumpLinks(cache, ImportingCategory.ImbaseFolders, category1, message, ref rowIndex, rowCount, 1, 49);
      this.PumpLinks(cache, ImportingCategory.ImbaseTableLinks, category2, message, ref rowIndex, rowCount, 50, 99);
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseLinksFolder, ImportingCategory.ImbaseFolderKeyToLevel, ImportingCategory.ImbaseLinksTableLinks, ImportingCategory.ImbaseLinksCompleted, ImportingCategory.ImbaseFolders, ImportingCategory.ImbaseTableLinks, ImportingCategory.ImbaseFoldersGuids);
    }
    this.PumpCheckPoint("Закачка внутренних ссылок IMBASE успешно завершено", 100);
  }

  private void PumpLinks(
    IImportingData cacheData,
    ImportingCategory category,
    Dictionary<object, DictionaryValue> links,
    string message,
    ref int rowIndex,
    int rowCount,
    int startIdx,
    int endIdx)
  {
    IImportedObjectList iol = this.plugin.Idw.CreateImportedObjectList();
    List<long> importedIDs = new List<long>();
    iol.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
    {
      for (int index = 0; index < iol.Items.Count; ++index)
      {
        if (iol.Items[index] != null)
          cacheData.AddValue(ImportingCategory.ImbaseLinksCompleted, (object) importedIDs[index], 1L);
        else
          this.plugin.Idw.AppManager.AddWarningMessage($"Ссылка на папку Imbase для объекта {importedIDs[index]} не восстановлена!");
      }
      importedIDs.Clear();
    });
    LinkToFolderDecoder linkToFolderDecoder = new LinkToFolderDecoder(cacheData);
    foreach (KeyValuePair<object, DictionaryValue> link in links)
    {
      ++rowIndex;
      this.PumpCheckPoint(string.Format(message, (object) rowIndex, (object) rowCount), this.CalculatePercent(rowCount, rowIndex, startIdx, endIdx));
      long newKey = cacheData.GetNewKey(category, (object) (long) link.Key);
      if (newKey != 0L && cacheData.GetNewKey(ImportingCategory.ImbaseLinksCompleted, (object) newKey) == 0L)
      {
        LinkTag tag = link.Value.Tag as LinkTag;
        importedIDs.Add(newKey);
        iol.UseObject(newKey);
        foreach (Tuple<int, string> tuple in tag.Items)
        {
          if (string.IsNullOrEmpty(tuple.Item2))
          {
            iol.AddAttributeNull(tuple.Item1).IsNew = true;
          }
          else
          {
            DecoderItem decoderItem = new DecoderItem(tuple.Item2);
            if (linkToFolderDecoder.Decode(decoderItem, newKey))
              iol.AddAttributeLink(tuple.Item1, decoderItem.FolderID, decoderItem.FolderCaption).IsNew = true;
            else if (!string.IsNullOrEmpty(decoderItem.ErrorMessage))
              this.plugin.Idw.AppManager.AddWarningMessage(decoderItem.ErrorMessage);
          }
        }
      }
    }
    iol.Import();
  }
}
