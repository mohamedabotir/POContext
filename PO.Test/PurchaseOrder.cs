using Domain.Entity;
using Domain.ValueObject;
using FluentAssertions;

namespace PO.Test;
[TestFixture]
public class PurchaseOrder
{
    private readonly QuantityTest _quantityTest = new QuantityTest();

    [Test]
    public void CreatePurchaseOrder_Failed_DueTo_totalAmount()
    {
        var invalidAmount = -1;

        Action act = () => new PoEntity(invalidAmount);

        act.Should().Throw<ArgumentException>()
            .WithMessage($"totalAmount");
    }
}