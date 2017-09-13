using ATPProject.Model1;
using ATPProject.Presenter1;
using ATPProject.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ATPProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// When the running begin - we conncet between the presenter, model and view and start the Play window.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            IModel model = new Model();
            IView view2 = new PlayWindow();
            Presenter presenter2 = new Presenter(model, view2);
            view2.Start();
        }
    }
}
