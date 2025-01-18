using Application.UseCases;
using Application.UseCases.PO;
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
public class PurchaseOrderApproveUseCaseTest
{
    Mock<IPurchaseOrderRepository> mockPurchaseOrderRepository;
    Mock<IUnitOfWork> mockUnitOfWork;

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
        mockUnitOfWork = new Mock<IUnitOfWork>();
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
    public  void ApproveOrder_Success_DueTo_OrderOnCreatedStage()
    {
       
        var successResultOfPo =Result.Ok(new PoEntity(Guid.NewGuid(),customerUser.Value,supplierUser.Value, poNumber.Value,PurchaseOrderStage.Created ));
        mockPurchaseOrderRepository.Setup(e => e.UpdatePoStageAsync(successResultOfPo.Value.Guid,
            successResultOfPo.Value.PurchaseOrderStage));
        mockPurchaseOrderRepository.SetupGet(e => e.GetPoByPurchaseNumberAsync(It.IsAny<string>()).Result)
            .Returns(successResultOfPo);
        mockPurchaseOrderRepository.Setup(e => e.UpdatePoStageAsync(successResultOfPo.Value.Guid,
            successResultOfPo.Value.PurchaseOrderStage));
        mockUnitOfWork.Setup(e => e.SaveChangesAsync(It.IsAny<IEnumerable<DomainEventBase>>(),
            It.IsAny<CancellationToken>()));
        
        var purchaseOrder = new PurchaseOrderApproveUseCase(mockUnitOfWork.Object,mockPurchaseOrderRepository.Object);
        var approvedEvent = new PurchaseOrderApproved(successResultOfPo.Value.Guid, poNumber.Value.PoNumberValue,
            ActivationStatus.Active,
            Money.CreateInstance(150M).Value, customerUser.Value.Name,
            customerUser.Value.Address.AddressValue, customerUser.Value.PhoneNumber, PurchaseOrderStage.Created);
        var act =  purchaseOrder.Approve(successResultOfPo.Value.PoNumber.PoNumberValue,default).Result;

        act.IsFailure.Should().Be(false);
    }
    [Test]
    public  void ApproveOrder_Failed_DueTo_OrderNotOnCreatedStage()
    {
       
        var successResultOfPo =Result.Ok(new PoEntity(Guid.NewGuid(),customerUser.Value,supplierUser.Value, poNumber.Value,PurchaseOrderStage.BeingShipped ));
        mockPurchaseOrderRepository.Setup(e => e.UpdatePoStageAsync(successResultOfPo.Value.Guid,
            successResultOfPo.Value.PurchaseOrderStage));
        mockPurchaseOrderRepository.SetupGet(e => e.GetPoByPurchaseNumberAsync(It.IsAny<string>()).Result)
            .Returns(successResultOfPo);
        mockPurchaseOrderRepository.Setup(e => e.UpdatePoStageAsync(successResultOfPo.Value.Guid,
            successResultOfPo.Value.PurchaseOrderStage));
        mockUnitOfWork.Setup(e => e.SaveChangesAsync(It.IsAny<IEnumerable<DomainEventBase>>(),
            It.IsAny<CancellationToken>()));
        
        var purchaseOrder = new PurchaseOrderApproveUseCase(mockUnitOfWork.Object,mockPurchaseOrderRepository.Object);
        var approvedEvent = new PurchaseOrderApproved(successResultOfPo.Value.Guid, poNumber.Value.PoNumberValue,
            ActivationStatus.Active,
            Money.CreateInstance(150M).Value, customerUser.Value.Name,
            customerUser.Value.Address.AddressValue, customerUser.Value.PhoneNumber, PurchaseOrderStage.Created);
        var act =  purchaseOrder.Approve(successResultOfPo.Value.PoNumber.PoNumberValue,default).Result;

        act.IsFailure.Should().Be(true);
    }
}