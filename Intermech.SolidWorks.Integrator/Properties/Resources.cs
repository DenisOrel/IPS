// Decompiled with JetBrains decompiler
// Type: Intermech.SolidWorks.Integrator.Properties.Resources
// Assembly: Intermech.SolidWorks.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C58B767B-0480-4923-A6B5-4C5307770AFD
// Assembly location: D:\IPS\Client\Intermech.SolidWorks.Integrator.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.SolidWorks.Integrator.Properties;

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
      if (Intermech.SolidWorks.Integrator.Properties.Resources.resourceMan == null)
        Intermech.SolidWorks.Integrator.Properties.Resources.resourceMan = new ResourceManager("Intermech.SolidWorks.Integrator.Properties.Resources", typeof (Intermech.SolidWorks.Integrator.Properties.Resources).Assembly);
      return Intermech.SolidWorks.Integrator.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.SolidWorks.Integrator.Properties.Resources.resourceCulture;
    set => Intermech.SolidWorks.Integrator.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap sw16
  {
    get => (Bitmap) Intermech.SolidWorks.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (sw16), Intermech.SolidWorks.Integrator.Properties.Resources.resourceCulture);
  }

  internal static Bitmap sw32
  {
    get => (Bitmap) Intermech.SolidWorks.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (sw32), Intermech.SolidWorks.Integrator.Properties.Resources.resourceCulture);
  }
}
