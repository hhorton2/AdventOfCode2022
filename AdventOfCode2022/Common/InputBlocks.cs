using System.IO;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode2022.Common
{
    public class InputBlocks
    {
        public readonly TransformBlock<string, string[]> GetFileLines =
            new TransformBlock<string, string[]>(File.ReadAllLines);
    }
}