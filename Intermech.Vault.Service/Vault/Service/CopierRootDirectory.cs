// Decompiled with JetBrains decompiler
// Type: Intermech.Vault.Service.CopierRootDirectory
// Assembly: Intermech.Vault.Service, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 24B0DA67-997A-4E40-A745-DAEE647016D5
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Vault.Service.exe

using Intermech.Vault.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

#nullable disable
namespace Intermech.Vault.Service;

[Serializable]
internal class CopierRootDirectory : ICopierRootDirectory
{
  private CopierState state = CopierState.Stop;
  private bool cancel;
  private RootDirectory sourceDirectory;
  private string destRootDirectoryPath;

  public event EventHandler IndexCompletedEvent;

  public event EventHandler ItemMoveEvent;

  public event EventHandler MoveErrorEvent;

  public event EventHandler MoveCompleteEvent;

  public string SourceDirectoryPath => this.sourceDirectory.Path;

  public CopierState CopierState => this.state;

  public CopierRootDirectory(RootDirectory sourceRootDirectory, string destPath)
  {
    this.destRootDirectoryPath = destPath;
    this.sourceDirectory = sourceRootDirectory;
  }

  public void StartDirectoryReplace()
  {
    try
    {
      this.state = this.state == CopierState.Stop ? CopierState.Start : throw new Exception(Intermech.Localization.LocalizationHolder.rm.GetString("VaultService_10"));
      this.cancel = false;
      new Thread(new ThreadStart(this.DoWork))
      {
        Name = "RootDirectoryReplace",
        IsBackground = true
      }.Start();
    }
    catch
    {
      throw;
    }
  }

  public void Cancel() => this.cancel = true;

  private void DoWork()
  {
    try
    {
      List<DirectoryInfo> foldersPath = new List<DirectoryInfo>();
      List<FileInfo> filesPath = new List<FileInfo>();
      this.state = CopierState.Indexing;
      if (this.cancel)
        return;
      string path = this.sourceDirectory.Path;
      this.FolderIndexing(new DirectoryInfo(path), foldersPath, filesPath);
      this.OnIndexCompleted((object) this, new IndexCompleteEventArgs(filesPath.Count, foldersPath.Count));
      if (this.cancel)
        return;
      this.state = CopierState.CreateFolders;
      int count = path.Length - Path.GetFileName(path).Length;
      foreach (DirectoryInfo directoryInfo in foldersPath)
      {
        if (this.cancel)
          return;
        string str = Path.Combine(this.destRootDirectoryPath, directoryInfo.FullName.Remove(0, count));
        try
        {
          if (!Directory.Exists(str))
          {
            DirectoryInfo directory = Directory.CreateDirectory(str);
            this.OnItemMoved((object) this, new ItemMovedEventArgs(directoryInfo.FullName, str, true));
            if (Path.GetFileName(str) == Path.GetFileName(path))
              StorageSecurity.SetRootSecurity(directory);
            else
              StorageSecurity.RemoveDirectoryDeleteRights(str);
          }
        }
        catch (Exception ex)
        {
          this.OnMoveErrorEvent((object) this, new MoveErrorEventArgs(this.sourceDirectory, ex));
          return;
        }
      }
      this.state = CopierState.MoveFiles;
      if (this.cancel)
        return;
      foreach (FileInfo fileInfo in filesPath)
      {
        if (this.cancel)
          return;
        string str = Path.Combine(this.destRootDirectoryPath, fileInfo.FullName.Remove(0, count));
        try
        {
          if (!File.Exists(str))
          {
            StorageSecurity.AddFileDeleteRights(fileInfo.FullName);
            File.Move(fileInfo.FullName, str);
            StorageSecurity.RemoveFileDeleteRights(str);
            this.OnItemMoved((object) this, new ItemMovedEventArgs(fileInfo.FullName, str, false));
          }
        }
        catch (Exception ex)
        {
          this.OnMoveErrorEvent((object) this, new MoveErrorEventArgs(this.sourceDirectory, ex));
          return;
        }
      }
      this.OnMoveCompleteEvent((object) this, new MoveCompleteEventArgs(this.sourceDirectory, Path.Combine(this.destRootDirectoryPath, Path.GetFileName(this.sourceDirectory.Path))));
    }
    catch (Exception ex)
    {
      this.OnMoveErrorEvent((object) this, new MoveErrorEventArgs(this.sourceDirectory, ex));
    }
    finally
    {
      this.state = CopierState.Stop;
    }
  }

  private void FolderIndexing(
    DirectoryInfo folderInfo,
    List<DirectoryInfo> foldersPath,
    List<FileInfo> filesPath)
  {
    foldersPath.Add(folderInfo);
    foreach (FileInfo file in folderInfo.GetFiles())
      filesPath.Add(file);
    foreach (DirectoryInfo directory in folderInfo.GetDirectories())
      this.FolderIndexing(directory, foldersPath, filesPath);
  }

  private void OnIndexCompleted(object sender, IndexCompleteEventArgs e)
  {
    if (this.IndexCompletedEvent == null)
      return;
    this.IndexCompletedEvent(sender, (EventArgs) e);
  }

  private void OnItemMoved(object sender, ItemMovedEventArgs e)
  {
    if (this.ItemMoveEvent == null)
      return;
    this.ItemMoveEvent(sender, (EventArgs) e);
  }

  private void OnMoveErrorEvent(object sender, MoveErrorEventArgs e)
  {
    if (this.MoveErrorEvent == null)
      return;
    this.MoveErrorEvent(sender, (EventArgs) e);
  }

  private void OnMoveCompleteEvent(object sender, MoveCompleteEventArgs e)
  {
    if (this.MoveCompleteEvent == null)
      return;
    this.MoveCompleteEvent(sender, (EventArgs) e);
  }
}
