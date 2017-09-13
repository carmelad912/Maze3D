using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATPProject.Presenter1
{
    /// <summary>
    /// ICommand is the interface that "ACommand" will inherit.
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// The function that doing a given command - the following class
        /// will implement the function.
        /// </summary>
        /// <param name="parameters">the command and parameters required to do the command received</param>
        void DoCommand(params string[] parameters);

        /// <summary>
        /// Returns the name of the command.
        /// </summary>
        /// <returns>The name of the command.</returns>
        string GetName();

    }
}
