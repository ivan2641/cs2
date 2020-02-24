using System;
using System.Diagnostics;
using System.Threading;

namespace Lab1
{
    class Program
    {
        private static int N;
        private static int M;

        static void Main(string[] args)
        {
            int[] cntN = { 10, 100, 1000, 100000 };
            int[] cntM = { 2, 3, 4, 5, 10 };
            task1for3(cntN, cntM, 0);
            Console.WriteLine("\n");
            task1for3(cntN, cntM, 1);
        }

        private static void Parallel(double[] array, int typeComplication)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Thread[] threads = new Thread[M];
            var range = (int)Math.Ceiling((double)N /M);
            for (int i = 0; i < M; i++)
            {
                int start = i * range;
                int end = ((i + 1) * range) - 1;
                if (start >= N || end >= N)
                {
                    break;
                }
                if(typeComplication==0)
                    threads[i] = new Thread(() => Run(array,start, end));
                else if(typeComplication==1)
                    threads[i] = new Thread(() => RunHard(array, start, end));
                threads[i].Start();
                threads[i].Join();
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}.{1:0000}",
            ts.Seconds,
            ts.Milliseconds);
            Console.WriteLine(N + "\t\t\t" + M + "\t\t" + elapsedTime);

        }

        private static void Run(double[] array,int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                    array[i] = Math.Pow(array[i], 2.2);
            }
        }

        private static void RunUneven(double[] array, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < array[i]; j++)
                {
                    array[i] *= j;
                }
            }
        }

        private static void RunHard(double[] array, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    array[i] = Math.Pow(array[i], 2.2);
                }
            }
        }

        private static void NoParralel(double [] array, int typeComplication)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            if(typeComplication==0)
                Run(array, 0, array.Length);
            else if (typeComplication == 1)
                RunHard(array, 0, array.Length);
            else if (typeComplication == 2)
                RunUneven(array, 0, array.Length);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}.{1:0000}",
            ts.Seconds,
            ts.Milliseconds);
            Console.WriteLine( N + "\t\t\t" + "1" + "\t\t"+ elapsedTime);
        }

        static double[] createArray(int N)
        {
            Random rnd = new Random();
            double[] array = new double[N];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rnd.Next(0, 1000);
            }
            return array;
        }

        static void task1for3(int[] cntN, int[] cntM, int typeComplication)
        {
            String levelSingleThr= "Пока не присвоена сложность"; ;
            if (typeComplication==0)
                levelSingleThr = "Однопоточная обработка";
            else if (typeComplication == 1)
                levelSingleThr = "Однопоточная обработка, усложненный вариант";
            else if (typeComplication == 2)
                levelSingleThr = "Однопоточная обработка, неравномерная вычислительная сложность";
            Console.WriteLine(levelSingleThr);
            Console.WriteLine("Число элементов массива" + "\t" + "Число потоков" + "\t" + "Время");
            double[] arr;
            for (int i = 0; i < cntN.Length; i++)
            {
                N = cntN[i];
                arr = createArray(N);
                NoParralel(arr, typeComplication);
            }

            String levelMultyThr="Пока не присвоена сложность";
            if (typeComplication==0)
                levelMultyThr = "Многопоточная обработка";
            else if (typeComplication == 1)
                levelMultyThr = "Многопоточная обработка, усложненный вариант";
            else if(typeComplication == 2)
                levelMultyThr = "Многопоточная обработка, неравномерная вычислительная сложность";
            Console.WriteLine("\n");
            Console.WriteLine(levelMultyThr);
            Console.WriteLine("Число элементов массива" + "\t" + "Число потоков" + "\t" + "Время");
            for (int i = 0; i < cntN.Length; i++)
            {
                N = cntN[i];
                arr = createArray(N);
                for (int j = 0; j < cntM.Length; j++)
                {
                    M = cntM[j];
                    Parallel(arr, typeComplication);
                }
            }
        }
    }
}
