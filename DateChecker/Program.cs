// See https://aka.ms/new-console-template for more information

var numberOfSets = int.Parse(ReadLine()!);
var readSetCount = numberOfSets;
const string yesAnswer = "YES";
const string noAnswer = "NO";

do
{
    var set = ReadLine()!;
    var date = ParseDate(set);
    var isCorrectDate = date.IsCorrectDate();
    var answer = isCorrectDate ? yesAnswer : noAnswer;
    Console.WriteLine(answer);
    readSetCount -= 1;
} while (readSetCount > 0);

return;

Date ParseDate(string set)
{
    const char space = ' ';
    var spanOfSet = new ReadOnlySpan<char>(set.ToCharArray());
    var spaceCount = 0;
    int day = 0, month = 0, year = 0;
    var lastSpaceIndex = 0;
    for (var index = 0; index < spanOfSet.Length; index++)
    {
        if (spanOfSet[index] == space)
        {
            // day
            if (spaceCount == 0)
            {
                var daySpan = spanOfSet[..index];
                day = int.Parse(daySpan);
                spaceCount += 1;
                lastSpaceIndex = index;
                continue;
            }

            // month
            if (spaceCount == 1)
            {
                var monthSpan = spanOfSet.Slice(lastSpaceIndex + 1, index - lastSpaceIndex - 1);
                month = int.Parse(monthSpan);
                spaceCount += 1;
                lastSpaceIndex = index;
                continue;
            }
        }

        // year
        if (spaceCount > 1 && index == spanOfSet.Length - 1)
        {
            var yearSpan = spanOfSet.Slice(lastSpaceIndex + 1, index - lastSpaceIndex);
            year = int.Parse(yearSpan);
        }
    }

    return new Date(day, month, year);
}

string? ReadLine()
    => Console.ReadLine()?.Trim('\r');


public readonly struct Date
{
    private static ReadOnlySpan<byte> DaysInMonth365 => new byte[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
    private static ReadOnlySpan<byte> DaysInMonth366 => new byte[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    public readonly int Day;
    public readonly int Month;
    public readonly int Year;

    public Date(int day, int month, int year)
    {
        Day = day;
        Month = month;
        Year = year;
    }

    public bool IsCorrectDate()
    {
        const int minDay = 1;
        const int minMonth = 1;
        const int maxMonth = 12;
        const int minYear = 1950;
        const int maxYear = 2300;

        var isLeapYear = IsLeapYear(Year);
        var maxDayOfMonth = GetMaxDayOfMonths(Month, isLeapYear);
        var dayCorrect = Day >= minDay && Day <= maxDayOfMonth;
        var monthCorrect = Month >= minMonth && Month <= maxMonth;
        var yearCorrect = Year >= minYear && Year <= maxYear;
        return dayCorrect && monthCorrect && yearCorrect;
    }

    private bool IsLeapYear(int year)
    {
        if (year % 4 == 0 && year % 100 != 0)
        {
            return true;
        }

        return year % 400 == 0;
    }

    private static int GetMaxDayOfMonths(int month, bool isLeapYear)
        => isLeapYear ? DaysInMonth366[month - 1] : DaysInMonth365[month - 1];
}