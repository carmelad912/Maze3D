using ATP.MazeGenerators;
using ATP.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATPProject.View
{

    public delegate void ViewEventDelegate(string Command);
    interface IView
    {
        event ViewEventDelegate viewChanged;

        /// <summary>
        /// Start the running of our program. PlayWindow will implement the function.
        /// </summary>
        void Start();
        
        /// <summary>
        /// Shows an output on the screen. PlayWindow will implement the function.
        /// </summary>
        /// <param name="output">The string we would like to show to the user.</param>
        void Output(string output);
        
        /// <summary>
        /// Display the saved maze 'maze'. PlayWindow will implement the function.
        /// </summary>
        /// <param name="maze">The maze we like to display</param>
        /// <param name="isFirst">Boolean parameter which says if we display the first floor.</param>
        /// <param name="solution">The solution for 'maze'</param>
        void DisplayMaze(AMaze maze, bool isFirst, Solution solution);

        /// <summary>
        /// Set the current maze to be the maze 'mazename'
        /// </summary>
        /// <param name="mazename">The maze we want to set as current.</param>
        void CurrentMaze(string mazename);
        
        /// <summary>
        /// Display solution for the maze 'mazename'. PlayWindow will implement the method.
        /// </summary>
        /// <param name="maze">The maze we would like to show his solution.</param>
        /// <param name="solution">The solution of 'maze'</param>
        void DisplaySolution(AMaze maze,Solution solution);
    }
}
