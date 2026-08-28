// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.VaultSettings
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Protection;
using Intermech.Vault.Interfaces;
using Intermech.Vault.Interfaces.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Vault.Service;

public class VaultSettings : MarshalByRefObject, IVaultSettings
{
  private object SyncRoot = new object();

  public static IVaultSettings Login(string password)
  {
    if (password == CommonVariables.Password)
      return (IVaultSettings) new VaultSettings();
    throw new VaultException(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_65"));
  }

  public bool ValidatePassword(string password)
  {
    return CommonVariables.Password == CryptHelper.CryptPassword(password, CryptHelper.DVSCrypt);
  }

  public bool SyncModeOff
  {
    get
    {
      lock (this.SyncRoot)
        return CommonVariables.SyncModeOff;
    }
    set
    {
      lock (this.SyncRoot)
      {
        CommonVariables.SyncModeOff = value;
        this.ChangeSettings("syncmodeoff", value.ToString());
      }
    }
  }

  public string Password
  {
    set
    {
      lock (this.SyncRoot)
      {
        string attrValue = CryptHelper.CryptPassword(value, CryptHelper.DVSCrypt);
        this.ChangeSettings("password", attrValue);
        CommonVariables.Password = attrValue;
      }
      ApplicationEventLog.Log.InfoFormat(EventStringMessage.PASSWORD_CHANGE);
    }
  }

  public long CurrentVolumeSize
  {
    get
    {
      lock (this.SyncRoot)
        return CommonVariables.MaxVolumeSize;
    }
    set
    {
      lock (this.SyncRoot)
      {
        CommonVariables.MaxVolumeSize = value;
        this.ChangeSettings("folder_size", value.ToString());
      }
      ApplicationEventLog.Log.InfoFormat(EventStringMessage.VOLUME_SIZE_CHANGE, (object) value);
    }
  }

  public uint HistoryLifeTime
  {
    get
    {
      lock (this.SyncRoot)
        return CommonVariables.HistoryLife;
    }
    set
    {
      lock (this.SyncRoot)
      {
        CommonVariables.HistoryLife = value;
        this.ChangeSettings("history", value.ToString());
      }
      ApplicationEventLog.Log.InfoFormat(EventStringMessage.HISTORY_LIFETIME_CHANGE, (object) value);
    }
  }

  public uint DeletedLifeTime
  {
    get
    {
      lock (this.SyncRoot)
        return CommonVariables.DeletedLife;
    }
    set
    {
      lock (this.SyncRoot)
      {
        CommonVariables.DeletedLife = value;
        this.ChangeSettings("trash", value.ToString());
      }
      ApplicationEventLog.Log.InfoFormat(EventStringMessage.DELETED_LIFETIME_CHANGE, (object) value);
    }
  }

  public bool IsFullLogging
  {
    get
    {
      lock (this.SyncRoot)
        return CommonVariables.FullLogging;
    }
    set
    {
      lock (this.SyncRoot)
      {
        this.ChangeSettings("full_logging", value.ToString());
        ApplicationEventLog.LoggingTypeChange(value);
        CommonVariables.FullLogging = value;
      }
      if (!value)
        return;
      ApplicationEventLog.Log.InfoFormat(EventStringMessage.FULL_LOGGING_START);
    }
  }

  public string EventLogFileName => CommonVariables.EventLogPath;

  public DataTable CurrentConnections => DiskFileStorageCollection.GetConnections();

  public List<RootDirectory> RootDirectoriesList
  {
    get
    {
      return new List<RootDirectory>((IEnumerable<RootDirectory>) CommonVariables.RootDirectoriesList);
    }
  }

  public void ChangeRootDirectorySize(string storageGuid, string storageName, short percent)
  {
    lock (this.SyncRoot)
    {
      RootDirectory rootDirectory = CommonVariables.GetRootDirectory(storageName, storageGuid);
      XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage(CommonVariables.XmlFilePath);
      xmlSettingsStorage.FindNodeWithAttr(xmlSettingsStorage.FindNode((XmlNode) xmlSettingsStorage.document.DocumentElement, "root_folders", false), "storage", "guid", storageGuid, false).Attributes["max_size"].Value = percent.ToString();
      xmlSettingsStorage.Save(CommonVariables.XmlFilePath);
      int num = (int) percent;
      rootDirectory.MaxSize = (short) num;
    }
  }

  public RootDirectory AddRootFolder(string path, short maxSize)
  {
    lock (this.SyncRoot)
    {
      XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage(CommonVariables.XmlFilePath);
      XmlNode node = xmlSettingsStorage.AddNode(xmlSettingsStorage.FindNode((XmlNode) xmlSettingsStorage.document.DocumentElement, "root_folders", false), "storage");
      xmlSettingsStorage.SetAttributeValue(node, nameof (path), path);
      xmlSettingsStorage.SetAttributeValue(node, "guid", string.Empty);
      xmlSettingsStorage.SetAttributeValue(node, "max_size", maxSize.ToString());
      xmlSettingsStorage.Save(CommonVariables.XmlFilePath);
      RootDirectory root = new RootDirectory(path, maxSize, string.Empty);
      CommonVariables.AddRootDirectory(root);
      return root;
    }
  }

  public RootDirectory RestoreRootFolder(string path, string guid, short maxSize)
  {
    lock (this.SyncRoot)
    {
      XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage(CommonVariables.XmlFilePath);
      XmlNode node = xmlSettingsStorage.AddNode(xmlSettingsStorage.FindNode((XmlNode) xmlSettingsStorage.document.DocumentElement, "root_folders", false), "storage");
      xmlSettingsStorage.SetAttributeValue(node, nameof (path), path);
      xmlSettingsStorage.SetAttributeValue(node, nameof (guid), guid);
      xmlSettingsStorage.SetAttributeValue(node, "max_size", maxSize.ToString());
      xmlSettingsStorage.Save(CommonVariables.XmlFilePath);
      RootDirectory root = new RootDirectory(path, maxSize, guid);
      CommonVariables.AddRootDirectory(root);
      return root;
    }
  }

  public List<string> GetNamesForStorage(string guid)
  {
    List<string> namesForStorage = new List<string>();
    foreach (RootDirectory rootDirectories in this.RootDirectoriesList)
    {
      if (rootDirectories.Guid == guid && !namesForStorage.Contains(rootDirectories.StorageName))
        namesForStorage.Add(rootDirectories.StorageName);
    }
    return namesForStorage;
  }

  public void DeleteRootFolder(string storageGuid, string storageName)
  {
    this.DeleteRootFolder(storageGuid, storageName, true);
  }

  private void DeleteRootFolder(string storageGuid, string storageName, bool deleteFromSettings)
  {
    lock (IntermechVault.SyncRoot)
    {
      RootDirectory rootDirectory = CommonVariables.GetRootDirectory(storageName, storageGuid);
      if (rootDirectory != null)
      {
        string path = rootDirectory.Path;
        if (Directory.Exists(path))
        {
          StorageSecurity.AddDirectoryDeleteRights(path);
          Directory.Delete(rootDirectory.Path, true);
        }
      }
      if (!deleteFromSettings)
        return;
      this.DeleteRootFromSettings(rootDirectory);
    }
  }

  private void DeleteRootFromSettings(RootDirectory deletedDirectory)
  {
    CommonVariables.RemoveRootDirectory(deletedDirectory);
    XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage(CommonVariables.XmlFilePath);
    XmlNode node = xmlSettingsStorage.FindNode((XmlNode) xmlSettingsStorage.document.DocumentElement, "root_folders", false);
    XmlNode nodeWithAttr = xmlSettingsStorage.FindNodeWithAttr(node, "storage", "guid", deletedDirectory.Guid, false);
    if (nodeWithAttr != null)
      node.RemoveChild(nodeWithAttr);
    xmlSettingsStorage.Save(CommonVariables.XmlFilePath);
  }

  public ICopierRootDirectory ReplaceRootDirectory(
    RootDirectory sourceRootDirectory,
    string rootDestPath)
  {
    lock (this.SyncRoot)
    {
      if (DiskFileStorageCollection.IsConnectionExists(sourceRootDirectory.Guid, sourceRootDirectory.StorageName))
        throw new VaultException(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_66"));
      try
      {
        CommonVariables.RemoveRootDirectory(sourceRootDirectory);
        return (ICopierRootDirectory) new CopierRootDirectory(sourceRootDirectory, rootDestPath);
      }
      catch (Exception ex)
      {
        throw new VaultException(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_67"), ex);
      }
    }
  }

  public void CompleteReplaceDirectory(string source, string path, string guid, short maxSize)
  {
    lock (this.SyncRoot)
    {
      XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage(CommonVariables.XmlFilePath);
      XmlNode nodeWithAttr = xmlSettingsStorage.FindNodeWithAttr(xmlSettingsStorage.FindNode((XmlNode) xmlSettingsStorage.document.DocumentElement, "root_folders", false), "storage", nameof (guid), guid, false);
      if (nodeWithAttr != null)
        nodeWithAttr.Attributes[nameof (path)].Value = path;
      xmlSettingsStorage.Save(CommonVariables.XmlFilePath);
      CommonVariables.AddRootDirectory(new RootDirectory(path, maxSize, guid));
    }
    if (!Directory.Exists(source))
      return;
    StorageSecurity.AddDirectoryDeleteRights(source);
    try
    {
      Directory.Delete(source, true);
    }
    catch
    {
    }
  }

  private void ChangeSettings(string attrName, string attrValue)
  {
    XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage(CommonVariables.XmlFilePath);
    xmlSettingsStorage.SetAttributeValue((XmlNode) xmlSettingsStorage.document.DocumentElement, attrName, attrValue);
    xmlSettingsStorage.Save(CommonVariables.XmlFilePath);
  }

  public override object InitializeLifetimeService() => (object) null;
}
