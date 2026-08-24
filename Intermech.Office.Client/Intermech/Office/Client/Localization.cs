// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.Localization
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Office.Client;

internal class Localization
{
  [NotNull]
  private static readonly ResourceManager _resources = new ResourceManager("Intermech.Office.Client.Resources.OfficeClientResources", Assembly.GetExecutingAssembly());
  [NotNull]
  private static readonly ResourceManager _attributes = new ResourceManager("Intermech.Office.Client.Resources.CustomAttributesResources", Assembly.GetExecutingAssembly());

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string GetString([NotNull] string stringName, [NotNull] params object[] args)
  {
    string format = Localization._resources.GetString(stringName);
    if (format == null)
      return stringName;
    return args.Length != 0 ? string.Format(format, args) : format;
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string GetString([NotNull] string stringName, [NotNull] CultureInfo cultureInfo, [NotNull] params object[] args)
  {
    string format = Localization._resources.GetString(stringName, cultureInfo);
    if (format == null)
      return stringName;
    return args.Length != 0 ? string.Format(format, args) : format;
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string GetAttributesString([NotNull] string stringName, [NotNull] params object[] args)
  {
    string format = Localization._attributes.GetString(stringName);
    if (format == null)
      return stringName;
    return args.Length != 0 ? string.Format(format, args) : format;
  }

  [NotNull]
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static string GetAttributesString(
    [NotNull] string stringName,
    [NotNull] CultureInfo cultureInfo,
    [NotNull] params object[] args)
  {
    string format = Localization._attributes.GetString(stringName, cultureInfo);
    if (format == null)
      return stringName;
    return args.Length != 0 ? string.Format(format, args) : format;
  }
}
