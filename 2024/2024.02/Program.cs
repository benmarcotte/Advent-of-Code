using ClassLibrary;

int safe = 0;
int dampened = 0;

using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 2)));
{ 

    string line;
    while ((line = sr.ReadLine()) != null)
    {
        var strs = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        List<int> nums = [];
        foreach (var str in strs)
        {
            nums.Add(int.Parse(str));
        }

        bool currentlySafe = CheckIfSafe(nums);

        if (currentlySafe)
            safe++;
        else
        {
            for (int i = 0; i < nums.Count; i++)
            {
                var temp = new List<int>(nums);
                temp.RemoveAt(i);
                if (CheckIfSafe(temp))
                {
                    dampened++;
                    break;
                }
            }
        }
    }
}
Console.WriteLine($"safe: {safe}");
Console.WriteLine($"safe if dampened: {dampened}");

static bool CheckIfSafe(List<int> nums)
{
    bool currentlySafe = true;
    bool asc = false;
    bool dsc = false;
    int prev = -1;

    foreach (var n in nums)
    {
        if (prev == -1)
        {
            prev = n;
            continue;
        }

        if (prev < n) asc = true;
        if (prev > n) dsc = true;
        if (asc && dsc) currentlySafe = false;
        if (Math.Abs(n - prev) < 1 || Math.Abs(n - prev) > 3) currentlySafe = false;
        if (!currentlySafe) break;
        prev = n;
    }

    return currentlySafe;
}