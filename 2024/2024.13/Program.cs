using ClassLibrary;


List<((decimal x, decimal y) a, (decimal x, decimal y) b, (decimal x, decimal y) prize)> games = [];
using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 13)));
{
    while (!sr.EndOfStream)
    {
        string buttonA = sr.ReadLine();
        string buttonB = sr.ReadLine();
        string prize = sr.ReadLine();
        games.Add(((int.Parse(buttonA.Split(',')[0][12..]), int.Parse(buttonA.Split(',')[1][3..])),
            (int.Parse(buttonB.Split(',')[0][12..]), int.Parse(buttonB.Split(',')[1][3..])),
            (int.Parse(prize.Split(',')[0][9..]) + 10000000000000, int.Parse(prize.Split(',')[1][3..]) + 10000000000000)));
        sr.ReadLine();
    }
}

long tokenCount = 0;
// game.a.x * aTokens + game.b.x * bTokens == game.prize.x
// aTokens = (game.prize.x - bTokens * game.b.x) / game.a.x

// bTokens * game.b.y = game.prize.y - game.a.y * (game.prize.x - bTokens * game.b.x) / game.a.x 
// game.a.y * aTokens + game.b.y * bTokens == game.prize.y

foreach (var game in games)
{
    decimal bTokens;
    decimal aTokens;
    decimal tolerance = 0.00001m;
    if (game.prize.x < game.prize.y)
    {
        bTokens = (game.prize.y - (game.prize.x * game.a.y / game.a.x)) / (game.b.y - (game.b.x * game.a.y / game.a.x));
        aTokens = (game.prize.x - (game.b.x * bTokens)) / game.a.x;
    }
    else
    {
        bTokens = (game.prize.x - (game.prize.y * game.a.x / game.a.y)) / (game.b.x - (game.b.y * game.a.x / game.a.y));
        aTokens = (game.prize.y - (game.b.y * bTokens)) / game.a.y;
    }
    if (Math.Abs(Math.Round(aTokens) - aTokens) < tolerance && Math.Abs(Math.Round(bTokens) - bTokens) < tolerance)
    {
        Console.WriteLine($"{aTokens} * {game.a.x} + {bTokens} * {game.b.x} == {(aTokens * game.a.x) + (bTokens * game.b.x)}");
        Console.WriteLine($"{aTokens} * {game.a.y} + {bTokens} * {game.b.y} == {(aTokens * game.a.y) + (bTokens * game.b.y)}\n");
        tokenCount += (long)((3 * Math.Round(aTokens)) + Math.Round(bTokens));
    }
}

Console.WriteLine($"Sum of tokens: {tokenCount}");