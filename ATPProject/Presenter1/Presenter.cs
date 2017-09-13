using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP.MazeGenerators;
using ATPProject.Model1;
using ATPProject.View;
using ATPProject.Presenter1;
using System.Collections;

namespace ATPProject.Presenter1
{

    /// <summary>
    /// The class that represents the presenter face in MVP
    /// </summary>
    class Presenter
    {
        /// <summary>
        /// The property that holds the model.
        /// </summary>
        private IModel m_model;

        /// <summary>
        /// The property that holds the view.
        /// </summary>
        private IView m_view;

        /// <summary>
        /// Holds the dictionary of all commands.
        /// </summary>
        private Dictionary<string, ACommand> m_commands;

        /// <summary>
        /// The constructor of the presenter.
        /// </summary>
        /// <remarks>Sets the model and the view, set the events and commands. define nuofthreads, solvingalgo, generatealgo, noofcomp to be
        /// the proper property from the settings file. send to the model to set the properties.</remarks>
        /// <param name="model">The model we get to set.</param>
        /// <param name="view">The view we get to set.</param>
        public Presenter(IModel model, IView view)
        {
            m_model = model;
            m_view = view;
            GetEvent();
            GetCommands();
            int nuofthreads = Settings.Default.workingThreads;
            string solvingalgo = Settings.Default.SolvingAlgorithm;
            string generatealgo = Settings.Default.Generate;
            int noofcomp = Settings.Default.completedThreads;
            m_model.SetProp(nuofthreads, noofcomp, solvingalgo, generatealgo);
        }

        /// <summary>
        /// Set all the commands we use in the MVP.
        /// </summary>
        /// <returns>The dictionary of commands.</returns>
        private Dictionary<string, ACommand> GetCommands()
        {
            m_commands = new Dictionary<string, ACommand>();
            ACommand Generate = new CommandGen(m_model, m_view);
            ACommand DisplayMaze = new CommandDisplayMaze(m_model, m_view);
            ACommand DisplaySolution = new CommandDisplaySolution(m_model, m_view);
            ACommand Solve = new CommandSolve(m_model, m_view);
            ACommand Save = new CommandSave(m_model, m_view);
            ACommand Load = new CommandLoad(m_model, m_view);
            ACommand Exit = new CommandExit(m_model, m_view);
            ACommand DoesExist = new CommandDoesExist(m_model, m_view);
            m_commands.Add(Generate.GetName(), Generate);
            m_commands.Add(DisplayMaze.GetName(), DisplayMaze);
            m_commands.Add(Solve.GetName(), Solve);
            m_commands.Add(DisplaySolution.GetName(), DisplaySolution);
            m_commands.Add(Save.GetName(), Save);
            m_commands.Add(Load.GetName(), Load);
            m_commands.Add(Exit.GetName(), Exit);
            return m_commands;
        }

        /// <summary>
        /// Set the events.
        /// </summary>
        /// <remarks>Send to RetriveSolution.</remarks>
        private void SetEvents()
        {
            m_model.Event += RetriveSolution;
        }

        /// <summary>
        /// Get the solution and displayes with event to the view.
        /// </summary>
        /// <param name="mazename">the name of the maze for which we want to display solution.</param>
        private void RetriveSolution(string mazename)
        {
            string s = m_model.printsolution(mazename);
            m_view.Output("solution for maze '" + mazename.ToLower() + "' \n" + s);
        }

        /// <summary>
        /// Get The maze maze name.
        /// </summary>
        /// <param name="mazename"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private void RetriveSolution(string mazename, int x, int y, int z)
        {
            AMaze maze = m_model.GetMaze(mazename);
            if (maze != null)
                m_view.Output("maze generated!");
        }

        /// <summary>
        /// Set the events.
        /// </summary>
        /// <remarks>For the view - split the viewevent and do the command in the first place of the splitted array.
        /// For the model - split the modelevent and the string in the first place send to the proper method in the model.</remarks>
        private void GetEvent()
        {
            m_view.viewChanged += new ViewEventDelegate((string viewEvent) =>
            {
                string s = viewEvent;
                string[] splitted = s.Split(' ');
                m_commands[splitted[0]].DoCommand(splitted);
            });
            m_model.Event += new ModelEventDelegate((string modelEvent) =>
              {
                  string s = modelEvent;
                  string[] splitted = s.Split(' ');
                  if (s != null)
                  {
                      switch (splitted[0])
                      {
                          case "SolutionExist":
                              m_view.Output("Solution for " + splitted[1].ToLower() + " already exist!");
                              break;
                          case "SolutionNotExist":
                              m_view.Output("Solution for " + splitted[1].ToLower() + " does not exist!");
                              break;
                          case "MazeNotExist":
                              m_view.Output("Maze '" + splitted[1].ToLower() + "' does not exist!");
                              break;
                          case "MazeExist":
                              m_view.Output("Maze '" + splitted[1].ToLower() + "' already exist!");
                              break;
                          case "MazeSaved":
                              break;
                          case "SolutionReady":
                              m_view.Output("Solution for '" + splitted[1].ToLower() + "' is ready!");
                              break;
                          case "MazeReady":
                              m_view.Output("Maze '" + splitted[1].ToLower() + "' is ready!");
                              break;
                          case "UnzipError":
                              m_view.Output("Error while unziping: " + splitted[1]);
                              break;
                          case "ZipError":
                              m_view.Output(splitted[1]);
                              break;
                          case "DisplayMaze":
                              m_view.DisplayMaze(m_model.GetMaze(splitted[1]), true, m_model.GetSolution(splitted[1]));
                              break;
                          case "DisplayError":
                              m_view.Output("Error accurred while trying to display '" + splitted[1].ToLower() + "'");
                              break;
                      }
                  }
              });
        }
    }
}
