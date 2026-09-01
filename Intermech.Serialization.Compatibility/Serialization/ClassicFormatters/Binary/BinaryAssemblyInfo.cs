// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.Binary.BinaryAssemblyInfo
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters.Binary;

internal sealed class BinaryAssemblyInfo
{
  internal string _assemblyString;
  private Assembly _assembly;

  internal BinaryAssemblyInfo(string assemblyString) => this._assemblyString = assemblyString;

  internal BinaryAssemblyInfo(string assemblyString, Assembly assembly)
    : this(assemblyString)
  {
    this._assembly = assembly;
  }

  internal Assembly GetAssembly()
  {
    if (this._assembly == (Assembly) null)
    {
      this._assembly = Intermech.Serialization.ClassicFormatters.FormatterServices.LoadAssemblyFromStringNoThrow(this._assemblyString);
      if (this._assembly == (Assembly) null)
        throw new SerializationException(SR.Format(SR2.Serialization_AssemblyNotFound, (object) this._assemblyString));
    }
    return this._assembly;
  }
}
