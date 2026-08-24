// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.RelationVisualizer.VisToolSelecting
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Map;

#nullable disable
namespace Intermech.Pdm.RelationVisualizer;

internal class VisToolSelecting(MapView v) : MapToolSelecting(v)
{
  public override void DoSelect(MapInputEventArgs evt)
  {
    MapObject mapObject = this.View.PickObject(true, false, evt.DocPoint, true);
    if (mapObject != null && !mapObject.Selectable)
      return;
    base.DoSelect(evt);
  }
}
