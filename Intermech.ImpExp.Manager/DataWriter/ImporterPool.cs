// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ImporterPool
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class ImporterPool
{
  private List<ImporterPoolItem> Items = new List<ImporterPoolItem>();

  public IUserSession DefaultSession { get; }

  public int Length => this.Items.Count;

  public ImporterPool(IMServer imServer, int poolSize)
  {
    this.DefaultSession = imServer.CreateSession();
    for (int index = 0; index < poolSize; ++index)
      this.Items.Add(new ImporterPoolItem(this));
  }
}
