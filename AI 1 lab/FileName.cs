//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Diagnostics;
//using System.Linq;
//using System.Windows.Forms;
//using System.IO;
//using System.Threading.Tasks;
//using System.Net.NetworkInformation;

//namespace AI_1_lab
//{
//    public partial class Form1 : Form
//    {
//        private const int MinBoardSize = 3;
//        private const int MaxBoardSize = 10;
//        private int currentBoardSize = 4;
//        private Button[,] buttons;
//        private bool[,] forbiddenCells;
//        private Knight[] knights;
//        private Color blackColor = ColorTranslator.FromHtml("#000000");
//        private Color whiteColor = ColorTranslator.FromHtml("#FFFFFF");
//        private Color lightColor = ColorTranslator.FromHtml("#B0E0E6");
//        private Color darkColor = ColorTranslator.FromHtml("#7B68EE");

//        private int Depth = 1;
//        private int Step = 1;
//        public Form1()
//        {
//            InitializeComponent();
//            InitializeBoard();
//        }

//        private void InitializeBoard()
//        {
//            // Удаляем старые кнопки, если они есть
//            if (buttons != null)
//            {
//                foreach (var button in buttons)
//                {
//                    this.Controls.Remove(button);
//                }
//            }

//            // Очистка старых меток (если они были добавлены)
//            var labelsToRemove = this.Controls.OfType<Label>().ToList();
//            foreach (var label in labelsToRemove)
//            {
//                this.Controls.Remove(label);
//            }

//            // Инициализация кнопок для доски
//            buttons = new Button[currentBoardSize, currentBoardSize];
//            forbiddenCells = new bool[currentBoardSize, currentBoardSize];
//            knights = new Knight[]
//            {
//                new Knight(0, 0, blackColor),
//                new Knight(0, currentBoardSize - 1, blackColor),
//                new Knight(currentBoardSize - 1, 0, whiteColor),
//                new Knight(currentBoardSize - 1, currentBoardSize - 1, whiteColor)
//            };

//            // Устанавливаем размер окна
//            this.ClientSize = new Size(currentBoardSize * 60 + 250, currentBoardSize * 60 + 80);
//            this.btnSolve.Location = new Point(this.ClientSize.Width - 180, this.btnSolve.Location.Y);
//            this.btnDFS.Location = new Point(this.ClientSize.Width - 180, this.btnDFS.Location.Y);
//            this.BtnIncreaseSize.Location = new Point(this.ClientSize.Width - 180, this.BtnIncreaseSize.Location.Y);
//            this.BtnDecreaseSize.Location = new Point(this.ClientSize.Width - 180, this.BtnDecreaseSize.Location.Y);
//            this.btnReset.Location = new Point(this.ClientSize.Width - 180, this.btnReset.Location.Y);
//            this.UpDownDepth.Location = new Point(this.ClientSize.Width - 180, this.UpDownDepth.Location.Y);
//            this.UpDownStep.Location = new Point(this.ClientSize.Width - 180, this.UpDownStep.Location.Y);

//            // Создаем кнопки для шахматной доски
//            for (int i = 0; i < currentBoardSize; i++)
//            {
//                for (int j = 0; j < currentBoardSize; j++)
//                {
//                    buttons[i, j] = new Button
//                    {
//                        Location = new Point(i * 60, j * 60 + 40),
//                        Size = new Size(60, 60),
//                        BackColor = (i + j) % 2 == 0 ? lightColor : darkColor,
//                        Enabled = true
//                    };
//                    buttons[i, j].Click += CellButton_Click;
//                    this.Controls.Add(buttons[i, j]);
//                }
//            }

//            // Добавляем буквы сверху
//            for (int i = 0; i < currentBoardSize; i++)
//            {
//                Label label = new Label
//                {
//                    Text = ((char)('A' + i)).ToString(), // Преобразуем индекс в букву
//                    Location = new Point(i * 60 + 15, 10), // Позиция в верхней части
//                    Size = new Size(30, 20),
//                    TextAlign = ContentAlignment.MiddleCenter
//                };
//                this.Controls.Add(label);
//            }

//            // Добавляем цифры справа
//            for (int j = 0; j < currentBoardSize; j++)
//            {
//                Label label = new Label
//                {
//                    Text = (currentBoardSize - j).ToString(), // Преобразуем индекс в число
//                    Location = new Point(currentBoardSize * 60 + 10, j * 60 + 40 + 15), // Позиция справа от кнопок
//                    Size = new Size(30, 20),
//                    TextAlign = ContentAlignment.MiddleCenter
//                };
//                this.Controls.Add(label);
//            }

//            UpdateBoard();
//        }
//        private async Task AnimatePath(List<string> path, State initialState)
//        {
//            // Копируем исходное состояние для анимации
//            knights = CloneKnights(initialState.Knights);
//            UpdateBoard(); // Обновляем доску, чтобы начать с исходного состояния

//            // Пошаговая анимация передвижения коней
//            foreach (var move in path)
//            {
//                // Разбираем строку с описанием хода, чтобы понять координаты
//                var parts = move.Split(new[] { '(', ';', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
//                string knightColor = parts[0];
//                int startX = int.Parse(parts[2]);
//                int startY = int.Parse(parts[3]);
//                int endX = int.Parse(parts[5]);
//                int endY = int.Parse(parts[6]);

//                // Находим коня по исходной позиции
//                var knight = knights.FirstOrDefault(k => k.X == startX && k.Y == startY);
//                if (knight != null)
//                {
//                    // Обновляем позицию коня
//                    knight.X = endX;
//                    knight.Y = endY;

//                    // Обновляем отображение на доске
//                    UpdateBoard();
//                    await Task.Delay(500); // Пауза в 500 мс между шагами для наглядности
//                }
//            }
//        }

//        private void CellButton_Click(object sender, EventArgs e)
//        {
//            Button button = sender as Button;
//            int x = button.Location.X / 60; // Количество пикселей между кнопками
//            int y = (button.Location.Y - 40) / 60; // Учитываем высоту заголовка окна

//            if (forbiddenCells[x, y])
//            {
//                forbiddenCells[x, y] = false; // Делаем клетку доступной
//                button.BackColor = (x + y) % 2 == 0 ? lightColor : darkColor;
//            }
//            else
//            {
//                // Запретить клетку
//                forbiddenCells[x, y] = true;
//                button.BackColor = Color.Red; // Измените цвет, чтобы выделить запрещенную клетку
//            }


//        }
//        private void BtnIncreaseSize_Click(object sender, EventArgs e)
//        {
//            if (currentBoardSize < MaxBoardSize)
//            {
//                currentBoardSize++;
//                InitializeBoard(); // Переинициализируем доску
//            }
//        }
//        private void BtnDecreaseSize_Click(object sender, EventArgs e)
//        {
//            if (currentBoardSize > MinBoardSize)
//            {
//                currentBoardSize--;
//                InitializeBoard(); // Переинициализируем доску
//            }
//        }

//        private void UpdateBoard()
//        {
//            for (int i = 0; i < currentBoardSize; i++)
//            {
//                for (int j = 0; j < currentBoardSize; j++)
//                {
//                    if (forbiddenCells[i, j])
//                        buttons[i, j].BackColor = Color.Red;
//                    else
//                        buttons[i, j].BackColor = (i + j) % 2 == 0 ? lightColor : darkColor;
//                    buttons[i, j].Text = "";
//                    buttons[i, j].Enabled = true;
//                }
//            }

//            foreach (var knight in knights)
//            {
//                if (knight.X < currentBoardSize && knight.Y < currentBoardSize)
//                {
//                    buttons[knight.X, knight.Y].Text = "♞";
//                    buttons[knight.X, knight.Y].ForeColor = knight.Color;
//                    buttons[knight.X, knight.Y].Font = new Font(buttons[knight.X, knight.Y].Font.FontFamily, 30);
//                    buttons[knight.X, knight.Y].Enabled = true;
//                }
//            }

//        }
//        private string GetForbiddenCellsInfo()
//        {
//            List<string> forbiddenPositions = new List<string>();

//            for (int i = 0; i < currentBoardSize; i++)
//            {
//                for (int j = 0; j < currentBoardSize; j++)
//                {
//                    if (forbiddenCells[i, j])
//                    {
//                        forbiddenPositions.Add($"({i},{j})");
//                    }
//                }
//            }

//            return forbiddenPositions.Count > 0 ? $"Запрещённые клетки: {string.Join(", ", forbiddenPositions)}" : "Нет запрещённых клеток.";
//        }

//        private string ToChessNotation(int x, int y)
//        {
//            char file = (char)('a' + x); // Преобразуем индекс x в символ (a, b, c...)
//            int rank = currentBoardSize - y; // Преобразуем индекс y в номер (1, 2, 3...)
//            return $"{file}{rank}";
//        }

//        private async void BFS()
//        {
//            var stopwatch = new Stopwatch();
//            stopwatch.Start();

//            int operationCount = 0;

//            Queue<State> queue = new Queue<State>();
//            HashSet<string> visited = new HashSet<string>();
//            Dictionary<State, List<string>> paths = new Dictionary<State, List<string>>();
//            Dictionary<State, List<string>> paths2 = new Dictionary<State, List<string>>();

//            State initialState = new State(CloneKnights(knights));
//            queue.Enqueue(initialState);
//            visited.Add(initialState.ToString());
//            paths[initialState] = new List<string>();
//            paths2[initialState] = new List<string>();


//            while (queue.Count > 0)
//            {
//                State currentState = queue.Dequeue();
//                operationCount++;

//                if (IsGoalState(currentState))
//                {
//                    var path = paths[currentState];
//                    var path2 = paths2[currentState];
//                    string pathDescription = string.Join(Environment.NewLine, path2);
//                    int totalMoves = path.Count;

//                    stopwatch.Stop();
//                    TimeSpan elapsed = stopwatch.Elapsed;
//                    string forbidden = GetForbiddenCellsInfo();

//                    MessageBox.Show($"Решение найдено!\n" +
//                                    $"Общее количество ходов: {totalMoves}\n" +
//                                    $"Запрещённые поля: {forbidden}\n" +
//                                    $"Общее количество итераций: {operationCount}\n" +
//                                    $"Время поиска: {elapsed.TotalSeconds} мс\n" +
//                                    $"Ходы:\n{pathDescription}");

//                    await AnimatePath(path, initialState); // Запуск анимации
//                    return;
//                }

//                foreach (var nextState in GenerateNextStates(currentState))
//                {
//                    if (!visited.Contains(nextState.ToString()))
//                    {
//                        queue.Enqueue(nextState);
//                        visited.Add(nextState.ToString());
//                        operationCount++;

//                        if (!paths.ContainsKey(nextState))
//                        {
//                            paths[nextState] = new List<string>(paths[currentState]);
//                        }
//                        if (!paths2.ContainsKey(nextState))
//                        {
//                            paths2[nextState] = new List<string>(paths2[currentState]);
//                        }

//                        for (int i = 0; i < nextState.Knights.Length; i++)
//                        {
//                            var knightMove = nextState.Knights[i];
//                            var originalKnight = currentState.Knights[i];

//                            if (originalKnight.X != knightMove.X || originalKnight.Y != knightMove.Y)
//                            {
//                                string knightColor = originalKnight.Color == blackColor ? "Черный" : "Белый";
//                                string moveDescription = $"{knightColor} конь ({originalKnight.X};{originalKnight.Y}) -> ({knightMove.X};{knightMove.Y})";
//                                paths[nextState].Add(moveDescription);
//                                string moveDescription2 = $"{knightColor} конь {ToChessNotation(originalKnight.X, originalKnight.Y)} -> {ToChessNotation(knightMove.X, knightMove.Y)}"; // Изменено
//                                paths2[nextState].Add(moveDescription2);
//                            }
//                        }
//                    }
//                }
//            }


//            stopwatch.Stop();
//            TimeSpan elapsedTime = stopwatch.Elapsed;

//            MessageBox.Show($"Решение не найдено.\nОбщее количество операций: {operationCount}\nВремя поиска: {elapsedTime.TotalSeconds} мс");
//            ClearResources(queue, visited, paths, paths2);
//        }

//        private async void ReverseBFS()
//        {
//            var stopwatch = new Stopwatch();
//            stopwatch.Start();

//            int operationCount = 0;

//            Queue<State> queue = new Queue<State>();
//            HashSet<string> visited = new HashSet<string>();
//            Dictionary<State, List<string>> paths = new Dictionary<State, List<string>>();
//            Dictionary<State, List<string>> paths2 = new Dictionary<State, List<string>>();

//            State goalState = new State(new Knight[]
//            {
//        new Knight(currentBoardSize - 1, currentBoardSize - 1, blackColor),
//        new Knight(currentBoardSize - 1, 0, blackColor),
//        new Knight(0, currentBoardSize - 1, whiteColor),
//        new Knight(0, 0, whiteColor)
//            });

//            queue.Enqueue(goalState);
//            visited.Add(goalState.ToString());
//            paths[goalState] = new List<string>();
//            paths2[goalState] = new List<string>();

//            while (queue.Count > 0)
//            {
//                State currentState = queue.Dequeue();
//                operationCount++;

//                // Проверка на начальное состояние
//                if (currentState.ToString() == new State(CloneKnights(knights)).ToString())
//                {
//                    var path = paths[currentState];
//                    var path2 = paths2[currentState];
//                    string pathDescription = string.Join(Environment.NewLine, path2);
//                    int totalMoves = path.Count;

//                    stopwatch.Stop();
//                    TimeSpan elapsed = stopwatch.Elapsed;
//                    string forbidden = GetForbiddenCellsInfo();

//                    MessageBox.Show($"Решение найдено (в обратную сторону)!\n" +
//                                    $"Общее количество ходов: {totalMoves}\n" +
//                                    $"Запрещённые поля: {forbidden}\n" +
//                                    $"Общее количество итераций: {operationCount}\n" +
//                                    $"Время поиска: {elapsed.TotalSeconds} мс\n" +
//                                    $"Ходы:\n{pathDescription}");

//                    await AnimatePath(path, goalState); // Запуск анимации
//                    return;
//                }

//                // Генерация предыдущих состояний
//                foreach (var previousState in GeneratePreviousStates(currentState))
//                {
//                    if (!visited.Contains(previousState.ToString()))
//                    {
//                        queue.Enqueue(previousState);
//                        visited.Add(previousState.ToString());
//                        operationCount++;

//                        if (!paths.ContainsKey(previousState))
//                        {
//                            paths[previousState] = new List<string>(paths[currentState]);
//                        }
//                        if (!paths2.ContainsKey(previousState))
//                        {
//                            paths2[previousState] = new List<string>(paths2[currentState]);
//                        }

//                        for (int i = 0; i < previousState.Knights.Length; i++)
//                        {
//                            var knightMove = previousState.Knights[i];
//                            var originalKnight = currentState.Knights[i];

//                            if (originalKnight.X != knightMove.X || originalKnight.Y != knightMove.Y)
//                            {
//                                string knightColor = originalKnight.Color == blackColor ? "Черный" : "Белый";
//                                string moveDescription = $"{knightColor} конь ({originalKnight.X};{originalKnight.Y}) -> ({knightMove.X};{knightMove.Y})";
//                                paths[previousState].Add(moveDescription);
//                                string moveDescription2 = $"{knightColor} конь {ToChessNotation(originalKnight.X, originalKnight.Y)} -> {ToChessNotation(knightMove.X, knightMove.Y)}";
//                                paths2[previousState].Add(moveDescription2);
//                            }
//                        }
//                    }
//                }
//            }

//            stopwatch.Stop();
//            TimeSpan elapsedTime = stopwatch.Elapsed;

//            MessageBox.Show($"Решение не найдено (в обратную сторону).\nОбщее количество операций: {operationCount}\nВремя поиска: {elapsedTime.TotalSeconds} мс");
//            ClearResources(queue, visited, paths, paths2);
//        }

//        public List<string> MergeSolutions(List<string> bfsSolution, List<string> reverseBfsSolution)
//        {
//            // Найдем точку соприкосновения — последнюю общую точку в обоих списках решений
//            string meetingPoint = null;

//            // Перебираем все шаги прямого BFS
//            foreach (var step in bfsSolution)
//            {
//                // Если шаг из прямого BFS присутствует и в обратном решении
//                if (reverseBfsSolution.Contains(step))
//                {
//                    meetingPoint = step;
//                    break;
//                }
//            }

//            // Если точка соприкосновения не найдена, возвращаем пустой список
//            if (meetingPoint == null)
//            {
//                return new List<string>(); // Решений нет
//            }

//            // 1. Собираем решение от начального состояния до точки соприкосновения
//            List<string> mergedSolution = new List<string>();
//            foreach (var step in bfsSolution)
//            {
//                mergedSolution.Add(step);
//                if (step == meetingPoint)
//                {
//                    break; // Останавливаемся на точке соприкосновения
//                }
//            }

//            // 2. Добавляем путь от точки соприкосновения до конечного состояния (реверсируем путь обратного BFS)
//            int meetingIndex = reverseBfsSolution.IndexOf(meetingPoint);
//            for (int i = meetingIndex + 1; i < reverseBfsSolution.Count; i++)
//            {
//                mergedSolution.Add(reverseBfsSolution[i]);
//            }

//            return mergedSolution;
//        }

//        private void ClearResources(Queue<State> queue, HashSet<string> visited, Dictionary<State, List<string>> paths, Dictionary<State, List<string>> paths2)
//        {
//            queue.Clear();
//            visited.Clear();
//            paths.Clear();
//            paths2.Clear();
//        }

//        private void ClearResources(Stack<State> stack, HashSet<string> visited, Dictionary<State, List<string>> paths, Dictionary<State, List<string>> paths2)
//        {
//            stack.Clear();
//            visited.Clear();
//            paths.Clear();
//            paths2.Clear();
//        }

//        private void ClearResources(Stack<(State state, int depth)> stack, HashSet<string> visited, Dictionary<State, List<string>> paths, Dictionary<State, List<string>> paths2)
//        {
//            stack.Clear();
//            visited.Clear();
//            paths.Clear();
//            paths2.Clear();
//        }
//        private bool IsGoalState(State state)
//        {
//            return (state.Knights[0].X == currentBoardSize - 1 && state.Knights[0].Y == currentBoardSize - 1 &&
//                state.Knights[1].X == currentBoardSize - 1 && state.Knights[1].Y == 0 &&
//                state.Knights[2].X == 0 && state.Knights[2].Y == currentBoardSize - 1 &&
//                state.Knights[3].X == 0 && state.Knights[3].Y == 0);
//        }

//        private List<State> GenerateNextStates(State currentState)
//        {
//            List<State> nextStates = new List<State>();

//            foreach (var knight in currentState.Knights)
//            {
//                List<Knight> possibleMoves = knight.GetPossibleMoves(currentState.Knights, forbiddenCells, currentBoardSize); // Передаем размер доски

//                foreach (var move in possibleMoves)
//                {
//                    // Создаем новую копию состояния с обновленными позициями коней
//                    Knight[] newKnights = CloneKnights(currentState.Knights);
//                    newKnights[Array.IndexOf(currentState.Knights, knight)] = move; // Перемещение коня

//                    // Проверка на выход за пределы доски
//                    if (IsInBounds(move.X, move.Y))
//                    {
//                        nextStates.Add(new State(newKnights)); // Добавление нового состояния
//                    }
//                }
//            }

//            return nextStates;
//        }

//        // Метод для проверки, находится ли позиция в пределах границ доски
//        private bool IsInBounds(int x, int y)
//        {
//            return x >= 0 && x < currentBoardSize && y >= 0 && y < currentBoardSize;
//        }

//        private void btnSolve_Click(object sender, EventArgs e)
//        {
//            BFS();
//        }

//        public class Knight
//        {
//            public int X { get; set; }
//            public int Y { get; set; }
//            public Color Color { get; }

//            public Knight(int x, int y, Color color)
//            {
//                X = x;
//                Y = y;
//                Color = color;
//            }

//            public List<Knight> GetPossibleMoves(Knight[] knights, bool[,] forbiddenCells, int boardSize)
//            {
//                var moves = new (int dx, int dy)[]
//                {
//                    (2, 1), (2, -1), (-2, 1), (-2, -1),
//                    (1, 2), (1, -2), (-1, 2), (-1, -2)
//                };

//                List<Knight> possibleMoves = new List<Knight>();

//                foreach (var (dx, dy) in moves)
//                {
//                    int newX = X + dx;
//                    int newY = Y + dy;

//                    if (IsInBounds(newX, newY, boardSize) && !IsOccupied(newX, newY, knights) && !forbiddenCells[newX, newY])
//                    {
//                        possibleMoves.Add(new Knight(newX, newY, Color));
//                    }
//                }

//                return possibleMoves;
//            }


//            private bool IsInBounds(int x, int y, int boardSize) // Передаем размер доски
//            {
//                return x >= 0 && x < boardSize && y >= 0 && y < boardSize;
//            }

//            private bool IsOccupied(int x, int y, Knight[] knights)
//            {
//                return knights.Any(k => k.X == x && k.Y == y);
//            }
//        }

//        public class State
//        {
//            public Knight[] Knights { get; }

//            public State(Knight[] knights)
//            {
//                Knights = knights;
//            }

//            public override string ToString()
//            {
//                return string.Join(",", Knights.Select(k => $"{k.X},{k.Y}"));
//            }
//        }

//        private Knight[] CloneKnights(Knight[] knights)
//        {
//            return knights.Select(k => new Knight(k.X, k.Y, k.Color)).ToArray();
//        }


//        private void btnReset_Click(object sender, EventArgs e)
//        {
//            InitializeBoard();
//        }


//        private void button3_Click(object sender, EventArgs e)
//        {
//            MergeSolutions();
//        }
//    }
//}
