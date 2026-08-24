// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.Properties.Resources
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.Imbase.Properties;

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

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (Intermech.ImpExp.Imbase.Properties.Resources.resourceMan == null)
        Intermech.ImpExp.Imbase.Properties.Resources.resourceMan = new ResourceManager("Intermech.ImpExp.Imbase.Properties.Resources", typeof (Intermech.ImpExp.Imbase.Properties.Resources).Assembly);
      return Intermech.ImpExp.Imbase.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.ImpExp.Imbase.Properties.Resources.resourceCulture;
    set => Intermech.ImpExp.Imbase.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap _3point
  {
    get => (Bitmap) Intermech.ImpExp.Imbase.Properties.Resources.ResourceManager.GetObject("3point", Intermech.ImpExp.Imbase.Properties.Resources.resourceCulture);
  }

  internal static Bitmap Единицы_измерения
  {
    get
    {
      return (Bitmap) Intermech.ImpExp.Imbase.Properties.Resources.ResourceManager.GetObject("Единицы измерения", Intermech.ImpExp.Imbase.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap Физические_величины
  {
    get
    {
      return (Bitmap) Intermech.ImpExp.Imbase.Properties.Resources.ResourceManager.GetObject("Физические величины", Intermech.ImpExp.Imbase.Properties.Resources.resourceCulture);
    }
  }
}
