// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Properties.Resources
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Site.Client.Properties;

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
      if (Intermech.Site.Client.Properties.Resources.resourceMan == null)
        Intermech.Site.Client.Properties.Resources.resourceMan = new ResourceManager("Intermech.Site.Client.Properties.Resources", typeof (Intermech.Site.Client.Properties.Resources).Assembly);
      return Intermech.Site.Client.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.Site.Client.Properties.Resources.resourceCulture;
    set => Intermech.Site.Client.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap Classify
  {
    get
    {
      return (Bitmap) Intermech.Site.Client.Properties.Resources.ResourceManager.GetObject(nameof (Classify), Intermech.Site.Client.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap Delete
  {
    get => (Bitmap) Intermech.Site.Client.Properties.Resources.ResourceManager.GetObject(nameof (Delete), Intermech.Site.Client.Properties.Resources.resourceCulture);
  }

  internal static Icon export
  {
    get => (Icon) Intermech.Site.Client.Properties.Resources.ResourceManager.GetObject(nameof (export), Intermech.Site.Client.Properties.Resources.resourceCulture);
  }

  internal static Icon import
  {
    get => (Icon) Intermech.Site.Client.Properties.Resources.ResourceManager.GetObject(nameof (import), Intermech.Site.Client.Properties.Resources.resourceCulture);
  }

  internal static Bitmap ObjectTypes
  {
    get
    {
      return (Bitmap) Intermech.Site.Client.Properties.Resources.ResourceManager.GetObject(nameof (ObjectTypes), Intermech.Site.Client.Properties.Resources.resourceCulture);
    }
  }

  internal static Icon portal
  {
    get => (Icon) Intermech.Site.Client.Properties.Resources.ResourceManager.GetObject(nameof (portal), Intermech.Site.Client.Properties.Resources.resourceCulture);
  }

  internal static Bitmap refresh
  {
    get
    {
      return (Bitmap) Intermech.Site.Client.Properties.Resources.ResourceManager.GetObject(nameof (refresh), Intermech.Site.Client.Properties.Resources.resourceCulture);
    }
  }
}
