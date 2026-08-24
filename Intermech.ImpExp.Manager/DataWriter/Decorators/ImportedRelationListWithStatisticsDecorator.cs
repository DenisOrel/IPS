// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.Decorators.ImportedRelationListWithStatisticsDecorator
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.Interface.PumpStatistics;
using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter.Decorators;

internal class ImportedRelationListWithStatisticsDecorator : ImportedRelationListDecorator
{
  public const string StatAddedRelationsCount = "Импортировано связей";
  public const string StatUsedRelationsCount = "Обновлено связей";
  private readonly PumpStatisticsService _statisticsService;
  private readonly Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics _statistics;

  private void IncrementAddedRelationsCountStatistics()
  {
    this._statistics.IncrementStatisticsInt("Импортировано связей", 1);
  }

  private void IncrementUsedRelationsCountStatistics()
  {
    this._statistics.IncrementStatisticsInt("Обновлено связей", 1);
  }

  protected override void InternalAfterImportEventDelegate(object sender, EventArgs e)
  {
    base.InternalAfterImportEventDelegate(sender, e);
    this._statisticsService.SaveAsync(this._statistics.PumpGuid)?.Wait();
  }

  public ImportedRelationListWithStatisticsDecorator(
    IImportedRelationList origin,
    Intermech.ImpExp.Interface.PumpStatistics.PumpStatistics statistics)
    : base(origin, statistics.PumpGuid)
  {
    this._statisticsService = ApplicationServices.Container.GetService<PumpStatisticsService>();
    this._statistics = statistics;
  }

  public override RelationRecord AddRelation(long projId, long partId, int relType)
  {
    this.IncrementAddedRelationsCountStatistics();
    return base.AddRelation(projId, partId, relType);
  }

  public override RelationRecord AddRelation(RelationRecord rel)
  {
    this.IncrementAddedRelationsCountStatistics();
    return base.AddRelation(rel);
  }

  public override RelationRecord AddRelation(
    long projId,
    long partId,
    int relType,
    DateTime crtDate)
  {
    this.IncrementAddedRelationsCountStatistics();
    return base.AddRelation(projId, partId, relType, crtDate);
  }

  public override RelationRecord AddRelationFromID(long projId, long partId, int relType)
  {
    this.IncrementAddedRelationsCountStatistics();
    return base.AddRelationFromID(projId, partId, relType);
  }

  public override RelationRecord AddRelationFromID(
    long projId,
    long partId,
    int relType,
    DateTime crtDate)
  {
    this.IncrementAddedRelationsCountStatistics();
    return base.AddRelationFromID(projId, partId, relType, crtDate);
  }

  public override void UseRelation(long prjLinkID)
  {
    this.IncrementUsedRelationsCountStatistics();
    base.UseRelation(prjLinkID);
  }

  public override void UseRelation(RelationRecord rel)
  {
    this.IncrementUsedRelationsCountStatistics();
    base.UseRelation(rel);
  }
}
