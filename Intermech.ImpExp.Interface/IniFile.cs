// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IniFile
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Класс для работы с INI-файлами</summary>
public class IniFile
{
  /// <summary>Путь к INI-файлу</summary>
  public string Path;

  [DllImport("kernel32")]
  private static extern long WritePrivateProfileString(
    string section,
    string key,
    string val,
    string filePath);

  [DllImport("kernel32")]
  private static extern int GetPrivateProfileString(
    string section,
    string key,
    string def,
    StringBuilder retVal,
    int size,
    string filePath);

  [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringA")]
  private static extern int GetPrivateProfileStringBytes(
    string section,
    string key,
    string def,
    byte[] retVal,
    int size,
    string filePath);

  /// <summary>Конструктор</summary>
  /// <param name="IniPath">путь к INI-файлу</param>
  public IniFile(string IniPath) => this.Path = IniPath;

  /// <summary>Запись данных в INI-файл</summary>
  /// <param name="Section">имя секции INI-файла</param>
  /// <param name="Key">имя параметра</param>
  /// <param name="Value">записываемое значение параметра</param>
  public void IniWriteValue(string Section, string Key, string Value)
  {
    IniFile.WritePrivateProfileString(Section, Key, Value, this.Path);
  }

  /// <summary>Чтение значения параметра из INI-файла</summary>
  /// <param name="Section">имя секции INI-файла</param>
  /// <param name="Key">имя параметра</param>
  /// <returns>значение параметра</returns>
  public string IniReadValue(string Section, string Key) => this.IniReadValue(Section, Key, "");

  public string IniReadValue(string Section, string Key, string DefaultValue)
  {
    StringBuilder retVal = new StringBuilder((int) byte.MaxValue);
    IniFile.GetPrivateProfileString(Section, Key, DefaultValue, retVal, (int) byte.MaxValue, this.Path);
    return retVal.ToString();
  }

  public List<string> ReadSection(string Section)
  {
    StringBuilder stringBuilder = new StringBuilder(16384 /*0x4000*/);
    byte[] array = new byte[16384 /*0x4000*/];
    int profileStringBytes = IniFile.GetPrivateProfileStringBytes(Section, (string) null, (string) null, array, 16384 /*0x4000*/, this.Path);
    if (profileStringBytes == 0)
      return (List<string>) null;
    Array.Resize<byte>(ref array, profileStringBytes);
    return new List<string>((IEnumerable<string>) Encoding.ASCII.GetString(array).Split(new string[1]
    {
      "\0"
    }, StringSplitOptions.RemoveEmptyEntries));
  }
}
