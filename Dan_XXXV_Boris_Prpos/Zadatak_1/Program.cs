using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        static List<Thread> threadList = new List<Thread>();
        static int numberToGuess = 0;
        static readonly object countLock = new object();

        static void Main(string[] args)
        {
            Thread tFirst = new Thread(() => Start());
            tFirst.Start();
            tFirst.Join();
            
            Console.ReadLine();
        }
        static void Start()
        {
            Console.WriteLine("Enter number to guess:");
            numberToGuess = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter how many users will guess:");
            int numberOfUsers = Convert.ToInt32(Console.ReadLine());
            Thread Thread_Generator = new Thread(() => CreateThreads(numberOfUsers));
            Thread_Generator.Start();
            Console.WriteLine("There will be {0} users, and number to guess is also set.\n",numberOfUsers);
            Thread_Generator.Join();
            for (int i = 0; i < threadList.Count; i++)
            {
                Thread.Sleep(100);
                threadList[i].Start();
            }

        }
        static void CreateThreads (int num)
        {

            for (int i = 0; i < num; i++)
            {
                Thread t = new Thread(new ThreadStart(()=>GuessNumber(Thread.CurrentThread)))
                {
                    Name = String.Format("Participant_" + i)
                };
                threadList.Add(t);
            }
        }
        static void GuessNumber(Thread t)
        {
            bool guessed = true;
            Random rnd = new Random();
            while (guessed==true)
            {
                Thread.Sleep(100);
                Monitor.Enter(countLock);
                
                int num = rnd.Next(1, 101);
                Console.WriteLine("{0} tried to guess with number:{1}", t.Name, num);
                if ((numberToGuess % 2 == 0 && num % 2 == 0) || (numberToGuess % 2 == 1 && num % 2 == 1))
                {
                    Console.WriteLine("\tParity is guessed!\n");
                }
                if (num == numberToGuess)
                {
                    Console.WriteLine("{0} has won, and target number was:{1}", t.Name, numberToGuess);
                    guessed = false;
                }
                if (num != numberToGuess)
                {
                    Monitor.Exit(countLock);
                }
            }
        }

    }
}
