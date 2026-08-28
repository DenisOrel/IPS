// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.StorageSecurity
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Localization;
using Intermech.Vault.Interfaces.Server;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

#nullable disable
namespace Intermech.Vault.Service;

internal static class StorageSecurity
{
  private static SecurityIdentifier localAdminsGroupSID = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, (SecurityIdentifier) null);

  public static void SetRootSecurity(DirectoryInfo storageDirectory)
  {
    ApplicationEventLog.Log.DebugFormat("storageDirectory={0}", (object) storageDirectory.FullName);
    DirectorySecurity directorySecurity = new DirectorySecurity();
    DirectorySecurity accessControl = storageDirectory.GetAccessControl(AccessControlSections.Access);
    accessControl.SetAccessRuleProtection(true, false);
    accessControl.SetOwner((IdentityReference) StorageSecurity.localAdminsGroupSID);
    accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference) StorageSecurity.localAdminsGroupSID, FileSystemRights.Modify | FileSystemRights.ChangePermissions | FileSystemRights.TakeOwnership | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
    storageDirectory.SetAccessControl(accessControl);
  }

  public static void AddDirectoryDeleteRights(string dirPath)
  {
    if (Directory.Exists(dirPath))
    {
      ApplicationEventLog.Log.DebugFormat("dirPath={0}", (object) dirPath);
      DirectorySecurity accessControl = Directory.GetAccessControl(dirPath, AccessControlSections.Access);
      accessControl.RemoveAccessRuleAll(new FileSystemAccessRule((IdentityReference) StorageSecurity.localAdminsGroupSID, FileSystemRights.DeleteSubdirectoriesAndFiles, AccessControlType.Allow));
      accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference) StorageSecurity.localAdminsGroupSID, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
      Directory.SetAccessControl(dirPath, accessControl);
    }
    else
      ApplicationEventLog.Log.DebugFormat(LocalizationHolder.rm.GetString("VaultService_63"), (object) dirPath);
  }

  public static void RemoveFileDeleteRights(string filePath)
  {
    if (File.Exists(filePath))
    {
      ApplicationEventLog.Log.DebugFormat("filePath={0}", (object) filePath);
      FileSecurity accessControl = File.GetAccessControl(filePath, AccessControlSections.Access);
      accessControl.SetAccessRuleProtection(true, false);
      accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference) StorageSecurity.localAdminsGroupSID, FileSystemRights.ReadAndExecute | FileSystemRights.Write | FileSystemRights.DeleteSubdirectoriesAndFiles | FileSystemRights.ChangePermissions | FileSystemRights.TakeOwnership | FileSystemRights.Synchronize, AccessControlType.Allow));
      accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference) StorageSecurity.localAdminsGroupSID, FileSystemRights.Delete, AccessControlType.Deny));
      File.SetAccessControl(filePath, accessControl);
    }
    else
      ApplicationEventLog.Log.DebugFormat(LocalizationHolder.rm.GetString("VaultService_64"), (object) filePath);
  }

  public static void AddFileDeleteRights(string filePath)
  {
    if (File.Exists(filePath))
    {
      ApplicationEventLog.Log.DebugFormat("filePath={0}", (object) filePath);
      FileSecurity accessControl = File.GetAccessControl(filePath, AccessControlSections.Access);
      accessControl.RemoveAccessRule(new FileSystemAccessRule((IdentityReference) StorageSecurity.localAdminsGroupSID, FileSystemRights.Delete, AccessControlType.Deny));
      accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference) StorageSecurity.localAdminsGroupSID, FileSystemRights.Delete, AccessControlType.Allow));
      File.SetAccessControl(filePath, accessControl);
    }
    else
      ApplicationEventLog.Log.DebugFormat(LocalizationHolder.rm.GetString("VaultService_64"), (object) filePath);
  }

  public static void RemoveDirectoryDeleteRights(string dirPath)
  {
    if (Directory.Exists(dirPath))
    {
      ApplicationEventLog.Log.DebugFormat("dirPath={0}", (object) dirPath);
      DirectorySecurity accessControl = Directory.GetAccessControl(dirPath, AccessControlSections.Access);
      accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference) StorageSecurity.localAdminsGroupSID, FileSystemRights.Delete, AccessControlType.Deny));
      Directory.SetAccessControl(dirPath, accessControl);
    }
    else
      ApplicationEventLog.Log.DebugFormat(LocalizationHolder.rm.GetString("VaultService_63"), (object) dirPath);
  }
}
