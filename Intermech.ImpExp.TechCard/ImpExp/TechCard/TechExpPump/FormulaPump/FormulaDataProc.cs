// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechExpPump.FormulaPump.FormulaDataProc
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.Common;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;

internal class FormulaDataProc
{
  private bool GetVersion(BinaryReader reader, out int version)
  {
    if (reader == null)
    {
      version = 0;
      return false;
    }
    reader.BaseStream.Position = 0L;
    FormulaHeader formulaHeader = new FormulaHeader();
    if (!formulaHeader.Load(reader) || formulaHeader.Key != "TCFF")
    {
      version = 0;
      return false;
    }
    version = formulaHeader.Version;
    return true;
  }

  private bool ReaderGroup_V103(
    int ind,
    short level,
    ref int ocnt,
    FormulaHeader header,
    BinaryReader reader,
    List<EntryMember> levelIdList,
    List<EntryMember> memberList)
  {
    short index = level;
    while ((int) index >= (int) level && ocnt < header.OlItems)
    {
      index = reader.ReadInt16();
      EntryMemberType entryMemberType = (EntryMemberType) reader.ReadByte();
      EntryMember entryMember1 = new EntryMember()
      {
        MemberType = entryMemberType
      };
      if (entryMemberType == EntryMemberType.GroupWithCond)
      {
        if (reader.ReadInt32() != 560685328)
          return false;
        entryMember1.Load(reader);
      }
      string str = TechExpert.Utils.TechReadByteString(reader);
      ++ocnt;
      if ((int) index > (int) level)
      {
        int num = 0;
        EntryMember entryMember2 = (EntryMember) null;
        EntryMember member1 = memberList[ind];
        if (member1 != null)
        {
          foreach (EntryMember member2 in memberList)
          {
            if (member2 != null && member2.Owner == member1 && member2.MemberNo >= num)
            {
              num = member2.MemberNo;
              entryMember2 = member2;
            }
          }
        }
        entryMember1.Code = str;
        entryMember1.Owner = levelIdList[(int) index - 1];
        entryMember1.MemberNo = memberList.Count;
        memberList.Add(entryMember1);
        levelIdList[(int) index] = entryMember1;
        if (!this.ReaderGroup_V103((entryMember2 == null ? -1 : entryMember2.MemberNo) + 1, (short) ((int) level + 1), ref ocnt, header, reader, levelIdList, memberList))
          return false;
      }
      if ((int) index == (int) level)
      {
        entryMember1.Code = str;
        entryMember1.Owner = levelIdList[(int) index - 1];
        entryMember1.MemberNo = memberList.Count;
        memberList.Add(entryMember1);
        levelIdList[(int) index] = entryMember1;
      }
      if ((int) index < (int) level)
      {
        entryMember1.Code = str;
        entryMember1.Owner = levelIdList[(int) index - 1];
        entryMember1.MemberNo = memberList.Count;
        memberList.Add(entryMember1);
        levelIdList[(int) index] = entryMember1;
      }
    }
    return true;
  }

  private bool ReaderGroup_V104(
    FormulaHeader header,
    BinaryReader reader,
    List<EntryMember> levelIdList,
    List<EntryMember> memberList)
  {
    for (int index1 = 0; index1 < header.OlItems; ++index1)
    {
      short index2 = reader.ReadInt16();
      EntryMemberType entryMemberType = (EntryMemberType) reader.ReadByte();
      EntryMember entryMember1 = new EntryMember()
      {
        MemberType = entryMemberType
      };
      if (entryMemberType == EntryMemberType.GroupWithCond)
      {
        if (reader.ReadInt32() != 560685328)
          return false;
        entryMember1.Load(reader);
      }
      string str = TechExpert.Utils.TechReadByteString(reader);
      EntryMember entryMember2 = (EntryMember) null;
      if (index2 != (short) 0)
        entryMember2 = levelIdList[(int) index2 - 1];
      entryMember1.Code = str;
      entryMember1.Owner = entryMember2;
      entryMember1.MemberNo = memberList.Count;
      memberList.Add(entryMember1);
      levelIdList[(int) index2] = entryMember1;
    }
    return true;
  }

  private bool ReaderGroup_v102(
    int ind,
    short level,
    ref int ocnt,
    FormulaHeader header,
    BinaryReader reader,
    List<EntryMember> levelIdList,
    List<EntryMember> memberList)
  {
    bool flag = this.ReaderGroup_V103(ind, level, ref ocnt, header, reader, levelIdList, memberList);
    if (flag)
    {
      foreach (EntryMember member in memberList)
      {
        if (member != null)
        {
          member.Conditions.ProcessRs();
          member.CompiledConditions.ProcessRS_CMP();
        }
      }
    }
    return flag;
  }

  private bool Open_FormulaData_v102(
    BinaryReader reader,
    out List<EntryHead> entryList,
    out List<EntryMember> memberList)
  {
    entryList = new List<EntryHead>();
    memberList = new List<EntryMember>();
    if (reader == null)
      return false;
    List<EntryMember> levelIdList = new List<EntryMember>();
    for (int index = 1; index <= 256 /*0x0100*/; ++index)
      levelIdList.Add((EntryMember) null);
    reader.BaseStream.Position = 0L;
    FormulaHeader header = new FormulaHeader();
    if (!header.Load(reader) || header.Key != "TCFF" || header.Version != 102)
      return false;
    int count = (int) header.HdSize - 40;
    if (count > 0)
      reader.ReadBytes(count);
    if (header.OlItems > 0)
    {
      int num1 = 0;
      short num2 = reader.ReadInt16();
      EntryMemberType entryMemberType = (EntryMemberType) reader.ReadByte();
      EntryMember entryMember = new EntryMember()
      {
        MemberType = entryMemberType
      };
      if (entryMemberType == EntryMemberType.GroupWithCond)
      {
        if (reader.ReadInt32() != 560685328)
          return false;
        entryMember.Load_v102(reader);
      }
      entryMember.Code = TechExpert.Utils.TechReadByteString(reader);
      int ocnt = num1 + 1;
      entryMember.MemberNo = memberList.Count;
      memberList.Add(entryMember);
      levelIdList[1] = entryMember;
      if (ocnt < header.OlItems && !this.ReaderGroup_v102(1, (short) ((int) num2 + 1), ref ocnt, header, reader, levelIdList, memberList))
        return false;
    }
    for (int index = 1; index <= header.EntryAm; ++index)
    {
      if (reader.ReadInt32() != 558579984)
        return false;
      EntryHead entryHead = new EntryHead();
      if (!entryHead.Load(reader))
        return false;
      if (entryHead.Name != string.Empty)
        entryList.Add(entryHead);
    }
    return true;
  }

  private bool Open_FormulaData_v103(
    BinaryReader reader,
    out List<EntryHead> entryList,
    out List<EntryMember> memberList)
  {
    entryList = new List<EntryHead>();
    memberList = new List<EntryMember>();
    if (reader == null)
      return false;
    List<EntryMember> levelIdList = new List<EntryMember>();
    for (int index = 1; index <= 256 /*0x0100*/; ++index)
      levelIdList.Add((EntryMember) null);
    reader.BaseStream.Position = 0L;
    FormulaHeader header = new FormulaHeader();
    if (!header.Load(reader) || header.Key != "TCFF" || header.Version != 103)
      return false;
    int count = (int) header.HdSize - 40;
    if (count > 0)
      reader.ReadBytes(count);
    if (header.OlItems > 0)
    {
      int num1 = 0;
      short num2 = reader.ReadInt16();
      EntryMemberType entryMemberType = (EntryMemberType) reader.ReadByte();
      EntryMember entryMember = new EntryMember()
      {
        MemberType = entryMemberType
      };
      if (entryMemberType == EntryMemberType.GroupWithCond)
      {
        if (reader.ReadInt32() != 560685328)
          return false;
        entryMember.Load(reader);
      }
      entryMember.Code = TechExpert.Utils.TechReadByteString(reader);
      int ocnt = num1 + 1;
      entryMember.MemberNo = memberList.Count;
      memberList.Add(entryMember);
      levelIdList[1] = entryMember;
      if (ocnt < header.OlItems && !this.ReaderGroup_V103(1, (short) ((int) num2 + 1), ref ocnt, header, reader, levelIdList, memberList))
        return false;
    }
    for (int index = 1; index <= header.EntryAm; ++index)
    {
      if (reader.ReadInt32() != 558579984)
        return false;
      EntryHead entryHead = new EntryHead();
      if (!entryHead.Load(reader))
        return false;
      if (entryHead.Name != string.Empty)
        entryList.Add(entryHead);
    }
    return true;
  }

  private bool Open_FormulaData_v104(
    BinaryReader reader,
    out List<EntryHead> entryList,
    out List<EntryMember> memberList)
  {
    entryList = new List<EntryHead>();
    memberList = new List<EntryMember>();
    if (reader == null)
      return false;
    List<EntryMember> levelIdList = new List<EntryMember>();
    for (int index = 1; index <= 256 /*0x0100*/; ++index)
      levelIdList.Add((EntryMember) null);
    reader.BaseStream.Position = 0L;
    FormulaHeader header = new FormulaHeader();
    if (!header.Load(reader) || header.Key != "TCFF" || header.Version != 104)
      return false;
    int count = (int) header.HdSize - 40;
    if (count > 0)
      reader.ReadBytes(count);
    if (header.OlItems > 0 && !this.ReaderGroup_V104(header, reader, levelIdList, memberList))
      return false;
    for (int index = 1; index <= header.EntryAm; ++index)
    {
      if (reader.ReadInt32() != 558579984)
        return false;
      EntryHead entryHead = new EntryHead();
      if (!entryHead.Load(reader))
        return false;
      if (entryHead.Name != string.Empty)
        entryList.Add(entryHead);
    }
    return true;
  }

  public bool Open_FormulaData(
    BinaryReader reader,
    out List<EntryHead> entryList,
    out List<EntryMember> maxLevInd)
  {
    entryList = (List<EntryHead>) null;
    maxLevInd = (List<EntryMember>) null;
    int version;
    if (!this.GetVersion(reader, out version))
      return false;
    switch (version)
    {
      case 102:
        return this.Open_FormulaData_v102(reader, out entryList, out maxLevInd);
      case 103:
        return this.Open_FormulaData_v103(reader, out entryList, out maxLevInd);
      case 104:
        return this.Open_FormulaData_v104(reader, out entryList, out maxLevInd);
      default:
        throw new Exception($"Не известная версия файла {version}");
    }
  }
}
