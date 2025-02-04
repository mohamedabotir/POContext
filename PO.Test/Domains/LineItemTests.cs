using Common.Entity;
using Common.ValueObject;
using Domain.Entity;
using FluentAssertions;

namespace PO.Test;
[TestFixture]
public class LineItemTests
{
    [Test]
    public void CreateLineItem_Failed_DueTo_NullQuantity()
    {
        Quantity quantity  = null;
        Item purchaseOrderItem = new Item("Panadol",500,"asdssada");
        Action act = () => new LineItem(quantity,purchaseOrderItem,Guid.NewGuid(),0,0);

        act.Should().Throw<ArgumentNullException>();

    }
}