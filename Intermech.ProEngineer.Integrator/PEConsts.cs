// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.PEConsts
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using System;

#nullable disable
namespace Intermech.ProEngineer.Integrator;

internal static class PEConsts
{
  internal static readonly Guid objtypePEAssembly = new Guid("cad0076a-306c-11d8-b4e9-00304f19f545");
  internal static readonly Guid objtypePEPart = new Guid("cad00791-306c-11d8-b4e9-00304f19f545");
  internal static readonly Guid objtypePEAssmDrawing = new Guid("cad00903-306c-11d8-b4e9-00304f19f545");
  internal static readonly Guid objtypePEPartDrawing = new Guid("cad0090a-306c-11d8-b4e9-00304f19f545");
  internal static readonly Guid PEIntegratorId = new Guid("B178F1E8-B890-4AC4-AA1B-9CB068B61FCB");
  internal static readonly string AppName = "Pro/ENGINEER (Creo Parametric)";
  internal static readonly string IntegratorName = Localization.rma.GetString("ProEngineer_IntegratorName");
  internal static readonly Guid CLSID = new Guid("CE06F8E5-46AE-47D4-9C07-4EB144DD3C14");
  internal static readonly string ProgID = "ProECADSystem.1";
  internal static readonly string StandardLibrary = "PE Library";
  internal static readonly string AssemblyFileExtension = ".asm";
  internal static readonly string ManufacturingFileExtension = ".mfg";
  internal static readonly string PartFileExtension = ".prt";
  internal static readonly string DrawingFileExtension = ".drw";
  internal static readonly string LayoutFileExtension = ".lay";
  internal static readonly string SectionFileExtension = ".sec";
}
