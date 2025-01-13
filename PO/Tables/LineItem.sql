USE PurchaseOrder;
CREATE TABLE [dbo].[LineItem]
(
  [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  [Guid] UNIQUEIDENTIFIER NOT NULL UNIQUE,
  [QuantityValue] INT NOT NULL ,
  [QuantityType] INT NOT NULL,
  [PurchaseOrderId] INT NOT NULL,
  [Name] VARCHAR(50) NOT NULL,
  [Price] DECIMAL(8,2) NOT NULL,
  [SKU] VARCHAR(20) NOT NULL
  CONSTRAINT FK_CONSTRAINT_PurchaseOrder FOREIGN KEY(PurchaseOrderId) REFERENCES PurchaseOrder(Id)
  ON DELETE CASCADE,
  CONSTRAINT UQ_LineItem_SKU UNIQUE (PurchaseOrderId, SKU)
)
