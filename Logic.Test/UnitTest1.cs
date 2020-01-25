using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pasjans_Pająk;
using Logic;
using System.Linq;

namespace Logic.Test
{
    [TestClass]
    public class LogicTests
    {
        MainWindow w = new MainWindow();

        
        Card c = new Card(1, CardType.Clubs);
        

        [TestMethod]
        public void GetFreeBaseStack()
        {
            Stack s = LogicResources.Stacks.Where(x => x.Type == StackType.Base && x.Cards.Count() == 0).First();
            Assert.AreEqual(s, Functions.GetFreeBaseStack());
        }

        [TestMethod]
        public void GetElementByName()
        {
            Assert.AreEqual(LogicResources.Stacks.Where(x => x.Name == "Base1").First(), LogicResources.Stacks.GetElementByName(StackNames.Base1));
            
        }
        [TestMethod]
        public void CheckIfCanMove()
        {
            Stack s = LogicResources.Stacks.First();
            Assert.IsFalse(s.CheckIfCanMove(c));
        }
    }
}
