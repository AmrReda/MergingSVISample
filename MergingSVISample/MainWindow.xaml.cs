using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace MergingSVISample
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class MainWindow : SurfaceWindow
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }
        private void ScatterViewItem_TouchUp(object sender, TouchEventArgs e)
        {
            ScatterViewItem sviSource = sender as ScatterViewItem;

            //Iterate through all of the SVI's
            foreach (ScatterViewItem sviChild in _scatter.Items)
            {
                //ignore the SVI that generated the event
                if (sviChild == sviSource)
                    continue;
                else
                {
                    //Get a vector that represents the diffference between the 2 SVI's centers.
                    Vector v = Point.Subtract(sviChild.ActualCenter, sviSource.ActualCenter);

                    //Output the distance to the debug window for reference
                    Debug.WriteLine("Distance :" + v.Length.ToString());

                    //100 is an Ok value, use the output from above to fine tune this
                    if (v.Length < 100)
                    {
                        MergeBetween(sviChild, sviSource);
                        return;
                    }
                }
            }
        }

        private void MergeBetween(ScatterViewItem sviChild, ScatterViewItem sviSource)
        {
            //merge the colors and assign it to the remaining SVI
            //expanded for readibility
            SolidColorBrush scb1 = sviSource.Background as SolidColorBrush;
            SolidColorBrush scb2 = sviChild.Background as SolidColorBrush;

            sviChild.Background = new SolidColorBrush(Color.Add(scb1.Color, scb2.Color));
           
            _scatter.Items.Remove(sviSource);

            //No need to continue, exit out.
            return;
        }
    }
}