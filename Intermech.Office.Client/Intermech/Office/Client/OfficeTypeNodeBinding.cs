// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.OfficeTypeNodeBinding
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjectTypes;
using Intermech.Navigator.Selections;

#nullable disable
namespace Intermech.Office.Client;

internal class OfficeTypeNodeBinding(int objTypeID, BindingType bindingType) : ObjectTypeBinding(objTypeID, bindingType)
{
  public override ConditionStructure[] GetConditions(long selObjectID)
  {
    ConditionStructure[] conditions = base.GetConditions(selObjectID);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ConditionStructure[] privateCondition = OfficeTypeNode.GetPrivateCondition(sessionKeeper.Session);
      if (privateCondition == null)
        return conditions;
      if (conditions == null || conditions.Length == 0)
        return privateCondition;
      if (privateCondition.Length > 1)
      {
        ++privateCondition[0].GroupID;
        --privateCondition[privateCondition.Length - 1].GroupID;
      }
      return ConditionStructure.Join(privateCondition, conditions);
    }
  }
}
