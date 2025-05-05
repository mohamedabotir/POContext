using Application.Commands;
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
using PO.Test.DataSources.InMemory;

namespace PO.Test.UseCases;
[TestFixture]
public class PurchaseOrderCreationUseCaseTest
{
    Mock<IPurchaseOrderRepository> mockPurchaseOrderRepository;
    Mock<IUnitOfWork<PoEntity>> mockUnitOfWork;
    Mock<IEventSourcing<PoEntity>> mockEventSourcing;


    [SetUp]
    public void Setup()
    {
        mockPurchaseOrderRepository = new Mock<IPurchaseOrderRepository>();
        mockEventSourcing = new Mock<IEventSourcing<PoEntity>>();
        mockUnitOfWork = new Mock<IUnitOfWork<PoEntity>>();
        
    }
    [Test]
    public async Task Handle_ShouldSucceed_WhenPurchaseOrderCommandIsValid()
    {
        var rootGuid = Guid.NewGuid();

        var command = new List<PurchaseOrderCommand>([PurchaseOrderDatasource.GetValidPurchaseOrderDto(rootGuid)]);

        mockPurchaseOrderRepository.Setup(x => x.IsPoExists(rootGuid)).Returns(false);
        mockPurchaseOrderRepository.Setup(x => x.AddAsync(It.IsAny<PoEntity>()));
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<PoEntity>(), It.IsAny<CancellationToken>()));

        var useCase = new PurchaseOrderCreationCreationUseCase(
            mockUnitOfWork.Object,
            mockPurchaseOrderRepository.Object
        );

        var result = await useCase.CreatePurchaseOrder(command);

        result.IsSuccess.Should().BeTrue();
    }

    [Test]
    public async Task Handle_ShouldFail_WhenPurchaseOrderAlreadyExists()
    {
        var rootGuid = Guid.NewGuid();
        var command = new List<PurchaseOrderCommand> { PurchaseOrderDatasource.GetValidPurchaseOrderDto(rootGuid) };

        mockPurchaseOrderRepository.Setup(x => x.IsPoExists(rootGuid)).Returns(true);

        var useCase = new PurchaseOrderCreationCreationUseCase(
            mockUnitOfWork.Object,
            mockPurchaseOrderRepository.Object
        );

        var result = await useCase.CreatePurchaseOrder(command);

        result.IsFailure.Should().BeTrue();
    }
    [Test]
    public async Task Handle_ShouldPartiallySucceed_WhenOneValidAndOneInvalidPo()
    {
        var validGuid = Guid.NewGuid();
        var invalidGuid = Guid.NewGuid();

        var validDto = PurchaseOrderDatasource.GetValidPurchaseOrderDto(validGuid);
        var invalidDto = PurchaseOrderDatasource.GetValidPurchaseOrderDto(invalidGuid) with
        {
            Customer = validDto.Customer with { Email = "bad" }
        };

        var command = new List<PurchaseOrderCommand> { validDto, invalidDto };

        mockPurchaseOrderRepository.Setup(x => x.IsPoExists(validGuid)).Returns(false);
        mockPurchaseOrderRepository.Setup(x => x.IsPoExists(invalidGuid)).Returns(false);
        mockPurchaseOrderRepository.Setup(x => x.AddAsync(It.IsAny<PoEntity>()));
        mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<PoEntity>(), It.IsAny<CancellationToken>()));

        var useCase = new PurchaseOrderCreationCreationUseCase(
            mockUnitOfWork.Object,
            mockPurchaseOrderRepository.Object
        );

        var result = await useCase.CreatePurchaseOrder(command);

        result.Results.Where(e=>e.IsFailure).Should().HaveCount(1);
    }
    [Test]
    [TestCase("invalid-email", "supplier@example.com", 1, "egypt,cairo", false, TestName = "Invalid customer email")]
    [TestCase("customer@example.com", "bad-email", 1, "egypt,cairo", false, TestName = "Invalid supplier email")]
    [TestCase("customer@example.com", "supplier@example.com", -5, "egypt,cairo", false, TestName = "Negative quantity")]
    [TestCase("customer@example.com", "supplier@example.com", 1, "", false, TestName = "Missing address")]
    public async Task Handle_ShouldFail_WhenPurchaseOrderInputIsInvalid(
        string customerEmail,
        string supplierEmail,
        int quantity,
        string address,
        bool expectedSuccess)
    {
        var rootGuid = Guid.NewGuid();

        var dto = new PurchaseOrderCommand(
            rootGuid,
            new UserDto(customerEmail, "01061566310", "Customer"),
            new UserDto(supplierEmail, "01061566111", "Supplier"),
            new List<ItemLineDto>
            {
            new ItemLineDto(
                 quantity,QuantityType.Tab,
                "panadol",
                22,
                "SKU-123-123-123",
                Guid.NewGuid()
            )
            },
            NumberGenerator.PoAndyymmdd,
            address
        );

        mockPurchaseOrderRepository.Setup(x => x.IsPoExists(rootGuid)).Returns(false);

        var useCase = new PurchaseOrderCreationCreationUseCase(
            mockUnitOfWork.Object,
            mockPurchaseOrderRepository.Object
        );

        var result = await useCase.CreatePurchaseOrder(new List<PurchaseOrderCommand> { dto });

        result.IsSuccess.Should().Be(expectedSuccess);
    }



}