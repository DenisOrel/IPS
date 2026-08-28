// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.PortalHandler
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel.Services.PortalServices;
using Intermech.Metadata;
using Intermech.Workflow;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Project.Server;

public class PortalHandler
{
  [NotNull]
  private static readonly Dictionary<long, Dictionary<int, object>> _attrsCache = new Dictionary<long, Dictionary<int, object>>();

  private static int AttrLcStep => -4;

  public static void BeforeObjectRefreshEvent([CanBeNull] object sender, [NotNull] BeforeObjectRefreshEventArgs e)
  {
    if (e.DBObject.TypeID != (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project)
      return;
    IDBObject dbObject = e.DBObject;
    long objectId = dbObject.ObjectID;
    Dictionary<int, object> dictionary;
    if (!PortalHandler._attrsCache.TryGetValue(objectId, out dictionary))
    {
      dictionary = new Dictionary<int, object>();
      PortalHandler._attrsCache.Add(objectId, dictionary);
    }
    dictionary[PortalHandler.AttrLcStep] = (object) dbObject.LCStep;
    IDBAttribute attributeById = dbObject.GetAttributeByID((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.PercentCompleted);
    if (attributeById == null)
      return;
    dictionary[(int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.PercentCompleted] = (object) attributeById.AsDouble;
  }

  public static void ObjectImportedEvent([CanBeNull] object sender, [NotNull] ObjectImportedEventArgs e)
  {
    IDBObject importedObject = e.ImportedObject;
    if (importedObject.TypeID != (int) (IpsMetadataEntityBase<int>) Intermech.Project.ObjectTypes.Project)
      return;
    int num = 0;
    double prevValue = 0.0;
    Dictionary<int, object> dictionary;
    if (PortalHandler._attrsCache.TryGetValue(importedObject.ObjectID, out dictionary))
    {
      PortalHandler._attrsCache.Remove(importedObject.ObjectID);
      object obj;
      if (dictionary.TryGetValue(PortalHandler.AttrLcStep, out obj))
        num = Convert.ToInt32(obj);
      if (dictionary.TryGetValue((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.PercentCompleted, out obj))
        prevValue = Convert.ToDouble(obj);
      IDBAttribute attributeById = importedObject.GetAttributeByID((int) (IpsMetadataEntityBase<int>) Intermech.Project.Attributes.PercentCompleted);
      if (attributeById != null && !prevValue.Equals(attributeById.AsDouble))
        StandaloneTask.Get(importedObject.Session, importedObject.ObjectID).UpdateParentPercentCompleted(prevValue);
      if (num != importedObject.LCStep)
        ((DBProjectTask) importedObject).AfterSetLcStepInternal(true);
    }
    SiteID siteId = new SiteID(importedObject.SiteID);
    if (importedObject.LCStep == (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Designing && ((int) siteId.Owner != (int) siteId.CurrentSite && (int) siteId.CompositionOwner == (int) siteId.CurrentSite || (int) siteId.Owner == (int) siteId.CurrentSite && (int) siteId.CompositionOwner != (int) siteId.CurrentSite))
      importedObject.LCStep = (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Imported;
    else if (importedObject.LCStep == (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Imported && (int) siteId.Owner == (int) siteId.CurrentSite && (int) siteId.CompositionOwner == (int) siteId.CurrentSite && importedObject.LCStep == (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Imported)
      importedObject.LCStep = (int) (IpsMetadataEntityBase<int>) Intermech.Project.LCStep.Designing;
    if (!(importedObject is DBProject dbProject))
      return;
    switch (dbProject.RemoteStatus)
    {
      case RemoteProcessStatus.WaitingForPublish:
      case RemoteProcessStatus.InProgress:
        dbProject.RemoteStatus = RemoteProcessStatus.None;
        break;
    }
  }

  public static void GetTaskByTypeEvent([CanBeNull] object sender, [NotNull] GetTaskByTypeEventArgs e)
  {
    if (e.Handled || e.Type != TaskType.ProjectPublish)
      return;
    e.Task = (ITask) new AutoTransferPublishTask(e.TaskObject.GetAttributeByGuid(PortalConsts.attributeTaskFiles));
    e.Handled = true;
  }
}
