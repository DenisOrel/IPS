// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PublishCompositionQuery
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Queries;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class PublishCompositionQuery : ObjectsQuery
{
  private readonly List<PublishCompositionObject> _publishObjects;

  public PublishCompositionQuery(
    INodeQuerySupport support,
    int objectTypeID,
    ConditionStructure[] conditions,
    IServiceProvider services,
    List<PublishCompositionObject> publishObjects)
    : base(support, objectTypeID, conditions, services)
  {
    this._publishObjects = publishObjects;
  }

  protected override DataTable GetDataTable(DBRecordSetParams queryParams)
  {
    DataTable dataTable = base.GetDataTable(queryParams);
    int fieldIndex1 = -1;
    this.mapping.CheckFieldIndex(ref fieldIndex1, false, new NodeColumnID((object) MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeReasonInfo), AttributeSourceTypes.Object));
    int fieldIndex2 = -1;
    this.mapping.CheckFieldIndex(ref fieldIndex2, false, new NodeColumnID((object) MetaDataHelper.GetAttributeTypeID(PortalConsts.attributeEnabledSites), AttributeSourceTypes.Object));
    ISitesCacheService customService = (ApplicationServices.Container.GetService(typeof (IMServerService)) as IMServerService).GetCustomService(typeof (ISitesCacheService)) as ISitesCacheService;
    int objectIDIndex = -1;
    this.mapping.CheckFieldIndex(ref objectIDIndex, new NodeColumnID((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object));
    if (objectIDIndex != -1 && this._publishObjects != null)
    {
      Dictionary<string, string> dictionary = (Dictionary<string, string>) null;
      foreach (DataRow row1 in (InternalDataCollectionBase) dataTable.Rows)
      {
        DataRow row = row1;
        PublishCompositionObject compositionObject = this._publishObjects.Find((Predicate<PublishCompositionObject>) (x => x.ObjectID == Convert.ToInt64(row[objectIDIndex])));
        if (compositionObject != null)
        {
          if (fieldIndex1 != -1 && compositionObject.ReasonInfo != string.Empty)
            row[fieldIndex1] = (object) compositionObject.ReasonInfo;
          if (fieldIndex2 != -1)
          {
            if (dataTable.Rows.Count > 1)
            {
              if (dictionary == null)
                dictionary = new Dictionary<string, string>();
              string caption;
              if (!dictionary.TryGetValue(compositionObject.EnableSites, out caption))
              {
                caption = SiteIDHelper.GetCaption(customService, compositionObject.EnableSites);
                dictionary.Add(compositionObject.EnableSites, caption);
              }
              row[fieldIndex2] = (object) caption;
            }
            else
              row[fieldIndex2] = (object) SiteIDHelper.GetCaption(customService, compositionObject.EnableSites);
          }
        }
      }
      dataTable.AcceptChanges();
    }
    return dataTable;
  }
}
