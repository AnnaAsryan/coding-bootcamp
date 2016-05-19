using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrinterManager;

namespace Events
{

    public class Printer
    {
        public event EventHandler PrintStarted;
        public event EventHandler PrintFinished;
        public event EventHandler<PrintInProgressEventsArgs> PrintInProgress;
        public event EventHandler OutOfPapers;
        public event EventHandler<AddPapersEventArgs> AddedPapers;

        public Printer(string name)
        {
            Name = name;
            CurrentPage = 0;
        }

        public string Name { get; private set; }
        public int CurrentPage { get; private set; }
        public int PapersCount { get; private set; }
        public int PagesCount { get; private set; }

        public void AddPapers(int count)
        {
            PapersCount += count;
            var e = new AddPapersEventArgs
            {
                AddedPapersCount = count
            };
            AddedPapers?.Invoke(this, e);
        }
        public void Print(int count)
        {
            PagesCount = count;
            OnPrintStarted();

            for (int i = CurrentPage; i < PagesCount; i++)
            {
                CurrentPage = i;
                if (PapersCount == 0)
                {
                    OnOutOfPapers();
                    break;
                }
                PapersCount--;
                OnPrintInProgress(i + 1, PagesCount);
            }
            if (CurrentPage == PagesCount - 1)
                OnPrintFinished();
        }

        protected virtual void OnPrintStarted()
        {
            PrintStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPrintFinished()
        {
            PrintFinished?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPrintInProgress(int printedCount, int total)
        {
            var e = new PrintInProgressEventsArgs
            {
                PrintedPagesCount = printedCount,
                PagesCount = total
            };
            PrintInProgress?.Invoke(this, e);
        }

        protected virtual void OnOutOfPapers()
        {
            OutOfPapers?.Invoke(this, EventArgs.Empty);
        }

    }
}
