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

        public TextContainer Container
        {
            set { this.container = value; }
            private get { return this.container;  }
        }

        public RSVPControl Control
        {
            set { this.control = value; }
            private get { return this.control; }
        }

        public int Interval
        {
            set { this.timer.Interval = new TimeSpan(0, 0, 0, 0, value); }
            private get { return Convert.ToInt32(this.timer.Interval.TotalMilliseconds); }
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

        public void GoToPreviousParagraph()
        {
            container.GoToPreviousParagraph();
        }

        public void GoToPreviousSentence()
        {
            container.GoToPreviousSentence();
        }

        public void GoToNextParagraph()
        {
            container.GoToNextParagraph();
        }

        public void GoToNextSentence()
        {
            container.GoToNextSentence();
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
                case ".":
                    control.CurrentWord = control.CurrentWord + currentWord;
                    break;
                case "\n":
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
            initEnumeratorsLists();
        }

        public void ResetPointer()
        {
            currentWord = text.GetEnumerator();
            currentParagraphIndex = 0;
            currentSentenceIndex = 0;
        }

        public List<String> Text
        {
            set
            {
                this.text = value;
                currentWord = text.GetEnumerator();
                initEnumeratorsLists();
            }
            private get { return this.text; }
        }

        public String emitWord()
        {
            string word;
            if (currentWord.MoveNext())
            {
                word = currentWord.Current;
                if (word == "\n")
                    ++currentParagraphIndex;
                if (word == ".")
                    ++currentSentenceIndex;
                return word;
            }
            else
                return null;
        }

        public void GoToPreviousParagraph()
        {
            if (currentParagraphIndex > 0)
                currentWord = paragraphs[currentParagraphIndex--];
            else
                currentWord = paragraphs[currentParagraphIndex];
        }
        public void GoToNextParagraph()
        {
            if (currentParagraphIndex < paragraphs.Count-1)
                currentWord = paragraphs[++currentParagraphIndex];
            else
                currentWord = paragraphs[currentParagraphIndex];
        }
        public void GoToPreviousSentence()
        {
            if (currentSentenceIndex > 0)
                currentWord = sentences[currentSentenceIndex--];
            else
                currentWord = sentences[currentSentenceIndex];
        }
        public void GoToNextSentence()
        {
            if (currentParagraphIndex < sentences.Count-1)
                currentWord = sentences[++currentSentenceIndex];
            else
                currentWord = sentences[currentSentenceIndex];
        }

        private void initEnumeratorsLists()
        {
            paragraphs = new List<List<string>.Enumerator>();
            sentences = new List<List<string>.Enumerator>();
            var word = text.GetEnumerator();
            paragraphs.Add(word);
            sentences.Add(word);
            while (word.MoveNext())
            {
                if (word.Current == "\n")
                    paragraphs.Add(word);
                if (word.Current == ".")
                    sentences.Add(word);
            }
        }


        private List<String>.Enumerator currentWord;
        private List<List<String>.Enumerator> paragraphs;
        private List<List<String>.Enumerator> sentences;
        private int currentSentenceIndex = 0;
        private int currentParagraphIndex = 0;
        private List<String> text;
    }
}
