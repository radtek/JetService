using System;
using System.IO;
using JetService.IntegrationTests.Executable;
using JetService.IntegrationTests.WinForms;

namespace JetService.IntegrationTests
{
  public static class Files
  {
    private static readonly Lazy<string> myConsoleTestProgram = new Lazy<string>(
      () =>
        {
          string file = new Uri(typeof(TestConsoleProgram).Assembly.CodeBase).LocalPath;
          if (!File.Exists(file)) throw new Exception("Faied to find TestConsoleProgram");
          return file;
        });

    private static readonly Lazy<string> myWinFormsTestProgram = new Lazy<string>(
      () =>
        {
          string file = new Uri(typeof(TestWinFormsProgram).Assembly.CodeBase).LocalPath;
          if (!File.Exists(file)) throw new Exception("Faied to find TestConsoleProgram");
          return file;
        });

    public static string ConsoleTestProgram { get { return myConsoleTestProgram.Value; } }
    public static string WinFormsTestProgram { get { return myWinFormsTestProgram.Value; } }

    private static readonly Lazy<string> myHomePath = new Lazy<string>(
      () =>
        {
          string file = new Uri(typeof (Files).Assembly.CodeBase).LocalPath;
          while (file != null)
          {
            if (File.Exists(Path.Combine(file, "README")))
            {
              Console.Out.WriteLine("Root directory: {0}", file);
              return Path.GetFullPath(file);
            }
            file = Path.GetDirectoryName(file);
          }
          throw new Exception("Failed to find base directory");
        }
      );

    private static readonly Lazy<string> myServiceExe = new Lazy<string>(
      () =>
        {
          string debug = Path.Combine(myHomePath.Value, "src/Debug/JetService.exe");
          string release = Path.Combine(myHomePath.Value, "src/Release/JetService.exe");

          if (!File.Exists(release) && !File.Exists(debug))
            throw new Exception("JetService is not built");

          string file;
#if DEBUG
          file = debug;
#else
          file = release;
#endif
          if (!File.Exists(file)) throw new Exception("Failed to find path: " + file);
          return file;
        });

    public static string JetServiceExe { get { return myServiceExe.Value; } }
  }
}