// Decompiled with JetBrains decompiler
// Type: IPSAddIn.Installer.OutputQuestion`1
// Assembly: IPSAddIn.Installer, Version=8.0.3.1634, Culture=neutral, PublicKeyToken=null
// MVID: 0B42B756-5F54-4959-820D-851B2C3E0C84
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn.Installer.exe

#nullable disable
namespace IPSAddIn.Installer;

internal class OutputQuestion<T>
{
  public static T AskUser(string question, OutputQuestion<T>.AnswerHandler answerHandler)
  {
    T result;
    while (true)
    {
      Output.Write(question + ": ");
      if (!answerHandler(Output.ReadLine(), out result))
        Output.WriteError("Неверный выбор!");
      else
        break;
    }
    return result;
  }

  public delegate bool AnswerHandler(string answer, out T result);
}
