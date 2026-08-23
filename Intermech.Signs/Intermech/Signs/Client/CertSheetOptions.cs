// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetOptions
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Checksums;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>
/// Класс набора опций, выбранных в контроле CertSheetControl
/// </summary>
public class CertSheetOptions
{
  /// <summary>Флаг разрешения получения УЛ</summary>
  public bool ProcessCertSheets = true;
  /// <summary>Список идентификаторов объектов</summary>
  public List<long> ObjectIDList = new List<long>();
  /// <summary>Выбранный алгоритм</summary>
  public ChecksumAlgorithm ChecksumAlgorithm;
  /// <summary>
  /// Формирование УЛ для основных документов, при NormalFilesMode=true =&gt; AuthFilesMode=false
  /// </summary>
  public bool NormalFilesMode;
  /// <summary>
  /// Формирование УЛ для аутентичных документов, при AuthFilesMode=true =&gt; NormalFilesMode=false
  /// </summary>
  public bool AuthFilesMode;
  /// <summary>Разворачивать извещения</summary>
  public bool ExpandECO;
  /// <summary>Разворачивать состав</summary>
  public bool ExpandComposition;
  /// <summary>Параметр сохранения в выделенную папку</summary>
  public bool SaveToStandaloneFolder;
  public List<object[]> Graphs = new List<object[]>();
  public List<object[]> EmptyGraphs = new List<object[]>();
  /// <summary>Список расширений файлов</summary>
  public List<string> Extensions = new List<string>();

  public CertSheetOptions() => this.Clear();

  public void Clear()
  {
    this.ProcessCertSheets = true;
    this.ObjectIDList.Clear();
    this.ChecksumAlgorithm = ChecksumAlgorithm.Crc32;
    this.NormalFilesMode = false;
    this.AuthFilesMode = false;
    this.ExpandECO = false;
    this.ExpandComposition = false;
    this.SaveToStandaloneFolder = false;
    this.Graphs.Clear();
    this.EmptyGraphs.Clear();
    this.Extensions.Clear();
  }

  /// <summary>
  /// Определить, определена ли графа c подписью для вывода в УЛ
  /// </summary>
  /// <param name="graphID"></param>
  /// <returns></returns>
  public bool GraphEnabled(string graphID) => this.CustomGraphEnabled(graphID, this.Graphs);

  /// <summary>
  /// Определить, определена ли графа без подписи для вывода в УЛ
  /// </summary>
  /// <param name="graphID"></param>
  /// <returns></returns>
  public bool EmptyGraphEnabled(string graphID)
  {
    return this.CustomGraphEnabled(graphID, this.EmptyGraphs);
  }

  private bool CustomGraphEnabled(string graphID, List<object[]> lGraphs)
  {
    bool flag = false;
    for (int index = 0; index < lGraphs.Count; ++index)
    {
      if (lGraphs[index][0].Equals((object) graphID))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }
}
