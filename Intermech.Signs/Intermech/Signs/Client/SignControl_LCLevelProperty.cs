// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignControl_LCLevelProperty
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

internal class SignControl_LCLevelProperty : ICategoryProps
{
  private PropDescriptor _propertyDescriptor;

  public void Cancel(PropDescriptorHolder pdh, int category, object id)
  {
    PropertyDescriptor[] propertyDescriptorArray = new PropertyDescriptor[pdh.PropDescriptorCollection.Count];
    pdh.PropDescriptorCollection.CopyTo((Array) propertyDescriptorArray, 0);
    LevelFolder levelFolder = pdh as LevelFolder;
    foreach (PropDescriptor propDescriptor in propertyDescriptorArray)
    {
      if (propDescriptor.DisplayName.Equals(this.SubscriberID) && propDescriptor.ValueChanged)
      {
        SignControlPropertyClass controlPropertyClass = new SignControlPropertyClass(Convert.ToInt32(levelFolder.Id), SignControlPropertyEnum.LCLevel);
        propDescriptor.SetValue(propDescriptor.Component, (object) controlPropertyClass);
      }
    }
  }

  public string SubscriberID => LocalizationHolder.rm.GetString("Signs_20");

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
      SignControlPropertyClass controlPropertyClass = new SignControlPropertyClass(Convert.ToInt32((pdh as LevelFolder).Id), SignControlPropertyEnum.LCLevel);
      TypeConverter converter = TypeDescriptor.GetConverter(typeof (SignControlPropertyClass));
      object editor = TypeDescriptor.GetEditor(typeof (SignControlPropertyClass), typeof (UITypeEditor));
      this._propertyDescriptor = new PropDescriptor(0, (object) null, LocalizationHolder.rm.GetString("Signs_21"), (object) controlPropertyClass, typeof (SignControlPropertyClass), converter, editor, string.Empty, LocalizationHolder.rm.GetString("Signs_22"), false, true, false);
    }
    return new PropDescriptor[1]{ this._propertyDescriptor };
  }

  public bool Apply(PropDescriptorHolder pdh, int category, object id, object idOld)
  {
    PropertyDescriptor[] propertyDescriptorArray = new PropertyDescriptor[pdh.PropDescriptorCollection.Count];
    pdh.PropDescriptorCollection.CopyTo((Array) propertyDescriptorArray, 0);
    foreach (PropDescriptor propDescriptor in propertyDescriptorArray)
    {
      if (propDescriptor.DisplayName.Equals(this.SubscriberID) && propDescriptor.ValueChanged)
      {
        LevelFolder component = propDescriptor.Component as LevelFolder;
        return (propDescriptor.GetValue((object) component) as SignControlPropertyClass).Save(Convert.ToInt32(component.Id), SignControlPropertyEnum.LCLevel);
      }
    }
    return true;
  }

  public void ChangeEventData(PropDescriptorHolder pdh, int category, object id, EventArgs e)
  {
    this._propertyDescriptor.ValueChanged = true;
  }
}
