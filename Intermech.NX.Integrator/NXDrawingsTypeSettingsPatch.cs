// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.NXDrawingsTypeSettingsPatch
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.DBPatches;
using Intermech.IO;
using System;

#nullable disable
namespace Intermech.NX.Integrator;

internal sealed class NXDrawingsTypeSettingsPatch : AbstractPatch
{
  private static readonly Guid nxDrawingsTypeGuid = new Guid("CAD0090D-306C-11D8-B4E9-00304F19F545");

  protected override void DoPatch()
  {
    base.DoPatch();
    if (!(ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole).IsAdmin)
      return;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.FixNXDrawingsTypeSettings(sessionKeeper.Session);
  }

  private void FixNXDrawingsTypeSettings(IUserSession session)
  {
    IDBObjectType objectType = session.GetObjectType(NXDrawingsTypeSettingsPatch.nxDrawingsTypeGuid);
    IDocumentTypeSettingsService service = ServiceUtils.GetService<IDocumentTypeSettingsService>((object) session, true);
    DocumentTypeSettings settings = service.GetSettings(session.SessionGUID, objectType.ObjectType);
    bool flag = false;
    if (PathUtils.IsSamePath(settings.DocumentFileExt, ".ipt"))
    {
      settings.DocumentFileExt = ".prt";
      flag = ((flag ? 1 : 0) | 1) != 0;
    }
    if (!flag)
      return;
    service.SetSettings(session.SessionGUID, objectType.ObjectType, settings);
  }
}
