USE PurchaseOrder;
CREATE TABLE [dbo].[PurchaseOrder]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Guid] UNIQUEIDENTIFIER NOT NULL,
    [PoNumber] NVARCHAR(100) NOT NULL unique,
    [TotalAmount] DECIMAL(18, 2) NOT NULL,
    [CustomerEmail] VARCHAR(25) NOT NULL,
    [CustomerName] VARCHAR(50) NOT NULL,
    [CustomerPhoneNumber] VARCHAR(15) NOT NULL,
    [SupplierEmail] VARCHAR(25) NOT NULL,
    [SupplierName] VARCHAR(50) NOT NULL,
    [SupplierPhoneNumber] VARCHAR(15) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 0,
    [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
    [ModifiedOn] DATETIME NOT NULL DEFAULT GETDATE(),
);
