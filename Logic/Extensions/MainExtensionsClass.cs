using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Image = System.Windows.Controls.Image;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Reflection;
using Brushes = System.Windows.Media.Brushes;

namespace Logic
{
    public enum StackNames
    {
        Reserve,
        Base1,
        Base2,
        Base3,
        Base4,
        Base5,
        Base6,
        Base7,
        Base8,
        Main1,
        Main2,
        Main3,
        Main4,
        Main5,
        Main6,
        Main7,
        Main8,
        Main9,
        Main10
    }

    public static class Extensions
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }



        public static void SetUpCardsToStacks(this List<Stack> stacks, List<Card> cards)
        {
            stacks.GetElementByName(StackNames.Reserve).AddCardsToStackFromListOfNumer(cards, 50);

            stacks.GetElementByName(StackNames.Main1).AddCardsToStackFromListOfNumer(cards, 6);
            stacks.GetElementByName(StackNames.Main2).AddCardsToStackFromListOfNumer(cards, 6);
            stacks.GetElementByName(StackNames.Main3).AddCardsToStackFromListOfNumer(cards, 6);
            stacks.GetElementByName(StackNames.Main4).AddCardsToStackFromListOfNumer(cards, 6);
            stacks.GetElementByName(StackNames.Main5).AddCardsToStackFromListOfNumer(cards, 5);
            stacks.GetElementByName(StackNames.Main6).AddCardsToStackFromListOfNumer(cards, 5);
            stacks.GetElementByName(StackNames.Main7).AddCardsToStackFromListOfNumer(cards, 5);
            stacks.GetElementByName(StackNames.Main8).AddCardsToStackFromListOfNumer(cards, 5);
            stacks.GetElementByName(StackNames.Main9).AddCardsToStackFromListOfNumer(cards, 5);
            stacks.GetElementByName(StackNames.Main10).AddCardsToStackFromListOfNumer(cards, 5);
        }

        public static void ClearStacks(this List<Stack> stacks)
        {
            LogicResources.PickedCards.Clear();
            foreach (Stack s in stacks)
            {
               foreach(Card c in s.Cards)
                {
                    c.Clear();
                }
                s.Cards.Clear();
            }
        }
        public static int GetElementPosition(this List<Card> cards,Card c)
        {
            return cards.FindIndex(x => x == c) + 1;
        }

        public static int GetElementIndex(this List<Card> cards, Card c)
        {
            return cards.FindIndex(x => x == c);
        }

        public static Stack GetElementByName(this List<Stack> stacks, StackNames name)
        {
            return stacks.Where(x => x.Name == name.ToString()).First();
        }

        public static void DeleteCardsFromStack(this List<Card> cards)
        {
            foreach (Card c in cards)
            {
                LogicResources.Main.Children.Remove(c.Image);
                c.Stack.Cards.Remove(c);
            }
        }

        public static void AddCardsToStack(this List<Card> cards,Stack s)
        {
            foreach (Card c in cards)
            {
                s.AddCardToStack(c.Copy(), true);
            }
        }
    }
}
