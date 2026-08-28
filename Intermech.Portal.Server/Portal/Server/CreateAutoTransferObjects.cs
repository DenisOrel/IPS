// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.CreateAutoTransferObjects
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace Intermech.Portal.Server;

internal sealed class CreateAutoTransferObjects : CreateAutoTransferBase
{
  private List<TransferedObject> _trObjects;

  public CreateAutoTransferObjects(
    IUserSession session,
    SiteInfo info,
    PackAnalyzInfo packAnalyzInfo,
    List<TransferedObject> trObjects)
    : base(session, info, packAnalyzInfo)
  {
    this._trObjects = trObjects;
  }

  public override void OnCreate()
  {
    if (TraceLog.Enabled)
      TraceLog.Write("Start create update packet (composition)");
    List<char> codes = new List<char>();
    List<List<string>> stringListList = new List<List<string>>();
    foreach (TransferedObject trObject in this._trObjects)
    {
      if (trObject.Category == TransferedObjectCategory.GroupObject || trObject.Category == TransferedObjectCategory.Object || trObject.Category == TransferedObjectCategory.ObjectLink)
      {
        foreach (char ch in !(trObject.Tag is ObjectTag) || string.IsNullOrEmpty(((ObjectTag) trObject.Tag).EnableSites) ? this.packAnalyzInfo.SiteForUpdate : ((ObjectTag) trObject.Tag).EnableSites)
        {
          char site = ch;
          int index = codes.FindIndex((Predicate<char>) (x => x.Equals(site)));
          List<string> stringList;
          if (index < 0)
          {
            codes.Add(site);
            stringList = new List<string>();
            stringListList.Add(stringList);
          }
          else
            stringList = stringListList[index];
          stringList.Add(trObject.GUID);
        }
      }
    }
    List<StringBuilder> stringBuilderList = new List<StringBuilder>();
    for (int i = 0; i < codes.Count; i++)
    {
      if (stringBuilderList.Find((Predicate<StringBuilder>) (x => x.ToString().Contains<char>(codes[i]))) == null)
      {
        StringBuilder stringBuilder = new StringBuilder(codes[i].ToString());
        stringBuilderList.Add(stringBuilder);
        for (int index = 0; index < codes.Count; ++index)
        {
          if (index > i && stringListList[i].SequenceEqual<string>((IEnumerable<string>) stringListList[index], (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase))
            stringBuilder.Append(codes[index]);
        }
      }
    }
    if (stringBuilderList.Count == 1)
    {
      foreach (TransferedObject trObject in this._trObjects)
        this.SetPathForDataFiles(trObject, trObject.GUID);
      this.CreateSiteUpdate(this._trObjects, this.packAnalyzInfo.SiteForUpdate);
    }
    else
    {
      for (int index1 = 0; index1 < stringBuilderList.Count; ++index1)
      {
        List<string> stringList = stringListList[index1];
        List<TransferedObject> transferedObjectList = new List<TransferedObject>();
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        for (int index2 = 0; index2 < this._trObjects.Count; ++index2)
        {
          TransferedObject trObject = this._trObjects[index2];
          if (trObject.Category == TransferedObjectCategory.GroupObject)
            trObject.Category = TransferedObjectCategory.Object;
          else if (trObject.Category == TransferedObjectCategory.GroupRelation)
            trObject.Category = TransferedObjectCategory.Relation;
          if (trObject.Category == TransferedObjectCategory.Object || trObject.Category == TransferedObjectCategory.ObjectLink)
          {
            if (stringList.IndexOf(trObject.GUID) >= 0)
            {
              if (index1 == 0)
              {
                this.AddTransferedObjectAndCorrectDataFiles(transferedObjectList, trObject);
              }
              else
              {
                string str = Guid.NewGuid().ToString();
                dictionary.Add(trObject.GUID, str);
                TransferedObject transferedObject = trObject.Clone();
                transferedObject.GUID = str;
                this.AddTransferedObjectAndCorrectDataFiles(transferedObjectList, transferedObject, trObject.GUID);
              }
            }
          }
          else if (trObject.Category == TransferedObjectCategory.Relation)
          {
            RelationTag tag = trObject.Tag as RelationTag;
            if (stringList.IndexOf(tag.ProjectTransferedObjectGuid) >= 0 && stringList.IndexOf(tag.PartTransferedObjectGuid) >= 0)
            {
              if (index1 == 0)
              {
                this.AddTransferedObjectAndCorrectDataFiles(transferedObjectList, trObject);
              }
              else
              {
                TransferedObject transferedObject = trObject.Clone();
                transferedObject.GUID = Guid.NewGuid().ToString();
                transferedObject.Tag = (TransferedObjectTag) new RelationTag(dictionary[tag.ProjectTransferedObjectGuid], dictionary[tag.PartTransferedObjectGuid]);
                this.AddTransferedObjectAndCorrectDataFiles(transferedObjectList, transferedObject, trObject.GUID);
              }
            }
          }
          else if (index1 == 0)
          {
            this.AddTransferedObjectAndCorrectDataFiles(transferedObjectList, trObject);
          }
          else
          {
            TransferedObject transferedObject = trObject.Clone();
            transferedObject.GUID = Guid.NewGuid().ToString();
            this.AddTransferedObjectAndCorrectDataFiles(transferedObjectList, transferedObject, trObject.GUID);
          }
        }
        if (transferedObjectList.Count > 0)
          this.CreateSiteUpdate(transferedObjectList, stringBuilderList[index1].ToString());
      }
    }
  }

  private void AddTransferedObjectAndCorrectDataFiles(
    List<TransferedObject> collection,
    TransferedObject transferedObject)
  {
    this.AddTransferedObjectAndCorrectDataFiles(collection, transferedObject, transferedObject.GUID);
  }

  private void AddTransferedObjectAndCorrectDataFiles(
    List<TransferedObject> collection,
    TransferedObject transferedObject,
    string publishGuid)
  {
    this.SetPathForDataFiles(transferedObject, publishGuid);
    collection.Add(transferedObject);
  }

  private void SetPathForDataFiles(TransferedObject transferedObject, string publishGuid)
  {
    if (transferedObject.DataFiles == null)
      return;
    string publishUnitPath = TempStorage.GetPublishUnitPath(publishGuid);
    for (int index = 0; index < transferedObject.DataFiles.Length; ++index)
      transferedObject.DataFiles[index] = $"{Path.Combine(publishUnitPath, transferedObject.DataFiles[index])};{transferedObject.DataFiles[index]}";
  }

  private void CreateSiteUpdate(List<TransferedObject> trObjects, string enableSites)
  {
    if (TraceLog.Enabled)
      TraceLog.Write("Start CreateSiteUpdate for " + enableSites);
    for (int index1 = 0; index1 < trObjects.Count; ++index1)
    {
      TransferedObject trObject = trObjects[index1];
      if (trObject.Category == TransferedObjectCategory.GroupObject)
        trObject.Category = TransferedObjectCategory.Object;
      else if (trObject.Category == TransferedObjectCategory.GroupRelation)
        trObject.Category = TransferedObjectCategory.Relation;
      string updateUnitPath = TempStorage.GetUpdateUnitPath(trObjects[index1].GUID);
      Directory.CreateDirectory(updateUnitPath);
      for (int index2 = 0; index2 < trObjects[index1].DataFiles.Length; ++index2)
      {
        string[] strArray = trObjects[index1].DataFiles[index2].Split(';');
        string str = strArray[1];
        TempStorage.CheckAndCreateLocDirectory(updateUnitPath, str);
        string destFileName = Path.Combine(updateUnitPath, str);
        File.Copy(strArray[0], destFileName, true);
        trObjects[index1].DataFiles[index2] = str;
        if (TraceLog.Enabled)
          TraceLog.Write($"...save file {destFileName} for {trObjects[index1].Category}");
      }
    }
    new SiteUpdate(trObjects, this.GetSiteIDs(enableSites), this.info.Code.ToString()).SaveIntoBase(this.session, Guid.NewGuid());
    if (!TraceLog.Enabled)
      return;
    TraceLog.Write("End CreateSiteUpdate");
  }
}
