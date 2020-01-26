using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Brushes = System.Windows.Media.Brushes;
namespace Logic
{
    public enum StackType
    {
        Reserve,
        Base,
        Main
    }
    public class Stack
    {
        public string Name { get; set; }
        public StackType Type { get; set; }
        public List<Card> Cards = new List<Card>();
        public Border Border;
        public Canvas Canvas { get; set; }
        public Thickness Location;
        public Stack(string name, StackType type, Border border)
        {
            Name = name;
            Type = type;
            Border = border;
            Location = border.Margin;
            if (type == StackType.Main)
            {
                Canvas = CreateStackCanvas();
                LogicResources.Main.Children.Add(Canvas);
                Canvas.MouseEnter += Canvas_MouseEnter;
            }
        }

        public void Canvas_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Functions.ChangeAllCanvasOfMainStacksZindex(-1);

            if (!LogicResources.Picked && LogicResources.PickedCards.Count() != 0)
            {
                Stack s = LogicResources.PickedCards.First().Stack;
                LogicResources.PickedCards.DeleteCardsFromStack();
                if (Cards.Count() != 0 && LogicResources.PickedCards.First().Priority != Cards.Last().Priority - 1 || s == this)
                {
                    LogicResources.PickedCards.AddCardsToStack(s);
                } else
                {
                    if (s.Cards.Count() != 0)
                        s.Cards.Last().Visible = true;
                    LogicResources.PickedCards.AddCardsToStack(this);
                }


                LogicResources.PickedCards.Clear();
            }
        }

        public void AddCardsFromReserve()
        {
            List<Card> listOfCards = new List<Card>();

            int i = 9;
            foreach (Card c in Cards)
            {
                LogicResources.Stacks[i].AddCardToStack(c.Copy(), true);
                listOfCards.Add(c);
                i++;
                if (i == 19)
                    break;
            }
            listOfCards.DeleteCardsFromStack();
        }

        private void CheckIfFullCardSort()
        {
            List<Card> cards = new List<Card>();
            foreach(Card c in Cards)
            {
                if (c.Name == "K")
                {
                    cards.Clear();
                    cards.Add(c);
                }
                else if (c.Visible == true && (c.GetNextCardPriorityAndCheck() && c.GetPreviousCardPriorityAndCheck()))
                {
                    cards.Add(c);
                } else
                {
                    cards.Clear();
                }
            }

            if(cards.Count() == 13 && cards.All(x => x.Type == cards.First().Type))
            {
                Stack s = Functions.GetFreeBaseStack();

                foreach (Card c in cards)
                {
                    s.AddCardToStack(c.Copy(), true);
                }
                cards.DeleteCardsFromStack();

                if (Cards.Count() != 0)
                    Cards.Last().Visible = true;

                Functions.CheckIfWin();
            }


        }

        public Canvas CreateStackCanvas()
        {

            return new Canvas
            {
                Margin = Border.Margin,
                Height = LogicResources.MainWindow.Height - Border.Margin.Top,
                Width = LogicResources.CardWidth,
                Background = Brushes.Transparent
             };
        }
        public bool CheckIfCanMove(Card c)
        {
            if (!c.Visible)
                return false;

            while(GetNextCardInStack(c) != null)
            {
                if (c.Priority != GetNextCardInStack(c).Priority + 1 || c.Type != GetNextCardInStack(c).Type)
                    return false;
                return CheckIfCanMove(GetNextCardInStack(c));
            }
            return true;
        }

        private Card GetNextCardInStack(Card c)
        {
            try
            {
                return Cards[Cards.GetElementIndex(c) + 1];
            }
            catch 
            {
                return null;
            }
        }
        private Card GetPreviousCardInStack(Card c)
        {
            try
            {
                return Cards[Cards.GetElementIndex(c) - 1];
            }
            catch
            {
                return null;
            }
        }

        public List<Card> GetAllCardsAfterCard(Card c)
        {
            List<Card> cardList = new List<Card>();
            for (int i = Cards.GetElementPosition(c); i < Cards.Count(); i++)
            {
                cardList.Add(Cards[i]);
            }
            return cardList;
        }

        public List<Card> GetAllCardsFromCardIndexToEndOfTheStack(Card c)
        {
            List<Card> cardList = new List<Card>();
            for (int i = Cards.GetElementIndex(c); i < Cards.Count(); i++)
            {
                cardList.Add(Cards[i]);
            }
            return cardList;
        }

        public void AddCardsToStackFromListOfNumer(List<Card> list, int numbers)
        {
            int i = 0;
            Random random = new Random();
            while (i != numbers)
            {
                i++;
                int r = random.Next(list.Count);
                AddCardToStack(list[r], i == numbers && Type != StackType.Reserve);
                list.RemoveAt(r);
            }
        }

        public void AddCardToStack(Card card, bool visible)
        {
            Cards.Add(card);
            card.Stack = this;
            card.Zindex = Cards.Count();
            card.Visible = visible;
            card.Show();
            if(Type == StackType.Main)
                CheckIfFullCardSort();
        }
        
        public void Clear()
        {
        }
    }
}
