// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common.TechExpFolderObjectFactory
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;
using Intermech.ImpExp.TechCard.TechExpPump.TablesPump;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common;

internal class TechExpFolderObjectFactory
{
  private static readonly TechExpFolderObjectFactory _instance = new TechExpFolderObjectFactory();

  public TechExpFolderObject CreateFolderObject(TechExpKey key, EntryMember entryMember)
  {
    if (entryMember == null)
      throw new ArgumentNullException(nameof (entryMember));
    return new TechExpFolderObject(key, (object) entryMember)
    {
      Name = entryMember.Code,
      Condition = entryMember.Conditions != null ? new FormulaData((short) 3, (List<string>) entryMember, entryMember.Conditions) : (FormulaData) null
    };
  }

  public TechExpFolderObject CreateFolderObject(TechExpKey key, TableInfo tableInfo)
  {
    if (tableInfo == null)
      throw new ArgumentNullException(nameof (tableInfo));
    return new TechExpFolderObject(key, (object) tableInfo)
    {
      Name = string.IsNullOrEmpty(tableInfo.Code) ? tableInfo.Name : $"{tableInfo.Name}({tableInfo.Code})",
      Condition = tableInfo.Cond != null ? tableInfo.Cond : (FormulaData) null
    };
  }

  public static TechExpFolderObjectFactory Instance => TechExpFolderObjectFactory._instance;
}
