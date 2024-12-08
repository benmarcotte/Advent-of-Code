using ClassLibrary;

List<List<char>> map = [];
using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 6)));
{
    string line;
    while ((line = sr.ReadLine()) != null) map.Add(new List<char>(line));
}

//TODO: Use non-brute forcing technique

int x = -1, y = -1;
Facing direction = Facing.Up;
var initialPosition = (x, y);
for (int i = 0; i < map.Count; i++)
{
    if ((x = map[i].IndexOf('^')) > -1)
    {
        y = i;
        initialPosition = (x, y);
        break;
    }
}

int patrolledSpaces = 0;
int possibleLoops = 0;

var obstacles = new HashSet<(int x, int y)>();
var history = new Dictionary<(int x, int y), List<Facing>>();

int bruteForcedLoops = 0;

//brute force
for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        var c = map[i][j];
        map[i][j] = '#';
        if((j, i) != initialPosition)
        {
            if (FindLoop(map, initialPosition.x, initialPosition.y, Facing.Up)) bruteForcedLoops++;
        }
        map[i][j] = c;
    }
}

while (true)
{
    if (direction == Facing.Up)
    {
        if (y == 0) { patrolledSpaces++; break; }

        var spaceInFront = map[y - 1][x];
        if (spaceInFront == '#') direction = Facing.Right;
        else
        {
            if ((x, y - 1) != initialPosition && !obstacles.Contains((x, y - 1)))
            {
                var directionList = new List<Facing>();

                if (history.TryGetValue((x, y), out directionList))
                {
                    if (!directionList.Contains(Facing.Down))
                    {
                        var c = map[y - 1][x];
                        map[y - 1][x] = '#';
                        if (FindLoop(map, x, y, Facing.Up))
                        {
                            possibleLoops++;
                            obstacles.Add((x, y - 1));
                        }
                        map[y - 1][x] = c;
                    }
                }
            }
            if (map[y][x] != 'X')
            {
                patrolledSpaces++;
                map[y][x] = 'X';
                if (!history.ContainsKey((x, y))) history.Add((x, y), []);
                history[(x, y)].Add(Facing.Up);
            }
            y--;
        }
    }
    else if (direction == Facing.Down)
    {
        if (y == map.Count - 1) { patrolledSpaces++; break; }

        var spaceInFront = map[y + 1][x];
        if (spaceInFront == '#') direction = Facing.Left;
        else
        {
            if ((x, y + 1) != initialPosition && !obstacles.Contains((x, y + 1)))
            {
                var directionList = new List<Facing>();

                if (history.TryGetValue((x, y), out directionList))
                {
                    if (!directionList.Contains(Facing.Up))
                    {
                        var c = map[y + 1][x];
                        map[y + 1][x] = '#';
                        if (FindLoop(map, x, y, Facing.Down))
                        {
                            possibleLoops++;
                            obstacles.Add((x, y + 1));
                        }
                        map[y + 1][x] = c;
                    }
                }
            }
            if (map[y][x] != 'X')
            {
                patrolledSpaces++;
                map[y][x] = 'X';
                if (!history.ContainsKey((x, y))) history.Add((x, y), []);
                history[(x, y)].Add(Facing.Down);
            }
            y++;
        }
    }
    else if (direction == Facing.Right)
    {
        if (x == map[0].Count - 1) { patrolledSpaces++; break; }

        var spaceInFront = map[y][x + 1];
        if (spaceInFront == '#') direction = Facing.Down;
        else 
        {
            if ((x + 1, y) != initialPosition && !obstacles.Contains((x + 1, y)))
            {
                var directionList = new List<Facing>();

                if (history.TryGetValue((x, y), out directionList))
                {
                    if (!directionList.Contains(Facing.Left))
                    {
                        var c = map[y][x + 1];
                        map[y][x + 1] = '#';
                        if (FindLoop(map, x, y, Facing.Right))
                        {
                            possibleLoops++;
                            obstacles.Add((x + 1, y));
                        }
                        map[y][x + 1] = c;
                    }
                }
            }
            if (map[y][x] != 'X')
            {
                patrolledSpaces++;
                map[y][x] = 'X';
                if (!history.ContainsKey((x, y))) history.Add((x, y), []);
                history[(x, y)].Add(Facing.Right);
            }
            x++;
        }
    }
    else if (direction == Facing.Left)
    {
        if (x == 0) { patrolledSpaces++; break; }

        var spaceInFront = map[y][x - 1];
        if (spaceInFront == '#') direction = Facing.Up;
        else 
        {
            if((x - 1, y) != initialPosition && !obstacles.Contains((x - 1, y)))
            {
                var directionList = new List<Facing>();

                if (history.TryGetValue((x, y), out directionList))
                {
                    if (!directionList.Contains(Facing.Right))
                    {
                        var c = map[y][x - 1];
                        map[y][x - 1] = '#';
                        if (FindLoop(map, x, y, Facing.Left))
                        {
                            possibleLoops++;
                            obstacles.Add((x - 1, y));
                        }
                        map[y][x - 1] = c;
                    }
                }
            }
            if (map[y][x] != 'X')
            {
                patrolledSpaces++;
                map[y][x] = 'X';
                if (!history.ContainsKey((x, y))) history.Add((x, y), []);
                history[(x, y)].Add(Facing.Left);
            }
            x--; 
        }
    }
}



Console.WriteLine($"number of patrolled spaces: {patrolledSpaces}");
Console.WriteLine($"number of possible loops: {possibleLoops}");
Console.WriteLine($"number of possible loops by brute force: {bruteForcedLoops}");

static bool FindLoop(List<List<char>> map, int x, int y, Facing direction)
{
    var visitedPositions = new Dictionary<(int x, int y), List<Facing>>();

    while (true)
    {
        var directionList = new List<Facing>();

        if (visitedPositions.TryGetValue((x, y), out directionList))
        {
            if (directionList.Contains(direction))
                return true;
        }

        if (direction == Facing.Up)
        {
            if (y == 0) return false;

            var spaceInFront = map[y - 1][x];
            
            if (spaceInFront == '#') direction = Facing.Right;
            else 
            { 
                if(!visitedPositions.ContainsKey((x, y))) visitedPositions.Add((x, y), []);
                visitedPositions[(x, y)].Add(Facing.Up); 
                y -= 1; 
            }
        }
        else if (direction == Facing.Down)
        {
            if (y == map.Count - 1) return false;

            var spaceInFront = map[y + 1][x];

            if (spaceInFront == '#') direction = Facing.Left;
            else
            {
                if (!visitedPositions.ContainsKey((x, y))) visitedPositions.Add((x, y), []);
                visitedPositions[(x, y)].Add(Facing.Down);
                y += 1;
            }
        }
        else if (direction == Facing.Right)
        {
            if (x == map[0].Count - 1) return false;

            var spaceInFront = map[y][x + 1];

            if (spaceInFront == '#') direction = Facing.Down;
            else
            {
                if (!visitedPositions.ContainsKey((x, y))) visitedPositions.Add((x, y), []);
                visitedPositions[(x, y)].Add(Facing.Right);
                x += 1;
            }
        }
        else if (direction == Facing.Left)
        {
            if (x == 0) return false;

            var spaceInFront = map[y][x - 1];

            if (spaceInFront == '#') direction = Facing.Up;
            else
            {
                if (!visitedPositions.ContainsKey((x, y))) visitedPositions.Add((x, y), []);
                visitedPositions[(x, y)].Add(Facing.Left);
                x -= 1;
            }
        }
    }
}

enum Facing { Left, Right, Up, Down }
