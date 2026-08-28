// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.DBPublishObject
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using System;
using System.Data;

#nullable disable
namespace Intermech.Portal.Server;

internal class DBPublishObject(UserSession uSession, DataTable objectParams) : DBObject(uSession, objectParams)
{
  protected override void AfterSetLCStep()
  {
    if (this.LevelID == this.UserSession.IdentHelper.DeletedID)
    {
      try
      {
        IDBAttribute attributeByGuid = this.GetAttributeByGuid(PortalConsts.attributeLinkedGuid);
        if (attributeByGuid != null)
        {
          if (!attributeByGuid.IsNull)
            attributeByGuid.Clear();
        }
      }
      catch (Exception ex)
      {
        throw new Exception($"Ошибка очистки значения атрибута \"Глобальный идентификатор связанного объекта\" при удалении объекта: {ex.Message}", ex);
      }
      if (this.Session.GetAllObjectVersionsList(this.ID, true, false, false).Count > 0)
        this.CheckEntersInRelations();
    }
    base.AfterSetLCStep();
  }

  private void CheckEntersInRelations()
  {
    DataTable dataTable = this.UserSession.GetRelationCollection(MetaDataHelper.GetRelationTypeID(PortalConsts.reltypePublish)).EntersIn(new DBRecordSetParams((ConditionStructure[]) null, new object[1]
    {
      (object) -20
    }), this.ID);
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      IDBRelation relation = this.UserSession.GetRelation(Convert.ToInt64(dataTable.Rows[index][0]));
      if ((relation as DBPublishRelation).GetPartGuidFromFile(false).Equals(new Guid(this.GetAttributeByGuid(PortalConsts.attributePublishObjectGUID).AsString)))
        throw new Exception($"Нельзя удалить объект {this.NameInMessages} так как он указан в файле attributes.xml в связи с объектом {relation.ProjID}");
    }
  }
}
