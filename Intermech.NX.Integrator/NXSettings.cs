// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXSettings
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Tools.Integrators.CADInterface;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXSettings : CADSettings
{
  private bool enableModelJTFiles;

  public bool EnableModelJTFiles
  {
    get => this.enableModelJTFiles;
    set => this.enableModelJTFiles = value;
  }
}
