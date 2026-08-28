// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.ProjectTimersService
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Server;
using Intermech.Kernel;
using System;

#nullable disable
namespace Intermech.Project.Server;

internal class ProjectTimersService : DBTimedService
{
  [NotNull]
  [NullBefore("Register")]
  public static ProjectTimersService Self { get; private set; }

  public static void Register()
  {
    ProjectTimersService.Self = new ProjectTimersService();
    (ApplicationServices.Container.GetService<IDBTimedEvents>() as DBTimedEvents).RegisterService((object) ProjectTimersService.Self);
    ApplicationServices.Container.GetService<ICustomServices>().AddService(typeof (IProjectTimers), (object) new ProjectTimers());
  }

  public override Guid GUID => ProjectTimers.Guid;

  public override bool ProcessEvent(TimedEventProperties properties)
  {
    return properties.IntInfo != 0 || this.CreateMessage(properties);
  }

  [NotNull]
  public override string ServiceName => "ProjectTimers";

  internal bool CreateMessage(TimedEventProperties p)
  {
    IUserSession sessionTemporaryClone = this.TimedEventService.GetSystemSessionTemporaryClone("ProjectServer.CreateMessage");
    try
    {
      Task task = StandaloneTask.Get(sessionTemporaryClone, p.ObjectID);
      task.ProjectNeeded();
      if (task.Project.IsExecuted)
        task.SendOverdueNotification();
    }
    finally
    {
      sessionTemporaryClone?.Logout("ProjectServer.CreateMessage");
    }
    return true;
  }
}
