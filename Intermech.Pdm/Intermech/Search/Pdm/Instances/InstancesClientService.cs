// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Instances.InstancesClientService
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Search.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Search.Pdm.Instances;

public sealed class InstancesClientService : IInstancesClientService
{
  public long[] CreateInstances(long objectVersionID, long specFID = -1)
  {
    if (ObjectHelper.IsUnknownObjectID(objectVersionID))
      throw new ArgumentException();
    if (!InstancesHelper.CheckObjectForCreateInstances(objectVersionID))
    {
      int num = (int) MessageBox.Show("Создание исполнений невозможно, объект недопустимого типа или имеет в составе CAD-модель.", "Intermech Professional Solution", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return new long[0];
    }
    using (CreateInstancesForm createInstancesForm = new CreateInstancesForm())
    {
      createInstancesForm.ObjectVersionID = objectVersionID;
      createInstancesForm.SpecFID = specFID;
      int num = (int) createInstancesForm.ShowDialog();
      return createInstancesForm.LastCreatedInstancesVersionIds;
    }
  }

  public static long CheckExistingProductVersion(
    long specFID,
    string productDesignation,
    long prototypeObjectID,
    IUserSession session)
  {
    string FiltrationOwnerID = "cad001e0-306c-11d8-b4e9-00304f19f545";
    IDBRelationCollection relationCollection = session.GetRelationCollection(session.IdentHelper.DocRelationTypeID, FiltrationOwnerID);
    relationCollection.ChildObjectTypes = (IList<int>) new List<int>()
    {
      InstancesConstants.ProductObjectTypeID,
      InstancesConstants.StandardProductObjectTypeID,
      InstancesConstants.OtherProductObjectTypeID
    };
    ColumnDescriptor[] columns = new ColumnDescriptor[3]
    {
      new ColumnDescriptor((object) ObligatoryObjectAttributes.F_OBJECT_ID, AttributeSourceTypes.Object, ColumnContents.Value, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) InstancesConstants.GroupProductIDAttributeTypeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) InstancesConstants.DesignationAttributeTypeID, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.ASC, 0)
    };
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(InstancesConstants.DesignationAttributeTypeID, RelationalOperators.Equal, (object) productDesignation, LogicalOperators.NONE, 0, false)
      {
        AttributeSource = AttributeSourceTypes.Object
      }
    }, columns);
    if (paramSet.Tags == null)
      paramSet.Tags = new HybridDictionary();
    paramSet.Tags[(object) "{AB419A02-DE8A-4A8E-905A-D782F5B720E5}"] = (object) new long[2]
    {
      0L,
      1L
    };
    paramSet.Tags[(object) "{2FACA180-73B8-4F24-9928-5623661BBBE6}"] = (object) true;
    paramSet.Tags[(object) "{325F5CDB-8B8E-4B2D-9AA9-5624A0A64D7E}"] = (object) true;
    paramSet.Tags[(object) "{0422E069-0A1D-4235-85E8-C52C3516CFC1}"] = (object) true;
    DataTable dataTable = relationCollection.EntersIn(paramSet, specFID);
    string prevGenerationID = Guid.Empty.ToString();
    IDBObject objectById1 = session.GetObjectByID(prototypeObjectID, false);
    long id = objectById1 != null ? objectById1.ParentVersionID : -1L;
    if (id != -1L)
    {
      IDBObject objectById2 = session.GetObjectByID(id, false);
      if (objectById2 != null)
        prevGenerationID = Convert.ToString(objectById2.GetAttributeByID(InstancesConstants.GroupProductIDAttributeTypeID).Value);
    }
    long num = dataTable.Rows.Cast<DataRow>().Where<DataRow>((System.Func<DataRow, bool>) (dr => Convert.ToString(dr[2]) == productDesignation && Convert.ToString(dr[1]) == prevGenerationID)).Select<DataRow, long>((System.Func<DataRow, long>) (r => Convert.ToInt64(r[0]))).OrderByDescending<long, long>((System.Func<long, long>) (v => Math.Abs(v))).FirstOrDefault<long>();
    List<long> objectIDs;
    if (num == 0L || num == -1L)
      objectIDs = dataTable.Rows.Cast<DataRow>().Where<DataRow>((System.Func<DataRow, bool>) (dr => Convert.ToString(dr[2]) == productDesignation)).Select<DataRow, long>((System.Func<DataRow, long>) (r => Convert.ToInt64(r[0]))).OrderByDescending<long, long>((System.Func<long, long>) (v => Math.Abs(v))).ToList<long>();
    else
      objectIDs = new List<long>() { num };
    if (objectIDs.Count > 1)
    {
      ListDescriptor rootDescriptor = new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, 0, productDesignation, (IList) objectIDs);
      object[] objArray = SelectionWindow.Select($"Выберите базовую версию для новой версии исполнения \"{productDesignation}\"", (IDescriptor) rootDescriptor, typeof (IDBTypedObjectID), SelectionOptions.Default | SelectionOptions.DisableMultiselect | SelectionOptions.ForceFilterObjectsByRule);
      return objArray != null && objArray.Length != 0 && objArray[0] is IDBTypedObjectID dbTypedObjectId ? dbTypedObjectId.ObjectID : -1L;
    }
    return objectIDs.Count <= 0 ? -1L : objectIDs[0];
  }
}
