using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Events
{
    class Program
    {
        static void Main(string[] args)
        {
            var HP = new Printer("HP LaserJet");

            bool stop = false;
            HP.PrintStarted += (sender, e) => {
                Thread.Sleep(1000);
                Console.WriteLine($"{(sender as Printer)?.Name}:: Print Started");
            };
            HP.PrintFinished += (sender, e) => {
                Thread.Sleep(1000);
                Console.WriteLine($"{(sender as Printer)?.Name}:: Print Finished");
                stop = true;
            };
            HP.PrintInProgress += (sender, e) => {
                Thread.Sleep(1000);
                Console.WriteLine($"{(sender as Printer)?.Name}:: Printing {e.PrintedPagesCount} of {e.PagesCount} [{Convert.ToUInt32(e.Progress)}%]");
            };
            HP.OutOfPapers += (sender, e) => {
                Thread.Sleep(1000);
                Console.WriteLine($"{(sender as Printer)?.Name}:: There isn't paper in printer. Add papers, please");
            };
            HP.AddedPapers+= (sender, e) => {
                Thread.Sleep(1000);
                Console.WriteLine($"{(sender as Printer)?.Name}:: Added  {e.AddedPapersCount} papers");
            };

            while (!stop)
            {
                HP.AddPapers(4);
                HP.Print(15);
            }
        }
    }
}
