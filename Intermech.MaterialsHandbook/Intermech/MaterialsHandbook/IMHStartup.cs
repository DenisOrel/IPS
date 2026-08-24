// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.IMHStartup
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Bars;
using Intermech.Docking;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Configuration;
using Intermech.Interfaces.Imbase;
using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Interfaces.Plugins;
using Intermech.Localization;
using Intermech.NavBars;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Protection;
using Intermech.Search;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

#nullable disable
namespace Intermech.MaterialsHandbook;

public class IMHStartup : IPackage, IConfigurable
{
  private IServiceProvider _srvProvider;

  public void Load(IServiceProvider serviceProvider)
  {
    this._srvProvider = serviceProvider;
    ((IPluginManager) serviceProvider.GetService(typeof (IPluginManager))).LoadComplete += new EventHandler(this.Plugins_LoadComplete);
  }

  public void Unload()
  {
  }

  public string Name => IMHRootNodeDescriptor.RootNodeDescriptorCaption;

  public void LoadConfiguration(IConfigurationManager configurationManager)
  {
  }

  public void SaveConfiguration(IConfigurationManager configurationManager)
  {
  }

  private void Plugins_LoadComplete(object sender, EventArgs e)
  {
    if (ServiceUtils.GetService<IImbaseSelector>((object) ApplicationServices.Container, false) == null)
      throw new Exception(LocalizationHolder.rm.GetString("IMH_LoadError_Msg"));
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetCustomService(typeof (IIMHIndexingService)) is IIMHIndexingService))
        throw new Exception(LocalizationHolder.rm.GetString("IMH_NullServerPlugin"));
      int appId = 343;
      byte[][] numArray1 = new byte[32 /*0x20*/][]
      {
        new byte[16 /*0x10*/]
        {
          (byte) 90,
          (byte) 146,
          (byte) 254,
          byte.MaxValue,
          (byte) 246,
          (byte) 145,
          (byte) 49,
          (byte) 114,
          (byte) 8,
          (byte) 29,
          (byte) 14,
          (byte) 145,
          (byte) 219,
          (byte) 164,
          (byte) 84,
          (byte) 228
        },
        new byte[16 /*0x10*/]
        {
          (byte) 170,
          (byte) 121,
          (byte) 166,
          (byte) 181,
          (byte) 46,
          (byte) 249,
          (byte) 227,
          (byte) 181,
          (byte) 58,
          (byte) 75,
          (byte) 124,
          (byte) 122,
          (byte) 151,
          (byte) 50,
          (byte) 33,
          (byte) 54
        },
        new byte[16 /*0x10*/]
        {
          (byte) 80 /*0x50*/,
          (byte) 36,
          (byte) 206,
          (byte) 20,
          (byte) 69,
          (byte) 170,
          (byte) 83,
          (byte) 209,
          (byte) 85,
          (byte) 236,
          (byte) 90,
          (byte) 178,
          (byte) 210,
          (byte) 236,
          (byte) 121,
          (byte) 134
        },
        new byte[16 /*0x10*/]
        {
          (byte) 30,
          (byte) 104,
          (byte) 219,
          (byte) 104,
          (byte) 57,
          (byte) 37,
          (byte) 161,
          (byte) 18,
          (byte) 154,
          (byte) 127 /*0x7F*/,
          (byte) 89,
          (byte) 115,
          (byte) 108,
          (byte) 227,
          (byte) 3,
          (byte) 163
        },
        new byte[16 /*0x10*/]
        {
          (byte) 73,
          (byte) 52,
          (byte) 175,
          (byte) 11,
          (byte) 181,
          (byte) 136,
          (byte) 250,
          (byte) 72,
          (byte) 206,
          (byte) 213,
          (byte) 180,
          (byte) 12,
          (byte) 144 /*0x90*/,
          (byte) 102,
          (byte) 138,
          (byte) 25
        },
        new byte[16 /*0x10*/]
        {
          (byte) 230,
          (byte) 164,
          (byte) 169,
          (byte) 88,
          (byte) 77,
          (byte) 190,
          (byte) 62,
          (byte) 66,
          (byte) 248,
          (byte) 27,
          (byte) 94,
          (byte) 164,
          (byte) 84,
          (byte) 235,
          (byte) 200,
          (byte) 150
        },
        new byte[16 /*0x10*/]
        {
          (byte) 166,
          (byte) 209,
          (byte) 204,
          (byte) 3,
          (byte) 224 /*0xE0*/,
          (byte) 172,
          (byte) 215,
          (byte) 175,
          (byte) 63 /*0x3F*/,
          (byte) 44,
          (byte) 170,
          (byte) 9,
          (byte) 54,
          (byte) 126,
          (byte) 160 /*0xA0*/,
          (byte) 27
        },
        new byte[16 /*0x10*/]
        {
          (byte) 16 /*0x10*/,
          (byte) 13,
          (byte) 46,
          (byte) 38,
          (byte) 32 /*0x20*/,
          (byte) 6,
          (byte) 74,
          (byte) 213,
          (byte) 191,
          (byte) 155,
          (byte) 174,
          (byte) 209,
          (byte) 88,
          (byte) 110,
          (byte) 159,
          (byte) 136
        },
        new byte[16 /*0x10*/]
        {
          (byte) 163,
          (byte) 136,
          (byte) 72,
          (byte) 166,
          (byte) 245,
          (byte) 72,
          (byte) 47,
          (byte) 200,
          (byte) 41,
          (byte) 134,
          (byte) 70,
          (byte) 111,
          (byte) 246,
          (byte) 71,
          (byte) 65,
          (byte) 239
        },
        new byte[16 /*0x10*/]
        {
          (byte) 64 /*0x40*/,
          (byte) 125,
          (byte) 227,
          (byte) 60,
          (byte) 11,
          (byte) 128 /*0x80*/,
          (byte) 1,
          (byte) 156,
          (byte) 130,
          (byte) 86,
          (byte) 1,
          (byte) 53,
          (byte) 68,
          (byte) 229,
          (byte) 12,
          (byte) 37
        },
        new byte[16 /*0x10*/]
        {
          (byte) 98,
          (byte) 65,
          (byte) 11,
          (byte) 0,
          (byte) 31 /*0x1F*/,
          (byte) 117,
          (byte) 165,
          (byte) 157,
          (byte) 92,
          byte.MaxValue,
          (byte) 204,
          (byte) 136,
          (byte) 222,
          (byte) 241,
          (byte) 17,
          (byte) 79
        },
        new byte[16 /*0x10*/]
        {
          (byte) 154,
          (byte) 135,
          (byte) 253,
          (byte) 211,
          (byte) 245,
          (byte) 167,
          (byte) 127 /*0x7F*/,
          (byte) 247,
          (byte) 192 /*0xC0*/,
          (byte) 183,
          (byte) 35,
          (byte) 55,
          (byte) 204,
          (byte) 194,
          (byte) 185,
          (byte) 180
        },
        new byte[16 /*0x10*/]
        {
          (byte) 53,
          (byte) 152,
          (byte) 104,
          (byte) 7,
          (byte) 29,
          (byte) 227,
          (byte) 35,
          (byte) 93,
          (byte) 162,
          (byte) 4,
          (byte) 218,
          (byte) 90,
          (byte) 101,
          (byte) 104,
          (byte) 185,
          (byte) 247
        },
        new byte[16 /*0x10*/]
        {
          (byte) 15,
          (byte) 170,
          (byte) 228,
          (byte) 106,
          (byte) 53,
          (byte) 75,
          (byte) 133,
          (byte) 228,
          (byte) 252,
          (byte) 109,
          (byte) 150,
          (byte) 157,
          (byte) 132,
          (byte) 159,
          (byte) 120,
          (byte) 65
        },
        new byte[16 /*0x10*/]
        {
          (byte) 234,
          (byte) 187,
          (byte) 167,
          (byte) 244,
          (byte) 115,
          (byte) 160 /*0xA0*/,
          (byte) 252,
          (byte) 45,
          (byte) 226,
          (byte) 171,
          (byte) 73,
          (byte) 245,
          (byte) 124,
          (byte) 166,
          (byte) 233,
          (byte) 56
        },
        new byte[16 /*0x10*/]
        {
          (byte) 13,
          (byte) 9,
          (byte) 45,
          (byte) 114,
          (byte) 62,
          (byte) 78,
          (byte) 22,
          (byte) 137,
          (byte) 254,
          (byte) 170,
          (byte) 92,
          (byte) 162,
          (byte) 142,
          (byte) 83,
          (byte) 55,
          (byte) 13
        },
        new byte[16 /*0x10*/]
        {
          (byte) 117,
          (byte) 247,
          (byte) 58,
          (byte) 93,
          (byte) 222,
          (byte) 57,
          (byte) 209,
          (byte) 127 /*0x7F*/,
          (byte) 30,
          (byte) 173,
          (byte) 94,
          (byte) 26,
          (byte) 155,
          (byte) 75,
          (byte) 55,
          (byte) 244
        },
        new byte[16 /*0x10*/]
        {
          (byte) 106,
          (byte) 71,
          (byte) 22,
          (byte) 167,
          (byte) 44,
          (byte) 93,
          (byte) 243,
          (byte) 223,
          (byte) 118,
          (byte) 69,
          (byte) 74,
          (byte) 250,
          (byte) 10,
          (byte) 195,
          (byte) 209,
          (byte) 162
        },
        new byte[16 /*0x10*/]
        {
          (byte) 70,
          (byte) 212,
          (byte) 157,
          (byte) 35,
          (byte) 74,
          (byte) 29,
          (byte) 220,
          (byte) 148,
          (byte) 156,
          (byte) 58,
          (byte) 36,
          (byte) 114,
          (byte) 153,
          (byte) 46,
          (byte) 225,
          (byte) 251
        },
        new byte[16 /*0x10*/]
        {
          (byte) 5,
          (byte) 204,
          (byte) 47,
          (byte) 94,
          (byte) 126,
          (byte) 226,
          (byte) 88,
          (byte) 211,
          (byte) 138,
          (byte) 104,
          (byte) 7,
          (byte) 1,
          (byte) 89,
          (byte) 19,
          (byte) 213,
          (byte) 216
        },
        new byte[16 /*0x10*/]
        {
          (byte) 27,
          (byte) 161,
          (byte) 228,
          (byte) 110,
          (byte) 45,
          (byte) 92,
          (byte) 25,
          (byte) 176 /*0xB0*/,
          (byte) 233,
          (byte) 39,
          (byte) 214,
          (byte) 197,
          (byte) 65,
          (byte) 225,
          (byte) 146,
          (byte) 151
        },
        new byte[16 /*0x10*/]
        {
          (byte) 35,
          (byte) 74,
          (byte) 203,
          (byte) 202,
          (byte) 93,
          (byte) 147,
          (byte) 218,
          (byte) 32 /*0x20*/,
          (byte) 94,
          (byte) 47,
          (byte) 129,
          (byte) 229,
          (byte) 12,
          (byte) 99,
          (byte) 58,
          (byte) 177
        },
        new byte[16 /*0x10*/]
        {
          (byte) 116,
          (byte) 85,
          (byte) 175,
          (byte) 167,
          (byte) 8,
          (byte) 198,
          (byte) 238,
          (byte) 87,
          (byte) 27,
          (byte) 165,
          (byte) 192 /*0xC0*/,
          (byte) 164,
          (byte) 21,
          (byte) 95,
          (byte) 229,
          (byte) 239
        },
        new byte[16 /*0x10*/]
        {
          (byte) 8,
          (byte) 232,
          (byte) 229,
          (byte) 232,
          (byte) 63 /*0x3F*/,
          (byte) 57,
          (byte) 187,
          (byte) 216,
          (byte) 52,
          (byte) 124,
          (byte) 62,
          (byte) 34,
          (byte) 215,
          (byte) 202,
          (byte) 46,
          (byte) 196
        },
        new byte[16 /*0x10*/]
        {
          (byte) 204,
          (byte) 98,
          byte.MaxValue,
          (byte) 249,
          (byte) 102,
          (byte) 201,
          (byte) 198,
          (byte) 233,
          (byte) 187,
          (byte) 236,
          (byte) 57,
          (byte) 34,
          (byte) 70,
          (byte) 22,
          (byte) 83,
          (byte) 187
        },
        new byte[16 /*0x10*/]
        {
          (byte) 178,
          byte.MaxValue,
          (byte) 164,
          (byte) 188,
          (byte) 96 /*0x60*/,
          (byte) 47,
          (byte) 249,
          (byte) 202,
          (byte) 229,
          (byte) 245,
          (byte) 1,
          (byte) 182,
          (byte) 101,
          (byte) 204,
          (byte) 227,
          (byte) 202
        },
        new byte[16 /*0x10*/]
        {
          (byte) 168,
          (byte) 141,
          (byte) 253,
          (byte) 68,
          (byte) 127 /*0x7F*/,
          (byte) 57,
          (byte) 242,
          (byte) 84,
          (byte) 179,
          (byte) 141,
          (byte) 82,
          (byte) 136,
          (byte) 155,
          (byte) 117,
          (byte) 155,
          (byte) 137
        },
        new byte[16 /*0x10*/]
        {
          (byte) 163,
          (byte) 219,
          (byte) 33,
          (byte) 9,
          (byte) 149,
          (byte) 23,
          (byte) 187,
          (byte) 177,
          (byte) 130,
          (byte) 215,
          (byte) 110,
          (byte) 100,
          (byte) 219,
          (byte) 168,
          (byte) 140,
          (byte) 90
        },
        new byte[16 /*0x10*/]
        {
          (byte) 66,
          (byte) 230,
          (byte) 249,
          (byte) 223,
          (byte) 225,
          (byte) 152,
          (byte) 48 /*0x30*/,
          (byte) 170,
          (byte) 142,
          (byte) 197,
          (byte) 236,
          (byte) 27,
          (byte) 140,
          (byte) 204,
          (byte) 16 /*0x10*/,
          (byte) 4
        },
        new byte[16 /*0x10*/]
        {
          (byte) 24,
          (byte) 7,
          (byte) 187,
          (byte) 229,
          (byte) 179,
          (byte) 153,
          (byte) 220,
          (byte) 58,
          (byte) 197,
          (byte) 174,
          (byte) 108,
          (byte) 191,
          (byte) 8,
          (byte) 244,
          (byte) 122,
          (byte) 16 /*0x10*/
        },
        new byte[16 /*0x10*/]
        {
          (byte) 82,
          (byte) 5,
          (byte) 251,
          (byte) 135,
          (byte) 97,
          (byte) 161,
          (byte) 112 /*0x70*/,
          (byte) 146,
          (byte) 111,
          (byte) 170,
          (byte) 70,
          (byte) 217,
          (byte) 118,
          (byte) 62,
          (byte) 78,
          (byte) 70
        },
        new byte[16 /*0x10*/]
        {
          (byte) 56,
          (byte) 224 /*0xE0*/,
          (byte) 11,
          (byte) 84,
          (byte) 216,
          (byte) 51,
          (byte) 142,
          (byte) 117,
          (byte) 35,
          (byte) 144 /*0x90*/,
          (byte) 135,
          (byte) 2,
          (byte) 83,
          (byte) 249,
          (byte) 114,
          (byte) 195
        }
      };
      if (!(this._srvProvider.GetService(typeof (IProtectionKey)) is IProtectionKey service1))
        throw new Exception(LocalizationHolder.rm.GetString("IMH_ServiceError_IProtectionKey"));
      int index1 = (Environment.TickCount & 15) * 2;
      byte[] queryData = numArray1[index1];
      byte[] numArray2 = numArray1[index1 + 1];
      byte[] response = new byte[numArray2.Length];
      service1.Query(true, appId, queryData, response);
      int length = queryData.Length;
      for (int index2 = 0; index2 < length; ++index2)
      {
        if ((int) numArray2[index2] != (int) response[index2])
          throw new Exception(LocalizationHolder.rm.GetString("IMH_KeyProtection_Error"));
      }
      INamedImageList service2 = this._srvProvider.GetService(typeof (INamedImageList)) as INamedImageList;
      IGuidMapper service3 = this._srvProvider.GetService(typeof (IGuidMapper)) as IGuidMapper;
      IFactory service4 = this._srvProvider.GetService(typeof (IFactory)) as IFactory;
      IMHRootViewProvider provider = new IMHRootViewProvider();
      if (service3 != null && service4 != null)
      {
        Consts.IMHRootNodeCategoryID = service3.Register(Consts.IMHRootNodeGuid);
        service4.AddNodeType(Consts.IMHRootNodeCategoryID, typeof (IMHRootNode));
        service4.AddViewsProvider(Consts.IMHRootNodeCategoryID, (IViewsProvider) provider);
        service4.AddGlobalNode(new Guid("{11CF6939-0AD4-4271-90FB-395ADB7BC0FC}"), (IDescriptor) new IMHRootNodeDescriptor(), 70);
        Consts.IMHMaterialsHandbookNodeCategoryID = service3.Register(Consts.IMHMaterialsHandbookNodeGuid);
        service4.AddNodeType(Consts.IMHMaterialsHandbookNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHMaterialsHandbookNodeCategoryID, (IViewsProvider) provider);
        Consts.IMHMaterialsNodeCategoryID = service3.Register(Consts.IMHMaterialsNodeGuid);
        service4.AddNodeType(Consts.IMHMaterialsNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHMaterialsNodeCategoryID, (IViewsProvider) provider);
        Consts.IMHAssortmentNodeCategoryID = service3.Register(Consts.IMHAssortmentNodeGuid);
        service4.AddNodeType(Consts.IMHAssortmentNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHAssortmentNodeCategoryID, (IViewsProvider) provider);
        Consts.IMHProfilesNodeCategoryID = service3.Register(Consts.IMHProfilesNodeGuid);
        service4.AddNodeType(Consts.IMHProfilesNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHProfilesNodeCategoryID, (IViewsProvider) provider);
        Consts.IMHStandardNodeCategoryID = service3.Register(Consts.IMHStandardNodeGuid);
        service4.AddNodeType(Consts.IMHStandardNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHStandardNodeCategoryID, (IViewsProvider) provider);
        Consts.IMHGluesHandbookNodeCategoryID = service3.Register(Consts.IMHGluesHandbookNodeGuid);
        service4.AddNodeType(Consts.IMHGluesHandbookNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHGluesHandbookNodeCategoryID, (IViewsProvider) provider);
        Consts.IMHCoatingsHandbookNodeCategoryID = service3.Register(Consts.IMHCoatingsHandbookNodeGuid);
        service4.AddNodeType(Consts.IMHCoatingsHandbookNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHCoatingsHandbookNodeCategoryID, (IViewsProvider) provider);
        Consts.IMHCoatingsVarietiesNodeCategoryID = service3.Register(Consts.IMHCoatingsVarietiesNodeGuid);
        service4.AddNodeType(Consts.IMHCoatingsVarietiesNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHCoatingsVarietiesNodeCategoryID, (IViewsProvider) provider);
        Consts.IMHDetailsMaterialNodeCategoryID = service3.Register(Consts.IMHDetailsMaterialNodeGuid);
        service4.AddNodeType(Consts.IMHDetailsMaterialNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHDetailsMaterialNodeCategoryID, (IViewsProvider) provider);
        Consts.IMHOilHandbookNodeCategoryID = service3.Register(Consts.IMHOilHandbookNodeGuid);
        service4.AddNodeType(Consts.IMHOilHandbookNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHOilHandbookNodeCategoryID, (IViewsProvider) provider);
        Consts.IMHVarnishHandbookNodeCategoryID = service3.Register(Consts.IMHVarnishHandbookNodeGuid);
        service4.AddNodeType(Consts.IMHVarnishHandbookNodeCategoryID, typeof (VirtualNode));
        service4.AddViewsProvider(Consts.IMHVarnishHandbookNodeCategoryID, (IViewsProvider) provider);
        IMHHelper.ChildNodesColl = new Dictionary<int, List<NodeInfo>>(4);
        IMHHelper.ChildNodesColl.Add(Consts.IMHRootNodeCategoryID, new List<NodeInfo>((IEnumerable<NodeInfo>) new NodeInfo[5]
        {
          new NodeInfo(Consts.IMHMaterialsHandbookNodeGuid, Consts.IMHMaterialsHandbookNodeCategoryID, LocalizationHolder.rm.GetString("IMH_MaterialsHandbookNode_Caption")),
          new NodeInfo(Consts.IMHGluesHandbookNodeGuid, Consts.IMHGluesHandbookNodeCategoryID, LocalizationHolder.rm.GetString("IMH_GluesHandbookNode_Caption")),
          new NodeInfo(Consts.IMHCoatingsHandbookNodeGuid, Consts.IMHCoatingsHandbookNodeCategoryID, LocalizationHolder.rm.GetString("IMH_CoatingsHandbookNode_Caption")),
          new NodeInfo(Consts.IMHOilHandbookNodeGuid, Consts.IMHOilHandbookNodeCategoryID, LocalizationHolder.rm.GetString("IMH_OilHandbookNode_Caption")),
          new NodeInfo(Consts.IMHVarnishHandbookNodeGuid, Consts.IMHVarnishHandbookNodeCategoryID, LocalizationHolder.rm.GetString("IMH_VarnishHandbookNode_Caption"))
        }));
        IMHHelper.ChildNodesColl.Add(Consts.IMHMaterialsHandbookNodeCategoryID, new List<NodeInfo>((IEnumerable<NodeInfo>) new NodeInfo[4]
        {
          new NodeInfo(Consts.IMHMaterialsNodeGuid, Consts.IMHMaterialsNodeCategoryID, LocalizationHolder.rm.GetString("IMH_MaterialsNode_Caption")),
          new NodeInfo(Consts.IMHAssortmentNodeGuid, Consts.IMHAssortmentNodeCategoryID, LocalizationHolder.rm.GetString("IMH_AssortmentsNode_Caption")),
          new NodeInfo(Consts.IMHProfilesNodeGuid, Consts.IMHProfilesNodeCategoryID, LocalizationHolder.rm.GetString("IMH_ProfilesNode_Caption")),
          new NodeInfo(Consts.IMHStandardNodeGuid, Consts.IMHStandardNodeCategoryID, LocalizationHolder.rm.GetString("IMH_StandardNode_Caption"))
        }));
        IMHHelper.ChildNodesColl.Add(Consts.IMHCoatingsHandbookNodeCategoryID, new List<NodeInfo>((IEnumerable<NodeInfo>) new NodeInfo[2]
        {
          new NodeInfo(Consts.IMHCoatingsVarietiesNodeGuid, Consts.IMHCoatingsVarietiesNodeCategoryID, LocalizationHolder.rm.GetString("IMH_CoatingsVarietiesNode_Caption")),
          new NodeInfo(Consts.IMHDetailsMaterialNodeGuid, Consts.IMHDetailsMaterialNodeCategoryID, LocalizationHolder.rm.GetString("IMH_SearchCoatingNode_Caption"))
        }));
        IMHHelper.ChildNodesColl.Add(Consts.IMHStandardNodeCategoryID, new List<NodeInfo>((IEnumerable<NodeInfo>) new NodeInfo[2]
        {
          new NodeInfo(Consts.IMHMaterialsNodeGuid, Consts.IMHMaterialsNodeCategoryID, LocalizationHolder.rm.GetString("IMH_MaterialsNode_Caption")),
          new NodeInfo(Consts.IMHAssortmentNodeGuid, Consts.IMHAssortmentNodeCategoryID, LocalizationHolder.rm.GetString("IMH_AssortmentsNode_Caption"))
        }));
        Consts.MaterialObjTypeID = MetaDataHelper.GetObjectTypeID("cad00171-306c-11d8-b4e9-00304f19f545");
        Consts.IMHStandartFolderCategoryID = service3.Register(Consts.IMHStandartFolderNodeGuid);
        service4.AddViewsProvider(Consts.IMHStandartFolderCategoryID, (IViewsProvider) provider);
        service4.AddViewsProvider(1, Intermech.Imbase.Consts.ImbaseFolderTypeID, (IViewsProvider) provider);
        this.LoadResources(service2);
      }
      IMainMenuService service5 = ServiceUtils.GetService<IMainMenuService>((object) ApplicationServices.Container, false);
      if (service5 != null)
      {
        MenuButtonItem menuButtonItem = new MenuButtonItem(IMHRootNodeDescriptor.RootNodeDescriptorCaption, new EventHandler(this.OnViewRootNode));
        menuButtonItem.Image = service2.ImageList.Images[service2.ImageIndex("imgMaterialsHandbook")];
        service5.RegisterMenuItems(MainMenuItemSite.Applications, MainMenuItemPosition.Default, menuButtonItem);
      }
      if (this._srvProvider.GetService(typeof (INavigationBar)) is INavigationBar service6 && service6.FindPane("appPane") is IAppPane pane)
        pane.Add(IMHRootNodeDescriptor.RootNodeDescriptorCaption, new EventHandler(this.OnViewRootNode), service2.ImageIndex("imgMaterialsHandbook"));
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(Consts.NSIAdminRoleGUID);
      if (!objectInfo.Empty)
        Consts.NSIAdminRoleId = objectInfo.ObjectID;
      ICurrentUserAndRole service7 = ServicesManager.GetService(typeof (ICurrentUserAndRole)) as ICurrentUserAndRole;
      if (service7.IsAdmin || service7.RoleID == Consts.NSIAdminRoleId)
      {
        IMHSystemSettingsViewPage settingsViewPage = new IMHSystemSettingsViewPage(this._srvProvider);
      }
      ServicesManager.AddService(typeof (IIMHSelector), (object) new IMHSelector());
      this.RegisterViews();
      IViewsManagerService service8 = ServiceUtils.GetService<IViewsManagerService>((object) ApplicationServices.Container, false);
      if (service8 == null)
        return;
      service8.OnActivateView += new Intermech.Interfaces.Client.ActivateViewEventHandler(this.ActivateViewEventHandler);
    }
  }

  private void OnViewRootNode(object sender, EventArgs e)
  {
    bool flag = false;
    DockManager service = ServiceUtils.GetService<DockManager>((object) ApplicationServices.Container, false);
    if (service != null)
    {
      DockControl[] dockControls = service.GetDockControls();
      if (dockControls != null && dockControls.Length != 0)
      {
        foreach (DockControl dockControl in dockControls)
        {
          if (dockControl is NavWindowBase navWindowBase && navWindowBase.RootDescriptor is IMHRootNodeDescriptor)
          {
            dockControl.Activate();
            flag = true;
            break;
          }
        }
      }
    }
    if (flag)
      return;
    Intermech.Navigator.Utils.OpenNewWindow((IDescriptor) new IMHRootNodeDescriptor(), (IServiceProvider) null, new GetSupportedColumnsEventHandler(Intermech.Navigator.Utils.DefaultSupportedColumnsObjects));
  }

  private void LoadResources(INamedImageList namedImgList)
  {
    ICategoryTypeIconService service = this._srvProvider.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    if (namedImgList != null)
    {
      Assembly assembly = typeof (IMHStartup).Assembly;
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialsHandbook.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoMaterialsHandbook");
          service?.AddIcon(resourceData, Consts.IMHRootNodeCategoryID);
        }
      }
      using (Bitmap resourceData = ResourceHelper.GetResourceData<Bitmap>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialsHandbook.png"))
      {
        if (resourceData != null)
        {
          resourceData.MakeTransparent();
          namedImgList.Add((Image) resourceData, "imgMaterialsHandbook");
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.HandbookGlues.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoHandbookGlues");
          service?.AddIcon(resourceData, Consts.IMHGluesHandbookNodeCategoryID);
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.HandbookMaterials.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoHandbookMaterials");
          service?.AddIcon(resourceData, Consts.IMHMaterialsHandbookNodeCategoryID);
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.HandbookCoatings.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoHandbookCoatings");
          service?.AddIcon(resourceData, Consts.IMHCoatingsHandbookNodeCategoryID);
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.Standarts.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoStandarts");
          service?.AddIcon(resourceData, Consts.IMHStandardNodeCategoryID);
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.Profiles.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoProfiles");
          service?.AddIcon(resourceData, Consts.IMHProfilesNodeCategoryID);
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.Assortment.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoAssortment");
          service?.AddIcon(resourceData, Consts.IMHAssortmentNodeCategoryID);
        }
      }
      using (Bitmap resourceData = ResourceHelper.GetResourceData<Bitmap>(assembly, "Intermech.MaterialsHandbook.Resources.Assortment.png"))
      {
        if (resourceData != null)
        {
          resourceData.MakeTransparent();
          namedImgList.Add((Image) resourceData, "imgAssortment");
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.Coating.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoCoating");
          service?.AddIcon(resourceData, Consts.IMHCoatingsVarietiesNodeCategoryID);
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialCoating.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoMaterialCoating");
          service?.AddIcon(resourceData, Consts.IMHDetailsMaterialNodeCategoryID);
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.Varnish.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoVarnish");
          service?.AddIcon(resourceData, Consts.IMHVarnishHandbookNodeCategoryID);
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.Oils.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoOils");
          service?.AddIcon(resourceData, Consts.IMHOilHandbookNodeCategoryID);
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialProperties.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoMaterialProperties");
          service?.AddIcon(resourceData, 0);
        }
      }
      using (Bitmap resourceData = ResourceHelper.GetResourceData<Bitmap>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialProperties.bmp"))
      {
        if (resourceData != null)
        {
          resourceData.MakeTransparent();
          namedImgList.Add((Image) resourceData, "imgMaterialProperties");
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialSubstitutes.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoMaterialSubstitutes");
          service?.AddIcon(resourceData, 0);
        }
      }
      using (Bitmap resourceData = ResourceHelper.GetResourceData<Bitmap>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialSubstitutes.bmp"))
      {
        if (resourceData != null)
        {
          resourceData.MakeTransparent();
          namedImgList.Add((Image) resourceData, "imgMaterialSubstitutes");
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialFavourites.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoMaterialFavourites");
          service?.AddIcon(resourceData, 0);
        }
      }
      using (Bitmap resourceData = ResourceHelper.GetResourceData<Bitmap>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialFavourites.bmp"))
      {
        if (resourceData != null)
        {
          resourceData.MakeTransparent();
          namedImgList.Add((Image) resourceData, "imgMaterialFavourites");
        }
      }
      using (Icon resourceData = ResourceHelper.GetResourceData<Icon>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialFavouritesAdd.ico"))
      {
        if (resourceData != null)
        {
          namedImgList.Add(resourceData, "icoMaterialFavouritesAdd");
          service?.AddIcon(resourceData, 0);
        }
      }
      using (Bitmap resourceData = ResourceHelper.GetResourceData<Bitmap>(assembly, "Intermech.MaterialsHandbook.Resources.MaterialFavouritesAdd.bmp"))
      {
        if (resourceData != null)
        {
          resourceData.MakeTransparent();
          namedImgList.Add((Image) resourceData, "imgMaterialFavouritesAdd");
        }
      }
      using (Bitmap resourceData = ResourceHelper.GetResourceData<Bitmap>(assembly, "Intermech.MaterialsHandbook.Resources.Glue.png"))
      {
        if (resourceData != null)
        {
          resourceData.MakeTransparent();
          namedImgList.Add((Image) resourceData, "imgGlue");
        }
      }
    }
    if (service == null)
      return;
    Icon icon1 = service.GetIcon(1, MetaDataHelper.GetObjectTypeID("cad00171-306c-11d8-b4e9-00304f19f545"));
    if (icon1 != null)
    {
      namedImgList?.Add(icon1, "icoMaterials");
      service.AddIcon(icon1, Consts.IMHMaterialsNodeCategoryID);
    }
    if (Consts.IMHStandartFolderCategoryID == -1)
      return;
    Icon icon2 = service.GetIcon(1, Intermech.Imbase.Consts.ImbaseFolderTypeID);
    if (icon2 == null)
      return;
    service.AddIcon(icon2, Consts.IMHStandartFolderCategoryID);
  }

  internal void RegisterViews()
  {
    AdjustableViewsHelper.RegisterView("IMHView", LocalizationHolder.rm.GetString("IMH_RootNode_Caption"), "", "", "imgMaterialsHandbook", true, 0);
    AdjustableViewsHelper.RegisterView("MaterialsChildrenView", LocalizationHolder.rm.GetString("IMH_RootNode_Caption"), "", "", "imgContains", true, 0);
  }

  private void ActivateViewEventHandler(object sender, ActivateViewEventArgs e)
  {
    if (e == null || e.NewSelectedNodes == null || e.NewSelectedNodes.Count <= 0 || e.NewSelectedNodes[0].TypeID != Intermech.Imbase.Consts.ImbaseFolderTypeID || !(sender is PageViewsManager pageViewsManager))
      return;
    NavigatorTreeView navigatorTreeView = (NavigatorTreeView) null;
    if (pageViewsManager.ParentForm is Intermech.Navigator.Controls.SelectionWindow parentForm)
    {
      navigatorTreeView = parentForm.NavTreeView;
    }
    else
    {
      ICurrentNavWindow service = ServiceUtils.GetService<ICurrentNavWindow>((object) ApplicationServices.Container, false);
      if (service != null)
        navigatorTreeView = service.TreeView as NavigatorTreeView;
    }
    if (!(navigatorTreeView?.FocusedNode?.Handler is FolderNode))
      return;
    navigatorTreeView.PopulateNode(navigatorTreeView.FocusedNode);
    e.NewViewName = navigatorTreeView.FocusedNode.Children.Count > 0 ? "MaterialsChildrenView" : "IMHView";
  }
}
