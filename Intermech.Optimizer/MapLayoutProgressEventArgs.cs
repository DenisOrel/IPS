// Decompiled with JetBrains decompiler
// Type: Intermech.Map.Layout.MapLayoutProgressEventArgs
// Assembly: Intermech.Optimizer, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F2F8A027-9638-497B-A691-BEF61B30B332
// Assembly location: D:\IPS\Client\Intermech.Optimizer.dll
// XML documentation location: D:\IPS\Client\Intermech.Optimizer.xml

using System;

#nullable disable
namespace Intermech.Map.Layout;

[Serializable]
public class MapLayoutProgressEventArgs : EventArgs
{
  private string myMessage;
  private float myProgress;

  public MapLayoutProgressEventArgs(float done, string msg)
  {
    this.myProgress = done;
    this.myMessage = msg;
  }

  public string Message
  {
    get => this.myMessage;
    set => this.myMessage = value;
  }

  public float Progress
  {
    get => this.myProgress;
    set => this.myProgress = value;
  }
}
