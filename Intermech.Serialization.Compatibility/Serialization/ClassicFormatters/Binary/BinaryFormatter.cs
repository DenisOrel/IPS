// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryFormatter
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

public sealed class BinaryFormatter : IFormatter
{
  private static readonly ConcurrentDictionary<Type, TypeInformation> s_typeNameCache = new ConcurrentDictionary<Type, TypeInformation>();
  internal ISurrogateSelector _surrogates;
  internal StreamingContext _context;
  internal SerializationBinder _binder;
  internal FormatterTypeStyle _typeFormat = FormatterTypeStyle.TypesAlways;
  internal FormatterAssemblyStyle _assemblyFormat;
  internal TypeFilterLevel _securityLevel = TypeFilterLevel.Full;
  internal object[] _crossAppDomainArray;

  public object Deserialize(Stream serializationStream)
  {
    if (serializationStream == null)
      throw new ArgumentNullException(nameof (serializationStream));
    if (serializationStream.CanSeek && serializationStream.Length == 0L)
      throw new SerializationException(SR2.Serialization_Stream);
    InternalFE formatterEnums = new InternalFE()
    {
      _typeFormat = this._typeFormat,
      _serializerTypeEnum = InternalSerializerTypeE.Binary,
      _assemblyFormat = this._assemblyFormat,
      _securityLevel = this._securityLevel
    };
    ObjectReader objectReader = new ObjectReader(serializationStream, this._surrogates, this._context, formatterEnums, this._binder)
    {
      _crossAppDomainArray = this._crossAppDomainArray
    };
    try
    {
      BinaryFormatterEventSource.Log.DeserializationStart();
      BinaryParser serParser = new BinaryParser(serializationStream, objectReader);
      return objectReader.Deserialize(serParser);
    }
    catch (SerializationException ex)
    {
      throw;
    }
    catch (Exception ex)
    {
      throw new SerializationException(SR2.Serialization_CorruptedStream, ex);
    }
    finally
    {
      BinaryFormatterEventSource.Log.DeserializationStop();
    }
  }

  public void Serialize(Stream serializationStream, object graph)
  {
    if (serializationStream == null)
      throw new ArgumentNullException(nameof (serializationStream));
    InternalFE formatterEnums = new InternalFE()
    {
      _typeFormat = this._typeFormat,
      _serializerTypeEnum = InternalSerializerTypeE.Binary,
      _assemblyFormat = this._assemblyFormat
    };
    try
    {
      BinaryFormatterEventSource.Log.SerializationStart();
      ObjectWriter objectWriter = new ObjectWriter(this._surrogates, this._context, formatterEnums, this._binder);
      BinaryFormatterWriter serWriter = new BinaryFormatterWriter(serializationStream, objectWriter, this._typeFormat);
      objectWriter.Serialize(graph, serWriter);
      this._crossAppDomainArray = objectWriter._crossAppDomainArray;
    }
    finally
    {
      BinaryFormatterEventSource.Log.SerializationStop();
    }
  }

  public FormatterTypeStyle TypeFormat
  {
    get => this._typeFormat;
    set => this._typeFormat = value;
  }

  public FormatterAssemblyStyle AssemblyFormat
  {
    get => this._assemblyFormat;
    set => this._assemblyFormat = value;
  }

  public TypeFilterLevel FilterLevel
  {
    get => this._securityLevel;
    set => this._securityLevel = value;
  }

  public ISurrogateSelector SurrogateSelector
  {
    get => this._surrogates;
    set => this._surrogates = value;
  }

  public SerializationBinder Binder
  {
    get => this._binder;
    set => this._binder = value;
  }

  public StreamingContext Context
  {
    get => this._context;
    set => this._context = value;
  }

  public BinaryFormatter()
    : this((ISurrogateSelector) null, new StreamingContext(StreamingContextStates.All))
  {
  }

  public BinaryFormatter(ISurrogateSelector selector, StreamingContext context)
  {
    this._binder = BinaryFormatterDefaults.Binder;
    this._surrogates = selector ?? BinaryFormatterDefaults.SurrogateSelector;
    this._context = context;
  }

  internal static TypeInformation GetTypeInformation(Type type)
  {
    return BinaryFormatter.s_typeNameCache.GetOrAdd(type, (Func<Type, TypeInformation>) (t =>
    {
      bool hasTypeForwardedFrom;
      string clrAssemblyName = Intermech.Serialization.ClassicFormatters.FormatterServices.GetClrAssemblyName(t, out hasTypeForwardedFrom);
      return new TypeInformation(Intermech.Serialization.ClassicFormatters.FormatterServices.GetClrTypeFullName(t), clrAssemblyName, hasTypeForwardedFrom);
    }));
  }
}
