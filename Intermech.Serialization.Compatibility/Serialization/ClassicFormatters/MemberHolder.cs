// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.MemberHolder
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class MemberHolder
{
  internal readonly Type _memberType;
  internal readonly StreamingContext _context;

  internal MemberHolder(Type type, StreamingContext ctx)
  {
    this._memberType = type;
    this._context = ctx;
  }

  public override int GetHashCode() => this._memberType.GetHashCode();

  public override bool Equals(object obj)
  {
    return obj is MemberHolder memberHolder && (object) memberHolder._memberType == (object) this._memberType && memberHolder._context.State == this._context.State;
  }
}
