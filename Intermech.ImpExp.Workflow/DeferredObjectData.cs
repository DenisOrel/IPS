// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.DeferredObjectData
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

#nullable disable
namespace Intermech.ImpExp.Workflow;

internal class DeferredObjectData
{
  public readonly string Name;
  public readonly string BlobFileName;
  public readonly long BlobFileSize;
  public long ID = -1;
  public long Index = -1;
  public object Tag;

  public DeferredObjectData(string name, string blobFileName, long blobFileSize)
  {
    this.Name = name;
    this.BlobFileName = blobFileName;
    this.BlobFileSize = blobFileSize;
  }

  public DeferredObjectData(long id) => this.ID = id;
}
