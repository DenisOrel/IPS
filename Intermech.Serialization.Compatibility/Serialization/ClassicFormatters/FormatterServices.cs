// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.FormatterServices
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Text;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal static class FormatterServices
{
  private static readonly ConcurrentDictionary<MemberHolder, MemberInfo[]> s_memberInfoTable = new ConcurrentDictionary<MemberHolder, MemberInfo[]>();

  private static FieldInfo[] InternalGetSerializableMembers(Type type)
  {
    if (type.IsInterface)
      return Array.Empty<FieldInfo>();
    FieldInfo[] collection = type.IsSerializable ? FormatterServices.GetSerializableFields(type) : throw new SerializationException(SR.Format(SR2.Serialization_NonSerType, (object) type.FullName, (object) type.Assembly.FullName));
    List<FieldInfo> fieldInfoList = new List<FieldInfo>(collection.Length);
    fieldInfoList.AddRange((IEnumerable<FieldInfo>) collection);
    Type baseType = type.BaseType;
    if (baseType != (Type) null && baseType != typeof (object))
    {
      Type[] parentTypes1;
      int parentTypeCount;
      bool parentTypes2 = FormatterServices.GetParentTypes(baseType, out parentTypes1, out parentTypeCount);
      if (parentTypeCount > 0)
      {
        for (int index1 = 0; index1 < parentTypeCount; ++index1)
        {
          Type type1 = parentTypes1[index1];
          FieldInfo[] fieldInfoArray = type1.IsSerializable ? type1.GetFields(BindingFlags.Instance | BindingFlags.NonPublic) : throw new SerializationException(SR.Format(SR2.Serialization_NonSerType, (object) type1.FullName, (object) type1.Module.Assembly.FullName));
          string namePrefix = parentTypes2 ? type1.Name : type1.FullName;
          foreach (FieldInfo fieldInfo in fieldInfoArray)
          {
            FieldInfo field = fieldInfo;
            if (!field.IsNotSerialized)
            {
              if (!field.IsPrivate)
              {
                int index2 = fieldInfoList.FindIndex((Predicate<FieldInfo>) (x =>
                {
                  if (FormatterServices.FieldEquals(x, field))
                    return true;
                  return x is SerializationFieldInfo serializationFieldInfo2 && FormatterServices.FieldEquals(serializationFieldInfo2.FieldInfo, field);
                }));
                if (index2 >= 0)
                  fieldInfoList.RemoveAt(index2);
              }
              fieldInfoList.Add((FieldInfo) new SerializationFieldInfo(field, namePrefix));
            }
          }
        }
      }
    }
    return fieldInfoList.ToArray();
  }

  private static bool FieldEquals(FieldInfo x, FieldInfo y)
  {
    return x.Name == y.Name && x.DeclaringType == y.DeclaringType;
  }

  private static FieldInfo[] GetSerializableFields(Type type)
  {
    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    int length = 0;
    for (int index = 0; index < fields.Length; ++index)
    {
      if ((fields[index].Attributes & FieldAttributes.NotSerialized) != FieldAttributes.NotSerialized)
        ++length;
    }
    if (length == fields.Length)
      return fields;
    FieldInfo[] serializableFields = new FieldInfo[length];
    int index1 = 0;
    for (int index2 = 0; index2 < fields.Length; ++index2)
    {
      if ((fields[index2].Attributes & FieldAttributes.NotSerialized) != FieldAttributes.NotSerialized)
      {
        serializableFields[index1] = fields[index2];
        ++index1;
      }
    }
    return serializableFields;
  }

  private static bool GetParentTypes(
    Type parentType,
    out Type[] parentTypes,
    out int parentTypeCount)
  {
    parentTypes = (Type[]) null;
    parentTypeCount = 0;
    bool parentTypes1 = true;
    Type type1 = typeof (object);
    for (Type type2 = parentType; type2 != type1; type2 = type2.BaseType)
    {
      if (!type2.IsInterface)
      {
        string name1 = type2.Name;
        for (int index = 0; parentTypes1 && index < parentTypeCount; ++index)
        {
          string name2 = parentTypes[index].Name;
          if (name2.Length == name1.Length && (int) name2[0] == (int) name1[0] && name1 == name2)
          {
            parentTypes1 = false;
            break;
          }
        }
        if (parentTypes == null || parentTypeCount == parentTypes.Length)
          Array.Resize<Type>(ref parentTypes, Math.Max(parentTypeCount * 2, 12));
        parentTypes[parentTypeCount++] = type2;
      }
    }
    return parentTypes1;
  }

  public static MemberInfo[] GetSerializableMembers(Type type)
  {
    return FormatterServices.GetSerializableMembers(type, new StreamingContext(StreamingContextStates.All));
  }

  public static MemberInfo[] GetSerializableMembers(Type type, StreamingContext context)
  {
    if (type == (Type) null)
      throw new ArgumentNullException(nameof (type));
    return FormatterServices.s_memberInfoTable.GetOrAdd(new MemberHolder(type, context), (Func<MemberHolder, MemberInfo[]>) (mh => (MemberInfo[]) FormatterServices.InternalGetSerializableMembers(mh._memberType)));
  }

  public static void CheckTypeSecurity(Type t, TypeFilterLevel securityLevel)
  {
  }

  public static object GetUninitializedObject(Type type)
  {
    MethodInfo method = typeof (RuntimeHelpers).GetMethod(nameof (GetUninitializedObject), BindingFlags.Static | BindingFlags.Public);
    if (method == (MethodInfo) null)
      method = typeof (System.Runtime.Serialization.FormatterServices).GetMethod(nameof (GetUninitializedObject), BindingFlags.Static | BindingFlags.Public);
    return method.Invoke((object) null, new object[1]
    {
      (object) type
    });
  }

  public static object GetSafeUninitializedObject(Type type)
  {
    return FormatterServices.GetUninitializedObject(type);
  }

  internal static void SerializationSetValue(MemberInfo fi, object target, object value)
  {
    (fi as FieldInfo ?? throw new ArgumentException(SR2.Argument_InvalidFieldInfo)).SetValue(target, value);
  }

  public static object PopulateObjectMembers(object obj, MemberInfo[] members, object[] data)
  {
    if (obj == null)
      throw new ArgumentNullException(nameof (obj));
    if (members == null)
      throw new ArgumentNullException(nameof (members));
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (members.Length != data.Length)
      throw new ArgumentException(SR2.Argument_DataLengthDifferent);
    for (int p1 = 0; p1 < members.Length; ++p1)
    {
      MemberInfo member = members[p1];
      if (member == (MemberInfo) null)
        throw new ArgumentNullException(nameof (members), SR.Format(SR2.ArgumentNull_NullMember, (object) p1));
      if (data[p1] != null)
        (member as FieldInfo ?? throw new SerializationException(SR2.Serialization_UnknownMemberInfo)).SetValue(obj, data[p1]);
    }
    return obj;
  }

  public static object[] GetObjectData(object obj, MemberInfo[] members)
  {
    if (obj == null)
      throw new ArgumentNullException(nameof (obj));
    object[] objectData = members != null ? new object[members.Length] : throw new ArgumentNullException(nameof (members));
    for (int p1 = 0; p1 < members.Length; ++p1)
    {
      MemberInfo member = members[p1];
      FieldInfo fieldInfo = !(member == (MemberInfo) null) ? member as FieldInfo : throw new ArgumentNullException(nameof (members), SR.Format(SR2.ArgumentNull_NullMember, (object) p1));
      objectData[p1] = !(fieldInfo == (FieldInfo) null) ? fieldInfo.GetValue(obj) : throw new SerializationException(SR2.Serialization_UnknownMemberInfo);
    }
    return objectData;
  }

  public static ISerializationSurrogate GetSurrogateForCyclicalReference(
    ISerializationSurrogate innerSurrogate)
  {
    return innerSurrogate != null ? (ISerializationSurrogate) new SurrogateForCyclicalReference(innerSurrogate) : throw new ArgumentNullException(nameof (innerSurrogate));
  }

  public static Type GetTypeFromAssembly(Assembly assem, string name)
  {
    return !(assem == (Assembly) null) ? assem.GetType(name, false, false) : throw new ArgumentNullException(nameof (assem));
  }

  internal static Assembly LoadAssemblyFromString(string assemblyName)
  {
    return Assembly.Load(new AssemblyName(assemblyName));
  }

  internal static Assembly LoadAssemblyFromStringNoThrow(string assemblyName)
  {
    try
    {
      return FormatterServices.LoadAssemblyFromString(assemblyName);
    }
    catch (Exception ex)
    {
    }
    return (Assembly) null;
  }

  internal static string GetClrAssemblyName(Type type, out bool hasTypeForwardedFrom)
  {
    Type type1 = !(type == (Type) null) ? type : throw new ArgumentNullException(nameof (type));
    while (type1.HasElementType)
      type1 = type1.GetElementType();
    object[] customAttributes = type1.GetCustomAttributes(typeof (TypeForwardedFromAttribute), false);
    int index = 0;
    if (index < customAttributes.Length)
    {
      Attribute attribute = (Attribute) customAttributes[index];
      hasTypeForwardedFrom = true;
      return ((TypeForwardedFromAttribute) attribute).AssemblyFullName;
    }
    hasTypeForwardedFrom = false;
    return type.Assembly.FullName;
  }

  internal static string GetClrTypeFullName(Type type)
  {
    return !type.IsArray ? FormatterServices.GetClrTypeFullNameForNonArrayTypes(type) : FormatterServices.GetClrTypeFullNameForArray(type);
  }

  private static string GetClrTypeFullNameForArray(Type type)
  {
    int arrayRank = type.GetArrayRank();
    string clrTypeFullName = FormatterServices.GetClrTypeFullName(type.GetElementType());
    return arrayRank != 1 ? $"{clrTypeFullName}[{new string(',', arrayRank - 1)}]" : clrTypeFullName + "[]";
  }

  private static string GetClrTypeFullNameForNonArrayTypes(Type type)
  {
    if (!type.IsGenericType)
      return type.FullName;
    StringBuilder stringBuilder = new StringBuilder(type.GetGenericTypeDefinition().FullName).Append('[');
    foreach (Type genericArgument in type.GetGenericArguments())
    {
      stringBuilder.Append('[').Append(FormatterServices.GetClrTypeFullName(genericArgument)).Append(", ");
      stringBuilder.Append(FormatterServices.GetClrAssemblyName(genericArgument, out bool _)).Append("],");
    }
    return stringBuilder.Remove(stringBuilder.Length - 1, 1).Append(']').ToString();
  }
}
