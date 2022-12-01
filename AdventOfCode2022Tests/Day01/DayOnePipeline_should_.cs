using AdventOfCode2022.Day01;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2022Tests.Day01;

public class DayOnePipeline_should_
{
    private readonly ITestOutputHelper _output;

    public DayOnePipeline_should_(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void SolvePartOneExample()
    {
        var result = DayOnePipeline.SolvePartOne("./Day01/example.txt");

        result.Should().Be("24000");
    }

    [Fact]
    public void SolvePartOne()
    {
        var result = DayOnePipeline.SolvePartOne("./Day01/input.txt");

        _output.WriteLine(result);
    }

    [Fact]
    public void SolvePartTwoExample()
    {
        var result = DayOnePipeline.SolvePartTwo("./Day01/example.txt");

        result.Should().Be("45000");
    }

    [Fact]
    public void SolvePartTwo()
    {
        var result = DayOnePipeline.SolvePartTwo("./Day01/input.txt");

        _output.WriteLine(result);
    }
}