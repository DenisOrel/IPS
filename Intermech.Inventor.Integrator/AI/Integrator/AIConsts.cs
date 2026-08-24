// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.AIConsts
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using System;

#nullable disable
namespace Intermech.AI.Integrator;

internal static class AIConsts
{
  internal static readonly string IntegratorName = Intermech.Localization.Localization.rm.GetString("Inventor.Integrator_58");
  internal static readonly string ApplicationName = "Autodesk Inventor";
  internal static readonly Guid IntegratorId = new Guid("A6C782D1-DDF3-4d85-9F5F-A3F5148127B4");
  internal static readonly string AssemblyFileExtension = ".iam";
  internal static readonly string PartFileExtension = ".ipt";
  internal static readonly string DrawingFileExtension = ".idw";
  internal static readonly string PresentationFileExtension = ".ipn";
}
