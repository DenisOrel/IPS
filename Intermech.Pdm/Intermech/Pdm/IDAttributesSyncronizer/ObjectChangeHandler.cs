// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.IDAttributesSyncronizer.ObjectChangeHandler
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

internal abstract class ObjectChangeHandler : IObjectChangeHandler
{
  protected DBObjectsExtendedEventArgs eventArgs;
  protected IUserSession session;
  protected IObjectsCheckOutService objectsCheckOutService;
  protected IDocumentTypeSettingsService docSettingsService;
  protected INotificationService notificationService;

  public ObjectChangeHandler(DBObjectsExtendedEventArgs eventArgs, IUserSession session)
  {
    this.eventArgs = eventArgs;
    this.session = session;
    this.objectsCheckOutService = ServicesManager.GetService(typeof (IObjectsCheckOutService)) as IObjectsCheckOutService;
    this.docSettingsService = session.GetCustomService(typeof (IDocumentTypeSettingsService)) as IDocumentTypeSettingsService;
    this.notificationService = ServicesManager.GetService(typeof (INotificationService)) as INotificationService;
  }

  protected abstract void OnHandle(
    IDBObject changedObject,
    DBRecordSetParams dbParams,
    IDBRelationCollection rellCollection,
    IDAttributeInfo attrDesignation,
    IDAttributeInfo attrName);

  public void Handle(IDAttributeInfo attrDesignation, IDAttributeInfo attrName)
  {
    Dictionary<int, List<int>> applicabilities = this.GetApplicabilities(this.eventArgs.ObjectType);
    if (applicabilities.Count == 0)
      return;
    IDBTransactions customService = (IDBTransactions) this.session.GetCustomService(typeof (IDBTransactions));
    IFiltrationService service = (IFiltrationService) ServicesManager.GetService(typeof (IFiltrationService));
    try
    {
      customService.StartTransaction();
      foreach (KeyValuePair<int, List<int>> keyValuePair in applicabilities)
      {
        DBRecordSetParams dbParams = new DBRecordSetParams(new ConditionStructure[1]
        {
          keyValuePair.Value.Count > 1 ? new ConditionStructure(-7, RelationalOperators.In, (object) keyValuePair.Value.ToArray(), LogicalOperators.AND, 0, false) : new ConditionStructure(-7, RelationalOperators.Equal, (object) keyValuePair.Value[0], LogicalOperators.AND, 0, false)
        }, new object[2]{ (object) -2, (object) -7 });
        IDBRelationCollection relationCollection = this.session.GetRelationCollection(keyValuePair.Key);
        relationCollection.FiltrationOwnerID = service.FiltrationServiceOwnerID;
        for (int index = 0; index < this.eventArgs.ObjectIDs.Count; ++index)
        {
          IDBObject changedObject = this.session.GetObject(this.eventArgs.ObjectIDs[index]);
          if (changedObject.ObjectVerType == 0)
            this.OnHandle(changedObject, dbParams, relationCollection, attrDesignation, attrName);
        }
      }
      customService.Commit();
    }
    catch
    {
      customService.Rollback();
      throw;
    }
  }

  public bool IDAttributesChanged(out IDAttributeInfo attrDesignation, out IDAttributeInfo attrName)
  {
    attrDesignation = (IDAttributeInfo) null;
    attrName = (IDAttributeInfo) null;
    if (this.eventArgs.AttributeValuesArray == null)
      return false;
    foreach (AttributeValues attributeValues in this.eventArgs.AttributeValuesArray)
    {
      if (attributeValues.AttributeID == SyncronizerService.AttrDesignationID)
      {
        attrDesignation = new IDAttributeInfo(true, Convert.ToString(attributeValues.Values[0]));
        foreach (AttributeValues origAttributeValues in this.eventArgs.OrigAttributeValuesArray)
        {
          if (origAttributeValues.AttributeID == SyncronizerService.AttrDesignationID)
          {
            attrDesignation.OrigValue = origAttributeValues;
            break;
          }
        }
        this.OnGetDesignationValue(attrDesignation, this.eventArgs.ObjectType);
      }
      else if (attributeValues.AttributeID == SyncronizerService.AttrNameID)
      {
        attrName = new IDAttributeInfo(true, Convert.ToString(attributeValues.Values[0]));
        foreach (AttributeValues origAttributeValues in this.eventArgs.OrigAttributeValuesArray)
        {
          if (origAttributeValues.AttributeID == SyncronizerService.AttrNameID)
          {
            attrName.OrigValue = origAttributeValues;
            break;
          }
        }
      }
    }
    return attrDesignation != null || attrName != null;
  }

  protected virtual void OnGetDesignationValue(IDAttributeInfo attrDesignation, int objectTypeID)
  {
  }

  protected abstract Dictionary<int, List<int>> GetApplicabilities(int objectTypeID);

  protected void SetEnabledApplicabilities(
    Dictionary<int, List<int>> applics,
    int objTypeID,
    int relTypeID)
  {
    List<int> intList = (List<int>) null;
    List<int> childObjectTypes = this.GetChildObjectTypes(objTypeID);
    if (applics.TryGetValue(relTypeID, out intList))
    {
      foreach (int num in childObjectTypes)
      {
        if (!intList.Contains(num))
          intList.Add(num);
      }
    }
    else
      applics.Add(relTypeID, childObjectTypes);
  }

  protected void RemoveDisabledApplicabilities(
    Dictionary<int, List<int>> applics,
    int objTypeID,
    int relTypeID)
  {
    List<int> intList = (List<int>) null;
    if (!applics.TryGetValue(relTypeID, out intList) || !intList.Contains(objTypeID))
      return;
    intList.Remove(objTypeID);
  }

  protected List<int> GetChildObjectTypes(int parentType)
  {
    List<int> childObjectTypes = new List<int>((IEnumerable<int>) new int[1]
    {
      parentType
    });
    DataTable dataTable = this.session.GetObjectTypeCollection(parentType).SelectRecursive(string.Empty);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
      childObjectTypes.Add(Convert.ToInt32(dataTable.Rows[index]["F_OBJECT_TYPE"]));
    return childObjectTypes;
  }
}
