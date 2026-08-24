// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.ObjectMainAttributesGridTab
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

public class ObjectMainAttributesGridTab : GtcObjectPropertyGridTab
{
  private readonly Guid _tabGuid = new Guid("AABD6C3D-F920-453e-B334-D1180DDDEBA9");
  private readonly string _tabName = ServiceHolder.Rm.GetString("GTC_30");
  private static Bitmap _tabBitmap;
  private static readonly GetAttributeValuesModes tabAttributeValuesModes = GetAttributeValuesModes.IncludeName | GetAttributeValuesModes.IncludeGroupName | GetAttributeValuesModes.CheckWriteAccess | GetAttributeValuesModes.IncludeDescriptions | GetAttributeValuesModes.CheckVisibility | GetAttributeValuesModes.IncludeCaption;

  public override GetAttributeValuesModes TabAttributeValuesModes
  {
    get => ObjectMainAttributesGridTab.tabAttributeValuesModes;
  }

  public override Guid TabGuid => this._tabGuid;

  public override string TabName => this._tabName;

  public override Bitmap Bitmap
  {
    get
    {
      if (ObjectMainAttributesGridTab._tabBitmap == null)
      {
        INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
        if (service != null)
          ObjectMainAttributesGridTab._tabBitmap = new Bitmap(service.ImageList.Images[service.ImageIndex("imgPrintPreview")]);
      }
      return ObjectMainAttributesGridTab._tabBitmap;
    }
  }
}
