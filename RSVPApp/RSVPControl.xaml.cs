using System;
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

using Windows.Graphics;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace RSVPApp
{
    public sealed partial class RSVPControl : UserControl
    {
        public RSVPControl()
        {
            this.InitializeComponent();
        }

        public string CurrentWord
        {
            set {
                    currentWord = value;
                    var newPadding = textBlock.Padding;
                    newPadding.Left = 90.0 - 15.0 * ((currentWord.Length + 6)/4 - 1);
                    textBlock.Padding = newPadding;
                    textBlock.Text = currentWord;
                }
            get { return currentWord; }
        }

        private string currentWord;
    }

    class Controller
    {
        public Controller()
        {
            timer.Tick += showNewWord;
        }

        public void SetContainer(TextContainer container)
        {
            this.container = container;
        }

        public void SetControl(RSVPControl control)
        {
            this.control = control;
        }

        public void SetInterval(int interval)
        {
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
        }

        public void GoToBegining()
        {
            container.ResetPointer();
            control.CurrentWord = "";
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void showNewWord(object sender, object e)
        {
            currentWord = container.emitWord() ?? "";
            switch (currentWord)
            {
                case "":
                    timer.Stop();
                    GoToBegining();
                    break;
                default:
                    control.CurrentWord = currentWord;
                    break;
            }
        }

        private DispatcherTimer timer = new DispatcherTimer();
        private TextContainer container;
        private RSVPControl control;
        String currentWord;
    } 

    class TextContainer
    {
        public TextContainer()
        { }

        public TextContainer(List<String> text)
        {
            this.text = text;
            currentWord = text.GetEnumerator();
            DispatcherTimer timer = new DispatcherTimer();
        }

        public void ResetPointer()
        {
            currentWord = text.GetEnumerator();
        }

        public void SetText(List<String> text)
        {
            this.text = text;
            currentWord = text.GetEnumerator();
        }

        public String emitWord()
        {
            if (currentWord.MoveNext())
                return currentWord.Current;
            else
                return null;
        }

        private List<String>.Enumerator currentWord;
        private List<String> text;
    }
}
