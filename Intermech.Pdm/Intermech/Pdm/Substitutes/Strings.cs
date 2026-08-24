// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Substitutes.Strings
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Localization;

#nullable disable
namespace Intermech.Pdm.Substitutes;

internal static class Strings
{
  public static readonly string Cancel = LocalizationHolder.rm.GetString("Pdm_248");
  public static readonly string Close = LocalizationHolder.rm.GetString("Pdm_249");
  public static readonly string FormCaption = LocalizationHolder.rm.GetString("Pdm_254");
  public static readonly string CreateSubstituteGroup = LocalizationHolder.rm.GetString("Pdm_255");
  public static readonly string DeleteSubstituteGroup = LocalizationHolder.rm.GetString("Pdm_256");
  public static readonly string ActualizeSubstitute = LocalizationHolder.rm.GetString("Pdm_257");
  public static readonly string Attention = LocalizationHolder.rm.GetString("Pdm_264");
  public static readonly string Hint0 = LocalizationHolder.rm.GetString("Pdm_276");
  public static readonly string Hint4 = LocalizationHolder.rm.GetString("Pdm_280");
  public static readonly string Hint5 = LocalizationHolder.rm.GetString("Pdm_515");
  public static readonly string Hint6 = LocalizationHolder.rm.GetString("Pdm_516");
  public static readonly string CheckSuccess = LocalizationHolder.rm.GetString("Pdm_281");
  public static readonly string Error0 = LocalizationHolder.rm.GetString("Pdm_282");
  public static readonly string Error0a = LocalizationHolder.rm.GetString("Pdm_283");
  public static readonly string Error1 = LocalizationHolder.rm.GetString("Pdm_284");
  public static readonly string Error2 = LocalizationHolder.rm.GetString("Pdm_285");
  public static readonly string Error3 = LocalizationHolder.rm.GetString("Pdm_286");
  public static readonly string Error6 = LocalizationHolder.rm.GetString("Pdm_289");
  public static readonly string Error7 = LocalizationHolder.rm.GetString("Pdm_290");
  public static readonly string Error8 = LocalizationHolder.rm.GetString("Pdm_291");

  public static string Confirmation => LocalizationHolder.rm.GetString(nameof (Confirmation));

  public static string NeedDeleteAuxiliaryPositionsWithRelations
  {
    get => LocalizationHolder.rm.GetString("Substitutes.NeedDeleteAuxiliaryPositionsWithRelations");
  }
}
