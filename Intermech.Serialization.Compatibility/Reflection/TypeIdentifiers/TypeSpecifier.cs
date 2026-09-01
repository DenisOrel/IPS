// Decompiled with JetBrains decompiler
// Type: Intermech.Reflection.TypeIdentifiers.TypeSpecifier
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Text;

#nullable disable
namespace Intermech.Reflection.TypeIdentifiers;

public struct TypeSpecifier : IEquatable<TypeSpecifier>
{
  public static readonly TypeSpecifier Reference = new TypeSpecifier(TypeSpecifierKind.Reference, 0);
  public static readonly TypeSpecifier Pointer = new TypeSpecifier(TypeSpecifierKind.Pointer, 0);

  public static TypeSpecifier Array(int rank) => new TypeSpecifier(TypeSpecifierKind.Array, rank);

  private TypeSpecifier(TypeSpecifierKind kind, int arrayRank)
  {
    this.Kind = kind;
    this.ArrayRank = arrayRank;
  }

  public TypeSpecifierKind Kind { get; }

  public int ArrayRank { get; }

  public override string ToString()
  {
    switch (this.Kind)
    {
      case TypeSpecifierKind.Pointer:
        return "*";
      case TypeSpecifierKind.Reference:
        return "&";
      case TypeSpecifierKind.Array:
        if (this.ArrayRank == 1)
          return "[]";
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append('[');
        for (int index = 1; index < this.ArrayRank; ++index)
          stringBuilder.Append(',');
        stringBuilder.Append(']');
        return stringBuilder.ToString();
      default:
        return "<unknown specifier>";
    }
  }

  public bool Equals(TypeSpecifier other)
  {
    return this.Kind.Equals((object) other.Kind) && this.ArrayRank.Equals(other.ArrayRank);
  }

  public override bool Equals(object obj) => obj is TypeSpecifier other && this.Equals(other);

  public override int GetHashCode()
  {
    return (29 * 31 /*0x1F*/ + this.Kind.GetHashCode()) * 31 /*0x1F*/ + this.ArrayRank.GetHashCode();
  }
}
