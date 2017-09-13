using ATP.MazeGenerators;
using ATP.Search;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ATPProject.View
{
    /// <summary>
    /// Interaction logic for PlayWindow.xaml
    /// </summary>
    public partial class PlayWindow : Window, IView
    {
        /// <summary>
        /// The name of the current maze displayed.
        /// </summary>
        private string currentmaze;

        /// <summary>
        /// The current floor displayed.
        /// </summary>
        private int currentfloor;

        /// <summary>
        /// The current maze displayed.
        /// </summary>
        private AMaze mazemaze;

        /// <summary>
        /// The current position in which the player is.
        /// </summary>
        private Position currentPosition;

        /// <summary>
        /// The solution of the current maze displayed.
        /// </summary>
        private Solution currentSolution;

        /// <summary>
        /// The size of each cell.
        /// </summary>
        private int cellSize;

        /// <summary>
        /// The view event.
        /// </summary>
        public event ViewEventDelegate viewChanged;

        /// <summary>
        /// The path of the images we use to display the character, end and the image appears when you get to the end.
        /// </summary>
        private string character, characterAtTheEnd, afterEnd;

        /// <summary>
        /// The location of a mouse before move (x,y) and after move (x1,y1).
        /// </summary>
        private double x, y, x1, y1;

        /// <summary>
        /// Boolean property which true when the mouse is pressed.
        /// </summary>
        private bool mousepressed;

        /// <summary>
        /// The constructor of the play window.
        /// </summary>
        /// <remarks>The current maze is null, the arrows which sasy if we can go pgup/pgdn are hidden and so are the buttons of
        /// display solution, solve and save (there is not maze we can save or solve). The default character is Ned Stark.</remarks>
        public PlayWindow()
        {
            InitializeComponent();
            currentmaze = "---";
            up.Visibility = Visibility.Hidden;
            down.Visibility = Visibility.Hidden;
            character = "\\nedhead.png";
            characterAtTheEnd = "\\mavochbli.png";
            afterEnd = "\\mavoch.png";
            displaysolution.IsEnabled = false;
            solve.IsEnabled = false;
            save.IsEnabled = false;
        }

        /// <summary>
        /// Click on the new button - generates maze.
        /// </summary>
        /// <remarks>Send to generate_click.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_Click(object sender, RoutedEventArgs e)
        {
            generate_Click(sender, e);
        }

        /// <summary>
        /// Click on save - save the current maze.
        /// </summary>
        /// <remarks>If no maze displayed - notify the user (will not happen because the button is disabled).
        /// Else determine that the extension of the file is .maze and the file name will be the name of the maze we want to save.
        /// open the save dialog and after clicking 'save' send a viewevent to the presenter.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (this.maze_cnvs.Children.Count <= 0)
                Output("No maze displayed");
            else {
                SaveFileDialog saveDialog = new SaveFileDialog()
                {
                    FileName = currentmaze.ToLower(),
                    DefaultExt = ".maze",
                    Filter = "Maze File (.maze)|*.maze"
                };
                if (saveDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveDialog.FileName, currentmaze.ToLower());
                    viewChanged("save " + currentmaze.ToLower() + " " + saveDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Load a maze file.
        /// </summary>
        /// <remarks>open the open dialog and show only files with the extension .maze, get the file name (the name of the maze) and send viewevent to the presenter.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                DefaultExt = ".maze",
                Filter = "Maze File (.maze)|*.maze"
            };
            if (openDialog.ShowDialog() == true)
            {
                string s = openDialog.FileName;
                int index = s.LastIndexOf("\\");
                string ans = s.Substring(index + 1);
                ans = ans.Substring(0, ans.Length - 5);
                currentmaze = ans.ToLower();

                viewChanged("load " + ans.ToLower() + " " + openDialog.FileName);
            }

        }

        /// <summary>
        /// Click on generate button - genereates a maze.
        /// </summary>
        /// <remarks>open the 'generate' window and get all the properties needed to generate maze - maze name' rows, columns, floors
        /// and send a viewevent to the presenter.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generate_Click(object sender, RoutedEventArgs e)
        {
            Generate gen = new Generate();
            gen.Tname.Focus();
            gen.ShowDialog();
            if (gen.cangenerate)
            {
                string s = "generate " + gen.m_mazename.ToLower() + " " + gen.m_rows + " " + gen.m_columns + " " + gen.m_floors;
                this.viewChanged(s);
            }
        }

        /// <summary>
        /// Start the running of the program.
        /// </summary>
        /// <remarks>Show the PlayWindow.</remarks>
        public void Start()
        {
            Show();
        }

        /// <summary>
        /// Set the current maze to be 'mazename'.
        /// </summary>
        /// <param name="mazename">The name of the current maze.</param>
        public void CurrentMaze(string mazename)
        {
            currentmaze = mazename.ToLower();
        }

        /// <summary>
        /// Click on the button 'display solution' - displays solution
        /// </summary>
        /// <remarks>Set the current floor to be 1 (we show the solution from the start of the maze), send viewevent to the presenter and disable
        /// the button of display solutin - solution will be displayed so the button is not neccesary.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displaysol_Click(object sender, RoutedEventArgs e)
        {
            currentfloor = 1;
            viewChanged("displaysolution " + currentmaze.ToLower());
            displaysolution.IsEnabled = false;

        }

        /// <summary>
        /// Displaying a solution.
        /// </summary>
        /// <remarks>Set the current maze (mazemaze) to be the maze the method got, the current position will be null - we dont want to let the user 
        /// move around the maze, and the current solution is the solution the method got.
        /// We set the cellSize Proportionally to the maze_cnvs size. Display in 'rcurrentfloorw' the current floor.
        /// For each step of the solution - set the background of the proper cell of 'maze', make the end point visible and mark it with the picture of the character. </remarks>
        /// <param name="maze">The maze for which solution we show.</param>
        /// <param name="solution">The solution we want to display.</param>
        public void DisplaySolution(AMaze maze, Solution solution)
        {
            mazemaze = maze;
            currentPosition = null;
            currentSolution = solution;
            cellSize = (int)maze_cnvs.ActualHeight / (int)maze.sizes[0];
            if ((double)(cellSize * (int)maze.sizes[1]) > maze_cnvs.ActualWidth)
                cellSize = (int)maze_cnvs.ActualWidth / (int)maze.sizes[1];
            MazeBoard mb = new MazeBoard(maze, cellSize);
            rcurrentfloorw.Text = currentfloor + " (out of " + mb.Floors + ")";
            int i = currentfloor;
            for (int j = 0; j < (int)maze.sizes[1]; j++)
            {
                for (int k = 0; k < (int)maze.sizes[0]; k++)
                {
                    Canvas.SetLeft(mb.m_cells[i, k, j], cellSize * j);
                    Canvas.SetTop(mb.m_cells[i, k, j], cellSize * k);
                }
            }
            this.maze_cnvs.Children.Clear();
            this.maze_cnvs.Children.Add(mb);
            foreach (Astate ms in solution.getsolpath())
            {
                Position p = (ms as MazeState).currentp;
                if (p.z == currentfloor)
                    mb[p.z, p.x, p.y].Background = Brushes.IndianRed;
            }
            mb[maze.end.z - 1, maze.end.x, maze.end.y].Visibility = Visibility.Visible;
            mb[maze.end.z - 1, maze.end.x, maze.end.y].image.Source = new BitmapImage(new Uri(string.Concat(Directory.GetCurrentDirectory(), afterEnd)));
            rcurrentmazew.Text = currentmaze.ToLower();
        }

        /// <summary>
        /// Display an output in MessageBox.
        /// </summary>
        /// <param name="output">The string we like to display on the screen.</param>
        public void Output(string output)
        {
            MessageBox.Show(output);
        }

        /// <summary>
        /// Click on display maze - Displaying a maze.
        /// </summary>
        /// <remarks>Show the Window of maze name, if the string from the window is not empty - the current maze will be the mazename we got
        /// and send a viewevent to the presenter.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displaymaze_Click(object sender, RoutedEventArgs e)
        {
            MazeName window = new MazeName();
            window.Mazename.Focus();
            window.ShowDialog();
            if (window.m_mazename != null)
            {
                currentmaze = window.m_mazename.ToLower();
                currentSolution = null;
                viewChanged("displaymaze " + window.m_mazename.ToLower());
            }
        }

        /// <summary>
        /// Display a maze.
        /// </summary>
        /// <remarks>if maze is not null - enable solve and save and disable display solution - if solution exist - disable solve and enable display solution.
        /// if isfirst is true, current floor will be 1 (dispalying the first floor). create a new maze board and add it to the maze_cnvs, set the start and end position.</remarks>
        /// <param name="maze">The maze we want to display.</param>
        /// <param name="isFirst">True if we display the first floor.</param>
        /// <param name="solution">The solution for maze</param>
        public void DisplayMaze(AMaze maze, bool isFirst, Solution solution)
        {
            if (maze != null){
                solve.IsEnabled = true;
                save.IsEnabled = true;
                displaysolution.IsEnabled = false;
                if (solution != null){
                    solve.IsEnabled = false;
                    displaysolution.IsEnabled = true;
                    currentSolution = solution;
                }}
            if (isFirst){
                currentPosition = maze.start;
                currentfloor = 1;
                up.Visibility = Visibility.Hidden;
                down.Visibility = Visibility.Hidden;
            }
            cellSize = (int)maze_cnvs.ActualHeight / (int)maze.sizes[0];
            if ((double)(cellSize * (int)maze.sizes[1]) > maze_cnvs.ActualWidth)
                cellSize = (int)maze_cnvs.ActualWidth / (int)maze.sizes[1];
            MazeBoard mb = new MazeBoard(maze, cellSize);
            rcurrentfloorw.Text = currentfloor + " (out of " + mb.Floors + ")";
            int i = currentfloor;
            for (int j = 0; j < (int)maze.sizes[1]; j++){
                for (int k = 0; k < (int)maze.sizes[0]; k++){
                    Canvas.SetLeft(mb.m_cells[i, k, j], cellSize * j);
                    Canvas.SetTop(mb.m_cells[i, k, j], cellSize * k);
                }}
            this.maze_cnvs.Children.Clear();
            this.maze_cnvs.Children.Add(mb);
            Canvas.SetLeft(mb, 0);
            Canvas.SetTop(mb, 0);
            mb[currentPosition.z, currentPosition.x, currentPosition.y].image.Source = new BitmapImage(new Uri(string.Concat(Directory.GetCurrentDirectory(), character)));
            mb[maze.end.z - 1, maze.end.x, maze.end.y].image.Source = new BitmapImage(new Uri(string.Concat(Directory.GetCurrentDirectory(), characterAtTheEnd)));
            mazemaze = maze;
            rcurrentmazew.Text = currentmaze.ToLower();
        }

        /// <summary>
        /// Click on the solve button - solve the maze.
        /// </summary>
        /// <remarks>Send viewevent to the presenter, enable display solution button and disable solve button.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void solve_Click(object sender, RoutedEventArgs e)
        {
            viewChanged("solve " + currentmaze.ToLower());
            rcurrentmazew.Text = currentmaze.ToLower();
            displaysolution.IsEnabled = true;
            solve.IsEnabled = false;
        }

        /// <summary>
        /// Click on help button - Display the inctructions of the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Output("⚔ Click Generate Maze to add a maze to the system. \n" +
                "⚔ In options you can choose with which character you want to play - ned, arya or hodor \n" +
                "⚔ You can try to solve the maze by yourself using arrow keys, you can go up/down a floor using pgup/pgdn - when it is possible an arrow ⇧/⇩ appears, \n" +
                "⚔ The maze starts at floor 1 and ends at the top floor and marked with a picture. \n" +
                "⚔ You can click Solve Maze to let the system solve it for you. \n" +
                "⚔ You can display a solution by clickling Display Solution (if one exists) \n"
                + "⚔ All mazes and solutions created are automatically saved when closing the app \n" +
                "⚔ You can save a single displayed maze by clicking Save Maze and load him later using Load Maze \n" +
                "⚔ Use Ctrl + Mouse wheel to zoom in \\ out \n⚔ You can also use the mouse to drag the character on the maze board");

        }

        /// <summary>
        /// Exit the program.
        /// </summary>
        /// <remarks>send viewevent to the presenter for proper exit and closes the window.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            viewChanged("exit");
            Close();
        }

        /// <summary>
        /// Move the character.
        /// </summary>
        /// <remarks>get string of direction and positon (current position) if the string is left/right/up/down - check if the move is not out of bounds
        /// and send to direction with the maze board, the position we want to be in next and the current position. if we got floorup/floordown - 
        /// check if the move is not out of bounds, send to UpOrDown with the maze board, the position we want to be in next and the current position.</remarks>
        /// <param name="dir">The direction of the move.</param>
        /// <param name="p">The current position.</param>
        private void move(string dir, Position p)
        {
            if (p == null)
                return;
            int x = p.x, y = p.y, z = p.z;
            MazeBoard mb = (MazeBoard)this.maze_cnvs.Children[0];
            string s = dir;
            if (s != null){
                if (s.Equals("left")){
                    if (y <= 0)
                        return;
                    Direction(mb, new Position(x, y - 1, z), p);
                }
                if (s.Equals("right")){
                    if (y > mb.Columns - 1)
                        return;
                    Direction(mb, new Position(x, y + 1, z), p);
                }
                if (s.Equals("up")){
                    if (x <= 0)
                        return;
                    Direction(mb, new Position(x - 1, y, z), p);
                }
                if (s.Equals("down")){
                    if (x > mb.Columns - 1)
                        return;
                    Direction(mb, new Position(x + 1, y, z), p);
                }
                if (s.Equals("floorup")){
                    if (z >= mb.Floors)
                        return;
                    UpOrDown(mb, new Position(x, y, z + 1), p, "up");
                }
                if (s.Equals("floordown")){
                    if (z < 1)
                        return;
                    UpOrDown(mb, new Position(x, y, z - 1), p, "down");
                }
            }
        }

        /// <summary>
        /// Move the character.
        /// </summary>
        /// <remarks>If it is possible to move to the newp (it is not wall or it is the end), the character in the current position is now null, and will appear
        /// in newp, the current position is now newp. if it is possible to go up or down a floor, make the arrows visible, if we got to the end of 
        /// the maze, notify the user.</remarks>
        /// <param name="mb">The maze board.</param>
        /// <param name="newp">The position after move.</param>
        /// <param name="oldp">Current position</param>
        private void Direction(MazeBoard mb, Position newp, Position oldp)
        {

            if (mb[newp.z, newp.x, newp.y].image.Source == null || (newp.x == mazemaze.end.x && newp.y == mazemaze.end.y && newp.z + 1 == mazemaze.end.z))
            {
                up.Visibility = Visibility.Hidden;
                down.Visibility = Visibility.Hidden;
                mb[oldp.z, oldp.x, oldp.y].image.Source = null;
                mb[newp.z, newp.x, newp.y].image.Source = new BitmapImage(new Uri(string.Concat(Directory.GetCurrentDirectory(), character)));
                currentPosition = new Position(newp.x, newp.y, newp.z);
                if (currentPosition.z == mazemaze.end.z - 1 && currentPosition.x == mazemaze.end.x && currentPosition.y == mazemaze.end.y)
                {
                    mb[mazemaze.end.z - 1, mazemaze.end.x, mazemaze.end.y].image.Source = new BitmapImage(new Uri(string.Concat(Directory.GetCurrentDirectory(), afterEnd)));
                    Output("hooray! \nyou live to see another day");
                }
                if (newp.z < mb.Floors)
                    if (mb[newp.z + 1, newp.x, newp.y].image.Source == null || (newp.x == mazemaze.end.x && newp.y == mazemaze.end.y && newp.z + 1 == mazemaze.end.z - 1))
                        up.Visibility = Visibility.Visible;
                if (newp.z > 1)
                    if (mb[newp.z - 1, newp.x, newp.y].image.Source == null)
                        down.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Move to Character.
        /// </summary>
        /// <remarks>
        /// If it is possible to move to the newp (it is not wall or it is the end), the character in the current position is now null, and will appear
        /// in newp, the current position is now newp. if it is possible to go up or down a floor, make the arrows visible, if we got to the end of 
        /// the maze, notify the user . update the current floor and display the new floor.</remarks>
        /// <param name="mb"The maze board.</param>
        /// <param name="newp">The current position</param>
        /// <param name="oldp">The position after the move</param>
        /// <param name="upordown">If the direction is up or down</param>
        private void UpOrDown(MazeBoard mb, Position newp, Position oldp, string upordown)
        {
            if (mb[newp.z, newp.x, newp.y].image.Source == null || (newp.x == mazemaze.end.x && newp.y == mazemaze.end.y && newp.z + 1 == mazemaze.end.z))
            {
                up.Visibility = Visibility.Hidden;
                down.Visibility = Visibility.Hidden;
                if (upordown.Equals("up"))
                    currentfloor++;
                else
                    currentfloor--;
                rcurrentfloorw.Text = currentfloor + " (out of " + mb.Floors + ")";
                currentPosition = newp;
                DisplayMaze(mazemaze, false, currentSolution);
                mb[oldp.z, oldp.x, oldp.y].image.Source = null;
                if (currentPosition.z == mazemaze.end.z - 1 && currentPosition.x == mazemaze.end.x && currentPosition.y == mazemaze.end.y)
                {
                    mb[mazemaze.end.z - 1, mazemaze.end.x, mazemaze.end.y].image.Source = new BitmapImage(new Uri(string.Concat(Directory.GetCurrentDirectory(), afterEnd)));
                    Output("hooray! \nyou live to see another day");
                }
                if (newp.z < mb.Floors)
                    if (mb[newp.z + 1, newp.x, newp.y].image.Source == null || (newp.x == mazemaze.end.x && newp.y == mazemaze.end.y && newp.z + 1 == mazemaze.end.z - 1))
                        up.Visibility = Visibility.Visible;
                if (newp.z > 1)
                    if (mb[newp.z - 1, newp.x, newp.y].image.Source == null)
                        down.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Handles with key down.
        /// </summary>
        /// <remarks>If the maze_cnvs has a child - if pressed left - send to move with left and current position, if pressed right - send to move with right and current position,
        /// if pressed up - send to move with up and current position, if pressed down - send to move with down and current position, if pressed pgup - 
        /// check if possible to move up a floor, change the currentfloor, send to move with floorup and current position,
        /// if pressed pgdn - check if possible to move down a floor, change the currentfloor, send to move with floordown and current position.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
            if (this.maze_cnvs.Children.Count > 0){
                switch (key){
                    case Key.Left:
                            move("left", currentPosition);
                            break;
                    case Key.Right:
                            move("right", currentPosition);
                            break;
                    case Key.Up:
                            move("up", currentPosition);
                            break;
                    case Key.Down:
                            move("down", currentPosition);
                            break;
                    case Key.PageUp:
                            if (currentPosition != null)
                                move("floorup", currentPosition);
                            else {
                                if (currentfloor >= (int)mazemaze.sizes[2])
                                    break;
                                currentfloor++;
                                viewChanged("displaysolution " + currentmaze.ToLower());
                            }
                            break;
                    case Key.PageDown:
                            if (currentPosition != null)
                                move("floordown", currentPosition);
                            else {
                                if (currentfloor <= 1)
                                    break;
                                currentfloor--;
                                viewChanged("displaysolution " + currentmaze.ToLower());
                            }
                            break;
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles with the resize of a window.
        /// </summary>
        /// <remarks>Change the cellsize Proportionally to the maze_cnvs, and send to display with the new cell size - if solution is null - display maze, else - display solution.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.maze_cnvs.Children.Count > 0){
                MazeBoard mb = maze_cnvs.Children[0] as MazeBoard;
                if (cellSize * (double)mb.Rows > this.maze_cnvs.ActualHeight){
                    cellSize = ((int)maze_cnvs.ActualHeight / mb.Rows);
                    if (currentPosition == null)
                        DisplaySolution(mazemaze, currentSolution);
                    else
                        DisplayMaze(mazemaze, false, currentSolution);
                }
                if (cellSize * (double)mb.Columns > this.maze_cnvs.ActualWidth){
                    cellSize = ((int)maze_cnvs.ActualWidth / mb.Columns);
                    if (currentPosition == null)
                        DisplaySolution(mazemaze, currentSolution);
                    else
                        DisplayMaze(mazemaze, false, currentSolution);
                }
                if (cellSize * (double)mb.Rows < this.maze_cnvs.ActualHeight){
                    if (currentPosition == null)
                        DisplaySolution(mazemaze, currentSolution);
                    else
                        DisplayMaze(mazemaze, false, currentSolution);
                }
                if (cellSize * (double)mb.Columns < this.maze_cnvs.ActualWidth){
                    cellSize = ((int)maze_cnvs.ActualWidth / mb.Columns);
                    if (currentPosition == null)
                        DisplaySolution(mazemaze, currentSolution);
                    else
                        DisplayMaze(mazemaze, false, currentSolution);
                }
            }
        }

        /// <summary>
        /// Click on the 'X' button - exit the program.
        /// </summary>
        /// <remarks>send viewevet to the presenter for proper exit and close the program.</remarks>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            viewChanged("exit");
            Close();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Click on the charater ned stark - change the paths of images to the proper path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ned_Click(object sender, RoutedEventArgs e)
        {
            character = "\\nedhead.png";
            characterAtTheEnd = "\\mavochbli.png";
            afterEnd = "\\mavoch.png";
            ChangeCharImage();
        }

        /// <summary>
        /// Click on the charater arya stark - change the paths of images to the proper path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void arya_Click(object sender, RoutedEventArgs e)
        {
            character = "\\aryahead.png";
            characterAtTheEnd = "\\aryaeyes.png";
            afterEnd = "\\aryaandeyes.png";
            ChangeCharImage();
        }

        /// <summary>
        /// Click on the charater Hodor - change the paths of images to the proper path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hodor_Click(object sender, RoutedEventArgs e)
        {
            character = "\\hodorhead.png";
            characterAtTheEnd = "\\hodordoor.jpg";
            afterEnd = "\\hodoranddoor.jpg";
            ChangeCharImage();
        }

        /// <summary>
        /// Change the image of the character.
        /// </summary>
        /// <remarks>Change the image of the character in the current positon and in the end of the maze.</remarks>
        private void ChangeCharImage()
        {
            if (maze_cnvs.Children.Count > 0)
            {
                MazeBoard mb = (MazeBoard)this.maze_cnvs.Children[0];
                if (currentPosition != null)
                    mb[currentPosition.z, currentPosition.x, currentPosition.y].image.Source = new BitmapImage(new Uri(string.Concat(Directory.GetCurrentDirectory(), character)));
                mb[mazemaze.end.z - 1, mazemaze.end.x, mazemaze.end.y].image.Source = new BitmapImage(new Uri(string.Concat(Directory.GetCurrentDirectory(), characterAtTheEnd)));
            }
        }
        const double ScaleRate = 1.1;

        /// <summary>
        /// Handles with the mouse wheel - zoom in or out.
        /// </summary>
        /// <remarks>If the ctrl is pressed - change the scalerate if zoom in or out.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Delta > 0)
                {
                    str.ScaleX *= ScaleRate;
                    str.ScaleY *= ScaleRate;
                }
                else
                {
                    str.ScaleX /= ScaleRate;
                    str.ScaleY /= ScaleRate;
                }
                e.Handled = true;
            }

        }

        /// <summary>
        /// Click ont the item 'properties' in the menu - dispaly the settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Output("Worker Thread: " + Settings.Default.workingThreads + "\n" +
                "Completion Threads: " + Settings.Default.completedThreads + "\n" +
                "Algorithm Type: " + Settings.Default.SolvingAlgorithm + "\n");
        }

        /// <summary>
        /// Click on about - shows the name of the programmers and the algorithm use to solve the maze.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            Output("⚔ programmers:  \n" +
                    "Dar Nettler \nCarmela Davidovsky \n" +
                "⚔ All maze floors are built using prim algorithm and can be solved using DFS/BFS algorithms \n");

        }
        
        /// <summary>
        /// Event Hendler when we clicked the mouse and hold him.
        /// </summary>
        /// <remarks>If the boolean mousepressed is false - get the current position of the mouse, and turn mouse pressed to true.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maze_cnvs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!mousepressed)
            {
                x = e.GetPosition(sender as MazeCell).X;
                y = e.GetPosition(sender as MazeCell).Y;
                mousepressed = true;
            }
        }

        /// <summary>
        /// Handles with the event of moving mouse.
        /// </summary>
        /// <remarks>if the mouse is prresed - get the positon of the mouse right now. if the difference between x1 and x is larger than the cell size - move to the right
        /// if the difference between x and x1 is larger than cell size - move left, if the difference between y1 and y is larger then cell size move up, 
        /// if the difference between y and y1 is larger then cell size move down. in each case set the x or y to be the current position of the mouse.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maze_cnvs_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousepressed)
            {
                MazeCell c = sender as MazeCell;
                x1 = e.GetPosition(c).X;
                y1 = e.GetPosition(c).Y;
                if (x1 - x > cellSize)
                {
                    move("right", currentPosition);
                    x = x1;
                }
                else if (x - x1 > cellSize)
                {
                    move("left", currentPosition);
                    x = x1;
                }
                if (y1 - y > cellSize)
                {
                    move("down", currentPosition);
                    y = y1;
                }
                else if (y - y1 > cellSize)
                {
                    move("up", currentPosition);
                    y = y1;
                }
            }
        }

        /// <summary>
        /// Handles the event of stop pressing a mouse.
        /// </summary>
        /// <remarks>Change mousepressed to false.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maze_cnvs_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mousepressed = false;
        }
    }
}
