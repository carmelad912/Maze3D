using ATP.MazeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ATPProject.View
{
    /// <summary>
    /// Interaction logic for MazeBoard.xaml
    /// </summary>
    public partial class MazeBoard : UserControl
    {
        /// <summary>
        /// The propert that holds a 3D array of maze cells - the maze board.
        /// </summary>
        public MazeCell[,,] m_cells;

        /// <summary>
        /// Holds the number of rows in the game board.
        /// </summary>
        private int rows;

        /// <summary>
        /// Holds the number of columns in the game board.
        /// </summary>
        private int column;

        /// <summary>
        /// Holds the number of floors in the game board.
        /// </summary>
        private int floors;

        /// <summary>
        /// Holds the size of a cell.
        /// </summary>
        private int size;

        /// <summary>
        /// getter for m_cells.
        /// </summary>
        public MazeCell this[int i, int j, int k]
        {
            get { return m_cells[i, j, k]; }
        }

        /// <summary>
        /// Getter for column.
        /// </summary>
        public int Columns
        {
            get { return column; }
            set { column = value; }
        }

        /// <summary>
        /// Getter for rows.
        /// </summary>
        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        /// <summary>
        /// Getter for floors.
        /// </summary>
        public int Floors
        {
            get { return floors; }
            set { floors = value; }
        }

        /// <summary>
        /// Getter for size.
        /// </summary>
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// Constructor of MazeBoard
        /// </summary>
        /// <param name="mazename">The name of the maze of the mazeboard.</param>
        /// <param name="mazeCellSize">The size of each cell</param>
        public MazeBoard(AMaze mazename, int mazeCellSize)
        {
            InitializeComponent();
            CreateMaze(mazename, mazeCellSize);
        }

        /// <summary>
        /// Creating a new maze.
        /// </summary>
        /// <remarks>initialize rows, column and floors - for each cell decide if he should be hidden (if the proper cell in the maze is 1 or 2) or
        /// visible otherwise.</remarks>
        /// <param name="mazename">The name of the maze</param>
        /// <param name="mazeCellSize">The size of each cell.</param>
        private void CreateMaze(AMaze mazename, int mazeCellSize)
        {
            size = mazeCellSize;
            Rows = (int)mazename.sizes[0];
            column = (int)mazename.sizes[1];
            floors = (int)mazename.sizes[2];
            m_cells = new MazeCell[(mazename as Maze3d).maze3d.Length, (int)mazename.sizes[0], (int)mazename.sizes[1]];
            for (int i = 0; i < (mazename as Maze3d).maze3d.Length; i++)
            {
                for (int j = 0; j < (int)mazename.sizes[1]; j++)
                {
                    for (int k = 0; k < (int)mazename.sizes[0]; k++)
                    {
                        if ((mazename as Maze3d).maze3d[i].maze2d[k, j] == 1 || (mazename as Maze3d).maze3d[i].maze2d[k, j] == 2)
                            m_cells[i, k, j] = new MazeCell(mazeCellSize, true);
                        else
                            m_cells[i, k, j] = new MazeCell(mazeCellSize, false);
                        mazeBoard.Children.Add(m_cells[i, k, j]);
                    }
                }
            }
        }
    }
}
