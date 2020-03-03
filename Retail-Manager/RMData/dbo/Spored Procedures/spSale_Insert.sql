CREATE PROCEDURE [dbo].[spSale_Insert]
	@Id int output,
	@CashierId nvarchar(128),
	@SaleDate datetime2,
	@SubTotal money,
	@Discount money,
	@Total money
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.Sale(CashierId,SaleDate,SubTotal,Discount,Total)
	VALUES (@CashierId,@SaleDate,@SubTotal,@Discount,@Total)

END 
