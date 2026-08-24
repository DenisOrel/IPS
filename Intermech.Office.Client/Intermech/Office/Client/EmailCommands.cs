// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.EmailCommands
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using ImSSP;
using Intermech.Controls;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class EmailCommands
{
  public static void SendEmail(long documentID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      EmailAccaunt[] accaunts = sessionKeeper.Session.GetCustomService<IEmailService>().GetAccaunts(sessionKeeper.Session.UserID, false);
      Guid accountGuid = Guid.Empty;
      if (accaunts != null)
      {
        if (accaunts.Length == 1)
          accountGuid = accaunts[0].Guid;
        IDBObject dbObject = sessionKeeper.Session.GetObject(documentID);
        bool registeredDoc;
        if (!EmailCommands.CheckOutgoingDocument(dbObject, out registeredDoc))
          return;
        using (SendEmailForm sendEmailForm = new SendEmailForm())
        {
          sendEmailForm.Init(sessionKeeper.Session, dbObject, registeredDoc, accaunts);
          SimpleEmailSender simpleEmailSender = new SimpleEmailSender(accountGuid, documentID);
          sendEmailForm.OnSendClickEvent += new OnSendClickEventHandler(((EmailSender) simpleEmailSender).OnSendClickEvent);
          int num = (int) sendEmailForm.ShowDialog();
        }
      }
      else
      {
        int num1 = (int) IMMessageBox.Show(Localization.GetString(sc_15080.ssp_office_15081()), Localization.GetString("Office.Client_18"), MessageBoxButtons.OK, IMMessageBoxImage.Error);
      }
    }
  }

  public static void SendEmailProcess(long documentID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      OfficeGeneralSettings settings = sessionKeeper.Session.GetCustomService<IOfficeGeneralSettingsService>().Settings;
      if (settings.TemplateID == 0L)
      {
        int num1 = (int) IMMessageBox.Show(Localization.GetString(sc_15080.ssp_office_15082()), Localization.GetString("Office.Client_21"), MessageBoxButtons.OK, IMMessageBoxImage.Warning);
      }
      else if (settings.AutoSendEmail == string.Empty || !EmailHelper.IsEmail(settings.AutoSendEmail ?? string.Empty))
      {
        int num2 = (int) IMMessageBox.Show(Localization.GetString(sc_15080.ssp_office_15083()), Localization.GetString("Office.Client_22"), MessageBoxButtons.OK, IMMessageBoxImage.Warning);
      }
      else if (settings.UserID == 0L)
      {
        int num3 = (int) IMMessageBox.Show(Localization.GetString(sc_15080.ssp_office_15084()), Localization.GetString("Office.Client_23"), MessageBoxButtons.OK, IMMessageBoxImage.Warning);
      }
      else
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(documentID);
        bool registeredDoc;
        if (!EmailCommands.CheckOutgoingDocument(dbObject, out registeredDoc))
          return;
        using (SendEmailForm sendEmailForm = new SendEmailForm())
        {
          sendEmailForm.Init(sessionKeeper.Session, dbObject, registeredDoc, (EmailAccaunt[]) null);
          ProcessEmailSender processEmailSender = new ProcessEmailSender(settings, documentID);
          sendEmailForm.OnSendClickEvent += new OnSendClickEventHandler(((EmailSender) processEmailSender).OnSendClickEvent);
          int num4 = (int) sendEmailForm.ShowDialog();
        }
      }
    }
  }

  private static bool CheckOutgoingDocument([NotNull] IDBObject document, out bool registeredDoc)
  {
    registeredDoc = false;
    if (document.GetAttributeByID(OfficeConsts.AttrRegNumberID) != null)
    {
      IDBAttribute attributeById = document.GetAttributeByID(OfficeConsts.AttrOfficeDocumentTypeID);
      if (attributeById == null || (int) attributeById.AsInteger != 1)
      {
        int num = (int) IMMessageBox.Show(Localization.GetString(sc_15080.ssp_office_15085()), Localization.GetString("Office.Client_29"), MessageBoxButtons.OK, IMMessageBoxImage.Warning);
        return false;
      }
      registeredDoc = true;
    }
    return true;
  }
}
