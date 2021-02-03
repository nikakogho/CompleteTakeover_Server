using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CompleteTakeover.Service;

namespace CompleteTakeoverServer
{
    class Program
    {
        static bool isRunning = false;

        static void Main()
        {
            Console.Title = "Complete Takeover Server";
            int maxPlayers = 50;
            int port = 26950;

            isRunning = true;

            var mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Server.Start(maxPlayers, port);

            //ShowBuildingCount();
            FixMines();
        }

        static void FixMines()
        {
            using (var provider = new ServiceProvider())
            {
                foreach (var mine in provider.MineService.Find(m => m.Stored < 0)) mine.Stored = 0;
                provider.SaveChanges();
            }
        }

        static void ShowBuildingCount()
        {
            using (var provider = new ServiceProvider())
            {
                var buildings = provider.GetAllBuildings();
                Console.WriteLine($"There are {buildings.Count} buildings!");
            }
        }

        static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at {Constants.TICKS_PER_SEC} ticks per second");
            DateTime nextLoop = DateTime.Now;

            while (isRunning)
            {
                while(nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    nextLoop = nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (nextLoop > DateTime.Now) Thread.Sleep(nextLoop - DateTime.Now);
                }
            }
        }
    }
}
