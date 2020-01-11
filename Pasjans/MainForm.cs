using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pasjans
{
    public partial class MainForm : Form
    {
        public TimeSpan Time { get; set; } = TimeSpan.Zero;
        public int Score { get; set; } = 0;
        public int Moves { get; set; } = 0;
        public Stack Rest;

        public Stack Base1;
        public Stack Base2;
        public Stack Base3;
        public Stack Base4;
        public Stack Base5;
        public Stack Base6;
        public Stack Base7;
        public Stack Base8;

        public Stack Main1;
        public Stack Main2;
        public Stack Main3;
        public Stack Main4;
        public Stack Main5;
        public Stack Main6;
        public Stack Main7;
        public Stack Main8;
        public Stack Main9;
        public Stack Main10;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Images.Width = panel1.Width;
            Images.Height = panel1.Height;
            Rest = new Stack("Rest", StackType.Rest, panel1,this);

            Base1 = new Stack("Base1", StackType.Base, panel2,this);
            Base2 = new Stack("Base2", StackType.Base, panel3,this);
            Base3 = new Stack("Base3", StackType.Base, panel4,this);
            Base4 = new Stack("Base4", StackType.Base, panel5,this);
            Base5 = new Stack("Base5", StackType.Base, panel6,this);
            Base6 = new Stack("Base6", StackType.Base, panel7,this);
            Base7 = new Stack("Base7", StackType.Base, panel8,this);
            Base8 = new Stack("Base8", StackType.Base, panel9,this);

            Main1 = new Stack("Main1", StackType.Main, MP1,this);
            Main2 = new Stack("Main2", StackType.Main, MP2,this);
            Main3 = new Stack("Main3", StackType.Main, MP3,this);
            Main4 = new Stack("Main4", StackType.Main, MP4,this);
            Main5 = new Stack("Main5", StackType.Main, MP5,this);
            Main6 = new Stack("Main6", StackType.Main, MP6,this);
            Main7 = new Stack("Main7", StackType.Main, MP7,this);
            Main8 = new Stack("Main8", StackType.Main, MP8,this);
            Main9 = new Stack("Main9", StackType.Main, MP9,this);
            Main10 = new Stack("Main10", StackType.Main, MP10,this);
        }


        private void nowaGraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }

        public Stack GetFreeStack()
        {
            foreach (Stack s in new List<Stack>(new Stack[] { Base1, Base2, Base3, Base4, Base5, Base6, Base7, Base8 }))
            {
                if (s.Panel.BackgroundImage == Images.Frame)
                    return s;
            }
            return null;
        }
        public void UpdateScore()
        {
            label1.Text = string.Format("Czas: {0:mm\\:ss} Wynik: {1} Ruchy: {2}", Time, Score, Moves);
        }

        public void FinishGame()
        {
            timer1.Enabled = false;
            MessageBox.Show($"Gratulacje! Udało Ci się wygrać z wynikiem : {Score}");
            Properties.Settings.Default.Wyniki += $"{Score},{Time},{Moves};";
            Properties.Settings.Default.Save();
        }

        private void StartNewGame()
        {
            Time = TimeSpan.Zero;
            Score = 0;
            Moves = 0;

            timer1.Enabled = true;
            ClearAllControls();
            Array values = Enum.GetValues(typeof(CardType));
            Random random = new Random();
            CardType cardType = (CardType)values.GetValue(random.Next(values.Length));

            List<Card> cards = Extensions.GenerateCard(cardType);

            Rest.AddCardToStack(cards, 50);
            Main1.AddCardToStack(cards, 6);
            Main2.AddCardToStack(cards, 6);
            Main3.AddCardToStack(cards, 6);
            Main4.AddCardToStack(cards, 6);
            Main5.AddCardToStack(cards, 5);
            Main6.AddCardToStack(cards, 5);
            Main7.AddCardToStack(cards, 5);
            Main8.AddCardToStack(cards, 5);
            Main9.AddCardToStack(cards, 5);
            Main10.AddCardToStack(cards, 5);

        }

        private void ClearAllControls()
        {

            Rest.Clear();

            Base1.Clear();
            Base2.Clear();
            Base3.Clear();
            Base4.Clear();
            Base5.Clear();
            Base6.Clear();
            Base7.Clear();
            Base8.Clear();

            Main1.Clear();
            Main2.Clear();
            Main3.Clear();
            Main4.Clear();
            Main5.Clear();
            Main6.Clear();
            Main7.Clear();
            Main8.Clear();
            Main9.Clear();
            Main10.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Time += new TimeSpan(0, 0, 1);
            if(Time.TotalSeconds % 10 == 0)
            {
                Score -= 2;
            }
            UpdateScore();
        }

        private void wynikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Results r = new Results(Properties.Settings.Default.Wyniki);
            r.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FinishGame();
        }
    }
}
