// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.ProjectPublisher
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Services.PortalServices;
using Intermech.Metadata;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Project.Server;

internal sealed class ProjectPublisher(
  [NotNull] CustomPublishDataInfo taskInfo,
  [NotNull] PublishComposition composition,
  [NotNull] ExtendedPublishOptions options) : AutoTransferPublisher(taskInfo, composition, options)
{
  protected override void BeforeCompositionPack(
    [NotNull] IUserSession session,
    [NotNull] SiteInfo siteInfo,
    [NotNull] IBackupWriter writer,
    [NotNull, ItemNotNull] List<ITransferedObject> transObjs)
  {
    base.BeforeCompositionPack(session, siteInfo, writer, transObjs);
    transObjs.Add((ITransferedObject) ProjectXmlFileFormer.Pack(this.info, session, writer, this.info.Data));
  }

  [NotNull]
  public override ITask GetExportTask(
    [NotNull] IUserSession session,
    long userID,
    [NotNull] string taskName,
    Guid userGuid,
    TaskPriority priority,
    [NotNull] ITransferedObject[] units,
    [CanBeNull] IDBAttribute attributeTaskFiles)
  {
    return (ITask) new AutoTransferPublishTask(userID, userGuid, taskName, TaskType.ProjectPublish, TaskPriority.Normal, units, this.composition.Objects, this.options, (Packet4Publish) null, this.recordedCodes, attributeTaskFiles);
  }

  protected override void WriteCodes([NotNull] ObjectTag tag, char currentSiteCode, [NotNull] IDBObject obj)
  {
    char? ownerSite = this.info.Options.OwnerSite;
    char? compositionOwnerSite = this.info.Options.CompositionOwnerSite;
    try
    {
      int objectType = obj.ObjectType;
      bool flag = false;
      if (objectType == (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Task || objectType == (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Dependency)
      {
        if (compositionOwnerSite.HasValue)
          this.info.Options.OwnerSite = this.info.Options.CompositionOwnerSite;
        else
          flag = true;
      }
      else if (objectType != (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project)
        flag = true;
      if (flag)
      {
        this.info.Options.OwnerSite = new char?();
        this.info.Options.CompositionOwnerSite = new char?();
      }
      base.WriteCodes(tag, currentSiteCode, obj);
    }
    finally
    {
      this.info.Options.OwnerSite = ownerSite;
      this.info.Options.CompositionOwnerSite = compositionOwnerSite;
    }
  }
}
