// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.PortalImportedObjectsPump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Article;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Impl.Search.Document;
using Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal.Interfaces;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.DataCache.Portal;

[TaskDescription("Инициализация данных для перекачки - Загрузка кэшей объектов, импортированных через портал", "Перекачка данных - Импортированные через портал данные")]
internal class PortalImportedObjectsPump : PumpClass
{
  private void RegisterImportedObjects<T>(IUserSession session, IPortalImportedObjectCache<T> cache) where T : PortalImportedObject
  {
    if (cache.Objects.Count == 0 || !(ServicesManager.GetService(typeof (IMetadataInfo)) is IMetadataInfo service))
      return;
    IDBObjectCollection objectCollection = session.GetObjectCollection(cache.ObjectType);
    ColumnDescriptor[] columns = new ColumnDescriptor[5]
    {
      new ColumnDescriptor((object) -12, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -18, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -7, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -2, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0),
      new ColumnDescriptor((object) -3, AttributeSourceTypes.Object, ColumnContents.Text, ColumnNameMapping.Index, SortOrders.NONE, 0)
    };
    DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(-12, RelationalOperators.In, (object) cache.Objects.Select<T, Guid>((System.Func<T, Guid>) (obj => obj.IpsObjVerGuid)).ToArray<Guid>(), LogicalOperators.NONE, 0, false)
    }, columns);
    DataTable dataTable;
    try
    {
      dataTable = objectCollection.Select(paramSet);
    }
    catch
    {
      dataTable = (DataTable) null;
    }
    if (dataTable == null || dataTable.Rows.Count <= 0)
      return;
    Dictionary<Guid, List<T>> dictionary = new Dictionary<Guid, List<T>>();
    foreach (T obj in (IEnumerable<T>) cache.Objects)
    {
      List<T> objList;
      if (!dictionary.TryGetValue(obj.IpsObjVerGuid, out objList))
      {
        objList = new List<T>();
        dictionary[obj.IpsObjVerGuid] = objList;
      }
      objList.Add(obj);
    }
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
    {
      Guid guidValue1 = DataSetProcessor.GetGuidValue(row[0], Guid.Empty);
      Guid guidValue2 = DataSetProcessor.GetGuidValue(row[1], Guid.Empty);
      int int32Value = DataSetProcessor.GetInt32Value(row[2], -1);
      long int64Value1 = DataSetProcessor.GetInt64Value(row[3], 0L);
      long int64Value2 = DataSetProcessor.GetInt64Value(row[4], 0L);
      if (int64Value1 != 0L && !(guidValue1 == Guid.Empty))
      {
        if (service.ImportedObjects.GetInfo(int64Value1) == null)
          service.ImportedObjects.AddValue(int64Value1, int64Value2, int32Value, guidValue1, guidValue2);
        List<T> objList;
        if (dictionary.TryGetValue(guidValue1, out objList))
        {
          foreach (T obj in objList)
          {
            obj.IpsObjVerId = int64Value1;
            obj.IpsObjGuid = guidValue2;
            obj.IpsObjId = int64Value2;
          }
        }
      }
    }
  }

  protected override Guid GUID => new Guid("4A3E8D82-B21A-4A61-8934-DDCAFC9FBC32");

  public override void Exam()
  {
    base.Exam();
    IPortalSearchArticleVersionCache service1 = ApplicationServices.Container.GetService<IPortalSearchArticleVersionCache>();
    if (!service1.Loaded)
      service1.Load();
    this.RegisterImportedObjects<PortalSearchArticleVersion>(this.plugin.Idw.GetUserSession(), (IPortalImportedObjectCache<PortalSearchArticleVersion>) service1);
    IPortalSearchDocumentVersionCache service2 = ApplicationServices.Container.GetService<IPortalSearchDocumentVersionCache>();
    if (!service2.Loaded)
      service2.Load();
    this.RegisterImportedObjects<PortalSearchDocumentVersion>(this.plugin.Idw.GetUserSession(), (IPortalImportedObjectCache<PortalSearchDocumentVersion>) service2);
  }

  public PortalImportedObjectsPump(PluginClass plugin)
    : base(plugin)
  {
    this.taskExam.Repumpble = true;
  }

  public static void RegisterPortalServices()
  {
    ApplicationServices.Container.AddService(typeof (IPortalSearchDocumentVersionCache), (object) new PortalSearchDocumentVersionCache());
    ApplicationServices.Container.AddService(typeof (IPortalSearchArticleVersionCache), (object) new PortalSearchArticleVersionCache());
  }
}
