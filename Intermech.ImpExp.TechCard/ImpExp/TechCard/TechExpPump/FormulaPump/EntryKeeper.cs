// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.EntryKeeper
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

internal class EntryKeeper
{
  private readonly List<EntryHead> _headList;
  private readonly List<EntryMember> _memberList;

  public EntryKeeper(List<EntryHead> headList, List<EntryMember> memberList)
  {
    this._headList = headList;
    this._memberList = memberList;
  }

  public List<EntryHead> HeadList => this._headList;

  public List<EntryMember> MemberList => this._memberList;

  public EntryHead GetHeadByName(string name)
  {
    if (name == string.Empty)
      return (EntryHead) null;
    foreach (EntryHead head in this._headList)
    {
      if (head != null && head.Name == name)
        return head;
    }
    return (EntryHead) null;
  }
}
