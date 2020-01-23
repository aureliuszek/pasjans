using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Logic
{
    public static class Functions
    {
        public static IEnumerable<T> GetAllControlsOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in GetAllControlsOfType<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static void ChangeBackgroundImage(IEnumerable<Image> imageList, BitmapImage imageSource)
        {
            foreach (Image i in imageList)
            {
                i.Source = imageSource;
                i.Stretch = Stretch.Fill;
            }
        }

        public static void ChangeAllCanvasOfMainStacksZindex(int zIndex)
        {
            foreach(Canvas c in LogicResources.Stacks.Where(x => x.Type == StackType.Main).Select(x => x.Canvas))
            {
                Panel.SetZIndex(c, zIndex);
            }
        }

        public static void GetBackPickedCardToPosition()
        {
            if (LogicResources.PickedCards.Count() != 0)
            {
                LogicResources.PickedCards.DeleteCardsFromStack();

                LogicResources.PickedCards.AddCardsToStack(LogicResources.PickedCards.First().Stack);

                LogicResources.PickedCards.Clear();

                LogicResources.Picked = false;
            }
        }

        public static Stack GetFreeBaseStack()
        {
            return LogicResources.Stacks.Where(x => x.Type == StackType.Base && x.Cards.Count() == 0).First();
        }

        public static void CheckIfWin()
        {
            if(LogicResources.Stacks.Where(x => x.Type == StackType.Base && x.Cards.Count() == 0).Count() == 0)
            {
                MessageBox.Show("Wygrałeś");
            }
        }

        public static CardType RandomDeckColor()
        {
                Array values = Enum.GetValues(typeof(CardType));
                Random random = new Random();
                CardType randomType = (CardType)values.GetValue(random.Next(values.Length));
                return randomType;
        }
    }
}
