CREATE PROCEDURE [dbo].[spSaleDetail_Insert]
	@SaleId int,
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@Discount money
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.SaleDetail(SaleId,ProductId,Quantity,PurchasePrice,Discount)
	VALUES (@SaleId,@ProductId,@Quantity,@PurchasePrice,@Discount)
END 