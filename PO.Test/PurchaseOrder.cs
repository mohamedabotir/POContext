using Domain.DomainEvents;
using Domain.Entity;
using Domain.Handlers;
using Domain.Result;
using Domain.ValueObject;
using FluentAssertions;

namespace PO.Test;
[TestFixture]
public class PurchaseOrder
{
    private User _customer;
    private User _supplier;
    [SetUp]
   public void Oninit()
    {
        var customerMail =Email.CreateInstance("cusomter@example.com");
        var supplierMail =Email.CreateInstance("suppier@example.com");
         _customer = User.CreateInstance(customerMail.Value, "customer", "").Value;
        _supplier = User.CreateInstance(supplierMail.Value, "supplier", "").Value;
    }

    [TestCase(-1,"c2a987f6-54d7-4e1b-a624-24be50544cfb")]
    [Test]
    public void CreatePurchaseOrder_Failed_DueTo_invalidAmount(decimal totalAmount,Guid rootGuid)
    {
        var money = Money.CreateInstance(totalAmount);
        money.IsFailure.Should().Be(true);
    }
    [TestCase(5,"00000000-0000-0000-0000-000000000000")]
    public void CreatePurchaseOrder_Failed_DueTo_invalidGuid(decimal totalAmount,Guid rootGuid)
    {
        var money = Money.CreateInstance(totalAmount);
        Action act = () => new PoEntity(money.Value,rootGuid , _customer , _supplier);
        act.Should().Throw<ArgumentException>();
    }
    [Test]
    public void CreatePurchaseOrder_Success_DueTo_ValidParameters()
    {
        var money = Money.CreateInstance(100);
         var rootGuid = Guid.NewGuid();
        Action act = () => new PoEntity(money.Value,rootGuid,_customer, _supplier);
        act.Should().NotThrow();
    }
    [Test]
    public void  AddItemLineForPurchaseOrder_Failed_DueTo_Duplication()
    {
        var PoGuid = Guid.NewGuid(); 
        var money = Money.CreateInstance(100);

        var PoEntity = new PoEntity(money.Value,PoGuid,_customer, _supplier);
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
        var money = Money.CreateInstance(15);

        var PoEntity = new PoEntity(money.Value,PoGuid,_customer, _supplier);
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