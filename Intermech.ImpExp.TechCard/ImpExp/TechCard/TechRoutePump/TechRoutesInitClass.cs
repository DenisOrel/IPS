// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechRoutePump.TechRoutesInitClass
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.TechCard.Pumpers.Data.TechRoutePump.TechRoute_Links.TechRouteElem;
using Intermech.ImpExp.TechCard.TechRoutePump.TechRoute_Links.Routes2Zag;
using Intermech.ImpExp.TechCard.TechRoutePump.TechRouteElem;
using Intermech.ImpExp.TechCard.TechRoutePump.TechRoutes;
using Intermech.ImpExp.TechCard.TechRoutePump.TechRouteTemplate;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechRoutePump;

[TaskDescription("Подготовка к закачке расцеховочных объектов", "Закачка расцеховочных объектов")]
internal class TechRoutesInitClass : PumpClass
{
  private readonly Guid _guid = new Guid("{0A9DA8C2-812F-41cb-AD04-2D6D8BFAEF32}");
  private readonly TechRouteElemTemplatePump _techRouteElemTemplatePump;
  private readonly TechRouteElemPump _techRouteElemPump;
  private readonly TechRouteTemplatePump _techRouteTemplatePump;
  private readonly TechRoutesPump _techRoutesPump;
  private readonly TechRoute2ZagPump _linkRoute2ZagPump;

  public TechRoutesInitClass(PluginClass plugin)
    : base(plugin)
  {
    this._techRouteElemTemplatePump = new TechRouteElemTemplatePump(plugin);
    this._techRouteElemPump = new TechRouteElemPump(plugin);
    this._techRoutesPump = new TechRoutesPump(plugin);
    this._techRouteTemplatePump = new TechRouteTemplatePump(plugin);
    this._linkRoute2ZagPump = new TechRoute2ZagPump(plugin);
  }

  public void Load(List<IPumpTask> verificationsList, List<IPumpTask> pumpsList)
  {
    verificationsList.Add(this._techRouteTemplatePump.TaskExam);
    verificationsList.Add(this._techRouteElemTemplatePump.TaskExam);
    verificationsList.Add(this._techRoutesPump.TaskExam);
    verificationsList.Add(this._techRouteElemPump.TaskExam);
    pumpsList.Add(this._techRouteTemplatePump.TaskPump);
    pumpsList.Add(this._techRouteElemTemplatePump.TaskPump);
    pumpsList.Add(this._techRoutesPump.TaskPump);
    pumpsList.Add(this._techRouteElemPump.TaskPump);
    verificationsList.Add(this._linkRoute2ZagPump.TaskExam);
    pumpsList.Add(this._linkRoute2ZagPump.TaskPump);
  }

  protected override Guid GUID => this._guid;

  public override void Exam()
  {
  }

  public override void Pump()
  {
  }
}
