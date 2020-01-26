using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;


namespace Logic
{
    public enum CardType
    {
        Hearts, //serce
        Spades,  //pik
        Diamonds, //diament
        Clubs //trefl
    };

    public class Card
    {
        public Point? imagePosition;
        private double deltaX;
        private double deltaY;
        private TranslateTransform currentTT;

        public Bitmap CardIcon = null;
        public Bitmap BackCardIcon = LogicResources.Back;
        private int zIndex;

        public int Zindex
        {
            get { return zIndex; }
            set 
            { 
                zIndex = value;
                Panel.SetZIndex(Image, zIndex);
            }
        }

        public int Priority = -1;
        public string Name = string.Empty;
        public CardType Type;
        private bool pcked;

        public bool Picked
        {
            get { return pcked; }
            set 
            { 
                pcked = value;
                LogicResources.Picked = value;
            }
        }


        private bool visible;

        public bool Visible
        {
            get 
            { 
                return visible; 
            }
            set 
            {
                visible = value;
                Image.Source = visible ? CardIcon.ToBitmapImage() : BackCardIcon.ToBitmapImage();
            }
        }

        public Thickness thic;
        public Stack Stack { get; set; }
        public Image Image { get; set; }

        public Card(int cardnum, CardType CardType)
        {
            Priority = cardnum;
            Type = CardType;
            CardIcon = GetCardIcon();
            Name = GetNameByPriority(Priority);
            Image = CreateImageControl();
        }
        public void Show()
        {
            SetUpImageControl();
        }

        private void SetUpImageControl()
        {
            LogicResources.Main.Children.Add(Image);
            double tempTop = Stack.Border.Margin.Top;
            double tempBottom = Stack.Border.Margin.Bottom;
            if (Stack.Type == StackType.Main)
            {
                Image.MouseDown += Image_MouseDown;
                Image.MouseMove += Image_MouseMove;
                Image.MouseUp += Image_MouseUp;
                tempTop = tempTop + ((Stack.Cards.Count() - 1) * LogicResources.Space);
                tempBottom = tempTop - ((Stack.Cards.Count() - 1) * LogicResources.Space);
            } else if(Stack.Type == StackType.Reserve)
            {
                Image.MouseDown += Image_MouseDown1;
            }

            Image.IsEnabled = true;
            Image.Margin = new Thickness(Stack.Border.Margin.Left, tempTop, Stack.Border.Margin.Right, tempBottom);
        }

        private void Image_MouseDown1(object sender, MouseButtonEventArgs e)
        {
            Stack.AddCardsFromReserve();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Stack.CheckIfCanMove(this))
            {
                if(LogicResources.PickedCards.Count() == 0)
                    LogicResources.PickedCards = Stack.GetAllCardsFromCardIndexToEndOfTheStack(this);

                if (Stack.GetAllCardsAfterCard(this).Count() != 0)
                {
                    Stack.GetAllCardsAfterCard(this).First().Image_MouseDown(Stack.GetAllCardsAfterCard(this).First(), e);
                }

                if (imagePosition == null)
                    imagePosition = Image.TransformToAncestor(LogicResources.Main).Transform(new Point(0, 0));
                var mousePosition = Mouse.GetPosition(LogicResources.Main);
                deltaX = mousePosition.X - imagePosition.Value.X;
                deltaY = mousePosition.Y - imagePosition.Value.Y;
                Picked = true;
            }
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
           if(Picked)
            {
                if (Stack.GetAllCardsAfterCard(this).Count() != 0)
                {
                    Stack.GetAllCardsAfterCard(this).First().Image_MouseMove(Stack.GetAllCardsAfterCard(this).First(), e);
                }

                var mousePoint = Mouse.GetPosition(LogicResources.Main);

                var offsetX = (currentTT == null ? imagePosition.Value.X : imagePosition.Value.X - currentTT.X) + deltaX - mousePoint.X;
                var offsetY = (currentTT == null ? imagePosition.Value.Y : imagePosition.Value.Y - currentTT.Y) + deltaY - mousePoint.Y;

                Image.RenderTransform = new TranslateTransform(-offsetX, -offsetY);
                Zindex = 100;
            }

        }

        public void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Functions.ChangeAllCanvasOfMainStacksZindex(1000);
            if (Stack.GetAllCardsAfterCard(this).Count() != 0)
            {
                Stack.GetAllCardsAfterCard(this).First().Image_MouseUp(Stack.GetAllCardsAfterCard(this).First(), e);
            }
            else
            {
                if (Stack.Border.Margin.Top >= Mouse.GetPosition(LogicResources.Main).Y)
                {
                    Functions.GetBackPickedCardToPosition();
                }
            }

            currentTT = Image.RenderTransform as TranslateTransform;
            Picked = false;
            Zindex = Stack.Cards.GetElementPosition(this);

        }

        public bool GetNextCardPriorityAndCheck()
        {
            try
            {
                return Stack.Cards[Stack.Cards.GetElementIndex(this) + 1].Priority == Priority -1;
            }
            catch
            {
                return true;
            }

        }

        public bool GetPreviousCardPriorityAndCheck()
        {
            try
            {
               return Stack.Cards[Stack.Cards.GetElementIndex(this) - 1].Priority == Priority + 1;
            }
            catch 
            {
                return true;
            }
        }


        private string GetNameByPriority(int priority)
        {
            switch (priority)
            {
                case 0: return "A";
                case 1: return "2";
                case 2: return "3";
                case 3: return "4";
                case 4: return "5";
                case 5: return "6";
                case 6: return "7";
                case 7: return "8";
                case 8: return "9";
                case 9: return "10";
                case 10: return "J";
                case 11: return "Q";
                default: return "K";
            }
        }

        private Bitmap GetCardIcon()
        {
            Bitmap cards = LogicResources.Deck;

            int width = cards.Width / 13;
            int height = cards.Height / 4;
            int left = width * Priority;
            int top = height * (int)Type + 1;
            var bmp = new Bitmap(width, height,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (var graph = Graphics.FromImage(bmp))
            {
                graph.DrawImage(cards,
                    new Rectangle(0, 0, width, height),
                    new Rectangle(left, top, width, height),
                    GraphicsUnit.Pixel);
            }

            return bmp;
        }

        public Image CreateImageControl()
        {

            LogicResources.UniqueName++;

            return new Image
            {
                Width = LogicResources.CardWidth,
                Height = LogicResources.CardHeight,
                Stretch = Stretch.Fill,
                Name = $"Card{LogicResources.UniqueName+"_"+Name}",
                IsEnabled = false
            };


        }

        public Card Copy()
        {
            return new Card(Priority, Type);
        }

        public void Clear()
        {
            LogicResources.Main.Children.Remove(Image);
        }
    }

    public static class CardGlobalFunctions
    {
        public static List<Card> GenerateCard()
        {
            List<Card> cards = new List<Card>();
            switch (LogicResources.Difficulty)
            {
                case Difficulty.Easy:
                    Easy(ref cards);
                    break;
                case Difficulty.Hard:
                    Hard(ref cards);
                    break;

                case Difficulty.Impossible:
                    Impossible(ref cards);
                    break;
            }

            return cards;
        }

        private static void Easy(ref List<Card> cards)
        {
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    cards.Add(new Card(j, LogicResources.DeckColor));
                }
            }
        }

        private static void Hard(ref List<Card> cards)
        {
            CardType Color = CardType.Clubs;
            CardType Color2 = CardType.Clubs;
            switch(LogicResources.DeckColor)
            {
                case CardType.Hearts:
                    Color = CardType.Hearts;
                    Color2 = CardType.Spades;
                    break;

                case CardType.Spades:
                    Color = CardType.Hearts;
                    Color2 = CardType.Spades;
                    break;

                case CardType.Diamonds:
                    Color = CardType.Diamonds;
                    Color2 = CardType.Clubs;
                    break;

                case CardType.Clubs:
                    Color = CardType.Diamonds;
                    Color2 = CardType.Clubs;
                    break;

            }

            for (int i = 1; i <= 4; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    cards.Add(new Card(j, Color));
                }

                for (int j = 0; j <= 12; j++)
                {
                    cards.Add(new Card(j, Color2));
                }
            }
        }

        private static void Impossible(ref List<Card> cards)
        {
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    cards.Add(new Card(j, CardType.Hearts));
                }

                for (int j = 0; j <= 12; j++)
                {
                    cards.Add(new Card(j, CardType.Spades));
                }

                for (int j = 0; j <= 12; j++)
                {
                    cards.Add(new Card(j, CardType.Diamonds));
                }

                for (int j = 0; j <= 12; j++)
                {
                    cards.Add(new Card(j, CardType.Clubs));
                }
            }
        }
    }

}
