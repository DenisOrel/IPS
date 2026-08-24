// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.TextEditorActions.InsertDBAttributeGuidUIAction
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime.TextEditorActions;

internal sealed class InsertDBAttributeGuidUIAction : ITextEditorUIAction, ITextEditorAction
{
  public string Text
  {
    [DebuggerStepThrough] get => "Вставить Guid атрибута";
  }

  public bool CanInvoke(ITextEditor textEditor)
  {
    if (textEditor == null)
      throw new ArgumentNullException(nameof (textEditor));
    return true;
  }

  public void Invoke(ITextEditor textEditor)
  {
    if (textEditor == null)
      throw new ArgumentNullException(nameof (textEditor));
    using (AttributesSelectDlg attributesSelectDlg = new AttributesSelectDlg(true))
    {
      if (attributesSelectDlg.ShowDialog() != DialogResult.OK || attributesSelectDlg.SelectedAttributesID.Count == 0)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (int attrTypeID in attributesSelectDlg.SelectedAttributesID)
      {
        if (stringBuilder.Length != 0)
          stringBuilder.Append(" ");
        IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attrTypeID);
        stringBuilder.AppendFormat("\"{0}\" /*{1}*/", (object) attributeType.AttributeGuid, (object) attributeType.Name);
      }
      if (stringBuilder.Length == 0)
        return;
      textEditor.Document.Insert(textEditor.CaretOffset, stringBuilder.ToString());
    }
  }
}
