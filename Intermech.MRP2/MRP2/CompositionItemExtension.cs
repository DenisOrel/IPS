// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.CompositionItemExtension
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces.Pdm;
using Intermech.Kernel.Search;
using System;

#nullable disable
namespace Intermech.MRP2;

internal static class CompositionItemExtension
{
  private const int _replacedObjectIdAttribute = -111;
  private const int _replacedRelationTypeIdAttribute = -112;

  public static void UpdateNewVersion(this CompositionItem item, CompositionItem newItem)
  {
    item.SetAttributeValue(-111, (object) newItem.ObjectID);
    item.SetAttributeValue(-112, (object) newItem.RelationTypeID);
    item.SetAttributeValue(-50, (object) newItem.Caption);
    item.SetAttributeValue(-5, (object) newItem.Version);
  }

  public static long ReplacedObjectID(this CompositionItem item)
  {
    int index = item.Attributes.FindIndex((Predicate<CompositionItemAttribute>) (x => x.AttributeID == -111));
    return index == -1 ? 0L : Convert.ToInt64(item.Attributes[index].Value);
  }

  public static long ReplacedRelationTypeID(this CompositionItem item)
  {
    int index = item.Attributes.FindIndex((Predicate<CompositionItemAttribute>) (x => x.AttributeID == -112));
    return index == -1 ? 0L : Convert.ToInt64(item.Attributes[index].Value);
  }

  public static void SetAttributeValue(this CompositionItem item, int attributeId, object value)
  {
    CompositionItemAttribute compositionItemAttribute = new CompositionItemAttribute(attributeId, AttributeSourceTypes.Object, value);
    int index = item.Attributes.FindIndex((Predicate<CompositionItemAttribute>) (x => x.AttributeID == attributeId));
    if (index != -1)
      item.Attributes[index] = compositionItemAttribute;
    else
      item.Attributes.Add(compositionItemAttribute);
  }
}
