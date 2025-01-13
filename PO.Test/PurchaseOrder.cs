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
    private PoNumber _poNumber;
    [SetUp]
   public void Oninit()
    {
        var customerMail =Email.CreateInstance("cusomter@example.com");
        var supplierMail =Email.CreateInstance("suppier@example.com");
         _customer = User.CreateInstance(customerMail.Value, "customer", "").Value;
        _supplier = User.CreateInstance(supplierMail.Value, "supplier", "").Value;
        _poNumber = PoNumber.CreateInstance(NumberGenerator.PoAndyymmdd).Value;
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
        Action act = () => new PoEntity(rootGuid , _customer , _supplier,_poNumber);
        act.Should().Throw<ArgumentException>();
    }
    [Test]
    public void CreatePurchaseOrder_Success_DueTo_ValidParameters()
    {
        var money = Money.CreateInstance(100);
         var rootGuid = Guid.NewGuid();
        Action act = () => new PoEntity(rootGuid,_customer, _supplier,_poNumber);
        act.Should().NotThrow();
    }
    [Test]
    public void  AddItemLineForPurchaseOrder_Failed_DueTo_Duplication()
    {
        var PoGuid = Guid.NewGuid(); 
        var money = Money.CreateInstance(100);

        var PoEntity = new PoEntity(PoGuid,_customer, _supplier,_poNumber);
        var item = new Item("panadol", 25M, "123");
       var result =   PoEntity.AddLineItems(new List<LineItem>()
        {
            new LineItem(Quantity.Tab, item,Guid.NewGuid(),0),
            new LineItem(Quantity.Tab, item,Guid.NewGuid(),0)
        });
        result.Message.Should().Be("Item already added");
    }
    [Test]
    public void  AddItemLineForPurchaseOrder_Success_DueTo_ValidParameters()
    {
        
        var PoGuid = Guid.NewGuid();
        var money = Money.CreateInstance(15);

        var PoEntity = new PoEntity(PoGuid,_customer, _supplier,_poNumber);
        var item1 = new Item("panadol", 25M, "123");
        var item2 = new Item("panadol", 40M, "124");
         
        var result =  PoEntity.AddLineItems(new List<LineItem>()
        {
            new LineItem(Quantity.Tab, item1,Guid.NewGuid(),0),
            new LineItem(Quantity.Tab, item2,Guid.NewGuid(),0)
        });

        result.IsFailure.Should().Be(false);
    }
}