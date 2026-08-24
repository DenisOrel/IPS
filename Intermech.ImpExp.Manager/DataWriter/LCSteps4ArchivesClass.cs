// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.LCSteps4ArchivesClass
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class LCSteps4ArchivesClass
{
  private Dictionary<int, LCSchemaInfo> _info;
  private IImportingData _cacheData;

  public void Load()
  {
    this._cacheData = (ServicesManager.GetService(typeof (ICache)) as ICache).GetCache(ImportingCategory.LCSteps4Archives, ImportingCategory.StatusesToLevels);
    this._info = (ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo).dbImporter.GetSchemaInfo4ObjTypes();
  }

  public int GetLCStep(int archiveID, int objectType, int docStateID)
  {
    LCSchemaInfo lcSchemaInfo = this._info[objectType];
    if (docStateID != 0)
    {
      long newKey = this._cacheData.GetNewKey(ImportingCategory.StatusesToLevels, (object) docStateID);
      if (newKey != 0L)
      {
        int step = lcSchemaInfo.GetStep(Convert.ToInt32(newKey));
        if (step != -1)
          return step;
      }
    }
    ITagImportObject tag = this._cacheData.GetTag(ImportingCategory.LCSteps4Archives, (object) archiveID);
    int num;
    return tag != null && (tag as LCSteps4Archives).LCSteps4 != null && (tag as LCSteps4Archives).LCSteps4.TryGetValue(lcSchemaInfo.SchemaID, out num) ? num : lcSchemaInfo.FirtsLCStep;
  }
}
