// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Pdm.Analogs.AnalogsResource
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.Search.Pdm.Analogs;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class AnalogsResource
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal AnalogsResource()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (AnalogsResource.resourceMan == null)
        AnalogsResource.resourceMan = new ResourceManager("Intermech.Pdm.Intermech.Search.Pdm.Analogs.AnalogsResource", typeof (AnalogsResource).Assembly);
      return AnalogsResource.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => AnalogsResource.resourceCulture;
    set => AnalogsResource.resourceCulture = value;
  }

  internal static Bitmap ChooseActingAnalog
  {
    get
    {
      return (Bitmap) AnalogsResource.ResourceManager.GetObject(nameof (ChooseActingAnalog), AnalogsResource.resourceCulture);
    }
  }

  internal static Bitmap ChooseOneAnalog
  {
    get
    {
      return (Bitmap) AnalogsResource.ResourceManager.GetObject(nameof (ChooseOneAnalog), AnalogsResource.resourceCulture);
    }
  }

  internal static Bitmap DoNotChooseAnalogs
  {
    get
    {
      return (Bitmap) AnalogsResource.ResourceManager.GetObject(nameof (DoNotChooseAnalogs), AnalogsResource.resourceCulture);
    }
  }

  internal static Bitmap ShowAllAnalogs
  {
    get
    {
      return (Bitmap) AnalogsResource.ResourceManager.GetObject(nameof (ShowAllAnalogs), AnalogsResource.resourceCulture);
    }
  }
}
