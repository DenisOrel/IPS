// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionListTree
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Collections;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.MRP2;

public class ProductionListTree
{
  private DataTable _source;
  private long _root;
  private ProductionListTreeItem _rootItem;
  private List<Guid> _target;
  private List<ProductionListTreeItem> _targetItems;

  internal List<ProductionListTreeItem> targetItems => this._targetItems;

  internal List<Guid> target => this._target;

  public ProductionListTree(DataTable source, long root, string filterIDs)
  {
    this._source = source;
    this._root = root;
    this._target = (List<Guid>) new HashedList<Guid>();
    this._targetItems = (List<ProductionListTreeItem>) new HashedList<ProductionListTreeItem>();
    this._rootItem = new ProductionListTreeItem(this, this._root);
    foreach (DataRowView dataRowView in new DataView(this._source)
    {
      RowFilter = filterIDs
    })
      this._target.Add(DataSetProcessor.GetGuidValue(dataRowView.Row, $"{-26}", Guid.Empty));
    this.BuildTree(this._rootItem);
    this._targetItems.Reverse();
  }

  private void BuildTree(ProductionListTreeItem parentItem)
  {
    string str = $"[{-21}] = {parentItem.ObjectID}";
    foreach (DataRowView dataRowView in new DataView(this._source)
    {
      RowFilter = str
    })
    {
      ProductionListTreeItem parentItem1 = new ProductionListTreeItem(parentItem, dataRowView.Row);
      parentItem.Add(parentItem1);
      this.BuildTree(parentItem1);
    }
  }

  /// <summary>
  /// Ф-ия возьмет на редактирование ветку состава до копий которые перечислены в targetItems и заменит их на другую копию
  /// </summary>
  /// <param name="session">Сессия</param>
  /// <param name="replaceObjectID">Идентификатор копии на которую будем менять</param>
  public void CheckOutCopiesAndReplacePart(IUserSession session, long replaceObjectID)
  {
    Guid empty = Guid.Empty;
    foreach (ProductionListTreeItem targetItem in this._targetItems)
    {
      if (targetItem.ObjectID != replaceObjectID)
      {
        long num = replaceObjectID;
        Guid prjGuid = targetItem._prjGuid;
        for (ProductionListTreeItem parent = targetItem._parent; parent != null; parent = parent._parent)
        {
          if (parent._checkout_by != session.UserID && parent._parent != null)
          {
            Dictionary<Guid, Guid> newGuids;
            long withReplacedPart = MRP2Service.CreateProductionCopyWithReplacedPart(session, parent.ObjectID, parent._object_type, prjGuid, num, true, out Guid _, out newGuids);
            this.replacePrjGuids(parent, newGuids);
            num = withReplacedPart;
            parent._object_id = num;
            parent._checkout_by = session.UserID;
            prjGuid = parent._prjGuid;
          }
          else
          {
            MRP2Service.ReplaceLink(session, parent.ObjectID, prjGuid, num);
            break;
          }
        }
      }
    }
  }

  /// <summary>
  /// заменить гуиды связей в состве копии, т.к. когда копия берется на редактирование - создается новая копия с новыми связями
  /// </summary>
  /// <param name="parent"></param>
  /// <param name="newGuids"></param>
  private void replacePrjGuids(ProductionListTreeItem parent, Dictionary<Guid, Guid> newGuids)
  {
    foreach (ProductionListTreeItem productionListTreeItem in (List<ProductionListTreeItem>) parent)
      productionListTreeItem._prjGuid = newGuids[productionListTreeItem._prjGuid];
  }

  /// <summary>проверить что item выходит в выходную сборку</summary>
  /// <param name="item">элемент состава</param>
  /// <param name="exitasm_id">идентификатор ПК ДСЕ выходной сборки</param>
  /// <returns></returns>
  private bool CheckExitAssembly(
    ProductionListTreeItem item,
    string exitasm_id,
    out long exitAsmID)
  {
    exitAsmID = 0L;
    if (exitasm_id == "")
      return true;
    for (; item != null && item._row != null; item = item._parent)
    {
      if (!(DataSetProcessor.GetStringValue(item._row, $"{MRP2Consts.attrIdPKDSE_Id}", "") != exitasm_id))
      {
        exitAsmID = item.ObjectID;
        return true;
      }
    }
    return false;
  }

  /// <summary>
  /// Функция возьмет на редактирование ветку состава до копии и выполнит диалого замены версии для копии
  /// </summary>
  /// <param name="session">Сессия</param>
  /// <param name="replaceObjectID">Идентификатор иззделия на которое надо сделать замену</param>
  /// <param name="plVersion">Номер версии заказа в котором мы сейчас делаем замену(для сохранения спец.полей)</param>
  public void CheckOutCopiesAndReplacePartVersionDialog(
    IUserSession session,
    long replaceObjectID,
    long plVersion,
    string exitasm_id,
    string from_complect,
    string to_complect)
  {
    Guid empty = Guid.Empty;
    Dictionary<long, long> dictionary = new Dictionary<long, long>();
    int start = int.TryParse(from_complect, out start) ? start : -1;
    int end = int.TryParse(to_complect, out end) ? end : -1;
    foreach (ProductionListTreeItem targetItem in this._targetItems)
    {
      long exitAsmID;
      if (this.CheckExitAssembly(targetItem, exitasm_id, out exitAsmID))
      {
        long num1;
        if (dictionary.ContainsKey(targetItem.ObjectID))
        {
          num1 = dictionary[targetItem.ObjectID];
        }
        else
        {
          num1 = ReplaceVersionCommand.ReplacePartVersionDialog(session, targetItem.ObjectID, replaceObjectID, plVersion, new MRP2Consts.ArticleSupplyMethod?());
          dictionary[targetItem.ObjectID] = num1;
        }
        long num2 = !Consts.IsUndefinedObjectId(num1) ? num1 : throw new AbortException();
        Guid prjGuid = targetItem._prjGuid;
        for (ProductionListTreeItem parent = targetItem._parent; parent != null; parent = parent._parent)
        {
          if (parent._checkout_by != session.UserID && parent._parent != null)
          {
            Guid new_relation;
            Dictionary<Guid, Guid> newGuids;
            long withReplacedPart = MRP2Service.CreateProductionCopyWithReplacedPart(session, parent.ObjectID, parent._object_type, prjGuid, num1, true, out new_relation, out newGuids);
            this.replacePrjGuids(parent, newGuids);
            if (num1 == num2 && exitasm_id != "")
              ApplyComplectAttr(session.GetRelation(new_relation, withReplacedPart));
            num1 = withReplacedPart;
            parent._object_id = num1;
            parent._checkout_by = session.UserID;
            prjGuid = parent._prjGuid;
          }
          else
          {
            IDBRelation rel = MRP2Service.ReplaceLink(session, parent.ObjectID, prjGuid, num1);
            if (num1 == num2 && exitasm_id != "")
            {
              ApplyComplectAttr(rel);
              break;
            }
            break;
          }
        }
      }

      void ApplyComplectAttr(IDBRelation rel)
      {
        ComplectNodeList complectNodeList = new ComplectNodeList();
        complectNodeList.LoadData(rel);
        complectNodeList.AppendData(session, this._root, exitAsmID, start, end);
        complectNodeList.SaveData(rel);
        if (start != -1)
          rel.SetAttributesValues(new AttributeValues[1]
          {
            new AttributeValues(MRP2Consts.attrIdFromComplect, (object) start)
          });
        if (end == -1)
          return;
        rel.SetAttributesValues(new AttributeValues[1]
        {
          new AttributeValues(MRP2Consts.attrIdToComplect, (object) end)
        });
      }
    }
  }
}
