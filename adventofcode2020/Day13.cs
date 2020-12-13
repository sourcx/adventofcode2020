using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Markup;

namespace adventofcode
{
    class Day13
    {
        public static void Start()
        {
            Part1();
            Part2();
        }

        // What is the ID of the earliest bus you can take to the airport multiplied by the number of minutes you'll need to wait for that bus?
        static void Part1()
        {
            var lines = File.ReadAllLines("input/13");

            var departureTime = int.Parse(lines[0]);
            int earliestBusNr = int.MaxValue;
            int earliestBusWaitTime = int.MaxValue;

            foreach (var bus in lines[1].Split(","))
            {
                if (bus == "x")
                {
                    continue;
                }

                var busNr = int.Parse(bus);
                var waitTime = busNr - (departureTime % busNr);

                if (waitTime < earliestBusWaitTime)
                {
                    earliestBusNr = busNr;
                    earliestBusWaitTime = waitTime;
                }
            }

            Console.WriteLine($"Day 13: Bus {earliestBusNr} * wait time {earliestBusWaitTime} = {earliestBusNr * earliestBusWaitTime}."); // 4938
        }

        // What is the earliest timestamp such that all of the listed bus IDs depart at offsets matching their positions in the list?
        static void Part2()
        {
            var buses = ReadBuses("input/13");

            long time = buses[0].nr;
            var inSync = false;

            while (!inSync)
            {
                var failedBus = false;

                for (int i = 0; i < buses.Count() && !failedBus; i++)
                {
                    var bus = buses[i];

                    if (!bus.InSync(time))
                    {
                        time += TimeOffset(buses, i);
                        failedBus = true;
                    }
                }

                if (!failedBus)
                {
                    inSync = true;
                }
            }

            Console.WriteLine($"Day 13: At t = {time} all buses are in sync."); // 230903629977901
        }

        class Bus
        {
            public int nr { get; set; }
            public int offset { get; set; }

            public bool InSync(long time)
            {
                return ((time + offset) % nr) == 0;
            }
        }

        // Multiplication of times of buses already synced so far.
        private static long TimeOffset(List<Bus> buses, int nrBusesInSync)
        {
            long increase = buses[0].nr;

            for (int j = 1; j < nrBusesInSync; j++)
            {
                increase *= buses[j].nr;
            }

            return increase;
        }

        private static List<Bus> ReadBuses(string file)
        {
            var busInput = File.ReadAllLines(file)[1].Split(",");
            var buses = new List<Bus>();

            for (int i = 0; i < busInput.Count(); i++)
            {
                if (busInput[i] != "x")
                {
                    buses.Add(new Bus() {
                        nr = int.Parse(busInput[i]),
                        offset = i
                    });
                }
            }

            return buses;
        }
    }
}
