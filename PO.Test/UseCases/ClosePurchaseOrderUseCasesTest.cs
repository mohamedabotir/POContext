using Application.UseCases;
using Common.Constants;
using Common.Entity;
using Common.Events;
using Common.Repository;
using Common.Result;
using Common.Utils;
using Common.ValueObject;
using FluentAssertions;
using Moq;

namespace PO.Test.UseCases;
[TestFixture]
public class ClosePurchaseOrderUseCasesTest
{
    Mock<IPurchaseOrderRepository> mockPurchaseOrderRepository;
    private Result<Email> customerEmail;
    private Result<Email> supplierEmail;
    private Result<Address> userLocation;
    private Result<User> customerUser;
    private Result<User> supplierUser;
    private Result<PoNumber> poNumber;

    [SetUp]
    public void Setup()
    {
        mockPurchaseOrderRepository = new Mock<IPurchaseOrderRepository>();
          customerEmail = Email.CreateInstance("customer@email.com");
          supplierEmail = Email.CreateInstance("supplier@email.com");
          userLocation = Address.CreateInstance("El Maadi ndgsfdasdaddfg");
          customerUser = User.CreateInstance(customerEmail.Value, "Customer"
            , "0154651321564",userLocation.Value);
          supplierUser = User.CreateInstance(supplierEmail.Value, "supplier"
            , "12154155454",userLocation.Value);
          poNumber = PoNumber.CreateInstance(NumberGenerator.PoAndyymmdd);  
    }

    [Test]
    public  void ClosePurchaseOrder_Success_DueTo_OrderOnShipped()
    {
       
        //var successResultOfPo =Result.Ok(new PoEntity(Guid.NewGuid(),customerUser.Value,supplierUser.Value, poNumber.Value,PurchaseOrderStage.Shipped ));
      
        //mockPurchaseOrderRepository.SetupGet(e => e.GetPoByPurchaseNumberWithFactoryAsync(It.IsAny<string>()).Result)
        //    .Returns(successResultOfPo);
        //mockPurchaseOrderRepository.Setup(e => e.UpdatePoStageWithFactoryAsync(successResultOfPo.Value.Guid,
        //    successResultOfPo.Value.PurchaseOrderStage));
        
        //var purchaseOrder = new PurchaseOrderClosed(mockPurchaseOrderRepository.Object);
        
        //var act =  purchaseOrder.ClosePurchaseOrder(new OrderClosed(successResultOfPo.Value.Guid,poNumber.Value.PoNumberValue)).Result;

        //act.IsFailure.Should().Be(false);
    }
    [Test]
    public  void ClosePurchaseOrder_Failed_DueTo_NotOrderOnShipped()
    {
       
        //var successResultOfPo =Result.Ok(new PoEntity(Guid.NewGuid(),customerUser.Value,supplierUser.Value, poNumber.Value,PurchaseOrderStage.BeingShipped ));
      
        //mockPurchaseOrderRepository.SetupGet(e => e.GetPoByPurchaseNumberWithFactoryAsync(It.IsAny<string>()).Result)
        //    .Returns(successResultOfPo);
        //mockPurchaseOrderRepository.Setup(e => e.UpdatePoStageWithFactoryAsync(successResultOfPo.Value.Guid,
        //    successResultOfPo.Value.PurchaseOrderStage));
        
        //var purchaseOrder = new PurchaseOrderClosed(mockPurchaseOrderRepository.Object);
        
        //var act =  purchaseOrder.ClosePurchaseOrder(new OrderClosed(successResultOfPo.Value.Guid,poNumber.Value.PoNumberValue)).Result;

        //act.IsFailure.Should().Be(true);
    }
}