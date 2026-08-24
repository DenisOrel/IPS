// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.PleaseWaitFormManager
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using System.Threading;

#nullable disable
namespace Intermech.Site.Client;

internal class PleaseWaitFormManager
{
  private PleaseWaitForm _form;

  public void ShowForm()
  {
    new Thread(new ThreadStart(this.ThreadMethod))
    {
      IsBackground = true,
      Name = "PleaseWaitForm_Method"
    }.Start();
  }

  public void Close()
  {
    if (this._form == null)
      return;
    this._form.CloseForm();
    this._form = (PleaseWaitForm) null;
  }

  private void ThreadMethod()
  {
    this._form = new PleaseWaitForm();
    int num = (int) this._form.ShowDialog();
  }
}
