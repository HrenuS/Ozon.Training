// See https://aka.ms/new-console-template for more information

var numberOfSets = int.Parse(ReadLine()!);
var readSetCount = numberOfSets;

do
{
    var set = ReadLine()!;
    var answer = GetAnswerByShipSet(set);
    Console.WriteLine(answer);
    readSetCount -= 1;
} while (readSetCount > 0);

return;

string? ReadLine()
    => Console.ReadLine()?.Trim('\r');

Dictionary<char, ShipCount> GetVariousShipCounts()
{
    const int variousShipCount = 4;
    return new Dictionary<char, ShipCount>(variousShipCount)
    {
        { '1', new ShipCount(4) },
        { '2', new ShipCount(3) },
        { '3', new ShipCount(2) },
        { '4', new ShipCount(1) }
    };
}

string GetAnswerByShipSet(string set)
{
    const string yes = "YES";
    const string no = "NO";

    var answer = string.Empty;
    var variousShips = GetVariousShipCounts();
    foreach (var ship in from symbol in set where symbol != ' ' select variousShips[symbol])
    {
        ship.Increment();
        if (ship.CheckShipLimit())
        {
            answer = yes;
            continue;
        }

        answer = no;
        break;
    }

    return answer;
}

internal class ShipCount
{
    private readonly int _maxCount;

    private int _count;

    public ShipCount(int maxCount)
        => _maxCount = maxCount;

    public void Increment()
    {
        _count += 1;
    }

    public bool CheckShipLimit()
        => _count <= _maxCount;
}