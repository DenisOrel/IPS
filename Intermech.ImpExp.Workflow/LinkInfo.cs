// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.LinkInfo
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Workflow;

internal class LinkInfo
{
  public int SchemeID;
  public int ActivityID;
  public int LinkID;
  public int LinkKind;
  public int LinkTo;
  public int LinkCondition;
  public long NewLinkID;

  public LinkInfo(IDataReader reader)
  {
    this.SchemeID = Convert.ToInt32(reader[0]);
    this.ActivityID = Convert.ToInt32(reader[1]);
    this.LinkID = Convert.ToInt32(reader[2]);
    this.LinkKind = Convert.ToInt32(reader[3]);
    this.LinkTo = Convert.ToInt32(reader[4]);
    this.LinkCondition = Convert.ToInt32(reader[5]);
  }
}
