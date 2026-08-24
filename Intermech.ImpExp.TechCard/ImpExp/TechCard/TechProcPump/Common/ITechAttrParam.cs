// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.ITechAttrParam
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

[Obsolete("Use ITechParamEntity instead")]
public interface ITechAttrParam : 
  ITechParamAttribute,
  ITechParamBase,
  IComparable<ITechParamBase>,
  IEquatable<ITechParamBase>
{
  IAttributeTypeItem AttrType { get; }

  byte AttrBelong { get; }
}
