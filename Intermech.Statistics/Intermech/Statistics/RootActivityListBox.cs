// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.RootActivityListBox
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;

#nullable disable
namespace Intermech.Statistics;

public class RootActivityListBox
{
  public string ActivityCaption { get; private set; }

  public long ActivityObjID { get; private set; }

  public long ID { get; private set; }

  public RootActivityListBox(object caption, object objectID, object id)
  {
    this.ActivityCaption = caption.ToString();
    this.ActivityObjID = Convert.ToInt64(objectID);
    this.ID = Convert.ToInt64(id);
  }

  public override string ToString() => this.ActivityCaption;
}
