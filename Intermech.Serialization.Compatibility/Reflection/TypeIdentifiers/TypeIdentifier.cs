// Decompiled with JetBrains decompiler
// Type: Intermech.Reflection.TypeIdentifiers.TypeIdentifier
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

#nullable disable
namespace Intermech.Reflection.TypeIdentifiers;

[Serializable]
public class TypeIdentifier : IEquatable<TypeIdentifier>
{
  private List<string> m_nestedTypeName;
  private IList<TypeIdentifier> m_genericArguments;

  private TypeIdentifier(
    string namespaceName,
    List<string> nestedTypeName,
    IList<TypeSpecifier> typeSpecifiers,
    IList<TypeIdentifier> genericArguments,
    AssemblyName assemblyName)
  {
    this.TypeSpecifiers = typeSpecifiers ?? (IList<TypeSpecifier>) new List<TypeSpecifier>();
    this.Namespace = namespaceName;
    this.m_nestedTypeName = nestedTypeName ?? throw new ArgumentNullException(nameof (nestedTypeName));
    this.m_genericArguments = genericArguments;
    this.AssemblyName = assemblyName;
  }

  private TypeIdentifier(TypeIdentifier other)
  {
    this.TypeSpecifiers = (IList<TypeSpecifier>) new List<TypeSpecifier>((IEnumerable<TypeSpecifier>) other.TypeSpecifiers);
    this.m_genericArguments = TypeIdentifier.CloneGenericArguments(other.m_genericArguments);
    this.AssemblyName = other.AssemblyName;
    this.Namespace = other.Namespace;
    this.m_nestedTypeName = new List<string>((IEnumerable<string>) other.m_nestedTypeName);
  }

  internal TypeIdentifier(string typeName, string namespaceName, AssemblyName assemblyName)
  {
    string namespaceName1 = namespaceName;
    List<string> nestedTypeName = new List<string>();
    nestedTypeName.Add(typeName);
    AssemblyName assemblyName1 = assemblyName;
    // ISSUE: explicit constructor call
    this.\u002Ector(namespaceName1, nestedTypeName, (IList<TypeSpecifier>) null, (IList<TypeIdentifier>) null, assemblyName1);
  }

  private static IList<TypeIdentifier> CloneGenericArguments(IList<TypeIdentifier> genericArguments)
  {
    return genericArguments == null ? (IList<TypeIdentifier>) null : (IList<TypeIdentifier>) new List<TypeIdentifier>(genericArguments.Select<TypeIdentifier, TypeIdentifier>((Func<TypeIdentifier, TypeIdentifier>) (arg => new TypeIdentifier(arg))));
  }

  public AssemblyName AssemblyName { get; set; }

  public IList<TypeSpecifier> TypeSpecifiers { get; private set; }

  public string Namespace { get; set; }

  public string NamespaceTypeName
  {
    get
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!string.IsNullOrEmpty(this.Namespace))
      {
        stringBuilder.Append(this.Namespace);
        stringBuilder.Append('.');
      }
      stringBuilder.Append(string.Join("+", (IEnumerable<string>) this.m_nestedTypeName));
      return stringBuilder.ToString();
    }
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value is empty.", nameof (value));
        default:
          (this.Namespace, this.m_nestedTypeName) = TypeIdentifier.ParseNamespaceTypeName(new TypeIdentifier.CharReader(value), false);
          break;
      }
    }
  }

  public string AssemblyQualifiedName
  {
    get
    {
      StringBuilder result = new StringBuilder();
      this.BuildTypeFullName(result);
      if (this.AssemblyName != null)
      {
        result.Append(", ");
        result.Append(this.AssemblyName.FullName);
      }
      return result.ToString();
    }
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value is empty.", nameof (value));
        default:
          (this.Namespace, this.m_nestedTypeName, this.m_genericArguments, this.TypeSpecifiers, this.AssemblyName) = TypeIdentifier.ParseAssemblyQualifiedName(new TypeIdentifier.CharReader(value), false);
          break;
      }
    }
  }

  public IList<TypeIdentifier> GenericArguments
  {
    get
    {
      if (this.m_genericArguments == null)
        this.m_genericArguments = (IList<TypeIdentifier>) new List<TypeIdentifier>();
      return this.m_genericArguments;
    }
  }

  public string FullName
  {
    get
    {
      StringBuilder result = new StringBuilder();
      this.BuildTypeFullName(result);
      return result.ToString();
    }
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value is empty.", nameof (value));
        default:
          (this.Namespace, this.m_nestedTypeName, this.m_genericArguments, this.TypeSpecifiers) = TypeIdentifier.ParseFullName(new TypeIdentifier.CharReader(value), false);
          break;
      }
    }
  }

  public string Name
  {
    get => this.m_nestedTypeName.LastOrDefault<string>();
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value is empty.", nameof (value));
        default:
          this.m_nestedTypeName[this.m_nestedTypeName.Count - 1] = value;
          break;
      }
    }
  }

  public bool IsArray
  {
    get
    {
      return this.TypeSpecifiers.Count > 0 && this.TypeSpecifiers[this.TypeSpecifiers.Count - 1].Kind == TypeSpecifierKind.Array;
    }
  }

  public bool IsPointer
  {
    get
    {
      return this.TypeSpecifiers.Count > 0 && this.TypeSpecifiers[this.TypeSpecifiers.Count - 1].Kind == TypeSpecifierKind.Pointer;
    }
  }

  public bool IsReference
  {
    get
    {
      return this.TypeSpecifiers.Count > 0 && this.TypeSpecifiers[this.TypeSpecifiers.Count - 1].Kind == TypeSpecifierKind.Reference;
    }
  }

  public bool IsGenericType => this.NamespaceTypeName.Contains<char>('`');

  public bool IsGenericTypeDefinition => this.IsGenericType && this.GenericArguments.Count == 0;

  public TypeIdentifier GetElementType()
  {
    return this.TypeSpecifiers.Count == 0 ? (TypeIdentifier) null : new TypeIdentifier(this.Namespace, new List<string>((IEnumerable<string>) this.m_nestedTypeName), (IList<TypeSpecifier>) this.TypeSpecifiers.Take<TypeSpecifier>(this.TypeSpecifiers.Count - 1).ToList<TypeSpecifier>(), TypeIdentifier.CloneGenericArguments(this.m_genericArguments), this.AssemblyName);
  }

  public TypeIdentifier GetDeclaringType()
  {
    return this.m_nestedTypeName.Count <= 1 ? (TypeIdentifier) null : new TypeIdentifier(this.Namespace, this.m_nestedTypeName.GetRange(0, this.m_nestedTypeName.Count - 1), (IList<TypeSpecifier>) this.TypeSpecifiers.ToList<TypeSpecifier>(), TypeIdentifier.CloneGenericArguments(this.m_genericArguments), this.AssemblyName);
  }

  public TypeIdentifier GetGenericTypeDefinition()
  {
    if (this.IsGenericType)
      return new TypeIdentifier(this.Namespace, new List<string>((IEnumerable<string>) this.m_nestedTypeName), (IList<TypeSpecifier>) this.TypeSpecifiers.ToList<TypeSpecifier>(), (IList<TypeIdentifier>) null, this.AssemblyName);
    throw new ArgumentException();
  }

  public int GetArrayRank()
  {
    if (this.IsArray)
      return this.TypeSpecifiers[this.TypeSpecifiers.Count - 1].ArrayRank;
    throw new ArgumentException();
  }

  public static TypeIdentifier Parse(string typeName)
  {
    switch (typeName)
    {
      case null:
        throw new ArgumentNullException(nameof (typeName));
      case "":
        throw new ArgumentException("typeName must not be empty.", nameof (typeName));
      default:
        return TypeIdentifier.ParseTypeIdentifier(new TypeIdentifier.CharReader(typeName), true);
    }
  }

  private void BuildTypeFullName(StringBuilder result)
  {
    result.Append(this.NamespaceTypeName);
    if (this.m_genericArguments != null && this.m_genericArguments.Count > 0)
    {
      result.Append('[');
      for (int index = 0; index < this.m_genericArguments.Count; ++index)
      {
        if (index > 0)
          result.Append(',');
        if (this.m_genericArguments[index].AssemblyName != null)
          result.Append('[');
        result.Append(this.m_genericArguments[index].AssemblyQualifiedName);
        if (this.m_genericArguments[index].AssemblyName != null)
          result.Append(']');
      }
      result.Append(']');
    }
    if (this.TypeSpecifiers == null)
      return;
    foreach (TypeSpecifier typeSpecifier in (IEnumerable<TypeSpecifier>) this.TypeSpecifiers)
      result.Append(typeSpecifier.ToString());
  }

  private static TypeIdentifier ParseTypeIdentifier(
    TypeIdentifier.CharReader reader,
    bool fullyQualified)
  {
    if (fullyQualified)
    {
      (string Namespace, List<string> NestedTypeName, IList<TypeIdentifier> GenericArguments, IList<TypeSpecifier> Specifiers, AssemblyName AssemblyName) assemblyQualifiedName = TypeIdentifier.ParseAssemblyQualifiedName(reader, true);
      return new TypeIdentifier(assemblyQualifiedName.Namespace, assemblyQualifiedName.NestedTypeName, assemblyQualifiedName.Specifiers, assemblyQualifiedName.GenericArguments, assemblyQualifiedName.AssemblyName);
    }
    (string Namespace, List<string> NestedTypeName, IList<TypeIdentifier> GenericArguments, IList<TypeSpecifier> Specifiers) fullName = TypeIdentifier.ParseFullName(reader, true);
    return new TypeIdentifier(fullName.Namespace, fullName.NestedTypeName, fullName.Specifiers, fullName.GenericArguments, (AssemblyName) null);
  }

  private static (string Namespace, List<string> NestedTypeName, IList<TypeIdentifier> GenericArguments, IList<TypeSpecifier> Specifiers) ParseFullName(
    TypeIdentifier.CharReader reader,
    bool allowTrailingCharacters)
  {
    (string Namespace, List<string> NestedTypeName) namespaceTypeName = TypeIdentifier.ParseNamespaceTypeName(reader, true);
    int num = reader.Peek(1);
    IList<TypeIdentifier> genericArguments = reader.Peek() != 91 || num == 44 || num == 42 || num == 93 ? (IList<TypeIdentifier>) null : TypeIdentifier.ParseGenericArguments(reader);
    IList<TypeSpecifier> refPtrArrSpec = TypeIdentifier.ParseRefPtrArrSpec(reader);
    if (!allowTrailingCharacters && reader.HasMore)
      throw new TypeNameParserException($"Invalid type name \"{reader.Data}\"; Unexpected character '{(ValueType) (char) reader.Peek()}' at position {reader.Position}; expected end-of-string.");
    return (namespaceTypeName.Namespace, namespaceTypeName.NestedTypeName, genericArguments, refPtrArrSpec);
  }

  private static (string Namespace, List<string> NestedTypeName, IList<TypeIdentifier> GenericArguments, IList<TypeSpecifier> Specifiers, AssemblyName AssemblyName) ParseAssemblyQualifiedName(
    TypeIdentifier.CharReader reader,
    bool allowTrailingCharacters)
  {
    (string Namespace, List<string> NestedTypeName, IList<TypeIdentifier> GenericArguments, IList<TypeSpecifier> Specifiers) fullName = TypeIdentifier.ParseFullName(reader, true);
    AssemblyName assemblyName = (AssemblyName) null;
    if (reader.Peek() == 44)
    {
      reader.Read();
      TypeIdentifier.SkipWhitespace(reader);
      assemblyName = TypeIdentifier.ParseAssemblyName(reader);
    }
    if (!allowTrailingCharacters && reader.HasMore)
      throw new TypeNameParserException($"Invalid type name \"{reader.Data}\"; Unexpected character '{(ValueType) (char) reader.Peek()}' at position {reader.Position}; expected end-of-string.");
    return (fullName.Namespace, fullName.NestedTypeName, fullName.GenericArguments, fullName.Specifiers, assemblyName);
  }

  private static AssemblyName ParseAssemblyName(TypeIdentifier.CharReader reader)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (TypeIdentifier.ConsumeAssemblyName(reader, stringBuilder) == 0)
      throw new ArgumentException("Invalid type name; Expected assembly name.");
    TypeIdentifier.ConsumeWhitespace(reader, stringBuilder);
    if (reader.Peek() == 44)
    {
      stringBuilder.Append((char) reader.Read());
      TypeIdentifier.ConsumeWhitespace(reader, stringBuilder);
      TypeIdentifier.ConsumeAssemblyNameProperties(reader, stringBuilder);
    }
    return new AssemblyName(stringBuilder.ToString());
  }

  private static void ConsumeAssemblyNameProperties(
    TypeIdentifier.CharReader reader,
    StringBuilder assemblyName)
  {
    TypeIdentifier.ConsumeWhitespace(reader, assemblyName);
    while (TypeIdentifier.ConsumeAssemblyNamePropertyName(reader, assemblyName))
    {
      TypeIdentifier.ConsumeWhitespace(reader, assemblyName);
      if (reader.Peek() != 61)
        throw new ArgumentException("Invalid type name; Missing value for assembly name property.");
      assemblyName.Append((char) reader.Read());
      TypeIdentifier.ConsumeAssemblyNamePropertyValue(reader, assemblyName);
      TypeIdentifier.ConsumeWhitespace(reader, assemblyName);
      if (reader.Peek() != 44)
        break;
      assemblyName.Append((char) reader.Read());
    }
  }

  private static void ConsumeAssemblyNamePropertyValue(
    TypeIdentifier.CharReader reader,
    StringBuilder target)
  {
    if (reader.Peek() == 34)
      TypeIdentifier.ConsumeQuotedValue(reader, target);
    else
      TypeIdentifier.ConsumeUnquotedValue(reader, target);
  }

  private static void ConsumeQuotedValue(TypeIdentifier.CharReader reader, StringBuilder target)
  {
    target.Append((char) reader.Read());
    int num;
    while ((num = reader.Peek()) != -1)
    {
      target.Append((char) reader.Read());
      if (num == 34)
        break;
    }
    if (num != 34)
      throw new ArgumentException("Invalid type name; Missing closing quote in assembly name property value.");
  }

  private static void ConsumeUnquotedValue(TypeIdentifier.CharReader reader, StringBuilder target)
  {
    int c;
    while ((c = reader.Peek()) != -1 && c != 44 && c != 93 && !char.IsWhiteSpace((char) c))
      target.Append((char) reader.Read());
  }

  private static bool ConsumeAssemblyNamePropertyName(
    TypeIdentifier.CharReader reader,
    StringBuilder target)
  {
    int length = target.Length;
    TypeIdentifier.ConsumeWhitespace(reader, target);
    int c;
    while ((c = reader.Peek()) != -1 && c != 61 && !char.IsWhiteSpace((char) c) && c != 44)
      target.Append((char) reader.Read());
    return target.Length > length;
  }

  private static void ConsumeWhitespace(TypeIdentifier.CharReader reader, StringBuilder target)
  {
    int c;
    while ((c = reader.Peek()) != -1 && char.IsWhiteSpace((char) c))
      target.Append((char) reader.Read());
  }

  private static int ConsumeAssemblyName(TypeIdentifier.CharReader reader, StringBuilder target)
  {
    int position = reader.Position;
    int num;
    while ((num = reader.Peek()) != -1 && num != 44 && num != 93)
      target.Append((char) reader.Read());
    return reader.Position - position;
  }

  private static void SkipWhitespace(TypeIdentifier.CharReader reader)
  {
    int c;
    while ((c = reader.Peek()) != -1 && char.IsWhiteSpace((char) c))
      reader.Read();
  }

  private static IList<TypeIdentifier> ParseGenericArguments(TypeIdentifier.CharReader reader)
  {
    List<TypeIdentifier> genericArguments = new List<TypeIdentifier>();
    if (reader.Peek() == 91)
    {
      do
      {
        reader.Read();
        bool fullyQualified = false;
        if (reader.Peek() == 91)
        {
          fullyQualified = true;
          reader.Read();
        }
        genericArguments.Add(TypeIdentifier.ParseTypeIdentifier(reader, fullyQualified));
        if (fullyQualified)
        {
          if (reader.Peek() != 93)
            throw new TypeNameParserException($"Invalid type name \"{reader.Data}\"; Unexpected character '{(ValueType) (char) reader.Peek()}' at position {reader.Position}; expected ']'.");
          reader.Read();
        }
      }
      while (reader.Peek() == 44);
      reader.Read();
    }
    return (IList<TypeIdentifier>) genericArguments;
  }

  private static IList<TypeSpecifier> ParseRefPtrArrSpec(TypeIdentifier.CharReader reader)
  {
    List<TypeSpecifier> refPtrArrSpec = (List<TypeSpecifier>) null;
    int num;
    while ((num = reader.Peek()) != -1)
    {
      TypeSpecifier typeSpecifier;
      switch (num)
      {
        case 38:
          typeSpecifier = TypeSpecifier.Reference;
          reader.Read();
          break;
        case 42:
          typeSpecifier = TypeSpecifier.Pointer;
          reader.Read();
          break;
        case 44:
        case 93:
          return (IList<TypeSpecifier>) refPtrArrSpec;
        case 91:
          switch (reader.Peek(1))
          {
            case 42:
            case 44:
            case 93:
              typeSpecifier = TypeIdentifier.ParseArraySpecifier(reader);
              break;
            default:
              return (IList<TypeSpecifier>) refPtrArrSpec;
          }
          break;
        default:
          throw new TypeNameParserException($"Invalid type name \"{reader.Data}\"; Unexpected character '{(ValueType) (char) num}' at position {reader.Position}; one of '[', '*', '&', ',', ']'.");
      }
      if (refPtrArrSpec == null)
        refPtrArrSpec = new List<TypeSpecifier>();
      refPtrArrSpec.Add(typeSpecifier);
    }
    return (IList<TypeSpecifier>) refPtrArrSpec;
  }

  private static TypeSpecifier ParseArraySpecifier(TypeIdentifier.CharReader reader)
  {
    int rank = 1;
    reader.Read();
    int num1;
    while (true)
    {
      num1 = reader.Read();
      switch (num1)
      {
        case 42:
          goto label_4;
        case 44:
          ++rank;
          continue;
        case 93:
          goto label_3;
        default:
          goto label_6;
      }
label_1:;
    }
label_3:
    return TypeSpecifier.Array(rank);
label_4:
    int num2 = reader.Peek();
    switch (num2)
    {
      case 44:
      case 93:
        goto label_1;
      default:
        throw new TypeNameParserException($"Invalid type name \"{reader.Data}\"; Unexpected character '{(num2 == -1 ? (object) "EOS" : (object) ((char) num2).ToString())}' at position {reader.Position}; expected one of ',', ']'.");
    }
label_6:
    throw new TypeNameParserException($"Invalid type name \"{reader.Data}\"; Unexpected character '{(ValueType) (char) num1}' at position {reader.Position}; one of ',', ']', '*'.");
  }

  private static (string Namespace, List<string> NestedTypeName) ParseNamespaceTypeName(
    TypeIdentifier.CharReader reader,
    bool allowTrailingCharacters)
  {
    List<string> stringList = new List<string>();
    StringBuilder target = new StringBuilder();
    int num = -1;
    while (reader.HasMore && TypeIdentifier.TryParseIdentifierInto(reader, target) && reader.Peek() == 46)
    {
      num = target.Length;
      target.Append('.');
      reader.Read();
    }
    if (target.Length == 0)
      throw new TypeNameParserException($"Failed to parse type name from \"{reader.Data}\"; Expected NamespaceTypeName, but none found.");
    stringList.Add(target.ToString(num + 1, target.Length - num - 1));
    target.Length = num == -1 ? 0 : num;
    while (reader.Peek() == 43)
    {
      reader.Read();
      stringList.Add(TypeIdentifier.ParseIdentifier(reader));
    }
    if (!allowTrailingCharacters && reader.HasMore)
      throw new TypeNameParserException($"Invalid type name \"{reader.Data}\"; Unexpected character '{(ValueType) (char) reader.Peek()}' at position {reader.Position}; expected end-of-string.");
    return (target.Length == 0 ? (string) null : target.ToString(), stringList);
  }

  private static IReadOnlyList<string> ParseNestedTypeName(TypeIdentifier.CharReader reader)
  {
    List<string> nestedTypeName = new List<string>();
    nestedTypeName.Add(TypeIdentifier.ParseIdentifier(reader));
    while (reader.Peek() == 43)
      nestedTypeName.Add(TypeIdentifier.ParseIdentifier(reader));
    return (IReadOnlyList<string>) nestedTypeName;
  }

  private static string ParseIdentifier(TypeIdentifier.CharReader reader)
  {
    StringBuilder target = new StringBuilder();
    return TypeIdentifier.TryParseIdentifierInto(reader, target) ? target.ToString() : throw new TypeNameParserException($"Invalid type name; Expected IDENTIFIER at position {reader.Position}.");
  }

  private static bool TryParseIdentifierInto(TypeIdentifier.CharReader reader, StringBuilder target)
  {
    int position = reader.Position;
    int num;
    while ((num = reader.Peek()) != -1 && (num == 92 || !TypeIdentifier.IsSpecialCharacter((char) num)))
    {
      reader.Read();
      if (num == 92 && reader.HasMore)
      {
        target.Append('\\');
        num = reader.Read();
      }
      target.Append((char) num);
    }
    return reader.Position > position;
  }

  private static bool IsSpecialCharacter(char ch)
  {
    switch (ch)
    {
      case '&':
      case '*':
      case '+':
      case ',':
      case '.':
      case '[':
      case '\\':
      case ']':
        return true;
      default:
        return false;
    }
  }

  public bool Equals(TypeIdentifier other)
  {
    if (other == null || !EqualityComparer<string>.Default.Equals(this.Namespace, other.Namespace) || !this.m_nestedTypeName.SequenceEqual<string>((IEnumerable<string>) other.m_nestedTypeName) || (this.m_genericArguments != null || other.m_genericArguments != null) && (this.m_genericArguments == null || !this.m_genericArguments.SequenceEqual<TypeIdentifier>((IEnumerable<TypeIdentifier>) other.m_genericArguments)) || !this.TypeSpecifiers.SequenceEqual<TypeSpecifier>((IEnumerable<TypeSpecifier>) other.TypeSpecifiers))
      return false;
    if (this.AssemblyName == null && other.AssemblyName == null)
      return true;
    return this.AssemblyName != null && this.AssemblyName.FullName.Equals(other.AssemblyName?.FullName);
  }

  public override bool Equals(object obj) => this.Equals(obj as TypeIdentifier);

  public override string ToString() => this.FullName;

  public override int GetHashCode() => this.Name.GetHashCode();

  private sealed class CharReader
  {
    public CharReader(string data) => this.Data = data;

    public int Position { get; private set; }

    public bool HasMore => this.Peek() != -1;

    public int Peek() => this.Position < this.Data.Length ? (int) this.Data[this.Position] : -1;

    public int Peek(int offset)
    {
      int index = this.Position + offset;
      return index < this.Data.Length ? (int) this.Data[index] : -1;
    }

    public int Read() => this.Position < this.Data.Length ? (int) this.Data[this.Position++] : -1;

    public string Data { get; }
  }
}
