// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.PrivateRegNumberEditor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Interfaces;
using Intermech.Office.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Office.Client;

internal class PrivateRegNumberEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider sp,
    object value)
  {
    using (ManualRegNumberForm manualRegNumberForm = new ManualRegNumberForm(true))
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        PrivateRegNumberValueAttrProxy numberValueAttrProxy = value as PrivateRegNumberValueAttrProxy;
        bool readOnly = false;
        if (numberValueAttrProxy == null)
          return value;
        if (numberValueAttrProxy.Value == string.Empty)
        {
          readOnly = true;
        }
        else
        {
          IOfficeRegistrationService customService = sessionKeeper.Session.GetCustomService<IOfficeRegistrationService>();
          if (customService.GetUserUnit(sessionKeeper.Session.UserID) == 0L)
          {
            readOnly = true;
          }
          else
          {
            string privateRegNumber = customService.GetPrivateRegNumber(sessionKeeper.Session.SessionGUID, numberValueAttrProxy.ObjectID);
            if (privateRegNumber == string.Empty || !privateRegNumber.Equals(numberValueAttrProxy.Value))
              readOnly = true;
          }
        }
        QuickObjectInfo objectInfo = ApplicationServices.Container.GetService<IObjectsInfoCache>().GetObjectInfo(numberValueAttrProxy.ObjectID);
        manualRegNumberForm.Initialize(numberValueAttrProxy.ObjectID, objectInfo.ObjectTypeID, numberValueAttrProxy.Value, readOnly, false);
        return manualRegNumberForm.ShowDialog() == DialogResult.OK && numberValueAttrProxy.Value != manualRegNumberForm.Template ? (object) new PrivateRegNumberValueAttrProxy(manualRegNumberForm.Template, numberValueAttrProxy.ObjectID) : value;
      }
    }
  }
}
