// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.Tools.ClearPublishTrashFoldersTask
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using System;
using System.IO;

#nullable disable
namespace Intermech.Portal.Server.Tools;

internal class ClearPublishTrashFoldersTask : DBCustomManualScheduledService
{
  private readonly string _tempStoragePath;
  private readonly IEventLogHelper _eventLog;
  private readonly string _logFileName = "clear_publish_task.log";

  public ClearPublishTrashFoldersTask(string tempStoragePath)
  {
    this._eventLog = ServerServices.GetService(typeof (IEventLogHelper)) as IEventLogHelper;
    this._tempStoragePath = tempStoragePath;
  }

  public override Guid GUID => new Guid("{72A5B65E-D360-4E92-8A34-A2B55D9A3C62}");

  public override string ServiceName
  {
    get => "Очистка устаревших папок с временными данными для для публикаций узлов";
  }

  public override bool ProcessEvent(TimedEventProperties properties)
  {
    try
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(Path.Combine(this._tempStoragePath, PortalServerConsts.FolderPublishObjects));
      this._eventLog.AddToTrace("Старт очистки " + directoryInfo1.FullName, Consts.traceAlways, this._logFileName, this.Session.ComputerName, this.Session.UserName);
      DirectoryInfo[] directories = directoryInfo1.GetDirectories();
      int num = 0;
      foreach (DirectoryInfo directoryInfo2 in directories)
      {
        if ((DateTime.Now - directoryInfo2.CreationTime).TotalDays >= 1.0)
        {
          try
          {
            Directory.Delete(directoryInfo2.FullName, true);
            this._eventLog.AddToTrace(directoryInfo2.FullName + " удалена", Consts.traceAlways, this._logFileName);
            ++num;
          }
          catch (Exception ex)
          {
            this._eventLog.TraceExeption($"Ошибка при удалении временной папки {directoryInfo2.FullName} для публикации", ex, this._logFileName);
          }
        }
      }
      this._eventLog.AddToTrace($"Очистка завершена. Удалено {num} папок.", Consts.traceAlways, this._logFileName);
      return true;
    }
    catch (Exception ex)
    {
      this._eventLog.TraceExeption("Ошибка при очистке папок с временными данными для публикации узлов", ex, this._logFileName);
      return false;
    }
  }
}
