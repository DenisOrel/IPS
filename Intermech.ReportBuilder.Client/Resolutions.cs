// Decompiled with JetBrains decompiler
// Type: Resolutions
// Assembly: Intermech.ReportBuilder.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 84A30C1D-3856-44D0-92A6-A87D49736592
// Assembly location: D:\IPS\Client\Intermech.ReportBuilder.Client.dll

using Intermech;
using Intermech.Expert.Scenarios;
using Intermech.Interfaces;
using Intermech.Interfaces.Document;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

#nullable disable
public class Resolutions : ICustomReportScenario
{
  public bool Execute(IUserSession session, ImDocumentData doc, long[] objectIDs)
  {
    try
    {
      (doc.FindNode("37") as TextData).AssignText($"Отчет по поручениям на {DateTime.Now.ToString("dd.MM.yyyy")}", false, false, false);
      DocumentTreeNode node = doc.FindNode("2");
      DataTable dataTable1 = session.GetObjectCollection(new Guid("cadd9259-306c-11d8-b4e9-00304f19f545")).Select(new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(-2, RelationalOperators.In, (object) objectIDs, LogicalOperators.NONE, 0, false)
      }, new ColumnDescriptor[9]
      {
        new ColumnDescriptor((object) -2, SortOrders.NONE, 0),
        new ColumnDescriptor((object) -7, SortOrders.NONE, 0),
        new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid("cadd924f-306c-11d8-b4e9-00304f19f545")), SortOrders.NONE, 0),
        new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid("cadd924b-306c-11d8-b4e9-00304f19f545")), SortOrders.NONE, 0),
        new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid("cad0001c-306c-11d8-b4e9-00304f19f545")), SortOrders.NONE, 0),
        new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid("cadd9283-306c-11d8-b4e9-00304f19f545")), ColumnContents.String, ColumnNameMapping.Index, SortOrders.NONE, 0),
        new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid("cadd924c-306c-11d8-b4e9-00304f19f545")), SortOrders.NONE, 0),
        new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid("cadd9284-306c-11d8-b4e9-00304f19f545")), SortOrders.NONE, 0),
        new ColumnDescriptor((object) MetaDataHelper.GetAttributeTypeID(new Guid("28e5deeb-1dd8-4745-80b0-657ee0b44bee")), SortOrders.NONE, 0)
      }));
      IDBRelationCollection relationCollection = session.GetRelationCollection(MetaDataHelper.GetRelationTypeID(new Guid("cadd927c-306c-11d8-b4e9-00304f19f545")));
      for (int index1 = 0; index1 < dataTable1.Rows.Count; ++index1)
      {
        DocumentTreeNode child1 = doc.Template.FindNode("49").CloneFromTemplate(true, true);
        node.AddChildNode(child1, false, false);
        (child1.FindFirstNodeFromTemplate_Recursive("50") as TextData).AssignText(MetaDataHelper.GetObjectTypeName(Convert.ToInt32(dataTable1.Rows[index1][1])), false, false, false);
        object obj = dataTable1.Rows[index1][2];
        DateTime dateTime = Convert.ToDateTime(dataTable1.Rows[index1][3]);
        string str1 = dateTime.ToString("dd.MM.yyyy HH:mm");
        string str2 = $"{obj} от {str1}";
        bool flag = false;
        string str3 = "Отправитель: ";
        if (dataTable1.Rows[index1][5] != DBNull.Value)
        {
          str3 += Convert.ToString(dataTable1.Rows[index1][5]);
          flag = true;
        }
        if (dataTable1.Rows[index1][6] != DBNull.Value)
        {
          str3 += $" {dataTable1.Rows[index1][6]}";
          flag = true;
        }
        if (dataTable1.Rows[index1][7] != DBNull.Value)
        {
          string str4 = str3;
          dateTime = Convert.ToDateTime(dataTable1.Rows[index1][7]);
          string str5 = dateTime.ToString("dd.MM.yyyy HH:mm");
          str3 = str4 + str5;
          flag = true;
        }
        if (flag)
          str2 = $"{str2}\n{str3}";
        (child1.FindFirstNodeFromTemplate_Recursive("51") as TextData).AssignText(str2, false, false, false);
        (child1.FindFirstNodeFromTemplate_Recursive("53") as TextData).AssignText(Convert.ToString(dataTable1.Rows[index1][4]), false, false, false);
        (child1.FindFirstNodeFromTemplate_Recursive("43") as TextData).AssignText(Convert.ToString(dataTable1.Rows[index1][8]), false, false, false);
        DataTable dataTable2 = relationCollection.ConsistFrom(new DBRecordSetParams(new ConditionStructure[1]
        {
          new ConditionStructure(-7, RelationalOperators.Equal, (object) MetaDataHelper.GetObjectTypeID(new Guid("cadd927f-306c-11d8-b4e9-00304f19f545")), LogicalOperators.NONE, 0, false)
        }, new object[1]{ (object) -2 }), Convert.ToInt64(dataTable1.Rows[index1][0]));
        DocumentTreeNode templateRecursive1 = child1.FindFirstNodeFromTemplate_Recursive("54");
        for (int index2 = 0; index2 < dataTable2.Rows.Count; ++index2)
        {
          IDBObject dbObject = session.GetObject(Convert.ToInt64(dataTable2.Rows[index2][0]));
          DocumentTreeNode child2 = doc.Template.FindNode("55").CloneFromTemplate(true, true);
          templateRecursive1.AddChildNode(child2, false, false);
          IDBAttribute attributeByGuid1 = dbObject.GetAttributeByGuid(new Guid("cadd9291-306c-11d8-b4e9-00304f19f545"));
          (child2.FindFirstNodeFromTemplate_Recursive("56") as TextData).AssignText(attributeByGuid1.AsString, false, false, false);
          IDBAttribute attributeByGuid2 = dbObject.GetAttributeByGuid(new Guid("cadd9253-306c-11d8-b4e9-00304f19f545"));
          TextData templateRecursive2 = child2.FindFirstNodeFromTemplate_Recursive("58") as TextData;
          string empty1;
          if (attributeByGuid2 == null || !(attributeByGuid2.AsDateTime != DateTime.MinValue))
          {
            empty1 = string.Empty;
          }
          else
          {
            dateTime = attributeByGuid2.AsDateTime;
            empty1 = dateTime.ToString("dd.MM.yyyy", (IFormatProvider) CultureInfo.CurrentCulture);
          }
          templateRecursive2.AssignText(empty1, false, false, false);
          IDBAttribute attributeByGuid3 = dbObject.GetAttributeByGuid(new Guid("cadd924e-306c-11d8-b4e9-00304f19f545"));
          TextData templateRecursive3 = child2.FindFirstNodeFromTemplate_Recursive("8") as TextData;
          string empty2;
          if (attributeByGuid3 == null || !(attributeByGuid3.AsDateTime != DateTime.MinValue))
          {
            empty2 = string.Empty;
          }
          else
          {
            dateTime = attributeByGuid3.AsDateTime;
            empty2 = dateTime.ToString("dd.MM.yyyy", (IFormatProvider) CultureInfo.CurrentCulture);
          }
          templateRecursive3.AssignText(empty2, false, false, false);
          string empty3 = string.Empty;
          IDBAttribute attributeByGuid4 = dbObject.GetAttributeByGuid(new Guid("13446afb-6d1e-4b74-9959-60a4fcf949fa"));
          if (attributeByGuid4 != null)
            (child2.FindFirstNodeFromTemplate_Recursive("57") as TextData).AssignText(attributeByGuid4.Description, false, false, false);
          IDBAttribute attributeByGuid5 = dbObject.GetAttributeByGuid(new Guid("cadd928c-306c-11d8-b4e9-00304f19f545"));
          if (attributeByGuid5 != null)
            (child2.FindFirstNodeFromTemplate_Recursive("7") as TextData).AssignText(attributeByGuid5.AsString, false, false, false);
          IDBAttribute attributeByGuid6 = dbObject.GetAttributeByGuid(new Guid("cadd9294-306c-11d8-b4e9-00304f19f545"));
          Dictionary<long, string> dictionary1 = new Dictionary<long, string>();
          for (int index3 = 0; index3 < attributeByGuid6.ValuesCount; ++index3)
          {
            attributeByGuid6.Index = index3;
            dictionary1.Add(attributeByGuid6.AsInteger, attributeByGuid6.AsString);
          }
          IDBAttribute attributeByGuid7 = dbObject.GetAttributeByGuid(new Guid("cadd9296-306c-11d8-b4e9-00304f19f545"));
          IDBAttribute attributeByGuid8 = dbObject.GetAttributeByGuid(new Guid("cadd9298-306c-11d8-b4e9-00304f19f545"));
          Dictionary<long, string> dictionary2 = (Dictionary<long, string>) null;
          if (attributeByGuid7 != null && attributeByGuid8 != null)
          {
            dictionary2 = new Dictionary<long, string>(attributeByGuid7.ValuesCount);
            for (int index4 = 0; index4 < attributeByGuid7.ValuesCount; ++index4)
            {
              attributeByGuid7.Index = index4;
              attributeByGuid8.Index = index4;
              dictionary2.Add(attributeByGuid7.AsInteger, (string) attributeByGuid8.Value);
            }
          }
          if (dictionary1.Count > 0)
          {
            DocumentTreeNode templateRecursive4 = child2.FindFirstNodeFromTemplate_Recursive("23");
            foreach (KeyValuePair<long, string> keyValuePair in dictionary1)
            {
              DocumentTreeNode child3 = doc.Template.FindNode("24").CloneFromTemplate(true, true);
              templateRecursive4.AddChildNode(child3, false, false);
              (child3.FindFirstNodeFromTemplate_Recursive("25") as TextData).AssignText(keyValuePair.Value, false, false, false);
              if (dictionary2 != null && dictionary2.ContainsKey(keyValuePair.Key))
                (child3.FindFirstNodeFromTemplate_Recursive("26") as TextData).AssignText(dictionary2[keyValuePair.Key], false, false, false);
            }
          }
        }
      }
      doc.UpdateLayout(0, true, false);
      return true;
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
      return false;
    }
  }
}
