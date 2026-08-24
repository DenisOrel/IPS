// Decompiled with JetBrains decompiler
// Type: Intermech.ProEngineer.Integrator.Properties.Resources
// Assembly: Intermech.ProEngineer.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 19987673-5EB5-4BB3-AE60-6A96614A14F3
// Assembly location: D:\IPS\Client\Intermech.ProEngineer.Integrator.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ProEngineer.Integrator.Properties;

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
      if (Intermech.ProEngineer.Integrator.Properties.Resources.resourceMan == null)
        Intermech.ProEngineer.Integrator.Properties.Resources.resourceMan = new ResourceManager("Intermech.ProEngineer.Integrator.Properties.Resources", typeof (Intermech.ProEngineer.Integrator.Properties.Resources).Assembly);
      return Intermech.ProEngineer.Integrator.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.ProEngineer.Integrator.Properties.Resources.resourceCulture;
    set => Intermech.ProEngineer.Integrator.Properties.Resources.resourceCulture = value;
  }

  internal static string PE_IntegratorDescription
  {
    get
    {
      return Intermech.ProEngineer.Integrator.Properties.Resources.ResourceManager.GetString(nameof (PE_IntegratorDescription), Intermech.ProEngineer.Integrator.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap pe16
  {
    get => (Bitmap) Intermech.ProEngineer.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (pe16), Intermech.ProEngineer.Integrator.Properties.Resources.resourceCulture);
  }

  internal static Bitmap pe32
  {
    get => (Bitmap) Intermech.ProEngineer.Integrator.Properties.Resources.ResourceManager.GetObject(nameof (pe32), Intermech.ProEngineer.Integrator.Properties.Resources.resourceCulture);
  }
}
