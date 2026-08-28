// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PublishTask
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class PublishTask
{
  private static readonly string _cacheName = nameof (PublishTask);

  public string TaskFolder { get; }

  public IDBObject DBTask { [DebuggerStepThrough] get; }

  public PublishTask(IDBObject dbTask, string folderPath)
  {
    this.TaskFolder = folderPath;
    this.DBTask = dbTask;
  }

  public static PublishTask NewTask(
    IUserSession session,
    string name,
    string fileStorage,
    string enabledSites,
    out IDBObject dbTask)
  {
    IDBObjectCollection objectCollection = session.GetObjectCollection(new Guid("cad0149e-306c-11d8-b4e9-00304f19f545"));
    int attributeTypeId1 = MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeTaskNo);
    int attributeTypeId2 = MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeTaskType);
    DataTable dataTable = objectCollection.Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(attributeTypeId2, RelationalOperators.Equal, (object) 1, LogicalOperators.AND, 0, false)
    }, new ColumnDescriptor[1]
    {
      new ColumnDescriptor((object) attributeTypeId1, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.ASC, 0)
    }));
    int num = dataTable.Rows.Count > 0 ? Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1][0]) + 1 : 0;
    dbTask = objectCollection.Create();
    dbTask.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")), false, new object[1]
    {
      (object) name
    });
    dbTask.Attributes.AddAttribute(attributeTypeId1, false, new object[1]
    {
      (object) num
    });
    dbTask.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeTaskStatus), false, new object[1]
    {
      (object) 3
    });
    dbTask.Attributes.AddAttribute(attributeTypeId2, false, new object[1]
    {
      (object) 1
    });
    dbTask.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeTaskUser), false, new object[1]
    {
      (object) session.UserID
    });
    dbTask.CommitCreation(true);
    string backupTasksUnitPath = TempStorage.GetFolderBackupTasksUnitPath(dbTask.ObjectGUID.ToString());
    Directory.CreateDirectory(backupTasksUnitPath);
    PublishTask publishTask = new PublishTask(dbTask, backupTasksUnitPath);
    session.SetSessionPluginsData((object) PublishTask._cacheName, (object) new Tuple<long, PublishTask>(dbTask.ObjectID, publishTask));
    using (FileStream output = File.Open(Path.Combine(backupTasksUnitPath, ActionsHelper.TaskDataFileName), FileMode.Create, FileAccess.Write))
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output, Encoding.UTF8);
      try
      {
        if (enabledSites != null && enabledSites.Length > 0)
        {
          binaryWriter.Write(enabledSites.Length);
          binaryWriter.Write(enabledSites.ToCharArray());
        }
        else
          binaryWriter.Write(0);
      }
      finally
      {
        binaryWriter.Flush();
        binaryWriter.Close();
      }
    }
    return publishTask;
  }

  public static PublishTask GetPublishTask(IUserSession session, long taskID)
  {
    if (session.GetSessionPluginsData((object) PublishTask._cacheName) is Tuple<long, PublishTask> sessionPluginsData && sessionPluginsData.Item1 == taskID)
      return sessionPluginsData.Item2;
    IDBObject dbTask = session.GetObject(taskID);
    PublishTask publishTask = new PublishTask(dbTask, TempStorage.GetFolderBackupTasksUnitPath(dbTask.ObjectGUID.ToString()));
    session.SetSessionPluginsData((object) PublishTask._cacheName, (object) new Tuple<long, PublishTask>(dbTask.ObjectID, publishTask));
    return publishTask;
  }

  public TaskStatus Status
  {
    get => (TaskStatus) this.DBTask.GetAttributeByGuid(PortalConsts.attributeTaskStatus).AsInteger;
    set
    {
      IDBAttribute attributeByGuid1 = this.DBTask.GetAttributeByGuid(PortalConsts.attributeTaskStatus);
      if (attributeByGuid1.AsInteger != (long) value)
        attributeByGuid1.AsInteger = (long) value;
      if (!TaskStatusHelper.StatusInWork(value))
        return;
      IDBAttribute attributeByGuid2 = this.DBTask.GetAttributeByGuid(PortalConsts.attributeError);
      if (attributeByGuid2 == null)
        return;
      attributeByGuid2.AsString = string.Empty;
    }
  }

  public void ClearTempData()
  {
    try
    {
      if (!Directory.Exists(this.TaskFolder))
        return;
      Directory.Delete(this.TaskFolder, true);
    }
    catch (Exception ex)
    {
      if (!TraceLog.Enabled)
        return;
      TraceLog.Write("ClearTempData error: " + ex.Message);
    }
  }

  public void AddUnitData(TransferedObject unit)
  {
    string pathFromGuid = TempStorageHelper.CreatePathFromGuid(this.TaskFolder, unit.GUID);
    string path = Path.Combine(pathFromGuid, ActionsHelper.TransferedUnitFileName);
    Directory.CreateDirectory(pathFromGuid);
    using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
    {
      byte[] buffer = unit.Save();
      if (buffer == null || buffer.Length == 0)
        return;
      fileStream.Write(buffer, 0, buffer.Length);
    }
  }

  public string EnabledSites
  {
    get
    {
      string empty = string.Empty;
      BinaryReader br = new BinaryReader((Stream) File.Open(Path.Combine(this.TaskFolder, ActionsHelper.TaskDataFileName), FileMode.Open, FileAccess.Read), Encoding.UTF8);
      try
      {
        int length = br.ReadInt32();
        if (length > 0)
          empty = ActionsHelper.GetString(length, br);
        if (TraceLog.Enabled)
          TraceLog.Write("...enabledSites=" + empty);
        return empty;
      }
      finally
      {
        br.Close();
      }
    }
  }
}
