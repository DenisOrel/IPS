// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.AddresseeAttEditor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.DataFormats;
using Intermech.Extensions;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Navigator;
using Intermech.Office.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;

#nullable disable
namespace Intermech.Office.Client;

internal class AddresseeAttEditor : UITypeEditor
{
  private readonly bool _multiValues;

  public AddresseeAttEditor(int attributeID)
  {
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeID);
    this._multiValues = attributeType.MultiValueMode == MultiValueModes.MultiValues || attributeType.MultiValueMode == MultiValueModes.MultiValuesFromList;
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider sp,
    object value)
  {
    SelectionOptions options = SelectionOptions.SelectObjects;
    if (!this._multiValues)
      options |= SelectionOptions.DisableMultiselect;
    IDBTypedObjectID[] source1 = SelectionWindow.Select(Localization.GetString("Office.Client_11"), OfficeClientHelper.GetAddresseesDescriptor(), typeof (IDBTypedObjectID), options, OfficeClientHelper.AddresseeTypes) as IDBTypedObjectID[];
    long objectID = 0;
    if (context?.PropertyDescriptor is PropDescriptor propertyDescriptor && propertyDescriptor.Component is IElementInfo component && component.ElementKind == AttributableElements.Object)
      objectID = component.ElementIdentifier;
    if (source1 == null || source1.Length == 0)
      return value;
    OfficeDocumentTypes officeDocumentTypes = OfficeDocumentTypes.Unknown;
    long[] oldAddresseeIDs = (long[]) null;
    if (objectID != 0L)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject dbObject = sessionKeeper.Session.GetObject(objectID, false);
        if (dbObject != null)
        {
          IDBAttribute attributeById = dbObject.GetAttributeByID(OfficeConsts.AttrOfficeDocumentTypeID);
          if (attributeById != null)
            officeDocumentTypes = (OfficeDocumentTypes) attributeById.AsInteger;
          if (context?.Instance is AttributeValues instance)
          {
            if (instance.Values != null)
            {
              if (instance.Values.Length != 0)
                oldAddresseeIDs = instance.Values.OfType<long>().Where<long>((Func<long, bool>) (id => id != 0L)).ToArray<long>(instance.Values.Length);
            }
          }
        }
      }
    }
    switch (officeDocumentTypes)
    {
      case OfficeDocumentTypes.Incoming:
        if (!((IEnumerable<IDBTypedObjectID>) source1).Any<IDBTypedObjectID>((Func<IDBTypedObjectID, bool>) (iDbTypedObject => !OfficeClientHelper.CheckDirector(iDbTypedObject))))
          break;
        goto case OfficeDocumentTypes.Internal;
      case OfficeDocumentTypes.Internal:
        return value;
    }
    return (object) ((IEnumerable<IDBTypedObjectID>) source1).Where<IDBTypedObjectID>((Func<IDBTypedObjectID, bool>) (x =>
    {
      long[] source2 = oldAddresseeIDs;
      return source2 == null || !((IEnumerable<long>) source2).Contains<long>(x.ObjectID);
    })).Select<IDBTypedObjectID, ObjectPropertyClass>((Func<IDBTypedObjectID, ObjectPropertyClass>) (x => new ObjectPropertyClass(x.ObjectID, x.Caption))).ToArray<ObjectPropertyClass>();
  }
}
