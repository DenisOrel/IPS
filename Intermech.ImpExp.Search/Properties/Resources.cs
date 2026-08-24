// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.Properties.Resources
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.Search.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
      if (Intermech.ImpExp.Search.Properties.Resources.resourceMan == null)
        Intermech.ImpExp.Search.Properties.Resources.resourceMan = new ResourceManager("Intermech.ImpExp.Search.Properties.Resources", typeof (Intermech.ImpExp.Search.Properties.Resources).Assembly);
      return Intermech.ImpExp.Search.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.ImpExp.Search.Properties.Resources.resourceCulture;
    set => Intermech.ImpExp.Search.Properties.Resources.resourceCulture = value;
  }

  internal static Icon archives
  {
    get => (Icon) Intermech.ImpExp.Search.Properties.Resources.ResourceManager.GetObject(nameof (archives), Intermech.ImpExp.Search.Properties.Resources.resourceCulture);
  }

  internal static Bitmap ArchiveSettings
  {
    get
    {
      return (Bitmap) Intermech.ImpExp.Search.Properties.Resources.ResourceManager.GetObject(nameof (ArchiveSettings), Intermech.ImpExp.Search.Properties.Resources.resourceCulture);
    }
  }

  internal static Icon EmptyIcon
  {
    get
    {
      return (Icon) Intermech.ImpExp.Search.Properties.Resources.ResourceManager.GetObject(nameof (EmptyIcon), Intermech.ImpExp.Search.Properties.Resources.resourceCulture);
    }
  }

  internal static Icon SearchStatus
  {
    get
    {
      return (Icon) Intermech.ImpExp.Search.Properties.Resources.ResourceManager.GetObject(nameof (SearchStatus), Intermech.ImpExp.Search.Properties.Resources.resourceCulture);
    }
  }

  internal static Bitmap ThematicParams
  {
    get
    {
      return (Bitmap) Intermech.ImpExp.Search.Properties.Resources.ResourceManager.GetObject(nameof (ThematicParams), Intermech.ImpExp.Search.Properties.Resources.resourceCulture);
    }
  }
}
