// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DropIndexesPumper
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

[TaskDescription("", "Отключение индексов в базе данных назначения")]
internal class DropIndexesPumper : PumpClass
{
  protected override Guid GUID => new Guid("6D0BEB01-F87A-462f-A3D2-3A560996BEC9");

  public DropIndexesPumper()
    : base((PluginClass) null)
  {
  }

  public override void Pump()
  {
    IDBImporter dbImporter = (ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo).dbImporter;
    Guid guid = Guid.NewGuid();
    if (dbImporter != null)
    {
      dbImporter.SetTriggersIMS_OBJECT_ATTRS(false);
      this.PumpCheckPoint("Отключение индексов", 0);
      dbImporter.DropIndexes(guid);
      BriefcaseImportProgress progress;
      while (true)
      {
        Thread.Sleep(100);
        progress = dbImporter.GetProgress(guid);
        if (progress != null)
        {
          if (progress.Operation != OperationType.Finished)
            this.PumpCheckPoint("Отключение индексов", progress.Percent);
          else
            break;
        }
        else
          goto label_8;
      }
      if (progress.ErrorException != null)
        (ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter).AppManager.AddWarningMessage(progress.ErrorException.Message);
      dbImporter.EndImportMetadata(guid);
    }
label_8:
    this.PumpCheckPoint("Отключение индексов завершено", 100);
  }
}
