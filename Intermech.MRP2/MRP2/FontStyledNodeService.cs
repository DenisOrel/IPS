// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.FontStyledNodeService
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Queries;
using System.Drawing;

#nullable disable
namespace Intermech.MRP2;

internal class FontStyledNodeService : IFontStyledNode
{
  private readonly NodeColumnID ncF_DELETE_TAG = new NodeColumnID((object) MRP2Consts.attrIdDeleteTag, AttributeSourceTypes.Relation);

  public FontStyle ComputeFontStyleStatus(
    object[] fieldValues,
    RecordAdapter adapter,
    byte[] stateAttr)
  {
    return stateAttr != null && ServicesManager.GetService<IElementStatusesClientService>().GetElementStatuses16("cad8491c-5d67-476f-b87a-f2c6dcd807a2", stateAttr) == (short) 4 ? FontStyle.Strikeout : FontStyle.Regular;
  }
}
