﻿using System;
using System.Collections.Generic;
using System.Threading;
using Xunit.Runners;
class Program
{
    static object consoleLock = new object();

    // Use an event to know when we're done
    static ManualResetEvent finished = new ManualResetEvent(false);

    // Start out assuming success; we'll set this to 1 if we get a failed test
    static int result = 0;

    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("usage: TestRunner <assembly> [typeName [typeName...]]");
            return 2;
        }

        var testAssembly = args[0];
        var typeNames = new List<string>();
        for (int idx = 1; idx < args.Length; ++idx)
            typeNames.Add(args[idx]);

        using (var runner = AssemblyRunner.WithAppDomain(testAssembly))
        {
            runner.OnDiscoveryComplete = OnDiscoveryComplete;
            runner.OnExecutionComplete = OnExecutionComplete;
            runner.OnTestFailed = OnTestFailed;
            runner.OnTestSkipped = OnTestSkipped;

            Console.WriteLine("Discovering...");

            var options = new AssemblyRunnerStartOptions { TypesToRun = typeNames.ToArray() };
            runner.Start(options);

            finished.WaitOne();
            finished.Dispose();

            return result;
        }
    }

    static void OnDiscoveryComplete(DiscoveryCompleteInfo info)
    {
        lock (consoleLock)
            Console.WriteLine($"Running {info.TestCasesToRun} of {info.TestCasesDiscovered} tests...");
    }

    static void OnExecutionComplete(ExecutionCompleteInfo info)
    {
        lock (consoleLock)
            Console.WriteLine($"Finished: {info.TotalTests} tests in {Math.Round(info.ExecutionTime, 3)}s ({info.TestsFailed} failed, {info.TestsSkipped} skipped)");

        finished.Set();
    }

    static void OnTestFailed(TestFailedInfo info)
    {
        lock (consoleLock)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("[FAIL] {0}: {1}", info.TestDisplayName, info.ExceptionMessage);
            if (info.ExceptionStackTrace != null)
                Console.WriteLine(info.ExceptionStackTrace);

            Console.ResetColor();
        }

        result = 1;
    }

    static void OnTestSkipped(TestSkippedInfo info)
    {
        lock (consoleLock)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[SKIP] {0}: {1}", info.TestDisplayName, info.SkipReason);
            Console.ResetColor();
        }
    }
}