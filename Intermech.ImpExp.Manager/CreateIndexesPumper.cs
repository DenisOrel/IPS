// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CreateIndexesPumper
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Briefcase;
using Intermech.Interfaces.Client;
using System;
using System.Threading;

#nullable disable
namespace Intermech.ImpExp.Manager;

[TaskDescription("", "Включение индексов в базе данных назначения")]
internal class CreateIndexesPumper : PumpClass
{
  protected override Guid GUID => new Guid("FD3F9B70-F9C0-4bd9-854C-902AA83E53F6");

  public CreateIndexesPumper()
    : base((PluginClass) null)
  {
  }

  public override void Pump()
  {
    IDBImporter dbImporter = (ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo).dbImporter;
    Guid guid = Guid.NewGuid();
    if (dbImporter != null)
    {
      dbImporter.SetTriggersIMS_OBJECT_ATTRS(true);
      this.PumpCheckPoint("Включение индексов", 0);
      dbImporter.CreateIndexes(guid);
      BriefcaseImportProgress progress;
      do
      {
        Thread.Sleep(100);
        progress = dbImporter.GetProgress(guid);
        if (progress != null)
          this.PumpCheckPoint("Включение индексов", progress.Percent);
        else
          goto label_7;
      }
      while (progress.Operation != OperationType.Finished);
      if (progress.ErrorException != null)
        (ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter).AppManager.AddWarningMessage(progress.ErrorException.Message);
      dbImporter.EndImportMetadata(guid);
    }
label_7:
    this.PumpCheckPoint("Включение индексов завершено", 100);
  }
}
