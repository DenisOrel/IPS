// Decompiled with JetBrains decompiler
// Type: Script
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Workflow;
using Intermech.Workflow;
using System;

#nullable disable
public class Script
{
  public static void Execute([NotNull] IActivity activity)
  {
    string empty1 = string.Empty;
    IVariable variable1 = activity.Variables.Find("SUBJECT");
    if (variable1 != null)
      empty1 = variable1.Value;
    string empty2 = string.Empty;
    IVariable variable2 = activity.Variables.Find("MESSAGE");
    if (variable2 != null)
      empty2 = variable2.Value;
    string empty3 = string.Empty;
    IVariable variable3 = activity.Variables.Find("TO_EMAIL");
    if (variable3 != null)
      empty3 = variable3.Value;
    string empty4 = string.Empty;
    IVariable variable4 = activity.Variables.Find("FROM_EMAIL");
    if (variable4 != null)
      empty4 = variable4.Value;
    int[] attachmentIdxs = (int[]) null;
    long objectID = 0;
    if (activity.Attachments != null && activity.Attachments.Count > 0)
    {
      IVariable variable5 = activity.Variables.Find("ATTACHMENT_FILEINDEXES");
      if (variable5 != null && variable5.Value != string.Empty)
      {
        string[] strArray = variable5.Value.Split(';');
        attachmentIdxs = new int[strArray.Length];
        for (int index = 0; index < strArray.Length; ++index)
          attachmentIdxs[index] = Convert.ToInt32(strArray[index]);
        objectID = activity.Attachments[0].ObjectID;
      }
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      string str = sessionKeeper.Session.GetCustomService<IEmailService>().SendMessage(sessionKeeper.Session.SessionGUID, (object) empty4, empty3, empty1, empty2, objectID, attachmentIdxs);
      if (!(str != string.Empty) || activity.Attachments == null || activity.Attachments.Count <= 0)
        return;
      sessionKeeper.Session.GetObject(activity.Attachments[0].ObjectID).Attributes.AddAttribute(wfConsts.attributeMessageID, false, new object[1]
      {
        (object) str
      });
    }
  }
}
