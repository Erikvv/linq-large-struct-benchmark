using static System.Linq.Enumerable;
using Int32 = System.Int32;
using TimeSpan = System.TimeSpan;
using Random = System.Random;
using Console = System.Console;
using Enumerable = System.Linq.Enumerable;
using Stopwatch = System.Diagnostics.Stopwatch;
using GenericCollections = System.Collections.Generic;

namespace LinqLargeStructBenchmark
{
    // 4 byte struct
    readonly struct SmallStruct
    {
        public readonly int e;

        public SmallStruct(int e)
        {
            this.e = e;
        }
    }

    // 68 byte struct
    readonly struct LargeStruct
    {
        public readonly decimal a;
        public readonly decimal b;
        public readonly decimal c;
        public readonly decimal d;
        public readonly decimal e;
        public readonly decimal f;
        public readonly decimal g;
        public readonly decimal h;
        public readonly int i;

        public LargeStruct(int i)
        {
            this.a = 1;
            this.b = 2;
            this.c = 3;
            this.d = 4;
            this.e = 5;
            this.f = 6;
            this.g = 7;
            this.h = 8;

            this.i = i;
        } 
    }

    class SmallClass
    {
        public readonly int e;

        public SmallClass(int e)
        {
            this.e = e;
        }
    }

    class LargeClass
    {
        public readonly decimal a;
        public readonly decimal b;
        public readonly decimal c;
        public readonly decimal d;
        public readonly decimal e;
        public readonly decimal f;
        public readonly decimal g;
        public readonly decimal h;
        public readonly int i;

        public LargeClass(int i)
        {
            this.a = 1;
            this.b = 2;
            this.c = 3;
            this.d = 4;
            this.e = 5;
            this.f = 6;
            this.g = 7;
            this.h = 8;

            this.i = i;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Benchmarking...");
            

            var size = 10_000_000;
            var random = new Random();
            var threshold = Int32.MaxValue / 2;
            threshold = Int32.MaxValue / 2 - 1000;


            // Do all actions twice to make sure the JIT is fully optimized
            var largeStructArray = Enumerable.Range(0, size)
                .Select(_ => new LargeStruct(random.Next()));

            var (largeStructTime, filteredLargeStructArray) = MeasureTime(() => largeStructArray.Where(s => s.i < threshold));
            (largeStructTime, filteredLargeStructArray) = MeasureTime(() => largeStructArray.Where(s => s.i < threshold));
            Console.WriteLine("68 Byte Struct: Elapsed={0}, Count={1}", largeStructTime, filteredLargeStructArray.Count());
            filteredLargeStructArray = null;
            largeStructArray = null;

            var smallStructArray = Enumerable.Range(0, size)
                .Select(_ => new SmallStruct(random.Next()));

            var (smallStructTime, filteredSmallStructArray) = MeasureTime(() => smallStructArray.Where(s => s.e < threshold));
            (smallStructTime, filteredSmallStructArray) = MeasureTime(() => smallStructArray.Where(s => s.e < threshold));
            Console.WriteLine("4 Byte Struct: Elapsed={0}, Count={1}", smallStructTime, filteredSmallStructArray.Count());
            filteredSmallStructArray = null;
            smallStructArray = null;

            var smallClassArray = Enumerable.Range(0, size)
                .Select(_ => new SmallClass(random.Next()));

            var (smallClassTime, filteredSmallClassArray) = MeasureTime(() => smallClassArray.Where(s => s.e < threshold));
            (smallClassTime, filteredSmallClassArray) = MeasureTime(() => smallClassArray.Where(s => s.e < threshold));
            Console.WriteLine("4-byte Class: Elapsed={0}, Count={1}", smallClassTime, filteredSmallClassArray.Count());
            filteredSmallClassArray = null;
            smallClassArray = null;

            var classArray = Enumerable.Range(0, size)
                .Select(_ => new LargeClass(random.Next()));

            var (classTime, filteredClassArray) = MeasureTime(() => classArray.Where(s => s.i < threshold));
            (classTime, filteredClassArray) = MeasureTime(() => classArray.Where(s => s.i < threshold));
            Console.WriteLine("68-byte Class: Elapsed={0}, Count={1}", classTime, filteredClassArray.Count());
        }

        public static System.ValueTuple<TimeSpan, GenericCollections.List<T>> MeasureTime<T>(System.Func<GenericCollections.IEnumerable<T>> action)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var result = action.Invoke().ToList();
            stopwatch.Stop();
            return System.ValueTuple.Create(stopwatch.Elapsed, result);
        }
    }
}
