// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.MAPI
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Office.Client;

internal class MAPI
{
  [NotNull]
  private readonly string[] errors = new string[27]
  {
    "OK [0]",
    "User abort [1]",
    "General MAPI failure [2]",
    "MAPI login failure [3]",
    "Disk full [4]",
    "Insufficient memory [5]",
    "Access denied [6]",
    "-unknown- [7]",
    "Too many sessions [8]",
    "Too many files were specified [9]",
    "Too many recipients were specified [10]",
    "A specified attachment was not found [11]",
    "Attachment open failure [12]",
    "Attachment write failure [13]",
    "Unknown recipient [14]",
    "Bad recipient type [15]",
    "No messages [16]",
    "Invalid message [17]",
    "Text too large [18]",
    "Invalid session [19]",
    "Type not supported [20]",
    "A recipient was specified ambiguously [21]",
    "Message in use [22]",
    "Network failure [23]",
    "Invalid edit fields [24]",
    "Invalid recipients [25]",
    "Not supported [26]"
  };
  [NotNull]
  private readonly List<MapiRecipDesc> _recipients = new List<MapiRecipDesc>();
  [NotNull]
  private readonly List<string> _attachments = new List<string>();
  private int _lastError;
  private const int MAPI_LOGON_UI = 1;
  private const int MAPI_DIALOG = 8;
  private const int maxAttachments = 20;

  public bool AddRecipientTo([NotNull] string email)
  {
    return this.AddRecipient(email, MAPI.HowTo.MAPI_TO);
  }

  public bool AddRecipientCC([NotNull] string email)
  {
    return this.AddRecipient(email, MAPI.HowTo.MAPI_TO);
  }

  public bool AddRecipientBCC([NotNull] string email)
  {
    return this.AddRecipient(email, MAPI.HowTo.MAPI_TO);
  }

  public void AddAttachment([NotNull] string strAttachmentFileName)
  {
    this._attachments.Add(strAttachmentFileName);
  }

  public int SendMailPopup([NotNull] string strSubject, [NotNull] string strBody)
  {
    return this.SendMail(strSubject, strBody, 9);
  }

  public int SendMailDirect([NotNull] string strSubject, [NotNull] string strBody)
  {
    return this.SendMail(strSubject, strBody, 1);
  }

  [DllImport("MAPI32.DLL")]
  private static extern int MAPISendMail(
    IntPtr sess,
    IntPtr hwnd,
    [NotNull] MapiMessage message,
    int flg,
    int rsv);

  private int SendMail([NotNull] string strSubject, [NotNull] string strBody, int how)
  {
    MapiMessage msg = new MapiMessage()
    {
      subject = strSubject,
      noteText = strBody
    };
    msg.recips = this.GetRecipients(out msg.recipCount);
    msg.files = this.GetAttachments(out msg.fileCount);
    this._lastError = MAPI.MAPISendMail(new IntPtr(0), new IntPtr(0), msg, how, 0);
    int lastError = this._lastError;
    this.Cleanup(ref msg);
    return this._lastError;
  }

  private bool AddRecipient([NotNull] string email, MAPI.HowTo howTo)
  {
    this._recipients.Add(new MapiRecipDesc()
    {
      recipClass = (int) howTo,
      name = email
    });
    return true;
  }

  private IntPtr GetRecipients(out int recipientsCount)
  {
    recipientsCount = 0;
    if (this._recipients.Count == 0)
      return IntPtr.Zero;
    int num = Marshal.SizeOf(typeof (MapiRecipDesc));
    IntPtr recipients = Marshal.AllocHGlobal(this._recipients.Count * num);
    int ptr = (int) recipients;
    foreach (MapiRecipDesc recipient in this._recipients)
    {
      Marshal.StructureToPtr<MapiRecipDesc>(recipient, (IntPtr) ptr, false);
      ptr += num;
    }
    recipientsCount = this._recipients.Count;
    return recipients;
  }

  private IntPtr GetAttachments(out int fileCount)
  {
    fileCount = 0;
    if (this._attachments.Count <= 0 || this._attachments.Count > 20)
      return IntPtr.Zero;
    int num = Marshal.SizeOf(typeof (MapiFileDesc));
    IntPtr attachments = Marshal.AllocHGlobal(this._attachments.Count * num);
    MapiFileDesc structure = new MapiFileDesc();
    structure.position = -1;
    int ptr = (int) attachments;
    foreach (string attachment in this._attachments)
    {
      structure.name = Path.GetFileName(attachment);
      structure.path = attachment;
      Marshal.StructureToPtr<MapiFileDesc>(structure, (IntPtr) ptr, false);
      ptr += num;
    }
    fileCount = this._attachments.Count;
    return attachments;
  }

  public void ComposeMail([NotNull] string[] recipients, [NotNull] string subject, [NotNull] string body, [NotNull] string[] attachments)
  {
    foreach (string recipient in recipients)
      this.AddRecipientTo(recipient);
    foreach (string attachment in attachments)
      this.AddAttachment(attachment);
    this.SendMailPopup(subject, body);
  }

  private void Cleanup([NotNull] ref MapiMessage msg)
  {
    int num1 = Marshal.SizeOf(typeof (MapiRecipDesc));
    if (msg.recips != IntPtr.Zero)
    {
      int recips = (int) msg.recips;
      for (int index = 0; index < msg.recipCount; ++index)
      {
        Marshal.DestroyStructure((IntPtr) recips, typeof (MapiRecipDesc));
        recips += num1;
      }
      Marshal.FreeHGlobal(msg.recips);
    }
    if (msg.files != IntPtr.Zero)
    {
      int num2 = Marshal.SizeOf(typeof (MapiFileDesc));
      int files = (int) msg.files;
      for (int index = 0; index < msg.fileCount; ++index)
      {
        Marshal.DestroyStructure((IntPtr) files, typeof (MapiFileDesc));
        files += num2;
      }
      Marshal.FreeHGlobal(msg.files);
    }
    this._recipients.Clear();
    this._attachments.Clear();
    this._lastError = 0;
  }

  [NotNull]
  public string GetLastError()
  {
    return this._lastError <= 26 ? this.errors[this._lastError] : $"MAPI error [{(object) this._lastError}]";
  }

  private enum HowTo
  {
    MAPI_ORIG,
    MAPI_TO,
    MAPI_CC,
    MAPI_BCC,
  }
}
