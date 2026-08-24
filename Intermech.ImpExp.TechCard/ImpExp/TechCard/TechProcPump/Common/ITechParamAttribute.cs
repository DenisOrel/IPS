// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.TechProcPump.Common.ITechParamAttribute
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface.DataWriter;
using System;

#nullable disable
namespace Intermech.ImpExp.TechCard.TechProcPump.Common;

public interface ITechParamAttribute : 
  ITechParamBase,
  IComparable<ITechParamBase>,
  IEquatable<ITechParamBase>
{
  IAttributeTypeItem AttributeType { get; }

  int Index { get; }

  string Caption { get; }

  EntitySetting.AttributeBelongs AttributeBelongs { get; }
}
