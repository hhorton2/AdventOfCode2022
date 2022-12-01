using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using AdventOfCode2022.Common;

namespace AdventOfCode2022.Day01
{
    public class DayOnePipeline
    {
        public string SolvePartOne(string filename)
        {
            var getAllLineBlock = new InputBlocks().GetFileLines;
            getAllLineBlock.LinkTo(_linesToRations, DataflowConstants.DefaultLinkOptions);
            _linesToRations.LinkTo(_sumRationCalories, DataflowConstants.DefaultLinkOptions);
            _sumRationCalories.LinkTo(_calorieBuffer, DataflowConstants.DefaultLinkOptions);
            _calorieBuffer.LinkTo(_highestCalorieFinder, DataflowConstants.DefaultLinkOptions);
            getAllLineBlock.Post(filename);
            getAllLineBlock.Complete();
            var result = "" + _highestCalorieFinder.Receive();
            _highestCalorieFinder.Completion.Wait();
            return result;
        }

        public string SolvePartTwo(string filename)
        {
            var getAllLineBlock = new InputBlocks().GetFileLines;
            getAllLineBlock.LinkTo(_linesToRations, DataflowConstants.DefaultLinkOptions);
            _linesToRations.LinkTo(_sumRationCalories, DataflowConstants.DefaultLinkOptions);
            _sumRationCalories.LinkTo(_calorieBuffer, DataflowConstants.DefaultLinkOptions);
            _calorieBuffer.LinkTo(_topThreeHighestCalorieFinder, DataflowConstants.DefaultLinkOptions);
            getAllLineBlock.Post(filename);
            getAllLineBlock.Complete();
            var result = "" + _topThreeHighestCalorieFinder.Receive();
            _topThreeHighestCalorieFinder.Completion.Wait();
            return result;
        }

        private readonly TransformManyBlock<string[], IEnumerable<long>> _linesToRations =
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

        private readonly TransformBlock<IEnumerable<long>, long> _sumRationCalories =
            new TransformBlock<IEnumerable<long>, long>(rations => rations.Sum());

        private readonly BatchBlock<long> _calorieBuffer = new BatchBlock<long>(int.MaxValue);

        private readonly TransformBlock<IEnumerable<long>, long> _highestCalorieFinder =
            new TransformBlock<IEnumerable<long>, long>(totals =>
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

        private readonly TransformBlock<IEnumerable<long>, long> _topThreeHighestCalorieFinder =
            new TransformBlock<IEnumerable<long>, long>(totals =>
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