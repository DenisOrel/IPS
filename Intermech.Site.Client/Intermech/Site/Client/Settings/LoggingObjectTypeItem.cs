// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.LoggingObjectTypeItem
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Tools.Settings.PropertyEditors;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Site.Client.Settings;

internal sealed class LoggingObjectTypeItem : ICloneable
{
  public LoggingObjectTypeItem() => this.TypeId = new LocalId<int>(-1, "Тип не указан");

  public LoggingObjectTypeItem(int id, string name) => this.TypeId = new LocalId<int>(id, name);

  [DisplayName("Тип объекта")]
  [Editor(typeof (SelectObjectTypeUIEditor), typeof (UITypeEditor))]
  public LocalId<int> TypeId { get; set; }

  [Browsable(false)]
  public string Name => this.TypeId.Name ?? "";

  public LoggingObjectTypeItem Clone()
  {
    return new LoggingObjectTypeItem()
    {
      TypeId = (LocalId<int>) this.TypeId.Clone()
    };
  }

  object ICloneable.Clone() => (object) this.Clone();

  public override int GetHashCode() => this.TypeId.Id.GetHashCode();

  public override bool Equals(object obj)
  {
    return obj is LoggingObjectTypeItem loggingObjectTypeItem ? loggingObjectTypeItem.TypeId.Id == this.TypeId.Id : base.Equals(obj);
  }
}
