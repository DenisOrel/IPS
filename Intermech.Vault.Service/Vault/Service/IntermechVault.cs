// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.IntermechVault
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Interfaces.Server;
using Intermech.Vault.Interfaces;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml;

#nullable disable
namespace Intermech.Vault.Service;

public class IntermechVault : MarshalByRefObject, IIntermechVault, IDisposable
{
  public static object SyncRoot = new object();
  internal UndertakerThread DisconnectThread;

  public IntermechVault()
  {
    CommonVariables.XmlFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), CommonVariables.XML_FILE_NAME);
    if (!File.Exists(CommonVariables.XmlFilePath))
      this.CreateXmlFile();
    else
      this.LoadSettings();
    this.DisconnectThread = new UndertakerThread();
  }

  private void CreateXmlFile()
  {
    XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage(CommonVariables.XmlFilePath);
    XmlNode documentElement = (XmlNode) xmlSettingsStorage.document.DocumentElement;
    CommonVariables.HistoryLife = 0U;
    CommonVariables.DeletedLife = 365U;
    CommonVariables.FullLogging = false;
    CommonVariables.MaxVolumeSize = 4700000000L;
    CommonVariables.SyncModeOff = false;
    CommonVariables.Password = "yYszkfOjnefxNoPk13ib/5arHkk=";
    xmlSettingsStorage.SetAttributeValue(documentElement, "history", CommonVariables.HistoryLife.ToString());
    xmlSettingsStorage.SetAttributeValue(documentElement, "trash", CommonVariables.DeletedLife.ToString());
    xmlSettingsStorage.SetAttributeValue(documentElement, "full_logging", CommonVariables.FullLogging.ToString());
    xmlSettingsStorage.SetAttributeValue(documentElement, "folder_size", CommonVariables.MaxVolumeSize.ToString());
    xmlSettingsStorage.SetAttributeValue(documentElement, "syncmodeoff", CommonVariables.SyncModeOff.ToString());
    xmlSettingsStorage.SetAttributeValue(documentElement, "password", CommonVariables.Password);
    xmlSettingsStorage.AddNode(documentElement, "root_folders");
    xmlSettingsStorage.Save(CommonVariables.XmlFilePath);
  }

  private void LoadSettings()
  {
    XMLSettingsStorage xmlSettingsStorage = new XMLSettingsStorage(CommonVariables.XmlFilePath);
    XmlNode documentElement = (XmlNode) xmlSettingsStorage.document.DocumentElement;
    CommonVariables.HistoryLife = Convert.ToUInt32(documentElement.Attributes["history"].Value);
    CommonVariables.DeletedLife = Convert.ToUInt32(documentElement.Attributes["trash"].Value);
    CommonVariables.FullLogging = Convert.ToBoolean(documentElement.Attributes["full_logging"].Value);
    CommonVariables.MaxVolumeSize = Convert.ToInt64(documentElement.Attributes["folder_size"].Value);
    CommonVariables.Password = documentElement.Attributes["password"].Value;
    if (documentElement.Attributes["syncmodeoff"] != null)
      CommonVariables.SyncModeOff = Convert.ToBoolean(documentElement.Attributes["syncmodeoff"].Value);
    foreach (XmlNode childNode in xmlSettingsStorage.FindNode(documentElement, "root_folders", false).ChildNodes)
      CommonVariables.AddRootDirectory(new RootDirectory(childNode.Attributes["path"].Value, Convert.ToInt16(childNode.Attributes["max_size"].Value), childNode.Attributes["guid"].Value));
  }

  public IVaultSettings Login(string password) => VaultSettings.Login(password);

  public IDiskFileStorage Login(
    string storageGuid,
    string storagName,
    string password,
    string mName)
  {
    lock (IntermechVault.SyncRoot)
      return (IDiskFileStorage) DiskFileStorage.Login(storageGuid, storagName, password, mName);
  }

  public IDiskFileStorage CreateStorage(
    string storageGuid,
    string storageName,
    string password,
    string mName)
  {
    lock (IntermechVault.SyncRoot)
      return (IDiskFileStorage) DiskFileStorage.CreateStorage(storageGuid, storageName, password, mName);
  }

  public void Dispose()
  {
    this.DisconnectThread.Thread.Abort();
    this.DisconnectThread.Thread = (Thread) null;
    this.DisconnectThread = (UndertakerThread) null;
    DiskFileStorageCollection.RemoveAllConnections();
  }

  public override object InitializeLifetimeService() => (object) null;
}
