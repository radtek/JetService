﻿using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace JetService.IntegrationTests.Executable
{
  public enum TestAction
  {
    TEST_STDOUT, 
    TEST_STDERR,
    TEST_STDIN_READ,
    TEST_STDIN_SLEEP_READ,

    TEST_IM_ALIVE,
    TEST_RUN_10500,
    TEST_STOP_SERVICE,
    TEST_SERVICE_STDIN_READ,
    TEST_STOP_EVENT_HANDLED,
    TEST_WAIT_FILE,
    TEST_WRITE_FILE,

    UNKNOWN
  }

  public class TestProgram : TestProgramUtil
  {
    private static void WaitServiceToStart()
    {
      Thread.Sleep(TimeSpan.FromSeconds(5.5));
    }

    static int Main(string[] args)
    {
      TestAction result;
      if (!Enum.TryParse(args.FirstOrDefault(), true, out result))
        result = TestAction.UNKNOWN;

      Console.Out.WriteLine("Executing action: {0}", result);

      switch (result)
      {
        case TestAction.TEST_STDOUT:
          Console.Out.WriteLine("This is service std-out 44");
          return 0;

        case TestAction.TEST_STDERR:
          Console.Out.WriteLine("This is service std-err 42");
          return 0;

        case TestAction.TEST_SERVICE_STDIN_READ:
          WaitServiceToStart();
          goto case TestAction.TEST_STDIN_READ;

        case TestAction.TEST_STDIN_SLEEP_READ:
          WaitServiceToStart();
          goto case TestAction.TEST_STDIN_READ;

        case TestAction.TEST_STDIN_READ:
          Console.Error.WriteLine("Try to read from console");
          RunInThreadAndWait(() => Console.In.Read());
          Console.Error.WriteLine("Try to read from console:Completed");
          return 0;

        case TestAction.TEST_IM_ALIVE:
          Console.Error.WriteLine("I'm alive");
          File.WriteAllText(args[1]??"file.yxy", "I'm alive");
          WaitServiceToStart();
          Console.Error.WriteLine("I'm alive");
          return 0;

        case TestAction.TEST_WRITE_FILE:
          Console.Error.WriteLine("I'm alive");
          File.WriteAllText(args[1]??"file.yxy", "I'm alive");          
          Console.Error.WriteLine("I'm alive. Done");
          return 0;

        case TestAction.TEST_WAIT_FILE:
          while(File.Exists(args[1]??"file.xxx"))
          {
            Thread.Sleep(TimeSpan.FromSeconds(.5));
            Console.Out.WriteLine(".");
          }
          Console.Out.WriteLine("File found. Exit");
          return 0;

        case TestAction.TEST_STOP_EVENT_HANDLED:
          Console.CancelKeyPress += (sender, eventArgs) =>
                                      {
                                        Console.Out.WriteLine("CTRL+C event recieved");
                                        File.WriteAllText(args[1] ?? "file.yxy", "I'm alive");
                                        if (args[2] == "true")
                                        {
                                          eventArgs.Cancel = true;
                                          Console.Out.WriteLine("Event canceled");
                                        }
                                      };
          WaitServiceToStart();
          return 0;
        
        case TestAction.TEST_RUN_10500:          
          Console.Error.WriteLine("I'm alive");
          WaitServiceToStart();
          return 0;

        case TestAction.TEST_STOP_SERVICE:
          Console.Error.WriteLine("I'm alive");
          File.WriteAllText(args[1] ?? "file.yxy", "I'm alive");
          WaitServiceToStart();

          var ps = ProcessExecutor.ExecuteProcess("net.exe", "stop " + (args[2] ?? "no-service"));
          ps.Dump();

          Console.Out.WriteLine("net.exe exit code is: {0}", ps.ExitCode);
          Console.Out.WriteLine("Service should be stopped now");
          Console.Error.WriteLine("I'm alive");
          //Service should be stopped and process must be killed
          Thread.Sleep(TimeSpan.FromDays(1));
          return 0;

        default:
        case TestAction.UNKNOWN:        
          Console.Out.WriteLine("Unknwon");
          Console.Error.WriteLine("Unknwon");
          return 42;
      }
    }
  }
}
