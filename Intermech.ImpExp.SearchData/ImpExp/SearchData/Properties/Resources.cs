// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.Properties.Resources
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImpExp.SearchData.Properties;

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
      if (Intermech.ImpExp.SearchData.Properties.Resources.resourceMan == null)
        Intermech.ImpExp.SearchData.Properties.Resources.resourceMan = new ResourceManager("Intermech.ImpExp.SearchData.Properties.Resources", typeof (Intermech.ImpExp.SearchData.Properties.Resources).Assembly);
      return Intermech.ImpExp.SearchData.Properties.Resources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => Intermech.ImpExp.SearchData.Properties.Resources.resourceCulture;
    set => Intermech.ImpExp.SearchData.Properties.Resources.resourceCulture = value;
  }

  internal static Bitmap info
  {
    get => (Bitmap) Intermech.ImpExp.SearchData.Properties.Resources.ResourceManager.GetObject(nameof (info), Intermech.ImpExp.SearchData.Properties.Resources.resourceCulture);
  }

  internal static Bitmap Search
  {
    get => (Bitmap) Intermech.ImpExp.SearchData.Properties.Resources.ResourceManager.GetObject(nameof (Search), Intermech.ImpExp.SearchData.Properties.Resources.resourceCulture);
  }
}
