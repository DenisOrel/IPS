// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.ElementListTypeMarkerUIEditor
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.PropertyEditors;
using Intermech.Tools.Integrators.Electrical;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class ElementListTypeMarkerUIEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    SelectorForm selectorForm = new SelectorForm(typeof (ObjectTypesFolder), "Типы объектов", typeof (ObjectTypeFolder), false);
    selectorForm.SelectorFilter = (ISelectorFilter) new ElectricalSchemaElementListTypesFilter();
    return selectorForm.ShowDialog() == DialogResult.OK && selectorForm.IDList.Count == 1 ? (object) this.ConvertToObjectType((int) selectorForm.IDList[0]) : value;
  }

  private GlobalId<int> ConvertToObjectType(int objTypeId)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObjectType objectType = sessionKeeper.Session.GetObjectType(objTypeId, true);
      return new GlobalId<int>(((IDBGuid) objectType).GUID, objTypeId, objectType.ObjectTypeName);
    }
  }
}
