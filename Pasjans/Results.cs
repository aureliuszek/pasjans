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
    public partial class Results : Form
    {
        public class Wynik
        {
            public int Score { get; set; }
            public int Moves { get; set; }
            public TimeSpan Time { get; set; }

            public Wynik(string[] s)
            {
                Score = int.Parse(s[0]);
                Time = TimeSpan.Parse(s[1]);
                Moves = int.Parse(s[2]);
            }
        }
        public Results(string wyniki)
        {
            InitializeComponent();
            string[] s = wyniki.Split(';');
            List<Wynik> lw = new List<Wynik>();

            foreach (string p in s.Where(x => !string.IsNullOrEmpty(x)))
            {
                lw.Add(new Wynik(p.Split(',')));
            }
            int i = 0;

            foreach (Wynik w in lw.OrderByDescending(x => x.Score))
            {
                i++;
                dataGridView1.Rows.Add(i, w.Score, w.Time, w.Moves);
            }
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);
        }

        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }
    }
}
