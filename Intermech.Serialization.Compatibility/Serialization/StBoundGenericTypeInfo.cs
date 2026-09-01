// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.StBoundGenericTypeInfo
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;
using System.Text;

#nullable disable
namespace Intermech.Serialization;

public sealed class StBoundGenericTypeInfo : StTypeInfo
{
  public StBoundGenericTypeInfo(
    string typeName,
    string assemblyName,
    StTypeInfo definition,
    StTypeInfo[] arguments)
    : base(typeName, assemblyName)
  {
    if (definition == null)
      throw new ArgumentNullException(nameof (definition));
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    this.Definition = definition;
    this.Arguments = arguments;
  }

  public StBoundGenericTypeInfo(StTypeInfo definition, StTypeInfo[] arguments)
    : base(string.Empty, string.Empty)
  {
    if (definition == null)
      throw new ArgumentNullException(nameof (definition));
    if (arguments == null)
      throw new ArgumentNullException(nameof (arguments));
    string assemblyName = definition.AssemblyName;
    StringBuilder stringBuilder = new StringBuilder(definition.TypeName);
    stringBuilder.Append("[");
    for (int index = 0; index < arguments.Length; ++index)
    {
      StTypeInfo stTypeInfo = arguments[index];
      stringBuilder.Append("[");
      stringBuilder.Append(stTypeInfo.TypeName);
      if (stTypeInfo.AssemblyName != string.Empty)
      {
        stringBuilder.Append(", ");
        stringBuilder.Append(stTypeInfo.AssemblyName);
      }
      stringBuilder.Append("]");
      stringBuilder.Append(", ");
    }
    stringBuilder.Remove(stringBuilder.Length - 2, 2);
    stringBuilder.Append(']');
    this.assemblyName = assemblyName;
    this.typeName = stringBuilder.ToString();
    this.Definition = definition;
    this.Arguments = arguments;
  }

  public StTypeInfo Definition { get; }

  public StTypeInfo[] Arguments { get; }
}
