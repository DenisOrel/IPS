// Decompiled with JetBrains decompiler
// Type: Intermech.BugReports.RejectBugAction
// Assembly: Intermech.BugReports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 16F80F46-2B9D-4747-9BFD-4CC209192F4E
// Assembly location: D:\IPS\Client\Intermech.BugReports.dll

using Intermech.Interfaces;

#nullable disable
namespace Intermech.BugReports;

public class RejectBugAction : BugAction
{
  public RejectBugAction()
  {
    this._userAttrID = MetaDataHelper.GetAttributeTypeID(BugReportsHolder.AT.FixUser);
    this._dataAttrID = MetaDataHelper.GetAttributeTypeID(BugReportsHolder.AT.FixData);
    this._resultAttrID = MetaDataHelper.GetAttributeTypeID(BugReportsHolder.AT.BugStatus);
    this._status = "Отклонена";
  }
}
