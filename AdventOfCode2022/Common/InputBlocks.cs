using System.IO;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode2022.Common
{
    public static class InputBlocks
    {
        public static TransformBlock<string, string[]> GetFileLines =
            new TransformBlock<string, string[]>(File.ReadAllLines);
    }
}