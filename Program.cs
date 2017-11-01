using System;
using System.Collections.Generic;
using System.Threading;

namespace sky_observer
{
    class Program
    {
        static void Main(string[] args)
        {            
            ISkyObserver observer1 = new Astronomer();
            ISkyObserver observer2 = new LittleBoy();
            ISkyObserver observer3 = new Dreamer();

            ISkyObserved sky = new SkyFullOfStars();
            Console.ResetColor();
            Console.WriteLine("Press any key to register Astronomer observer...");
            Console.ReadLine();

            sky.RegisterObserver(observer1);

            Console.ResetColor();            
            Console.WriteLine("Press any key to register Little boy observer...");
            Console.ReadLine();

            sky.RegisterObserver(observer2);

            Console.ResetColor();
            Console.WriteLine("Press any key to register Dreamer observer...");
            Console.ReadLine();

            sky.RegisterObserver(observer3);

            Console.ResetColor();
            Console.WriteLine("Press any key to unregister Astronomer observer...");
            Console.ReadLine();

            sky.UnregisterObserver(observer1);

            Console.ResetColor();            
            Console.WriteLine("Press any key to unregister Little boy observer...");
            Console.ReadLine();

            sky.UnregisterObserver(observer2);

            Console.ResetColor();
            Console.WriteLine("Press any key to unregister Dreamer observer...");
            Console.ReadLine();

            sky.UnregisterObserver(observer3);

            Console.ResetColor();
            Console.WriteLine("Exiting...");

            SkyFullOfStars sfos = sky as SkyFullOfStars;

            sfos.StopFalling();

        }

        class SkyFullOfStars : ISkyObserved
        {
            Thread Worker;
            bool starsAreFalling = false;
            Random rnd = new Random();
            List<ISkyObserver> observers = new List<ISkyObserver>();

            public SkyFullOfStars()
            {
                Worker = new Thread(new ThreadStart(LetFallAStar));
                Worker.Start();
            }

            public void RegisterObserver(ISkyObserver observer)
            {
                observers.Add(observer);
                string observerName = observer.GetType().Name;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{observerName} is watching the sky.");
            }

            public void StopFalling()
            {
                starsAreFalling = false;
                Worker.Join();
            }

            public void UnregisterObserver(ISkyObserver observer)
            {
                observers.Remove(observer);
                string observerName = observer.GetType().Name;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{observerName} stop watching the sky.");
            }

            void LetFallAStar()
            {
                starsAreFalling = true;
                while(starsAreFalling)
                {
                    Console.BackgroundColor = ConsoleColor.White;

                    double lat = rnd.NextDouble() * 100;
                    double lng = rnd.NextDouble() * 100;
                    string time = DateTime.Now.ToString("HH:mm:ss");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"[{time}] Falling star ({lat}, {lng})");

                    foreach(ISkyObserver observer in observers.ToArray())
                    {
                        observer.NotifyFallingStar(lat, lng);
                    }

                    Console.WriteLine();

                    Thread.Sleep(rnd.Next(1000, 5000));
                }
            }
        }

        interface ISkyObserved
        {
            void RegisterObserver(ISkyObserver observer);
            void UnregisterObserver(ISkyObserver observer);
        }

        interface ISkyObserver
        {
            void NotifyFallingStar(double lat, double lng);
        }

        class LittleBoy : ISkyObserver
        {
            public void NotifyFallingStar(double lat, double lng)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Ooooooh. This star is so beautifull.");
            }
        }

        class Dreamer : ISkyObserver
        {
            int count = 0;
            public void NotifyFallingStar(double lat, double lng)
            {
                count++;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"That is the {count} falling star. I am making a wish.");
            }
        }

        class Astronomer : ISkyObserver
        {
            public void NotifyFallingStar(double lat, double lng)
            {
                string time = DateTime.Now.ToString("HH:mm:ss");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"[{time}] This star falled to {lat}, {lng}.");
            }
        }
    }
}
