// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.EmailSender
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using ImSSP;
using Intermech.Controls;
using Intermech.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal abstract class EmailSender
{
  protected long _Document;

  protected EmailSender(long documentID) => this._Document = documentID;

  public abstract bool OnSendClickEvent([CanBeNull] object sender, [NotNull] OnSendClickEventArgs e);

  protected void OkMessage([NotNull] string subject)
  {
    int num = (int) IMMessageBox.Show(Localization.GetString(sc_15086.ssp_office_15087()), Localization.GetString("Office.Client_19", (object) subject), MessageBoxButtons.OK, IMMessageBoxImage.Information);
  }
}
