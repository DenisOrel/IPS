// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ProcessEmailSender
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Workflow;
using Intermech.Office.Interfaces;
using Intermech.Workflow;
using System;
using System.Text;

#nullable disable
namespace Intermech.Office.Client;

internal class ProcessEmailSender : EmailSender
{
  [NotNull]
  private readonly OfficeGeneralSettings _settings;

  public ProcessEmailSender([NotNull] OfficeGeneralSettings settings, long documentID)
    : base(documentID)
  {
    this._settings = settings;
  }

  public override bool OnSendClickEvent([CanBeNull] object sender, [NotNull] OnSendClickEventArgs e)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IProcess process = sessionKeeper.Session.GetCustomService<IRouterService>(true, Localization.GetString("Office.Client_24")).CreateProcess(sessionKeeper.Session.SessionGUID, this._settings.TemplateID);
      process.Name = Localization.GetString("Office.Client_25", (object) e.Subject);
      if (process.StartActivity == null)
        throw new Exception("Start activity not found!");
      process.StartActivity.Attachments.Add(this._Document);
      IVariable variable1 = process.StartActivity.Variables.Find("SUBJECT");
      if (variable1 != null)
        variable1.Value = e.Subject;
      IVariable variable2 = process.StartActivity.Variables.Find("MESSAGE");
      if (variable2 != null)
        variable2.Value = e.Message;
      IVariable variable3 = process.StartActivity.Variables.Find("TO_EMAIL");
      if (variable3 != null)
        variable3.Value = e.ToEmail;
      IVariable variable4 = process.StartActivity.Variables.Find("FROM_EMAIL");
      if (variable4 != null)
        variable4.Value = this._settings.AutoSendEmail;
      IVariable variable5 = process.StartActivity.Variables.Find("FROM_USER");
      if (variable5 != null)
      {
        ParticipantList participantList = new ParticipantList(sessionKeeper.Session);
        participantList.AddParticipant(ParticipantKind.User, this._settings.UserID);
        variable5.Value = participantList.AsString;
      }
      if (e.Indexes.Length != 0)
      {
        IVariable variable6 = process.StartActivity.Variables.Find("ATTACHMENT_FILEINDEXES");
        if (variable6 != null)
        {
          StringBuilder stringBuilder = new StringBuilder();
          for (int index = 0; index < e.Indexes.Length; ++index)
          {
            if (index > 0)
              stringBuilder.Append(";");
            stringBuilder.Append(e.Indexes[index]);
          }
          variable6.Value = stringBuilder.ToString();
        }
      }
      process.StartProcess();
      this.OkMessage(e.Subject);
      return true;
    }
  }
}
