// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignDBRelation
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using System.Data;

#nullable disable
namespace Intermech.Signs.Server;

public class SignDBRelation(UserSession uSession, DataTable relationsTable) : DBRelation(uSession, relationsTable)
{
  protected override int DoDelete(long DeleteMode)
  {
    int num = base.DoDelete(DeleteMode);
    IDBObject objectById;
    if (num == 0 && this.ProjID >= 0L && (objectById = this.UserSession.GetObjectByID(this.PartID, false)) != null && (!this.DontDeleteChildObjectMode || SiteIDHelper.IsForeign((ISitesCacheService) this.Session.GetCustomService(typeof (ISitesCacheService)), objectById.SiteID)))
      objectById.Delete(DeleteMode);
    return num;
  }
}
