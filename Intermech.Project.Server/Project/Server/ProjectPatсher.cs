// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.ProjectPatсher
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.LifeCycles;
using Intermech.Interfaces.MetadataUpdates;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Kernel.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

#nullable disable
namespace Intermech.Project.Server;

[Serializable]
internal class ProjectPatсher : LongLifeObject, IUpdatable, IProjectServer
{
  private const string ModuleName = "PROJECT";
  [NotNull]
  private static readonly string[] _updateScripts = new string[3]
  {
    "Intermech.Project.xml",
    "Intermech.Project.AdvImport.xml",
    "Intermech.Project.PrintSchemes.xml"
  };
  private const int PluginDbVersion = 43;
  private const int PluginDbRevision = 0;

  [NotNull]
  public string[] GetUpdateScripts() => ProjectPatсher._updateScripts;

  public void BeforeExecScript([NotNull] IUserSession session, [NotNull] string scriptName)
  {
    if (!string.Equals(scriptName, ((IEnumerable<string>) this.GetUpdateScripts()).FirstOrDefault<string>(), StringComparison.Ordinal))
      return;
    session.CheckMaximumPluginDbVersion("PROJECT", 43, 0);
  }

  public void AfterExecScript([NotNull] IUserSession session, [NotNull] string scriptName)
  {
  }

  public void AfterExecAllScripts([NotNull] IUserSession session)
  {
    UserSession userSession = Intermech.Diagnostics.Check.Is<UserSession>((object) session, nameof (session));
    MetadataLoader.Init(session);
    IPublishTypesConfiguration service1 = ApplicationServices.Container.GetService<IPublishTypesConfiguration>(false);
    if (service1 != null)
    {
      int attributeId = session.GetAttributeType(PortalConsts.attributePublishObjTypeGuid).AttributeID;
      Guid guid = new Guid("cad00e91-306c-11d8-b4e9-00304f19f545");
      int objectType = session.GetObjectType(guid).ObjectType;
      if (!service1.IsPublishObjectType(objectType))
      {
        service1.AddPublishObjectType(objectType, true);
        IContainerService service2 = ApplicationServices.Container.GetService<IContainerService>();
        ProjectPatсher.SetPublishTypeAttribute(session, service2, attributeId, guid, PortalConsts.objtypePublishObjects);
        ProjectPatсher.SetPublishTypeAttribute(session, service2, attributeId, Intermech.Project.ObjectTypes.Dependency.Guid, PortalConsts.objtypePublishObjects);
        ProjectPatсher.SetPublishTypeAttribute(session, service2, attributeId, Intermech.Project.ObjectTypes.Task.Guid, new Guid("cadd922e-306c-11d8-b4e9-00304f19f545"));
        ProjectPatсher.SetPublishTypeAttribute(session, service2, attributeId, Intermech.Project.ObjectTypes.Project.Guid, new Guid("cadd922f-306c-11d8-b4e9-00304f19f545"));
      }
    }
    int projectsTypeID = session.GetObjectType(Intermech.Project.ObjectTypes.Project.Guid).ObjectType;
    int objectType1 = session.GetObjectType(Intermech.Project.ObjectTypes.Task.Guid).ObjectType;
    int relationType1 = session.GetRelationType(new Guid("cadd95aa-306c-11d8-b4e9-00304f19f545")).RelationType;
    int relationType2 = session.GetRelationType(new Guid("cad00e93-306c-11d8-b4e9-00304f19f545")).RelationType;
    int attributeId1 = session.GetAttributeType(new Guid("cadd95a7-306c-11d8-b4e9-00304f19f545")).AttributeID;
    IDBVersionUpdater service3 = ApplicationServices.Container.GetService<IDBVersionUpdater>();
    int num1 = 42;
    if (service3.IsNeedUpdateModule(userSession.DataManager, userSession.EventLogHelper, "PROJECT", "PROJECT", num1))
    {
      using (userSession.UpdateDbVersion("PROJECT", false, num1))
      {
        DataTable dataTable1 = session.GetObjectCollection(projectsTypeID).Select(DB.Column(in DB.ObjectAttr.VersionID));
        IReadOnlyCollection<long> list = dataTable1 == null || dataTable1.Rows.Count == 0 ? (IReadOnlyCollection<long>) null : (IReadOnlyCollection<long>) dataTable1.Select<long>((System.Func<DataRow, long>) (row => row.FieldAsLong(0))).ToList<long>(dataTable1.Rows.Count);
        if (list != null)
        {
          if (list.Count > 0)
          {
            ApplicationServices.Container.GetService<IVersionRulesCacheService>().Load((object) session);
            DataTable dataTable2 = session.GetRelationCollection(relationType1).Select((ColumnDescriptor[]) DB.ColumnsWithSorting(DB.RelationAttr.ProjID, DB.ObjectAttr.ID, DB.RelationAttr.PrjLinkID).OrderBy(in DB.RelationAttr.ProjID, SortOrders.ASC), DB.Condition(DB.RelationAttr.ProjID, DB.In(list)));
            if (dataTable2 != null)
            {
              if (dataTable2.Rows.Count > 0)
              {
                session.GetRelationCollection(relationType2).LocalTypesMode = true;
                long[] array = dataTable2.Select<long>((System.Func<DataRow, long>) (row => row.FieldAsLong(0))).ToArray<long>(dataTable2.Rows.Count);
                DataTable dataTable3 = ApplicationServices.Container.GetService<CompositionLoadService>().LoadComplexCompositions((object) session, list.Select<long, ObjInfoItem>((System.Func<long, ObjInfoItem>) (projectID => new ObjInfoItem(projectID, projectsTypeID))), Enumeration.Create<int>(relationType2), Enumeration.Create<int>(objectType1), (IEnumerable<ColumnDescriptor>) DB.ColumnsWithSorting(DB.RelationAttr.ProjID, DB.ObjectAttribute(attributeId1, ColumnContents.ID)).OrderBy(in DB.RelationAttr.ProjID, SortOrders.ASC), true, false, (VersionsRule) null, (IEnumerable<ConditionStructure>) DB.Conditions((DB.RelationAttr.ProjID, DB.In(array)), (DB.ObjectAttribute(attributeId1), DB.NotEmpty), (DB.ObjectAttribute(attributeId1), DB.NotEqualTo((object) 0L))), "cad001df-306c-11d8-b4e9-00304f19f545", (Dictionary<long, HybridDictionary>) null, 1);
                if (dataTable3 != null)
                {
                  if (dataTable3.Rows.Count > 0)
                  {
                    Dictionary<(long, long), long> dictionary = ((IEnumerable<(long, long)>) dataTable3.Rows.Select<(long, long)>((System.Func<DataRow, (long, long)>) (row => (Convert.ToInt64(row[0]), Convert.ToInt64(row[1])))).Distinct<(long, long)>().ToArray<(long, long)>(dataTable3.Rows.Count)).ToDictionary<(long, long), (long, long), long>((System.Func<(long, long), (long, long)>) (value => (value.ProjID, session.GetObjectInfo(value.ImportedObjectVerID).ID)), (System.Func<(long, long), long>) (value => value.ImportedObjectVerID));
                    if (!dictionary.IsEmpty<KeyValuePair<(long, long), long>>())
                    {
                      foreach (DataRow row in (InternalDataCollectionBase) dataTable2.Rows)
                      {
                        long num2 = row.FieldAsLong(0);
                        long num3 = row.FieldAsLong(1);
                        long num4;
                        if (dictionary.TryGetValue((num2, num3), out num4))
                        {
                          long aRelationID = row.FieldAsLong(2);
                          session.GetRelation(aRelationID).Attributes.AddAttribute(attributeId1, false, new object[1]
                          {
                            (object) num4
                          });
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
    int num5 = 43;
    if (service3.IsNeedUpdateModule(userSession.DataManager, userSession.EventLogHelper, "PROJECT", "PROJECT", num5))
    {
      using (userSession.UpdateDbVersion("PROJECT", false, num5))
      {
        Guid guid = new Guid("CAE0D226-F228-401F-BFF4-8395E19C05A8");
        IMSObjectType objectType2 = MetaDataHelperService.Instance.GetObjectType(guid);
        if (objectType2 != null)
        {
          IDBObjectType objectType3 = session.GetObjectType(guid);
          Intermech.Diagnostics.Check.NotNull<IDBObjectType>(objectType3, "dbObjectType");
          objectType3.ObjectTypeName = "[удалить] Сценарии обработки импортированных задач IMProject";
          objectType3.ObjectInstanceName = "[удалить] Сценарий обработки импортированных задач IMProject";
          objectType3.Note = "Тип объекта должен быть удалён вручную, у него некорректные параметры";
          DataTable dataTable = userSession.SelectObjects(objectType2.ObjectTypeID, (ColumnDescriptor[]) null, recordCount: 1);
          if (dataTable == null || dataTable.Rows.Count == 0)
          {
            IDeletable deletable = Intermech.Diagnostics.Check.Is<IDeletable>((object) objectType3);
            try
            {
              deletable.Delete(0L);
            }
            catch
            {
            }
          }
        }
        if (MetaDataHelperService.Instance.GetObjectType(Intermech.Project.ObjectTypes.ScriptInitAfterImportTasks.ID) == null)
        {
          IDBObjectTypeCollection objectTypeCollection = session.GetObjectTypeCollection(Intermech.Metadata.ObjectTypes.Scripts.ID);
          IDBLCSchemaCollection schemaCollection = session.GetLCSchemaCollection();
          ObjectTypeProperties typeProperties = new ObjectTypeProperties(-99999, "Сценарии обработки импортированных задач IMProject", "Сценарий обработки импортированных задач IMProject", string.Empty, ObjectVersionModes.SingleVersion, Intermech.Metadata.RelationTypes.Simple.ID, string.Empty, Intermech.Project.ObjectTypes.ScriptInitAfterImportTasks.Guid, 0, false, InheritModes.Inherited, string.Empty, 0, ObjectTypeOptions.LocalObjectType, schemaCollection.GetDefaultSchemaID());
          objectTypeCollection.Create(typeProperties);
        }
      }
    }
    session.CheckMaximumPluginDbVersion("PROJECT", 43, 0);
  }

  private static void SetPublishTypeAttribute(
    [NotNull] IUserSession session,
    [NotNull] IContainerService containerService,
    int attrPublishTypeID,
    Guid objTypeGuid,
    Guid publishTypeGuid)
  {
    containerService.GetContainerForObjectType((object) session, objTypeGuid, true).Attributes.AddAttribute(attrPublishTypeID, false).Value = (object) publishTypeGuid;
  }
}
