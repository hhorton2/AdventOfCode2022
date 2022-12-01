using System.Threading.Tasks.Dataflow;

namespace AdventOfCode2022.Common
{
    public class DataflowConstants
    {
        public static DataflowLinkOptions DefaultLinkOptions = new DataflowLinkOptions { PropagateCompletion = true };
    }
}