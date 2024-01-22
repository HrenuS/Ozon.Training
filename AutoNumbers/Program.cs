// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var numberOfSets = int.Parse(ReadLine()!);
var readSetCount = numberOfSets;

var regexPattern = new Regex("[A-Z]{1}[0-9]{2}[A-Z]{2}|[A-Z]{1}[0-9]{1}[A-Z]{2}", RegexOptions.Compiled);

do
{
    var set = ReadLine()!;
    var answers = MatchNumberSet(set);
    foreach (var answer in answers)
    {
        Console.Write(answer);
        Console.Write(" ");
    }

    Console.WriteLine();
    readSetCount -= 1;
} while (readSetCount > 0);

return;

IEnumerable<string> MatchNumberSet(string numberSet)
{
    var matches = regexPattern.Matches(numberSet);
    if (matches.Count > 0)
    {
        var matchLen = matches.Sum(_ => _.Length);
        if (matchLen < numberSet.Length)
        {
            yield return "-";
        }
        else
        {
            foreach (Match match in matches)
            {
                yield return match.Value;
            }
        }
    }
    else
    {
        yield return "-";
    }
}

string? ReadLine()
    => Console.ReadLine()?.Trim('\r');