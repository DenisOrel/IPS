// Decompiled with JetBrains decompiler
// Type: Intermech.BugReports.CheckBugAction
// Assembly: Intermech.BugReports, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 16F80F46-2B9D-4747-9BFD-4CC209192F4E
// Assembly location: D:\IPS\Client\Intermech.BugReports.dll

using Intermech.Interfaces;

#nullable disable
namespace Intermech.BugReports;

public class CheckBugAction : BugAction
{
  public CheckBugAction()
  {
    this._userAttrID = MetaDataHelper.GetAttributeTypeID(BugReportsHolder.AT.CheckUser);
    this._dataAttrID = MetaDataHelper.GetAttributeTypeID(BugReportsHolder.AT.CheckData);
    this._resultAttrID = MetaDataHelper.GetAttributeTypeID(BugReportsHolder.AT.CheckResult);
    this._status = "Исправлена";
  }
}
