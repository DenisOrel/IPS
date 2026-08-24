// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.DrawCreator.DrawCreatorResult
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

#nullable disable
namespace Intermech.NX.Integrator.DrawCreator;

internal class DrawCreatorResult
{
  public DrawCreatorResult()
  {
    this.ModelID = 0L;
    this.DrawingToModelRelationID = 0L;
  }

  public long ModelID { get; set; }

  public long DrawingToModelRelationID { get; set; }
}
