// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.SWConsts
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using System;

#nullable disable
namespace Intermech.SolidWorks.Integrator;

internal static class SWConsts
{
  internal static readonly Guid objtypeSWAssembly = new Guid("cad0076c-306c-11d8-b4e9-00304f19f545");
  internal static readonly Guid objtypeSWPart = new Guid("cad00793-306c-11d8-b4e9-00304f19f545");
  internal static readonly Guid objtypeSWAssmDrawing = new Guid("cad00905-306c-11d8-b4e9-00304f19f545");
  internal static readonly Guid objtypeSWPartDrawing = new Guid("cad0090c-306c-11d8-b4e9-00304f19f545");
  internal static readonly Guid SWIntegratorId = new Guid("FDBE0FD7-D10B-41f6-99CC-9841FF2D52F8");
  internal static readonly string IntegratorAppName = "SolidWorks";
  internal static readonly string DisplayIntegratorName = Localization.rma.GetString("SolidWorks_IntegratorName");
  internal static readonly string ProgID = "SWCAD";
  internal static readonly string StandardLibrary = "SW Library";
  internal static readonly string AssemblyFileExtension = ".sldasm";
  internal static readonly string PartFileExtension = ".sldprt";
  internal static readonly string DrawingFileExtension = ".slddrw";
}
