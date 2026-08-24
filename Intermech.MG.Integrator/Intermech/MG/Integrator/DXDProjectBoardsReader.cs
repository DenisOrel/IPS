// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.DXDProjectBoardsReader
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Data;
using Intermech.Tools.Data;
using Intermech.Tools.Integrators.Electrical;
using Interop.Viewdraw;
using System;

#nullable disable
namespace Intermech.MG.Integrator;

internal class DXDProjectBoardsReader(ECADIntegratorSettings settings) : BoardReader<IMGProjectItem>(settings)
{
  protected override string ReadDesignation(string boardName, IValueBagContainer component)
  {
    string designationPropName = this.BoardDesignationPropName;
    if (designationPropName != string.Empty)
    {
      object propertyValue = ((MGComponent<IVdComp>) component).GetPropertyValue(designationPropName);
      if (propertyValue != null)
        return Convert.ToString(propertyValue);
    }
    return this.ReadArticleKey(component);
  }

  protected override string ReadName(string boardName, IValueBagContainer component)
  {
    string boardNamePropName = this.BoardNamePropName;
    if (boardNamePropName != string.Empty)
    {
      object propertyValue = ((MGComponent<IVdComp>) component).GetPropertyValue(boardNamePropName);
      if (propertyValue != null)
        return Convert.ToString(propertyValue);
    }
    return string.Empty;
  }

  protected override bool ReadIsMain(IValueBagContainer component)
  {
    return ((MGComponent<IVdComp>) component).GetProperty(((MGIntegratorSettings) this.settings).MainSchemeId) != null;
  }

  protected override IValueBagContainer GetAsmComponent(IMGProjectItem board)
  {
    return (IValueBagContainer) board.AssemblyComponent;
  }

  protected override string ReadArticleKey(IValueBagContainer component)
  {
    return ((MGComponent<IVdComp>) component).UID;
  }

  private string BoardDesignationPropName
  {
    get
    {
      Tuple<StringKey, StringKey, bool> tuple = this.settings.AssemblyAttributesTable.Find((Predicate<Tuple<StringKey, StringKey, bool>>) (x => x.Item1 == (StringKey) IDCache.Default.Designation.Text));
      return tuple == null ? string.Empty : tuple.Item2.ToString();
    }
  }

  private string BoardNamePropName
  {
    get
    {
      Tuple<StringKey, StringKey, bool> tuple = this.settings.AssemblyAttributesTable.Find((Predicate<Tuple<StringKey, StringKey, bool>>) (x => x.Item1 == (StringKey) IDCache.Default.Name.Text));
      return tuple == null ? string.Empty : tuple.Item2.ToString();
    }
  }
}
