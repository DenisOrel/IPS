// Decompiled with JetBrains decompiler
// Type: Intermech.ImShape.Client.Resources.ImShapeResources
// Assembly: Intermech.ImShape.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: EAEE73DE-1C1F-4401-8BB6-D181BFA32870
// Assembly location: D:\IPS\Client\Intermech.ImShape.Client.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

#nullable disable
namespace Intermech.ImShape.Client.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class ImShapeResources
{
  private static ResourceManager resourceMan;
  private static CultureInfo resourceCulture;

  internal ImShapeResources()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static ResourceManager ResourceManager
  {
    get
    {
      if (ImShapeResources.resourceMan == null)
        ImShapeResources.resourceMan = new ResourceManager("Intermech.ImShape.Client.Resources.ImShapeResources", typeof (ImShapeResources).Assembly);
      return ImShapeResources.resourceMan;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Advanced)]
  internal static CultureInfo Culture
  {
    get => ImShapeResources.resourceCulture;
    set => ImShapeResources.resourceCulture = value;
  }

  internal static string ImShape_SystemPage_Name
  {
    get
    {
      return ImShapeResources.ResourceManager.GetString(nameof (ImShape_SystemPage_Name), ImShapeResources.resourceCulture);
    }
  }

  internal static string ImShape_SystemPage_Path
  {
    get
    {
      return ImShapeResources.ResourceManager.GetString(nameof (ImShape_SystemPage_Path), ImShapeResources.resourceCulture);
    }
  }
}
