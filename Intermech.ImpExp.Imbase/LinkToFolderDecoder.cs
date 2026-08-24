// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.LinkToFolderDecoder
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using System;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal sealed class LinkToFolderDecoder
{
  private string _folderPathPattern = "(?<catalog>\\d+)\\.(?<folder>\\d+)\\|\\\\\\\\(?<path>.+)";
  private string _folderNamePattern = "\\\\(?<folderName>[\\w\\s]+)$";
  private string _groupCatalog = "catalog";
  private string _groupFolder = "folder";
  private string _groupFolderName = "folderName";
  private string _groupPath = "path";
  private IImportingData _cacheData;

  public LinkToFolderDecoder(IImportingData cacheData) => this._cacheData = cacheData;

  public bool Decode(DecoderItem item, long objectID)
  {
    if (item == null || string.IsNullOrEmpty(item.EncodedValue))
      return false;
    Match match1 = new Regex(this._folderPathPattern).Match(item.EncodedValue);
    int result1 = 0;
    int result2 = 0;
    if (match1.Groups[this._groupCatalog] != null && int.TryParse(match1.Groups[this._groupCatalog].Value, out result1) && match1.Groups[this._groupFolder] != null && int.TryParse(match1.Groups[this._groupFolder].Value, out result2) && match1.Groups[this._groupPath] != null)
    {
      if (match1.Groups[this._groupPath].Length > 0)
      {
        try
        {
          long newKey1 = this._cacheData.GetNewKey(ImportingCategory.ImbaseFolderKeyToLevel, (object) this.GetCatalogCacheKey(result1, result2));
          if (newKey1 == 0L)
          {
            item.ErrorMessage = $"Невозможно восстановить ссылку на папку Imbase (каталог {result1}, папка F_KEY={result2}) для объекта {objectID}. Папка не найдена среди закачанных!";
            return false;
          }
          long newKey2 = this._cacheData.GetNewKey(ImportingCategory.ImbaseFolders, (object) newKey1);
          if (newKey2 == 0L)
          {
            item.ErrorMessage = $"Невозможно восстановить ссылку на папку Imbase (каталог {result1}, папка F_KEY={result2}) для объекта {objectID}. Папка не найдена среди закачанных!";
            return false;
          }
          item.FolderID = newKey2;
          string caption = this._cacheData.GetCaption(ImportingCategory.ImbaseFoldersGuids, (object) newKey1);
          if (!string.IsNullOrEmpty(caption) && GuidHelper.IsGuid(caption))
            item.FolderGuid = new Guid(caption);
          Match match2 = new Regex(this._folderNamePattern).Match(match1.Groups[this._groupPath].Value.TrimEnd());
          if (match2.Groups[this._groupFolderName] != null && match2.Groups[this._groupFolderName].Length > 0)
            item.FolderCaption = match2.Groups[this._groupFolderName].Value;
          return true;
        }
        catch (Exception ex)
        {
          item.ErrorMessage = $"Невозможно восстановить ссылку на папку Imbase (каталог {result1}, папка F_KEY={result2}) для объекта {objectID}: {ex.Message}";
          return false;
        }
      }
    }
    return false;
  }

  private long GetCatalogCacheKey(int parentKey, int level)
  {
    return ((long) parentKey << 32 /*0x20*/) + (long) level;
  }
}
