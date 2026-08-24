// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.TechParamList
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[Serializable]
public class TechParamList : List<ITechParamBase>
{
  public void AddOrUpdate(ITechParamBase item)
  {
    int index = this.IndexOf(item);
    if (index != -1)
      this[index] = item;
    else
      this.Add(item);
  }
}
