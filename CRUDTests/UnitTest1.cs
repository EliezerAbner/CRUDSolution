namespace CRUDTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        //Arrange
        MyMath mm = new MyMath();
        int input1 = 10, input2 = 5;
        int expected = input1 + input2;
        
        //Act
        int actual = mm.Add(input1, input2);
        
        //Assert
        Assert.Equal(expected, actual);
    }
}


// There's 3 steps for a unit Test:
// Arrange  -> declaration of variables, collect inputs 
// Act      -> call the method which you want to test
// Assert   -> compare the expected value with the actual value