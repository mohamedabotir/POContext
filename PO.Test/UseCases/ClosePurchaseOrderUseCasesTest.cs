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
}