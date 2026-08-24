// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.IDAttributesSyncronizer.DocumentChangeHandler
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.IDAttributesSyncronizer;

internal sealed class DocumentChangeHandler(
  DBObjectsExtendedEventArgs eventArgs,
  IUserSession session) : ObjectChangeHandler(eventArgs, session)
{
  protected override void OnGetDesignationValue(IDAttributeInfo attrDesignation, int objectTypeID)
  {
    DocumentTypeSettings settings = (this.session.GetCustomService(typeof (IDocumentTypeSettingsService)) as IDocumentTypeSettingsService).GetSettings(this.session.SessionGUID, objectTypeID);
    if (!settings.DocumentTypeCodeInDesignation)
      return;
    attrDesignation.NewValue = PDMHelper.GetDesignationWithoutCode(this.session, attrDesignation.NewValue, settings);
  }

  protected override Dictionary<int, List<int>> GetApplicabilities(int objectTypeID)
  {
    IDBRelationsApplicabilityCollection applicabilityCollection = this.session.GetRelationsApplicabilityCollection();
    Dictionary<int, List<int>> applics = new Dictionary<int, List<int>>();
    int objectType = objectTypeID;
    DataTable applicabilitiesList = applicabilityCollection.GetApplicabilitiesList(-1, objectType, -1);
    for (int index = 0; index < applicabilitiesList.Rows.Count; ++index)
    {
      if ((Convert.ToInt32(applicabilitiesList.Rows[index]["F_OPTIONS"]) & 8) == 8)
        this.SetEnabledApplicabilities(applics, Convert.ToInt32(applicabilitiesList.Rows[index]["F_INOBJECT_TYPE"]), Convert.ToInt32(applicabilitiesList.Rows[index]["F_RELATION_TYPE"]));
    }
    for (int index = 0; index < applicabilitiesList.Rows.Count; ++index)
    {
      if ((Convert.ToInt32(applicabilitiesList.Rows[index]["F_OPTIONS"]) & 8) == 0)
        this.RemoveDisabledApplicabilities(applics, Convert.ToInt32(applicabilitiesList.Rows[index]["F_INOBJECT_TYPE"]), Convert.ToInt32(applicabilitiesList.Rows[index]["F_RELATION_TYPE"]));
    }
    return applics;
  }

  protected override void OnHandle(
    IDBObject changedObject,
    DBRecordSetParams dbParams,
    IDBRelationCollection rellCollection,
    IDAttributeInfo attrDesignation,
    IDAttributeInfo attrName)
  {
    DataTable dataTable = rellCollection.EntersInVersion(dbParams, changedObject.ObjectID);
    if (dataTable.Rows.Count <= 0)
      return;
    string oldValue = string.Empty;
    if (attrDesignation != null && attrDesignation.OrigValue != null && attrDesignation.OrigValue.Values != null && attrDesignation.OrigValue.Values.Length == 1)
    {
      DocumentTypeSettings settings = this.docSettingsService.GetSettings(this.session.SessionGUID, this.eventArgs.ObjectType);
      oldValue = settings.DocumentTypeCodeInDesignation ? PDMHelper.GetDesignationWithoutCode(this.session, Convert.ToString(attrDesignation.OrigValue.Values[0]), settings) : Convert.ToString(attrDesignation.OrigValue.Values[0]);
    }
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      IList<long> longList = this.objectsCheckOutService.CheckOut(this.session, (IList<long>) new long[1]
      {
        Convert.ToInt64(dataTable.Rows[index][0])
      }, true);
      lock (this)
      {
        SyncronizerService.CurrentChangesArticle = longList[0];
        try
        {
          IDBObject dbObject = this.session.GetObject(longList[0]);
          if (attrDesignation != null && attrDesignation.Changed)
          {
            string initValue = Convert.ToString(dbObject.GetValuesByID(SyncronizerService.AttrDesignationID, true)[0]);
            AttributeValues newAttrValues = !(oldValue != string.Empty) ? new AttributeValues(SyncronizerService.AttrDesignationID, (object) attrDesignation.NewValue) : new AttributeValues(SyncronizerService.AttrDesignationID, (object) initValue.Replace(oldValue, attrDesignation.NewValue));
            dbObject.SetAttributesValues(new AttributeValues[1]
            {
              newAttrValues
            });
            if (this.notificationService != null)
              this.notificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsExtendedEventArgs(dbObject.ObjectID, this.eventArgs.ObjectType, new AttributeValues(SyncronizerService.AttrDesignationID, (object) initValue), newAttrValues));
          }
          if (attrName != null)
          {
            if (attrName.Changed)
            {
              AttributeValues newAttrValues = new AttributeValues(SyncronizerService.AttrNameID, (object) attrName.NewValue);
              dbObject.SetAttributesValues(new AttributeValues[1]
              {
                newAttrValues
              });
              if (this.notificationService != null)
                this.notificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsExtendedEventArgs(dbObject.ObjectID, this.eventArgs.ObjectType, new AttributeValues(SyncronizerService.AttrNameID, (object) attrName.OrigValue), newAttrValues));
            }
          }
        }
        finally
        {
          SyncronizerService.CurrentChangesArticle = 0L;
        }
      }
    }
  }
}
