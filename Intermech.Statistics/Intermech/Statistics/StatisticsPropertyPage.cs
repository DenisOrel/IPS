// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.StatisticsPropertyPage
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace Intermech.Statistics;

public class StatisticsPropertyPage : IPropertyPage, IPropertyPageSearchOptionEvents
{
  private bool _showAllStatisticsObjects;
  private object _wrapper;

  [DisplayName("Видеть все задачи статистики")]
  [Description("Будут ли отображаться в окне статистики все объекты статистики или только созданные текущим пользователем")]
  [DefaultValue(false)]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  public bool CanShowAllStatisticsObjects
  {
    [DebuggerStepThrough] get => this._showAllStatisticsObjects;
    set => this._showAllStatisticsObjects = value;
  }

  public StatisticsPropertyPage(IUserSession session)
  {
    this._wrapper = (object) new ClassWrapperForPropertyGrid((object) this);
    this.Load(session);
  }

  private void Load(IUserSession session)
  {
    this.CanShowAllStatisticsObjects = session.Configurations.ReadBool(StatisticsConst.ModuleName, StatisticsConst.SETTINGS, StatisticsConst.CANSHOWALLOBJECTS, false, DBConfigMode.UserOnly);
  }

  public event EventHandler Changed;

  private void OnChanged()
  {
    EventHandler changed = this.Changed;
    if (changed == null)
      return;
    changed((object) this, new EventArgs());
  }

  [Browsable(false)]
  public PropertyPageType Type
  {
    [DebuggerStepThrough] get => PropertyPageType.Object;
  }

  [Browsable(false)]
  public object Control
  {
    [DebuggerStepThrough] get => this._wrapper;
  }

  [Browsable(false)]
  public string PageName
  {
    [DebuggerStepThrough] get => "Статистика";
  }

  public void Apply()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      sessionKeeper.Session.Configurations.WriteBool(StatisticsConst.ModuleName, StatisticsConst.SETTINGS, StatisticsConst.CANSHOWALLOBJECTS, this._showAllStatisticsObjects, sessionKeeper.Session.UserID);
  }

  public void Cancel()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      this.Load(sessionKeeper.Session);
  }

  [Browsable(false)]
  public string HelpTopicID => "0";

  [Browsable(false)]
  public string HeaderText
  {
    [DebuggerStepThrough] get => this.PageName;
  }

  public List<string> GetOptionNames()
  {
    return !(this.Control is ClassWrapperForPropertyGrid control) ? new List<string>() : IPropertyPageHelper.GetOptionNames((ICustomTypeDescriptor) control);
  }
}
