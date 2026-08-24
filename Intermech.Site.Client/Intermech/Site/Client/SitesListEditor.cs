// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.SitesListEditor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Client.Core;
using Intermech.Holders;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.PropertyEditors;
using System.Collections;

#nullable disable
namespace Intermech.Site.Client;

public class SitesListEditor : SomethingListEditor<SitesListPropertyClass>
{
  public SitesListEditor() => this.getObjList = new EventsHolder.GetListDelegate(this.GetSitesList);

  private ArrayList GetSitesList(object s, object[] args)
  {
    ArrayList sitesList = new ArrayList();
    ISitesCacheService customService = (ISitesCacheService) (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (ISitesCacheService));
    foreach (SiteInfo site in customService.Sites)
    {
      if (!customService.Info.ID.Equals(site.ID))
        sitesList.Add((object) new object[3]
        {
          (object) site.ID,
          (object) site.Caption,
          (object) CoreConsts.NegativeIdDefaultFCaption
        });
    }
    return sitesList;
  }
}
