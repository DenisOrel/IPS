// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ImbaseFolderPropertyClass
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.PropertyEditors;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Site.Client;

[Editor(typeof (ImbaseFolderEditor), typeof (UITypeEditor))]
public class ImbaseFolderPropertyClass : ObjectPropertyClass
{
  public ImbaseFolderPropertyClass(long aObjectID)
    : base(aObjectID)
  {
  }

  public ImbaseFolderPropertyClass(long aObjectID, string aCaption)
    : base(aObjectID, aCaption)
  {
  }
}
