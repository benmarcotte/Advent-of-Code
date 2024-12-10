using ClassLibrary;

List<List<char>> map = [];

using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 8)));
{
    string line;
    while ((line = sr.ReadLine()) != null) map.Add(new List<char>(line));
}

Dictionary<char, HashSet<(int i, int j)>> antennae = [];
HashSet<(int i, int j)> antinodes = [];
HashSet<(int i, int j)> resonantAntinodes = [];

FindAntennae(map, antennae);
FindAntinodes(map, antennae, antinodes, resonantAntinodes);

Console.WriteLine($"number of antinodes: {antinodes.Count}");
Console.WriteLine($"number of resonant antinodes: {resonantAntinodes.Count}");

static void FindAntennae(List<List<char>> map, Dictionary<char, HashSet<(int i, int j)>> antennae)
{
    for (int i = 0; i < map.Count; i++)
    {
        for (int j = 0; j < map[i].Count; j++)
        {
            if (map[i][j] != '.')
            {
                if (!antennae.ContainsKey(map[i][j])) antennae[map[i][j]] = [];
                antennae[map[i][j]].Add((i, j));
            }
        }
    }
}

static void FindAntinodes(List<List<char>> map, Dictionary<char, HashSet<(int i, int j)>> antennae, HashSet<(int i, int j)> antinodes, HashSet<(int i, int j)> resonantAntinodes)
{
    foreach (var c in antennae.Keys)
    {
        foreach (var (i1, j1) in antennae[c])
        {
            foreach (var (i2, j2) in antennae[c])
            {
                if ((i1, j1) != (i2, j2))
                {
                    int iStep = i2 - i1;
                    int jStep = j2 - j1;
                    var newAntinode = (0, 0);
                    var inBounds = true;
                    for (int k = 1; inBounds; k++)
                    {
                        newAntinode = (i2 + (k * iStep), j2 + (k * jStep));
                        if (newAntinode.Item1 >= 0 && newAntinode.Item1 < map.Count && newAntinode.Item2 >= 0 && newAntinode.Item2 < map[0].Count)
                        {
                            if (k == 1) antinodes.Add(newAntinode);
                            resonantAntinodes.Add(newAntinode);
                        }
                        else inBounds = false;
                    }
                }
                else if (antennae[c].Count > 1) resonantAntinodes.Add((i1, j1));
            }
        }
    }
}

