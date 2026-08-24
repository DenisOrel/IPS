// Decompiled with JetBrains decompiler
// Type: Intermech.Scripting.Common.ByteArrayComparer
// Assembly: Intermech.Scripting, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 8614EF74-D879-46F7-BBB4-441A9D335356
// Assembly location: D:\IPS\Client\RoslynScriptCompiler\Intermech.Scripting.dll
// XML documentation location: D:\IPS\Client\Intermech.Scripting.xml

using System.Collections.Generic;

#nullable disable
namespace Intermech.Scripting.Common;

internal sealed class ByteArrayComparer : IEqualityComparer<byte[]>
{
  public bool Equals(byte[] x, byte[] y)
  {
    if (x.Length != y.Length)
      return false;
    for (int index = 0; index < x.Length; ++index)
    {
      if ((int) x[index] != (int) y[index])
        return false;
    }
    return true;
  }

  public int GetHashCode(byte[] obj)
  {
    int hashCode = obj.Length;
    for (int index = 0; index < obj.Length; ++index)
      hashCode = hashCode << 5 ^ (int) obj[index];
    return hashCode;
  }
}
