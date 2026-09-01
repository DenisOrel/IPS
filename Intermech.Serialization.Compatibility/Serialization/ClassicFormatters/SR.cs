// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.SR
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

using System;

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal static class SR
{
  internal static string Format(string resourceFormat, object p1)
  {
    return string.Format(resourceFormat, p1);
  }

  internal static string Format(string resourceFormat, object p1, object p2)
  {
    return string.Format(resourceFormat, p1, p2);
  }

  internal static string Format(string resourceFormat, object p1, object p2, object p3)
  {
    return string.Format(resourceFormat, p1, p2, p3);
  }

  internal static string Format(string resourceFormat, params object[] args)
  {
    return args != null ? string.Format(resourceFormat, args) : resourceFormat;
  }

  internal static string Format(IFormatProvider provider, string resourceFormat, object p1)
  {
    return string.Format(provider, resourceFormat, p1);
  }

  internal static string Format(
    IFormatProvider provider,
    string resourceFormat,
    object p1,
    object p2)
  {
    return string.Format(provider, resourceFormat, p1, p2);
  }

  internal static string Format(
    IFormatProvider provider,
    string resourceFormat,
    object p1,
    object p2,
    object p3)
  {
    return string.Format(provider, resourceFormat, p1, p2, p3);
  }

  internal static string Format(
    IFormatProvider provider,
    string resourceFormat,
    params object[] args)
  {
    return args != null ? string.Format(provider, resourceFormat, args) : resourceFormat;
  }
}
