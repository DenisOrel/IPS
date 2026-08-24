// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Mbom.MbomClientConstants
// Assembly: Intermech.Mbom, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 13559C9A-4DBC-479B-BA71-AFEA0247DEC7
// Assembly location: D:\IPS\Client\Intermech.Mbom.dll
// XML documentation location: D:\IPS\Client\Intermech.Mbom.xml

using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.Search.Mbom;

public static class MbomClientConstants
{
  public const string CreateMbomCommandName = "CreateMbom";
  public const string CreateTauCommandName = "PDM.CreateContext";
  public const string EditMbomCommandName = "EditDocument";
  public static readonly Guid MbomEditorCategoryGuid;
  private static int _mbomEditorCategoryID = -1;

  public static int MbomEditorCategoryID
  {
    get
    {
      if (MbomClientConstants._mbomEditorCategoryID < 0)
        MbomClientConstants._mbomEditorCategoryID = ServiceLocator.Get<IGuidMapper>().Register(MbomClientConstants.MbomEditorCategoryGuid);
      return MbomClientConstants._mbomEditorCategoryID;
    }
  }
}
