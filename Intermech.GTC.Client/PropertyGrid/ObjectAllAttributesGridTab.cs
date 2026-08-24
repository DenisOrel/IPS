// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.ObjectAllAttributesGridTab
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

public class ObjectAllAttributesGridTab : GtcObjectPropertyGridTab
{
  private readonly Guid _tabGuid = new Guid("7E74807E-46B8-4b26-AEB1-A222F9DED498");
  private readonly string _tabName = ServiceHolder.Rm.GetString("GTC_31");
  private static Bitmap _tabBitmap;
  private static readonly GetAttributeValuesModes tabAttributeValuesModes = GetAttributeValuesModes.IncludeName | GetAttributeValuesModes.IncludeObligatoryAttributes | GetAttributeValuesModes.IncludeGroupName | GetAttributeValuesModes.CheckWriteAccess | GetAttributeValuesModes.IncludeDescriptions | GetAttributeValuesModes.IncludeOnlyInvisible | GetAttributeValuesModes.IncludeCaption;

  public override GetAttributeValuesModes TabAttributeValuesModes
  {
    get => ObjectAllAttributesGridTab.tabAttributeValuesModes;
  }

  public override Guid TabGuid => this._tabGuid;

  public override string TabName => this._tabName;

  public override Bitmap Bitmap
  {
    get
    {
      if (ObjectAllAttributesGridTab._tabBitmap == null)
      {
        INamedImageList service = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
        if (service != null)
          ObjectAllAttributesGridTab._tabBitmap = new Bitmap(service.ImageList.Images[service.ImageIndex("imgPrintPreview")]);
      }
      return ObjectAllAttributesGridTab._tabBitmap;
    }
  }
}
