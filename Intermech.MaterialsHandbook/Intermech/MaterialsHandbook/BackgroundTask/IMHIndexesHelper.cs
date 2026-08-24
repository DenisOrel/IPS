// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.BackgroundTask.IMHIndexesHelper
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase.Indexes;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook.BackgroundTask;

public class IMHIndexesHelper
{
  private Dictionary<string, List<Guid>> _addedIndexes;
  private Dictionary<string, List<Guid>> _removedIndexes;

  public IndexesStatus Actions { get; set; }

  public Dictionary<string, List<Guid>> AddedIndexes
  {
    get => this._addedIndexes = this._addedIndexes ?? new Dictionary<string, List<Guid>>(0);
  }

  public int ImageIndex { get; set; }

  public bool NeedIndexindMaterials { get; }

  public Dictionary<string, List<Guid>> RemovedIndexes
  {
    get => this._removedIndexes = this._removedIndexes ?? new Dictionary<string, List<Guid>>(0);
  }

  public long SourceID { get; }

  public IMHIndexesHelper(
    long sourceID,
    bool needIndexindMaterials,
    Dictionary<string, List<Guid>> addedIndexes,
    Dictionary<string, List<Guid>> removedIndexes)
  {
    this.SourceID = sourceID;
    this.NeedIndexindMaterials = needIndexindMaterials;
    this._addedIndexes = addedIndexes;
    this._removedIndexes = removedIndexes;
    this.Actions = IndexesStatus.None;
    this.ImageIndex = -1;
    INamedImageList service = ServiceUtils.GetService<INamedImageList>((object) ApplicationServices.Container, false);
    if (service == null)
      return;
    this.ImageIndex = service.ImageIndex("imgIndexes");
  }
}
