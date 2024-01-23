using BattleMethod;
namespace GAmeTestTest
{
    [TestClass]
    public class BattleTest
    {
        [TestMethod]
        public void randomOrder()
        {
            //arrange
            int[] init = { 0, 1, 2, 3, },
                cop = init;
            //act
            Battle.RandomOrder(init,cop);

            //assert
            Assert.AreEqual(4, cop.Length);

        }
    }
}