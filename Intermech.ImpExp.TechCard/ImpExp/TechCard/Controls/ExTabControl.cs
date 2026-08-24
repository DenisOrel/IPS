// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Controls.ExTabControl
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Controls;

public class ExTabControl : TabControl
{
  protected bool _showTabHeader = true;

  protected override void WndProc(ref Message m)
  {
    if (!this._showTabHeader && m.Msg == 4904 && !this.DesignMode)
      m.Result = (IntPtr) 1;
    else
      base.WndProc(ref m);
  }

  public bool ShowTabHeader
  {
    get => this._showTabHeader;
    set => this._showTabHeader = value;
  }
}
