using DataHandlerLibrary.Services;
using EntityFrameworkDatabaseLibrary.Models;
using EposRetail.Models;

public class CheckoutService
{
    private readonly ProductServices _productServices;
    private readonly SalesTransactionServices _salesTransactionServices;
    private readonly GeneralServices _generalServices;

    public CheckoutService(ProductServices productServices,
                          SalesTransactionServices salesTransactionServices,
                          GeneralServices generalServices)
    {
        _productServices = productServices;
        _salesTransactionServices = salesTransactionServices;
        _generalServices = generalServices;
    }

    public async Task<Product?> GetProductByBarcodeAsync(string barcode)
    {
        return await _productServices.GetProductByBarcode(barcode);
    }

    public async Task SaveTransactionAsync(SalesTransaction transaction)
    {
        await _salesTransactionServices.AddAsync(transaction);
    }

    public decimal CalculateGrandTotal(SalesBasket basket)
    {
        if (basket?.SalesItemsList?.Count == 0) return 0;

        var grandTotal = basket.Transaction.SaleTransaction_Total_Amount;
        var totalPaid = basket.Transaction.SaleTransaction_Cash + basket.Transaction.SaleTransaction_Card;
        return grandTotal - totalPaid;
    }

    public void AddProductToBasket(SalesBasket basket, Product product, bool isGeneric)
    {
        basket.SalesItemsList ??= new List<SalesItemTransaction>();

        var existingItem = basket.SalesItemsList.FirstOrDefault(x => x.Product_ID == product.Product_ID);

        if (existingItem != null && !isGeneric)
        {
            existingItem.Product_QTY += 1;
            existingItem.Product_Total_Amount = existingItem.Product_QTY * product.Product_Selling_Price;
            existingItem.Product_Total_Amount_Before_Discount = existingItem.Product_Total_Amount;

            if (basket.Transaction.Is_Refund)
            {
                existingItem.Product_Total_Amount = -existingItem.Product_Total_Amount;
                existingItem.Product_Total_Amount_Before_Discount = -existingItem.Product_Total_Amount_Before_Discount;
            }


        }
        else
        {         
            basket.SalesItemsList.Add(new SalesItemTransaction
            {
                Product_ID = product.Product_ID,
                Product = product,
                Product_QTY = 1,
                Product_Amount = product.Product_Selling_Price,
                Product_Total_Amount = basket.Transaction.Is_Refund ? -product.Product_Selling_Price : product.Product_Selling_Price,
                Product_Total_Amount_Before_Discount = basket.Transaction.Is_Refund ? - product.Product_Selling_Price :  product.Product_Selling_Price
            });
        }

        basket.Transaction.SaleTransaction_Total_Amount = basket.SalesItemsList.Sum(s => s.Product_Total_Amount);
    }
}