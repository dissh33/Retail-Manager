using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Library.Api;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private int _itemQuantity = 1;
        private readonly IProductEndpoint _productEndpoint;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productList);
        }

        public BindingList<ProductModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<CartItemModel> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public int ItemQuantity
        {
            get => _itemQuantity;
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private ProductModel _selectedProduct;

        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        public string SubTotal
        {
            get
            {
                decimal subTotal = CalculateSubTotal();
                return subTotal.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }

        public string Discount
        {
            get
            {
                decimal discountAmount = CalculateDiscount();
                return discountAmount.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() - CalculateDiscount();
                return total.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            }
        }

        private decimal CalculateSubTotal()
        {
            return Cart.Sum(item => item.Product.RetailPrice * item.QuantityInCart);
        }

        private decimal CalculateDiscount()
        {
            decimal subTotal = CalculateSubTotal();
            if (subTotal > 50M)
            {
                return subTotal * 0.2M;
            }
            else if (subTotal > 25M)
            {
                return subTotal * 0.1M;
            }
            else if (subTotal > 10M)
            {
                return subTotal * 0.05M;
            }
            else
            {
                return 0;
            }
        }
    

    public bool CanAddToCart
        {
            get
            {
                bool output = ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;

                return output;
            }
        }
        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;

                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Discount);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                //Make sure something is selected
                //Make sure there is an item quantity

                return output;
            }
        }
        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Discount);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                //Make sure something is in the cart

                return output;
            }
        }
        public void CheckOut()
        {

        }

    }
}
