// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.AddresseeEditorActionHandler
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Client.Core.FormDesigner.Controls;
using Intermech.Interfaces;
using Intermech.Office.Interfaces;

#nullable disable
namespace Intermech.Office.Client;

public class AddresseeEditorActionHandler : IFormDesignerActionHandler
{
  public bool ButtonEnabled(object button, object form)
  {
    if (button is AttrButton attrButton && attrButton.FormDesignerAction != null && attrButton.FormDesignerAction.ActionGuid == AddresseeInfo.AddresseeActionGuid && form is DesForm desForm && desForm.Info != null && desForm.Info.ElementIdentifier != 0L)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(desForm.Info.ElementIdentifier);
        if (!objectInfo.Empty)
        {
          IDBObjectType objectType = sessionKeeper.Session.GetObjectType(objectInfo.ObjectTypeID);
          if (objectType != null)
          {
            if (objectType.Attributes.GetAttributeByID(OfficeConsts.AttrAddresseesID) is IDBAttributeType4Object attributeById)
            {
              if ((attributeById.Options & AttributeOptions.DisableManualEdit) == AttributeOptions.DisableManualEdit)
                return false;
            }
          }
        }
      }
    }
    return true;
  }

  public void ButtonPressed(object button, object form)
  {
    if (!(form is DesForm dForm) || dForm.Info == null || dForm.Info.ElementIdentifier == 0L)
      return;
    using (AddresseeEditor addresseeEditor = new AddresseeEditor(dForm.Info.ElementIdentifier, dForm))
    {
      addresseeEditor.Init();
      int num = (int) addresseeEditor.ShowDialog();
    }
  }
}
