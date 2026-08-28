// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.DBPortalTask
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class DBPortalTask(UserSession uSession, DataTable objectParams) : DBObject(uSession, objectParams)
{
  public override int Delete(long deleteMode)
  {
    string backupTasksUnitPath = TempStorage.GetFolderBackupTasksUnitPath(this.ObjectGUID.ToString());
    if (Directory.Exists(backupTasksUnitPath))
    {
      if (deleteMode != (long) PortalConsts.DeleteWithoutFiles)
      {
        foreach (FileInfo file in BackupTaskUnitFiles.FindFiles(backupTasksUnitPath))
        {
          if (file.Exists)
          {
            string path = Path.Combine(TempStorage.GetPublishUnitPath(TransferedObject.LoadFromFile(file.FullName).GUID));
            if (Directory.Exists(path))
              Directory.Delete(path, true);
          }
        }
      }
      Directory.Delete(backupTasksUnitPath, true);
    }
    return base.Delete(deleteMode != (long) PortalConsts.DeleteWithoutFiles ? deleteMode : 0L);
  }
}
