using ClassLibrary;
using System.Text.RegularExpressions;

string data;
using var sr = new StreamReader(await InputManager.GetInput(new DateTime(2024, 12, 3)));
{
    data = sr.ReadToEnd();
}

var mulRegex = new Regex(@"(?:mul\()[0-9]{1,3},[0-9]{1,3}\)");

var matches = mulRegex.Matches(data);

int result = 0;

foreach (Match match in matches)
{
    var split = match.Value.Split(',');
    result += int.Parse(split[0].Split('(')[1]) * int.Parse(split[1].Split(')')[0]);
}

Console.WriteLine($"all results: {result}");

var doRegex = new Regex(@"(?>^|do\(\))([\s\S]*?)(?>$|don't\(\))", RegexOptions.None);

var permitted = doRegex.Matches(data);

int permittedResults = 0;

foreach (Match p in permitted)
{
    var multExpression = mulRegex.Matches(p.Value);
    foreach (Match m in multExpression)
    {
        var split = m.Groups.Values.First().Value.Split(',');
        permittedResults += int.Parse(split[0].Split('(')[1]) * int.Parse(split[1].Split(')')[0]);
    }
}

Console.WriteLine($"permitted results: {permittedResults}");
