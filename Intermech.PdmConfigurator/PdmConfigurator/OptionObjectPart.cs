// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.OptionObjectPart
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Queries;
using System;

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class OptionObjectPart : ObjectsPart
{
  private long _optionID;

  public OptionObjectPart(long optionID, IServiceProvider service)
    : base(service)
  {
    this._optionID = optionID;
  }

  protected override INodeQuery GetQuery(ConditionStructure[] conditions)
  {
    return (INodeQuery) new OptionQuery(this._optionID, (INodeQuerySupport) this);
  }
}
