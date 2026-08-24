// Decompiled with JetBrains decompiler
// Type: Script
// Assembly: Intermech.ReportBuilder.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 84A30C1D-3856-44D0-92A6-A87D49736592
// Assembly location: D:\IPS\Client\Intermech.ReportBuilder.Client.dll

using Intermech;
using Intermech.Interfaces;
using Intermech.Interfaces.Workflow;
using Intermech.Kernel.Search;
using System;
using System.Data;

#nullable disable
public class Script
{
  public static void Execute(IActivity activity)
  {
    int num = 3;
    Guid guid = new Guid("13446afb-6d1e-4b74-9959-60a4fcf949fa");
    IUserSession session = activity.Session;
    IDBTransactions customService = (IDBTransactions) session.GetCustomService(typeof (IDBTransactions));
    customService.StartTransaction();
    try
    {
      IDBAttribute attributeByGuid1 = activity.GetAttributeByGuid(new Guid("cad002ce-306c-11d8-b4e9-00304f19f545"));
      IDBAttribute attributeByGuid2 = session.GetObject(attributeByGuid1.AsInteger, true).GetAttributeByGuid(new Guid("cadd92dd-306c-11d8-b4e9-00304f19f545"), true);
      IDBObject dbObject1 = session.GetObject(attributeByGuid2.AsInteger, true);
      IDBAttribute dbAttribute1 = dbObject1.GetAttributeByGuid(guid);
      if (dbAttribute1 == null)
      {
        IDBAttributeType attributeType = session.GetAttributeType(guid, true);
        dbAttribute1 = dbObject1.Attributes.AddAttribute(attributeType.AttributeID, false);
      }
      dbAttribute1.AsInteger = (long) num;
      bool flag1 = false;
      IDBObject dbObject2 = activity.Attachments[0].Object;
      DataTable dataTable = session.GetRelationCollection(session.GetRelationType(new Guid("cadd927c-306c-11d8-b4e9-00304f19f545")).RelationType).ConsistFrom(new DBRecordSetParams((ConditionStructure[]) null, new object[2]
      {
        (object) -2,
        (object) dbAttribute1.AttributeID
      }), dbObject2.ObjectID);
      bool flag2 = true;
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        if (Convert.ToInt64(dataTable.Rows[index][0]) != dbObject1.ObjectID && (dataTable.Rows[index][1] == DBNull.Value || dataTable.Rows[index][1] == null || Convert.ToInt32(dataTable.Rows[index][1]) != num))
        {
          flag2 = false;
          break;
        }
      }
      if (flag2)
      {
        if (dbObject2.ObjectModifyMode == ObjectModifyModes.Checkout && dbObject2.CheckoutBy == 0L)
        {
          dbObject2 = dbObject2.CheckOut();
          flag1 = true;
        }
        IDBAttribute dbAttribute2 = dbObject2.GetAttributeByGuid(guid);
        if (dbAttribute2 == null)
        {
          IDBAttributeType attributeType = session.GetAttributeType(guid, true);
          dbAttribute2 = dbObject2.Attributes.AddAttribute(attributeType.AttributeID, false);
        }
        dbAttribute2.AsInteger = (long) num;
        if (flag1)
          dbObject2.CheckIn();
      }
      customService.Commit();
    }
    catch (Exception ex)
    {
      customService.Rollback();
      throw new Exception($"Ошибка при установке статуса поручения: {ex.Message}", ex);
    }
  }
}
