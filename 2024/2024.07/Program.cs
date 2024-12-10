using ClassLibrary;
using System.Numerics;
using System.Text.RegularExpressions;

using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 7)));
{
    string line;
    long calibratedSum = 0;
    long concatenatedSum = 0;
    while ((line = sr.ReadLine()) != null)
    {
        var split = line.Split(':');
        long total = long.Parse(split[0]);
        var arguments = split[1].Trim().Split(' ');
		int permutations = (int)Math.Pow(3, arguments.Length - 1);
		
		for (int i = 0; i < permutations; i++)
		{
			long check = 0;
			var ternaryRepresentation = ToTernaryString(i, arguments.Length - 1);
			List<long> concatenatedNumbers = [];
			foreach (var number in arguments) concatenatedNumbers.Add(long.Parse(number));
			for (int j = 0; j <= ternaryRepresentation.Length; j++)
			{
				if (j == 0)
					check = long.Parse(arguments[j]);
				else
				{
					if (ternaryRepresentation[j - 1] == '0')
						check += long.Parse(arguments[j]);
					else if (ternaryRepresentation[j - 1] == '1')
						check *= long.Parse(arguments[j]);
					else
						check = long.Parse(check.ToString() + arguments[j]);
				}
			}
			if (check == total)
			{
				if (!ternaryRepresentation.Contains('2'))
					calibratedSum += check;
				else 
					concatenatedSum += check;
				break;
			}
		}
    }
    Console.WriteLine($"Calibrated sum: {calibratedSum}");
    Console.WriteLine($"Concatenated sum: {concatenatedSum}");
    Console.WriteLine($"Total: {calibratedSum + concatenatedSum}");
}


static string ToTernaryString(int n, int stringLength)
{
	string returnal = "";
	while (n > 0)
	{
		int t = n % 3;
		returnal = t + returnal;
		n -= t;
		n /= 3;
	}
	if (returnal.Length < stringLength)
	{
		returnal = 0.ToString($"B{stringLength - returnal.Length}") + returnal;
	}

	return returnal;
}