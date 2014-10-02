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
        private Controller()
        {
            timer.Tick += showNewWord;
        }

        public static void SetContainer(TextContainer container)
        {
            instance.container = container;
        }

        public static void SetControl(RSVPControl control)
        {
            instance.control = control;
        }

        public static void SetInterval(int interval)
        {
            instance.timer.Interval = new TimeSpan(0, 0, 0, 0, interval);
        }

        public static void GoToBegining()
        {
            instance.container.ResetPointer();
        }

        public static void Start()
        {
            instance.timer.Start();
        }

        public static void Stop()
        {
            instance.timer.Stop();
        }

        private void showNewWord(object sender, object e)
        {
            control.CurrentWord = container.emitWord() ?? "";
            if (control.CurrentWord == "")
            {
                timer.Stop();
                container.ResetPointer();
            }
        }

        private DispatcherTimer timer = new DispatcherTimer();
        private TextContainer container;
        private RSVPControl control;
        private static Controller instance = new Controller();
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
