// Decompiled with JetBrains decompiler
// Type: Intermech.MRP.Properties.Resources
// Assembly: Intermech.MRP, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: FB727D7B-3877-440B-B401-3C7E86A45794
// Assembly location: D:\IPS\Client\Intermech.MRP.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP.xml

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.MRP.Properties;

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
      if (Intermech.MRP.Properties.Resources.resourceMan == null)
        Intermech.MRP.Properties.Resources.resourceMan = new ResourceManager("Intermech.MRP.Properties.Resources", typeof (Intermech.MRP.Properties.Resources).Assembly);
      return Intermech.MRP.Properties.Resources.resourceMan;
    }
  }

  /// <summary>
  ///   Overrides the current thread's CurrentUICulture property for all
  ///   resource lookups using this strongly typed resource class.
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.MRP.Properties.Resources.resourceCulture;
    set => Intermech.MRP.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap Configurator
  {
    get
    {
      return (Bitmap) Intermech.MRP.Properties.Resources.ResourceManager.GetObject(nameof (Configurator), Intermech.MRP.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap Routes
  {
    get => (Bitmap) Intermech.MRP.Properties.Resources.ResourceManager.GetObject(nameof (Routes), Intermech.MRP.Properties.Resources.resourceCulture);
  }

  internal static Bitmap Substitutes
  {
    get
    {
      return (Bitmap) Intermech.MRP.Properties.Resources.ResourceManager.GetObject(nameof (Substitutes), Intermech.MRP.Properties.Resources.resourceCulture);
    }
  }
}
