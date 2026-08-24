// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ObjectType4PublicationProrertiesEx
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class ObjectType4PublicationProrertiesEx : ObjectType4PublicationProrerties
{
  public ObjectType4PublicationProrertiesEx(
    bool isPublish,
    bool linkPublishObject,
    PublishTypeAttProxy publishType)
    : base(isPublish, linkPublishObject)
  {
    this.PublishType = publishType;
  }

  [DisplayName("Тип объектов на портале")]
  [Description("Тип объектов на портале который соотвествует текущему типу объетов при публикации")]
  [Editor(typeof (PublishTypeUITypeEditor), typeof (UITypeEditor))]
  public PublishTypeAttProxy PublishType { get; set; }
}
