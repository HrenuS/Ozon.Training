// See https://aka.ms/new-console-template for more information

var numberOfSets = int.Parse(ReadLine()!);

for (var setIndex = 0; setIndex < numberOfSets; setIndex++)
{
    var commentCount = int.Parse(ReadLine()!);
    var commentStorage = new CommentStorage();
    for (var commentIndex = 0; commentIndex < commentCount; commentIndex++)
    {
        var commentLine = ReadLine()!;
        var comment = ParseComment(commentLine);
        commentStorage.AddComment(comment);
    }

    var tree = new TreeNode();
    foreach (var sortedComment in commentStorage.GetSortedComments())
    {
        tree.Add(sortedComment);
    }

    tree.PrintTree();
}

return;


Comment ParseComment(string line)
{
    const char space = ' ';
    var id = 0;
    var parentId = 0;
    var text = string.Empty;
    var span = new ReadOnlySpan<char>(line.ToCharArray());
    var spaceCount = 0;
    var parentSpaceIndex = 0;
    for (var index = 0; index < span.Length; index++)
    {
        if (span[index] == space)
        {
            if (spaceCount == 0)
            {
                id = int.Parse(span[..index]);
                parentSpaceIndex = index;
            }
            else if (spaceCount == 1)
            {
                parentId = int.Parse(span[parentSpaceIndex..index]);
                text = span[(index + 1)..].ToString();
                break;
            }

            spaceCount += 1;
        }
    }

    return new Comment(id, parentId, text);
}

string? ReadLine()
    => Console.ReadLine()?.Trim('\r');

public class CommentStorage
{
    private readonly SortedSet<Comment> _comments = new(new CommentComparer());

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public IEnumerable<Comment> GetSortedComments()
        => _comments;

    private class CommentComparer : IComparer<Comment>
    {
        public int Compare(Comment? x, Comment? y)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (ReferenceEquals(null, y))
            {
                return 1;
            }

            if (ReferenceEquals(null, x))
            {
                return -1;
            }

            if (x.ParentId == y.ParentId)
            {
                return x.Id.CompareTo(y.Id);
            }

            return x.ParentId.CompareTo(y.ParentId);
        }
    }
}

public class TreeNode
{
    private readonly List<TreeNode> _children = new();

    private readonly Comment? _comment;

    private TreeNode(Comment comment)
        => _comment = comment;

    public TreeNode()
    {
    }

    private TreeNode? FindNode(int id)
    {
        if (_comment?.Id == id)
        {
            return this;
        }

        return _children
            .Select(node => node.FindNode(id))
            .OfType<TreeNode>()
            .FirstOrDefault();
    }

    public void Add(Comment comment)
    {
        if (comment.ParentId == -1)
        {
            _children.Add(new TreeNode(comment));
            return;
        }

        if (_comment?.Id == comment.ParentId)
        {
            _children.Add(new TreeNode(comment));
            return;
        }

        var node = FindNode(comment.ParentId);
        if (node == null)
        {
            _children.Add(new TreeNode(comment));
        }
        else
        {
            node.Add(comment);
        }
    }

    public void PrintTree()
    {
        for (var i = 0; i < _children.Count; i++)
        {
            _children[i].PrintNode(string.Empty, false);
            Console.WriteLine();
        }
    }

    private void PrintNode(string indent, bool last)
    {
        if (_comment!.ParentId != -1)
        {
            Console.WriteLine(indent + "|");
            Console.Write(indent);

            if (!last)
            {
                Console.Write("|--");
                indent += "|  ";
            }
            else
            {
                Console.Write("|--");
                if (indent.Length > 1 && indent[indent.Length - 1] == '|')
                {
                    indent += "  ";
                }
                else
                {
                    indent += "   ";
                }
            }
        }

        Console.WriteLine(_comment!.Text);


        for (var i = 0; i < _children.Count; i++)
        {
            _children[i].PrintNode(indent, i == _children.Count - 1);
        }
    }
}


public record Comment(int Id, int ParentId, string Text)
{
    public readonly int Id = Id;
    public readonly int ParentId = ParentId;
    public readonly string Text = Text;
}