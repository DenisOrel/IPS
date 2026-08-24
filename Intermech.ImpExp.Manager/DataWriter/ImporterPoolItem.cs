// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.ImporterPoolItem
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.Interfaces;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class ImporterPoolItem
{
  private IUserSession userSession;
  private IDBImporter dbImporter;
  private ImporterPool importerPool;

  public ImporterPoolItem(ImporterPool pool)
  {
    this.importerPool = pool;
    this.userSession = this.importerPool.DefaultSession.Clone(nameof (ImporterPoolItem));
    this.dbImporter = this.userSession.GetImporter($"importer{pool.Length.ToString()}.log");
  }
}
