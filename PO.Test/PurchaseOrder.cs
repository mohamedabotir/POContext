using Domain.DomainEvents;
using Domain.Entity;
using Domain.Handlers;
using Domain.ValueObject;
using FluentAssertions;
using Infrastructure.Extensions;
using Moq;

namespace PO.Test;
[TestFixture]
public class PurchaseOrder
{
    private User _customer;
    private User _supplier;
    [SetUp]
   public void Oninit()
    {
        _customer = new User("cusomter@example.com", "customer","+20000000000");
        _supplier = new User("suppier@example.com", "suppier","+20000000000");

    }

    [TestCase(-1,"c2a987f6-54d7-4e1b-a624-24be50544cfb")]
    [TestCase(5,"00000000-0000-0000-0000-000000000000")]
    [Test]
    public void CreatePurchaseOrder_Failed_DueTo_invalidParameters(decimal totalAmount,Guid rootGuid)
    {
        Action act = () => new PoEntity(totalAmount,rootGuid , _customer , _supplier);
        act.Should().Throw<ArgumentException>();
    }
    [Test]
    public void CreatePurchaseOrder_Success_DueTo_ValidParameters()
    {
         var totalAmount = 100;
         var rootGuid = Guid.NewGuid();
        Action act = () => new PoEntity(totalAmount,rootGuid,_customer, _supplier);
        act.Should().NotThrow();
    }
    [Test]
    public void  AddItemLineForPurchaseOrder_Failed_DueTo_Duplication()
    {
        var PoGuid = Guid.NewGuid(); 
        var PoEntity = new PoEntity(15,PoGuid,_customer, _supplier);
        var item = new Item("panadol", 25M, "123");
       var result =   PoEntity.AddLineItems(new List<LineItem>()
        {
            new LineItem(Quantity.Tab, item),
            new LineItem(Quantity.Tab, item)
        });
        result.Error.Should().Be("Item already added");
    }
    [Test]
    public void  AddItemLineForPurchaseOrder_Success_DueTo_ValidParameters()
    {
        
        var PoGuid = Guid.NewGuid();
        var PoEntity = new PoEntity(15,PoGuid,_customer, _supplier);
        var item1 = new Item("panadol", 25M, "123");
        var item2 = new Item("panadol", 40M, "124");
         
        var result =  PoEntity.AddLineItems(new List<LineItem>()
        {
            new LineItem(Quantity.Tab, item1),
            new LineItem(Quantity.Tab, item2)
        });

        result.IsFailure.Should().Be(false);
    }
}