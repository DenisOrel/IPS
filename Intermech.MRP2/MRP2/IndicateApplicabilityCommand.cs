// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.IndicateApplicabilityCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Client.Core;
using Intermech.DataFormats;
using Intermech.ECO.Client;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.Compositions.CompositionService;
using Intermech.Interfaces.Document;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2;

internal class IndicateApplicabilityCommand
{
  /// <summary>
  /// Команда Указать применяемость в ПВ
  /// 1) Выбрать ПВ
  /// 2) Выбрать выходную сборку из ПВ
  /// 3) Задать диапазон комплектов
  /// 4) Записать текст в ИИ
  /// </summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    if (!(viewServices.GetService(typeof (IViewState)) is IViewState service1) || (service1.ViewState & ViewStateFlags.ReadOnly) != ViewStateFlags.None)
      return;
    ECOAncestorForm service2 = viewServices.GetService(typeof (ECOAncestorForm)) as ECOAncestorForm;
    if (items.Count <= 0 || service2 == null || service2.ReadOnly)
      return;
    long objVerId = items.GetItemID(0).GetObjVerID();
    long revRelation = RevHelper.GetRevRelation(service2.ecoID, objVerId);
    string str1 = IndicateApplicabilityCommand.SetPLForAll(new List<PendingLink>()
    {
      new PendingLink(ECOGoal.NoGoal, -1)
      {
        verID = objVerId,
        relId = revRelation
      }
    });
    if (string.IsNullOrEmpty(str1))
      return;
    DocumentTreeNode ecoRow4Relation = (DocumentTreeNode) service2.ECO.FindEcoRow4Relation(objVerId);
    if (ecoRow4Relation == null || !(ecoRow4Relation.FindFirstNodeFromTemplate_Recursive("IT1") is TextData templateRecursive))
      return;
    string str2 = $"{templateRecursive.Text.Trim()}\r\n{str1}";
    templateRecursive.Text = str2.Trim();
  }

  internal static string SetPLForAll(List<PendingLink> objList)
  {
    IReadOnlyList<IDBObjectID> dbObjectIdList = SelectDialog.Objects((IReadOnlyCollection<int>) new int[1]
    {
      MRP2Consts.objtypeIdProductionLists
    }, "Выберите объект", options: SelectionOptions.SelectObjects | SelectionOptions.DisableMultiselect | SelectionOptions.ForceFilterObjectsByRule, operationName: "IndicateApplicability", disableGlobalContextMenuCommands: true);
    if (dbObjectIdList == null || dbObjectIdList.Count <= 0)
      return string.Empty;
    List<ObjInfoItem> objects = new List<ObjInfoItem>();
    objects.Add(new ObjInfoItem(dbObjectIdList[0].Value));
    ColumnDescriptor[] columns = new ColumnDescriptor[2]
    {
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.ID, ColumnNameMapping.ID, SortOrders.NONE, 0),
      new ColumnDescriptor((object) MRP2Consts.attrIdCount, AttributeSourceTypes.Relation, ColumnContents.Text, ColumnNameMapping.ID, SortOrders.NONE, 0)
    };
    CompositionLoadingParams loadingParams = new CompositionLoadingParams((IEnumerable<ObjInfoItem>) objects, (IEnumerable<int>) new int[1]
    {
      MRP2Consts.objtypeIdExitAssembly
    }, (IEnumerable<int>) null, (IEnumerable<int>) new int[1]
    {
      MRP2Consts.reltypeIdProductComposition
    }, (IEnumerable<ColumnDescriptor>) columns, (IEnumerable<ConditionStructure>) null, true, false, 1, (VersionsRule) null, "cad00601-306c-11d8-b4e9-00304f19f545");
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      DataTable source = ServiceUtils.GetService<ICompositionLoadService>((object) sessionKeeper.Session, true).LoadComplexCompositions((object) sessionKeeper.Session, loadingParams);
      if (source == null || source.Rows.Count <= 0)
        throw new NotificationException("В выбранной ведомости отсутсвуют выходные сборочные единицы");
      object[] objArray = SelectionWindow.Select("Выберите выходную сборку", "", (IDescriptor) new ListDescriptor(Intermech.Navigator.Consts.CategoryVersionsObjectNode, MRP2Consts.objtypeIdExitAssembly, "", (IList) source.AsEnumerable().Select<DataRow, long>((System.Func<DataRow, long>) (row => Convert.ToInt64(row[0]))).ToList<long>()), typeof (IDBObjectID), SelectionOptions.HideTree | SelectionOptions.SelectObjects | SelectionOptions.DisableSelectFromTree | SelectionOptions.DisableMultiselect);
      if (objArray == null)
        return string.Empty;
      string from = "";
      string to = "";
      long objectID = (objArray[0] as IDBObjectID).Value;
      string max_count = source.Select($"([{-2}] = {objectID})")[0][1].ToString();
      if (DialogResult.OK != ComplectNumberDialog.Execute(ref from, ref to, max_count))
        return string.Empty;
      long id = sessionKeeper.Session.GetObjectInfo(dbObjectIdList[0].Value).ID;
      string asString1 = sessionKeeper.Session.GetObjectAttribute(dbObjectIdList[0].Value, (object) MRP2Consts.attrIdProductionListNumber, true, false).AsString;
      string asString2 = sessionKeeper.Session.GetObjectAttributeByGuid(objectID, new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).AsString;
      string asString3 = (sessionKeeper.Session.GetObjectAttributeByID(objectID, MRP2Consts.attrIdPKDSE_Id) ?? throw new NotificationException("В выходной сборке отсутствует атрибут \"Идентификатор ПК ДСЕ\" на изделие")).AsString;
      string str = $"Ведомость {asString1} изделие {asString2} комплекты с {from} по {to}";
      string newValue = $"{id}:{asString3}:{from}:{to}";
      foreach (PendingLink pendingLink in objList)
      {
        IDBRelation relation = !Intermech.Consts.IsUndefinedObjectId(pendingLink.relId) ? sessionKeeper.Session.GetRelation(pendingLink.relId, false) : (IDBRelation) null;
        if (relation != null)
        {
          IDBAttribute attributeById = relation.GetAttributeByID(MRP2Consts.attrIdApplicabilityinPL);
          if (attributeById == null)
            relation.Attributes.AddAttribute(MRP2Consts.attrIdApplicabilityinPL, true, new object[1]
            {
              (object) newValue
            });
          else
            attributeById.AddValue((object) newValue);
        }
      }
      return str;
    }
  }
}
