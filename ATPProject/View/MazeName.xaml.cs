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
using System.Windows.Shapes;

namespace ATPProject.View
{
    /// <summary>
    /// Interaction logic for MazeName.xaml
    /// </summary>
    public partial class MazeName : Window
    {
        /// <summary>
        /// The property that holds the mazename.
        /// </summary>
        private string m_Mazename;

        /// <summary>
        /// Getter and setter for m_Mazename.
        /// </summary>
        public string m_mazename
        {
            get { return m_Mazename; }
            set { m_Mazename = value; }
        }

        /// <summary>
        /// Constructor of MazeName.
        /// </summary>
        public MazeName()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles with the pressing on the button of display.
        /// </summary>
        /// <remarks>save the content of the text box in m_mazename and close the window.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Display_Click(object sender, RoutedEventArgs e)
        {
            m_mazename = Mazename.Text.Trim();
            base.Close();
        }

        /// <summary>
        /// Handles if a key is pressed.
        /// </summary>
        /// <remarks>If enter pressed - get the text in the textbox and save to m_mazename then close the window.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MazeName_KeyDown(object sender, KeyEventArgs e)
        {
            Key key = e.Key;
                switch (key)
                {
                    case Key.Enter:
                        {
                            m_mazename = Mazename.Text.Trim();
                            e.Handled = true;
                            base.Close();
                            break;
                        }
            }
        }

        /// <summary>
        /// When click 'X' button - close the window.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
