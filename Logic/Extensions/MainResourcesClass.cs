using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Logic
{
    public enum Difficulty
    {
        Easy,
        Hard,
        Impossible
    }

    public static class LogicResources
    {
        public const int Space = 15;
        public static Difficulty Difficulty;
        public static CardType DeckColor;
        public static Bitmap Back;
        public static Bitmap Frame;
        public static Canvas Main;
        public static Window MainWindow;
        public static Bitmap Deck;
        public static int UniqueName = 0;
        public static double CardHeight;
        public static double CardWidth;
        public static bool Picked = false;
        public static List<Card> PickedCards = new List<Card>();
        public static List<Stack> Stacks = new List<Stack>();
    }
}
