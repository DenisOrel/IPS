// Decompiled with JetBrains decompiler
// Type: Intermech.AI.Integrator.Properties.Resources
// Assembly: Intermech.Inventor.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 5DE4AB90-6F29-45A8-A3E7-0F17B3967045
// Assembly location: D:\IPS\Client\Intermech.Inventor.Integrator.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.AI.Integrator.Properties;

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
      if (Intermech.AI.Integrator.Properties.Resources.resourceMan == null)
        Intermech.AI.Integrator.Properties.Resources.resourceMan = new ResourceManager("Intermech.AI.Integrator.Properties.Resources", typeof (Intermech.AI.Integrator.Properties.Resources).Assembly);
      return Intermech.AI.Integrator.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.AI.Integrator.Properties.Resources.resourceCulture;
    set => Intermech.AI.Integrator.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap IR_AI2009
  {
    get
    {
      return (Bitmap) Intermech.AI.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (IR_AI2009), Intermech.AI.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap IR_AI2009_32x32
  {
    get
    {
      return (Bitmap) Intermech.AI.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (IR_AI2009_32x32), Intermech.AI.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static string SR_IntegratorDescription
  {
    get
    {
      return Intermech.AI.Integrator.Properties.Resources.ResourceManager.GetString(nameof (SR_IntegratorDescription), Intermech.AI.Integrator.Properties.Resources.resourceCulture);
    }
  }
}
