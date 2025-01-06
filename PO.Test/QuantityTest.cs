using Domain.ValueObject;
using FluentAssertions;

namespace PO.Test;

public class QuantityTest
{
    
    [TestCase(0)]
    [TestCase(-1)]
    [Test]
    public void CreateTabQuantity_Failed_DueTo_MultiplierEqualOrLessThanZero(int multiplier)
    {
        var tab = Quantity.Tab;
        Action act = () =>
        {
            var operation = tab * multiplier;
        };

        act.Should().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void CreateTabQuantity_Success_MultiplyBy5()
    {
        var tab = Quantity.Tab;
        int act = (tab * 5).QuantityValue;
        act.Should().Be(500);
    }
}