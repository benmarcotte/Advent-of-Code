using ClassLibrary;
using System.Runtime.CompilerServices;

List<List<(int red, int blue, int green)>> games = [];
using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2023, 12, 2)));
{
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        games.Add([]);
        var rounds = line.Split(':')[1].Split(';');
        foreach (var round in rounds)
        {
            var pulls = round.Split(",");
            int red = 0;
            int blue = 0;
            int green = 0;
            foreach (var pull in pulls)
            {
                var trimmedPull = pull.Trim();
                if (trimmedPull.Contains("red"))
                    red = int.Parse(pull.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
                if (trimmedPull.Contains("blue"))
                    blue = int.Parse(pull.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
                if (trimmedPull.Contains("green"))
                    green = int.Parse(pull.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]);
            }
            games[^1].Add((red, blue, green));
        }
    }
}

int possibleGameIdSum = 0;
int powerOfCubes = 0;
const int maxReds = 12;
const int maxGreens = 13;
const int maxBlues = 14;

for (int i = 0; i < games.Count; i++)
{
    int minReds = 0;
    int minBlues = 0;
    int minGreens = 0;
    var gameIsPossible = true;
    foreach (var (red, blue, green) in games[i])
    {
        if (red > maxReds || blue > maxBlues || green > maxGreens)
        {
            gameIsPossible = false;
        }
        if (red > minReds)
            minReds = red;
        if (blue > minBlues)
            minBlues = blue;
        if (green > minGreens)
            minGreens = green;
    }
    if (gameIsPossible)
        possibleGameIdSum += i + 1;
    powerOfCubes += minReds * minGreens * minBlues;
}

Console.WriteLine($"Sum of IDs of possible games: {possibleGameIdSum}");
Console.WriteLine($"Sum powers of games: {powerOfCubes}");

enum Colors { Red, Blue, Green };


