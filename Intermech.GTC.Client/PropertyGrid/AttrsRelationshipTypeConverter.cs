// Decompiled with JetBrains decompiler
// Type: Intermech.GTC.Client.PropertyGrid.AttrsRelationshipTypeConverter
// Assembly: Intermech.GTC.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 539B70F6-18D3-4230-8795-0EE95CBE5B1C
// Assembly location: D:\IPS\Client\Intermech.GTC.Client.dll

using Intermech.Interfaces;
using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Intermech.GTC.Client.PropertyGrid;

internal class AttrsRelationshipTypeConverter : TypeConverter
{
  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destType)
  {
    if (!(value is AttrsRelationshipPropertyClass relationshipPropertyClass) || !(destType == typeof (string)))
      return base.ConvertTo(context, culture, value, destType);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      string str1 = sessionKeeper.Session.GetAttributeType(relationshipPropertyClass.RelatingAttrId) != null ? sessionKeeper.Session.GetAttributeType(relationshipPropertyClass.RelatingAttrId).Name : string.Empty;
      string str2 = sessionKeeper.Session.GetAttributeType(relationshipPropertyClass.RelatedAttrId) != null ? sessionKeeper.Session.GetAttributeType(relationshipPropertyClass.RelatedAttrId).Name : string.Empty;
      return !(str1 != string.Empty) || !(str2 != string.Empty) ? (object) string.Empty : (object) $"{str1} <- {str2}";
    }
  }
}
