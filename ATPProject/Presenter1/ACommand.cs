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
    /// The abstract class of command.
    /// </summary>
    /// <remarks>Represent the abstract form of command and holds all the properties of every command.</remarks>
    abstract class ACommand:ICommand
    {
        /// <summary>
        /// The property that holds the model.
        /// </summary>
        protected IModel m_model;

        /// <summary>
        ///The property that holds the view. 
        /// </summary>
        protected IView m_view;

        /// <summary>
        /// Constructor of a command.
        /// </summary>
        /// <param name="model">The model of the command</param>
        /// <param name="view">The view of the command</param>
        public ACommand(IModel model, IView view)
        {
            m_model = model;
            m_view = view;
        }

        /// <summary>
        /// The function that doing a given command - each command
        /// will implement the function.
        /// </summary>
        /// <param name="parameters">the command and parameters required to do the command received</param>
        public abstract void DoCommand(params string[] parameters);

        /// <summary>
        /// Returns the name of the command.
        /// </summary>
        /// <returns>The name of the command.</returns>
        public abstract string GetName();
    }
}
