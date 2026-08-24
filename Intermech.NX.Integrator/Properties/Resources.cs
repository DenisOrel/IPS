// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.Properties.Resources
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.NX.Integrator.Properties;

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
      if (Intermech.NX.Integrator.Properties.Resources.resourceMan == null)
        Intermech.NX.Integrator.Properties.Resources.resourceMan = new ResourceManager("Intermech.NX.Integrator.Properties.Resources", typeof (Intermech.NX.Integrator.Properties.Resources).Assembly);
      return Intermech.NX.Integrator.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.NX.Integrator.Properties.Resources.resourceCulture;
    set => Intermech.NX.Integrator.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap NX_16x16
  {
    get
    {
      return (Bitmap) Intermech.NX.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (NX_16x16), Intermech.NX.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap NX_32x32
  {
    get
    {
      return (Bitmap) Intermech.NX.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (NX_32x32), Intermech.NX.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static string SR_IntegratorDescription
  {
    get
    {
      return Intermech.NX.Integrator.Properties.Resources.ResourceManager.GetString(nameof (SR_IntegratorDescription), Intermech.NX.Integrator.Properties.Resources.resourceCulture);
    }
  }
}
