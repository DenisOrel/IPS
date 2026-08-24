// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.ActivityList
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Workflow;

internal class ActivityList : List<ActInfo>
{
  public ActInfo FindByOldID(int id)
  {
    if (id < this.Count && this[id].ID == id)
      return this[id];
    foreach (ActInfo byOldId in (List<ActInfo>) this)
    {
      if (byOldId.ID == id)
        return byOldId;
    }
    return (ActInfo) null;
  }

  public int IndexByOldID(int oldID)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].ID == oldID)
        return index;
    }
    return -1;
  }
}
