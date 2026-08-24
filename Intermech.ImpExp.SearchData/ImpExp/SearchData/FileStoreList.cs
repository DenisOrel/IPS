// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.FileStoreList
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class FileStoreList : Dictionary<string, FileStore>
{
  public new FileStore this[string storeAlias]
  {
    get
    {
      if (this.ContainsKey(storeAlias))
        return base[storeAlias];
      FileStore fileStore = FileStoreList.CreateFileStore(storeAlias);
      if (fileStore != null)
        this.Add(storeAlias, fileStore);
      return fileStore;
    }
  }

  public static FileStore CreateFileStore(string storeAlias)
  {
    AliasInfo ai = (AliasInfo) null;
    if (!PumpHelper.Plugin.AliasInfo.TryGetValue(storeAlias.ToLower(), out ai))
      return (FileStore) null;
    return ai[AliasData.Type] != "IMDOCS" ? new FileStore(ai) : (FileStore) new ImdocsStore(ai);
  }
}
