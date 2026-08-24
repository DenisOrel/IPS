// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.Decorators.ImportedObjectListWithStatisticsDecorator
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Interface.PumpStatistics;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter.Decorators;

internal class ImportedObjectListWithStatisticsDecorator : ImportedObjectListDecorator
{
  public const string StatAddedObjectsCount = "Импортировано объектов";
  public const string StatUsedObjectsCount = "Обновлено объектов";
  private readonly PumpStatisticsService _statisticsService;
  private readonly Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics _statistics;

  private void IncrementAddedObjectCountStatistics()
  {
    this._statistics.IncrementStatisticsInt("Импортировано объектов", 1);
  }

  private void IncrementUsedObjectCountStatistics()
  {
    this._statistics.IncrementStatisticsInt("Обновлено объектов", 1);
  }

  protected override void InternalAfterImportEventDelegate(object sender, EventArgs e)
  {
    base.InternalAfterImportEventDelegate(sender, e);
    this._statisticsService.SaveAsync(this._statistics.PumpGuid)?.GetAwaiter().GetResult();
  }

  public ImportedObjectListWithStatisticsDecorator(
    IImportedObjectList origin,
    Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics statistics)
    : base(origin, statistics.PumpGuid)
  {
    this._statisticsService = ApplicationServices.Container.GetService<PumpStatisticsService>();
    this._statistics = statistics;
  }

  public override ObjectRecord AddObject(int objType, int owner)
  {
    this.IncrementAddedObjectCountStatistics();
    return base.AddObject(objType, owner);
  }

  public override ObjectRecord AddObject(int objType, int owner, string caption)
  {
    this.IncrementAddedObjectCountStatistics();
    return base.AddObject(objType, owner, caption);
  }

  public override ObjectRecord AddObject(ObjectRecord obj)
  {
    this.IncrementAddedObjectCountStatistics();
    return base.AddObject(obj);
  }

  public override ObjectRecord AddObject(
    int objType,
    int owner,
    int lcStep,
    int versionId,
    int userId,
    int objVerType,
    DateTime modifDate,
    int lewelId,
    DateTime createDate,
    string caption)
  {
    this.IncrementAddedObjectCountStatistics();
    return base.AddObject(objType, owner, lcStep, versionId, userId, objVerType, modifDate, lewelId, createDate, caption);
  }

  public override void UseObject(long objectID)
  {
    this.IncrementUsedObjectCountStatistics();
    base.UseObject(objectID);
  }

  public override void UseObject(ObjectRecord obj)
  {
    this.IncrementUsedObjectCountStatistics();
    base.UseObject(obj);
  }

  public override void UseObject(Guid objectGuid, long objectID)
  {
    this.IncrementUsedObjectCountStatistics();
    base.UseObject(objectGuid, objectID);
  }
}
