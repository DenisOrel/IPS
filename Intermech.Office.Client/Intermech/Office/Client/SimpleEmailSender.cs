// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SimpleEmailSender
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;
using System;

#nullable disable
namespace Intermech.Office.Client;

internal class SimpleEmailSender : EmailSender
{
  private Guid _accountGuid;

  public SimpleEmailSender(Guid accountGuid, long documentID)
    : base(documentID)
  {
    this._accountGuid = accountGuid;
  }

  public override bool OnSendClickEvent([CanBeNull] object sender, [NotNull] OnSendClickEventArgs e)
  {
    if (e.AccountGuid != Guid.Empty)
      this._accountGuid = e.AccountGuid;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      string str = sessionKeeper.Session.GetCustomService<IEmailService>().SendMessage(sessionKeeper.Session.SessionGUID, (object) this._accountGuid, e.ToEmail, e.Subject, e.Message, e.Indexes.Length != 0 ? this._Document : 0L, e.Indexes);
      if (!(str != string.Empty))
        return false;
      sessionKeeper.Session.GetObject(this._Document).Attributes.AddAttribute(OfficeConsts.AttrMessageIdentityID, false, new object[1]
      {
        (object) str
      });
      this.OkMessage(e.Subject);
      return true;
    }
  }
}
