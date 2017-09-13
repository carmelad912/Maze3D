using ATP.MazeGenerators;
using ATP.Search;
using ATPProject.Model1;
using ATPProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATPProject.Presenter1
{
    /// <summary>
    /// The class that deals with the command 'load'.
    /// </summary>
    class CommandLoad : ACommand
    {
        /// <summary>
        /// The constructor of the class. we use the constructor of ACommand
        /// </summary>
        /// <param name="model">The model of the command</param>
        /// <param name="view">The view of the command</param>
        public CommandLoad(IModel model, IView view) : base(model, view) { }

        /// <summary>
        /// Executes the command of loading a maze.
        /// </summary>
        /// <remarks>Call m_model.LoadMaze with the name of the maze we like to load (parameters[1])
        /// and the path (parameters[2])</remarks>
        /// <param name="parameters">the command and parameters required to do the command received.</param>
        public override void DoCommand(params string[] parameters)
        {
            m_model.LoadMaze(parameters[1], parameters[2]);
        }

        /// <summary>
        /// Returns the name of the command, "load".
        /// </summary>
        /// <returns>The name of the command.</returns>
        public override string GetName()
        {
            return "load";
        }
    }

    /// <summary>
    /// The class that deals with the command 'save'.
    /// </summary>
    class CommandSave : ACommand
    {
        /// <summary>
        /// The constructor of the class. we use the constructor of ACommand
        /// </summary>
        /// <param name="model">The model of the command</param>
        /// <param name="view">The view of the command</param>
        public CommandSave(IModel model, IView view) : base(model, view) { }

        /// <summary>
        /// Executes the command of saving a maze.
        /// </summary>
        /// <remarks>Call m_model.SaveMaze with the name of the maze we like to load (parameters[1])
        /// and the path (parameters[2])</remarks>
        /// <param name="parameters">the command and parameters required to do the command received</param>
        public override void DoCommand(params string[] parameters)
        {
            m_model.SaveMaze(parameters[1], parameters[2]);
        }

        /// <summary>
        /// Returns the name of the command, "save".
        /// </summary>
        /// <returns>The name of the command.</returns>
        public override string GetName()
        {
            return "save";
        }
    }

    /// <summary>
    /// The class that deals with the command 'generate'.
    /// </summary>
    class CommandGen : ACommand
    {
        /// <summary>
        /// The constructor of the class. we use the constructor of ACommand
        /// </summary>
        /// <param name="model">The model of the command</param>
        /// <param name="view">The view of the command</param>
        public CommandGen(IModel model, IView view) : base(model, view) { }

        /// <summary>
        /// Executes the command of generating a maze.
        /// </summary>
        /// <remarks>convert parameters[2], parameters[3], parameters[4] to number of rows, columns, floors 
        /// then call to the method GenerateMaze of the model with the maze name (parameters[1]) and x, y, z.</remarks>
        /// <param name="parameters">the command and parameters required to do the command received</param>
        public override void DoCommand(params string[] parameters)
        {
            int x = Convert.ToInt32(parameters[2]);
            int y = Convert.ToInt32(parameters[3]);
            int z = Convert.ToInt32(parameters[4]);
            m_model.GenerateMaze(parameters[1], x, y, z);
        }

        /// <summary>
        /// Returns the name of the command, "generate".
        /// </summary>
        /// <returns>The name of the command.</returns>
        public override string GetName()
        {
            return "generate";
        }
    }

    /// <summary>
    /// The class that deals with the command 'solve'.
    /// </summary>
    class CommandSolve : ACommand
    {
        /// <summary>
        /// The constructor of the class. we use the constructor of ACommand
        /// </summary>
        /// <param name="model">The model of the command</param>
        /// <param name="view">The view of the command</param>
        public CommandSolve(IModel model, IView view) : base(model, view) { }

        /// <summary>
        /// Executes the command of solving a maze.
        /// </summary>
        /// <remarks>Call the method of solving a maze in the model with the maze name (parameters[1])</remarks>
        /// <param name="parameters">the command and parameters required to do the command received</param>
        public override void DoCommand(params string[] parameters)
        {
            m_model.SolveMaze(parameters[1]);
        }

        /// <summary>
        /// Returns the name of the command, "solve".
        /// </summary>
        /// <returns>The name of the command.</returns>
        public override string GetName()
        {
            return "solve";
        }
    }

    /// <summary>
    /// The class that deals with the command 'displaymaze'.
    /// </summary>
    class CommandDisplayMaze : ACommand
    {
        /// <summary>
        /// The constructor of the class. we use the constructor of ACommand
        /// </summary>
        /// <param name="model">The model of the command</param>
        /// <param name="view">The view of the command</param>
        public CommandDisplayMaze(IModel model, IView view) : base(model, view) { }

        /// <summary>
        /// Executes the command of displaying a maze.
        /// </summary>
        /// <remarks>Try to get the proper maze (parameters[1]) and solution. if the maze is null (does not exist) we exit the method
        /// else call the method of displaying a maze of the model with the maze, true (the first floor) and the solution. Change 
        /// the current maze to be maze (parameters[1]).</remarks>
        /// <param name="parameters">the command and parameters required to do the command received</param>
        public override void DoCommand(params string[] parameters)
        {

            AMaze maze = m_model.GetMaze(parameters[1]);
            Solution solution = m_model.GetSolution(parameters[1]);
            if (maze == null)
                return;
            m_view.DisplayMaze(maze,true,solution);
            m_view.CurrentMaze(parameters[1]);
        }

        /// <summary>
        /// Returns the name of the command, "displaymaze".
        /// </summary>
        /// <returns>The name of the command.</returns>
        public override string GetName()
        {
            return "displaymaze";
        }
    }

    /// <summary>
    /// The class that deals with the command 'displaysolution'.
    /// </summary>
    class CommandDisplaySolution : ACommand
    {
        /// <summary>
        /// The constructor of the class. we use the constructor of ACommand
        /// </summary>
        /// <param name="model">The model of the command</param>
        /// <param name="view">The view of the command</param>
        public CommandDisplaySolution(IModel model, IView view) : base(model, view) { }

        /// <summary>
        /// Executes the command of displaying a solution of maze.
        /// </summary>
        /// <remarks>Try to get the maze and solution from the model, if both not null -
        /// call the method of displaying a solution with the maze and solution.</remarks>
        /// <param name="parameters">the command and parameters required to do the command received</param>
        public override void DoCommand(params string[] parameters)
        {
            AMaze maze = m_model.GetMaze(parameters[1]);
            Solution solution = m_model.GetSolution(parameters[1]);
            if (maze != null && solution!= null)
                m_view.DisplaySolution(maze, solution);
        }

        /// <summary>
        /// Returns the name of the command, "displaysolution".
        /// </summary>
        /// <returns>The name of the command.</returns>
        public override string GetName()
        {
            return "displaysolution";
        }
    }

    /// <summary>
    /// The class that deals with the command 'exit'.
    /// </summary>
    class CommandExit : ACommand
    {
        /// <summary>
        /// The constructor of the class. we use the constructor of ACommand
        /// </summary>
        /// <param name="model">The model of the command</param>
        /// <param name="view">The view of the command</param>
        public CommandExit(IModel model, IView view) : base(model, view) { }

        /// <summary>
        /// Executes the command of exit the program.
        /// </summary>
        /// <remarks>Call m_model.stop.</remarks>
        /// <param name="parameters">the command and parameters required to do the command received.</param>
        public override void DoCommand(params string[] parameters)
        {
            (m_model as Model).Stop();
        }

        /// <summary>
        /// Returns the name of the command, "exit".
        /// </summary>
        /// <returns>The name of the command.</returns>
        public override string GetName()
        {
            return "exit";
        }
    }

    /// <summary>
    /// The class that deals with the command 'doesexist'.
    /// </summary>
    class CommandDoesExist : ACommand
    {
        /// <summary>
        /// The constructor of the class. we use the constructor of ACommand
        /// </summary>
        /// <param name="model">The model of the command</param>
        /// <param name="view">The view of the command</param>
        public CommandDoesExist(IModel model, IView view) : base(model, view) { }

        /// <summary>
        /// Executes the command of saying if maze exists.
        /// </summary>
        /// <remarks>Call m_model.GetMaze with the maze name (parameters[1]).</remarks>
        /// <param name="parameters">the command and parameters required to do the command received.</param>
        public override void DoCommand(params string[] parameters)
        {
            m_model.GetMaze(parameters[1]);
        }

        /// <summary>
        /// Returns the name of the command, "doesexist".
        /// </summary>
        /// <returns>The name of the command.</returns>
        public override string GetName()
        {
            return "doesexist";
        }
    }
}
