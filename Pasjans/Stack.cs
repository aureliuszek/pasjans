using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pasjans
{
    public enum StackType
    {
        Rest,
        Base,
        Main
    }
    public class Stack
    {
        private bool picked = false;
        private PictureBox FocusedPB { get; set; }
        public string Name { get; set; }
        public List<PictureBox> Pb = new List<PictureBox>();
        public List<PictureBox> TempPb = new List<PictureBox>();
        public StackType Type { get; set; }
        public List<Card> Cards = new List<Card>();
        public Panel Panel { get; set; }
        public MainForm Mf;
        public Stack(string name, StackType type, Panel panel, MainForm m)
        {
            Name = name;
            Type = type;
            Panel = panel;
            Panel.Height = Images.Height;
            Panel.Width = Images.Width;
            Panel.BackgroundImage = Images.Frame;
            Panel.BackgroundImageLayout = ImageLayout.Stretch;
            Panel.BackColor = Color.Transparent;
            Mf = m;
        }

        public void AddCardToStack(Card card, bool is6 = false, bool n = false)
        {
            switch (Type)
            {
                case StackType.Rest:
                    Cards.Add(card);
                    PictureBox p = new PictureBox
                    {
                        BackColor = Color.Transparent,
                        BackgroundImage = Images.Back,
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Width = Images.Width,
                        Height = Images.Height
                    };
                    Panel.Controls.Add(p);
                    Pb.Add(p);
                    p.Click += P_Click;

                    Panel.Controls.Add(p);

                    break;

                case StackType.Main:
                    Cards.Add(card);
                    PictureBox p2 = new PictureBox
                    {
                        AllowDrop = true,
                        BackColor = Color.Transparent,
                        BackgroundImage = Cards.Count == (is6 ? 6 : 5) ? card.CardIcon : Images.Back,
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Location = new Point(0, (Cards.Count - 1) * 15),
                        Width = Images.Width,
                        Height = Images.Height
                    };

                    if (n)
                    {
                        p2.BackgroundImage = card.CardIcon;
                        Images.stack = this;
                    }

                    Panel.AllowDrop = true;
                    Panel.DragEnter += Panel_DragEnter;
                    Panel.Parent.Controls.Add(p2);
                    Panel.Parent.Controls[Panel.Parent.Controls.GetChildIndex(p2)].BringToFront();
                    Pb.Add(p2);
                    p2.MouseMove += P2_MouseMove;
                    p2.MouseDown += P2_MouseDown;
                    p2.MouseUp += P2_MouseUp;
                    p2.DragEnter += P2_DragEnter;

                    break;
            }
        }

        private void CheckIfFullCardSort(Stack stack)
        {
            if (stack != null)
            {
                List<PictureBox> Pb = new List<PictureBox>();
                foreach (Card c in stack.Cards)
                {
                    if (stack.Pb[stack.Cards.IndexOf(c)].BackgroundImage != Images.Back && ((stack.Cards.IndexOf(c) - 1 >= 0 && (stack.Cards[stack.Cards.IndexOf(c) - 1].Priority - 1 == c.Priority || c.Priority == 12)) || c.Priority == 12))
                    {
                        Pb.Add(stack.Pb[stack.Cards.IndexOf(c)]);
                    }
                    else
                    {
                        Pb.Clear();
                    }
                }

                if (Pb.Count() == 13)
                {
                    Stack FreeStack = Mf.GetFreeStack();
                    foreach (PictureBox p in Pb)
                    {
                        FreeStack.AddToBase(p);
                        stack.Clear(p);
                    }
                    if (stack.Pb.Count != 0)
                        stack.Pb.Last().BackgroundImage = stack.Cards.Last().CardIcon;

                    if(Mf.GetFreeStack() == null)
                    {
                        Mf.FinishGame();
                    }
                }
            }
        }

        private void AddToBase(PictureBox p)
        {
            Panel.BackgroundImage = p.BackgroundImage;
        }

        private void Panel_DragEnter(object sender, DragEventArgs e)
        {
            if (Images.f2)
            {
                PictureBox p = (sender as PictureBox).GetPB();
                AddCardToStack(p.GetCard().Copy(), true, true);
                p.GetStack().Clear(p);

                if (p.GetStack().Pb.Count != 0)
                    p.GetStack().Pb.Last().BackgroundImage = p.GetStack().Cards.Last().CardIcon;

                Images.f2 = false;
                foreach (PictureBox pb in p.GetPB().GetStack().TempPb)
                {
                    AddCardToStack(p.GetPB().GetStack().Cards[(p.GetPB().GetStack().Pb.IndexOf(pb))].Copy(), true, true);
                    Mf.Controls.Remove(pb);
                    p.GetStack().Cards.Remove(p.GetStack().Cards[p.GetStack().Pb.IndexOf(pb)]);
                    p.GetStack().Pb.Remove(pb);
                    if (p.GetStack().Pb.Count != 0)
                        p.GetStack().Pb.Last().BackgroundImage = p.GetStack().Cards.Last().CardIcon;
                }
            }

        }

        private void P_Click(object sender, EventArgs e)
        {
            Mf.Main1.AddCardToStack(Cards.Last(), true, true);
            Clear(Pb[Pb.Count - 1]);
            Mf.Main2.AddCardToStack(Cards.Last(), true, true);
            Clear(Pb[Pb.Count - 1]);
            Mf.Main3.AddCardToStack(Cards.Last(), true, true);
            Clear(Pb[Pb.Count - 1]);
            Mf.Main4.AddCardToStack(Cards.Last(), true, true);
            Clear(Pb[Pb.Count - 1]);
            Mf.Main5.AddCardToStack(Cards.Last(), true, true);
            Clear(Pb[Pb.Count - 1]);
            Mf.Main6.AddCardToStack(Cards.Last(), true, true);
            Clear(Pb[Pb.Count - 1]);
            Mf.Main7.AddCardToStack(Cards.Last(), true, true);
            Clear(Pb[Pb.Count - 1]);
            Mf.Main8.AddCardToStack(Cards.Last(), true, true);
            Clear(Pb[Pb.Count - 1]);
            Mf.Main9.AddCardToStack(Cards.Last(), true, true);
            Clear(Pb[Pb.Count - 1]);
            Mf.Main10.AddCardToStack(Cards.Last(), true, true);
            Clear(Pb[Pb.Count - 1]);
        }

        private bool CheckIfCanMove(List<Card> list, Card p)
        {
            if (Pb[Cards.IndexOf(p)].BackgroundImage == Images.Back)
            {
                return false;
            }

            int c = 0;
            foreach (Card card in list)
            {
                if (list.IndexOf(card) > list.IndexOf(p))
                {
                    c++;
                    if (card.Priority + c != p.Priority)
                        return false;
                }
            }
            return true;
        }

        private List<PictureBox> ClaimAllCardsAfterIndex(PictureBox p)
        {
            List<PictureBox> pb = new List<PictureBox>();
            foreach (PictureBox c in Pb)
            {
                if (Pb.IndexOf(c) > Pb.IndexOf(p))
                {
                    pb.Add(c as PictureBox);
                }
            }
            return pb;
        }

        private void P2_DragEnter(object sender, DragEventArgs e)
        {
            PictureBox p = (sender as PictureBox).GetPB();
            if (Panel.Parent != p.GetParent() && Cards.Last().Priority == p.GetCard().Priority + 1 && Images.f)
            {
                AddCardToStack(p.GetCard().Copy(), true, true);
                p.GetStack().Clear((sender as PictureBox).GetPB());
                if (p.GetStack().Pb.Count != 0)
                    p.GetStack().Pb.Last().BackgroundImage = p.GetStack().Cards.Last().CardIcon;
            }
            else
            {
                Images.f = false;
                p.Parent = p.GetParent();
                p.Location = p.GetLocation();
                p.BringToFront();
            }
        }

        private void P2_MouseDown(object sender, MouseEventArgs e)
        {

            FocusedPB = sender as PictureBox;
            FocusedPB.SavePB(Cards[Pb.IndexOf(FocusedPB)], this);
            if (CheckIfCanMove(Cards, (sender as PictureBox).GetCard()))
            {
                picked = true;
                FocusedPB.Parent = Form.ActiveForm;
                FocusedPB.Location = Cursor.Position;
                FocusedPB.BringToFront();
            }
            else
            {
                FocusedPB = null;
            }
        }

        private void P2_MouseMove(object sender, MouseEventArgs e)
        {
            if (FocusedPB != null && picked)
            {
                Point newlocation = FocusedPB.Location;
                newlocation.X += e.X;
                newlocation.Y += e.Y;
                FocusedPB.Location = newlocation;
                int t = 0;
                TempPb = ClaimAllCardsAfterIndex(FocusedPB);
                foreach (PictureBox p in TempPb)
                {
                    t += 15;
                    p.Parent = Form.ActiveForm;
                    p.Location = new Point(newlocation.X, newlocation.Y + t);
                    p.BringToFront();
                }
            }
        }
        private void P2_MouseUp(object sender, MouseEventArgs e)
        {
            if (FocusedPB != null)
            {
                Images.f = true;
                picked = false;
                Control tempP = null;
                FocusedPB.Parent = FocusedPB.GetParent();
                tempP = FocusedPB.GetParent(); ;
                FocusedPB.Location = FocusedPB.GetLocation();
                FocusedPB.BringToFront();
                FocusedPB.DoDragDrop(this, DragDropEffects.All);

                int t = 15;
                foreach (PictureBox p in TempPb)
                {
                    if (Images.f2)
                    {
                        p.Parent = FocusedPB.GetParent();
                        p.Location = new Point(FocusedPB.GetLocation().X, FocusedPB.GetLocation().Y + t);
                        p.SavePB(Cards[Pb.IndexOf(p)], this);
                        p.BringToFront();
                        p.DoDragDrop(this, DragDropEffects.All);
                    }
                }
                Images.f2 = true;
                if (FocusedPB.Parent != tempP)
                {
                    Mf.Moves += 1;
                    Mf.Score += 5;
                    Mf.UpdateScore();
                }

                CheckIfFullCardSort(Images.stack);
            }
        }

        public void AddCardToStack(List<Card> list, int numbers)
        {
            int i = 0;
            Random random = new Random();
            while (i != numbers)
            {
                i++;
                int r = random.Next(list.Count);
                AddCardToStack(list[r], numbers == 6);
                list.RemoveAt(r);
            }
        }

        public void Clear()
        {
            foreach (PictureBox p in Pb)
            {
                p.Parent.Controls.Remove(p);
            }

            if(Type == StackType.Base)
            {
                Panel.BackgroundImage = Images.Frame;
            }

            Pb.Clear();
            Cards.Clear();
        }

        public void Clear(PictureBox p)
        {
            p.Parent.Controls.Remove(p);

            Cards.Remove(Cards[Pb.IndexOf(p)]);

            Pb.Remove(p);
        }
    }
}
