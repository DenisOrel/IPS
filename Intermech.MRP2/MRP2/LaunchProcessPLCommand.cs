// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.LaunchProcessPLCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Workflow;
using Intermech.Workflow.Design;
using System;
using System.Data;

#nullable disable
namespace Intermech.MRP2;

/// <summary>команда меню запустить процесс ПВ</summary>
internal class LaunchProcessPLCommand
{
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      long schemeID = 0;
      long num1 = 0;
      IDBObject dbObject = sessionKeeper.Session.GetObject(MRP2Consts.WorkFlowGroupGuid, false);
      if (dbObject != null)
      {
        num1 = dbObject.ObjectID;
        int relationTypeId = MetaDataHelper.GetRelationTypeID("cad00022-306c-11d8-b4e9-00304f19f545");
        IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(relationTypeId);
        DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[2]
        {
          (object) ObligatoryObjectAttributes.F_OBJECT_ID,
          (object) ObligatoryObjectAttributes.F_PRJLINK_ID
        });
        relationCollection.ObjectTypeID = wfConsts.SchemeCategoriesID;
        long count = (long) relationCollection.ConsistFrom(paramSet, num1).Rows.Count;
        relationCollection.ObjectTypeID = wfConsts.SchemesTypeID;
        DataTable dataTable = relationCollection.ConsistFrom(paramSet, num1);
        long num2 = count + (long) dataTable.Rows.Count;
        if (num2 == 1L && dataTable.Rows.Count == 1)
          schemeID = Convert.ToInt64(dataTable.Rows[0][0]);
        else if (num2 == 0L)
          num1 = 0L;
      }
      wfFunx.CreateProcess(schemeID, (ISimpleSelectedItems) items, num1);
    }
  }
}
