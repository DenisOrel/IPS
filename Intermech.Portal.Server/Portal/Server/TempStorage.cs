// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.TempStorage
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces.WebPortal;
using System;
using System.IO;

#nullable disable
namespace Intermech.Portal.Server;

internal class TempStorage
{
  public static string RootFolder;

  public static void Initialize(string fileStorage)
  {
    if (fileStorage != null && fileStorage != string.Empty)
    {
      DirectoryInfo directoryInfo = new DirectoryInfo(fileStorage);
      if (!directoryInfo.Exists)
        Directory.CreateDirectory(directoryInfo.FullName);
      TempStorage.RootFolder = directoryInfo.FullName;
    }
    else
      TempStorage.RootFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PortalConsts.StorageFolder);
    TempStorage.CheckDirectory(PortalServerConsts.FolderPublishObjects);
    TempStorage.CheckDirectory(PortalServerConsts.FolderUpdatesObjects);
  }

  public static void CheckAndCreateLocDirectory(string rootDirectory, string fileName)
  {
    string directoryName = Path.GetDirectoryName(fileName);
    if (!(directoryName != string.Empty))
      return;
    string path = Path.Combine(rootDirectory, directoryName);
    if (Directory.Exists(path))
      return;
    Directory.CreateDirectory(path);
  }

  public static void CheckDirectory(string dirName)
  {
    DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(TempStorage.RootFolder, dirName));
    if (directoryInfo.Exists)
      return;
    Directory.CreateDirectory(directoryInfo.FullName);
  }

  public static string GetUpdateUnitPath(string unitGuid)
  {
    return TempStorageHelper.CreatePathFromGuid(Path.Combine(TempStorage.RootFolder, PortalServerConsts.FolderUpdatesObjects), unitGuid);
  }

  public static string GetFolderBackupTasksUnitPath(string taskGuid)
  {
    return TempStorageHelper.CreatePathFromGuid(Path.Combine(TempStorage.RootFolder, PortalServerConsts.FolderBackupTasks), taskGuid);
  }

  public static string GetPublishUnitPath(string unitGuid)
  {
    return TempStorageHelper.CreatePathFromGuid(Path.Combine(TempStorage.RootFolder, PortalServerConsts.FolderPublishObjects), unitGuid);
  }
}
