// Decompiled with JetBrains decompiler
// Type: Intermech.Mbom.Properties.Resources
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Mbom.Properties;

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
      if (Intermech.Mbom.Properties.Resources.resourceMan == null)
        Intermech.Mbom.Properties.Resources.resourceMan = new ResourceManager("Intermech.Mbom.Properties.Resources", typeof (Intermech.Mbom.Properties.Resources).Assembly);
      return Intermech.Mbom.Properties.Resources.resourceMan;
    }
  }

  /// <summary>
  ///   Overrides the current thread's CurrentUICulture property for all
  ///   resource lookups using this strongly typed resource class.
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.Mbom.Properties.Resources.resourceCulture;
    set => Intermech.Mbom.Properties.Resources.resourceCulture = value;
  }

  /// <summary>
  ///   Looks up a localized resource of type System.Drawing.Bitmap.
  /// </summary>
  internal static Bitmap Actions_go_next_view_icon
  {
    get
    {
      return (Bitmap) Intermech.Mbom.Properties.Resources.ResourceManager.GetObject("Actions-go-next-view-icon", Intermech.Mbom.Properties.Resources.resourceCulture);
    }
  }

  /// <summary>
  ///   Looks up a localized resource of type System.Drawing.Bitmap.
  /// </summary>
  internal static Bitmap Actions_go_previous_view_icon
  {
    get
    {
      return (Bitmap) Intermech.Mbom.Properties.Resources.ResourceManager.GetObject("Actions-go-previous-view-icon", Intermech.Mbom.Properties.Resources.resourceCulture);
    }
  }
}
