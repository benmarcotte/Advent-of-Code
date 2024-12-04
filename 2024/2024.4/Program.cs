using ClassLibrary;
using System.Runtime.CompilerServices;

List<string> grid = [];


var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 4)));
{
    var line = sr.ReadLine();
    while (line != null)
    {
        grid.Add(line);
        line = sr.ReadLine();
    }
}

int xmasCount = 0;
int xMasCount = 0;

for (int i = 0; i < grid.Count; i++)
{
    for(int j = 0; j < grid[0].Length; j++)
    {
        if (grid[i][j] == 'X')
        {
            bool upSafe = i >= 3;
            bool downSafe = i < grid.Count - 3;
            bool leftSafe = j >= 3;
            bool rightSafe = j < grid.Count - 3;
            if (upSafe)
            {
                string check = "X";
                check += grid[i - 1][j];
                check += grid[i - 2][j];
                check += grid[i - 3][j];
                if(check == "XMAS") xmasCount++;
                if (leftSafe)
                {
                    check = "X";
                    check += grid[i - 1][j - 1];
                    check += grid[i - 2][j - 2];
                    check += grid[i - 3][j - 3];
                    if (check == "XMAS") xmasCount++;
                }
                if (rightSafe)
                {
                    check = "X";
                    check += grid[i - 1][j + 1];
                    check += grid[i - 2][j + 2];
                    check += grid[i - 3][j + 3];
                    if (check == "XMAS") xmasCount++;
                }
            }
            if (downSafe)
            {
                string check = "X";
                check += grid[i + 1][j];
                check += grid[i + 2][j];
                check += grid[i + 3][j];
                if (check == "XMAS") xmasCount++;
                if (leftSafe)
                {
                    check = "X";
                    check += grid[i + 1][j - 1];
                    check += grid[i + 2][j - 2];
                    check += grid[i + 3][j - 3];
                    if (check == "XMAS") xmasCount++;
                }
                if (rightSafe)
                {
                    check = "X";
                    check += grid[i + 1][j + 1];
                    check += grid[i + 2][j + 2];
                    check += grid[i + 3][j + 3];
                    if (check == "XMAS") xmasCount++;
                }
            }
            if (leftSafe)
            {
                string check = "X";
                check += grid[i][j - 1];
                check += grid[i][j - 2];
                check += grid[i][j - 3];
                if (check == "XMAS") xmasCount++;
            }
            if (rightSafe)
            {
                string check = "X";
                check += grid[i][j + 1];
                check += grid[i][j + 2];
                check += grid[i][j + 3];
                if (check == "XMAS") xmasCount++;
            }
        }
        if (grid[i][j] == 'A')
        {
            if (i > 0 && j > 0 && i < grid.Count - 1 && j < grid[0].Length - 1)
            {
                string check = "";
                check += grid[i + 1][j + 1];
                check += grid[i - 1][j + 1];
                check += grid[i - 1][j - 1];
                check += grid[i + 1][j - 1];
                if (check.Contains("MM") || check.Contains("SS"))
                {
                    int mCount = 0;
                    int sCount = 0;
                    foreach (char c in check)
                    {
                        if (c == 'M') mCount++;
                        if (c == 'S') sCount++;
                    }
                    if (mCount == 2 && sCount == 2) xMasCount++;
                }
            }
        }
    }
}

Console.WriteLine($"XMAS occruences: {xmasCount}");
Console.WriteLine($"X-MAS occruences: {xMasCount}");