// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Properties.Resources
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Signs.Properties;

/// <summary>
///   A strongly-typed resource class, for looking up localized strings, etc.
/// </summary>
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal Resources()
  {
  }

  /// <summary>
  ///   Returns the cached ResourceManager instance used by this class.
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (Intermech.Signs.Properties.Resources.resourceMan == null)
        Intermech.Signs.Properties.Resources.resourceMan = new ResourceManager("Intermech.Signs.Properties.Resources", typeof (Intermech.Signs.Properties.Resources).Assembly);
      return Intermech.Signs.Properties.Resources.resourceMan;
    }
  }

  /// <summary>
  ///   Overrides the current thread's CurrentUICulture property for all
  ///   resource lookups using this strongly typed resource class.
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.Signs.Properties.Resources.resourceCulture;
    set => Intermech.Signs.Properties.Resources.resourceCulture = value;
  }

  /// <summary>
  ///   Looks up a localized resource of type System.Drawing.Bitmap.
  /// </summary>
  internal static Bitmap SignConditions
  {
    get
    {
      return (Bitmap) Intermech.Signs.Properties.Resources.ResourceManager.GetObject(nameof (SignConditions), Intermech.Signs.Properties.Resources.resourceCulture);
    }
  }
}
