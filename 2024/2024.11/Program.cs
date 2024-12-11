using ClassLibrary;
using System.Diagnostics.CodeAnalysis;

List<string> rocks = [];
using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 11)));
{
    string line = sr.ReadLine();
    rocks = new List<string>(line.Split(' '));
}

Dictionary<(string, int), long> memory = [];

int maxBlinks = 75;

long count = 0;

foreach (var rock in rocks)
{
    count += processRock(rock, maxBlinks, ref memory);
}

long processRock(string rock, int blinks, ref Dictionary<(string, int), long> memory)
{
    if (blinks == 0)
    {
        return 1;
    }
    else
    {
        if (rock == "0")
        {
            if (memory.TryGetValue((rock, blinks), out long returnal)) return returnal;
            returnal = processRock("1", blinks - 1, ref memory);
            memory.Add((rock, blinks), returnal);
            return returnal;
        }
        else if (rock.Length % 2 == 0)
        {
            if (memory.TryGetValue((rock, blinks), out long returnal)) return returnal;
            returnal = processRock(rock[..(rock.Length / 2)], blinks - 1, ref memory) + processRock(long.Parse(rock[(rock.Length / 2)..]).ToString(), blinks - 1, ref memory);
            memory.Add((rock, blinks), returnal);
            return returnal;
        }
        else
        {
            if (memory.TryGetValue((rock, blinks), out long returnal)) return returnal;
            returnal = processRock((long.Parse(rock) * 2024).ToString(), blinks - 1, ref memory);
            memory.Add((rock, blinks), returnal);
            return returnal;
        }
    }
}

Console.WriteLine($"number of rocks after {maxBlinks} blinks: {count}");