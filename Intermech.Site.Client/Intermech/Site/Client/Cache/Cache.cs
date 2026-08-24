// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Cache.Cache
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces.WebPortal;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Intermech.Site.Client.Cache;

[Serializable]
internal sealed class Cache
{
  public DateTime LastModify = DateTime.MinValue;
  public PortalObjectType[] ObjTypes;
  public PortalAttributeType[] PublishRelationAttributes;
  public List<Tuple<string, PortalAttributeType>> PublishAttributes;
  public Dictionary<int, Dictionary<object, string>> PossibleValues;

  public Cache()
  {
  }

  public Cache(
    DateTime lastModify,
    PortalObjectType[] objTypes,
    PortalAttributeType[] publishRelationAttributes,
    AttributePossibleValues[] attrPosibleValues)
  {
    this.LastModify = lastModify;
    this.ObjTypes = objTypes;
    this.PublishRelationAttributes = publishRelationAttributes;
    if (objTypes != null)
    {
      this.PublishAttributes = new List<Tuple<string, PortalAttributeType>>();
      for (int i = 0; i < objTypes.Length; i++)
      {
        if (objTypes[i].Attributes != null)
        {
          for (int j = 0; j < objTypes[i].Attributes.Length; j++)
          {
            if (!this.PublishAttributes.Exists((Predicate<Tuple<string, PortalAttributeType>>) (x => x.Item1.Equals(objTypes[i].Attributes[j].GUID))))
              this.PublishAttributes.Add(new Tuple<string, PortalAttributeType>(objTypes[i].Attributes[j].GUID, objTypes[i].Attributes[j]));
          }
        }
      }
    }
    if (attrPosibleValues == null || attrPosibleValues.Length == 0)
      return;
    this.PossibleValues = new Dictionary<int, Dictionary<object, string>>(attrPosibleValues.Length);
    for (int index1 = 0; index1 < attrPosibleValues.Length; ++index1)
    {
      AttributePossibleValues attrPosibleValue = attrPosibleValues[index1];
      if (attrPosibleValue != null && attrPosibleValue.PossibleValues != null && attrPosibleValue.AttributeID != 0)
      {
        Dictionary<object, string> dictionary = new Dictionary<object, string>(attrPosibleValue.PossibleValues.Length);
        for (int index2 = 0; index2 < attrPosibleValue.PossibleValues.Length; ++index2)
        {
          object key = (object) null;
          switch (attrPosibleValue.PossibleValues[index2].ValueFieldName)
          {
            case "F_STRING_VALUE":
              key = (object) attrPosibleValue.PossibleValues[index2].StringValue;
              break;
            case "F_INTEGER_VALUE":
              key = (object) attrPosibleValue.PossibleValues[index2].IntegerValue;
              break;
            case "F_DATE_VALUE":
              key = (object) Convert.ToDateTime(attrPosibleValue.PossibleValues[index2].DateTimeValue, (IFormatProvider) CultureInfo.InvariantCulture);
              break;
            case "F_DOUBLE_VALUE":
              key = (object) attrPosibleValue.PossibleValues[index2].DoubleValue;
              break;
          }
          if (key != null)
            dictionary.Add(key, attrPosibleValue.PossibleValues[index2].Description);
        }
        this.PossibleValues.Add(attrPosibleValue.AttributeID, dictionary);
      }
    }
  }
}
