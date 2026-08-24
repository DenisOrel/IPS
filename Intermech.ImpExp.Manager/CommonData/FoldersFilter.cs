// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.FoldersFilter
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData;

internal class FoldersFilter : IFoldersFilter
{
  private int _attrFilterFolders;

  private int _packageSize
  {
    get
    {
      return (ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize;
    }
  }

  public FoldersFilter()
  {
    this._attrFilterFolders = ((ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo).AttributeTypes.GetByGuid(new Guid("cad0146f-306c-11d8-b4e9-00304f19f545")) ?? throw new Exception("Атрибут \"Данные фильтра папок TECHCARD\" не найден в базе назначения")).ID;
  }

  private IDataReader GetDataReader(IDbConnection dbConnection, string sqlText)
  {
    IDbCommand command = dbConnection.CreateCommand();
    command.CommandText = sqlText;
    return command.ExecuteReader(CommandBehavior.Default);
  }

  public void PumpTCLinks(IDbConnection dbConnection)
  {
    IDataWriter service1 = ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter;
    IMetadataInfo service2 = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    ICache service3 = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service3.GetCache(ImportingCategory.ImbaseFoldersGuids, ImportingCategory.ImbaseFolderFilters);
    IDbCommand command = dbConnection.CreateCommand();
    command.CommandText = "SELECT COUNT(*) FROM TC_LINKS";
    int int32 = Convert.ToInt32(command.ExecuteScalar());
    int countAll = int32 * 2;
    try
    {
      string sql1 = "SELECT F_SPRAVKEY1, F_LEVEL1, F_SPRAVKEY2, F_LEVEL2 FROM TC_LINKS ORDER BY F_SPRAVKEY1, F_LEVEL1";
      this.Import(dbConnection, sql1, cache, service1, service2, 0, countAll, false);
      string sql2 = "SELECT F_SPRAVKEY2, F_LEVEL2, F_SPRAVKEY1, F_LEVEL1 FROM TC_LINKS ORDER BY F_SPRAVKEY2, F_LEVEL2";
      this.Import(dbConnection, sql2, cache, service1, service2, int32, countAll, false);
    }
    finally
    {
      service3?.ReleaseCache(ImportingCategory.ImbaseFoldersGuids, ImportingCategory.ImbaseFolderFilters);
    }
  }

  public void PumpTCUserLinks(IDbConnection dbConnection)
  {
    IDataWriter service1 = ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter;
    IMetadataInfo service2 = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    ICache service3 = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cache = service3.GetCache(ImportingCategory.ImbaseFoldersGuids, ImportingCategory.ImbaseFolderFilters);
    IDbCommand command = dbConnection.CreateCommand();
    command.CommandText = "SELECT COUNT(*) FROM TC_USERLINKS";
    int int32 = Convert.ToInt32(command.ExecuteScalar());
    int countAll = int32 * 2;
    try
    {
      string sql1 = "SELECT F_SPRAVKEY1, F_LEVEL1, F_SPRAVKEY2, F_LEVEL2, F_USERID FROM TC_USERLINKS ORDER BY F_SPRAVKEY1, F_LEVEL1";
      this.Import(dbConnection, sql1, cache, service1, service2, 0, countAll, false);
      string sql2 = "SELECT F_SPRAVKEY2, F_LEVEL2, F_SPRAVKEY1, F_LEVEL1 FROM TC_USERLINKS, F_USERID ORDER BY F_SPRAVKEY2, F_LEVEL2";
      this.Import(dbConnection, sql2, cache, service1, service2, int32, countAll, false);
    }
    finally
    {
      service3?.ReleaseCache(ImportingCategory.ImbaseFoldersGuids, ImportingCategory.ImbaseFolderFilters);
    }
  }

  private long GetCacheKey(int catalogKey, int folderKey)
  {
    return ((long) catalogKey << 32 /*0x20*/) + (long) folderKey;
  }

  private void Import(
    IDbConnection dbConnection,
    string sql,
    IImportingData cacheData,
    IDataWriter dataWriter,
    IMetadataInfo metadataInfo,
    int startIndex,
    int countAll,
    bool withUserID)
  {
    IDataReader dataReader = this.GetDataReader(dbConnection, sql);
    List<string> stringList = new List<string>();
    try
    {
      FoldersFilter.FiltersRec filtersRec = new FoldersFilter.FiltersRec();
      DataTable dataTable = new DataTable();
      dataTable.Columns.Add(new DataColumn("F_GUID", typeof (string)));
      dataTable.Columns.Add(new DataColumn("F_OWNER", typeof (string)));
      dataTable.AcceptChanges();
      List<long> addedFolders = new List<long>(this._packageSize);
      IImportedObjectList iol = dataWriter.CreateImportedObjectList();
      iol.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
      {
        for (int index = 0; index < iol.Items.Count; ++index)
        {
          if (iol.Items[index] == null)
          {
            int num1 = (int) (addedFolders[index] >> 32 /*0x20*/);
            int num2 = (int) (addedFolders[index] & (long) uint.MaxValue);
            if (this.OnMessage != null)
              this.OnMessage((object) this, $"Для папки F_KEY = \"{num2}\" каталога F_KEY = \"{num1}\" не закачаны данные фильтрации для Techcard. См. серверный лог.");
          }
          else
            cacheData.AddValue(ImportingCategory.ImbaseFolderFilters, (object) addedFolders[index], iol.Items[index].Object.Object_id);
        }
        addedFolders.Clear();
      });
      int current = startIndex;
      IPackedStream service = (IPackedStream) ServicesManager.ServiceContainer.GetService(typeof (IPackedStream));
      while (dataReader.Read())
      {
        ++current;
        if (this.OnProgress != null)
          this.OnProgress((object) this, new ProgressInfo(current, countAll));
        int int32_1 = this.ToInt32(dataReader[0]);
        int int32_2 = this.ToInt32(dataReader[1]);
        int int32_3 = this.ToInt32(dataReader[2]);
        int int32_4 = this.ToInt32(dataReader[3]);
        int userID = withUserID ? this.ToInt32(dataReader[4]) : -1;
        long cacheKey = this.GetCacheKey(int32_1, int32_2);
        if (cacheData.GetNewKey(ImportingCategory.ImbaseFolderFilters, (object) cacheKey) == 0L)
        {
          if (filtersRec.IsEmpty)
          {
            filtersRec = new FoldersFilter.FiltersRec(int32_1, int32_2);
            filtersRec.Folders.Add(new FoldersFilter.FolderInfo(int32_3, int32_4, userID));
          }
          else if (filtersRec.Catalog != int32_1 || filtersRec.Folder != int32_2)
          {
            long newKey = cacheData.GetNewKey(ImportingCategory.ImbaseFoldersGuids, (object) this.GetCacheKey(filtersRec.Catalog, filtersRec.Folder));
            if (newKey != 0L)
            {
              addedFolders.Add(cacheKey);
              iol.UseObject(newKey);
              DataTable graph = dataTable.Clone();
              foreach (FoldersFilter.FolderInfo folder in filtersRec.Folders)
              {
                DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ImbaseFoldersGuids, (object) this.GetCacheKey(folder.Catalog, folder.Folder));
                if (dictionaryValue != null)
                {
                  DataRow row = graph.NewRow();
                  row["F_GUID"] = (object) dictionaryValue.Caption;
                  if (withUserID)
                  {
                    Guid guid = metadataInfo.ImportedUsers.GetGUID(folder.UserID);
                    if (guid != Guid.Empty)
                      row["F_OWNER"] = (object) Convert.ToString((object) guid);
                    else if (this.OnMessage != null)
                      this.OnMessage((object) this, $"Пользователь \"{folder.UserID}\" не был закачан. Невозможно включить его в данные по фильтрации для Techcard.");
                  }
                  graph.Rows.Add(row);
                }
                else if (this.OnMessage != null)
                  this.OnMessage((object) this, $"Папка F_KEY = \"{folder.Folder}\" каталога F_KEY = \"{folder.Catalog}\" не была закачана. Невозможно включить ее в данные по фильтрации для Techcard.");
              }
              graph.AcceptChanges();
              string str = Path.Combine(Path.GetTempPath(), $"ftrf_{newKey}.tmp");
              long fileSize = 0;
              using (MemoryStream memoryStream = new MemoryStream())
              {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                graph.RemotingFormat = SerializationFormat.Binary;
                FileStream outStream = new FileStream(str, FileMode.Create, FileAccess.Write);
                try
                {
                  binaryFormatter.Serialize((Stream) memoryStream, (object) graph);
                  memoryStream.Position = 0L;
                  service.PackStream((Stream) outStream, (Stream) memoryStream, 9);
                }
                finally
                {
                  outStream.Flush();
                  fileSize = outStream.Length;
                  outStream.Close();
                  stringList.Add(str);
                }
              }
              iol.AddAttributeBlob(this._attrFilterFolders, str, fileSize, string.Empty, ArcMethods.ZLibPacked);
            }
            else if (this.OnMessage != null)
              this.OnMessage((object) this, $"Папка F_KEY = \"{filtersRec.Folder}\" каталога F_KEY = \"{filtersRec.Catalog}\" не была закачана. Невозможно закачать данные по фильтрации для Techcard.");
            filtersRec = new FoldersFilter.FiltersRec(int32_1, int32_2);
            filtersRec.Folders.Add(new FoldersFilter.FolderInfo(int32_3, int32_4, userID));
          }
          else
            filtersRec.Folders.Add(new FoldersFilter.FolderInfo(int32_3, int32_4, userID));
        }
      }
      iol.Import();
    }
    finally
    {
      dataReader.Close();
      foreach (string str in stringList)
      {
        if (new FileInfo(str).Exists)
          File.Delete(str);
      }
    }
  }

  private int ToInt32(object obj) => DBNull.Value.Equals(obj) ? 0 : Convert.ToInt32(obj);

  public event Intermech.ImpExp.Interface.ProgressEventHandler OnProgress;

  public event MessageEventHandler OnMessage;

  private class FolderInfo
  {
    public int Catalog;
    public int Folder;
    public int UserID;

    public FolderInfo(int catalog, int folder, int userID)
    {
      this.Catalog = catalog;
      this.Folder = folder;
      this.UserID = userID;
    }
  }

  private class FiltersRec
  {
    public int Catalog;
    public int Folder;
    public bool IsEmpty;
    public List<FoldersFilter.FolderInfo> Folders;

    public FiltersRec() => this.IsEmpty = true;

    public FiltersRec(int catalog, int folder)
    {
      this.Catalog = catalog;
      this.Folder = folder;
      this.IsEmpty = false;
      this.Folders = new List<FoldersFilter.FolderInfo>();
    }
  }
}
