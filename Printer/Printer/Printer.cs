using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Printer
{
    public class Printer
    {
        public const uint MAX_PAGE_COUNT = 100;

        public enum PrintState
        {
            init,
            printing,
            paused,
            finished
        };

        PrintState prState = PrintState.init;
        uint pageCount = 0;
        uint currPage = 0;
        Timer timer = new Timer(1000);

        public event Action<uint, uint, string> Processing;
        public event Action<bool, string> Finished;

        public Printer(string file, uint count = MAX_PAGE_COUNT)
        {
            if (file == null)
                throw new Exception("File not found");

            if (count > MAX_PAGE_COUNT)
                throw new PrinterException($"The count is great than  {nameof(MAX_PAGE_COUNT)} = {MAX_PAGE_COUNT}");

            pageCount = count;
            timer.Elapsed += this.OnTimerTick;
        }
        /// <summary>
        /// Starts printing process.Throws PrinterException if something goes wrong.        
        /// </summary>
        public void Start()
        {
            if (prState != PrintState.init)
                DoAbort(false, "Command START called in not correct state");

            prState = PrintState.printing;
            timer.Start();
        }
        /// <summary>
        /// Pauses printing process.Throws PrinterException if something goes wrong.         
        /// </summary>
        public void Pause()
        {
            if (prState != PrintState.printing)
                DoAbort(false, "Command PAUSE called in not correct state");

            prState = PrintState.paused;
            timer.Stop();
            Processing(currPage, (currPage * 100) / pageCount, prState.ToString());
        }
        /// <summary>
        ///Resumes paused state of printing.Throws PrinterException if something goes wrong.      
        /// </summary>
        public void Resume()
        {
            if (prState != PrintState.paused)
                DoAbort(false, "Command RESUME called in not correct state");

            prState = PrintState.printing;
            timer.Start();
            Processing(currPage, (currPage * 100) / pageCount, prState.ToString());
        }
        /// <summary>
        ///Aborts printing process 
        /// </summary>
        public void Abort()
        {
            DoAbort(true, "Aborted by user");
        }
        /// <summary>
        /// 1.Stop timer
        /// 2.change state to finished
        /// 3.Invoke Finished event
        /// 4.In wrong case throws PrinterException
        /// /// </summary>
        /// <param name="normal"></param>
        /// <param name="reason"></param>
        private void DoAbort(bool normal, string reason)
        {
            timer.Stop();
            prState = PrintState.finished;
            Finished(normal, reason);

            if (!normal)
                throw new PrinterException(reason);
        }
        /// <summary>
        /// Timer's Tick event handler, simulates HW like events
        /// Emulates finish printing of each page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerTick(object sender, ElapsedEventArgs e)
        { 
            switch (prState)
            {
                case PrintState.printing:
                    if (++currPage < pageCount)
                    {
                        Processing(currPage, (currPage * 100) / pageCount, prState.ToString());
                    }
                    else
                    {
                        prState = PrintState.finished;
                        Processing(pageCount, 100, prState.ToString());
                        Finished(true, "normal");
                        timer.Stop();
                    }
                    break;
                case PrintState.paused:
                    // do nothing
                    break;
                default:
                    timer.Stop();
                    break;
            }

        }
    }
}
