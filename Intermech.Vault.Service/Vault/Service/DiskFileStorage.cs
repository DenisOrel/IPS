// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.DiskFileStorage
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Vault.Interfaces;
using Intermech.Vault.Interfaces.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Xml;

#nullable disable
namespace Intermech.Vault.Service;

public class DiskFileStorage : 
  MarshalByRefObject,
  IDiskFileStorage,
  IDisposable,
  IFileProcReader2,
  IFileProcWriter2
{
  private static EventWaitHandle FinishReading = (EventWaitHandle) new AutoResetEvent(false);
  private List<TransactionClass> transactionStack = new List<TransactionClass>();
  private int connectionID;
  internal object SyncRoot = new object();
  private string storageGUID;
  private string storageName;
  private string storagePath;
  private string workingFolderPath;
  private string historyFolderPath;
  private string deletedFolderPath;
  private string tempFolderPath;
  private string dbFilePath;
  private DateTime startTransactionTime;
  private string computerName;
  private string userName;
  private DBManager manager;
  private SQLiteConnection connection;
  private string connectionString;
  private bool inTransaction;
  private short maxPercent;
  private SQLiteCommand command;

  public short MaxPercent
  {
    get => this.maxPercent;
    set => this.maxPercent = value;
  }

  public DateTime StartTransactionTime
  {
    get => this.startTransactionTime;
    set => this.startTransactionTime = value;
  }

  public string СomputerName
  {
    get => this.computerName;
    set => this.computerName = value;
  }

  public string UserName
  {
    get => this.userName;
    set => this.userName = value;
  }

  public string StoragePath
  {
    get => this.storagePath;
    set
    {
      this.storagePath = value;
      this.workingFolderPath = Path.Combine(this.storagePath, CommonVariables.WORKING_FOLDER_NAME);
      this.historyFolderPath = Path.Combine(this.storagePath, CommonVariables.HISTORY_FOLDER_NAME);
      this.deletedFolderPath = Path.Combine(this.storagePath, CommonVariables.DELETED_FOLDER_NAME);
      this.tempFolderPath = Path.Combine(this.storagePath, CommonVariables.TEMP_FOLDER_NAME);
      this.dbFilePath = Path.Combine(this.storagePath, this.storageGUID + ".db3");
      this.connectionString = $"Data Source={this.dbFilePath}; Pooling=true; Min Pool Size=5; Max Pool Size=25;";
      this.manager = DBManagerHolder.CreateConnection(this.dbFilePath);
    }
  }

  public bool InTransaction
  {
    set
    {
      lock (this.SyncRoot)
      {
        this.inTransaction = value;
        if (!this.inTransaction)
          return;
        this.startTransactionTime = DateTime.UtcNow;
      }
    }
    get
    {
      lock (this.SyncRoot)
        return this.inTransaction;
    }
  }

  public int ConnectionID
  {
    get => this.connectionID;
    set => this.connectionID = value;
  }

  public string StorageGUID => this.storageGUID;

  public string StorageName
  {
    get => this.storageName;
    set => this.storageName = value;
  }

  private DiskFileStorage()
  {
  }

  internal static DiskFileStorage Login(
    string storageGuid,
    string storageName,
    string password,
    string mName)
  {
    return DiskFileStorage.Login(storageGuid, storageName, password, mName, true, true);
  }

  private static DiskFileStorage Login(
    string storageGuid,
    string storageName,
    string password,
    string mName,
    bool checkLogin,
    bool checkFolderExists)
  {
    DiskFileStorage addedFileStorage = new DiskFileStorage();
    if (checkLogin && !password.Equals(CommonVariables.Password))
      throw new VaultException(string.Format(EventStringMessage.CANNOT_LOGIN, (object) storageGuid));
    addedFileStorage.StartTransactionTime = DateTime.UtcNow;
    addedFileStorage.СomputerName = mName;
    addedFileStorage.storageGUID = storageGuid;
    if (checkFolderExists)
    {
      RootDirectory rootDirectory = CommonVariables.GetRootDirectory(storageName, storageGuid);
      addedFileStorage.StorageName = rootDirectory.StorageName;
      addedFileStorage.StoragePath = rootDirectory.Path;
      addedFileStorage.MaxPercent = rootDirectory.MaxSize;
    }
    addedFileStorage.ConnectionID = DiskFileStorageCollection.AddStorageConnection((IDiskFileStorage) addedFileStorage);
    ApplicationEventLog.LogginEventWrite(string.Format(EventStringMessage.LOG_IN_STORAGE, (object) mName, (object) storageGuid));
    return addedFileStorage;
  }

  internal static DiskFileStorage CreateStorage(
    string storageGuid,
    string storageName,
    string password,
    string mName)
  {
    ApplicationEventLog.LogginEventWrite(string.Format(EventStringMessage.CREATE_STORAGE, (object) mName, (object) storageGuid));
    if (!password.Equals(CommonVariables.Password))
      throw new VaultException(string.Format(EventStringMessage.INVALID_PASSWORD, (object) storageGuid));
    RootDirectory rootDirectory = CommonVariables.GetRootDirectory(storageName, storageGuid);
    if (rootDirectory.Guid != string.Empty)
    {
      if (!(rootDirectory.Guid == storageGuid))
        throw new VaultException(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_28"));
      ApplicationEventLog.Log.InfoFormat(EventStringMessage.STORAGE_ALREADY_EXISTS, (object) storageGuid);
      return DiskFileStorage.Login(storageGuid, storageName, password, mName, false, true);
    }
    DiskFileStorage storage = DiskFileStorage.Login(storageGuid, storageName, password, mName, false, false);
    storage.StorageName = storageName;
    storage.StoragePath = rootDirectory.Path;
    storage.CreateStorageHierarchy();
    rootDirectory.Guid = storageGuid;
    XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage(CommonVariables.XmlFilePath);
    xmlSettingsStorage.SetAttributeValue(xmlSettingsStorage.FindNodeWithAttr(xmlSettingsStorage.FindNode((XmlNode) xmlSettingsStorage.document.DocumentElement, "root_folders", false), "storage", "path", rootDirectory.Path, false), "guid", storageGuid);
    xmlSettingsStorage.Save(CommonVariables.XmlFilePath);
    return storage;
  }

  protected void CreateStorageHierarchy()
  {
    try
    {
      DirectoryInfo directory = Directory.CreateDirectory(this.storagePath);
      StorageSecurity.SetRootSecurity(directory);
      DirectoryInfo subdirectory1 = directory.CreateSubdirectory(CommonVariables.WORKING_FOLDER_NAME);
      StorageSecurity.RemoveDirectoryDeleteRights(subdirectory1.FullName);
      StorageSecurity.RemoveDirectoryDeleteRights(subdirectory1.CreateSubdirectory(CommonVariables.VOLUME_NAME + "0").FullName);
      DirectoryInfo subdirectory2 = directory.CreateSubdirectory(CommonVariables.HISTORY_FOLDER_NAME);
      StorageSecurity.RemoveDirectoryDeleteRights(subdirectory2.FullName);
      StorageSecurity.RemoveDirectoryDeleteRights(subdirectory2.CreateSubdirectory(CommonVariables.VOLUME_NAME + "0").FullName);
      DirectoryInfo subdirectory3 = directory.CreateSubdirectory(CommonVariables.DELETED_FOLDER_NAME);
      StorageSecurity.RemoveDirectoryDeleteRights(subdirectory3.FullName);
      StorageSecurity.RemoveDirectoryDeleteRights(subdirectory3.CreateSubdirectory(CommonVariables.VOLUME_NAME + "0").FullName);
      StorageSecurity.RemoveDirectoryDeleteRights(directory.CreateSubdirectory(CommonVariables.TEMP_FOLDER_NAME).FullName);
      SQLiteConnection.CreateFile(this.dbFilePath);
      StorageSecurity.RemoveFileDeleteRights(this.dbFilePath);
      this.manager.StartTransaction(this.transactionStack);
      this.manager.CreateDBStorageHierarchy();
      this.manager.Commit();
    }
    catch (Exception ex)
    {
      this.manager.Rollback();
      GC.Collect();
      if (Directory.Exists(this.storagePath))
      {
        StorageSecurity.AddDirectoryDeleteRights(this.storagePath);
        try
        {
          Directory.Delete(this.storagePath, true);
        }
        catch
        {
        }
      }
      throw new VaultException(string.Format(EventStringMessage.CREATE_STORAGE_ERROR, (object) this.computerName, (object) this.storageGUID, (object) ex.Message), ex);
    }
  }

  public void Logout()
  {
    ApplicationEventLog.Log.InfoFormat(EventStringMessage.LOGOUT_FROM_STORAGE, (object) this.computerName, (object) this.userName, (object) this.storageGUID);
    if (this.inTransaction)
      this.Rollback();
    DBManagerHolder.RemoveConnection(this.storageGUID, this.StorageName);
    this.manager = (DBManager) null;
    DiskFileStorageCollection.DeleteStorageConnection(this.connectionID);
    this.connectionID = 0;
    GC.Collect();
  }

  public void DeleteStorage()
  {
    lock (DiskFileStorageCollection.SyncRoot)
    {
      if (!DiskFileStorageCollection.CheckLogin(this.connectionID))
        throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
      ApplicationEventLog.Log.InfoFormat(EventStringMessage.DELETE_STORAGE, (object) this.userName, (object) this.storageName);
      this.transactionStack.Add(new TransactionClass(TransactionType.DeleteStorage));
    }
  }

  public FileInformation GetFileInformation(long blobID)
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    ApplicationEventLog.Log.DebugFormat("blobID={0}", (object) blobID);
    FileInformation fileInformation = new FileInformation();
    try
    {
      this.OpenConnection();
      SQLiteParameter[] parameters = new SQLiteParameter[1]
      {
        new SQLiteParameter("@blobid", (object) blobID)
      };
      if (this.inTransaction)
      {
        foreach (TransactionClass transaction in this.transactionStack)
        {
          if ((transaction.ActionType == TransactionType.AddFile || transaction.ActionType == TransactionType.AddFileInfo) && transaction.fileInfo.BlobID == blobID)
          {
            fileInformation.Folder = transaction.FileNameBeforeCommit;
            fileInformation.ID = transaction.fileInfo.ID;
            fileInformation.BlobID = transaction.fileInfo.BlobID;
            fileInformation.ObjectID = transaction.fileInfo.ObjectID;
            fileInformation.HistoryID = transaction.fileInfo.HistoryID;
            fileInformation.Name = transaction.fileInfo.Name;
            fileInformation.FileDate = transaction.fileInfo.FileDate;
            fileInformation.ArcMethod = transaction.fileInfo.ArcMethod;
            fileInformation.PacketFileSize = transaction.fileInfo.PacketFileSize;
            fileInformation.RealSize = transaction.fileInfo.RealSize;
            fileInformation.Note = transaction.fileInfo.Note;
            return fileInformation;
          }
        }
      }
      using (this.command = this.CreateCommand(SQLCommands.SelectFileInfo, parameters))
      {
        using (SQLiteDataReader sqLiteDataReader = this.command.ExecuteReader(CommandBehavior.SingleRow))
        {
          fileInformation.Volume = sqLiteDataReader.Read() ? sqLiteDataReader.GetString(2) : throw new Exception(string.Format(EventStringMessage.CANNOT_FIND_FILE, (object) this.userName, (object) this.storageName, (object) blobID));
          fileInformation.ID = sqLiteDataReader.GetInt64(4);
          fileInformation.BlobID = sqLiteDataReader.GetInt64(5);
          fileInformation.ObjectID = sqLiteDataReader.GetInt64(6);
          fileInformation.Name = sqLiteDataReader.GetString(8);
          DateTime dateTime = sqLiteDataReader.GetDateTime(9);
          fileInformation.FileDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
          fileInformation.UserName = sqLiteDataReader.GetString(10);
          fileInformation.ArcMethod = (ArcMethods) Convert.ToInt32(sqLiteDataReader["F_ARC_METHOD"]);
          fileInformation.PacketFileSize = sqLiteDataReader.GetInt64(13);
          fileInformation.RealSize = sqLiteDataReader.GetInt64(14);
          fileInformation.Note = Convert.ToString(sqLiteDataReader["F_NOTE"]);
          fileInformation.HistoryID = sqLiteDataReader.GetInt32(7);
          fileInformation.Folder = this.workingFolderPath;
        }
      }
      SQLiteParameter[] sqLiteParameterArray = new SQLiteParameter[1]
      {
        new SQLiteParameter("@volume", (object) fileInformation.Volume)
      };
    }
    catch (Exception ex)
    {
      throw new VaultException(string.Format(EventStringMessage.FILE_READ_ERROR, (object) this.userName, (object) this.storageName, (object) blobID, (object) ex.Message), ex);
    }
    finally
    {
      this.CloseConnection();
    }
    return fileInformation;
  }

  public FileInformation GetFileHistoryInformation(int historyID, long objectID)
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    this.OpenConnection();
    FileInformation historyInformation;
    try
    {
      SQLiteParameter[] parameters = new SQLiteParameter[2]
      {
        new SQLiteParameter("@objectID", (object) objectID),
        new SQLiteParameter("@historyID", (object) historyID)
      };
      using (this.command = this.CreateCommand(SQLCommands.SelectFileForObjectID, parameters))
      {
        using (SQLiteDataReader sqLiteDataReader = this.command.ExecuteReader(CommandBehavior.CloseConnection))
        {
          if (sqLiteDataReader.Read())
          {
            historyInformation = new FileInformation();
            historyInformation.Folder = sqLiteDataReader.GetString(1);
            historyInformation.Volume = sqLiteDataReader.GetString(2);
            historyInformation.ID = sqLiteDataReader.GetInt64(4);
            historyInformation.BlobID = sqLiteDataReader.GetInt64(5);
            historyInformation.ObjectID = sqLiteDataReader.GetInt64(6);
            historyInformation.HistoryID = sqLiteDataReader.GetInt32(7);
            historyInformation.Name = sqLiteDataReader.GetString(8);
            DateTime dateTime = sqLiteDataReader.GetDateTime(9);
            historyInformation.FileDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
            historyInformation.UserName = sqLiteDataReader.GetString(10);
            historyInformation.ArcMethod = (ArcMethods) Convert.ToInt32(sqLiteDataReader["F_ARC_METHOD"]);
            historyInformation.PacketFileSize = sqLiteDataReader.GetInt64(13);
            historyInformation.RealSize = sqLiteDataReader.GetInt64(14);
            historyInformation.Note = Convert.ToString(sqLiteDataReader["F_NOTE"]);
            switch (historyInformation.Folder)
            {
              case "work":
                historyInformation.Folder = this.workingFolderPath;
                break;
              case "history":
                historyInformation.Folder = this.historyFolderPath;
                break;
              case "deleted":
                historyInformation.Folder = this.deletedFolderPath;
                break;
            }
          }
          else
            throw new VaultException(string.Format(EventStringMessage.CANNOT_FIND_HISTORY_FILE, (object) this.userName, (object) this.storageName, (object) objectID, (object) historyID));
        }
      }
    }
    finally
    {
      this.CloseConnection();
    }
    return historyInformation;
  }

  private void UnlockReadingFile(long blobID, int historyID)
  {
    try
    {
      this.manager.RemoveBlockedFiles(blobID, historyID);
    }
    catch (Exception ex)
    {
      throw new VaultException(string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_29"), (object) this.userName, (object) this.storageName, (object) blobID, (object) ex.Message), ex);
    }
  }

  public void DeleteFile(long blobID)
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    FileInformation fileInformation = new FileInformation();
    this.transactionStack.Add(new TransactionClass(TransactionType.DeleteFile, new FileInformation()
    {
      BlobID = blobID,
      UserName = this.userName
    }));
    ApplicationEventLog.Log.InfoFormat(EventStringMessage.DELETE_FILE, (object) this.userName, (object) blobID, (object) this.storageName);
  }

  public void ChangeObjectLinkID(FileInformation fileInfo)
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    this.transactionStack.Add(new TransactionClass(TransactionType.UpdateFileInfo, fileInfo));
    ApplicationEventLog.Log.InfoFormat(EventStringMessage.UPDATE_FILE, (object) this.userName, (object) fileInfo.BlobID, (object) this.storageName);
  }

  public DataTable GetVersionHistory(long objectID)
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    DataTable versionHistory = new DataTable();
    this.OpenConnection();
    try
    {
      SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter();
      SQLiteParameter[] parameters = new SQLiteParameter[1]
      {
        new SQLiteParameter("@objectID", (object) objectID.ToString())
      };
      sqLiteDataAdapter.SelectCommand = this.CreateCommand(SQLCommands.SelectVersionHistory, parameters);
      sqLiteDataAdapter.Fill(versionHistory);
      this.FillColumsCaption(versionHistory);
      ApplicationEventLog.Log.InfoFormat(EventStringMessage.GET_FILE_INFO, (object) this.userName, (object) this.userName, (object) objectID);
    }
    catch (Exception ex)
    {
      throw new VaultException(string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_30"), (object) this.userName, (object) this.userName, (object) objectID, (object) ex.Message), ex);
    }
    finally
    {
      this.CloseConnection();
    }
    return versionHistory;
  }

  public DataTable GetObjectHistory(long id)
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter();
    DataTable objectHistory = new DataTable();
    this.OpenConnection();
    try
    {
      SQLiteParameter[] parameters = new SQLiteParameter[1]
      {
        new SQLiteParameter("@id", (object) id.ToString())
      };
      sqLiteDataAdapter.SelectCommand = this.CreateCommand(SQLCommands.SelectObjectHistory, parameters);
      sqLiteDataAdapter.Fill(objectHistory);
      this.FillColumsCaption(objectHistory);
    }
    catch (Exception ex)
    {
      throw new VaultException(string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_31"), (object) ex.Message), ex);
    }
    finally
    {
      this.CloseConnection();
    }
    ApplicationEventLog.Log.InfoFormat(EventStringMessage.GET_FILE_INFO, (object) this.userName, (object) this.storageName, (object) id);
    return objectHistory;
  }

  public DataTable GetHistoryForFile(long blobID, long id)
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter();
    DataTable historyForFile = new DataTable();
    this.OpenConnection();
    try
    {
      SQLiteParameter[] parameters = new SQLiteParameter[2]
      {
        new SQLiteParameter("@blobID", (object) blobID),
        new SQLiteParameter("@id", (object) id)
      };
      sqLiteDataAdapter.SelectCommand = this.CreateCommand(SQLCommands.SelectHistoryForBlobID, parameters);
      sqLiteDataAdapter.Fill(historyForFile);
      this.FillColumsCaption(historyForFile);
    }
    catch (Exception ex)
    {
      throw new VaultException(string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_32"), (object) blobID, (object) ex.Message), ex);
    }
    finally
    {
      this.CloseConnection();
    }
    ApplicationEventLog.Log.InfoFormat(EventStringMessage.GET_FILE_INFO, (object) this.userName, (object) this.storageName, (object) id);
    return historyForFile;
  }

  public DataTable GetHistoryForFile(string fileName, long id)
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter();
    DataTable historyForFile = new DataTable();
    this.OpenConnection();
    try
    {
      SQLiteParameter[] parameters = new SQLiteParameter[2]
      {
        new SQLiteParameter("@fileName", (object) fileName),
        new SQLiteParameter("@id", (object) id)
      };
      sqLiteDataAdapter.SelectCommand = this.CreateCommand(SQLCommands.SelectHistoryForFileName, parameters);
      sqLiteDataAdapter.Fill(historyForFile);
      this.FillColumsCaption(historyForFile);
    }
    catch (Exception ex)
    {
      throw new VaultException(string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_33"), (object) fileName, (object) ex.Message), ex);
    }
    finally
    {
      this.CloseConnection();
    }
    ApplicationEventLog.Log.InfoFormat(EventStringMessage.GET_FILE_INFO, (object) this.userName, (object) this.storageName, (object) id);
    return historyForFile;
  }

  public DataTable GetStorageInfo()
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    DataTable storageInfo = new DataTable();
    storageInfo.Columns.AddRange(new DataColumn[9]
    {
      new DataColumn("Working files amount"),
      new DataColumn("Working folder packed size"),
      new DataColumn("Working folder real size"),
      new DataColumn("History files amount"),
      new DataColumn("History folder packed size"),
      new DataColumn("History folder real size"),
      new DataColumn("Deleted files amount"),
      new DataColumn("Deleted folder packed size"),
      new DataColumn("Deleted folder real size")
    });
    storageInfo.Rows.Add();
    this.OpenConnection();
    try
    {
      using (this.command = this.CreateCommand(SQLCommands.SelectStorageInfo))
      {
        using (SQLiteDataReader sqLiteDataReader = this.command.ExecuteReader(CommandBehavior.SingleRow))
        {
          if (sqLiteDataReader.Read())
          {
            storageInfo.Rows[0][0] = sqLiteDataReader[0];
            storageInfo.Rows[0][1] = sqLiteDataReader[1] == DBNull.Value ? (object) 0 : sqLiteDataReader[1];
            storageInfo.Rows[0][2] = sqLiteDataReader[2] == DBNull.Value ? (object) 0 : sqLiteDataReader[2];
            storageInfo.Rows[0][3] = sqLiteDataReader[3];
            storageInfo.Rows[0][4] = sqLiteDataReader[4] == DBNull.Value ? (object) 0 : sqLiteDataReader[4];
            storageInfo.Rows[0][5] = sqLiteDataReader[5] == DBNull.Value ? (object) 0 : sqLiteDataReader[5];
            storageInfo.Rows[0][6] = sqLiteDataReader[6];
            storageInfo.Rows[0][7] = sqLiteDataReader[7] == DBNull.Value ? (object) 0 : sqLiteDataReader[7];
            storageInfo.Rows[0][8] = sqLiteDataReader[8] == DBNull.Value ? (object) 0 : sqLiteDataReader[8];
          }
        }
      }
      ApplicationEventLog.Log.InfoFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_34"), (object) this.userName, (object) this.storageName);
      return storageInfo;
    }
    catch (Exception ex)
    {
      throw new VaultException(string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_35"), (object) this.userName, (object) this.storageName, (object) ex.Message), ex);
    }
    finally
    {
      this.CloseConnection();
    }
  }

  public void DeleteTrash()
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    this.transactionStack.Add(new TransactionClass(TransactionType.PurgeFile));
    ApplicationEventLog.Log.InfoFormat(EventStringMessage.DELETE_TRASH, (object) this.userName, (object) this.storageName);
  }

  public string WriteFileInfo(FileInformation fileInfo)
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    ApplicationEventLog.Log.InfoFormat(EventStringMessage.ADD_FILE, (object) this.userName, (object) this.storageName, (object) fileInfo.BlobID);
    if (fileInfo.PacketFileSize > CommonVariables.MaxVolumeSize)
      throw new VaultException(EventStringMessage.BIG_FILE_SIZE);
    fileInfo.UserName = this.userName;
    fileInfo.MachineName = this.computerName;
    TransactionClass transactionClass = new TransactionClass(!fileInfo.IsStreamEmty ? TransactionType.AddFile : TransactionType.AddFileInfo, fileInfo);
    string str = Path.Combine(this.tempFolderPath, DateTime.Now.Ticks.ToString() + fileInfo.BlobID.ToString());
    transactionClass.FileNameBeforeCommit = str;
    this.transactionStack.Add(transactionClass);
    ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_36"), (object) fileInfo.BlobID, (object) str);
    return str;
  }

  IWriteWorker IFileProcWriter2.OpenFileWriter(string tempFileName)
  {
    return (IWriteWorker) new DiskFileStorage.FileWriteWorker(tempFileName);
  }

  IReadWorker IFileProcReader2.OpenFileReader(FileInformation fileInfo)
  {
    if (this.connectionID <= 0)
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    ApplicationEventLog.Log.InfoFormat(EventStringMessage.READ_FILE, (object) this.userName, (object) this.storageName, (object) fileInfo.BlobID);
    string empty = string.Empty;
    string str;
    bool flag;
    if (string.IsNullOrEmpty(fileInfo.Volume))
    {
      str = fileInfo.Folder;
      flag = File.Exists(str);
    }
    else
    {
      string path1 = Path.Combine(fileInfo.Folder, fileInfo.Volume);
      string path2_1 = $"{fileInfo.ID}_{Math.Abs(fileInfo.ObjectID)}_{fileInfo.BlobID}_{fileInfo.HistoryID}";
      str = Path.Combine(path1, path2_1);
      flag = File.Exists(str);
      if (!flag && fileInfo.ObjectID < 0L && fileInfo.ObjectID < 0L)
      {
        string path2_2 = $"{fileInfo.ID}_{fileInfo.ObjectID}_{fileInfo.BlobID}_{fileInfo.HistoryID}";
        str = Path.Combine(path1, path2_2);
        flag = File.Exists(str);
      }
    }
    if (!flag)
      throw new VaultException(string.Format(EventStringMessage.CANNOT_FIND_FILE, (object) this.userName, (object) this.storageName, (object) fileInfo.BlobID));
    return (IReadWorker) new DiskFileStorage.FileReadWorker(str, fileInfo.BlobID, fileInfo.HistoryID, this.manager, this.userName, this.storageName);
  }

  public void StartTransaction()
  {
    if (!DiskFileStorageCollection.CheckLogin(this.connectionID))
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    try
    {
      ApplicationEventLog.Log.InfoFormat(EventStringMessage.START_TRANSACTION, (object) this.userName, (object) this.storageName);
      this.InTransaction = true;
    }
    catch (Exception ex)
    {
      throw new VaultException($"{this.userName}, {this.storageName}: {ex.Message}", ex);
    }
  }

  public void Rollback()
  {
    if (!DiskFileStorageCollection.CheckLogin(this.connectionID))
      throw new VaultException(string.Format(EventStringMessage.USER_NOT_LOGGING, (object) this.userName, (object) this.storageName));
    try
    {
      ApplicationEventLog.Log.DebugFormat(EventStringMessage.ROLLBACK_TRANSACTION, (object) this.userName, (object) this.storageName);
      this.manager.Rollback();
      foreach (TransactionClass transaction in this.transactionStack)
      {
        ApplicationEventLog.Log.DebugFormat("ActionType={0} OperationType={1}", (object) EnumTypeHelper.GetCaption((Enum) transaction.ActionType), (object) EnumTypeHelper.GetCaption((Enum) transaction.OperationType));
        if (transaction.ActionType == TransactionType.AddFile)
        {
          StorageSecurity.AddFileDeleteRights(transaction.FileNameBeforeCommit);
          if (File.Exists(transaction.FileNameBeforeCommit))
          {
            ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_38"), (object) transaction.FileNameBeforeCommit);
            File.Delete(transaction.FileNameBeforeCommit);
          }
          else if (File.Exists(transaction.FileNameAfterCommit))
          {
            ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_39"), (object) transaction.FileNameAfterCommit);
            File.Delete(transaction.FileNameAfterCommit);
          }
        }
        else if (transaction.ActionType == TransactionType.AddFileInfo)
        {
          if (transaction.OperationType == FileOperationType.RenameFile)
          {
            ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_40"), (object) transaction.FileNameAfterCommit, (object) transaction.FileNameBeforeCommit);
            File.Copy(transaction.FileNameAfterCommit, transaction.FileNameBeforeCommit);
            StorageSecurity.RemoveFileDeleteRights(transaction.FileNameBeforeCommit);
          }
          else if (transaction.OperationType == FileOperationType.CopyFile && File.Exists(transaction.FileNameAfterCommit))
          {
            StorageSecurity.AddFileDeleteRights(transaction.FileNameAfterCommit);
            ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_41"), (object) transaction.FileNameAfterCommit);
            File.Delete(transaction.FileNameAfterCommit);
          }
        }
        else if (transaction.ActionType == TransactionType.MoveFile)
        {
          if (File.Exists(transaction.FileNameAfterCommit))
          {
            StorageSecurity.AddFileDeleteRights(transaction.FileNameAfterCommit);
            ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_42"), (object) transaction.FileNameAfterCommit, (object) transaction.FileNameBeforeCommit);
            File.Move(transaction.FileNameAfterCommit, transaction.FileNameBeforeCommit);
            StorageSecurity.RemoveFileDeleteRights(transaction.FileNameBeforeCommit);
          }
        }
        else if (transaction.ActionType == TransactionType.DeleteFile && File.Exists(transaction.FileNameAfterCommit))
        {
          StorageSecurity.AddFileDeleteRights(transaction.FileNameAfterCommit);
          ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_42"), (object) transaction.FileNameAfterCommit, (object) transaction.FileNameBeforeCommit);
          File.Move(transaction.FileNameAfterCommit, transaction.FileNameBeforeCommit);
          StorageSecurity.RemoveFileDeleteRights(transaction.FileNameBeforeCommit);
        }
      }
      this.transactionStack.Clear();
    }
    catch (Exception ex)
    {
      ApplicationEventLog.Log.Error((object) string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_43"), (object) this.userName, (object) this.storageName, (object) ex.Message));
    }
    finally
    {
      this.InTransaction = false;
    }
  }

  public void Commit()
  {
    ApplicationEventLog.Log.InfoFormat(EventStringMessage.COMMIT_TRANSACTION, (object) this.userName, (object) this.storageName);
    List<TransactionClass> stack = new List<TransactionClass>((IEnumerable<TransactionClass>) this.transactionStack);
    this.manager.StartTransaction(stack);
    try
    {
      foreach (TransactionClass transaction in this.transactionStack)
        this.manager.DoAction(transaction);
      foreach (TransactionClass transactionClass in stack)
      {
        ApplicationEventLog.Log.DebugFormat("ActionType={0} OperationType={1}", (object) EnumTypeHelper.GetCaption((Enum) transactionClass.ActionType), (object) EnumTypeHelper.GetCaption((Enum) transactionClass.OperationType));
        if (transactionClass.ActionType == TransactionType.AddFileInfo)
        {
          if (transactionClass.OperationType != FileOperationType.MoveFile)
          {
            if (transactionClass.OperationType == FileOperationType.RenameFile)
            {
              StorageSecurity.AddFileDeleteRights(transactionClass.FileNameBeforeCommit);
              ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_42"), (object) transactionClass.FileNameBeforeCommit, (object) transactionClass.FileNameAfterCommit);
              File.Move(transactionClass.FileNameBeforeCommit, transactionClass.FileNameAfterCommit);
              StorageSecurity.RemoveFileDeleteRights(transactionClass.FileNameAfterCommit);
            }
            else
            {
              ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_40"), (object) transactionClass.FileNameBeforeCommit, (object) transactionClass.FileNameAfterCommit);
              File.Copy(transactionClass.FileNameBeforeCommit, transactionClass.FileNameAfterCommit, true);
            }
            StorageSecurity.RemoveFileDeleteRights(transactionClass.FileNameAfterCommit);
          }
        }
        else
        {
          if (transactionClass.ActionType == TransactionType.DeleteStorage)
          {
            DiskFileStorageCollection.DeleteStorageConnection(this.connectionID);
            this.manager = (DBManager) null;
            if (Directory.Exists(this.storagePath))
            {
              ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_44"), (object) this.storagePath);
              StorageSecurity.AddDirectoryDeleteRights(this.storagePath);
              Directory.Delete(this.storagePath, true);
            }
            CommonVariables.RemoveRootDirectory(CommonVariables.GetRootDirectory(this.storageName, this.storageGUID));
            XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage(CommonVariables.XmlFilePath);
            XmlNode node = xmlSettingsStorage.FindNode((XmlNode) xmlSettingsStorage.document.DocumentElement, "root_folders", false);
            XmlNode nodeWithAttr = xmlSettingsStorage.FindNodeWithAttr(node, "storage", "guid", this.storageGUID, false);
            if (nodeWithAttr != null)
              node.RemoveChild(nodeWithAttr);
            xmlSettingsStorage.Save(CommonVariables.XmlFilePath);
            return;
          }
          if (transactionClass.ActionType == TransactionType.PurgeFile)
          {
            if (File.Exists(transactionClass.FileNameBeforeCommit))
            {
              StorageSecurity.AddFileDeleteRights(transactionClass.FileNameBeforeCommit);
              ApplicationEventLog.Log.InfoFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_45"), (object) this.userName, (object) this.storageName, (object) transactionClass.FileNameBeforeCommit);
              File.Delete(transactionClass.FileNameBeforeCommit);
            }
            foreach (string str in Array.FindAll<string>(Directory.GetFiles(this.tempFolderPath, "*", SearchOption.AllDirectories), new Predicate<string>(this.OutdatedFiles)))
            {
              if (File.Exists(str))
              {
                StorageSecurity.AddFileDeleteRights(str);
                ApplicationEventLog.Log.InfoFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_45"), (object) this.userName, (object) this.storageName, (object) str);
                File.Delete(str);
              }
            }
          }
          else if (transactionClass.ActionType != TransactionType.UpdateFileInfo)
          {
            if (transactionClass.ActionType == TransactionType.DeleteFile)
            {
              if (!File.Exists(transactionClass.FileNameBeforeCommit))
              {
                ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_46"), (object) transactionClass.FileNameBeforeCommit);
                continue;
              }
              if (this.manager.IsFileBlocked(transactionClass.fileInfo.BlobID, transactionClass.fileInfo.HistoryID))
              {
                ApplicationEventLog.Log.Debug((object) Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_47"));
                throw new VaultException(string.Format(EventStringMessage.CANNOT_DELETE_LOCKED_FILE, (object) transactionClass.fileInfo.BlobID));
              }
            }
            if (transactionClass.ActionType == TransactionType.MoveFile && !File.Exists(transactionClass.FileNameBeforeCommit))
            {
              ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_48"), (object) transactionClass.FileNameBeforeCommit);
            }
            else
            {
              StorageSecurity.AddFileDeleteRights(transactionClass.FileNameBeforeCommit);
              ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_42"), (object) transactionClass.FileNameBeforeCommit, (object) transactionClass.FileNameAfterCommit);
              File.Move(transactionClass.FileNameBeforeCommit, transactionClass.FileNameAfterCommit);
              StorageSecurity.RemoveFileDeleteRights(transactionClass.FileNameAfterCommit);
            }
          }
        }
      }
      this.manager.Commit();
    }
    catch (Exception ex)
    {
      this.Rollback();
      throw new VaultException($"{this.userName}, {this.storageName}: {ex.Message}", ex);
    }
    finally
    {
      this.transactionStack.Clear();
      this.InTransaction = false;
    }
  }

  private bool OutdatedFiles(string filePath)
  {
    DateTime dateTime = DateTime.UtcNow - CommonVariables.WAIT_SPAN;
    return File.Exists(filePath) && new FileInfo(filePath).CreationTimeUtc < dateTime;
  }

  public override object InitializeLifetimeService() => (object) null;

  public void Dispose() => this.Logout();

  private void OpenConnection()
  {
    if (this.connection == null)
    {
      this.connection = (SQLiteConnection) DbProviderFactories.GetFactory("System.Data.SQLite").CreateConnection();
      this.connection.ConnectionString = this.connectionString;
    }
    if (this.connection.State != ConnectionState.Closed)
      return;
    this.connection.Open();
  }

  private void CloseConnection()
  {
    if (this.connection == null)
      return;
    this.connection.Close();
    this.connection = (SQLiteConnection) null;
  }

  private SQLiteCommand CreateCommand(string commandText)
  {
    SQLiteCommand command = this.connection.CreateCommand();
    command.CommandText = commandText;
    return command;
  }

  private SQLiteCommand CreateCommand(string commandText, SQLiteParameter[] parameters)
  {
    SQLiteCommand command = this.CreateCommand(commandText);
    if (parameters != null)
    {
      foreach (SQLiteParameter parameter in parameters)
        command.Parameters.Add(parameter);
    }
    return command;
  }

  private void FillColumsCaption(DataTable table)
  {
    table.Columns[0].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_49");
    table.Columns[1].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_50");
    table.Columns[2].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_51");
    table.Columns[3].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_52");
    table.Columns[4].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_53");
    table.Columns[5].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_54");
    table.Columns[6].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_55");
    table.Columns[7].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_56");
    table.Columns[8].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_57");
    table.Columns[9].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_58");
    table.Columns[10].Caption = Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_59");
  }

  internal class FileWriteWorker : MarshalByRefObject, IWriteWorker
  {
    private FileStream _Writer;

    public FileWriteWorker(string tempFileName)
    {
      this._Writer = new FileStream(tempFileName, FileMode.Create, FileAccess.Write);
    }

    public void WriteBlock(byte[] dataBlock, int dataLength)
    {
      this._Writer.Write(dataBlock, 0, dataLength);
    }

    public void Close()
    {
      this._Writer.Close();
      this._Writer = (FileStream) null;
    }
  }

  internal class FileReadWorker : MarshalByRefObject, IReadWorker
  {
    private FileStream _Reader;
    private DBManager _manager;
    private long _blobID;
    private int _historyID;
    private string _userName;
    private string _storageName;

    public FileReadWorker(
      string tempFileName,
      long BlobID,
      int HistoryID,
      DBManager manager,
      string UserName,
      string StorageName)
    {
      this._manager = manager;
      this._blobID = BlobID;
      this._historyID = HistoryID;
      this._userName = UserName;
      this._storageName = StorageName;
      manager.AddBlockedFiles(BlobID, HistoryID);
      this._Reader = new FileStream(tempFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    public int ReadBlock(ref byte[] dataBlock, int dataLength)
    {
      return this._Reader.Read(dataBlock, 0, dataLength);
    }

    public void Close()
    {
      this._Reader.Close();
      this._Reader = (FileStream) null;
      try
      {
        this._manager.RemoveBlockedFiles(this._blobID, this._historyID);
      }
      catch (Exception ex)
      {
        throw new VaultException(string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_29"), (object) this._userName, (object) this._storageName, (object) this._blobID, (object) ex.Message), ex);
      }
    }
  }
}
