// See https://aka.ms/new-console-template for more information

var numberOfSets = int.Parse(ReadLine()!);

for (var setIndex = 0; setIndex < numberOfSets; setIndex++)
{
    var conditioningTemperature = new ConditioningTemperature();
    var workerCount = int.Parse(ReadLine()!);
    for (var workerIndex = 0; workerIndex < workerCount; workerIndex++)
    {
        var temperatureParameters = ReadLine()!;
        var temperature = ParseTemperature(temperatureParameters, out var conditionType);
        var matchTemperature = conditioningTemperature.MatchTemperature(temperature, conditionType);
        Console.WriteLine(matchTemperature);
    }

    Console.WriteLine();
}

return;

int ParseTemperature(string temperatureParameters, out ConditioningTemperature.ConditionType operationType)
{
    const char gt = '>';
    var span = new ReadOnlySpan<char>(temperatureParameters.ToCharArray());
    operationType = span[0] == gt ? ConditioningTemperature.ConditionType.Gt : ConditioningTemperature.ConditionType.Lt;
    var temperature = span[2..];
    return int.Parse(temperature);
}

string? ReadLine()
    => Console.ReadLine()?.Trim('\r');

internal class ConditioningTemperature
{
    private int _maximum = 30;
    private int _minimum = 15;

    public int MatchTemperature(int temperature, ConditionType conditionType)
    {
        if (conditionType == ConditionType.Gt)
        {
            if (_minimum <= temperature)
            {
                _minimum = temperature;
            }

            return _minimum <= _maximum ? _minimum : -1;
        }

        if (temperature <= _maximum)
        {
            _maximum = temperature;
        }

        return _maximum >= _minimum ? _minimum : -1;
    }

    internal enum ConditionType
    {
        Gt,
        Lt
    }
}