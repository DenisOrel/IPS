// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.ExecutionOrderEditorActionHandler
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal class ExecutionOrderEditorActionHandler : IFormDesignerActionHandler
{
  public bool ButtonEnabled([NotNull] object button, [NotNull] object form)
  {
    foreach (IAttributeEditor linkedControl in Intermech.Diagnostics.Check.Is<DesForm>(form, nameof (form)).GetLinkedControls())
    {
      if (linkedControl.AttributeInfo.AttributeGuid.Equals(OfficeConsts.AttrResolutionExecuteTypeGuid) && linkedControl.Values != null && linkedControl.Values.Values.Length != 0 && linkedControl.Values.Values[0] != DBNull.Value)
        return Convert.ToInt32(linkedControl.Values.Values[0]) == 2;
    }
    return false;
  }

  public void ButtonPressed([NotNull] object button, [NotNull] object form)
  {
    DesForm dForm = Intermech.Diagnostics.Check.Is<DesForm>(form, nameof (form));
    if (dForm.Info == null || dForm.Info.ElementIdentifier == 0L)
      return;
    using (ExecutionOrderEditor executionOrderEditor = new ExecutionOrderEditor())
    {
      executionOrderEditor.Init(dForm);
      if (executionOrderEditor.ShowDialog() != DialogResult.OK)
        return;
      List<int> executionOrders = executionOrderEditor.ExecutionOrders;
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(dForm.Info.ElementIdentifier);
        IDBAttribute dbAttribute = dbObject.GetAttributeByID(OfficeConsts.AttrExecutionOrderID);
        if (executionOrders == null)
        {
          dbAttribute?.Delete(0L);
        }
        else
        {
          if (dbAttribute == null)
            dbAttribute = dbObject.Attributes.AddAttribute(OfficeConsts.AttrExecutionOrderID, false);
          else
            dbAttribute.ClearValues();
          for (int index = 0; index < executionOrders.Count; ++index)
          {
            if (index == 0)
              dbAttribute.Value = (object) executionOrders[index];
            else
              dbAttribute.AddValue((object) executionOrders[index]);
          }
        }
      }
    }
  }
}
