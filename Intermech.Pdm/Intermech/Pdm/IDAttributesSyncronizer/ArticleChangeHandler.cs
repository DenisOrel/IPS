// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.IDAttributesSyncronizer.ArticleChangeHandler
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.Pdm.IDAttributesSyncronizer;

internal sealed class ArticleChangeHandler(
  DBObjectsExtendedEventArgs eventArgs,
  IUserSession session) : ObjectChangeHandler(eventArgs, session)
{
  protected override Dictionary<int, List<int>> GetApplicabilities(int objectTypeID)
  {
    IDBRelationsApplicabilityCollection applicabilityCollection = this.session.GetRelationsApplicabilityCollection();
    Dictionary<int, List<int>> applics = new Dictionary<int, List<int>>();
    int inObjectType = objectTypeID;
    DataTable applicabilitiesList = applicabilityCollection.GetApplicabilitiesList(-1, -1, inObjectType);
    for (int index = 0; index < applicabilitiesList.Rows.Count; ++index)
    {
      if ((Convert.ToInt32(applicabilitiesList.Rows[index]["F_OPTIONS"]) & 8) == 8)
        this.SetEnabledApplicabilities(applics, Convert.ToInt32(applicabilitiesList.Rows[index]["F_OBJECT_TYPE"]), Convert.ToInt32(applicabilitiesList.Rows[index]["F_RELATION_TYPE"]));
    }
    for (int index = 0; index < applicabilitiesList.Rows.Count; ++index)
    {
      if ((Convert.ToInt32(applicabilitiesList.Rows[index]["F_OPTIONS"]) & 8) == 0)
        this.RemoveDisabledApplicabilities(applics, Convert.ToInt32(applicabilitiesList.Rows[index]["F_OBJECT_TYPE"]), Convert.ToInt32(applicabilitiesList.Rows[index]["F_RELATION_TYPE"]));
    }
    return applics;
  }

  private bool CheckDesignation(
    IDBObject changedObject,
    IDBObject document,
    DocumentTypeSettings docTypeSettings,
    IDAttributeInfo attrDesignation)
  {
    string str = Convert.ToString(document.GetAttributeByID(SyncronizerService.AttrDesignationID).Values[0]);
    string empty = string.Empty;
    string designation;
    if (attrDesignation != null)
    {
      designation = Convert.ToString(attrDesignation.OrigValue.Values[0]);
    }
    else
    {
      IDBAttribute attributeById = changedObject.GetAttributeByID(SyncronizerService.AttrDesignationID);
      designation = attributeById != null ? attributeById.AsString : string.Empty;
    }
    return (!docTypeSettings.DocumentTypeCodeInDesignation || !(docTypeSettings.DocumentTypeCode != string.Empty) ? designation : DocumentsHelper.AppendDocCode(this.session, designation, docTypeSettings.DocumentTypeCode)) == str;
  }

  protected override void OnHandle(
    IDBObject changedObject,
    DBRecordSetParams dbParams,
    IDBRelationCollection rellCollection,
    IDAttributeInfo attrDesignation,
    IDAttributeInfo attrName)
  {
    IDBObjectCollection objectCollection = this.session.GetObjectCollection(changedObject.ObjectType);
    DataTable dataTable1 = rellCollection.ConsistFrom(dbParams, changedObject.ObjectID);
    for (int index1 = 0; index1 < dataTable1.Rows.Count; ++index1)
    {
      IList<long> longList = this.objectsCheckOutService.CheckOut(this.session, (IList<long>) new long[1]
      {
        Convert.ToInt64(dataTable1.Rows[index1][0])
      }, true);
      if (SyncronizerService.CurrentChangesArticle == 0L || longList[0] != SyncronizerService.CurrentChangesDocument)
      {
        lock (this)
        {
          SyncronizerService.CurrentChangesDocument = longList[0];
          try
          {
            IDBObject document = this.session.GetObject(longList[0]);
            DocumentTypeSettings settings = this.docSettingsService.GetSettings(this.session.SessionGUID, Convert.ToInt32(dataTable1.Rows[index1][1]));
            if (this.CheckDesignation(changedObject, document, settings, attrDesignation))
            {
              if (attrDesignation != null && attrDesignation.Changed)
              {
                string initValue1 = !settings.DocumentTypeCodeInDesignation || !(settings.DocumentTypeCode != string.Empty) ? attrDesignation.NewValue : DocumentsHelper.AppendDocCode(this.session, attrDesignation.NewValue, settings.DocumentTypeCode);
                AttributeValues newAttrValues1 = new AttributeValues(SyncronizerService.AttrDesignationID, (object) initValue1);
                document.SetAttributesValues(new AttributeValues[1]
                {
                  newAttrValues1
                });
                if (this.notificationService != null)
                  this.notificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsExtendedEventArgs(document.ObjectID, this.eventArgs.ObjectType, new AttributeValues(SyncronizerService.AttrDesignationID, (object) attrDesignation.OrigValue), newAttrValues1));
                if (attrDesignation.OrigValue != null && attrDesignation.OrigValue.Values != null && attrDesignation.OrigValue.Values.Length == 1)
                {
                  string str = Convert.ToString(attrDesignation.OrigValue.Values[0]);
                  IDBAttribute attributeByGuid = changedObject.GetAttributeByGuid(new Guid("cad001f9-306c-11d8-b4e9-00304f19f545"));
                  if (attributeByGuid != null && GuidHelper.IsGuid(attributeByGuid.AsString))
                  {
                    DataTable dataTable2 = objectCollection.Select(new DBRecordSetParams(new ConditionStructure[3]
                    {
                      new ConditionStructure(attributeByGuid.AttributeID, RelationalOperators.Equal, (object) new Guid(attributeByGuid.AsString), LogicalOperators.AND, 0, false),
                      new ConditionStructure(SyncronizerService.AttrDesignationID, RelationalOperators.StartString, (object) str, LogicalOperators.AND, 0, false),
                      new ConditionStructure(-2, RelationalOperators.NotEqual, (object) changedObject.ObjectID, LogicalOperators.AND, 0, false)
                    }, new object[1]{ (object) -2 }));
                    for (int index2 = 0; index2 < dataTable2.Rows.Count; ++index2)
                    {
                      IDBObject objectActualCopy = this.session.GetObjectActualCopy(this.objectsCheckOutService.CheckOut(this.session, (IList<long>) new long[1]
                      {
                        Convert.ToInt64(dataTable2.Rows[index2][0])
                      }, true)[0], true);
                      string initValue2 = Convert.ToString(objectActualCopy.GetValuesByID(SyncronizerService.AttrDesignationID, true)[0]);
                      AttributeValues newAttrValues2 = new AttributeValues(SyncronizerService.AttrDesignationID, (object) initValue2.Replace(str, attrDesignation.NewValue));
                      objectActualCopy.SetAttributesValues(new AttributeValues[1]
                      {
                        newAttrValues2
                      });
                      if (this.notificationService != null)
                        this.notificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsExtendedEventArgs(objectActualCopy.ObjectID, this.eventArgs.ObjectType, new AttributeValues(SyncronizerService.AttrDesignationID, (object) initValue2), newAttrValues2));
                    }
                  }
                }
              }
              if (attrName != null)
              {
                if (attrName.Changed)
                {
                  AttributeValues newAttrValues = new AttributeValues(SyncronizerService.AttrNameID, (object) attrName.NewValue);
                  document.SetAttributesValues(new AttributeValues[1]
                  {
                    newAttrValues
                  });
                  if (this.notificationService != null)
                    this.notificationService.FireEvent((object) this, (NotificationEventArgs) new DBObjectsExtendedEventArgs(document.ObjectID, this.eventArgs.ObjectType, new AttributeValues(SyncronizerService.AttrNameID, (object) attrName.OrigValue), newAttrValues));
                }
              }
            }
          }
          finally
          {
            SyncronizerService.CurrentChangesDocument = 0L;
          }
        }
      }
    }
  }
}
