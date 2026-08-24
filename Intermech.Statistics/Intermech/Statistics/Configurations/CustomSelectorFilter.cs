// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Configurations.CustomSelectorFilter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Statistics.Configurations;

internal class CustomSelectorFilter : ISelectorFilter
{
  private List<int> _availableTypes;
  private List<int> _parentTypes;

  public CustomSelectorFilter(List<int> availableTypes, List<int> parentTypes)
  {
    this._availableTypes = availableTypes;
    this._parentTypes = parentTypes;
  }

  public bool IsInFilter(int category, object id)
  {
    int int32 = Convert.ToInt32(id);
    if (category != 4)
      return false;
    foreach (int availableType in this._availableTypes)
    {
      if (this._availableTypes.Contains(int32) || this._parentTypes.Contains(int32))
        return true;
    }
    return false;
  }
}
