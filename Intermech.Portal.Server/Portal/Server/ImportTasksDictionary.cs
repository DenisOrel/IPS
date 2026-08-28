// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.ImportTasksDictionary
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class ImportTasksDictionary
{
  private readonly Dictionary<Guid, ImportInfo> _importTasks = new Dictionary<Guid, ImportInfo>();
  private readonly object _dataLock = new object();

  public void AddTask(Guid guid, ImportInfo info)
  {
    lock (this._dataLock)
      this._importTasks.Add(guid, info);
  }

  public void RemoveTask(Guid guid)
  {
    if (!this._importTasks.ContainsKey(guid))
      return;
    lock (this._dataLock)
      this._importTasks.Remove(guid);
  }

  public ImportInfo GetInfo(Guid guid)
  {
    ImportInfo importInfo;
    return !this._importTasks.TryGetValue(guid, out importInfo) ? (ImportInfo) null : importInfo;
  }

  public void SetPersent(Guid guid, int percent)
  {
    ImportInfo importInfo;
    if (!this._importTasks.TryGetValue(guid, out importInfo))
      return;
    lock (this._dataLock)
      importInfo.Persent = percent;
  }

  public void SetStatus(Guid guid, ImportTaskStatuses importTaskStatus)
  {
    ImportInfo importInfo;
    if (!this._importTasks.TryGetValue(guid, out importInfo))
      return;
    lock (this._dataLock)
      importInfo.ImportTaskStatus = importTaskStatus;
  }

  public void SetError(Guid guid, Exception ex)
  {
    ImportInfo importInfo;
    if (!this._importTasks.TryGetValue(guid, out importInfo))
      return;
    lock (this._dataLock)
    {
      importInfo.ImportTaskStatus = ImportTaskStatuses.Error;
      importInfo.ErrorMessage = ex.Message;
      importInfo.ErrorStack = this.FormingErrorString(ex, true);
    }
  }

  private string FormingErrorString(Exception ex, bool stackOnly)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (!stackOnly)
      stringBuilder.AppendLine(ex.Message);
    stringBuilder.AppendLine(ex.StackTrace);
    if (ex.InnerException != null)
      stringBuilder.AppendLine(this.FormingErrorString(ex.InnerException, false));
    return stringBuilder.ToString();
  }
}
