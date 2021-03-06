﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace RSVPApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            List<String> text = new List<string>();

            text.Add("This");
            text.Add("is");
            text.Add("my");
            text.Add("new");
            text.Add("application");
            text.Add("for");
            text.Add("fast");
            text.Add("reading");
            text.Add(".");
            text.Add("\n");

            controller.Control = readControl;
            controller.Container = new TextContainer(text);
            controller.Interval = 200;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void StopTextOutput(object sender, RoutedEventArgs e)
        {
            controller.Stop();
            controller.GoToBegining();
            PlayButton.Label = "Play";
        }

        private void StartTextOutput(object sender, RoutedEventArgs e)
        {
            controller.Start();
        }

        private void PauseTextOutput(object sender, RoutedEventArgs e)
        {
            controller.Stop();
            PlayButton.Label = "Resume";
        }

        private Controller controller = new Controller();

        private void GoBackOneParagraph(object sender, RoutedEventArgs e)
        {
            controller.GoToPreviousParagraph();
        }

        private void GoBackOneSentence(object sender, RoutedEventArgs e)
        {
            controller.GoToPreviousSentence();
        }

        private void GoForwardOneSentence(object sender, RoutedEventArgs e)
        {
            controller.GoToNextSentence();
        }

        private void GoForwardOneParagraph(object sender, RoutedEventArgs e)
        {
            controller.GoToNextParagraph();
        }       
    }
}
