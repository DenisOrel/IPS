// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Configurations.CustomNodeFilter
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Statistics.Configurations;

internal class CustomNodeFilter : INodeSelectorFilter
{
  private List<int> _availableTypes;

  public CustomNodeFilter(List<int> availableTypes) => this._availableTypes = availableTypes;

  public bool CanSelectNode(int category, object id, out string errorMessage)
  {
    errorMessage = string.Empty;
    if (category != 4)
      return false;
    if (this._availableTypes.Contains(Convert.ToInt32(id)))
    {
      errorMessage = string.Empty;
      return true;
    }
    errorMessage = "Один из выделенных типов не содержит необходимого атрибута.";
    return false;
  }
}
