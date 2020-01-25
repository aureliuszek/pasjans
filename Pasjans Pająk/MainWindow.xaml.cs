using Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = System.Windows.Controls.Image;

namespace Pasjans_Pająk
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetUpResources();
            Functions.ChangeBackgroundImage(Functions.GetAllControlsOfType<Image>(Main), LogicResources.Frame.ToBitmapImage());
            SetUpStacks();
        }


        private void SetUpStacks()
        {
            LogicResources.Stacks.Add(new Stack("Reserve", StackType.Reserve, ReserveStack));

            LogicResources.Stacks.Add(new Stack("Base1", StackType.Base, BaseStack1));
            LogicResources.Stacks.Add(new Stack("Base2", StackType.Base, BaseStack2));
            LogicResources.Stacks.Add(new Stack("Base3", StackType.Base, BaseStack3));
            LogicResources.Stacks.Add(new Stack("Base4", StackType.Base, BaseStack4));
            LogicResources.Stacks.Add(new Stack("Base5", StackType.Base, BaseStack5));
            LogicResources.Stacks.Add(new Stack("Base6", StackType.Base, BaseStack6));
            LogicResources.Stacks.Add(new Stack("Base7", StackType.Base, BaseStack7));
            LogicResources.Stacks.Add(new Stack("Base8", StackType.Base, BaseStack8));

            LogicResources.Stacks.Add(new Stack("Main1", StackType.Main, MainStack1));
            LogicResources.Stacks.Add(new Stack("Main2", StackType.Main, MainStack2));
            LogicResources.Stacks.Add(new Stack("Main3", StackType.Main, MainStack3));
            LogicResources.Stacks.Add(new Stack("Main4", StackType.Main, MainStack4));
            LogicResources.Stacks.Add(new Stack("Main5", StackType.Main, MainStack5));
            LogicResources.Stacks.Add(new Stack("Main6", StackType.Main, MainStack6));
            LogicResources.Stacks.Add(new Stack("Main7", StackType.Main, MainStack7));
            LogicResources.Stacks.Add(new Stack("Main8", StackType.Main, MainStack8));
            LogicResources.Stacks.Add(new Stack("Main9", StackType.Main, MainStack9));
            LogicResources.Stacks.Add(new Stack("Main10", StackType.Main, MainStack10));
        }

        private void SetUpResources()
        {
            LogicResources.Back = Properties.Resources.back;
            LogicResources.Frame = Properties.Resources.frame;
            LogicResources.Deck = Properties.Resources.deck;
            LogicResources.CardWidth = ReserveStack.Width;
            LogicResources.CardHeight = ReserveStack.Height;
            LogicResources.Main = Main;
            LogicResources.MainWindow = this;
            LogicResources.Difficulty = Difficulty.Easy;
            LogicResources.DeckColor = CardType.Clubs;
        }

        

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        public void StartNewGame()
        {
            if ((bool)R10.IsChecked)
                LogicResources.DeckColor = Functions.RandomDeckColor();

            LogicResources.Stacks.ClearStacks();
            List<Card> cards = CardGlobalFunctions.GenerateCard();
            LogicResources.Stacks.SetUpCardsToStacks(cards);
        }

        private void Main_MouseEnter(object sender, MouseEventArgs e)
        {
            Functions.GetBackPickedCardToPosition();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as RadioButton).Tag.ToString() == "-1")
                LogicResources.DeckColor = Functions.RandomDeckColor();

            LogicResources.DeckColor = (CardType)int.Parse((sender as RadioButton).Tag.ToString());
        }

        private void Diff_Checked(object sender, RoutedEventArgs e)
        {
            LogicResources.Difficulty = (Difficulty)int.Parse((sender as RadioButton).Tag.ToString());
        }
        void Win()
        {
            Win w = new Win(this);
            w.ShowDialog();
        }
    }
}
