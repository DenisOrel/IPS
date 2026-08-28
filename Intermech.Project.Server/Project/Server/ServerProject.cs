// Decompiled with JetBrains decompiler
// Type: Intermech.Project.Server.ServerProject
// Assembly: Intermech.Project.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EACE0DC6-7C3C-4F4A-987F-957BA13EA507
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Project.Server.dll

#nullable disable
namespace Intermech.Project.Server;

internal class ServerProject(long objectID, bool autoLoadSubTasks, bool autoLoadSubProjects) : 
  Intermech.Project.Project(objectID, autoLoadSubTasks, autoLoadSubProjects)
{
  internal new void DeleteNotifications() => base.DeleteNotifications();

  protected override void CheckEditRights()
  {
    if (this.RemoteSiteCode != ' ')
      return;
    base.CheckEditRights();
  }
}
