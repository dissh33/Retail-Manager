using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Caliburn.Micro;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Models;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        
        private int _itemQuantity = 1;
        private ProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;

        private readonly IProductEndpoint _productEndpoint;
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IMapper _mapper;

        public SalesViewModel(IProductEndpoint productEndpoint, ISaleEndpoint saleEndpoint, IMapper mapper)
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        public BindingList<ProductDisplayModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<CartItemDisplayModel> Cart
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

        
        public ProductDisplayModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        public CartItemDisplayModel SelectedCartItem 
        {
            get => _selectedCartItem; 
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
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
                return subTotal * 0.15M;
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
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;

                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
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
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0;

                return output;
            }
        }
        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += SelectedCartItem.QuantityInCart;
            Cart.Remove(SelectedCartItem);

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Discount);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = Cart.Count > 0;

                return output;
            }
        }
        public async Task CheckOut()
        {
            SaleModel sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            sale.Discount = CalculateDiscount();

            await _saleEndpoint.PostSale(sale);

            await ResetSalesViewModel();
        }

        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            
            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Discount);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

    }
}
