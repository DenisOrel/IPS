// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.WriteObjectInfo
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class WriteObjectInfo
{
  internal int _objectInfoId;
  internal object _obj;
  internal Type _objectType;
  internal bool _isSi;
  internal bool _isNamed;
  internal bool _isArray;
  internal SerializationInfo _si;
  internal SerObjectInfoCache _cache;
  internal object[] _memberData;
  internal ISerializationSurrogate _serializationSurrogate;
  internal StreamingContext _context;
  internal SerObjectInfoInit _serObjectInfoInit;
  internal long _objectId;
  internal long _assemId;
  private string _binderTypeName;
  private string _binderAssemblyString;

  internal WriteObjectInfo()
  {
  }

  internal void ObjectEnd() => WriteObjectInfo.PutObjectInfo(this._serObjectInfoInit, this);

  private void InternalInit()
  {
    this._obj = (object) null;
    this._objectType = (Type) null;
    this._isSi = false;
    this._isNamed = false;
    this._isArray = false;
    this._si = (SerializationInfo) null;
    this._cache = (SerObjectInfoCache) null;
    this._memberData = (object[]) null;
    this._objectId = 0L;
    this._assemId = 0L;
    this._binderTypeName = (string) null;
    this._binderAssemblyString = (string) null;
  }

  internal static WriteObjectInfo Serialize(
    object obj,
    ISurrogateSelector surrogateSelector,
    StreamingContext context,
    SerObjectInfoInit serObjectInfoInit,
    IFormatterConverter converter,
    ObjectWriter objectWriter,
    SerializationBinder binder)
  {
    WriteObjectInfo objectInfo = WriteObjectInfo.GetObjectInfo(serObjectInfoInit);
    objectInfo.InitSerialize(obj, surrogateSelector, context, serObjectInfoInit, converter, objectWriter, binder);
    return objectInfo;
  }

  internal void InitSerialize(
    object obj,
    ISurrogateSelector surrogateSelector,
    StreamingContext context,
    SerObjectInfoInit serObjectInfoInit,
    IFormatterConverter converter,
    ObjectWriter objectWriter,
    SerializationBinder binder)
  {
    this._context = context;
    this._obj = obj;
    this._serObjectInfoInit = serObjectInfoInit;
    this._objectType = obj.GetType();
    if (this._objectType.IsArray)
    {
      this._isArray = true;
      this.InitNoMembers();
    }
    else
    {
      this.InvokeSerializationBinder(binder);
      objectWriter.ObjectManager.RegisterObject(obj);
      if (surrogateSelector != null && (this._serializationSurrogate = surrogateSelector.GetSurrogate(this._objectType, context, out ISurrogateSelector _)) != null)
      {
        this._si = new SerializationInfo(this._objectType, converter);
        if (!this._objectType.IsPrimitive)
          this._serializationSurrogate.GetObjectData(obj, this._si, context);
        this.InitSiWrite();
      }
      else if (obj is ISerializable)
      {
        this._si = this._objectType.IsSerializable ? new SerializationInfo(this._objectType, converter) : throw new SerializationException(SR.Format(SR2.Serialization_NonSerType, (object) this._objectType.FullName, (object) this._objectType.Assembly.FullName));
        ((ISerializable) obj).GetObjectData(this._si, context);
        this.InitSiWrite();
        WriteObjectInfo.CheckTypeForwardedFrom(this._cache, this._objectType, this._binderAssemblyString);
      }
      else
      {
        this.InitMemberInfo();
        WriteObjectInfo.CheckTypeForwardedFrom(this._cache, this._objectType, this._binderAssemblyString);
      }
    }
  }

  internal static WriteObjectInfo Serialize(
    Type objectType,
    ISurrogateSelector surrogateSelector,
    StreamingContext context,
    SerObjectInfoInit serObjectInfoInit,
    IFormatterConverter converter,
    SerializationBinder binder)
  {
    WriteObjectInfo objectInfo = WriteObjectInfo.GetObjectInfo(serObjectInfoInit);
    objectInfo.InitSerialize(objectType, surrogateSelector, context, serObjectInfoInit, converter, binder);
    return objectInfo;
  }

  internal void InitSerialize(
    Type objectType,
    ISurrogateSelector surrogateSelector,
    StreamingContext context,
    SerObjectInfoInit serObjectInfoInit,
    IFormatterConverter converter,
    SerializationBinder binder)
  {
    this._objectType = objectType;
    this._context = context;
    this._serObjectInfoInit = serObjectInfoInit;
    if (objectType.IsArray)
    {
      this.InitNoMembers();
    }
    else
    {
      this.InvokeSerializationBinder(binder);
      if (surrogateSelector != null)
        this._serializationSurrogate = surrogateSelector.GetSurrogate(objectType, context, out ISurrogateSelector _);
      if (this._serializationSurrogate != null)
      {
        this._si = new SerializationInfo(objectType, converter);
        this._cache = new SerObjectInfoCache(objectType);
        this._isSi = true;
      }
      else if ((object) objectType != (object) Converter.s_typeofObject && Converter.s_typeofISerializable.IsAssignableFrom(objectType))
      {
        this._si = new SerializationInfo(objectType, converter);
        this._cache = new SerObjectInfoCache(objectType);
        WriteObjectInfo.CheckTypeForwardedFrom(this._cache, objectType, this._binderAssemblyString);
        this._isSi = true;
      }
      if (this._isSi)
        return;
      this.InitMemberInfo();
      WriteObjectInfo.CheckTypeForwardedFrom(this._cache, objectType, this._binderAssemblyString);
    }
  }

  private void InitSiWrite()
  {
    SerializationInfoEnumerator serializationInfoEnumerator = (SerializationInfoEnumerator) null;
    this._isSi = true;
    serializationInfoEnumerator = this._si.GetEnumerator();
    int memberCount = this._si.MemberCount;
    TypeInformation typeInformation = (TypeInformation) null;
    string fullTypeName = this._si.FullTypeName;
    string assemblyName = this._si.AssemblyName;
    bool hasTypeForwardedFrom = false;
    if (!this._si.IsFullTypeNameSetExplicit)
    {
      typeInformation = BinaryFormatter.GetTypeInformation(this._si.ObjectType);
      fullTypeName = typeInformation.FullTypeName;
      hasTypeForwardedFrom = typeInformation.HasTypeForwardedFrom;
    }
    if (!this._si.IsAssemblyNameSetExplicit)
    {
      if (typeInformation == null)
        typeInformation = BinaryFormatter.GetTypeInformation(this._si.ObjectType);
      assemblyName = typeInformation.AssemblyString;
      hasTypeForwardedFrom = typeInformation.HasTypeForwardedFrom;
    }
    this._cache = new SerObjectInfoCache(fullTypeName, assemblyName, hasTypeForwardedFrom);
    this._cache._memberNames = new string[memberCount];
    this._cache._memberTypes = new Type[memberCount];
    this._memberData = new object[memberCount];
    SerializationInfoEnumerator enumerator = this._si.GetEnumerator();
    int index = 0;
    while (enumerator.MoveNext())
    {
      this._cache._memberNames[index] = enumerator.Name;
      this._cache._memberTypes[index] = enumerator.ObjectType;
      this._memberData[index] = enumerator.Value;
      ++index;
    }
    this._isNamed = true;
  }

  private static void CheckTypeForwardedFrom(
    SerObjectInfoCache cache,
    Type objectType,
    string binderAssemblyString)
  {
  }

  private void InitNoMembers()
  {
    if (this._serObjectInfoInit._seenBeforeTable.TryGetValue(this._objectType, out this._cache))
      return;
    this._cache = new SerObjectInfoCache(this._objectType);
    this._serObjectInfoInit._seenBeforeTable.Add(this._objectType, this._cache);
  }

  private void InitMemberInfo()
  {
    if (!this._serObjectInfoInit._seenBeforeTable.TryGetValue(this._objectType, out this._cache))
    {
      this._cache = new SerObjectInfoCache(this._objectType);
      this._cache._memberInfos = Intermech.Serialization.ClassicFormatters.FormatterServices.GetSerializableMembers(this._objectType, this._context);
      int length = this._cache._memberInfos.Length;
      this._cache._memberNames = new string[length];
      this._cache._memberTypes = new Type[length];
      for (int index = 0; index < length; ++index)
      {
        this._cache._memberNames[index] = this._cache._memberInfos[index].Name;
        this._cache._memberTypes[index] = ((FieldInfo) this._cache._memberInfos[index]).FieldType;
      }
      this._serObjectInfoInit._seenBeforeTable.Add(this._objectType, this._cache);
    }
    if (this._obj != null)
      this._memberData = Intermech.Serialization.ClassicFormatters.FormatterServices.GetObjectData(this._obj, this._cache._memberInfos);
    this._isNamed = true;
  }

  internal string GetTypeFullName() => this._binderTypeName ?? this._cache._fullTypeName;

  internal string GetAssemblyString() => this._binderAssemblyString ?? this._cache._assemblyString;

  private void InvokeSerializationBinder(SerializationBinder binder)
  {
    BinaryFormatterEventSource.Log.SerializingObject(this._objectType);
    binder?.BindToName(this._objectType, out this._binderAssemblyString, out this._binderTypeName);
  }

  internal void GetMemberInfo(
    out string[] outMemberNames,
    out Type[] outMemberTypes,
    out object[] outMemberData)
  {
    outMemberNames = this._cache._memberNames;
    outMemberTypes = this._cache._memberTypes;
    outMemberData = this._memberData;
    if (this._isSi && !this._isNamed)
      throw new SerializationException(SR2.Serialization_ISerializableMemberInfo);
  }

  private static WriteObjectInfo GetObjectInfo(SerObjectInfoInit serObjectInfoInit)
  {
    WriteObjectInfo objectInfo;
    if (!serObjectInfoInit._oiPool.IsEmpty())
    {
      objectInfo = (WriteObjectInfo) serObjectInfoInit._oiPool.Pop();
      objectInfo.InternalInit();
    }
    else
    {
      objectInfo = new WriteObjectInfo();
      objectInfo._objectInfoId = serObjectInfoInit._objectInfoIdCount++;
    }
    return objectInfo;
  }

  private static void PutObjectInfo(SerObjectInfoInit serObjectInfoInit, WriteObjectInfo objectInfo)
  {
    serObjectInfoInit._oiPool.Push((object) objectInfo);
  }
}
