// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishTaskCreator
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Client;

#nullable disable
namespace Intermech.Site.Client;

internal class PublishTaskCreator
{
  private IBackgroundTaskView btw = (IBackgroundTaskView) ServicesManager.GetService(typeof (IBackgroundTaskView));
}
