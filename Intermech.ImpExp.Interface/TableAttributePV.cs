// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.TableAttributePV
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Interface;

[Serializable]
public class TableAttributePV
{
  public Guid AttributeGuid { get; private set; }

  public List<object> Values { get; set; }

  public TableAttributePV(Guid attributeGuid, List<object> values)
  {
    this.AttributeGuid = attributeGuid;
    this.Values = values;
  }
}
