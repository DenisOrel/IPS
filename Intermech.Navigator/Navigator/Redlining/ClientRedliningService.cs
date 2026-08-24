// Decompiled with JetBrains decompiler
// Type: Intermech.Navigator.Redlining.ClientRedliningService
// Assembly: Intermech.Navigator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FA68CCDA-C8AC-453D-A97D-7A56D5366A1E
// Assembly location: D:\IPS\Client\Intermech.Navigator.dll

using Intermech.Collections;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.IO;
using Intermech.Localization;
using Intermech.Redline;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.Navigator.Redlining;

internal class ClientRedliningService : IClientRedliningService
{
  private object syncRoot = new object();
  internal long generation;
  internal List<Tuple<long, string>> redlining = new List<Tuple<long, string>>();
  internal List<RedliningFiles> redliningSettings = new List<RedliningFiles>();
  internal ICurrentUserAndRole userAndRole;
  internal INotificationService notifyService;
  internal NotificationEventHandler notifyHandler;
  private readonly IFileVault fileVault;

  public ClientRedliningService()
  {
    this.fileVault = ClientContext.FileVault;
    this.InitServices();
  }

  private void InitServices()
  {
    if (ServicesManager.GetService(typeof (IClientRedliningService)) is IClientRedliningService)
      throw new Exception(LocalizationHolder.rm.GetString("Navigator_8"));
    if (this.userAndRole != null)
      return;
    this.userAndRole = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
    this.notifyService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
    if (this.notifyHandler == null)
    {
      this.notifyHandler = new NotificationEventHandler(this.NotificationEventFired);
      this.notifyService.Subscribe("RedliningChanged", this.notifyHandler);
    }
    ServicesManager.AddService(typeof (IClientRedliningService), (object) this);
    this.CheckServerSettings();
  }

  private void ReleaseServices()
  {
    ServicesManager.RemoveService(typeof (IClientRedliningService));
    if (this.userAndRole == null)
      return;
    if (this.notifyHandler != null)
    {
      this.notifyService.Unsubscribe("RedliningChanged", this.notifyHandler);
      this.notifyHandler = (NotificationEventHandler) null;
    }
    this.userAndRole = (ICurrentUserAndRole) null;
    this.notifyService = (INotificationService) null;
  }

  private void CheckServerSettings()
  {
    lock (this.syncRoot)
    {
      if (!((ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (IRedliningService)) is IRedliningService customService))
        return;
      long generation = customService.Generation;
      if (this.generation == generation)
        return;
      this.redliningSettings = customService.RedliningFilesSettings;
      this.generation = generation;
    }
  }

  private void NotificationEventFired(object sender, NotificationEventArgs e)
  {
    if (e.EventName != "RedliningChanged")
      return;
    this.Sync();
  }

  public void Sync()
  {
    lock (this.syncRoot)
      this.Sync(this.redlining);
  }

  public void Sync(List<Tuple<long, string>> items)
  {
    if (items == null || items.Count == 0)
      return;
    this.CheckServerSettings();
    Dictionary<long, List<Tuple<string, string>>> dictionary = new Dictionary<long, List<Tuple<string, string>>>();
    for (int index1 = 0; index1 < items.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.redliningSettings.Count; ++index2)
      {
        RedliningFiles redliningSetting = this.redliningSettings[index2];
        FileInfo fileInfo = new FileInfo(items[index1].Item2);
        string mainFilePath = items[index1].Item2;
        List<string> redliningFiles = redliningSetting.FindRedliningFiles(mainFilePath);
        if (redliningFiles.Count != 0)
        {
          List<Tuple<string, string>> collection = new List<Tuple<string, string>>();
          for (int index3 = 0; index3 < redliningFiles.Count; ++index3)
          {
            string relativePath = PathUtils.GetRelativePath(redliningFiles[index3], fileInfo.DirectoryName, RelativePathOptions.None);
            collection.Add(new Tuple<string, string>(redliningFiles[index3], relativePath));
          }
          if (collection.Count > 0)
          {
            if (!dictionary.ContainsKey(items[index1].Item1))
              dictionary.Add(items[index1].Item1, new List<Tuple<string, string>>());
            dictionary[items[index1].Item1].AddRange((IEnumerable<Tuple<string, string>>) collection);
          }
        }
      }
    }
    foreach (KeyValuePair<long, List<Tuple<string, string>>> keyValuePair in dictionary)
    {
      if (keyValuePair.Value.Count != 0)
      {
        PathDictionary<string> pathDictionary = new PathDictionary<string>(keyValuePair.Value.Count);
        List<FileState> localStates = new List<FileState>(keyValuePair.Value.Count);
        string masterDir = string.Empty;
        for (int index = 0; index < keyValuePair.Value.Count; ++index)
        {
          FileInfo fileInfo = new FileInfo(keyValuePair.Value[index].Item1);
          pathDictionary[fileInfo.FullName] = keyValuePair.Value[index].Item2;
          if (string.IsNullOrEmpty(masterDir))
            masterDir = fileInfo.FullName.Length == keyValuePair.Value[index].Item2.Length ? Path.GetDirectoryName(fileInfo.FullName) : Path.GetDirectoryName(fileInfo.FullName.Substring(0, fileInfo.FullName.Length - keyValuePair.Value[index].Item2.Length));
          FileState fileState = new FileState(fileInfo.FullName, fileInfo.LastWriteTimeUtc, fileInfo.Length);
          localStates.Add(fileState);
        }
        List<FileState> fileStates = this.fileVault.DBFilesInfo.GetFileStates(keyValuePair.Key);
        CollectionUtils.Transform<FileState>((IList<FileState>) fileStates, (Converter<FileState, FileState>) (relativeState => new FileState(Path.Combine(masterDir, relativeState.FileName), relativeState.LastWriteTimeUtc, relativeState.Length)));
        List<FileDifferencePair> fileDifferencePairList = new FileDifferenceCalculator().Calculate(localStates, fileStates);
        for (int index = fileDifferencePairList.Count - 1; index >= 0; --index)
        {
          if (fileDifferencePairList[index].DifferenceType != FileDifferenceType.NewFile && fileDifferencePairList[index].DifferenceType != FileDifferenceType.UpdatedFile)
            fileDifferencePairList.RemoveAt(index);
        }
        if (fileDifferencePairList.Count == 0)
          break;
        using (SessionKeeper sessionKeeper = new SessionKeeper())
        {
          IDBAttribute objectAttributeById = sessionKeeper.Session.GetObjectAttributeByID(keyValuePair.Key, sessionKeeper.Session.IdentHelper.FileAttributeID);
          List<string> stringList = objectAttributeById != null ? new List<string>((IEnumerable<string>) objectAttributeById.Descriptions) : new List<string>(0);
          IDBTransactions service = ServiceUtils.GetService<IDBTransactions>((object) sessionKeeper.Session, true);
          service.StartTransaction();
          try
          {
            for (int index = 0; index < fileDifferencePairList.Count; ++index)
            {
              if (fileDifferencePairList[index].DifferenceType == FileDifferenceType.NewFile || fileDifferencePairList[index].DifferenceType == FileDifferenceType.UpdatedFile)
              {
                FileState fileState = fileDifferencePairList[index].LocalState;
                int aIndex = stringList.FindIndex((Predicate<string>) (relativeName => PathUtils.IsSamePath(relativeName, fileState.FileName)));
                DateTime modifyDate = fileState.LastWriteTimeUtc + objectAttributeById.Session.TimeZoneOffset - TimeSpan.FromMilliseconds((double) fileState.LastWriteTimeUtc.Millisecond);
                BlobInformation aBlobInformation = new BlobInformation(fileState.Length, 0L, modifyDate, pathDictionary[fileState.FileName], ArcMethods.ZLibPacked, LocalizationHolder.rm.GetString("Navigator_9"));
                if (aIndex < 0)
                  aIndex = objectAttributeById.AddValue((object) null);
                else
                  objectAttributeById.Index = aIndex;
                using (Stream aSourceStream = (Stream) new FileStream(fileState.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                  new BlobProcWriter(objectAttributeById.DBObjectID, AttributableElements.Object, objectAttributeById.AttributeID, aIndex, 0, aBlobInformation, aSourceStream, (BlobProcCustomClass.ProgressEventHandler) null, (BlobProcCustomClass.ThreadFinishEventHandler) null).WriteData();
              }
            }
            service.Commit();
          }
          catch
          {
            service.Rollback();
            throw;
          }
        }
      }
    }
  }

  public void Clear()
  {
    lock (this.syncRoot)
      this.redlining.Clear();
  }

  public void AddObject(long objectID, string path)
  {
    this.AddObjects((IList<long>) new long[1]{ objectID }, (IList<string>) new string[1]
    {
      path
    });
  }

  public void AddObject(long objectID, IList<string> paths)
  {
    if (paths == null)
      throw new ArgumentNullException(nameof (paths));
    if (objectID == 0L)
      return;
    List<long> objectIDs = new List<long>(paths.Count);
    for (int index = 0; index < paths.Count; ++index)
      objectIDs.Add(objectID);
    this.AddObjects((IList<long>) objectIDs, paths);
  }

  public void AddObjects(IList<long> objectIDs, IList<string> paths)
  {
    if (objectIDs == null)
      throw new ArgumentNullException(nameof (objectIDs));
    if (paths == null)
      throw new ArgumentNullException(nameof (paths));
    if (objectIDs.Count != paths.Count)
      throw new ArgumentException(LocalizationHolder.rm.GetString("Navigator_10"));
    lock (this.syncRoot)
    {
      for (int index = objectIDs.Count - 1; index >= 0; --index)
      {
        if (objectIDs[index] != 0L)
        {
          this.Remove(objectIDs[index], paths[index]);
          this.redlining.Insert(0, new Tuple<long, string>(objectIDs[index], paths[index]));
        }
      }
    }
  }

  public void Remove(long objectID, string path)
  {
    this.Remove((IList<long>) new long[1]{ objectID }, (IList<string>) new string[1]
    {
      path
    });
  }

  public void Remove(IList<long> objectIDs, IList<string> paths)
  {
    if (objectIDs == null)
      throw new ArgumentNullException(nameof (objectIDs));
    if (paths == null)
      throw new ArgumentNullException(nameof (paths));
    if (objectIDs.Count != paths.Count)
      throw new ArgumentException(LocalizationHolder.rm.GetString("Navigator_10"));
    lock (this.syncRoot)
    {
      for (int index1 = this.redlining.Count - 1; index1 >= 0; --index1)
      {
        Tuple<long, string> tuple = this.redlining[index1];
        for (int index2 = 0; index2 < objectIDs.Count; ++index2)
        {
          if (tuple.Item1 == objectIDs[index2] && tuple.Item2 == paths[index2])
          {
            this.redlining.RemoveAt(index1);
            break;
          }
        }
      }
    }
  }

  public void Remove(long objectID)
  {
    this.Remove((IList<long>) new long[1]{ objectID });
  }

  public void Remove(IList<long> objectIDs)
  {
    if (objectIDs == null)
      throw new ArgumentNullException(nameof (objectIDs));
    lock (this.syncRoot)
    {
      for (int index1 = this.redlining.Count - 1; index1 >= 0; --index1)
      {
        Tuple<long, string> tuple = this.redlining[index1];
        for (int index2 = 0; index2 < objectIDs.Count; ++index2)
        {
          if (tuple.Item1 == objectIDs[index2])
          {
            this.redlining.RemoveAt(index1);
            break;
          }
        }
      }
    }
  }

  public void Remove(string path)
  {
    this.Remove((IList<string>) new string[1]{ path });
  }

  public void Remove(IList<string> paths)
  {
    if (paths == null)
      throw new ArgumentNullException(nameof (paths));
    lock (this.syncRoot)
    {
      for (int index1 = this.redlining.Count - 1; index1 >= 0; --index1)
      {
        Tuple<long, string> tuple = this.redlining[index1];
        for (int index2 = 0; index2 < paths.Count; ++index2)
        {
          if (tuple.Item2 == paths[index2])
          {
            this.redlining.RemoveAt(index1);
            break;
          }
        }
      }
    }
  }
}
