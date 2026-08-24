// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ImportedRelations
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.Interfaces.Client;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class ImportedRelations : IImportedRelations
{
  private IImportingData _cacheData;

  public ImportedRelations()
  {
    this._cacheData = (ServicesManager.GetService(typeof (ICache)) as ICache).GetCache(ImportingCategory.ImportedRelations);
  }

  public void Close()
  {
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      return;
    ImportingCategory[] importingCategoryArray = new ImportingCategory[1]
    {
      ImportingCategory.ImportedRelations
    };
    service.ReleaseCache(importingCategoryArray);
  }

  public void AddValue(long prjlinkID, int relationTypeID)
  {
    this._cacheData.AddValue(ImportingCategory.ImportedRelations, (object) prjlinkID, (long) relationTypeID);
  }

  public int GetRelationTypeID(long prjlinkID)
  {
    return (int) this._cacheData.GetNewKey(ImportingCategory.ImportedRelations, (object) prjlinkID);
  }

  public System.Collections.Generic.Dictionary<object, DictionaryValue> Dictionary
  {
    get => this._cacheData.GetCategory(ImportingCategory.ImportedRelations);
  }
}
