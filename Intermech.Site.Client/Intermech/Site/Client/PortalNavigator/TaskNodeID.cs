// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.TaskNodeID
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

public class TaskNodeID : NodeID
{
  public TaskNodeID(
    CreateObjectNodeParams pars,
    TaskStatus status,
    TaskType taskType,
    bool enabled)
    : base(pars)
  {
    this.Type = taskType;
    this.Status = status;
    this.Enabled = enabled;
    this.pars = pars;
  }

  public TaskType Type { get; }

  public TaskStatus Status { get; }

  public bool Enabled { get; }
}
