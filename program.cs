using System;
using System.Diagnostics;
using System.Threading;
using log4net;
using log4net.Config;

namespace Myapp1
{
    class MyProcess
    {
        void MonitorRunningProcesses()
        {   
			
			int workers, ports = 0;
			
			int count =0;
			
			
			
			ThreadPool.GetMaxThreads(out workers, out ports);
			
			log.InfoFormat("Maximum Worker threads in Threadpool: {0}", workers);
			log.InfoFormat("Maximum async I/O threads in Threadpool: {0}", ports);
			ThreadPool.GetMinThreads(out workers, out ports);
         
			log.InfoFormat("Minimum Worker threads in Threadpool: {0:#,##0}", workers);
			log.InfoFormat("Minimum async I/O threads in Threadpool: {0:#,##0}", ports);
			
			Process[] processlist = Process.GetProcesses();
		
			
			foreach (Process theprocess in processlist) {
				count=count+1;
				
				PerformanceCounter dotnetCLR_logical = new PerformanceCounter(".NET CLR LocksAndThreads","# of current logical Threads",theprocess.ProcessName);
				PerformanceCounter dotnetCLR_physical = new PerformanceCounter(".NET CLR LocksAndThreads","# of current physical Threads",theprocess.ProcessName);
				
				log.InfoFormat("Process: {0} PID: {1}", theprocess.ProcessName, theprocess.Id);
				log.InfoFormat("Thread count: {0}", theprocess.Threads.Count.ToString());
				
				
				
				try
				{
				
				
				log.InfoFormat("Logical thread count: {0}", dotnetCLR_logical.NextValue());
				log.InfoFormat("Physical thread count: {0}", dotnetCLR_physical.NextValue());
				}
				catch(InvalidOperationException e)
				{
				Console.WriteLine(e.Message);
				log.Warn("Exception",e);
				}
				
				ProcessThreadCollection threadList = theprocess.Threads;
				foreach (ProcessThread thr in threadList)
				{
				
				log.InfoFormat("Thread ID: {0}", thr.Id);
				log.InfoFormat("Thread State Property: {0}", thr.ThreadState.ToString());
				
				}
			}
			
			Stopwatch sw = new Stopwatch();
			sw.Start();

//No need to risk overflowing an int
			while(true)
				{
					Thread.Sleep(50000);
    
    //No need to stop the stopwatch, simply "mark the lap"
					if (sw.ElapsedMilliseconds > 5000)
					break;
				}

			sw.Stop();
        }
		private static readonly ILog log = LogManager.GetLogger(typeof(MyProcess));
        static void Main()
        {
            MyProcess myProcess = new MyProcess();
			BasicConfigurator.Configure();

			log.Info("Entering application.");
			log.Info("Hello And Welcome");
			
			int n=0;
			while(n<5){
				log.Info("This is another iteration");
				myProcess.MonitorRunningProcesses();
			}
        }
    }
}
