// Decompiled with JetBrains decompiler
// Type: Intermech.SolidEdge.Integrator.Properties.Resources
// Assembly: Intermech.SolidEdge.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 213B90F8-0434-43B8-B8F6-9AF19E139193
// Assembly location: D:\IPS\Client\Intermech.SolidEdge.Integrator.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.SolidEdge.Integrator.Properties;

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
      if (Intermech.SolidEdge.Integrator.Properties.Resources.resourceMan == null)
        Intermech.SolidEdge.Integrator.Properties.Resources.resourceMan = new ResourceManager("Intermech.SolidEdge.Integrator.Properties.Resources", typeof (Intermech.SolidEdge.Integrator.Properties.Resources).Assembly);
      return Intermech.SolidEdge.Integrator.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.SolidEdge.Integrator.Properties.Resources.resourceCulture;
    set => Intermech.SolidEdge.Integrator.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap SE_16x16
  {
    get
    {
      return (Bitmap) Intermech.SolidEdge.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (SE_16x16), Intermech.SolidEdge.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap SE_32x32
  {
    get
    {
      return (Bitmap) Intermech.SolidEdge.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (SE_32x32), Intermech.SolidEdge.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static string SR_IntegratorDescription
  {
    get
    {
      return Intermech.SolidEdge.Integrator.Properties.Resources.ResourceManager.GetString(nameof (SR_IntegratorDescription), Intermech.SolidEdge.Integrator.Properties.Resources.resourceCulture);
    }
  }
}
