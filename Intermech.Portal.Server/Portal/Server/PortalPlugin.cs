// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.PortalPlugin
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Plugins;
using Intermech.Interfaces.Portal;
using Intermech.Interfaces.Server;
using Intermech.Interfaces.WebPortal;
using Intermech.Kernel;
using Intermech.Kernel.Search;
using Intermech.Localization;
using Intermech.Protection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Runtime.Remoting;

#nullable disable
namespace Intermech.Portal.Server;

public class PortalPlugin : IPackage
{
  public void Load(IServiceProvider serviceProvider)
  {
    int appId = 364;
    byte[][] numArray1 = new byte[32 /*0x20*/][]
    {
      new byte[16 /*0x10*/]
      {
        (byte) 61,
        (byte) 70,
        (byte) 120,
        (byte) 225,
        (byte) 181,
        (byte) 165,
        (byte) 244,
        (byte) 183,
        (byte) 251,
        (byte) 128 /*0x80*/,
        (byte) 13,
        (byte) 184,
        (byte) 193,
        (byte) 96 /*0x60*/,
        (byte) 125,
        (byte) 241
      },
      new byte[16 /*0x10*/]
      {
        (byte) 131,
        (byte) 227,
        (byte) 226,
        (byte) 198,
        (byte) 224 /*0xE0*/,
        (byte) 150,
        (byte) 32 /*0x20*/,
        (byte) 18,
        (byte) 232,
        (byte) 244,
        (byte) 89,
        (byte) 215,
        (byte) 57,
        (byte) 110,
        (byte) 141,
        (byte) 197
      },
      new byte[16 /*0x10*/]
      {
        (byte) 102,
        (byte) 142,
        (byte) 127 /*0x7F*/,
        (byte) 124,
        (byte) 40,
        (byte) 66,
        (byte) 141,
        (byte) 18,
        (byte) 100,
        (byte) 72,
        (byte) 119,
        (byte) 20,
        (byte) 187,
        (byte) 61,
        (byte) 145,
        (byte) 186
      },
      new byte[16 /*0x10*/]
      {
        (byte) 118,
        (byte) 247,
        (byte) 11,
        (byte) 159,
        (byte) 219,
        (byte) 75,
        (byte) 236,
        (byte) 31 /*0x1F*/,
        (byte) 192 /*0xC0*/,
        (byte) 52,
        (byte) 127 /*0x7F*/,
        (byte) 110,
        (byte) 78,
        (byte) 242,
        (byte) 94,
        (byte) 153
      },
      new byte[16 /*0x10*/]
      {
        (byte) 53,
        (byte) 241,
        (byte) 228,
        (byte) 158,
        (byte) 19,
        (byte) 176 /*0xB0*/,
        (byte) 122,
        (byte) 144 /*0x90*/,
        (byte) 34,
        (byte) 231,
        (byte) 89,
        (byte) 124,
        (byte) 162,
        (byte) 224 /*0xE0*/,
        (byte) 22,
        (byte) 27
      },
      new byte[16 /*0x10*/]
      {
        (byte) 188,
        (byte) 183,
        (byte) 252,
        (byte) 3,
        (byte) 88,
        (byte) 130,
        (byte) 156,
        (byte) 225,
        (byte) 234,
        (byte) 220,
        (byte) 148,
        (byte) 111,
        (byte) 187,
        (byte) 157,
        (byte) 33,
        (byte) 16 /*0x10*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 37,
        (byte) 248,
        (byte) 175,
        (byte) 113,
        (byte) 3,
        (byte) 137,
        (byte) 224 /*0xE0*/,
        (byte) 251,
        (byte) 185,
        (byte) 102,
        (byte) 125,
        (byte) 109,
        (byte) 45,
        (byte) 224 /*0xE0*/,
        (byte) 252,
        (byte) 190
      },
      new byte[16 /*0x10*/]
      {
        (byte) 212,
        (byte) 195,
        (byte) 82,
        (byte) 131,
        (byte) 74,
        (byte) 45,
        (byte) 38,
        (byte) 6,
        (byte) 172,
        (byte) 164,
        (byte) 196,
        (byte) 47,
        (byte) 217,
        (byte) 203,
        (byte) 145,
        (byte) 205
      },
      new byte[16 /*0x10*/]
      {
        (byte) 238,
        (byte) 82,
        (byte) 130,
        (byte) 208 /*0xD0*/,
        (byte) 123,
        (byte) 223,
        (byte) 221,
        (byte) 182,
        (byte) 20,
        (byte) 238,
        (byte) 89,
        (byte) 65,
        (byte) 233,
        (byte) 16 /*0x10*/,
        (byte) 112 /*0x70*/,
        (byte) 132
      },
      new byte[16 /*0x10*/]
      {
        (byte) 233,
        (byte) 202,
        (byte) 232,
        (byte) 188,
        (byte) 172,
        (byte) 39,
        (byte) 236,
        (byte) 171,
        (byte) 171,
        (byte) 29,
        (byte) 73,
        (byte) 129,
        (byte) 240 /*0xF0*/,
        (byte) 127 /*0x7F*/,
        (byte) 161,
        (byte) 249
      },
      new byte[16 /*0x10*/]
      {
        (byte) 50,
        (byte) 92,
        (byte) 238,
        (byte) 194,
        (byte) 142,
        (byte) 31 /*0x1F*/,
        (byte) 182,
        (byte) 172,
        (byte) 16 /*0x10*/,
        (byte) 233,
        (byte) 229,
        (byte) 173,
        (byte) 51,
        (byte) 13,
        (byte) 99,
        (byte) 65
      },
      new byte[16 /*0x10*/]
      {
        (byte) 67,
        (byte) 49,
        (byte) 113,
        (byte) 238,
        (byte) 71,
        (byte) 79,
        (byte) 229,
        (byte) 139,
        (byte) 126,
        (byte) 152,
        (byte) 241,
        (byte) 194,
        (byte) 214,
        (byte) 208 /*0xD0*/,
        (byte) 33,
        (byte) 248
      },
      new byte[16 /*0x10*/]
      {
        (byte) 234,
        (byte) 154,
        (byte) 141,
        (byte) 79,
        (byte) 93,
        (byte) 69,
        (byte) 160 /*0xA0*/,
        (byte) 70,
        (byte) 26,
        (byte) 249,
        (byte) 93,
        (byte) 21,
        (byte) 47,
        (byte) 158,
        (byte) 17,
        (byte) 169
      },
      new byte[16 /*0x10*/]
      {
        byte.MaxValue,
        (byte) 108,
        (byte) 150,
        (byte) 17,
        (byte) 98,
        (byte) 80 /*0x50*/,
        (byte) 217,
        (byte) 3,
        (byte) 56,
        (byte) 145,
        (byte) 186,
        (byte) 0,
        (byte) 71,
        (byte) 15,
        (byte) 143,
        (byte) 159
      },
      new byte[16 /*0x10*/]
      {
        (byte) 245,
        (byte) 248,
        (byte) 59,
        (byte) 208 /*0xD0*/,
        (byte) 241,
        (byte) 57,
        (byte) 48 /*0x30*/,
        (byte) 207,
        (byte) 155,
        (byte) 214,
        (byte) 191,
        (byte) 145,
        (byte) 249,
        (byte) 48 /*0x30*/,
        (byte) 130,
        (byte) 6
      },
      new byte[16 /*0x10*/]
      {
        (byte) 165,
        (byte) 217,
        (byte) 113,
        (byte) 151,
        (byte) 73,
        (byte) 119,
        (byte) 38,
        (byte) 115,
        (byte) 187,
        (byte) 145,
        (byte) 157,
        (byte) 101,
        (byte) 252,
        (byte) 149,
        (byte) 236,
        (byte) 32 /*0x20*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 139,
        (byte) 23,
        (byte) 87,
        (byte) 78,
        (byte) 130,
        (byte) 33,
        (byte) 38,
        (byte) 236,
        (byte) 188,
        (byte) 168,
        (byte) 168,
        (byte) 149,
        (byte) 194,
        (byte) 161,
        (byte) 124,
        (byte) 114
      },
      new byte[16 /*0x10*/]
      {
        (byte) 204,
        (byte) 73,
        (byte) 184,
        (byte) 210,
        (byte) 51,
        (byte) 42,
        (byte) 213,
        (byte) 110,
        (byte) 195,
        (byte) 168,
        (byte) 31 /*0x1F*/,
        (byte) 235,
        (byte) 142,
        (byte) 108,
        (byte) 115,
        (byte) 198
      },
      new byte[16 /*0x10*/]
      {
        (byte) 215,
        (byte) 60,
        (byte) 183,
        (byte) 58,
        (byte) 210,
        (byte) 146,
        (byte) 80 /*0x50*/,
        (byte) 177,
        (byte) 106,
        (byte) 190,
        (byte) 179,
        (byte) 134,
        (byte) 133,
        (byte) 14,
        (byte) 76,
        (byte) 234
      },
      new byte[16 /*0x10*/]
      {
        (byte) 171,
        (byte) 73,
        (byte) 155,
        (byte) 153,
        (byte) 157,
        (byte) 15,
        (byte) 161,
        (byte) 24,
        (byte) 95,
        (byte) 102,
        (byte) 47,
        (byte) 102,
        (byte) 124,
        (byte) 35,
        (byte) 204,
        (byte) 246
      },
      new byte[16 /*0x10*/]
      {
        (byte) 118,
        (byte) 87,
        (byte) 137,
        (byte) 23,
        (byte) 71,
        (byte) 195,
        (byte) 38,
        (byte) 212,
        (byte) 210,
        (byte) 78,
        (byte) 20,
        (byte) 73,
        (byte) 145,
        (byte) 154,
        (byte) 12,
        (byte) 250
      },
      new byte[16 /*0x10*/]
      {
        (byte) 124,
        (byte) 195,
        (byte) 61,
        (byte) 29,
        (byte) 121,
        (byte) 224 /*0xE0*/,
        (byte) 49,
        (byte) 19,
        (byte) 111,
        (byte) 196,
        (byte) 29,
        (byte) 185,
        (byte) 254,
        (byte) 113,
        (byte) 45,
        (byte) 135
      },
      new byte[16 /*0x10*/]
      {
        (byte) 89,
        (byte) 77,
        (byte) 185,
        (byte) 189,
        (byte) 120,
        (byte) 72,
        (byte) 51,
        (byte) 249,
        (byte) 199,
        (byte) 165,
        (byte) 227,
        (byte) 195,
        (byte) 110,
        (byte) 159,
        (byte) 102,
        (byte) 173
      },
      new byte[16 /*0x10*/]
      {
        (byte) 19,
        (byte) 162,
        (byte) 241,
        (byte) 50,
        (byte) 45,
        (byte) 13,
        (byte) 29,
        (byte) 128 /*0x80*/,
        (byte) 21,
        (byte) 27,
        (byte) 226,
        (byte) 192 /*0xC0*/,
        (byte) 94,
        (byte) 211,
        (byte) 232,
        (byte) 152
      },
      new byte[16 /*0x10*/]
      {
        (byte) 91,
        (byte) 189,
        (byte) 30,
        (byte) 106,
        (byte) 24,
        (byte) 101,
        (byte) 43,
        (byte) 20,
        (byte) 21,
        (byte) 227,
        (byte) 103,
        (byte) 131,
        (byte) 125,
        (byte) 7,
        (byte) 32 /*0x20*/,
        (byte) 207
      },
      new byte[16 /*0x10*/]
      {
        (byte) 250,
        (byte) 51,
        (byte) 21,
        (byte) 177,
        byte.MaxValue,
        (byte) 230,
        (byte) 38,
        (byte) 1,
        (byte) 194,
        (byte) 185,
        (byte) 25,
        (byte) 99,
        (byte) 26,
        (byte) 69,
        (byte) 10,
        (byte) 176 /*0xB0*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 177,
        (byte) 185,
        (byte) 45,
        (byte) 56,
        (byte) 84,
        (byte) 143,
        (byte) 114,
        (byte) 45,
        (byte) 35,
        (byte) 144 /*0x90*/,
        (byte) 79,
        (byte) 161,
        (byte) 224 /*0xE0*/,
        (byte) 98,
        (byte) 101,
        (byte) 50
      },
      new byte[16 /*0x10*/]
      {
        (byte) 8,
        (byte) 119,
        (byte) 103,
        (byte) 134,
        (byte) 39,
        (byte) 59,
        (byte) 72,
        (byte) 165,
        (byte) 129,
        (byte) 11,
        (byte) 94,
        (byte) 79,
        (byte) 108,
        (byte) 91,
        (byte) 159,
        (byte) 140
      },
      new byte[16 /*0x10*/]
      {
        (byte) 232,
        (byte) 102,
        (byte) 237,
        (byte) 211,
        (byte) 124,
        (byte) 162,
        (byte) 225,
        (byte) 244,
        (byte) 34,
        (byte) 165,
        (byte) 167,
        (byte) 148,
        (byte) 225,
        (byte) 176 /*0xB0*/,
        (byte) 123,
        (byte) 192 /*0xC0*/
      },
      new byte[16 /*0x10*/]
      {
        (byte) 185,
        (byte) 56,
        (byte) 139,
        (byte) 189,
        (byte) 213,
        (byte) 95,
        (byte) 176 /*0xB0*/,
        (byte) 119,
        (byte) 211,
        (byte) 197,
        (byte) 232,
        (byte) 167,
        (byte) 174,
        (byte) 252,
        (byte) 89,
        (byte) 22
      },
      new byte[16 /*0x10*/]
      {
        (byte) 133,
        (byte) 19,
        (byte) 17,
        (byte) 180,
        (byte) 114,
        (byte) 45,
        (byte) 89,
        (byte) 204,
        (byte) 75,
        (byte) 241,
        (byte) 71,
        (byte) 135,
        (byte) 21,
        (byte) 137,
        (byte) 51,
        (byte) 178
      },
      new byte[16 /*0x10*/]
      {
        (byte) 244,
        (byte) 199,
        (byte) 208 /*0xD0*/,
        (byte) 30,
        (byte) 16 /*0x10*/,
        (byte) 86,
        (byte) 10,
        (byte) 108,
        (byte) 65,
        (byte) 232,
        byte.MaxValue,
        (byte) 197,
        (byte) 106,
        (byte) 134,
        (byte) 16 /*0x10*/,
        (byte) 133
      }
    };
    IProtectionKey service1 = serviceProvider.GetService(typeof (IProtectionKey)) as IProtectionKey;
    (serviceProvider.GetService(typeof (ILicenser)) as ILicenser).AllocateLicense(appId);
    if (service1 == null)
      return;
    int index1 = (Environment.TickCount & 15) * 2;
    byte[] queryData = numArray1[index1];
    byte[] numArray2 = numArray1[index1 + 1];
    byte[] response = new byte[numArray2.Length];
    service1.Query(true, appId, queryData, response);
    int length = queryData.Length;
    for (int index2 = 0; index2 < length; ++index2)
    {
      if ((int) numArray2[index2] != (int) response[index2])
        return;
    }
    IUserSession sessionTemporaryClone = ((IDBTimedEvents) serviceProvider.GetService(typeof (IDBTimedEvents))).GetSystemSessionTemporaryClone("Portal.Load");
    try
    {
      ICreatorContainer service2 = ServerServices.GetService(typeof (IDBObjectService)) as ICreatorContainer;
      if (service2.GetCreator((object) new Guid("cad0149e-306c-11d8-b4e9-00304f19f545")) != null)
        throw new Exception($"Сервер приложений сконфигурирован для узла. Работа модуля \"{this.Name}\" невозможна!");
      PortalSettings serviceInstance = new PortalSettings();
      serviceInstance.Initialize(ConfigurationManager.AppSettings);
      ServerServices.AddService(typeof (PortalSettings), (object) serviceInstance);
      IDHelper.Initialize(sessionTemporaryClone);
      TempStorage.Initialize(serviceInstance.PortalFileStorage);
      RemotingServices.Marshal((MarshalByRefObject) new Intermech.Portal.Server.Portal(), "IMPortalService.rem");
      ((IObjectsDeleteAnalyzerService) ServerServices.GetService(typeof (IObjectsDeleteAnalyzerService))).RegisterAnalyzer((IObjectsDeleteAnalyzer) new PublishObjectsDeleteAnalyzer());
      (ServerServices.GetService(typeof (IDBRelationService)) as ICreatorContainer).AddCreator((object) PortalConsts.reltypePublish, (object) new DBPublishRelationCreator());
      DBPublishCollectionCreator creatorInstance1 = new DBPublishCollectionCreator();
      DBPublishObjectCreator creatorInstance2 = new DBPublishObjectCreator();
      List<Guid> childTypeGuids = ObjectTypesCacheHelper.GetChildTypeGuids(sessionTemporaryClone, MetaDataHelper.GetObjectTypeID(PortalConsts.objtypePublishObjects));
      ICreatorContainer service3 = ServerServices.GetService(typeof (IDBObjectCollectionService)) as ICreatorContainer;
      foreach (Guid creatorType in childTypeGuids)
      {
        service3.AddCreator((object) creatorType, (object) creatorInstance1);
        service2.AddCreator((object) creatorType, (object) creatorInstance2);
      }
      service2.AddCreator((object) PortalConsts.objtypeChanges, (object) new DBChangesObjectCreator());
      service2.AddCreator((object) new Guid("cad0149e-306c-11d8-b4e9-00304f19f545"), (object) new DBPortalTaskCreator());
      if (ServerServices.GetService(typeof (IEventLogHelper)) is EventLogHelper service4)
        service4.AfterClearTrash += new ClearTrashHandler(this.AfterClearTrashHandler);
      ((ISitesCacheService) sessionTemporaryClone.GetCustomService(typeof (ISitesCacheService))).IsPortal = true;
    }
    finally
    {
      sessionTemporaryClone?.Logout("Portal.Load");
    }
  }

  private void AfterClearTrashHandler(IUserSession session, List<string> clearLog)
  {
    if (!((PortalSettings) ServerServices.GetService(typeof (PortalSettings))).DeleteImportedPackets)
      return;
    clearLog.Add($"Процесс удаления импортированных пакетов стартован пользователем {session.UserName} с компьютера {session.ComputerName}. Время начала операции: {DateTime.UtcNow + session.TimeZoneOffset}");
    DataTable dataTable = session.GetObjectCollection(PortalConsts.objtypePacket).Select(new DBRecordSetParams(new ConditionStructure[1]
    {
      new ConditionStructure(MetaDataHelper.GetAttributeTypeID(PortalServerConsts.attributePacketStatus), RelationalOperators.Equal, (object) 1, LogicalOperators.AND, 0, false)
    }, new object[1]{ (object) -2 }));
    if (dataTable.Rows.Count == 0)
      return;
    List<long> longList = new List<long>();
    for (int index = 0; index < dataTable.Rows.Count; ++index)
      longList.Add(Convert.ToInt64(dataTable.Rows[index][0]));
    int deletedPacketCount;
    int deletedReceiptCount;
    new PacketAction().DeletePackets(session, longList.ToArray(), false, out deletedPacketCount, out deletedReceiptCount);
    clearLog.Add($"Процесс удаления импортированных пакетов завершен. Удалено {deletedPacketCount} пакета(ов) и {deletedReceiptCount} квитанций. Время завершения: {DateTime.UtcNow + session.TimeZoneOffset}");
  }

  private IUserSession GetPermanentSessionClone(IUserSession masterSession, string sessionName)
  {
    return ((UserSession) masterSession).Clone(true, sessionName);
  }

  public void Unload()
  {
  }

  public string Name => LocalizationHolder.rm.GetString("PortalServer_69");

  private class ObjectsPaket : IComparable, IComparable<PortalPlugin.ObjectsPaket>
  {
    private List<TransferedObject> _objects;
    private List<Guid> _guids;

    public void AddObject(Guid objectGuid, TransferedObject unit)
    {
      if (this._objects == null)
      {
        this._objects = new List<TransferedObject>(1);
        this._guids = new List<Guid>(1);
      }
      this._objects.Add(unit);
      this._guids.Add(objectGuid);
    }

    public void AddRelation(TransferedObject unit) => this._objects.Add(unit);

    public bool FindObject(Guid guid) => this._guids != null && this._guids.Contains(guid);

    public List<TransferedObject> TransferedObjects => this._objects;

    public int CompareTo(object obj) => this.CompareTo(obj as PortalPlugin.ObjectsPaket);

    public int CompareTo(PortalPlugin.ObjectsPaket other)
    {
      if (other == null)
        return 1;
      if (other._objects == null && this._objects == null)
        return 0;
      if (other._objects != null && this._objects == null || other._objects == null && this._objects != null || other._objects == null || this._objects == null || other._objects.Count != this._objects.Count)
        return 1;
      for (int index1 = 0; index1 < this._objects.Count; ++index1)
      {
        bool flag = false;
        for (int index2 = 0; index2 < other._objects.Count; ++index2)
        {
          if (this._objects[index1].GUID == other._objects[index2].GUID)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return 1;
      }
      return 0;
    }
  }
}
