// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ArticlesView.ReleaseArticlesEnabledProp
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;

#nullable disable
namespace Intermech.Pdm.ArticlesView;

internal class ReleaseArticlesEnabledProp : ObjTypeOptionProp
{
  private int _objTypeDocumentID = -1;

  public ReleaseArticlesEnabledProp()
    : base(EnumDescConverter.GetEnumDescription((Enum) ObjectTypeOptions.ReleaseArticlesEnabled))
  {
  }

  protected override PropDescriptor[] OnGetDescriptors(
    PropDescriptorHolder pdh,
    int category,
    object id)
  {
    if (this._objTypeDocumentID == -1)
    {
      IDBObjectTypeInfo objectType = (ServicesManager.GetService(typeof (IClientMetadataCache)) as IClientMetadataCache).GetObjectType(new Guid("cad00070-306c-11d8-b4e9-00304f19f545"), true);
      if (objectType != null)
        this._objTypeDocumentID = objectType.ObjectType;
    }
    ObjectTypeFolder objectTypeFolder = ((CustomFolder) pdh).NodeParent.Tag as ObjectTypeFolder;
    tag = ((CustomFolder) pdh).NodeParent.Tag as ObjectTypeFolder;
    while (tag != null)
    {
      if (tag.NodeParent.Tag is ObjectTypeFolder tag)
        objectTypeFolder = tag;
    }
    if (objectTypeFolder == null || Convert.ToInt32(objectTypeFolder.Id) != this._objTypeDocumentID)
      return (PropDescriptor[]) null;
    this.propertyDescriptor = (PropDescriptor) null;
    foreach (PropDescriptor propDescriptor in pdh.PropDescriptorCollection)
    {
      if (propDescriptor.DisplayName.Equals(this.subscriberID))
      {
        this.propertyDescriptor = propDescriptor;
        break;
      }
    }
    bool aBoolean = false;
    if (Convert.ToInt32(id) > 0)
    {
      IMSObjectType objectType = MetaDataHelper.GetObjectType(Convert.ToInt32(id));
      if (objectType != null)
        aBoolean = (objectType.Options & ObjectTypeOptions.ReleaseArticlesEnabled) == ObjectTypeOptions.ReleaseArticlesEnabled;
    }
    this.attributeValue = (object) aBoolean;
    if (this.propertyDescriptor != null)
      this.propertyDescriptor.SetValue((object) this, (object) new BoolPropertyClass(aBoolean));
    else
      this.propertyDescriptor = new PropDescriptor(0, (object) null, this.SubscriberID, (object) new BoolPropertyClass(aBoolean), typeof (BoolPropertyClass), (TypeConverter) new BoolConverter(), (object) null, string.Empty, this.subscriberID, false, true, false);
    if (this.propertyDescriptor == null)
      return (PropDescriptor[]) null;
    return new PropDescriptor[1]{ this.propertyDescriptor };
  }

  protected override bool OnApply(PropDescriptorHolder pdh, int category, object id, object idOld)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      foreach (PropDescriptor propDescriptor in pdh.PropDescriptorCollection)
      {
        if (propDescriptor.DisplayName.Equals(this.subscriberID))
        {
          bool boolean = ((BoolPropertyClass) propDescriptor.GetValue(id)).Boolean;
          if (boolean != Convert.ToBoolean(this.attributeValue))
          {
            IDBObjectType objectType = sessionKeeper.Session.GetObjectType(Convert.ToInt32(id));
            if (boolean)
              objectType.Options |= ObjectTypeOptions.ReleaseArticlesEnabled;
            else
              objectType.Options &= ~ObjectTypeOptions.ReleaseArticlesEnabled;
            this.attributeValue = (object) boolean;
            break;
          }
          break;
        }
      }
      return false;
    }
  }
}
