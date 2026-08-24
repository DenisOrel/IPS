// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ExPCBElementList
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.Tools.Integrators.Electrical;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ExPCBElementList : ElementList
{
  protected override int projectTypeID
  {
    get => MetaDataHelper.GetObjectTypeID(MGConsts.ObjTypeExPCBProject);
  }

  protected override long GetProject(IUserSession session, long schemaID) => schemaID;
}
