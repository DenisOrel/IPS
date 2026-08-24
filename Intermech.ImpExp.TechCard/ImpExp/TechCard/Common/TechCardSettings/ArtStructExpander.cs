// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechCardSettings.ArtStructExpander
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common.TechCardSettings;

internal class ArtStructExpander : ObjStructExpander
{
  public override List<ArtInfoLight> CollectObjsFromObjsStructures(List<ArtInfoLight> headObjsInfo)
  {
    string str1 = string.Empty;
    if (SearchConnectionsManager.GetConnection().DataBaseType == "IntermechConnection.Interbase")
      str1 = "recursive";
    this.CleanArtStruTempSelection(-890879);
    this.PrepareArtVersToSelect(headObjsInfo, -890879);
    string str2 = "pSELECT_ID";
    string str3 = "select PART_AID from ARTSTRU where SELECT_ID = " + (SearchConnectionsManager.GetConnection().DataBaseType == "IntermechConnection.MsSQL" ? "@" + str2 : ":" + str2);
    string str4 = $"with {str1} TREE_PROC(PART_AID, PROJ_AID) as(select PART_AID, 0 as PROJ_AID from V_PC where PROJ_AID in ({str3})union all select source.PART_AID, tr.PROJ_AID from V_PC source, TREE_PROC tr where tr.PART_AID = source.PROJ_AID)select distinct va.VART_ID, va.ART_ID, va.ART_VER_ID from V_ARTICLES va, TREE_PROC tr where va.VART_ID = tr.PART_AID union select va.VART_ID, va.ART_ID, va.ART_VER_ID from V_ARTICLES va where va.VART_ID in ({str3})";
    using (IDbCommand command = SearchConnectionsManager.GetConnection().CreateCommand())
    {
      command.CommandText = str4;
      IDbDataParameter parameter = command.CreateParameter();
      parameter.ParameterName = str2;
      parameter.Direction = ParameterDirection.Input;
      parameter.Value = (object) -890879;
      command.Parameters.Add((object) parameter);
      List<ArtInfoLight> artInfoLightList = new List<ArtInfoLight>();
      try
      {
        using (IDataReader dataReader = command.ExecuteReader())
        {
          while (dataReader.Read())
          {
            int int32 = dataReader.GetInt32(0);
            ArtInfoLight artInfoLight = new ArtInfoLight(dataReader.GetInt32(1), dataReader.GetInt32(2), int32);
            artInfoLightList.Add(artInfoLight);
          }
          return artInfoLightList;
        }
      }
      finally
      {
        this.CleanArtStruTempSelection(-890879);
      }
    }
  }

  private void PrepareArtVersToSelect(List<ArtInfoLight> headObjsInfo, int selectId)
  {
    string str1 = "pSELECT_ID";
    string str2 = "pVART_ID";
    string str3 = $"insert into ARTSTRU (SELECT_ID, PART_AID)(select {(SearchConnectionsManager.GetConnection().DataBaseType == "IntermechConnection.MsSQL" ? "@" + str1 : ":" + str1)}, VART_ID from V_ARTICLES where ART_ID = {(SearchConnectionsManager.GetConnection().DataBaseType == "IntermechConnection.MsSQL" ? "@" + str2 : ":" + str2)})";
    using (IDbCommand cmd = SearchConnectionsManager.GetConnection().CreateCommand())
    {
      cmd.CommandText = str3;
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = str1;
      parameter.Direction = ParameterDirection.Input;
      parameter.DbType = DbType.Int32;
      parameter.Value = (object) selectId;
      cmd.Parameters.Add((object) parameter);
      IDbDataParameter pVArtId = cmd.CreateParameter();
      pVArtId.ParameterName = str2;
      pVArtId.DbType = DbType.Int32;
      pVArtId.Direction = ParameterDirection.Input;
      cmd.Parameters.Add((object) pVArtId);
      cmd.Prepare();
      headObjsInfo.ForEach((Action<ArtInfoLight>) (artInfo =>
      {
        pVArtId.Value = (object) artInfo.ArtId;
        cmd.ExecuteNonQuery();
      }));
    }
  }

  private void CleanArtStruTempSelection(int selectId)
  {
    string str1 = "pSELECT_ID";
    string empty = string.Empty;
    string str2 = "delete from ARTSTRU where SELECT_ID = " + (SearchConnectionsManager.GetConnection().DataBaseType == "IntermechConnection.MsSQL" ? "@" + str1 : ":" + str1);
    using (IDbCommand command = SearchConnectionsManager.GetConnection().CreateCommand())
    {
      command.CommandText = str2;
      IDbDataParameter parameter = command.CreateParameter();
      parameter.ParameterName = str1;
      parameter.Direction = ParameterDirection.Input;
      parameter.Value = (object) selectId;
      command.Parameters.Add((object) parameter);
      command.ExecuteNonQuery();
    }
  }
}
