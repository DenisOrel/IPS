// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.IExtendedBackgroundTask
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Interfaces.Client;

#nullable disable
namespace Intermech.GTC.Client;

public interface IExtendedBackgroundTask : IBackgroundTask
{
  new string Name { get; set; }

  bool IsProcessStoped { get; }

  void IncProgress();
}
