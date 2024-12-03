using ClassLibrary;

var sr = new StreamReader(await InputManager.GetInputAsync(new DateTime(2024, 12, 2)));


int safe = 0;
int dampened = 0;


for (string line = sr.ReadLine(); line != null; line = sr.ReadLine())
{
    var strs = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    List<int> nums = [];
    foreach (var str in strs)
    {
        nums.Add(int.Parse(str));
    }

    bool currentlySafe = CheckIfSafe(nums);

    if (currentlySafe) safe++;
    else
    {
        bool safeIfDampened = false;
        for (int i = 0; i < nums.Count; i++)
        {
            var temp = new List<int>(nums);
            temp.RemoveAt(i);
            safeIfDampened = CheckIfSafe(temp);
            if (safeIfDampened)
            {
                dampened++;
                Console.WriteLine($"Safe after dampening: {line}");
                break;
            }
        }
    }
}
sr.Close();
Console.WriteLine($"safe: {safe}");
Console.WriteLine($"safe if dampened: {safe} already safe + {dampened} safe after dampened = {safe + dampened}");

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