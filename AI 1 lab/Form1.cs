using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Runtime;
using System.Text;

namespace AI_1_lab
{
    public partial class Form1 : Form
    {
        private const int MinBoardSize = 3;
        private const int MaxBoardSize = 10;
        private int currentBoardSize = 4;
        private Button[,] buttons;
        private bool[,] forbiddenCells;
        private Knight[] knights;
        private Color blackColor = ColorTranslator.FromHtml("#000000");
        private Color whiteColor = ColorTranslator.FromHtml("#FFFFFF");
        private Color lightColor = ColorTranslator.FromHtml("#B0E0E6");
        private Color darkColor = ColorTranslator.FromHtml("#7B68EE");

        private int Depth = 1;
        private int Step = 1;
        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Удаляем старые кнопки, если они есть
            if (buttons != null)
            {
                foreach (var button in buttons)
                {
                    this.Controls.Remove(button);
                }
            }

            // Очистка старых меток (если они были добавлены)
            var labelsToRemove = this.Controls.OfType<Label>().ToList();
            foreach (var label in labelsToRemove)
            {
                this.Controls.Remove(label);
            }

            // Инициализация кнопок для доски
            buttons = new Button[currentBoardSize, currentBoardSize];
            forbiddenCells = new bool[currentBoardSize, currentBoardSize];
            knights = new Knight[]
            {
                new Knight(0, 0, blackColor),
                new Knight(0, currentBoardSize - 1, blackColor),
                new Knight(currentBoardSize - 1, 0, whiteColor),
                new Knight(currentBoardSize - 1, currentBoardSize - 1, whiteColor)
            };

            // Устанавливаем размер окна
            this.ClientSize = new Size(currentBoardSize * 60 + 250, currentBoardSize * 60 + 80);
            this.btnSolve.Location = new Point(this.ClientSize.Width - 180, this.btnSolve.Location.Y);
            this.btnDFS.Location = new Point(this.ClientSize.Width - 180, this.btnDFS.Location.Y);
            this.btnBBFS.Location = new Point(this.ClientSize.Width - 180, this.btnBBFS.Location.Y);
            this.btnIDFS.Location = new Point(this.ClientSize.Width - 180, this.btnIDFS.Location.Y);
            this.BtnIncreaseSize.Location = new Point(this.ClientSize.Width - 180, this.BtnIncreaseSize.Location.Y);
            this.BtnDecreaseSize.Location = new Point(this.ClientSize.Width - 180, this.BtnDecreaseSize.Location.Y);
            this.btnReset.Location = new Point(this.ClientSize.Width - 180, this.btnReset.Location.Y);
            this.UpDownDepth.Location = new Point(this.ClientSize.Width - 180, this.UpDownDepth.Location.Y);
            this.UpDownStep.Location = new Point(this.ClientSize.Width - 180, this.UpDownStep.Location.Y);

            // Создаем кнопки для шахматной доски
            for (int i = 0; i < currentBoardSize; i++)
            {
                for (int j = 0; j < currentBoardSize; j++)
                {
                    buttons[i, j] = new Button
                    {
                        Location = new Point(i * 60, j * 60 + 40),
                        Size = new Size(60, 60),
                        BackColor = (i + j) % 2 == 0 ? lightColor : darkColor,
                        Enabled = true
                    };
                    buttons[i, j].Click += CellButton_Click;
                    this.Controls.Add(buttons[i, j]);
                }
            }

            // Добавляем буквы сверху
            for (int i = 0; i < currentBoardSize; i++)
            {
                Label label = new Label
                {
                    Text = ((char)('A' + i)).ToString(), // Преобразуем индекс в букву
                    Location = new Point(i * 60 + 15, 10), // Позиция в верхней части
                    Size = new Size(30, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                this.Controls.Add(label);
            }

            // Добавляем цифры справа
            for (int j = 0; j < currentBoardSize; j++)
            {
                Label label = new Label
                {
                    Text = (currentBoardSize - j).ToString(), // Преобразуем индекс в число
                    Location = new Point(currentBoardSize * 60 + 10, j * 60 + 40 + 15), // Позиция справа от кнопок
                    Size = new Size(30, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                this.Controls.Add(label);
            }

            UpdateBoard();
        }
        private async Task AnimatePath(List<State> path, State initialState)
        {
            await Task.Delay(500);
            // Копируем исходное состояние для анимации
            knights = CloneKnights(initialState.knights.ToArray());
            UpdateBoard(); // Обновляем доску, чтобы начать с исходного состояния

            // Пошаговая анимация передвижения коней
            foreach (var state in path)
            {
                // Обновляем позиции коней
                knights = CloneKnights(state.knights.ToArray());

                // Обновляем отображение на доске
                UpdateBoard();
                await Task.Delay(500); // Пауза в 500 мс между шагами для наглядности
            }
        }

        private Knight[] CloneKnights(Knight[] knights)
        {
            return knights.Select(k => new Knight(k.X, k.Y, k.Color)).ToArray();
        }

        private async Task AnimatePath(List<State> path)
        {
            // Даем немного времени перед началом анимации
            await Task.Delay(500);

            // Копируем исходное состояние для анимации
            knights = CloneKnights(path.First().knights.ToArray());
            UpdateBoard(); // Обновляем доску, чтобы начать с исходного состояния

            // Пошаговая анимация передвижения коней
            foreach (var state in path)
            {
                // Обновляем позиции коней
                knights = CloneKnights(state.knights.ToArray());

                // Обновляем отображение на доске
                UpdateBoard();
                await Task.Delay(500); // Пауза в 500 мс между шагами для наглядности
            }
        }

        private void CellButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            int x = button.Location.X / 60; // Количество пикселей между кнопками
            int y = (button.Location.Y - 40) / 60; // Учитываем высоту заголовка окна

            if (forbiddenCells[x, y])
            {
                forbiddenCells[x, y] = false; // Делаем клетку доступной
                button.BackColor = (x + y) % 2 == 0 ? lightColor : darkColor;
            }
            else
            {
                // Запретить клетку
                forbiddenCells[x, y] = true;
                button.BackColor = Color.Red; // Измените цвет, чтобы выделить запрещенную клетку
            }


        }
        private void BtnIncreaseSize_Click(object sender, EventArgs e)
        {
            if (currentBoardSize < MaxBoardSize)
            {
                currentBoardSize++;
                InitializeBoard(); // Переинициализируем доску
            }
        }
        private void BtnDecreaseSize_Click(object sender, EventArgs e)
        {
            if (currentBoardSize > MinBoardSize)
            {
                currentBoardSize--;
                InitializeBoard(); // Переинициализируем доску
            }
        }

        private void UpdateBoard()
        {
            for (int i = 0; i < currentBoardSize; i++)
            {
                for (int j = 0; j < currentBoardSize; j++)
                {
                    if (forbiddenCells[i, j])
                        buttons[i, j].BackColor = Color.Red;
                    else
                        buttons[i, j].BackColor = (i + j) % 2 == 0 ? lightColor : darkColor;
                    buttons[i, j].Text = "";
                    buttons[i, j].Enabled = true;
                }
            }

            foreach (var knight in knights)
            {
                if (knight.X < currentBoardSize && knight.Y < currentBoardSize)
                {
                    buttons[knight.X, knight.Y].Text = "♞";
                    buttons[knight.X, knight.Y].ForeColor = knight.Color;
                    buttons[knight.X, knight.Y].Font = new Font(buttons[knight.X, knight.Y].Font.FontFamily, 30);
                    buttons[knight.X, knight.Y].Enabled = true;
                }
            }

        }
        private string GetForbiddenCellsInfo()
        {
            List<string> forbiddenPositions = new List<string>();

            for (int i = 0; i < currentBoardSize; i++)
            {
                for (int j = 0; j < currentBoardSize; j++)
                {
                    if (forbiddenCells[i, j])
                    {
                        forbiddenPositions.Add($"({i},{j})");
                    }
                }
            }

            return forbiddenPositions.Count > 0 ? $"Запрещённые клетки: {string.Join(", ", forbiddenPositions)}" : "Нет запрещённых клеток.";
        }

        private string ToChessNotation(int x, int y)
        {
            char file = (char)('a' + x); // Преобразуем индекс x в символ (a, b, c...)
            int rank = currentBoardSize - y; // Преобразуем индекс y в номер (1, 2, 3...)
            return $"{file}{rank}";
        }

private async void BFS()
{
    var stopwatch = new Stopwatch();
    stopwatch.Start();

    int iterationCount = 0;

    var queue = new Queue<State>();
    var visited = new HashSet<string>();
    var paths = new Dictionary<State, List<State>>();

    var initialState = new State(CloneKnights(knights));
    queue.Enqueue(initialState);
    visited.Add(initialState.ToString());
    paths[initialState] = new List<State> { initialState }; // Инициализируем путь

    int maxNodes = 0;

    while (queue.Count > 0)
    {
        iterationCount++;
        State currentState = queue.Dequeue();
        if (queue.Count > maxNodes)
            maxNodes = queue.Count;


        if (IsGoalState(currentState))
        {
            var fullPath = paths[currentState]; // Получаем полный путь
            string pathDescription = CreatePathDescription(fullPath);
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;
            string forbidden = GetForbiddenCellsInfo();
            MessageBox.Show($"Решение найдено!\n" +
                            $"Длина решения: {fullPath.Count-1}\n" +
                            $"Запрещённые поля: {forbidden}\n" +
                            $"Общее количество итераций: {iterationCount}\n" +
                            $"Время поиска: {elapsed.TotalSeconds} с\n" +
                            $"Ходы:\n{pathDescription}");
            await AnimatePath(fullPath, initialState); // Передаем тот же начальный путь
            return;
        }

        foreach (var nextState in GenerateNextStates(currentState))
        {
            if (!visited.Contains(nextState.ToString()))
            {
                queue.Enqueue(nextState);
                visited.Add(nextState.ToString());
                
                // Проверяем и добавляем в путь
                if (!paths.ContainsKey(nextState))
                {
                    paths[nextState] = new List<State>(paths[currentState]); // Копируем путь
                    paths[nextState].Add(nextState); // Добавляем текущее состояние
                }
            }
        }
    }

    stopwatch.Stop();
    await ShowNoSolutionMessage(iterationCount, stopwatch);
}

        private string CreatePathDescription(List<State> path)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < path.Count; i++) // Начинаем с 1, чтобы получить движение
            {
                var prevState = path[i - 1];
                var currentState = path[i];
                for (int j = 0; j < currentState.knights.Count; j++)
                {
                    var originalKnight = prevState.knights[j];
                    var knightMove = currentState.knights[j];
                    if (originalKnight.X != knightMove.X || originalKnight.Y != knightMove.Y)
                    {
                        string knightColor = originalKnight.Color == blackColor ? "Черный" : "Белый";
                        string moveDescription = $"{knightColor} конь {ToChessNotation(originalKnight.X, originalKnight.Y)} -> {ToChessNotation(knightMove.X, knightMove.Y)}";
                        sb.AppendLine(moveDescription);
                    }
                }
            }
            return sb.ToString();
        }

        private async void BBFS()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int iterationCount = 0;
            var queueForward = new Queue<State>();
            var queueBackward = new Queue<State>();
            var visitedForward = new HashSet<string>();
            var visitedBackward = new HashSet<string>();
            var pathsForward = new Dictionary<State, List<State>>();
            var pathsBackward = new Dictionary<State, List<State>>();
            int maxNodes = 0;

            var initialState = new State(CloneKnights(knights));
            var goalState = new State(new Knight[]
            {
                new Knight(currentBoardSize - 1, currentBoardSize - 1, blackColor),
                new Knight(currentBoardSize - 1, 0, blackColor),
                new Knight(0, currentBoardSize - 1, whiteColor),
                new Knight(0, 0, whiteColor)
            });

            queueForward.Enqueue(initialState);
            visitedForward.Add(initialState.ToString());
            pathsForward[initialState] = new List<State> { initialState }; // Инициализация пути для прямого поиска

            queueBackward.Enqueue(goalState);
            visitedBackward.Add(goalState.ToString());
            pathsBackward[goalState] = new List<State> { goalState }; // Инициализация пути для обратного поиска

            while (queueForward.Count > 0 && queueBackward.Count > 0)
            {
                // Шаг вперед
                iterationCount++;
                State currentStateForward = queueForward.Dequeue();
                if (queueForward.Count > maxNodes)
                    maxNodes = queueForward.Count;

                if (visitedBackward.Contains(currentStateForward.ToString()))
                {
                    // Создание полного пути
                    List<State> fullPath = new List<State>(pathsForward[currentStateForward]);

                    // Если встречная точка в обратном поиске
                    var meetingPoint = currentStateForward;
                    var backwardPath = pathsBackward.First(pair => pair.Key.ToString() == meetingPoint.ToString()).Value;
                    backwardPath.Reverse(); // Перевернуть назад
                    fullPath.AddRange(backwardPath.Skip(1)); // Соединяем пути, пропуская первую встречную точку

                    await ShowResult(fullPath, iterationCount, stopwatch, maxNodes);
                    //await AnimatePath(fullPath, initialState); // Запуск анимации
                    return;
                }

                foreach (var nextState in GenerateNextStates(currentStateForward))
                {
                    if (!visitedForward.Contains(nextState.ToString()))
                    {
                        queueForward.Enqueue(nextState);
                        visitedForward.Add(nextState.ToString());

                        if (!pathsForward.ContainsKey(nextState))
                        {
                            // Копируем путь
                            pathsForward[nextState] = new List<State>(pathsForward[currentStateForward]) { nextState };
                        }
                    }
                }

                // Шаг назад
                iterationCount++;
                State currentStateBackward = queueBackward.Dequeue();
                if (queueBackward.Count > maxNodes)
                {
                    maxNodes = queueBackward.Count;
                }
                if (visitedForward.Contains(currentStateBackward.ToString()))
                {
                    // Создание полного пути
                    List<State> fullPath = new List<State>(pathsBackward[currentStateBackward]);

                    // Если встречная точка в прямом поиске
                    var meetingPoint = currentStateBackward;
                    var forwardPath = pathsForward.First(pair => pair.Key.ToString() == meetingPoint.ToString()).Value;
                    fullPath.AddRange(forwardPath.Skip(1)); // Соединяем пути, пропуская первую встречную точку

                    await ShowResult(fullPath, iterationCount, stopwatch, maxNodes);
                    //await AnimatePath(fullPath, new State(CloneKnights(knights))); // Запуск анимации
                    return;
                }

                foreach (var nextState in GenerateNextStates(currentStateBackward))
                {
                    if (!visitedBackward.Contains(nextState.ToString()))
                    {
                        queueBackward.Enqueue(nextState);
                        visitedBackward.Add(nextState.ToString());

                        if (!pathsBackward.ContainsKey(nextState))
                        {
                            pathsBackward[nextState] = new List<State>(pathsBackward[currentStateBackward]) { nextState }; // Копируем путь
                        }
                    }
                }
            }

            stopwatch.Stop();
            await ShowNoSolutionMessage(iterationCount, stopwatch);
        }

        private async Task ShowResult(List<State> fullPath, int iterationCount, Stopwatch stopwatch, int maxNodes)
        {
            string pathDescription = CreatePathDescription(fullPath);
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;
            string forbidden = GetForbiddenCellsInfo();
            MessageBox.Show($"Решение найдено!\n" +
                            $"Длина решения: {fullPath.Count - 1}\n" +
                            $"Запрещённые поля: {forbidden}\n" +
                            $"Кол-во узлов: {maxNodes}\n" +
                            $"Общее количество итераций: {iterationCount}\n" +
                            $"Время поиска: {elapsed.TotalSeconds} с\n" +
                            $"Ходы:\n{pathDescription}");
        }

        private async Task ShowNoSolutionMessage(int iterationCount, Stopwatch stopwatch)
        {
            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;
            MessageBox.Show($"Решение не найдено.\nОбщее количество итераций: {iterationCount}\nВремя поиска: {elapsedTime.TotalSeconds} с");
        }

        private async void DFS()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int iterationCount = 0;

            var stack = new Stack<State>();
            var visited = new HashSet<string>();
            var paths = new Dictionary<State, List<State>>();

            var initialState = new State(CloneKnights(knights));
            paths[initialState] = new List<State>();
            stack.Push(initialState);

            int maxNodes = 0;


            while (stack.Count > 0)
            {
                iterationCount++;
                State currentState = stack.Pop();

                if (stack.Count > maxNodes)
                    maxNodes = stack.Count;

                if (IsGoalState(currentState))
                {
                    var fullPath = paths[currentState].ToList();
                    string pathDescription = CreatePathDescription(fullPath);
                    stopwatch.Stop();
                    TimeSpan elapsed = stopwatch.Elapsed;
                    string forbidden = GetForbiddenCellsInfo();
                    using (StreamWriter sw = new StreamWriter("log.txt"))
                    {
                        foreach (var step in fullPath)
                        {
                            sw.WriteLine(step);
                        }
                    }
                    MessageBox.Show($"Решение найдено!\n" +
                                    $"Длина решения: {fullPath.Count}\n" +
                                    $"Запрещённые поля: {forbidden}\n" +
                                    $"Общее количество итераций: {iterationCount}\n" +
                                    $"Максимальное количество узлов: {maxNodes}\n" +
                                    $"Время поиска: {elapsed.TotalSeconds} с\n");
                    //await AnimatePath(path, initialState); // Запуск анимации
                    return;
                }

                visited.Add(currentState.ToString());

                foreach (var nextState in GenerateNextStates(currentState))
                {
                    string nextStateString = nextState.ToString();
                    if (!visited.Contains(nextStateString) && !stack.Contains(nextState))
                    {
                        stack.Push(nextState);

                        if (!paths.ContainsKey(nextState))
                        {
                            paths[nextState] = new List<State>(paths[currentState]) { nextState };
                        }
                        //visited.Add(nextStateString);
                    }
                }
            }

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            MessageBox.Show($"Решение не найдено.\nОбщее количество операций: {iterationCount}\nВремя поиска: {elapsedTime.TotalSeconds} с");
            //ClearResources(stack, visited, paths, paths2);
        }

        private async void IterativeDFS(int initialDepth, int step)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int iterationCount = 0;
            int maxNodes = 0;

            while (true)
            {
                var stack = new Stack<(State state, int depth)>();
                var visited = new HashSet<string>();
                var paths = new Dictionary<string, List<State>>();

                var initialState = new State(CloneKnights(knights));
                string initialKey = initialState.ToString();
                paths[initialKey] = new List<State> { initialState };
                stack.Push((initialState, 0));

                while (stack.Count > 0)
                {
                    iterationCount++;
                    var (currentState, currentDepth) = stack.Pop();
                    string currentKey = currentState.ToString();

                    // Обновляем максимальное количество узлов в стеке
                    if (stack.Count > maxNodes) maxNodes = stack.Count;

                    // Проверка на целевое состояние
                    if (IsGoalState(currentState))
                    {
                        var fullPath = paths[currentKey].ToList();
                        string pathDescription = CreatePathDescription(fullPath);
                        stopwatch.Stop();
                        TimeSpan elapsed = stopwatch.Elapsed;
                        string forbidden = GetForbiddenCellsInfo();

                        MessageBox.Show($"Решение найдено!\n" +
                                        $"Длина решения: {fullPath.Count}\n" +
                                        $"Запрещённые поля: {forbidden}\n" +
                                        $"Общее количество итераций: {iterationCount}\n" +
                                        $"Максимальное количество узлов: {maxNodes}\n" +
                                        $"Время поиска: {elapsed.TotalSeconds} с\n");
                        return;
                    }

                    // Проверка текущей глубины
                    if (currentDepth < initialDepth)
                    {
                        // Генерируем все возможные следующие состояния
                        var nextStates = GenerateNextStates(currentState);

                        foreach (var nextState in nextStates)
                        {
                            string nextKey = nextState.ToString();

                            // Проверяем, чтобы избежать запрещенных состояний
                            if (!nextState.knights.Any(k => forbiddenCells[k.X, k.Y]))
                            {
                                // Добавляем в посещенные
                                if (!visited.Contains(nextKey) && !stack.Contains((nextState, currentDepth + 1)))
                                {
                                    stack.Push((nextState, currentDepth + 1)); // Добавляем в стек

                                    // Заполнение пути для newState
                                    if (!paths.ContainsKey(nextKey))
                                        paths[nextKey] = new List<State>(paths[currentKey]) { nextState }; // Заполняем путь
                                }
                            }
                        }
                    }
                    visited.Add(currentKey);
                }

                initialDepth += step; // Увеличиваем максимальную глубину для следующего цикла
            }

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            MessageBox.Show($"Решение не найдено.\nОбщее количество операций: {iterationCount}\nВремя поиска: {elapsedTime.TotalSeconds} с");
        }
        private bool IsGoalState(State state)
        {
            return (state.knights[0].X == currentBoardSize - 1 && state.knights[0].Y == currentBoardSize - 1 &&
                state.knights[1].X == currentBoardSize - 1 && state.knights[1].Y == 0 &&
                state.knights[2].X == 0 && state.knights[2].Y == currentBoardSize - 1 &&
                state.knights[3].X == 0 && state.knights[3].Y == 0);
        }

        private bool IsGoalStateBack(State state)
        {
            return (state.knights[0].X == 0 && state.knights[0].Y == 0 &&
                state.knights[1].X == 0 && state.knights[1].Y == currentBoardSize - 1 &&
                state.knights[2].X == currentBoardSize - 1 && state.knights[2].Y == 0 &&
                state.knights[3].X == currentBoardSize - 1 && state.knights[3].Y == currentBoardSize - 1);
        }


        private List<State> GenerateNextStates(State currentState)
        {
            List<State> nextStates = new List<State>();

            foreach (var knight in currentState.knights)
            {
                List<Knight> possibleMoves = knight.GetPossibleMoves(currentState.knights.ToArray(), forbiddenCells, currentBoardSize); // Передаем размер доски

                foreach (var move in possibleMoves)
                {
                    // Создаем новую копию состояния с обновленными позициями коней
                    Knight[] newKnights = CloneKnights(currentState.knights.ToArray());
                    newKnights[Array.IndexOf(currentState.knights.ToArray(), knight)] = move; // Перемещение коня

                    // Проверка на выход за пределы доски
                    if (IsInBounds(move.X, move.Y))
                    {
                        nextStates.Add(new State(newKnights, currentState)); // Добавление нового состояния
                    }
                }
            }

            return nextStates;
        }

        // Метод для проверки, находится ли позиция в пределах границ доски
        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < currentBoardSize && y >= 0 && y < currentBoardSize;
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            BFS();
        }

        public class Knight
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Color Color { get; }

            public Knight(int x, int y, Color color)
            {
                X = x;
                Y = y;
                Color = color;
            }

            public List<Knight> GetPossibleMoves(Knight[] knights, bool[,] forbiddenCells, int boardSize)
            {
                var moves = new (int dx, int dy)[]
                {
                    (2, 1), (2, -1), (-2, 1), (-2, -1),
                    (1, 2), (1, -2), (-1, 2), (-1, -2)
                };

                List<Knight> possibleMoves = new List<Knight>();

                foreach (var (dx, dy) in moves)
                {
                    int newX = X + dx;
                    int newY = Y + dy;

                    if (IsInBounds(newX, newY, boardSize) && !IsOccupied(newX, newY, knights) && !forbiddenCells[newX, newY])
                    {
                        possibleMoves.Add(new Knight(newX, newY, Color));
                    }
                }

                return possibleMoves;
            }


            private bool IsInBounds(int x, int y, int boardSize) // Передаем размер доски
            {
                return x >= 0 && x < boardSize && y >= 0 && y < boardSize;
            }

            private bool IsOccupied(int x, int y, Knight[] knights)
            {
                return knights.Any(k => k.X == x && k.Y == y);
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                Knight k = (Knight)obj;
                return X == k.X && Y == k.Y && Color == k.Color;
            }
        }

        public class State
        {
            public List<Knight> knights;
            public State prv;

            public State(Knight[] _knights)
            {
                knights = new List<Knight>();
                foreach (var k in _knights)
                {
                    knights.Add(new Knight(k.X, k.Y, k.Color));
                }
            }

            public State(Knight[] _knights, State _prv) : this(_knights)
            {
                prv = _prv;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                var s = (State)obj;
                foreach (var k in s.knights)
                {
                    if (!knights.Contains(k))
                    {
                        return false;
                    }
                }
                return true;
            }

            public override string ToString()
            {
                return string.Join(",", knights.Select(k => $"{k.X},{k.Y}"));
            }

        }

        private void btnDFS_Click(object sender, EventArgs e)
        {
            DFS();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InitializeBoard();
        }

        private void UpDownDepth_ValueChanged(object sender, EventArgs e)
        {
            Depth = (int)UpDownDepth.Value;
            //MessageBox.Show("Текущее значение глубины: " + Depth);
        }

        private void UpDownStep_ValueChanged(object sender, EventArgs e)
        {
            Step = (int)UpDownStep.Value;
            //MessageBox.Show("Текущее значение шага: " + Step);
        }

        private async void btnBBFS_Click(object sender, EventArgs e)
        {
            BBFS();
        }

        private void btnIDFS_Click(object sender, EventArgs e)
        {
            IterativeDFS(Depth, Step);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> pathh = new List<string>
        {
            "Черный конь (0;0) -> (1;2)",
            "Белый конь (2;0) -> (0;1)",
            "Белый конь (2;2) -> (1;0)",
            "Черный конь (0;2) -> (2;1)",

            "Черный конь (2;1) -> (0;0)",
            "Белый конь (0;1) -> (2;2)",
            "Белый конь (1;0) -> (0;2)",
            "Черный конь (1;2) -> (2;0)",

            "Черный конь (0;0) -> (1;2)",
            "Черный конь (2;0) -> (0;1)",
            "Белый конь (2;2) -> (1;0)",
            "Белый конь (0;2) -> (2;1)",

            "Белый конь (2;1) -> (0;0)",
            "Черный конь (0;1) -> (2;2)",
            "Белый конь (1;0) -> (0;2)",
            "Черный конь (1;2) -> (2;0)"
        };
            State initialState = new State(CloneKnights(knights));
            //AnimatePath(pathh, initialState);
        }

    }
}
