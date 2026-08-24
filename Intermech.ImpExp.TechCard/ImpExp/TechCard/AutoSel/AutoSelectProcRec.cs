// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.AutoSel.AutoSelectProcRec
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.AutoSel;

internal class AutoSelectProcRec
{
  private int key;
  private int groupId;
  private string name = string.Empty;
  private int rootKey;
  private int workType;
  private Guid proc = Guid.Empty;

  public AutoSelectProcRec() => this.Proc = Guid.Empty;

  public AutoSelectProcRec(int _key, int _groupId, string _name, int _rootkey, int _workType)
  {
    this.key = _key;
    this.groupId = _groupId;
    this.name = _name;
    this.rootKey = _rootkey;
    this.workType = _workType;
    this.Proc = Guid.Empty;
  }

  public Guid Proc
  {
    get => this.proc;
    set => this.proc = value;
  }
}
