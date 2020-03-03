CREATE TABLE [dbo].[Sale]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CashierId] NVARCHAR(128) NOT NULL, 
    [SaleDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [SubTotal] MONEY NOT NULL, 
    [Discount] MONEY NULL, 
    [Total] MONEY NOT NULL, 
    CONSTRAINT [FK_Sale_ToUser] FOREIGN KEY ([CashierId]) REFERENCES [User]([Id])
)
