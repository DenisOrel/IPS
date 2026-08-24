// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.RtfDataConvertor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal class RtfDataConvertor
{
  private int _richTextBoxThreadId = -1;
  private RichTextBox _richTextBox;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private bool AllowConversion(string rtfValue)
  {
    return rtfValue.IndexOf("{\\rtf1\\", 0, Math.Min(rtfValue.Length, 10), StringComparison.Ordinal) != -1;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private RichTextBox GetRichTextBox(int threadId)
  {
    lock (this)
    {
      if (this._richTextBoxThreadId == threadId)
        return this._richTextBox;
      this._richTextBoxThreadId = threadId;
      return this._richTextBox = new RichTextBox();
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private bool DoConvertToString(string rtfValue, out string strValue)
  {
    strValue = rtfValue;
    RichTextBox richTextBox = this.GetRichTextBox(Thread.CurrentThread.ManagedThreadId);
    try
    {
      richTextBox.Rtf = rtfValue;
      strValue = richTextBox.Text;
      return true;
    }
    catch
    {
    }
    return false;
  }

  public bool ConvertToString(string rtfValue, out string strValue)
  {
    strValue = rtfValue;
    return this.AllowConversion(rtfValue) && this.DoConvertToString(rtfValue, out strValue);
  }
}
