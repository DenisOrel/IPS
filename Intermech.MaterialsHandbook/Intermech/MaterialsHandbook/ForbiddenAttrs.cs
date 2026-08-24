// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ForbiddenAttrs
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class ForbiddenAttrs : ISelectorFilter
{
  internal List<int> AttrsIDs { get; set; }

  internal ForbiddenAttrs(List<int> attrsIDs) => this.AttrsIDs = attrsIDs;

  public bool IsInFilter(int category, object id) => this.AttrsIDs.Contains(Convert.ToInt32(id));
}
