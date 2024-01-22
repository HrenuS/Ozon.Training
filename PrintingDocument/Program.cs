// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Text;

var numberOfSets = int.Parse(ReadLine()!);
for (var setIndex = 0; setIndex < numberOfSets; setIndex++)
{
    var pageCount = int.Parse(ReadLine()!);
    var printer = new Printer(pageCount);
    var printedPages = ReadLine()!;
    printer.MarkPagesAsPrinted(printedPages);
    var answer = printer.GetPageForPrint();
    Console.WriteLine(answer);
}

return;

string? ReadLine()
    => Console.ReadLine()?.Trim('\r');

internal class Printer
{
    private const char Comma = ',';
    private const char Dash = '-';
    private readonly BitArray _pages;

    public Printer(int pageCount)
        => _pages = new BitArray(pageCount);

    private void MarkPageIsPrinted(int pageNumber)
    {
        // отсчет с 0
        _pages[pageNumber - 1] = true;
    }

    public void MarkPagesAsPrinted(string printedPages)
    {
        var sections = printedPages.Split(Comma, StringSplitOptions.RemoveEmptyEntries);
        foreach (var section in sections)
        {
            if (section.Length == 1)
            {
                var printedPage = int.Parse(section);
                MarkPageIsPrinted(printedPage);
            }
            else
            {
                var span = new ReadOnlySpan<char>(section.ToCharArray());
                var min = 0;
                var index = 0;
                for (; index < span.Length; index++)
                {
                    if (span[index] == Dash)
                    {
                        min = int.Parse(span[..index]);
                        break;
                    }
                }

                if (index == span.Length)
                {
                    MarkPageIsPrinted(int.Parse(span));
                    continue;
                }

                var max = int.Parse(span[(index + 1)..]);

                for (var page = min; page <= max; page++)
                {
                    MarkPageIsPrinted(page);
                }
            }
        }
    }

    private IEnumerable<string> GetRanges()
    {
        for (var index = 0; index < _pages.Length; index++)
        {
            var startIndex = index + 1;
            var start = _pages[index];
            var size = 1;
            if (start)
            {
                continue;
            }

            while (++index < _pages.Length && _pages[index] == start && start == false)
            {
                size += 1;
            }

            if (size == 1)
            {
                yield return startIndex.ToString();
            }

            if (size >= 2)
            {
                yield return $"{startIndex}-{startIndex + size - 1}";
            }
        }
    }

    public string GetPageForPrint()
    {
        var sb = new StringBuilder();

        foreach (var range in GetRanges())
        {
            if (sb.Length == 0)
            {
                sb.Append(range);
            }
            else
            {
                sb.Append(Comma);
                sb.Append(range);
            }
        }

        return sb.ToString();
    }
}