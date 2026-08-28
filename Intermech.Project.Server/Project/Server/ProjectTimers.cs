// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.ProjectTimers
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using System;

#nullable disable
namespace Intermech.Project.Server;

internal class ProjectTimers : LongLifeObject, IProjectTimers
{
  public static readonly Guid Guid = new Guid("{8D992130-FD79-4a67-874B-7162E03C847C}");

  public void Add(Guid sessionGuid, long objectID, DateTime date, ProjectTimerKind kind)
  {
    TimedEventProperties properties = new TimedEventProperties(0, date, DateTime.MinValue, ProjectTimers.Guid, objectID, 0L, string.Empty, (int) kind, 0);
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    if (sessionById == null)
      return;
    ApplicationServices.Container.GetService<IDBTimedEvents>().AddEvent(properties, ((UserSession) sessionById).DataManager);
  }

  public void Remove(Guid sessionGuid, long objectID, ProjectTimerKind kind)
  {
    IUserSession sessionById = UserSession.GetSessionByID(sessionGuid);
    if (sessionById == null)
      return;
    IDBTimedEvents service = ApplicationServices.Container.GetService<IDBTimedEvents>();
    int eventID = service.FindEvent(ProjectTimers.Guid, (int) kind, objectID, ((UserSession) sessionById).DataManager);
    if (eventID <= 0)
      return;
    service.DeleteEventID(eventID, ((UserSession) sessionById).DataManager);
  }
}
