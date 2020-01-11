using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pasjans
{
    public static class Extensions
    {
        public static Point LocationOfPb { get; set; }
        public static Control ParentOfPB { get; set; }
        public static PictureBox Pb { get; set; }
        public static Card Card { get; set; }
        public static Stack Stack { get; set; }
        public static void ChangeBackgroundImage(this List<PictureBox> ls,Bitmap img)
        {
            foreach(PictureBox p in  ls)
            {
                p.BackgroundImage = img;
                p.BackgroundImageLayout = ImageLayout.Stretch;
                p.BackColor = Color.Transparent;
            }
        }

        public static int GetNextCardPriorityInStack(this Stack s, Card c)
        {
            if (s.Cards.Count() >= s.Cards.IndexOf(c)+2)
            {
                return s.Cards[s.Cards.IndexOf(c) + 1].Priority;
            }
            else
            {
                return -1;
            }
        }

        public static int GetPrevCardPriorityInStack(this Stack s, Card c)
        {
            if (s.Cards.IndexOf(c) > 0)
            {
                return s.Cards[s.Cards.IndexOf(c) - 1].Priority;
            }
            else
            {
                return -1;
            }
        }

        public static void ChangeWidth(this List<PictureBox> ls, int width)
        {
            foreach (PictureBox p in ls)
            {
                p.Width = width;
                p.BackgroundImageLayout = ImageLayout.Stretch;
                p.BackColor = Color.Transparent;
            }
        }

        public static void ChangeHeight(this List<PictureBox> ls, int height)
        {
            foreach (PictureBox p in ls)
            {
                p.Height = height;
                p.BackgroundImageLayout = ImageLayout.Stretch;
                p.BackColor = Color.Transparent;
            }
        }

        public static List<Card> GenerateCard(CardType cardType)
        {
            List<Card> cards = new List<Card>();
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 0; j <= 12; j++)
                {
                    cards.Add(new Card(j, cardType));
                }
            }

            return cards;
        }

        public static void SavePB(this PictureBox pb,Card c,Stack stack)
        {
            Pb = pb;
            LocationOfPb = pb.Location;
            ParentOfPB = pb.Parent;
            Card = c;
            Stack = stack;
        }

        public static Point GetLocation(this PictureBox pb)
        {
            return LocationOfPb;
        }

        public static Control GetParent(this PictureBox pb)
        {
            return ParentOfPB;
        }

        public static Card GetCard(this PictureBox pb)
        {
            return Card;
        }

        public static PictureBox GetPB(this PictureBox pb)
        {
            return Pb;
        }

        public static Stack GetStack(this PictureBox pb)
        {
            return Stack;
        }
    }
}
