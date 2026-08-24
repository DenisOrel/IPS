// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.DesignTime.TextEditorActions.InsertDBObjectTypeGuidUIAction
// Assembly: Intermech.Scripting.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 9102AE80-F0DA-4332-9938-7F8D4C639EFA
// Assembly location: D:\IPS\Client\Intermech.Scripting.Client.dll

using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Scripting.Common.DesignTime.TextEditorActions;

internal sealed class InsertDBObjectTypeGuidUIAction : ITextEditorUIAction, ITextEditorAction
{
  public string Text
  {
    [DebuggerStepThrough] get => "Вставить Guid типа объектов";
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
    using (SelectorForm selectorForm = new SelectorForm(typeof (ObjectTypesFolder), "Типы объектов", typeof (ObjectTypeFolder), true))
    {
      if (selectorForm.ShowDialog() != DialogResult.OK || selectorForm.IDList.Count == 0)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (int id in selectorForm.IDList)
      {
        if (stringBuilder.Length != 0)
          stringBuilder.Append(" ");
        IMSObjectType objectType = MetaDataHelper.GetObjectType(id);
        stringBuilder.AppendFormat("\"{0}\" /*{1}*/", (object) objectType.Guid, (object) objectType.ObjectTypeName);
      }
      if (stringBuilder.Length == 0)
        return;
      textEditor.Document.Insert(textEditor.CaretOffset, stringBuilder.ToString());
    }
  }
}
