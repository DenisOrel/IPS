// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignControl_LCStepProperty
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.DatabaseConfigurator;
using Intermech.Localization;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Signs.Client;

internal class SignControl_LCStepProperty : ICategoryProps
{
  private PropDescriptor _propertyDescriptor;

  public void Cancel(PropDescriptorHolder pdh, int category, object id)
  {
    if (pdh.PropDescriptorCollection == null)
      return;
    PropertyDescriptor[] propertyDescriptorArray = new PropertyDescriptor[pdh.PropDescriptorCollection.Count];
    pdh.PropDescriptorCollection.CopyTo((Array) propertyDescriptorArray, 0);
    ILCStep lcStep = pdh as ILCStep;
    foreach (PropDescriptor propDescriptor in propertyDescriptorArray)
    {
      if (propDescriptor.DisplayName.Equals(this.SubscriberID) && propDescriptor.ValueChanged)
      {
        SignControlPropertyClass controlPropertyClass = new SignControlPropertyClass(lcStep.LCStepProperties.LCStep, SignControlPropertyEnum.LCStep, pdh.PropDescriptorCollection[3].IsReadOnly, lcStep.LCStepProperties.ObjectTypeID);
        propDescriptor.SetValue(propDescriptor.Component, (object) controlPropertyClass);
      }
    }
  }

  public string SubscriberID => LocalizationHolder.rm.GetString("Signs_17");

  public PropDescriptor[] GetPropDescriptors(PropDescriptorHolder pdh, int category, object id)
  {
    this._propertyDescriptor = (PropDescriptor) null;
    foreach (PropDescriptor propDescriptor in pdh.PropDescriptorCollection)
    {
      if (propDescriptor.DisplayName.Equals(this.SubscriberID))
      {
        this._propertyDescriptor = propDescriptor;
        break;
      }
    }
    if (this._propertyDescriptor == null)
    {
      ILCStep lcStep = pdh as ILCStep;
      SignControlPropertyClass controlPropertyClass = new SignControlPropertyClass(lcStep.LCStepProperties.LCStep, SignControlPropertyEnum.LCStep, pdh.PropDescriptorCollection[3].IsReadOnly, lcStep.LCStepProperties.ObjectTypeID);
      this._propertyDescriptor = (PropDescriptor) null;
      TypeConverter converter = TypeDescriptor.GetConverter(typeof (SignControlPropertyClass));
      object editor = TypeDescriptor.GetEditor(typeof (SignControlPropertyClass), typeof (UITypeEditor));
      this._propertyDescriptor = new PropDescriptor(0, (object) null, LocalizationHolder.rm.GetString("Signs_18"), (object) controlPropertyClass, typeof (SignControlPropertyClass), converter, editor, string.Empty, LocalizationHolder.rm.GetString("Signs_19"), false, true, false);
    }
    return new PropDescriptor[1]{ this._propertyDescriptor };
  }

  public bool Apply(PropDescriptorHolder pdh, int category, object id, object idOld)
  {
    if (pdh.PropDescriptorCollection != null)
    {
      PropertyDescriptor[] propertyDescriptorArray = new PropertyDescriptor[pdh.PropDescriptorCollection.Count];
      pdh.PropDescriptorCollection.CopyTo((Array) propertyDescriptorArray, 0);
      foreach (PropDescriptor propDescriptor in propertyDescriptorArray)
      {
        if (propDescriptor.DisplayName.Equals(this.SubscriberID) && propDescriptor.ValueChanged)
        {
          ILCStep component = propDescriptor.Component as ILCStep;
          return (propDescriptor.GetValue((object) component) as SignControlPropertyClass).Save(component.LCStepProperties.LCStep, SignControlPropertyEnum.LCStep);
        }
      }
    }
    return true;
  }

  public void ChangeEventData(PropDescriptorHolder pdh, int category, object id, EventArgs e)
  {
    this._propertyDescriptor.ValueChanged = true;
  }
}
