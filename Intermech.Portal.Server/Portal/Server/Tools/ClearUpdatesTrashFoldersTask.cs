// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.Tools.ClearUpdatesTrashFoldersTask
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.Portal.Server.Tools;

internal class ClearUpdatesTrashFoldersTask : DBCustomManualScheduledService
{
  private readonly string _tempStoragePath;
  private readonly IEventLogHelper _eventLog;
  private readonly string _logFileName = "clear_update_task.log";

  public ClearUpdatesTrashFoldersTask(string tempStoragePath)
  {
    this._eventLog = ServerServices.GetService(typeof (IEventLogHelper)) as IEventLogHelper;
    this._tempStoragePath = tempStoragePath;
  }

  public override Guid GUID => new Guid("{5599FD7C-7CF9-4E64-80DA-3CD93057DEA6}");

  public override string ServiceName
  {
    get => "Очистка неиспользуемых папок с временными данными для обновлений узлов";
  }

  public override bool ProcessEvent(TimedEventProperties properties)
  {
    try
    {
      DataTable dataTable = this.Session.GetObjectCollection(PortalConsts.objtypeChanges).Select(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
      {
        (object) ObligatoryObjectAttributes.F_OBJECT_ID
      }));
      List<string> stringList = new List<string>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        List<string> foldersForChange = this.GetFoldersForChange((IUserSession) this.Session, Convert.ToInt64(row[0]));
        if (foldersForChange.Count > 0)
          stringList.AddRange((IEnumerable<string>) foldersForChange);
      }
      DirectoryInfo directoryInfo1 = new DirectoryInfo(Path.Combine(this._tempStoragePath, PortalServerConsts.FolderUpdatesObjects));
      this._eventLog.AddToTrace("Старт очистки " + directoryInfo1.FullName, Consts.traceAlways, this._logFileName, this.Session.ComputerName, this.Session.UserName);
      DirectoryInfo[] directories = directoryInfo1.GetDirectories();
      int num = 0;
      foreach (DirectoryInfo directoryInfo2 in directories)
      {
        if (!stringList.Contains(directoryInfo2.Name.ToLower()))
        {
          try
          {
            Directory.Delete(directoryInfo2.FullName, true);
            this._eventLog.AddToTrace(directoryInfo2.FullName + " удалена", Consts.traceAlways, this._logFileName);
            ++num;
          }
          catch (Exception ex)
          {
            this._eventLog.TraceExeption($"Ошибка при удалении временной папки {directoryInfo2.FullName} для обновления", ex, this._logFileName);
          }
        }
      }
      this._eventLog.AddToTrace($"Очистка завершена. Удалено {num} папок.", Consts.traceAlways, this._logFileName);
      return true;
    }
    catch (Exception ex)
    {
      this._eventLog.TraceExeption("Ошибка при очистке папок с временными данными для обновлений узлов", ex, this._logFileName);
      return false;
    }
  }

  private List<string> GetFoldersForChange(IUserSession session, long changeID)
  {
    IDBObject dbObject = session.GetObject(changeID, false);
    if (dbObject != null)
    {
      try
      {
        List<TransferedObject> transferedObjectList = UpdateDataAttributeHelper.Load(dbObject.GetAttributeByGuid(PortalServerConsts.attributeUpdateData), false, false);
        if (transferedObjectList.Count > 0)
          return transferedObjectList.ConvertAll<string>((Converter<TransferedObject, string>) (x => x.GUID.ToLower()));
      }
      catch (Exception ex)
      {
        this._eventLog.TraceExeption("Ошибка при получении данные задачи синхронизации", ex, this._logFileName);
      }
    }
    return new List<string>(0);
  }
}
