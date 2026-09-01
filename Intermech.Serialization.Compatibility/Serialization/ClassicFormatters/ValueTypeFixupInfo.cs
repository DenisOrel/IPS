// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.ValueTypeFixupInfo
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Reflection;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class ValueTypeFixupInfo
{
  private readonly long _containerID;
  private readonly FieldInfo _parentField;
  private readonly int[] _parentIndex;

  public ValueTypeFixupInfo(long containerID, FieldInfo member, int[] parentIndex)
  {
    if (member == (FieldInfo) null && parentIndex == null)
      throw new ArgumentException(SR2.Argument_MustSupplyParent);
    if (containerID == 0L && member == (FieldInfo) null)
    {
      this._containerID = containerID;
      this._parentField = member;
      this._parentIndex = parentIndex;
    }
    if (member != (FieldInfo) null)
    {
      if (parentIndex != null)
        throw new ArgumentException(SR2.Argument_MemberAndArray);
      if (member.FieldType.IsValueType && containerID == 0L)
        throw new ArgumentException(SR2.Argument_MustSupplyContainer);
    }
    this._containerID = containerID;
    this._parentField = member;
    this._parentIndex = parentIndex;
  }

  public long ContainerID => this._containerID;

  public FieldInfo ParentField => this._parentField;

  public int[] ParentIndex => this._parentIndex;
}
