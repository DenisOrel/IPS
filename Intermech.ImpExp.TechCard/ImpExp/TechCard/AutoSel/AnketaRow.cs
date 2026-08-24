// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.AutoSel.AnketaRow
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.AutoSelection.Client;

#nullable disable
namespace Intermech.ImpExp.TechCard.AutoSel;

public class AnketaRow
{
  public AnketaRow(int id, string tableName, string fieldName, int ctlCondKey, int flag)
  {
    this.Id = id;
    this.TableName = tableName;
    this.FieldName = fieldName;
    this.CtlCondKey = ctlCondKey;
    this.Flag = this.GetAutoSelectionNodeCondRuleByFlag(flag);
  }

  private AutoSelectionNodeCondRule GetAutoSelectionNodeCondRuleByFlag(int flagId)
  {
    switch (flagId)
    {
      case 0:
        return AutoSelectionNodeCondRule.None;
      case 1:
        return AutoSelectionNodeCondRule.Min;
      case 2:
        return AutoSelectionNodeCondRule.Max;
      default:
        return AutoSelectionNodeCondRule.None;
    }
  }

  public int Id { get; }

  public string TableName { get; private set; }

  public string FieldName { get; private set; }

  public int CtlCondKey { get; }

  public AutoSelectionNodeCondRule Flag { get; }

  public override int GetHashCode()
  {
    int num = this.Id;
    int hashCode1 = num.GetHashCode();
    num = this.CtlCondKey;
    int hashCode2 = num.GetHashCode();
    return hashCode1 ^ hashCode2;
  }

  public override bool Equals(object obj)
  {
    if (obj == null)
      return false;
    if (!(obj is AnketaRow anketaRow))
      return this == obj;
    return this.Id == anketaRow.Id && !(this.TableName != anketaRow.TableName) && this.Flag == anketaRow.Flag && this.CtlCondKey == anketaRow.CtlCondKey && !(this.FieldName != anketaRow.FieldName);
  }
}
