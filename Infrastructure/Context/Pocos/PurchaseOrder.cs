using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Constants;
using Common.Result;
using Common.Entity;
using Common.ValueObject;

namespace Application.Context.Pocos;
[Table("PurchaseOrder")]
    public class PurchaseOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required] 
        [StringLength(100)] 
        public  string PoNumber { get; protected set; }
        [Required] 
        [StringLength(100)] 
        public  Guid Guid { get; protected set; }

        [Required] 
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal TotalAmount { get; set; }

        [Required] 
        [StringLength(25)] 
        [EmailAddress] 
        public string CustomerEmail { get; set; }

        [Required] 
        [StringLength(50)] 
        public string CustomerName { get; set; }

        [Required]
        [StringLength(15)] 
        public string CustomerPhoneNumber { get; set; }
        [Required]
        [StringLength(10)] 
        public string CustomerLocation { get; set; }

        [Required] 
        [StringLength(25)] 
        [EmailAddress] 
        public string SupplierEmail { get; set; }

        [Required] 
        [StringLength(50)] 
        public string SupplierName { get; set; }

        [Required] 
        [StringLength(15)]
        public string SupplierPhoneNumber { get; set; }

        [Required] 
        [Column(name:"IsActive")]
        public  ActivationStatus ActivationStatus { get;protected set; }

        [Required] 
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public  DateTime? CreatedOn { get; protected set; }

        [Required] 
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] 
        public  DateTime? ModifiedOn { get;protected set; }
        
        [Required] 
        public PurchaseOrderStage OrderStage { get;  set; }
        public  virtual ICollection<LineItems> LineItems { get; protected set; }

        public void MapPoEntityToPurchaseOrder(PoEntity purchaseEntity)
        {
            LineItems = new List<LineItems>();
            CustomerPhoneNumber = purchaseEntity.Customer.PhoneNumber;
            CustomerEmail = purchaseEntity.Customer.Email.EmailValue;
            CustomerName = purchaseEntity.Customer.Name;
            SupplierPhoneNumber = purchaseEntity.Supplier.PhoneNumber;
            SupplierEmail = purchaseEntity.Supplier.Email.EmailValue;
            Guid = purchaseEntity.Guid;
            SupplierName = purchaseEntity.Supplier.Name;
            SupplierPhoneNumber = purchaseEntity.Supplier.PhoneNumber;
            PoNumber = purchaseEntity.PoNumber.PoNumberValue;
            TotalAmount = purchaseEntity.TotalAmount.MoneyValue;
            ActivationStatus = purchaseEntity.ActivationStatus;
            CreatedOn = purchaseEntity.CreatedOn;
            ModifiedOn = purchaseEntity.ModifiedOn;
            CustomerLocation = purchaseEntity.Customer.Address.AddressValue;
            OrderStage = purchaseEntity.PurchaseOrderStage;
            var lineItems = purchaseEntity.LineItems.Select(new LineItems().MapItemLineToLineItemPoco).ToList();
            foreach (var lineItem in lineItems)
            {
                LineItems.Add(lineItem);
            }
        }

       

        public Result<PoEntity> GetPoEntity()
        {
            var money = Money.CreateInstance(TotalAmount);
            var customerEmail = Email.CreateInstance(CustomerEmail);
            var supplierEmail = Email.CreateInstance(SupplierEmail);
            var customerAddress = Address.CreateInstance(CustomerLocation);

            var validations = Result.Combine(money,customerEmail,supplierEmail,customerAddress);
            if (validations.IsFailure)
                return   Result.Fail<PoEntity>(validations.Message);
            
            var customerUser = User.CreateInstance(customerEmail.Value, CustomerName
                , CustomerPhoneNumber,customerAddress.Value);
            var supplierUser = User.CreateInstance(supplierEmail.Value, SupplierName
                , SupplierPhoneNumber,customerAddress.Value);
            var poNumber = Common.Utils.PoNumber.SetPoNumber(PoNumber);
            validations = Result.Combine(money,customerUser,supplierUser,poNumber);
            if (validations.IsFailure)
                return   Result.Fail<PoEntity>(validations.Message);
            var purchaseOrder = new PoEntity( Guid,
                customerUser.Value, supplierUser.Value,poNumber.Value,OrderStage);
            var lineItems = LineItems.Select(e=>e.MapLineItemPocoToItemLine()).ToList();
            var settingLineItemsResult = purchaseOrder.SetLineItems(lineItems);
            if(supplierEmail.IsFailure)
                return Result.Fail<PoEntity>(settingLineItemsResult.Message);
            return Result.Ok(purchaseOrder);
        }
    }

