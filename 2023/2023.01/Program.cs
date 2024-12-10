using ClassLibrary;
using System.Text.RegularExpressions;

int returnal = 0;
int letterReturnal = 0;

using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2023, 12, 1)));
{
    var regex = new Regex(@"[0-9]");
    var letterRegex = new Regex(@"(?=([0-9]|one|two|three|four|five|six|seven|eight|nine))");

    string line;
    while ((line = sr.ReadLine()) != null)
    {
        var matches = regex.Matches(line);

        returnal += int.Parse(matches.First().Value) * 10;
        returnal += int.Parse(matches.Last().Value);

        matches = letterRegex.Matches(line);

        try
        {
            letterReturnal += int.Parse(matches.First().Groups.Values.Last().Value) * 10;
        }
        catch
        {
            letterReturnal += GetNumberFromString(matches.First().Groups.Values.Last().Value) * 10;
        }

        try
        {
            letterReturnal += int.Parse(matches.Last().Groups.Values.Last().Value);
        }
        catch
        {
            letterReturnal += GetNumberFromString(matches.Last().Groups.Values.Last().Value);
        }
    }
}

static int GetNumberFromString(string value)
{
    return value switch
    {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        _ => 0,
    };
}

Console.WriteLine($"non-letter sum: {returnal}");
Console.WriteLine($"letter sum: {letterReturnal}");