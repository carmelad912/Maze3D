using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for MazeCell.xaml
    /// </summary>
    public partial class MazeCell : UserControl
    {
        /// <summary>
        /// Constructor of mazecell.
        /// </summary>
        /// <remarks>Initialize the mazecell, set the width and height with mazecellsize. if bool visible is true - put an image, else it will be transperent.</remarks>
        /// <param name="mazeCellSize"></param>
        /// <param name="visible"></param>
        public MazeCell(int mazeCellSize, bool visible)
        {
            InitializeComponent();

            cell.Width = mazeCellSize;
            cell.Height = mazeCellSize;

            if (visible)
                cell.image.Source = new BitmapImage(new Uri(string.Concat(Directory.GetCurrentDirectory(), "/rock.jpg")));
            else
                cell.Background = Brushes.Transparent;

        }
    }
}
