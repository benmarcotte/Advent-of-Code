using ClassLibrary;

using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 7)));
{
    string line;
    long calibratedSum = 0;
    while ((line = sr.ReadLine()) != null)
    {
        var split = line.Split(':');
        long total = long.Parse(split[0]);
        var arguments = split[1].Trim().Split(' ');
        int permutations = (int)Math.Pow(2, arguments.Length - 1);
        for (int i = 0; i < permutations; i++)
        {
            long check = 0;
            string binaryRepresentation = i.ToString($"B{arguments.Length - 1}");
            for (int j = 0; j <= binaryRepresentation.Length; j++)
            {
                if (j == 0)
                    check = int.Parse(arguments[j]);
                else
                {
                    if (binaryRepresentation[j-1] == '0')
                        check += int.Parse(arguments[j]);
                    else
                        check *= int.Parse(arguments[j]);
                }
            }
            if (check == total)
            {
                calibratedSum += check;
                break;
            }
        }
    }
    Console.WriteLine($"Calibrated sum: {calibratedSum}");
}