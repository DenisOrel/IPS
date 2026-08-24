// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.RuleToObjectTypeSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Configuration;
using System;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal static class RuleToObjectTypeSettings
{
  private static string _propertyRuleID = "RuleID";
  private static string _propertyRecursive = "Recursive";

  public static void GetSettings(int objectType, out Guid? ruleID, out bool recursive)
  {
    IConfiguration configuration = RuleToObjectTypeSettings.GetConfiguration(objectType);
    if (configuration != null)
    {
      ruleID = new Guid?(new Guid(configuration.GetProperty(RuleToObjectTypeSettings._propertyRuleID)));
      recursive = Convert.ToBoolean(configuration.GetProperty(RuleToObjectTypeSettings._propertyRecursive));
    }
    else
    {
      ruleID = new Guid?();
      recursive = false;
    }
  }

  public static void SetSettings(int objectType, Guid ruleID, bool recursive)
  {
    IConfiguration configuration = RuleToObjectTypeSettings.GetConfiguration(objectType, true);
    configuration.SetProperty(RuleToObjectTypeSettings._propertyRuleID, Convert.ToString((object) ruleID));
    configuration.SetProperty(RuleToObjectTypeSettings._propertyRecursive, Convert.ToString(recursive));
  }

  private static IConfiguration GetConfiguration(int objectType, bool create = false)
  {
    IConfigurationManager service = ServiceUtils.GetService<IConfigurationManager>((object) ApplicationServices.Container, true);
    IConfiguration configuration = service.Open($"compareTree{objectType}");
    if (configuration == null & create)
      configuration = service.Create($"compareTree{objectType}");
    return configuration;
  }
}
