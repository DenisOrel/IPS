// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common.TechExpFolderObject
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Expert;
using Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;
using Intermech.Interfaces;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.Common;

internal class TechExpFolderObject
{
  internal TechExpFolderObject(TechExpKey key, object sourceObject)
  {
    this.Key = key;
    this.SourceObject = sourceObject;
  }

  public TechExpKey Key { get; private set; }

  public object SourceObject { get; private set; }

  public string Name { get; set; }

  public FormulaData Condition { get; internal set; }

  public TempFormula IpsCondition { get; internal set; }

  public QuickObjectInfo ImportedObjectInfo { get; set; }
}
