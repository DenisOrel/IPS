// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareSettings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal static class CompareSettings
{
  private static readonly string _relationTypes = "RelationTypes";
  private static readonly string _compareAttributes = "CompareAttributes";

  private static IConfiguration GetConfiguration(int objectType)
  {
    IConfigurationManager service = ServicesManager.GetService(typeof (IConfigurationManager)) as IConfigurationManager;
    string name = $"Compare_{objectType}";
    return service.Open(name) ?? service.Create(name);
  }

  public static void Read(
    IUserSession session,
    int objectType,
    out Dictionary<int, bool> relationTypes,
    out List<int> compareAttributes)
  {
    relationTypes = (Dictionary<int, bool>) null;
    compareAttributes = (List<int>) null;
    IConfiguration configuration = CompareSettings.GetConfiguration(objectType);
    string property1 = configuration.GetProperty(CompareSettings._relationTypes);
    if (!string.IsNullOrEmpty(property1))
    {
      string[] strArray = property1.Split(';');
      relationTypes = new Dictionary<int, bool>(strArray.Length);
      for (int index = 0; index < strArray.Length; ++index)
      {
        int int32 = Convert.ToInt32(strArray[index]);
        if (MetaDataHelper.ExistsRelationType(int32))
          relationTypes.Add(int32, true);
      }
    }
    string property2 = configuration.GetProperty(CompareSettings._compareAttributes);
    if (!string.IsNullOrEmpty(property2))
    {
      string[] strArray = property2.Split(';');
      compareAttributes = new List<int>(strArray.Length);
      for (int index = 0; index < strArray.Length; ++index)
      {
        int int32 = Convert.ToInt32(strArray[index]);
        if (MetaDataHelper.ExistsAttributeType(int32))
          compareAttributes.Add(int32);
      }
    }
    if (relationTypes == null || relationTypes.Count == 0)
    {
      int defaultRelationTypeId = MetaDataHelper.GetDefaultRelationTypeID(objectType);
      if (defaultRelationTypeId != -1)
        relationTypes = new Dictionary<int, bool>(1)
        {
          {
            defaultRelationTypeId,
            true
          }
        };
    }
    if (compareAttributes != null)
      return;
    compareAttributes = new List<int>(0);
  }

  public static void Write(
    IUserSession session,
    int objectType,
    Dictionary<int, bool> relationTypes,
    List<int> compareAttributes)
  {
    IConfiguration configuration = CompareSettings.GetConfiguration(objectType);
    StringBuilder stringBuilder1 = new StringBuilder();
    if (relationTypes.Count > 0)
    {
      bool flag = true;
      foreach (KeyValuePair<int, bool> relationType in relationTypes)
      {
        if (relationType.Value)
        {
          if (!flag)
            stringBuilder1.Append(';');
          else
            flag = false;
          stringBuilder1.Append(relationType.Key);
        }
      }
    }
    configuration.SetProperty(CompareSettings._relationTypes, stringBuilder1.ToString());
    StringBuilder stringBuilder2 = new StringBuilder();
    if (compareAttributes.Count > 0)
    {
      for (int index = 0; index < compareAttributes.Count; ++index)
      {
        if (index > 0)
          stringBuilder2.Append(';');
        stringBuilder2.Append(compareAttributes[index]);
      }
    }
    configuration.SetProperty(CompareSettings._compareAttributes, stringBuilder2.ToString());
  }
}
