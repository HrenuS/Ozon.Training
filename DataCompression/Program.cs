// See https://aka.ms/new-console-template for more information

var compressor = new Compressor();
var numberOfSets = int.Parse(ReadLine()!);

for (var setIndex = 0; setIndex < numberOfSets; setIndex++)
{
    var sequenceLength = int.Parse(ReadLine()!);
    var sequenceSet = ReadLine()!;
    var sequence = ParseSequence(sequenceSet, sequenceLength);
    var compressedSequence = compressor.Compress(sequence);
    Console.WriteLine(compressedSequence.Count);
    Console.WriteLine(string.Join(" ", compressedSequence));
}

IReadOnlyList<int> ParseSequence(string sequenceSet, int sequenceLength)
{
    var list = new List<int>(sequenceLength);
    const int oneDigit = 1;
    const char space = ' ';
    var span = new ReadOnlySpan<char>(sequenceSet.ToCharArray());

    if (sequenceLength == oneDigit)
    {
        list.Add(int.Parse(span));
        return list;
    }

    var lastSpaceIndex = 0;
    for (var index = 0; index < span.Length; index++)
    {
        if (span[index] != space)
        {
            continue;
        }

        var digit = int.Parse(span[lastSpaceIndex..index]);
        list.Add(digit);
        lastSpaceIndex = index;
    }

    var lastDigit = int.Parse(span[lastSpaceIndex .. span.Length]);
    list.Add(lastDigit);
    return list;
}

return;

string? ReadLine()
    => Console.ReadLine()?.Trim('\r');

internal class Compressor
{
    private const int DescendingSequenceDiff = -1;
    private const int AscendingSequenceDiff = 1;
    private const int Zero = 0;

    public IReadOnlyList<int> Compress(IReadOnlyList<int> sequence)
    {
        var compressedSequence = new List<int>(sequence.Count + 1);
        if (sequence.Count == 1)
        {
            compressedSequence.Add(sequence[0]);
            compressedSequence.Add(Zero);
            return compressedSequence;
        }

        for (var compressElementIndex = 0; compressElementIndex < sequence.Count;)
        {
            var compressedElement = sequence[compressElementIndex];
            SequenceType? compressedSequenceType = null;
            var breakCount = 0;
            var index = compressElementIndex;
            for (; index < sequence.Count - 1; index++)
            {
                var currentElement = sequence[index];
                var nextElement = sequence[index + 1];
                var sequenceType = GetSequenceType(currentElement, nextElement);
                if (index == compressElementIndex)
                {
                    compressedSequenceType = sequenceType;
                }

                if (sequenceType == compressedSequenceType
                    && (sequenceType == SequenceType.Ascending
                        || sequenceType == SequenceType.Descending))
                {
                    breakCount += 1;
                    continue;
                }

                break;
            }

            if (breakCount > 0)
            {
                compressedSequence.Add(compressedElement);
                var compressCount = compressedSequenceType == SequenceType.Ascending
                    ? breakCount * AscendingSequenceDiff
                    : breakCount * DescendingSequenceDiff;
                compressedSequence.Add(compressCount);
            }
            else
            {
                compressedSequence.Add(compressedElement);
                compressedSequence.Add(Zero);
            }

            compressElementIndex = index + 1;
        }

        return compressedSequence;
    }

    private SequenceType GetSequenceType(int currentElement, int nextElement)
    {
        var diff = nextElement - currentElement;
        if (diff == AscendingSequenceDiff)
        {
            return SequenceType.Ascending;
        }

        if (diff == DescendingSequenceDiff)
        {
            return SequenceType.Descending;
        }

        return SequenceType.Other;
    }

    internal enum SequenceType
    {
        Descending,
        Ascending,
        Other
    }
}