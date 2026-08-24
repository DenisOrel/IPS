// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.Status
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

public class Status
{
  private Image img;
  private Guid pluginGuid = Guid.Empty;
  private int statusId;
  private byte[] statusKey;
  private string caption;

  public Status(Guid pluginGuid, int statusId, string caption, Image img)
  {
    this.pluginGuid = pluginGuid;
    this.statusId = statusId;
    this.caption = caption;
    this.img = img;
  }

  public Image Img
  {
    get => this.img;
    set => this.img = value;
  }

  public Guid PluginGuid
  {
    get => this.pluginGuid;
    set => this.pluginGuid = value;
  }

  public int StatusId
  {
    get => this.statusId;
    set => this.statusId = value;
  }

  public byte[] StatusKey
  {
    get => this.statusKey;
    set => this.statusKey = value;
  }

  public string Caption
  {
    get => this.caption;
    set => this.caption = value;
  }

  public override bool Equals(object obj)
  {
    return obj is Status status && status.pluginGuid == this.pluginGuid && status.statusId == this.statusId;
  }

  public override string ToString()
  {
    return this.caption != null ? this.caption : this.pluginGuid.ToString() + this.statusId.ToString();
  }

  public override int GetHashCode() => this.ToString().GetHashCode();
}
