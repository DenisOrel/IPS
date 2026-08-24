// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ObjectType4PublicationProrerties
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.Document;
using System.ComponentModel;

#nullable disable
namespace Intermech.Site.Client;

internal class ObjectType4PublicationProrerties
{
  public ObjectType4PublicationProrerties(bool isPublish, bool objectWithLink)
  {
    this.IsPublish = isPublish;
    this.ObjectWithLink = objectWithLink;
  }

  [DisplayName("Публикуемый тип")]
  [Description("Объекты текущего типа могут публиковаться на портал")]
  [TypeConverter(typeof (CustomBooleanConverter))]
  public bool IsPublish { get; set; }

  [DisplayName("Объект вместо ссылки")]
  [Description("При включении этой опции, если публикуется ссылка на объект этого типа, то будет публиковаться и этот объект.")]
  [TypeConverter(typeof (CustomBooleanConverter))]
  public bool ObjectWithLink { get; set; }
}
