using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using AdventOfCode2022.Common;

namespace AdventOfCode2022.Day01
{
    public static class DayOnePipeline
    {

        public static string SolvePartOne(string filename)
        {
            InputBlocks.GetFileLines.LinkTo(LinesToRations, DataflowConstants.DefaultLinkOptions);
            LinesToRations.LinkTo(SumRationCalories, DataflowConstants.DefaultLinkOptions);
            SumRationCalories.LinkTo(CalorieBuffer, DataflowConstants.DefaultLinkOptions);
            CalorieBuffer.LinkTo(HighestCalorieFinder, DataflowConstants.DefaultLinkOptions);
            InputBlocks.GetFileLines.Post(filename);
            InputBlocks.GetFileLines.Complete();
            var result = "" + HighestCalorieFinder.Receive();
            HighestCalorieFinder.Completion.Wait();
            return result;
        }

        public static string SolvePartTwo(string filename)
        {
            InputBlocks.GetFileLines.LinkTo(LinesToRations, DataflowConstants.DefaultLinkOptions);
            LinesToRations.LinkTo(SumRationCalories, DataflowConstants.DefaultLinkOptions);
            SumRationCalories.LinkTo(CalorieBuffer, DataflowConstants.DefaultLinkOptions);
            CalorieBuffer.LinkTo(TopThreeHighestCalorieFinder, DataflowConstants.DefaultLinkOptions);
            InputBlocks.GetFileLines.Post(filename);
            InputBlocks.GetFileLines.Complete();
            var result = "" + TopThreeHighestCalorieFinder.Receive();
            TopThreeHighestCalorieFinder.Completion.Wait();
            return result;
        }

        private static readonly TransformManyBlock<string[], IEnumerable<long>> LinesToRations =
            new TransformManyBlock<string[], IEnumerable<long>>(lines =>
            {
                var rations = new List<List<long>>();
                var currentRations = new List<long>();
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        rations.Add(currentRations);
                        currentRations = new List<long>();
                    }
                    else
                    {
                        currentRations.Add(long.Parse(line));
                    }
                }
                rations.Add(currentRations);

                return rations;
            });

        private static readonly TransformBlock<IEnumerable<long>, long> SumRationCalories =
            new TransformBlock<IEnumerable<long>, long>(rations => rations.Sum());

        private static readonly BatchBlock<long> CalorieBuffer = new BatchBlock<long>(int.MaxValue);
        private static readonly TransformBlock<IEnumerable<long>,long> HighestCalorieFinder = new TransformBlock<IEnumerable<long>,long>(totals =>
        {
            var highest = long.MinValue;
            foreach (var total in totals)
            {
                if (total > highest)
                {
                    highest = total;
                }
            }

            return highest;
        });
        
        private static readonly TransformBlock<IEnumerable<long>,long> TopThreeHighestCalorieFinder = new TransformBlock<IEnumerable<long>,long>(totals =>
        {
            var highest = long.MinValue;
            var second = long.MinValue;
            var third = long.MinValue;
            foreach (var currentTotal in totals)
            {
                if (currentTotal > highest)
                {
                    third = second;
                    second = highest;
                    highest = currentTotal;
                }
                else if (currentTotal > second)
                {
                    third = second;
                    second = currentTotal;
                }
                else if (currentTotal > third)
                {
                    third = currentTotal;
                }
            }

            return highest + second + third;
        });
    }
}