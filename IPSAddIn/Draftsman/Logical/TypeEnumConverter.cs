// Decompiled with JetBrains decompiler
// Type: CSharpPlugin.Draftsman.Logical.TypeEnumConverter
// Assembly: IPSAddIn, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: F6758E82-0F4D-46BA-A517-315691E31B38
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\IPSAddIn.dll

using Newtonsoft.Json;
using System;

#nullable disable
namespace CSharpPlugin.Draftsman.Logical;

internal sealed class TypeEnumConverter : JsonConverter
{
  public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();

  public override bool CanConvert(Type t) => t == typeof (TypeEnum) || t == typeof (TypeEnum?);

  public override object ReadJson(
    JsonReader reader,
    Type t,
    object existingValue,
    JsonSerializer serializer)
  {
    if (reader.TokenType == JsonToken.Null)
      return (object) null;
    switch (serializer.Deserialize<string>(reader))
    {
      case "__logicalDirectConnection":
        return (object) TypeEnum.LogicalDirectConnection;
      case "__logicalPinToPinConnection":
        return (object) TypeEnum.LogicalPinToPinConnection;
      default:
        throw new Exception("Cannot unmarshal type TypeEnum");
    }
  }

  public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
  {
    if (untypedValue == null)
    {
      serializer.Serialize(writer, (object) null);
    }
    else
    {
      switch ((TypeEnum) untypedValue)
      {
        case TypeEnum.LogicalDirectConnection:
          serializer.Serialize(writer, (object) "__logicalDirectConnection");
          break;
        case TypeEnum.LogicalPinToPinConnection:
          serializer.Serialize(writer, (object) "__logicalPinToPinConnection");
          break;
        default:
          throw new Exception("Cannot marshal type TypeEnum");
      }
    }
  }
}
