// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.StTypeInfoService
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using Intermech.ApplicationModel;
using Intermech.Reflection.TypeIdentifiers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization;

public sealed class StTypeInfoService
{
  private ConcurrentDictionary<Type, StTypeInfo> typeToTypeInfoTable;
  private readonly Func<Type, StTypeInfo> createTypeInfoFromTypeMethod;
  private ConcurrentDictionary<string, StTypeInfo> typeNameToTypeInfoTable;
  private readonly Func<string, StTypeInfo> createTypeInfoFromTypeNameMethod;
  private ConcurrentDictionary<StTypeInfo, Type> typeInfoToTypeTable;
  private readonly Func<StTypeInfo, Type> createTypeMethod;
  private readonly StTypeInfo arrayTemplateTypeInfo;
  private readonly ConcurrentQueue<TypeForwardingRule> typeForwardingRules;
  private static readonly StTypeInfoService defaultInstance = new StTypeInfoService();

  public StTypeInfoService()
  {
    this.typeToTypeInfoTable = new ConcurrentDictionary<Type, StTypeInfo>();
    this.createTypeInfoFromTypeMethod = new Func<Type, StTypeInfo>(this.CreateTypeInfoFromTypeSlow);
    this.typeNameToTypeInfoTable = new ConcurrentDictionary<string, StTypeInfo>();
    this.createTypeInfoFromTypeNameMethod = new Func<string, StTypeInfo>(this.CreateTypeInfoFromTypeNameSlow);
    this.typeInfoToTypeTable = new ConcurrentDictionary<StTypeInfo, Type>();
    this.createTypeMethod = new Func<StTypeInfo, Type>(this.CreateTypeSlow);
    this.arrayTemplateTypeInfo = this.GetTypeInfo(typeof (Array));
    this.typeForwardingRules = new ConcurrentQueue<TypeForwardingRule>();
    this.LoadDefaultCompatibilityRules();
  }

  public static StTypeInfoService Default
  {
    [DebuggerStepThrough] get => StTypeInfoService.defaultInstance;
  }

  private void LoadDefaultCompatibilityRules()
  {
    this.AddRule((TypeForwardingRule) new MaskPatternAssemblyForwardingRule(URTKind.NETCore, "System.Collections.BitArray", "System.Private.CoreLib, Version=*, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", "System.Collections, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"));
    this.AddRule((TypeForwardingRule) new MaskPatternAssemblyForwardingRule(URTKind.NETFX, "*", "System.Private.CoreLib, Version=*, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", URTAssemblyInfo.mscorlibAssemblyName));
    this.AddRule((TypeForwardingRule) new MaskPatternAssemblyForwardingRule(URTKind.NETFX, "System.ComponentModel.EditorBrowsableState", URTAssemblyInfo.mscorlibAssemblyName, "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
    this.AddRule((TypeForwardingRule) new MaskPatternAssemblyForwardingRule(URTKind.NETFX, "System.Data.*", "System.Data.Common, Version=*, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
  }

  public StTypeInfo ArrayTemplateTypeInfo
  {
    [DebuggerStepThrough] get => this.arrayTemplateTypeInfo;
  }

  public void AddRule(TypeForwardingRule rule)
  {
    if (rule == null)
      throw new ArgumentNullException(nameof (rule));
    this.typeForwardingRules.Enqueue(rule);
  }

  public StTypeInfo GetTypeInfo(string typeName, string assemblyName)
  {
    string key = typeName != null ? typeName : throw new ArgumentNullException(nameof (typeName));
    if (!string.IsNullOrEmpty(assemblyName))
      key = $"{typeName}, {assemblyName}";
    return this.typeNameToTypeInfoTable.GetOrAdd(key, this.createTypeInfoFromTypeNameMethod);
  }

  public StTypeInfo GetTypeInfo(string typeName)
  {
    if (typeName == null)
      throw new ArgumentNullException(nameof (typeName));
    return this.typeNameToTypeInfoTable.GetOrAdd(typeName, this.createTypeInfoFromTypeNameMethod);
  }

  private StTypeInfo CreateTypeInfoFromTypeNameSlow(string typeName)
  {
    TypeIdentifier parseResult = typeName != null ? TypeIdentifier.Parse(typeName) : throw new ArgumentNullException(nameof (typeName));
    if (parseResult.IsPointer || parseResult.IsReference)
      throw new SerializationException("The pointer and by-ref types is not supported.");
    if (parseResult.IsArray)
    {
      int arrayRank = parseResult.GetArrayRank();
      return (StTypeInfo) new StArrayTypeInfo(this.arrayTemplateTypeInfo, this.GetTypeInfo(parseResult.GetElementType().AssemblyQualifiedName), arrayRank);
    }
    return parseResult.IsGenericType && !parseResult.IsGenericTypeDefinition ? (StTypeInfo) new StBoundGenericTypeInfo(this.GetTypeInfo(parseResult.GetGenericTypeDefinition().AssemblyQualifiedName), parseResult.GenericArguments.Select<TypeIdentifier, StTypeInfo>((Func<TypeIdentifier, StTypeInfo>) (x => this.GetTypeInfo(x.AssemblyQualifiedName))).ToArray<StTypeInfo>()) : new StTypeInfo(parseResult.FullName, this.GetAssemblyNameFromParseResult(parseResult));
  }

  private string GetAssemblyNameFromParseResult(TypeIdentifier parseResult)
  {
    string str = parseResult.AssemblyName != null ? parseResult.AssemblyName.FullName : string.Empty;
    return this.GetAssemblyName(str, str);
  }

  public StTypeInfo GetTypeInfo(Type type)
  {
    if (type == (Type) null)
      throw new ArgumentNullException(nameof (type));
    return this.typeToTypeInfoTable.GetOrAdd(type, this.createTypeInfoFromTypeMethod);
  }

  private StTypeInfo CreateTypeInfoFromTypeSlow(Type type)
  {
    if (type.IsPointer || type.IsByRef)
      throw new SerializationException("The pointer and by-ref types is not supported.");
    if (type.IsArray)
    {
      int arrayRank = type.GetArrayRank();
      return (StTypeInfo) new StArrayTypeInfo(this.arrayTemplateTypeInfo, this.GetTypeInfo(type.GetElementType()), arrayRank);
    }
    if (!type.IsGenericType || type.IsGenericTypeDefinition)
      return new StTypeInfo(type.FullName, this.GetAssemblyNameFromType(type));
    return !type.ContainsGenericParameters ? (StTypeInfo) new StBoundGenericTypeInfo(this.GetTypeInfo(type.GetGenericTypeDefinition()), Array.ConvertAll<Type, StTypeInfo>(type.GetGenericArguments(), (Converter<Type, StTypeInfo>) (x => this.GetTypeInfo(x)))) : throw new SerializationException("The open generic types is not supported.");
  }

  private string GetAssemblyNameFromType(Type type)
  {
    string fullName = type.Assembly.FullName;
    return this.GetAssemblyName(this.TryGetForwardedAssemblyName(type) ?? fullName, fullName);
  }

  private string GetAssemblyName(string assemblyName, string originalName)
  {
    if (assemblyName == URTAssemblyInfo.AssemblyName)
      assemblyName = string.Empty;
    else if (assemblyName == URTAssemblyInfo.mscorlibAssemblyName && originalName == URTAssemblyInfo.AssemblyName)
      assemblyName = string.Empty;
    return assemblyName;
  }

  private string TryGetForwardedAssemblyName(Type type)
  {
    object[] customAttributes = type.GetCustomAttributes(typeof (TypeForwardedFromAttribute), false);
    int index = 0;
    return index < customAttributes.Length ? ((TypeForwardedFromAttribute) customAttributes[index]).AssemblyFullName : (string) null;
  }

  public Type GetType(StTypeInfo typeInfo)
  {
    if (typeInfo == null)
      throw new ArgumentNullException(nameof (typeInfo));
    return this.typeInfoToTypeTable.GetOrAdd(typeInfo, this.createTypeMethod);
  }

  public Type GetType(StTypeInfo typeInfo, bool throwOnError)
  {
    if (typeInfo == null)
      throw new ArgumentNullException(nameof (typeInfo));
    if (throwOnError)
      return this.typeInfoToTypeTable.GetOrAdd(typeInfo, this.createTypeMethod);
    Type type1;
    if (this.typeInfoToTypeTable.TryGetValue(typeInfo, out type1))
      return type1;
    Type type2 = this.GetTypeSlow(typeInfo, false);
    if (type2 != (Type) null)
      type2 = this.typeInfoToTypeTable.GetOrAdd(typeInfo, type2);
    return type2;
  }

  private Type CreateTypeSlow(StTypeInfo typeInfo) => this.GetTypeSlow(typeInfo, true);

  private Type GetTypeSlow(StTypeInfo typeInfo, bool throwOnError)
  {
    switch (typeInfo)
    {
      case StArrayTypeInfo stArrayTypeInfo:
        Type type1 = this.GetType(stArrayTypeInfo.ElementInfo, throwOnError);
        if (type1 == (Type) null)
          return (Type) null;
        return stArrayTypeInfo.Rank != 1 ? type1.MakeArrayType(stArrayTypeInfo.Rank) : type1.MakeArrayType();
      case StBoundGenericTypeInfo boundGenericTypeInfo:
        Type type2 = this.GetType(boundGenericTypeInfo.Definition, throwOnError);
        Type[] array = ((IEnumerable<StTypeInfo>) boundGenericTypeInfo.Arguments).Select<StTypeInfo, Type>((Func<StTypeInfo, Type>) (x => this.GetType(x, throwOnError))).ToArray<Type>();
        return type2 == (Type) null && ((IEnumerable<Type>) array).Any<Type>((Func<Type, bool>) (x => x == (Type) null)) ? (Type) null : type2.MakeGenericType(array);
      default:
        string typeName = typeInfo.TypeName;
        string assemblyName = typeInfo.AssemblyName != string.Empty ? typeInfo.AssemblyName : URTAssemblyInfo.AssemblyName;
        if (!this.typeForwardingRules.IsEmpty)
        {
          URTKind runtimeKind = URTAssemblyInfo.GetRuntimeKind();
          foreach (TypeForwardingRule typeForwardingRule in this.typeForwardingRules)
          {
            string resultTypeName;
            string resultAssemblyName;
            if (typeForwardingRule.RuntimeKind == runtimeKind && typeForwardingRule.TryApply(typeName, assemblyName, out resultTypeName, out resultAssemblyName))
            {
              typeName = resultTypeName;
              assemblyName = resultAssemblyName;
              break;
            }
          }
        }
        return Type.GetType($"{typeName}, {assemblyName}", new Func<AssemblyName, Assembly>(this.CompatAssemblyResolver), new Func<Assembly, string, bool, Type>(this.CompatTypeResolver), throwOnError);
    }
  }

  private Assembly CompatAssemblyResolver(AssemblyName assemblyName)
  {
    IAppAssemblyResolveFilter resolveFilter = AppAssemblyResolver.ResolveFilter;
    return resolveFilter != null && !resolveFilter.CanResolve(assemblyName) ? (Assembly) null : Assembly.Load(assemblyName);
  }

  private Type CompatTypeResolver(Assembly assembly, string typeName, bool throwOnError)
  {
    if (assembly == (Assembly) null)
      assembly = typeof (object).Assembly;
    return assembly.GetType(typeName, throwOnError);
  }
}
