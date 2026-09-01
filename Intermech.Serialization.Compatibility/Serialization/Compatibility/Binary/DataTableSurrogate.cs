// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.Compatibility.Binary.DataTableSurrogate
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.Compatibility.Binary;

public class DataTableSurrogate : ISerializationSurrogate
{
  private readonly ConstructorInfo specialConstructor;

  public DataTableSurrogate()
  {
    this.specialConstructor = typeof (DataTable).GetConstructor(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.ExactBinding, (Binder) null, new Type[2]
    {
      typeof (SerializationInfo),
      typeof (StreamingContext)
    }, (ParameterModifier[]) null);
  }

  public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
  {
    if (context.Context != null && !(context.Context is bool))
      context = new StreamingContext(context.State);
    DataTable dataTable = (DataTable) obj;
    dataTable.GetObjectData(info, context);
    if (dataTable.RemotingFormat != SerializationFormat.Binary)
      return;
    for (int index = 0; index < dataTable.Columns.Count; ++index)
    {
      Type dataType = dataTable.Columns[index].DataType;
      if (URTAssemblyInfo.IsNETFX)
        this.AddDataTypeAssemblyQualifiedNameMember(info, index, dataType);
      else
        this.AddDataTypeMember(info, index, dataType);
    }
  }

  public object SetObjectData(
    object obj,
    SerializationInfo info,
    StreamingContext context,
    ISurrogateSelector selector)
  {
    if (context.Context != null && !(context.Context is bool))
      context = new StreamingContext(context.State);
    if (this.TryGetTableRemotingFormat(info) == SerializationFormat.Binary)
    {
      int int32 = info.GetInt32("DataTable.Columns.Count");
      for (int dataColumnIndex = 0; dataColumnIndex < int32; ++dataColumnIndex)
      {
        if (URTAssemblyInfo.IsNETFX)
        {
          if (info.GetValueOrDefault<Type>($"DataTable.DataColumn_{dataColumnIndex}.DataType") == (Type) null)
          {
            Type type = Type.GetType(info.GetString($"DataTable.DataColumn_{dataColumnIndex}.DataType_AssemblyQualifiedName"), true);
            this.AddDataTypeMember(info, dataColumnIndex, type);
          }
        }
        else if (info.GetValueOrDefault<string>($"DataTable.DataColumn_{dataColumnIndex}.DataType_AssemblyQualifiedName") == null)
        {
          Type dataType = (Type) info.GetValue($"DataTable.DataColumn_{dataColumnIndex}.DataType", typeof (Type));
          this.AddDataTypeAssemblyQualifiedNameMember(info, dataColumnIndex, dataType);
        }
      }
    }
    this.specialConstructor.Invoke(obj, new object[2]
    {
      (object) info,
      (object) context
    });
    return obj;
  }

  private void AddDataTypeMember(SerializationInfo info, int dataColumnIndex, Type dataType)
  {
    info.AddValue($"DataTable.DataColumn_{dataColumnIndex}.DataType", (object) dataType);
  }

  private void AddDataTypeAssemblyQualifiedNameMember(
    SerializationInfo info,
    int dataColumnIndex,
    Type dataType)
  {
    StTypeInfo typeInfo = StTypeInfoService.Default.GetTypeInfo(dataType);
    string str = typeInfo.TypeName;
    if (typeInfo.AssemblyName != string.Empty)
      str = $"{str}, {typeInfo.AssemblyName}";
    info.AddValue($"DataTable.DataColumn_{dataColumnIndex}.DataType_AssemblyQualifiedName", (object) str);
  }

  private SerializationFormat TryGetTableRemotingFormat(SerializationInfo info)
  {
    return info.GetValueOrDefault<object>("DataTable.RemotingFormat") is SerializationFormat valueOrDefault ? valueOrDefault : SerializationFormat.Xml;
  }
}
