using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pasjans
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
        public Bitmap CardIcon = null;
        public int Priority = -1;
        public string Name = string.Empty;
        public CardType Type;

        public Card(int cardnum, CardType CardType)
        {
            Bitmap cards = Properties.Resources.deck;

            int width = cards.Width / 13;
            int height = cards.Height / 4;
            int left = width * cardnum;
            int top = height * (int)CardType + 1;
            var bmp = new Bitmap(width, height,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (var graph = Graphics.FromImage(bmp))
            {
                graph.DrawImage(cards,
                    new Rectangle(0, 0, width, height),
                    new Rectangle(left, top, width, height),
                    GraphicsUnit.Pixel);
            }
            CardIcon = bmp;
            Priority = cardnum;
            Type = CardType;
            switch (cardnum)
            {
                case 0: Name = "A"; break;
                case 1: Name = "2"; break;
                case 2: Name = "3"; break;
                case 3: Name = "4"; break;
                case 4: Name = "5"; break;
                case 5: Name = "6"; break;
                case 6: Name = "7"; break;
                case 7: Name = "8"; break;
                case 8: Name = "9"; break;
                case 9: Name = "10"; break;
                case 10: Name = "J"; break;
                case 11: Name = "Q"; break;
                default: Name = "K"; break;
            }
        }

        public Card Copy()
        {
            return new Card(Priority, Type);
        }
    }


}
