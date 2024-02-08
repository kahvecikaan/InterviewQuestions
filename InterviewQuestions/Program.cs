namespace InterviewQuestions;

class Result
{
    public static List<List<int>> FindEffectedCells(char[][] grid, int startX, int startY)
    {
        var affectedCells = new HashSet<(int, int)>();
        var toProcess = new Queue<(int, int)>();
        toProcess.Enqueue((startX, startY));

        while (toProcess.Count > 0)
        {
            var (x, y) = toProcess.Dequeue();
            if (!affectedCells.Contains((x, y)))
            {
                affectedCells.Add((x, y));
                
                char item = grid[y][x];
                
                switch (item)
                {
                    case 'c': // Cube affects only its own cell, already added to affectedCells
                        break;
                    case 'b': // Bomb affects 3x3 area
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                int newX = x + j, newY = y + i;
                                if (newX >= 0 && newX < grid[0].Length && newY >= 0 && newY < grid.Length)
                                {
                                    toProcess.Enqueue((newX, newY));
                                }
                            }
                        }

                        break;
                    case 'h': // Horizontal rocket affects all cells in its row
                        for(int i = 0; i < grid[0].Length; i++)
                        {
                            if (!affectedCells.Contains((i, y)))
                            {
                                toProcess.Enqueue((i, y));
                            }
                        }

                        break;
                    case 'v': // Vertical rocket affects all cells in its column
                        for (int i = 0; i < grid.Length; i++)
                        {
                            if (!affectedCells.Contains((x, i)))
                            {
                                toProcess.Enqueue((x, i));
                            }
                        }

                        break;
                }
            }
        }
        
        return affectedCells.Select(cell => new List<int> { cell.Item1, cell.Item2 }).ToList();
    }
}

class Result2
{
    public static List<List<int>> PerformActions(int width, int height, List<List<int>> snakeData, List<List<int>> foodsData, List<char> actions) {
        var snake = new LinkedList<(int, int)>(snakeData.Select(s => (s[0], s[1])));
        
        var foods = new HashSet<(int, int)>(foodsData.Select(f => (f[0], f[1])));
        
        // Direction vectors: Up, Right, Down, Left
        var directions = new[] { (0, -1), (1, 0), (0, 1), (-1, 0) };

        // Determine initial direction based on the last two segments of the snake
        var tailEnd = snake.ElementAt(snake.Count - 2);
        var head = snake.Last();
        int currentDirection;
        if (head.Item1 == tailEnd.Item1) // the movement is vertical
        {
            currentDirection = head.Item2 > tailEnd.Item2 ? 2 : 0; // Down if head is below tailEnd, Up otherwise
        }
        else // the movement is horizontal
        {
            currentDirection = head.Item1 > tailEnd.Item1 ? 1 : 3; // Right if head is to the right of tailEnd, Left otherwise
        }
        
        foreach (char action in actions)
        {
            if (action == 'L' || action == 'R')
            {
                // Update direction based on action
                currentDirection = action == 'L' ? (currentDirection + 3) % 4 : (currentDirection + 1) % 4;
                // Move one cell in the new direction
            }

            // Move one cell forward in the current direction
            head = snake.Last.Value;
            var newHead = (head.Item1 + directions[currentDirection].Item1, 
                head.Item2 + directions[currentDirection].Item2);

            // Check for grid boundary collisions and self-collision
            if (newHead.Item1 < 0 || newHead.Item1 >= width || newHead.Item2 < 0 || newHead.Item2 >= height || 
                snake.Any(s => s.Item1 == newHead.Item1 && s.Item2 == newHead.Item2))
            {
                break; // End simulation on collision
            }

            snake.AddLast(newHead); // Move forward by adding a new head

            if (!foods.Contains(newHead))
            {
                snake.RemoveFirst(); // No food found, so remove the tail
            }
            else
            {
                foods.Remove(newHead); // Food consumed, snake grows
            }
        }

        return snake.Select(s => new List<int> { s.Item1, s.Item2 }).ToList(); 
    }
}

class Solution
{
    public static void Main(string[] args)
    {
        
        /* FIRST QUESTION
        var grid = new char[5][];
        grid[0] = new char[] { 'c', 'c', 'c', 'h', 'c' };
        grid[1] = new char[] { 'c', 'b', 'c', 'c', 'c' };
        grid[2] = new char[] { 'c', 'c', 'v', 'c', 'c' };
        grid[3] = new char[] { 'c', 'c', 'c', 'c', 'c' };
        grid[4] = new char[] { 'c', 'c', 'c', 'h', 'c' };
        
        // Console.WriteLine(grid[0][2]); for accessing the element in first row and third column which is 'b'
        
        Console.WriteLine(grid[0]); // first row
        Console.WriteLine(grid.Length); // number of rows
        Console.WriteLine(grid[0].Length); // number of columns
        
        int startX = 1;
        int startY = 1;
        
        List<List<int>> result = Result.FindEffectedCells(grid, startX, startY);
        
        // Sort the result
        var sortedResult = result.OrderBy(list => list[0]).ThenBy(list => list[1]).ToList();
        
        // Build the output string in the desired format
        string output = "[" + string.Join(", ", sortedResult.Select(list => $"[{list[0]},{list[1]}]")) + "]";
        
        Console.WriteLine("Result: ");
        // Print the sorted list of tuples
        foreach (var item in sortedResult)
        {
            Console.WriteLine($"({item[0]}, {item[1]})");
        }
        
        Console.WriteLine(output); */
        
        // --------------------------------------------------------------------------------
        
        // SECOND QUESTION
        
        // Grid size
        int width = 6;
        int height = 6;
        
        // Initial snake position
        List<List<int>> snake = new List<List<int>>
        {
            new List<int> { 1, 2 },
            new List<int> { 1, 1 },
            new List<int> { 2, 1 },
            new List<int> { 3, 1 },
            new List<int> { 3, 2 }
        };
        
        // Food positions
        List<List<int>> foods = new List<List<int>>
        {
            new List<int> { 0, 1 },
            new List<int> { 1, 5 },
            new List<int> { 2, 2 },
            new List<int> { 3, 4 },
            new List<int> { 4, 0 }
        };
        
        // Actions
        List<char> actions = new List<char> { 'F', 'F', 'L', 'F', 'R', 'R', 'F' };
        
        // Perform actions
        List<List<int>> result = Result2.PerformActions(width, height, snake, foods, actions);
        
        Console.WriteLine("Final snake positions:");
        foreach (var cell in result)
        {
            Console.WriteLine($"[{cell[0]}, {cell[1]}]");
        }
    }
}