// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.Properties.Resources
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.MG.Integrator.Properties;

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
      if (Intermech.MG.Integrator.Properties.Resources.resourceMan == null)
        Intermech.MG.Integrator.Properties.Resources.resourceMan = new ResourceManager("Intermech.MG.Integrator.Properties.Resources", typeof (Intermech.MG.Integrator.Properties.Resources).Assembly);
      return Intermech.MG.Integrator.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.MG.Integrator.Properties.Resources.resourceCulture;
    set => Intermech.MG.Integrator.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap epcb_16x16
  {
    get
    {
      return (Bitmap) Intermech.MG.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (epcb_16x16), Intermech.MG.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap epcb_32x32
  {
    get
    {
      return (Bitmap) Intermech.MG.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (epcb_32x32), Intermech.MG.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static string Integrator_template
  {
    get
    {
      return Intermech.MG.Integrator.Properties.Resources.ResourceManager.GetString(nameof (Integrator_template), Intermech.MG.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap mg_16x16
  {
    get
    {
      return (Bitmap) Intermech.MG.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (mg_16x16), Intermech.MG.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap mg_32x32
  {
    get
    {
      return (Bitmap) Intermech.MG.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (mg_32x32), Intermech.MG.Integrator.Properties.Resources.resourceCulture);
    }
  }
}
