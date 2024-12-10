using ClassLibrary;

List<List<char>> map = [];
HashSet<(int x, int y)> trailHeads = [];
using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 10)));
{
    string line;
    while ((line = sr.ReadLine()) != null)
    {
        //line = s;
        map.Add([]);
        for(int i = 0; i < line.Length; i++)
        {
            map.Last().Add(line[i]);
            if (line[i] == '0')
            {
                trailHeads.Add((map.Count - 1, i));
            }
        }
    }
}

int sumTrailheadScore = 0;
int sumTrailheadPaths = 0;

foreach ((int x, int y) in trailHeads)
{
    HashSet<(int, int)> peaksReached = [];
    sumTrailheadPaths += GetTrailheadScore(map, x, y, [], out peaksReached);
    sumTrailheadScore += peaksReached.Count;
}

Console.WriteLine($"sum of trailhead scores: {sumTrailheadScore}");
Console.WriteLine($"sum of trailhead paths: {sumTrailheadPaths}");

static int GetTrailheadScore(List<List<char>> map, int x, int y, HashSet<(int, int)> peaksReached, out HashSet<(int, int)> peaksReachedOutput)
{
    peaksReachedOutput = peaksReached;
    if (map[x][y] == '9')
    {
        peaksReached.Add((x, y));
        return 1;
    }
    else
    {
        return (x < map.Count - 1 ?             (map[x + 1][y] == map[x][y] + 1 ? GetTrailheadScore(map, x + 1, y, peaksReached, out peaksReachedOutput) : 0) : 0)
             + (x > 0 ?                         (map[x - 1][y] == map[x][y] + 1 ? GetTrailheadScore(map, x - 1, y, peaksReached, out peaksReachedOutput) : 0) : 0)
             + (y < map[0].Count - 1 ?          (map[x][y + 1] == map[x][y] + 1 ? GetTrailheadScore(map, x, y + 1, peaksReached, out peaksReachedOutput) : 0) : 0)
             + (y > 0 ?                         (map[x][y - 1] == map[x][y] + 1 ? GetTrailheadScore(map, x, y - 1, peaksReached, out peaksReachedOutput) : 0) : 0);
    }
}