using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Printer
{
    
    class Program
    {
        // Handler of Processing event
        static void OnProgress(uint count, uint percent, string status)
        {
            Console.SetCursorPosition(0, 1);
            Console.WriteLine($"{status}, page: {count}, percent {percent}%                           ");
        }
        // Handler of Finished event
        static void OnFinished(bool success, string reason)
        {
            Console.WriteLine("Finished {0}, reason: {1}", success ? "normally" : "with error", reason);
            System.Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter the count of pages");
                uint count = UInt32.Parse(Console.ReadLine());
                Console.Clear();

                Console.WriteLine("Possible commands: start (s), pause (p), resume (r), abort (x)");

                Printer pr = new Printer("kuku.txt", count);
                pr.Processing += OnProgress;
                pr.Finished += OnFinished;

                bool exit = false;
                while (!exit)
                {
                    switch (Console.ReadLine())
                    {
                        case "start": case "s": pr.Start(); break;
                        case "pause": case "p": pr.Pause(); break;
                        case "resume": case "r": pr.Resume(); break;
                        case "abort": case "x": pr.Abort(); exit = true; break;
                    }
                }
            }
            catch (PrinterException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
