// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.RecalcCountsCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.Interfaces;
using Intermech.Search.UI;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.MRP2;

/// <summary>команда меню расчитать количества</summary>
internal class RecalcCountsCommand
{
  internal static long idShtuk = MeasureHelper.GetMeasureID(SystemGUIDs.objectShtukiGuid);
  internal static MeasuredValue uno = new MeasuredValue(1.0, RecalcCountsCommand.idShtuk);

  private static string GetScriptCode(IUserSession session)
  {
    long objectID = session.Configurations.ReadInteger("MRP2", "MRP2", "calc_scriptID", 0L, DBConfigMode.GlobalOnly);
    return session.GetObject(objectID, false)?.GetAttributeByGuid(new Guid("cad00366-306c-11d8-b4e9-00304f19f545"))?.Value.ToString();
  }

  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (!(items.GetItemData(0, typeof (IDBTypedObjectID)) is IDBTypedObjectID itemData) || !MetaDataHelper.IsObjectTypeChildOf(itemData.ObjectType, MRP2Consts.objtypeIdProductionLists))
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.StartLogHistory();
      try
      {
        string scriptCode1 = RecalcCountsCommand.GetScriptCode(sessionKeeper.Session);
        if (!string.IsNullOrEmpty(scriptCode1))
        {
          ICSharpScriptExecutor service = ServiceUtils.GetService<ICSharpScriptExecutor>((object) ApplicationServices.Container, true);
          AttributeValidationScriptParameters scriptParameters1 = new AttributeValidationScriptParameters()
          {
            UserSession = sessionKeeper.Session,
            ObjectID = itemData.ObjectID,
            RelationID = 0,
            ObjectAttributeValues = new List<AttributeValues>(),
            RelationAttributeValues = new List<AttributeValues>()
          };
          string scriptCode2 = scriptCode1;
          CSharpScriptInvocationOptions options = CSharpScriptInvocationOptions.Default;
          object[] objArray = new object[1]
          {
            (object) scriptParameters1
          };
          AttributeValidationScriptParameters scriptParameters2 = (AttributeValidationScriptParameters) service.Execute(scriptCode2, options, objArray);
        }
        else
        {
          IDBRelationCollection relationCollection = sessionKeeper.Session.GetRelationCollection(MRP2Consts.reltypeIdProductComposition);
          DBRecordSetParams ps = new DBRecordSetParams(new ConditionStructure[0], new object[4]
          {
            (object) ObligatoryObjectAttributes.F_PRJLINK_ID,
            (object) ObligatoryObjectAttributes.F_OBJECT_ID,
            (object) MRP2Consts.attrIdCount,
            (object) MRP2Consts.attrIdCountCorrect
          });
          RecalcCountsCommand.RecalcCountsInternal(sessionKeeper.Session, relationCollection, ref ps, itemData.ObjectID, 1, RecalcCountsCommand.uno, RecalcCountsCommand.uno, (MeasuredValue) null);
        }
        NotificationHelper.Notify((object) null, sessionKeeper.Session.GetModificationsHistoryList());
      }
      finally
      {
        sessionKeeper.Session.StopLogHistory();
      }
    }
  }

  internal static void RecalcCountsInternal(
    IUserSession session,
    IDBRelationCollection relcol,
    ref DBRecordSetParams ps,
    long proj,
    int level,
    MeasuredValue projCount,
    MeasuredValue totalCount,
    MeasuredValue exitAssemblyCount)
  {
    DataTable dataTable = relcol.ConsistFrom(ps, proj);
    if (dataTable == null)
      return;
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      long int64_1 = Convert.ToInt64(row[0]);
      long int64_2 = Convert.ToInt64(row[1]);
      string mValue1 = row[2].ToString();
      string mValue2 = row[3].ToString();
      MeasuredValue measuredValue1 = string.IsNullOrWhiteSpace(mValue1) ? (MeasuredValue) null : MeasureHelper.ConvertToMeasuredValue(mValue1, "шт", false);
      MeasuredValue measuredValue2;
      if (measuredValue1 != null && totalCount != null)
      {
        measuredValue2 = MeasureHelper.Multiply(totalCount, measuredValue1);
        if (measuredValue2.MeasureID != RecalcCountsCommand.idShtuk)
          measuredValue2 = MeasureHelper.ConvertToBaseMeasure(measuredValue2);
      }
      else
        measuredValue2 = (MeasuredValue) null;
      MeasuredValue operand1;
      if (measuredValue1 != null && projCount != null && level > 2)
      {
        operand1 = MeasureHelper.Multiply(projCount, measuredValue1);
        if (operand1.MeasureID != RecalcCountsCommand.idShtuk)
          operand1 = MeasureHelper.ConvertToBaseMeasure(operand1);
      }
      else
        operand1 = (MeasuredValue) null;
      MeasuredValue measuredValue3;
      if (measuredValue1 != null && exitAssemblyCount != null)
      {
        measuredValue3 = MeasureHelper.Multiply(exitAssemblyCount, measuredValue1);
        if (measuredValue3.MeasureID != RecalcCountsCommand.idShtuk)
          measuredValue3 = MeasureHelper.ConvertToBaseMeasure(measuredValue3);
      }
      else
        measuredValue3 = (MeasuredValue) null;
      MeasureDescriptor descriptor = MeasureHelper.FindDescriptor(measuredValue2.MeasureID);
      MeasuredValue measuredValue4 = MeasureHelper.ConvertToMeasuredValue(mValue2, descriptor, false);
      MeasuredValue measuredValue5;
      MeasuredValue measuredValue6;
      MeasuredValue measuredValue7;
      if (measuredValue4 != null)
      {
        measuredValue5 = measuredValue2 == null ? (MeasuredValue) null : MeasureHelper.Add(measuredValue2, measuredValue4);
        measuredValue6 = measuredValue3 == null ? (MeasuredValue) null : MeasureHelper.Add(measuredValue3, measuredValue4);
        measuredValue7 = operand1 == null ? (MeasuredValue) null : MeasureHelper.Add(operand1, measuredValue4);
      }
      else
      {
        measuredValue5 = measuredValue2;
        measuredValue6 = measuredValue3;
        measuredValue7 = operand1;
      }
      IDBRelation relation = session.GetRelation(int64_1);
      relation.Attributes.AddAttribute(MRP2Consts.attrIdCountForPL, false).Value = (object) measuredValue5;
      relation.Attributes.AddAttribute(MRP2Consts.attrIdCountFor1stAssembly, false).Value = (object) measuredValue7;
      IDBAttribute dbAttribute = relation.Attributes.AddAttribute(MRP2Consts.attrIdCountForExitAssembly, false);
      if (dbAttribute.IsNull)
        dbAttribute.Value = (object) measuredValue6;
      else if (dbAttribute is IDBMeasureAttribute measureAttribute)
        measuredValue3 = measureAttribute.Value;
      if (level == 1)
        measuredValue3 = RecalcCountsCommand.uno;
      RecalcCountsCommand.RecalcCountsInternal(session, relcol, ref ps, int64_2, level + 1, measuredValue1, measuredValue2, measuredValue3);
    }
  }
}
