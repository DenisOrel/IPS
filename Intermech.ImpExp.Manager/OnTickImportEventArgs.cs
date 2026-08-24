// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.OnTickImportEventArgs
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

#nullable disable
namespace Intermech.ImpExp.Manager;

internal class OnTickImportEventArgs
{
  public long Ticks;

  public OnTickImportEventArgs(long ticks) => this.Ticks = ticks;
}
