using ClassLibrary;
using System.Text.RegularExpressions;

int returnal = 0;

using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2023, 12, 1)));
{
    var regex = new Regex(@"(?=([0-9]|one|two|three|four|five|six|seven|eight|nine))");

    string line;
    while ((line = sr.ReadLine()) != null)
    {
        var matches = regex.Matches(line);

        try
        {
            returnal += int.Parse(matches.First().Groups.Values.Last().Value) * 10;
        }
        catch
        {
            returnal += GetNumberFromString(matches.First().Groups.Values.Last().Value) * 10;
        }

        try
        {
            returnal += int.Parse(matches.Last().Groups.Values.Last().Value);
        }
        catch
        {
            returnal += GetNumberFromString(matches.Last().Groups.Values.Last().Value);
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

Console.WriteLine(returnal);