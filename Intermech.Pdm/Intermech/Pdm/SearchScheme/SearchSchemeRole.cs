// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SearchScheme.SearchSchemeRole
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.SearchScheme;

internal sealed class SearchSchemeRole
{
  public Guid RoleGuid = Guid.Empty;
  public Icon RoleIcon;
  public long RoleID;
  public bool ValidRole;
  public string RoleName = string.Empty;

  public SearchSchemeRole(string Guid, IUserSession session)
  {
    if (!GuidHelper.IsGuid(Guid))
      return;
    this.Initialize(session.GetObject(new Guid(Guid), false), session);
  }

  public SearchSchemeRole(long id, IUserSession session)
  {
    this.Initialize(session.GetObject(id, false), session);
  }

  private void Initialize(IDBObject dbRole, IUserSession session)
  {
    if (dbRole == null || dbRole.ObjectType != session.IdentHelper.RolesTypeID)
      return;
    this.RoleGuid = dbRole.ObjectGUID;
    this.RoleID = dbRole.ObjectID;
    this.RoleName = dbRole.Caption;
    ICategoryTypeIconService service = (ICategoryTypeIconService) ServicesManager.GetService(typeof (ICategoryTypeIconService));
    if (service != null)
      this.RoleIcon = service.GetIcon(4, session.IdentHelper.RolesTypeID);
    this.ValidRole = true;
  }

  public override string ToString()
  {
    return this.RoleName != string.Empty ? this.RoleName : string.Format(LocalizationHolder.rm.GetString("Pdm_70"), (object) this.RoleID);
  }
}
