using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsefullAlgorightms.Parsing.ExpressionParsing;
using UsefullAlgorithms.Parsing.ExpressionParsing;

namespace UsefullAlgorithms.Tests
{
    [TestClass]
    public class ShuttingYardTests
    {
        [TestMethod]
        public void TestCorrectConvertion_ShouldPass()
        {
            NumericExpressionInfixToPostfix evauator = new NumericExpressionInfixToPostfix();

            var vals = evauator.Parse("( 1 + 2 ) * ( 3 / 4 ) ^ ( 5 + 6 )");
            
            Assert.AreEqual("1", vals[0]);
            Assert.AreEqual("2", vals[1]);
            Assert.AreEqual("+", vals[2]);
            Assert.AreEqual("3", vals[3]);
            Assert.AreEqual("4", vals[4]);
            Assert.AreEqual("/", vals[5]);
            Assert.AreEqual("5", vals[6]);
            Assert.AreEqual("6", vals[7]);
            Assert.AreEqual("+", vals[8]);
            Assert.AreEqual("^", vals[9]);
            Assert.AreEqual("*", vals[10]);

            vals = evauator.Parse("1 + 2 * 4");

            Assert.AreEqual("1", vals[0]);
            Assert.AreEqual("2", vals[1]);
            Assert.AreEqual("+", vals[2]);
            Assert.AreEqual("4", vals[3]);
            Assert.AreEqual("*", vals[4]);
        }

        [TestMethod]
        public void TestBooleanConvertion_ShouldPass()
        {
            BooleanExpressionInfixToPostfix converter = new BooleanExpressionInfixToPostfix();

            var vals = converter.Parse("1 and 2 or not 3");

            Assert.AreEqual("1", vals[0]);
            Assert.AreEqual("2", vals[1]);
            Assert.AreEqual("and", vals[2]);
            Assert.AreEqual("3", vals[3]);
            Assert.AreEqual("not", vals[4]);
            Assert.AreEqual("or", vals[5]);
        }

        [TestMethod]
        public void TestFunctionWithSimpleArgs_ShouldPass()
        {
            BooleanExpressionInfixToPostfix converter = new BooleanExpressionInfixToPostfix();

            var tokens = converter.Parse("1 and test ( )");

            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("vararg_0", tokens[1]);
            Assert.AreEqual("test", tokens[2]);
            Assert.AreEqual("and", tokens[3]);

            tokens = converter.Parse("1 and test ( 1 , 2 , 3 )");

            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("1", tokens[1]);
            Assert.AreEqual("2", tokens[2]);
            Assert.AreEqual("3", tokens[3]);
            Assert.AreEqual("vararg_3", tokens[4]);
            Assert.AreEqual("test", tokens[5]);
            Assert.AreEqual("and", tokens[6]);
        }

        [TestMethod]
        public void TestFunctionCall_ShouldPass()
        {
            BooleanExpressionInfixToPostfix converter = new BooleanExpressionInfixToPostfix();

            var tokens = converter.Parse("p ( 2 and 4 , ( 1 or 3 ) and 5 )");

            Assert.AreEqual("2", tokens[0]);
            Assert.AreEqual("4", tokens[1]);
            Assert.AreEqual("and", tokens[2]);
            Assert.AreEqual("1", tokens[3]);
            Assert.AreEqual("3", tokens[4]);
            Assert.AreEqual("or", tokens[5]);
            Assert.AreEqual("5", tokens[6]);
            Assert.AreEqual("and", tokens[7]);
            Assert.AreEqual("vararg_2", tokens[8]);
            Assert.AreEqual("p", tokens[9]);
        }

        [TestMethod]
        public void TestFunctionCall_NestedArgs_ShouldPass()
        {
            BooleanExpressionInfixToPostfix converter = new BooleanExpressionInfixToPostfix();

            var tokens = converter.Parse("p ( 2 and 4 , r ( 1 ) , t ( 3 , 2 ) )");

            Assert.AreEqual("2", tokens[0]);
            Assert.AreEqual("4", tokens[1]);
            Assert.AreEqual("and", tokens[2]);
            Assert.AreEqual("1", tokens[3]);
            Assert.AreEqual("vararg_1", tokens[4]);
            Assert.AreEqual("r", tokens[5]);
            Assert.AreEqual("3", tokens[6]);
            Assert.AreEqual("2", tokens[7]);
            Assert.AreEqual("vararg_2", tokens[8]);
            Assert.AreEqual("t", tokens[9]);
            Assert.AreEqual("vararg_3", tokens[10]);
            Assert.AreEqual("p", tokens[11]);
        }

        [TestMethod]
        public void TestAnonymousCommaBlock_ShouldPass()
        {
            BooleanExpressionInfixToPostfix converter = new BooleanExpressionInfixToPostfix();

            var tokens = converter.Parse("( 1 , 2 , ( 3 and 4 ) )");

            Assert.AreEqual("1", tokens[0]);
            Assert.AreEqual("2", tokens[1]);
            Assert.AreEqual("3", tokens[2]);
            Assert.AreEqual("4", tokens[3]);
            Assert.AreEqual("and", tokens[4]);
            Assert.AreEqual("vararg_3", tokens[5]);
        }

        [TestMethod]
        public void TestFunctionCallAsArgument_ShouldPass()
        {
            BooleanExpressionInfixToPostfix converter = new BooleanExpressionInfixToPostfix();

            var tokens = converter.Parse("f ( p ( ) )");

            Assert.AreEqual("vararg_0", tokens[0]);
            Assert.AreEqual("p", tokens[1]);
            Assert.AreEqual("vararg_1", tokens[2]);
            Assert.AreEqual("f", tokens[3]);
            
            tokens = converter.Parse("f ( p ( 3 ) )");

            Assert.AreEqual("3", tokens[0]);
            Assert.AreEqual("vararg_1", tokens[1]);
            Assert.AreEqual("p", tokens[2]);
            Assert.AreEqual("vararg_1", tokens[3]);
            Assert.AreEqual("f", tokens[4]);

            tokens = converter.Parse("f ( p ( 3 ) and 5 , 1 )");
            
            Assert.AreEqual("3", tokens[0]);
            Assert.AreEqual("vararg_1", tokens[1]);
            Assert.AreEqual("p", tokens[2]);
            Assert.AreEqual("5", tokens[3]);
            Assert.AreEqual("and", tokens[4]);
            Assert.AreEqual("1", tokens[5]);
            Assert.AreEqual("vararg_2", tokens[6]);
            Assert.AreEqual("f", tokens[7]);
        }
    }
}
