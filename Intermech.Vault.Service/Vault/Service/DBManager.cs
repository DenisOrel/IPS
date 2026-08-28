// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.DBManager
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

#nullable disable
namespace Intermech.Vault.Service;

public class DBManager
{
  private SQLiteConnection connection;
  private SQLiteTransaction transaction;
  private string storagePath;
  private string dbFilePath;
  private string workingFolderPath;
  private string historyFolderPath;
  private string deletedFolderPath;
  private string tempFolderPath;
  private string storageGuid;
  private string storageName;
  private SQLiteCommand command;
  private List<TransactionClass> transactionStack = new List<TransactionClass>();
  private EventWaitHandle UnblockDataBase = (EventWaitHandle) new AutoResetEvent(true);
  private Dictionary<BlockedFileInfo, int> blockedFiles = new Dictionary<BlockedFileInfo, int>();
  private int сonnCounter = 1;
  private string connectionString;
  private ExtConnections extConnections;

  public void AddBlockedFiles(long blocked, int history)
  {
    lock (this.blockedFiles)
    {
      BlockedFileInfo key = new BlockedFileInfo(blocked, history);
      if (this.blockedFiles.ContainsKey(key))
        this.blockedFiles[key]++;
      else
        this.blockedFiles.Add(key, 1);
    }
  }

  public void RemoveBlockedFiles(long blocked, int history)
  {
    lock (this.blockedFiles)
    {
      BlockedFileInfo key = new BlockedFileInfo(blocked, history);
      if (!this.blockedFiles.ContainsKey(key) || --this.blockedFiles[key] != 0)
        return;
      this.blockedFiles.Remove(key);
    }
  }

  public bool IsFileBlocked(long blocked, int history)
  {
    lock (this.blockedFiles)
    {
      if (this.blockedFiles.ContainsKey(new BlockedFileInfo(blocked, history)))
        return true;
    }
    return false;
  }

  public bool InTransaction => this.transaction != null;

  public int СonnCounter
  {
    get => this.сonnCounter;
    set => this.сonnCounter = value;
  }

  public DBManager(string dbPath)
  {
    this.dbFilePath = dbPath;
    this.storagePath = Directory.GetParent(this.dbFilePath).FullName;
    this.storageName = Directory.GetParent(this.dbFilePath).Name;
    this.storageGuid = Path.GetFileNameWithoutExtension(this.dbFilePath);
    this.workingFolderPath = Path.Combine(this.storagePath, CommonVariables.WORKING_FOLDER_NAME);
    this.historyFolderPath = Path.Combine(this.storagePath, CommonVariables.HISTORY_FOLDER_NAME);
    this.deletedFolderPath = Path.Combine(this.storagePath, CommonVariables.DELETED_FOLDER_NAME);
    this.tempFolderPath = Path.Combine(this.storagePath, CommonVariables.TEMP_FOLDER_NAME);
    this.connectionString = $"Data Source={this.dbFilePath}; Pooling=true; Min Pool Size=2; Max Pool Size=25";
    if (CommonVariables.SyncModeOff)
      this.connectionString += ";synchronous=off";
    this.extConnections = new ExtConnections();
  }

  public void DoAction(TransactionClass transaction)
  {
    ApplicationEventLog.Log.DebugFormat("actionType={0}", (object) EnumTypeHelper.GetCaption((Enum) transaction.ActionType));
    if (transaction.ActionType == TransactionType.AddFile || transaction.ActionType == TransactionType.AddFileInfo)
      this.PutFile(transaction);
    else if (transaction.ActionType == TransactionType.UpdateFileInfo)
      this.UpdateFileInfo(transaction);
    else if (transaction.ActionType == TransactionType.DeleteFile)
      this.DeleteFile(transaction);
    else if (transaction.ActionType == TransactionType.DeleteStorage)
    {
      this.DeleteStorage();
    }
    else
    {
      if (transaction.ActionType != TransactionType.PurgeFile)
        return;
      this.DeleteTrash(transaction);
    }
  }

  private void DeleteStorage()
  {
    bool boolean;
    using (this.command = this.CreateCommand(SQLCommands.CheckWorkFilesExist))
      boolean = Convert.ToBoolean(this.command.ExecuteScalar());
    if (boolean)
      throw new VaultException(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_11"));
    if (this.сonnCounter != 1)
      throw new VaultException(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_12"));
    this.connection.Close();
    GC.Collect();
    this.transaction.Dispose();
    this.transaction = (SQLiteTransaction) null;
    this.connection.Dispose();
    this.connection = (SQLiteConnection) null;
  }

  private void PutFile(TransactionClass transaction)
  {
    try
    {
      FileInformation fileInfo = transaction.fileInfo;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      RootDirectory rootDirectory = CommonVariables.GetRootDirectory(this.storageName, this.storageGuid);
      if (fileInfo.PacketFileSize > 0L)
      {
        DriveInfo driveInfo = new DriveInfo(rootDirectory.Path);
        if (driveInfo.IsReady)
        {
          long totalFreeSpace = driveInfo.TotalFreeSpace;
          long int64;
          using (this.command = this.CreateCommand(SQLCommands.SelectStorageFullSize))
          {
            int64 = Convert.ToInt64(this.command.ExecuteScalar());
            ApplicationEventLog.Log.DebugFormat("rootDirectory.MaxSize={0} storageSize={1} freeSpace={2}", (object) rootDirectory.MaxSize, (object) int64, (object) totalFreeSpace);
          }
          long num = totalFreeSpace * (long) rootDirectory.MaxSize / 100L;
          if (int64 + fileInfo.PacketFileSize > num)
            throw new VaultException(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_13"));
        }
      }
      int num1 = -1;
      FileInformation fileInformation = new FileInformation();
      SQLiteParameter[] parameters1 = new SQLiteParameter[1]
      {
        new SQLiteParameter("@id", (object) transaction.fileInfo.ID)
      };
      if (fileInfo.ID != 0L)
      {
        using (this.command = this.CreateCommand(SQLCommands.SelectMaxHistoryID, parameters1))
        {
          object obj = this.command.ExecuteScalar();
          num1 = obj == DBNull.Value ? -1 : Convert.ToInt32(obj);
          ApplicationEventLog.Log.DebugFormat("historyID={0}", (object) num1);
        }
      }
      int num2;
      fileInfo.HistoryID = num2 = num1 + 1;
      SQLiteParameter[] parameters2 = new SQLiteParameter[1]
      {
        new SQLiteParameter("@blobid", (object) transaction.fileInfo.BlobID)
      };
      object obj1 = (object) null;
      using (this.command = this.CreateCommand(SQLCommands.CheckFileExists, parameters2))
        obj1 = this.command.ExecuteScalar();
      bool flag = obj1 != DBNull.Value && obj1 != null && Convert.ToBoolean(obj1);
      ApplicationEventLog.Log.DebugFormat("isFileExists={0}", (object) flag);
      string str1;
      if (flag)
      {
        SQLiteParameter[] parameters3 = new SQLiteParameter[1]
        {
          new SQLiteParameter("@blobid", (object) fileInfo.BlobID)
        };
        using (this.command = this.CreateCommand(SQLCommands.SelectFileInfo, parameters3))
        {
          using (SQLiteDataReader sqLiteDataReader = this.command.ExecuteReader(CommandBehavior.SingleRow))
          {
            if (sqLiteDataReader.Read())
            {
              fileInformation.ArcMethod = (ArcMethods) Convert.ToInt32(sqLiteDataReader["F_ARC_METHOD"]);
              fileInformation.Volume = sqLiteDataReader.GetString(2);
              fileInformation.ID = sqLiteDataReader.GetInt64(4);
              fileInformation.BlobID = sqLiteDataReader.GetInt64(5);
              fileInformation.ObjectID = sqLiteDataReader.GetInt64(6);
              fileInformation.HistoryID = sqLiteDataReader.GetInt32(7);
              fileInformation.Name = sqLiteDataReader.GetString(8);
              DateTime dateTime = sqLiteDataReader.GetDateTime(9);
              fileInformation.FileDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
              fileInformation.UserName = sqLiteDataReader.GetString(10);
              fileInformation.MachineName = sqLiteDataReader.GetString(11);
              fileInformation.PacketFileSize = sqLiteDataReader.GetInt64(13);
              fileInformation.RealSize = sqLiteDataReader.GetInt64(14);
              fileInformation.Note = Convert.ToString(sqLiteDataReader["F_NOTE"]);
            }
          }
        }
        string path2_1 = this.SelectCurrentVolume("history", fileInformation);
        SQLiteParameter[] parameters4 = new SQLiteParameter[5]
        {
          new SQLiteParameter("@folder_name", (object) "history"),
          new SQLiteParameter("@blobid", (object) fileInfo.BlobID),
          new SQLiteParameter("@volume_name", (object) path2_1),
          new SQLiteParameter("@user_name", (object) fileInformation.UserName),
          new SQLiteParameter("@change_date", (object) DateTime.UtcNow)
        };
        using (this.command = this.CreateCommand(SQLCommands.MoveFileFromWork, parameters4))
          this.command.ExecuteNonQuery();
        string path2_2 = $"{fileInformation.ID}_{Math.Abs(fileInformation.ObjectID)}_{fileInformation.BlobID}_{fileInformation.HistoryID}";
        ApplicationEventLog.Log.DebugFormat("compexFileName={0}", (object) path2_2);
        SQLiteParameter[] sqLiteParameterArray = new SQLiteParameter[1]
        {
          new SQLiteParameter("@volume", (object) fileInformation.Volume)
        };
        string beforeCommit = Path.Combine(Path.Combine(this.workingFolderPath, fileInformation.Volume), path2_2);
        transaction.FileNameAfterCommit = beforeCommit;
        string str2 = Path.Combine(this.historyFolderPath, path2_1);
        string afterCommit = Path.Combine(str2, path2_2);
        ApplicationEventLog.Log.DebugFormat("afterCommitFileName={0}", (object) afterCommit);
        TransactionType action = transaction.ActionType == TransactionType.AddFile ? TransactionType.MoveFile : TransactionType.AddFileInfo;
        FileOperationType operation = transaction.ActionType == TransactionType.AddFile ? FileOperationType.MoveFile : FileOperationType.CopyFile;
        TransactionClass transactionClass = new TransactionClass(afterCommit, beforeCommit, action, operation);
        this.transactionStack.Insert(this.transactionStack.IndexOf(transaction), transactionClass);
        this.AddVolumeFileInfo(fileInformation, str2, "history");
        str1 = this.SearchFreeVolume(fileInformation, fileInfo);
        if (transaction.fileInfo.IsStreamEmty)
        {
          transaction.OperationType = FileOperationType.RenameFile;
          transaction.FileNameBeforeCommit = beforeCommit;
        }
      }
      else
        str1 = this.SelectCurrentVolume("work", fileInfo);
      string path2 = $"{fileInfo.ID}_{Math.Abs(fileInfo.ObjectID)}_{fileInfo.BlobID}_{fileInfo.HistoryID}";
      string str3 = Path.Combine(this.workingFolderPath, str1);
      string str4 = Path.Combine(str3, path2);
      ApplicationEventLog.Log.DebugFormat("afterCommitFileName={0}", (object) str4);
      transaction.FileNameAfterCommit = str4;
      this.AddFileInfo(fileInfo, "work", str1);
      if (flag)
        this.UpdateVolumeFileInfo(fileInfo, str3);
      else
        this.AddVolumeFileInfo(fileInfo, str3, "work");
    }
    catch (Exception ex)
    {
      throw new VaultException(string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_14"), (object) ex.Message), ex);
    }
  }

  private void UpdateFileInfo(TransactionClass transaction)
  {
    FileInformation fileInfo = transaction.fileInfo;
    SQLiteParameter[] parameters = new SQLiteParameter[3]
    {
      new SQLiteParameter("@objectID", (object) fileInfo.ObjectID),
      new SQLiteParameter("@blobID", (object) fileInfo.BlobID),
      new SQLiteParameter("@id", (object) fileInfo.ID)
    };
    using (SQLiteCommand command = this.CreateCommand(SQLCommands.ChangeObjectLinkID, parameters))
      command.ExecuteScalar();
    ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_15"), (object) fileInfo.BlobID);
  }

  private void UpdateVolumeFileInfo(FileInformation fileInfo, string volumePath)
  {
    ApplicationEventLog.Log.Debug((object) Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_16"));
    this.extConnections.CreateConnection(volumePath);
    SQLiteParameter[] parameters = new SQLiteParameter[15]
    {
      new SQLiteParameter("@folder_name", (object) "work"),
      new SQLiteParameter("@volume_name", (object) Path.GetFileName(volumePath)),
      new SQLiteParameter("@created_date", (object) DateTime.UtcNow),
      new SQLiteParameter("@id", (object) fileInfo.ID),
      new SQLiteParameter("@blobID", (object) fileInfo.BlobID),
      new SQLiteParameter("@objectID", (object) fileInfo.ObjectID),
      new SQLiteParameter("@historyID", (object) fileInfo.HistoryID),
      new SQLiteParameter("@file_name", (object) fileInfo.Name),
      new SQLiteParameter("@file_date", (object) fileInfo.FileDate),
      new SQLiteParameter("@user_name", (object) fileInfo.UserName),
      new SQLiteParameter("@computer_name", (object) fileInfo.MachineName),
      new SQLiteParameter("@arc_method", (object) (int) fileInfo.ArcMethod),
      new SQLiteParameter("@packed_size", (object) fileInfo.PacketFileSize),
      new SQLiteParameter("@real_size", (object) fileInfo.RealSize),
      new SQLiteParameter("@note", (object) fileInfo.Note)
    };
    using (this.command = this.extConnections.CreateCommand(volumePath, SQLCommands.UpdateVolumeFileInfo, parameters))
      this.command.ExecuteNonQuery();
  }

  private void AddVolumeFileInfo(FileInformation fileInfo, string volumePath, string folder)
  {
    ApplicationEventLog.Log.Info((object) Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_17"));
    this.extConnections.CreateConnection(volumePath);
    string fileName = Path.GetFileName(volumePath);
    SQLiteParameter[] parameters = new SQLiteParameter[15]
    {
      new SQLiteParameter("@folder_name", (object) folder),
      new SQLiteParameter("@volume_name", (object) fileName),
      new SQLiteParameter("@created_date", (object) DateTime.UtcNow),
      new SQLiteParameter("@id", (object) fileInfo.ID),
      new SQLiteParameter("@blobID", (object) fileInfo.BlobID),
      new SQLiteParameter("@objectID", (object) fileInfo.ObjectID),
      new SQLiteParameter("@historyID", (object) fileInfo.HistoryID),
      new SQLiteParameter("@file_name", (object) fileInfo.Name),
      new SQLiteParameter("@file_date", (object) fileInfo.FileDate),
      new SQLiteParameter("@user_name", (object) fileInfo.UserName),
      new SQLiteParameter("@computer_name", (object) fileInfo.MachineName),
      new SQLiteParameter("@arc_method", (object) (int) fileInfo.ArcMethod),
      new SQLiteParameter("@packed_size", (object) fileInfo.PacketFileSize),
      new SQLiteParameter("@real_size", (object) fileInfo.RealSize),
      new SQLiteParameter("@note", (object) fileInfo.Note)
    };
    using (this.command = this.extConnections.CreateCommand(volumePath, SQLCommands.InsertFileInfo, parameters))
      this.command.ExecuteNonQuery();
    ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_18"), (object) fileInfo.ID, (object) fileInfo.ObjectID, (object) fileInfo.HistoryID, (object) fileInfo.BlobID, (object) fileName, (object) folder);
  }

  private void DeleteVolumeFileInfo(long blobID, string volumePath)
  {
    ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_19"), (object) blobID);
    this.extConnections.CreateConnection(volumePath);
    SQLiteParameter[] parameters = new SQLiteParameter[1]
    {
      new SQLiteParameter("@blobID", (object) blobID)
    };
    using (this.command = this.extConnections.CreateCommand(volumePath, SQLCommands.DeleteFileInfo, parameters))
      this.command.ExecuteNonQuery();
    ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_20"), (object) blobID);
  }

  private void DeleteFile(TransactionClass transaction)
  {
    ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_21"), (object) transaction.fileInfo.BlobID);
    FileInformation fileInfo1 = transaction.fileInfo;
    long blobId = fileInfo1.BlobID;
    lock (this.blockedFiles)
    {
      if (this.blockedFiles.ContainsKey(new BlockedFileInfo(blobId, 0)))
        throw new VaultException(string.Format(EventStringMessage.CANNOT_DELETE_LOCKED_FILE, (object) blobId));
    }
    SQLiteParameter[] parameters1 = new SQLiteParameter[1]
    {
      new SQLiteParameter("@blobid", (object) blobId)
    };
    FileInformation fileInfo2 = new FileInformation();
    using (this.command = this.CreateCommand(SQLCommands.SelectFileInfo, parameters1))
    {
      using (SQLiteDataReader sqLiteDataReader = this.command.ExecuteReader(CommandBehavior.SingleRow))
      {
        if (sqLiteDataReader.Read())
        {
          fileInfo2.ArcMethod = (ArcMethods) Convert.ToInt32(sqLiteDataReader["F_ARC_METHOD"]);
          fileInfo2.Volume = sqLiteDataReader.GetString(2);
          fileInfo2.ID = sqLiteDataReader.GetInt64(4);
          fileInfo2.BlobID = sqLiteDataReader.GetInt64(5);
          fileInfo2.ObjectID = sqLiteDataReader.GetInt64(6);
          fileInfo2.HistoryID = sqLiteDataReader.GetInt32(7);
          fileInfo2.Name = sqLiteDataReader.GetString(8);
          DateTime dateTime = sqLiteDataReader.GetDateTime(9);
          fileInfo2.FileDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
          fileInfo2.UserName = sqLiteDataReader.GetString(10);
          fileInfo2.MachineName = sqLiteDataReader.GetString(11);
          fileInfo2.PacketFileSize = sqLiteDataReader.GetInt64(13);
          fileInfo2.RealSize = sqLiteDataReader.GetInt64(14);
          fileInfo2.Note = Convert.ToString(sqLiteDataReader["F_NOTE"]);
        }
        else
        {
          ApplicationEventLog.Log.Debug((object) Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_22"));
          return;
        }
      }
    }
    string path2_1 = this.SelectCurrentVolume("deleted", fileInfo2);
    SQLiteParameter[] parameters2 = new SQLiteParameter[5]
    {
      new SQLiteParameter("@folder_name", (object) "deleted"),
      new SQLiteParameter("@blobid", (object) blobId),
      new SQLiteParameter("@volume_name", (object) path2_1),
      new SQLiteParameter("@user_name", (object) fileInfo1.UserName),
      new SQLiteParameter("@change_date", (object) DateTime.UtcNow)
    };
    using (this.command = this.CreateCommand(SQLCommands.MoveFileFromWork, parameters2))
      this.command.ExecuteScalar();
    string path2_2 = $"{fileInfo2.ID}_{Math.Abs(fileInfo2.ObjectID)}_{fileInfo2.BlobID}_{fileInfo2.HistoryID}";
    string str1 = Path.Combine(this.workingFolderPath, fileInfo2.Volume);
    string str2 = Path.Combine(str1, path2_2);
    ApplicationEventLog.Log.DebugFormat("fileNameBeforeCommit={0}", (object) str2);
    this.DeleteVolumeFileInfo(fileInfo2.BlobID, str1);
    string str3 = Path.Combine(this.deletedFolderPath, path2_1);
    string str4 = Path.Combine(str3, path2_2);
    ApplicationEventLog.Log.DebugFormat("fileNameAfterCommit={0}", (object) str4);
    this.AddVolumeFileInfo(fileInfo2, str3, "delete");
    transaction.FileNameBeforeCommit = str2;
    transaction.FileNameAfterCommit = str4;
  }

  private void DeleteTrash(TransactionClass transaction)
  {
    DateTime utcNow = DateTime.UtcNow;
    long num1 = CommonVariables.HistoryLife == 0U ? long.MaxValue : (long) CommonVariables.HistoryLife;
    long num2 = CommonVariables.DeletedLife == 0U ? long.MaxValue : (long) CommonVariables.DeletedLife;
    SQLiteParameter[] parameters1 = new SQLiteParameter[3]
    {
      new SQLiteParameter("@current_date", (object) utcNow),
      new SQLiteParameter("@historyLife", (object) num1),
      new SQLiteParameter("@deletedLife", (object) num2)
    };
    bool flag = true;
    using (this.command = this.CreateCommand(SQLCommands.SelectTrashInfo, parameters1))
    {
      using (SQLiteDataReader sqLiteDataReader = this.command.ExecuteReader())
      {
        while (sqLiteDataReader.Read())
        {
          long int64_1 = sqLiteDataReader.GetInt64(0);
          long int64_2 = sqLiteDataReader.GetInt64(1);
          long int64_3 = sqLiteDataReader.GetInt64(2);
          int int32 = sqLiteDataReader.GetInt32(3);
          string path2_1 = sqLiteDataReader.GetString(4);
          string str1 = sqLiteDataReader.GetString(5);
          string path2_2 = $"{int64_1}_{Math.Abs(int64_2)}_{int64_3}_{int32}";
          string str2 = Path.Combine(str1.Equals("history") ? this.historyFolderPath : this.deletedFolderPath, path2_1);
          string beforeCommit = Path.Combine(str2, path2_2);
          if (flag)
          {
            transaction.FileNameBeforeCommit = beforeCommit;
            flag = false;
          }
          else
          {
            TransactionClass transactionClass = new TransactionClass(TransactionType.PurgeFile, beforeCommit);
            this.transactionStack.Insert(this.transactionStack.IndexOf(transaction), transactionClass);
          }
          this.DeleteVolumeFileInfo(int64_3, str2);
        }
      }
    }
    SQLiteParameter[] parameters2 = new SQLiteParameter[3]
    {
      new SQLiteParameter("@current_date", (object) utcNow),
      new SQLiteParameter("@historyLife", (object) num1),
      new SQLiteParameter("@deletedLife", (object) num2)
    };
    using (this.command = this.CreateCommand(SQLCommands.DeleteTrashInfo, parameters2))
      this.command.ExecuteNonQuery();
  }

  private string SelectCurrentVolume(string folderName, FileInformation fileInfo)
  {
    string path2;
    long int64_1;
    int int32;
    using (this.command = this.CreateCommand(string.Format(SQLCommands.SelectCurrentVolumeInfo, (object) folderName)))
    {
      using (SQLiteDataReader sqLiteDataReader = this.command.ExecuteReader(CommandBehavior.SingleRow))
      {
        path2 = sqLiteDataReader.Read() ? sqLiteDataReader[0].ToString() : throw new VaultException(string.Format(EventStringMessage.CANNOT_FIND_CURRENT_VALUE, (object) folderName));
        int64_1 = Convert.ToInt64(sqLiteDataReader[1]);
        int32 = Convert.ToInt32(path2.Replace(CommonVariables.VOLUME_NAME, string.Empty));
      }
    }
    ApplicationEventLog.Log.DebugFormat("VolumeName={0}, VolumeMaxSize={1}, VolumeIndex={2} FolderName={3}", (object) path2, (object) int64_1, (object) int32, (object) folderName);
    SQLiteParameter[] parameters1 = new SQLiteParameter[2]
    {
      new SQLiteParameter("@volume_name", (object) path2),
      new SQLiteParameter("@folder_name", (object) folderName)
    };
    object obj = (object) null;
    using (this.command = this.CreateCommand(SQLCommands.SelectVolumeSize, parameters1))
      obj = this.command.ExecuteScalar();
    long int64_2 = obj == DBNull.Value ? 0L : Convert.ToInt64(obj);
    ApplicationEventLog.Log.DebugFormat("VolumeSize={0}", (object) int64_2);
    if (int64_2 + fileInfo.PacketFileSize >= int64_1)
    {
      SQLiteParameter[] parameters2 = new SQLiteParameter[1]
      {
        new SQLiteParameter("@volume_name", (object) path2)
      };
      using (this.command = this.CreateCommand(string.Format(SQLCommands.CloseCurrentVolume, (object) folderName), parameters2))
        this.command.ExecuteNonQuery();
      int num;
      path2 = CommonVariables.VOLUME_NAME + Convert.ToString(num = int32 + 1);
      SQLiteParameter[] parameters3 = new SQLiteParameter[4]
      {
        new SQLiteParameter("@foldername", (object) folderName),
        new SQLiteParameter("@volume_name", (object) path2),
        new SQLiteParameter("@max_size", (object) CommonVariables.MaxVolumeSize),
        new SQLiteParameter("@current", (object) true)
      };
      using (this.command = this.CreateCommand(string.Format(SQLCommands.InsertNewVolume, (object) folderName), parameters3))
        this.command.ExecuteNonQuery();
      string path1 = string.Empty;
      switch (folderName)
      {
        case "work":
          path1 = this.workingFolderPath;
          break;
        case "history":
          path1 = this.historyFolderPath;
          break;
        case "deleted":
          path1 = this.deletedFolderPath;
          break;
      }
      ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_23"), (object) path2);
      string str = Path.Combine(path1, path2);
      Directory.CreateDirectory(str);
      StorageSecurity.RemoveDirectoryDeleteRights(str);
    }
    return path2;
  }

  private string SearchFreeVolume(FileInformation beforeChangeInfo, FileInformation afterChangeInfo)
  {
    if (beforeChangeInfo.PacketFileSize >= afterChangeInfo.PacketFileSize)
      return beforeChangeInfo.Volume;
    SQLiteParameter[] parameters1 = new SQLiteParameter[2]
    {
      new SQLiteParameter("@volume_name", (object) beforeChangeInfo.Volume),
      new SQLiteParameter("@folder_name", (object) "work")
    };
    object obj = (object) null;
    using (this.command = this.CreateCommand(SQLCommands.SelectVolumeSize, parameters1))
      obj = this.command.ExecuteScalar();
    long int64_1 = obj == DBNull.Value ? 0L : Convert.ToInt64(obj);
    ApplicationEventLog.Log.DebugFormat("currentVolumeSize={0}", (object) int64_1);
    SQLiteParameter[] parameters2 = new SQLiteParameter[1]
    {
      new SQLiteParameter("@volume_name", (object) beforeChangeInfo.Volume)
    };
    long num = 0;
    using (this.command = this.CreateCommand(SQLCommands.SelectMaxSizeWorkVolume, parameters2))
      num = Convert.ToInt64(this.command.ExecuteScalar());
    ApplicationEventLog.Log.DebugFormat("maxVolumeSize={0}", (object) num);
    if (afterChangeInfo.PacketFileSize + int64_1 <= num)
      return beforeChangeInfo.Volume;
    SQLiteParameter[] parameters3 = new SQLiteParameter[1]
    {
      new SQLiteParameter("@folder_name", (object) "history")
    };
    using (this.command = this.CreateCommand(SQLCommands.SelectVolumesSizes, parameters3))
    {
      using (SQLiteDataReader sqLiteDataReader = this.command.ExecuteReader())
      {
        while (sqLiteDataReader.Read())
        {
          if (!sqLiteDataReader.GetBoolean(2))
          {
            string str = sqLiteDataReader.GetString(1);
            if (!(str == beforeChangeInfo.Volume))
            {
              long int64_2 = sqLiteDataReader.GetInt64(0);
              if (afterChangeInfo.PacketFileSize <= int64_2)
                return str;
            }
          }
          else
            break;
        }
      }
    }
    return this.SelectCurrentVolume("work", afterChangeInfo);
  }

  private void UpdateFileInfoTable(FileInformation fileInfo, string folderName, string volumeName)
  {
    SQLiteParameter[] parameters = new SQLiteParameter[14]
    {
      new SQLiteParameter("@folder_name", (object) folderName),
      new SQLiteParameter("@volume_name", (object) volumeName),
      new SQLiteParameter("@created_date", (object) DateTime.UtcNow),
      new SQLiteParameter("@id", (object) fileInfo.ID),
      new SQLiteParameter("@blobID", (object) fileInfo.BlobID),
      new SQLiteParameter("@objectID", (object) fileInfo.ObjectID),
      new SQLiteParameter("@file_name", (object) fileInfo.Name),
      new SQLiteParameter("@file_date", (object) fileInfo.FileDate),
      new SQLiteParameter("@user_name", (object) fileInfo.UserName),
      new SQLiteParameter("@computer_name", (object) fileInfo.MachineName),
      new SQLiteParameter("@arc_method", (object) (int) fileInfo.ArcMethod),
      new SQLiteParameter("@packed_size", (object) fileInfo.PacketFileSize),
      new SQLiteParameter("@real_size", (object) fileInfo.RealSize),
      new SQLiteParameter("@note", (object) fileInfo.Note)
    };
    using (this.command = this.CreateCommand(SQLCommands.UpdateFileInfo, parameters))
      this.command.ExecuteNonQuery();
  }

  private void AddFileInfo(FileInformation fileInfo, string folderName, string volumeName)
  {
    SQLiteParameter[] parameters = new SQLiteParameter[15]
    {
      new SQLiteParameter("@folder_name", (object) folderName),
      new SQLiteParameter("@volume_name", (object) volumeName),
      new SQLiteParameter("@created_date", (object) DateTime.UtcNow),
      new SQLiteParameter("@id", (object) fileInfo.ID),
      new SQLiteParameter("@blobID", (object) fileInfo.BlobID),
      new SQLiteParameter("@objectID", (object) fileInfo.ObjectID),
      new SQLiteParameter("@historyID", (object) fileInfo.HistoryID),
      new SQLiteParameter("@file_name", (object) fileInfo.Name),
      new SQLiteParameter("@file_date", (object) fileInfo.FileDate),
      new SQLiteParameter("@user_name", (object) fileInfo.UserName),
      new SQLiteParameter("@computer_name", (object) fileInfo.MachineName),
      new SQLiteParameter("@arc_method", (object) (int) fileInfo.ArcMethod),
      new SQLiteParameter("@packed_size", (object) fileInfo.PacketFileSize),
      new SQLiteParameter("@real_size", (object) fileInfo.RealSize),
      new SQLiteParameter("@note", (object) fileInfo.Note)
    };
    using (this.command = this.CreateCommand(SQLCommands.InsertFileInfo, parameters))
      this.command.ExecuteNonQuery();
  }

  public void CreateDBStorageHierarchy()
  {
    using (this.command = this.CreateCommand(SQLCommands.CreatedWorkTable))
      this.command.ExecuteNonQuery();
    SQLiteParameter[] parameters1 = new SQLiteParameter[3]
    {
      new SQLiteParameter("@folder", (object) (CommonVariables.VOLUME_NAME + "0")),
      new SQLiteParameter("@maxsize", (object) CommonVariables.MaxVolumeSize),
      new SQLiteParameter("@current", (object) true)
    };
    using (this.command = this.CreateCommand(SQLCommands.insertDataIntoWorkTable, parameters1))
      this.command.ExecuteNonQuery();
    using (this.command = this.CreateCommand(SQLCommands.CreateHistoryTable))
      this.command.ExecuteNonQuery();
    SQLiteParameter[] parameters2 = new SQLiteParameter[3]
    {
      new SQLiteParameter("@folder", (object) (CommonVariables.VOLUME_NAME + "0")),
      new SQLiteParameter("@maxsize", (object) CommonVariables.MaxVolumeSize),
      new SQLiteParameter("@current", (object) true)
    };
    using (this.command = this.CreateCommand(SQLCommands.InsertDataIntoHistoryTable, parameters2))
      this.command.ExecuteNonQuery();
    using (this.command = this.CreateCommand(SQLCommands.CreateDeletedTable))
      this.command.ExecuteNonQuery();
    SQLiteParameter[] parameters3 = new SQLiteParameter[3]
    {
      new SQLiteParameter("@folder", (object) (CommonVariables.VOLUME_NAME + "0")),
      new SQLiteParameter("@maxsize", (object) CommonVariables.MaxVolumeSize),
      new SQLiteParameter("@current", (object) true)
    };
    using (this.command = this.CreateCommand(SQLCommands.InsertDataIntoDeletedTable, parameters3))
      this.command.ExecuteNonQuery();
    using (this.command = this.CreateCommand(SQLCommands.CreateFilesTable))
      this.command.ExecuteNonQuery();
  }

  public void StartTransaction(List<TransactionClass> stack)
  {
    try
    {
      this.UnblockDataBase.WaitOne();
      this.OpenConnection();
      this.transactionStack = stack;
      this.transaction = this.connection.BeginTransaction();
    }
    catch (Exception ex)
    {
      throw new VaultException(string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_24"), (object) this.storageName, (object) this.storageGuid, (object) ex.Message), ex);
    }
  }

  public void Rollback()
  {
    try
    {
      this.extConnections.RollbackTransactions();
      if (this.transaction == null)
        return;
      this.transaction.Rollback();
      this.transaction.Dispose();
      this.transaction = (SQLiteTransaction) null;
    }
    catch (Exception ex)
    {
      throw new VaultException(string.Format(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_25"), (object) this.storageName, (object) this.storageGuid, (object) ex.Message), ex);
    }
    finally
    {
      this.CloseConnection();
      this.UnblockDataBase.Set();
    }
  }

  public void Commit()
  {
    if (this.transaction == null)
      return;
    try
    {
      ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_26"), (object) this.storageName, (object) this.storageGuid);
      this.transaction.Commit();
      this.extConnections.CommitTransactions();
      this.transaction.Dispose();
      this.transaction = (SQLiteTransaction) null;
    }
    finally
    {
      this.CloseConnection();
      this.UnblockDataBase.Set();
    }
    ApplicationEventLog.Log.DebugFormat(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_27"), (object) this.storageName, (object) this.storageGuid);
  }

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
    if (this.connection != null)
    {
      this.connection.Close();
      this.connection = (SQLiteConnection) null;
    }
    this.extConnections.CloseConnections();
  }

  private SQLiteCommand CreateCommand(string commandText)
  {
    SQLiteCommand command = (SQLiteCommand) null;
    if (this.connection != null)
    {
      command = this.connection.CreateCommand();
      command.CommandText = commandText;
      if (this.transaction != null)
        command.Transaction = this.transaction;
    }
    return command;
  }

  private SQLiteCommand CreateCommand(string commandText, SQLiteParameter[] parameters)
  {
    SQLiteCommand command = this.CreateCommand(commandText);
    command.Parameters.Clear();
    if (parameters != null)
    {
      foreach (SQLiteParameter parameter in parameters)
        command.Parameters.Add(parameter);
    }
    return command;
  }
}
