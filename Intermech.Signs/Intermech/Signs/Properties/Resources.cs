// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Properties.Resources
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Signs.Properties;

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
      if (Intermech.Signs.Properties.Resources.resourceMan == null)
        Intermech.Signs.Properties.Resources.resourceMan = new ResourceManager("Intermech.Signs.Properties.Resources", typeof (Intermech.Signs.Properties.Resources).Assembly);
      return Intermech.Signs.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.Signs.Properties.Resources.resourceCulture;
    set => Intermech.Signs.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap SignConditions
  {
    get
    {
      return (Bitmap) Intermech.Signs.Properties.Resources.ResourceManager.GetObject(nameof (SignConditions), Intermech.Signs.Properties.Resources.resourceCulture);
    }
  }
}
