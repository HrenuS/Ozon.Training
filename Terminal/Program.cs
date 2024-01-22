// See https://aka.ms/new-console-template for more information

using System.Text;

var numberOfSets = int.Parse(ReadLine()!);

for (var setIndex = 0; setIndex < numberOfSets; setIndex++)
{
    var terminal = new Terminal();
    var inputText = ReadLine()!;
    terminal.InputText(inputText);
    terminal.PrintLines();

    Console.WriteLine("-");
}

return;

string? ReadLine()
    => Console.ReadLine()?.Trim('\r');

internal class Terminal
{
    private readonly Dictionary<char, Action<Cursor, List<StringBuilder>>> _commands;

    private readonly Cursor _cursor = new(0);

    private readonly List<StringBuilder> _terminalText = new() { new StringBuilder() };

    public Terminal()
    {
        _commands = new Dictionary<char, Action<Cursor, List<StringBuilder>>>
        {
            { 'L', (cursor, _) => { cursor.MoveLeft(); } },
            { 'R', (cursor, lines) => cursor.MoveRight(lines[cursor.Line].Length) },
            { 'U', MoveUp },
            { 'D', MoveDown },
            { 'B', (cursor, _) => cursor.MoveStart() },
            { 'E', (cursor, lines) => cursor.MoveEnd(lines[cursor.Line].Length) },
            { 'N', NewLine }
        };
    }

    private void InputSymbol(char symbol)
    {
        _terminalText[_cursor.Line].Insert(_cursor.Index, symbol);
        _cursor.MoveRight();
    }

    private void MoveUp(Cursor cursor, List<StringBuilder> terminalText)
    {
        if (terminalText.Count == 1)
        {
            return;
        }

        if (cursor.Line - 1 < 0)
        {
            return;
        }

        var upLineLength = terminalText[cursor.Line - 1].Length;

        if (upLineLength >= cursor.Index)
        {
            cursor.MoveUp(cursor.Index);
        }
        else
        {
            cursor.MoveUp(upLineLength);
        }
    }

    private void MoveDown(Cursor cursor, List<StringBuilder> terminalText)
    {
        if (terminalText.Count == 1)
        {
            return;
        }

        if (cursor.Line + 1 >= terminalText.Count)
        {
            return;
        }

        var downLineLength = terminalText[cursor.Line + 1].Length;

        if (downLineLength >= cursor.Index)
        {
            cursor.MoveDown(cursor.Index);
        }
        else
        {
            cursor.MoveDown(downLineLength);
        }
    }

    private void NewLine(Cursor cursor, List<StringBuilder> terminalText)
    {
        var currentLine = terminalText[cursor.Line];
        var newLine = new StringBuilder();
        var length = 0;
        for (var index = cursor.Index; index < currentLine.Length; index++)
        {
            length += 1;
            newLine.Append(currentLine[index]);
        }

        currentLine.Remove(cursor.Index, length);
        cursor.Enter();
        terminalText.Insert(cursor.Line, newLine);
    }

    public void InputText(string text)
    {
        foreach (var symbol in text)
        {
            if (_commands.TryGetValue(symbol, out var command))
            {
                command(_cursor, _terminalText);
            }
            else
            {
                InputSymbol(symbol);
            }
        }
    }

    public void PrintLines()
    {
        foreach (var line in _terminalText)
        {
            Console.WriteLine(line);
        }
    }

    private class Cursor
    {
        public Cursor(int index)
            => Index = index;

        public int Index { get; private set; }
        public int Line { get; private set; }

        public void MoveLeft()
        {
            if (Index == 0)
            {
                return;
            }

            Index -= 1;
        }

        public void MoveRight(int maxPosition)
        {
            if (Index + 1 > maxPosition)
            {
                return;
            }

            MoveRight();
        }

        public void MoveRight()
        {
            Index += 1;
        }

        public void MoveUp(int index)
        {
            Line -= 1;
            Index = index;
        }

        public void MoveDown(int index)
        {
            Line += 1;
            Index = index;
        }

        public void MoveStart()
        {
            Index = 0;
        }

        public void MoveEnd(int index)
        {
            Index = index;
        }

        public void Enter()
        {
            Line += 1;
            Index = 0;
        }
    }
}