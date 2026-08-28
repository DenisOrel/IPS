// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.DBChanges
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using System;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class DBChanges(UserSession uSession, DataTable objectParams) : DBObject(uSession, objectParams)
{
  protected override void DoDelete()
  {
    IDBAttribute attributeByGuid = this.GetAttributeByGuid(PortalServerConsts.attributeUpdateData);
    if (attributeByGuid == null || attributeByGuid.IsNull)
      return;
    foreach (TransferedObject transferedObject in UpdateDataAttributeHelper.Load(attributeByGuid, false, false))
    {
      string updateUnitPath = TempStorage.GetUpdateUnitPath(transferedObject.GUID);
      if (Directory.Exists(updateUnitPath))
        this.DeleteDirectory(updateUnitPath);
    }
    base.DoDelete();
  }

  private void DeleteDirectory(string tempDir)
  {
    try
    {
      Directory.Delete(tempDir, true);
    }
    catch (Exception ex)
    {
      (this.EventHelper as EventLogHelper).AddToTrace($"Ошибка удаления временной папки {tempDir} при удалении обновления ObjectId={this.ObjectID}: {ex.Message}", Consts.traceAlways, string.Empty);
    }
  }
}
