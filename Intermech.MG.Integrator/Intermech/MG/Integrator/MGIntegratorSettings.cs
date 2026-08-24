// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGIntegratorSettings
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.Tools.Integrators.Electrical;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class MGIntegratorSettings : ECADIntegratorSettings
{
  public string PartPosDesignationAttribute { get; set; }

  public string MainSchemeId { get; set; }

  public string Sheet { get; set; }

  public List<Tuple<Guid, string>> ElementListTypes { get; set; }

  public GlobalId<int> AssemblyDocumentType { get; set; }

  public string FilterParameterName { get; set; }

  public List<Tuple<StringKey, CompositionVariants>> ComponentsFilter { get; set; }

  public List<Tuple<StringKey, StringKey>> NotImportetBoardSettings { get; set; }

  public string FGPosDesignation { get; set; }
}
