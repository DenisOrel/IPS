// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.Compatibility.Binary.DataSetSurrogate
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;

#nullable disable
namespace Intermech.Serialization.Compatibility.Binary;

public class DataSetSurrogate : ISerializationSurrogate
{
  private readonly BinaryFormatterCompatibilitySurrogateServices services;
  private readonly ConstructorInfo defaultConstructor;

  public DataSetSurrogate(
    BinaryFormatterCompatibilitySurrogateServices services)
  {
    this.services = services != null ? services : throw new ArgumentNullException(nameof (services));
    this.defaultConstructor = typeof (DataSet).GetConstructor(Type.EmptyTypes);
  }

  public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
  {
    ((DataSet) obj).GetObjectData(info, context);
  }

  public object SetObjectData(
    object obj,
    SerializationInfo info,
    StreamingContext context,
    ISurrogateSelector selector)
  {
    this.DeserializationConstructor((DataSet) obj, info, context);
    return obj;
  }

  private void DeserializationConstructor(
    DataSet target,
    SerializationInfo info,
    StreamingContext context)
  {
    this.defaultConstructor.Invoke((object) target, new object[0]);
    SerializationFormat remotingFormat = SerializationFormat.Xml;
    SchemaSerializationMode schemaSerializationMode = SchemaSerializationMode.IncludeSchema;
    SerializationInfoEnumerator enumerator = info.GetEnumerator();
    while (enumerator.MoveNext())
    {
      switch (enumerator.Name)
      {
        case "DataSet.RemotingFormat":
          remotingFormat = (SerializationFormat) enumerator.Value;
          continue;
        case "SchemaSerializationMode.DataSet":
          schemaSerializationMode = (SchemaSerializationMode) enumerator.Value;
          continue;
        default:
          continue;
      }
    }
    if (schemaSerializationMode == SchemaSerializationMode.ExcludeSchema)
      this.ReflectionInvoke("InitializeDerivedDataSet", (object) target);
    if (remotingFormat == SerializationFormat.Xml)
      return;
    this.DeserializeDataSet(target, info, context, remotingFormat, schemaSerializationMode);
  }

  private void DeserializeDataSet(
    DataSet target,
    SerializationInfo info,
    StreamingContext context,
    SerializationFormat remotingFormat,
    SchemaSerializationMode schemaSerializationMode)
  {
    this.DeserializeDataSetSchema(target, info, context, remotingFormat, schemaSerializationMode);
    this.DeserializeDataSetData(target, info, context, remotingFormat);
  }

  private void DeserializeDataSetSchema(
    DataSet target,
    SerializationInfo info,
    StreamingContext context,
    SerializationFormat remotingFormat,
    SchemaSerializationMode schemaSerializationMode)
  {
    if (remotingFormat != SerializationFormat.Xml)
    {
      if (schemaSerializationMode == SchemaSerializationMode.IncludeSchema)
      {
        this.ReflectionInvoke("DeserializeDataSetProperties", (object) target, (object) info, (object) context);
        int int32 = info.GetInt32("DataSet.Tables.Count");
        for (int index = 0; index < int32; ++index)
        {
          MemoryStream serializationStream = new MemoryStream((byte[]) info.GetValue(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "DataSet.Tables_{0}", new object[1]
          {
            (object) index
          }), typeof (byte[])));
          serializationStream.Position = 0L;
          DataTable table = (DataTable) this.services.BinaryFormatterFactory(new StreamingContext(context.State, (object) false)).Deserialize((Stream) serializationStream);
          target.Tables.Add(table);
        }
        for (int index = 0; index < int32; ++index)
          this.ReflectionInvoke("DeserializeConstraints", (object) target.Tables[index], (object) info, (object) context, (object) index, (object) true);
        this.ReflectionInvoke("DeserializeRelations", (object) target, (object) info, (object) context);
        for (int index = 0; index < int32; ++index)
          this.ReflectionInvoke("DeserializeExpressionColumns", (object) target.Tables[index], (object) info, (object) context, (object) index);
      }
      else
        this.ReflectionInvoke("DeserializeDataSetProperties", (object) target, (object) info, (object) context);
    }
    else
    {
      string s = (string) info.GetValue("XmlSchema", typeof (string));
      if (s == null)
        return;
      this.ReflectionInvoke("ReadXmlSchema", (object) target, (object) new XmlTextReader((TextReader) new StringReader(s)), (object) true);
    }
  }

  private void DeserializeDataSetData(
    DataSet target,
    SerializationInfo info,
    StreamingContext context,
    SerializationFormat remotingFormat)
  {
    this.ReflectionInvoke(nameof (DeserializeDataSetData), (object) target, (object) info, (object) context, (object) remotingFormat);
  }

  private object ReflectionInvoke(string methodName, object target, params object[] args)
  {
    Type type = target.GetType();
    MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    if (method != (MethodInfo) null)
      return method.Invoke(target, args);
    throw new SerializationException($"No method named '{methodName}' found in the type '{type.AssemblyQualifiedName}'.");
  }
}
