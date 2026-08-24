// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PortalNavigator.ObjectsListConditionsProvider
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Kernel.Search;
using Intermech.Navigator.DB;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Site.Client.PortalNavigator;

internal class ObjectsListConditionsProvider : IConditionsProvider
{
  private List<long> _objectIDs;

  public ObjectsListConditionsProvider(List<long> objectIDs) => this._objectIDs = objectIDs;

  public ConditionStructure[] GetConditions()
  {
    return new ConditionStructure[1]
    {
      new ConditionStructure(-2, RelationalOperators.In, (object) this._objectIDs.ToArray(), LogicalOperators.AND, 0, false)
    };
  }

  public bool ConditionsChanged => false;
}
