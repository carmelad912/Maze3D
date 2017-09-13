using ATP.MazeGenerators;
using ATP.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATPProject.Model1
{
    
    public delegate void ModelEventDelegate(string Command);

    /// <summary>
    /// IModel is the interface that "Model" will inherit.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// The event of the model.
        /// </summary>
        event ModelEventDelegate Event;

        /// <summary>
        /// Generate a maze with the given parameters. Model will implement the function.
        /// </summary>
        /// <param name="mazename">the name we will give to the maze</param>
        /// <param name="x">nummber of rows</param>
        /// <param name="y">number of columns</param>
        /// <param name="z">numer of floors</param>
        void GenerateMaze(string mazenam, int x, int y, int z);

        /// <summary>
        /// Solve a maze 'mazename'. Model will implement the function.
        /// </summary>
        /// <param name="mazename">The name of the maze we want to solve.</param>
        void SolveMaze(string mazename);

        /// <summary>
        /// Save a maze into a given path. Model will implement the function.
        /// </summary>
        /// <param name="mazename">the maze name that we want to save</param>
        /// <param name="path">the path we will save in the maze</param>
        void SaveMaze(string mazename, string path);

        /// <summary>
        /// Load a maze from a given path and name it 'mazename'.  Model will implement the function.
        /// </summary>
        /// <param name="mazename">the name we will give to the maze</param>
        /// <param name="path">the path where the maze saved</param>
        void LoadMaze(string mazename, string path);

        /// <summary>
        /// Load a dictionary from a given path.  Model will implement the function.
        /// </summary>
        /// <param name="path">the path where the maze saved</param>
        void LoadDictionaryFromDisk(string path);

        /// <summary>
        /// Save a dictionary to a given path.  Model will implement the function.
        /// </summary>
        /// <param name="path">the path where we want to save the maze.</param>
        void SaveDictionaryToDisk(string path);

        /// <summary>
        /// Print the solution of 'mazename'. Model will implement the function.
        /// </summary>
        /// <param name="mazename">The name of the maze we would like to display its solution.</param>
        string printsolution(string mazename);

        /// <summary>
        /// Get the maze 'mazename'. Model will implement the function.
        /// </summary>
        /// <param name="mazename">The maze we like to get.</param>
        /// <returns>The proper maze.</returns>
        AMaze GetMaze(string mazename);

        /// <summary>
        /// Get the solution for 'mazename'. Model will implement the function.
        /// </summary>
        /// <param name="mazename">The maze we like to get its solution.</param>
        /// <returns>The proper solution.</returns>
        Solution GetSolution(string mazename);

        /// <summary>
        /// Set the propperties of the our settings. Model will implement the function.
        /// </summary>
        /// <param name="numoft">Number of working threads.</param>
        /// <param name="numofct">Number of completing threads.</param>
        /// <param name="solvingalgorithm">The solving algorithm.</param>
        /// <param name="generatealgo">The generating algorithm.</param>
        void SetProp(int numoft, int numofct, string solvingalgorithm, string generatealgo); 
    }
}
