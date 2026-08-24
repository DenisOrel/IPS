// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.OptionQuery
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Interfaces;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Queries;
using System;
using System.Data;

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class OptionQuery : ObjectsQuery
{
  private long _optionID;

  public OptionQuery(long optionID, INodeQuerySupport support)
    : base(support, Intermech.Interfaces.PdmConfigurator.Consts.objtypeOptionID, (ConditionStructure[]) null, (IServiceProvider) null)
  {
    this._optionID = optionID;
  }

  protected override DataTable GetDataTable(DBRecordSetParams queryParams)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
      return (sessionKeeper.Session.GetCustomService(typeof (IPdmConfiguratorService)) as IPdmConfiguratorService).GetDataTable(this._optionID, queryParams, sessionKeeper.Session.SessionGUID);
  }
}
