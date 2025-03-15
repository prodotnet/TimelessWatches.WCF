using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    DataClassesDataContext data = new DataClassesDataContext();

    public  UserRegistration Login(string email, string password)
    {

        var login = (from s in data.UserRegistrations where s.Email.Equals(email) && s.Password.Equals(password) select s).FirstOrDefault();

        if (login != null)
        {


            if (login.UserType == "Manager")
            {
                var Manager = new UserRegistration
                {
                    Id = login.Id,
                    FirstName = login.FirstName,
                    LastName = login.LastName,
                    Email = login.Email,
                    UserType = login.UserType,

                };

                return Manager;
            }
            else if (login.UserType == "Customer")
            {
                var Customer = new UserRegistration
                {
                    Id = login.Id,
                    FirstName = login.FirstName,
                    LastName = login.LastName,
                    Email = login.Email,
                    UserType = login.UserType,

                };

                return Customer;
            }



            return login;
        }
        else
        {

            return  null;
        }
    }


    public bool Register(string firstName, string lastName, string email, string password,  string gender, string userType, DateTime createDate)
    {
        

        // Checking if the user already exists
        var IsUserExit = data.UserRegistrations.FirstOrDefault(x => x.Email == email);
        if (IsUserExit != null)
        {
            return false; 
        }

        // Create a new user registration
        var newUser = new UserRegistration
        {
           
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            Gender = gender,
            UserType = userType,
            CreateDate = createDate
        };

        data.UserRegistrations.InsertOnSubmit(newUser);

        try
        {
            data.SubmitChanges(); 
            return true; 
        }
        catch (Exception)
        {
            return false; 
        }
    }


    //Method to Add Product
    public bool AddProduct(Product product)
    {
       
            
            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                ImageUrl_ = product.ImageUrl_,
                Active = 1 
            };

          
            data.Products.InsertOnSubmit(newProduct);
         
           try
           {

              data.SubmitChanges();

               return true;
           }
           catch (Exception ex)
           {
              return false;
           }
    }



    public bool UpdateProduct(Product product)
    {

        // Checking if the user already exists
        var IsProductExit = data.Products.FirstOrDefault(p => p.Id == product.Id);


        try
        {
            
            if (IsProductExit != null)
            {
                IsProductExit.Name = product.Name;
                IsProductExit.Description = product.Description;
                IsProductExit.Price = product.Price;
                IsProductExit.Category = product.Category;
                IsProductExit.ImageUrl_ = product.ImageUrl_;

                data.SubmitChanges();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }

    //Method  to get Product
    public Product GetProduct(int id)
    {
        var get = (from p in data.Products where p.Id.Equals(id) select p).FirstOrDefault();

        if (get != null)
        {
            var Prod = new Product
            {
                Id = get.Id,
                Name = get.Name,
                Description = get.Description,
                ImageUrl_ = get.ImageUrl_,
                Price = get.Price,


            };


            return Prod;
        }
        else

            return null;
    }



    //Method  to get all  Products dynamically 
    public List<Product> GetAllProducts()
    {
        dynamic Prods = new List<Product>();

        dynamic tempProds = (from p in data.Products
                             where p.Active == 1
                             select p).DefaultIfEmpty();

        if (tempProds != null)
        {
            foreach (Product p in tempProds)
            {
                var AllProds = new Product
                {
                    Id = p.Id,
                    Name = p.Name,                    
                    Description = p.Description,
                    ImageUrl_ = p.ImageUrl_,
                    Price = p.Price,                  
                    Category = p.Category,
                    Active = 1,

                };

                Prods.Add(AllProds);
            }

            return Prods;
        }
        else
        {
            return null;
        }


    }

    //method to get best sellings products
    public List<Product> GetBestSellingProducts()
    {
        dynamic Prods = new List<Product>();

        dynamic tempProds = (from p in data.Products
                             where p.Active == 2
                             select p).DefaultIfEmpty();

        if (tempProds != null)
        {
            foreach (Product p in tempProds)
            {
                var AllProds = new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl_ = p.ImageUrl_,
                    Price = p.Price,
                    Category = p.Category,
                    Active = 2,

                };

                Prods.Add(AllProds);
            }

            return Prods;
        }
        else
        {
            return null;
        }


    }

    //method to sort by catagory by name
    public List<Product> GetProductsByCategory(string category)
    {
        dynamic Prods = new List<Product>();

      
        var tempProds = (from p in data.Products
                         where p.Active == 1
                         select p).DefaultIfEmpty();

    
        if (tempProds != null && tempProds.Any())
        {
            foreach (Product p in tempProds)
            {
                // Filter products based on category
                bool categoryMatches = false;

                switch (category.ToLower())
                {
                    case "smart watches":
                        categoryMatches = p.Category == "Smart Watches";
                        break;
                    case "rolex":
                        categoryMatches = p.Category == "Rolex";
                        break;
                    case "omega":
                        categoryMatches = p.Category == "Omega";
                        break;
                    default:
                        categoryMatches = true; 
                        break;
                }

               
                if (categoryMatches)
                {
                    var AllProds = new Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        ImageUrl_ = p.ImageUrl_,
                        Price = p.Price,
                        Category = p.Category,
                        Active = 1,
                    };

                    Prods.Add(AllProds);
                }
            }

            return Prods;
        }
        else
        {
            return new List<Product>(); 
        }
    }



    //method to sort the products by price
    public List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        dynamic Prods = new List<Product>();

        // Get all active products
        var tempProds = (from p in data.Products
                         where p.Active == 1
                         select p).DefaultIfEmpty();

       
        if (tempProds != null && tempProds.Any())
        {
            foreach (Product p in tempProds)
            {
               
                if (p.Price >= minPrice && p.Price <= maxPrice)
                {
                    var AllProds = new Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        ImageUrl_ = p.ImageUrl_,
                        Price = p.Price,
                        Category = p.Category,
                        Active = 1,
                    };

                    Prods.Add(AllProds);
                }
            }

            return Prods;
        }
        else
        {
            return new List<Product>(); 
        }
    }



    //a function to delete products
    public bool DeleteProduct(int id)
    {
        try
        {
            var product = data.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                data.Products.DeleteOnSubmit(product);
                data.SubmitChanges();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
           
            return false;
        }
    }



    //a method to add a products to cart
    public bool AddToCart(int userId, int productId, int quantity)
    {
        var cartItem = data.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

        var get = (from p in data.Products where p.Id.Equals(productId)  select p).FirstOrDefault();

        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cartItem = new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                Name = get.Name,
                Price = get.Price,
                ImageUrl = get.ImageUrl_

            };
            data.CartItems.InsertOnSubmit(cartItem);
        }

        try
        {
            data.SubmitChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }


    //a method to count cart items
    public int GetCartItemCount(int userId)
    {
        try
        {
            
            int itemCount = data.CartItems.Count(c => c.UserId == userId);
            return itemCount;
        }
        catch (Exception ex)
        {
          
            return 0; 
        }
    }


    //a method to remove a products to cart
    public bool RemoveFromCart(int userId, int productId)
    {
        var cartItem = data.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

        if (cartItem != null)
        {
            data.CartItems.DeleteOnSubmit(cartItem);
            try
            {
                data.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        return false;
    }


    // A function to update cart
    public bool UpdateCart(int userId, int productId, int quantity)
    {
        var cartItem = data.CartItems.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

        if (cartItem != null)
        {
            cartItem.Quantity = quantity;

            try
            {
                data.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        return false;
    }



    public List<CartItem> GetCartItems(int userId)
    {
    
      

       dynamic Lists_CartItems = new List<CartItem>();

       var tempCartItem = (from p in data.CartItems
                             where p.UserId == userId
                             select p).DefaultIfEmpty();


        if (tempCartItem != null)
        {
            foreach (CartItem C in tempCartItem)
            {

                if (C != null)
                {
                    var Items = new CartItem
                    {
                        Id = C.Id,
                        UserId = C.UserId,
                        ProductId = C.ProductId,
                        Quantity = C.Quantity,
                        Price = C.Price,
                        Name = C.Name,
                        ImageUrl = C.ImageUrl

                    };

                    Lists_CartItems.Add(Items);
                }
                else
                {
                    return null;
                }
               
            }

            return Lists_CartItems;
        }
        else
        {
            return null;
        }


    }


   

    public decimal ApplyDiscount(decimal totalAmount)
    {
        decimal discount = 0;

        if (totalAmount >= 300000)
        {
            discount = totalAmount * 0.10m;  
            totalAmount -= discount;
        }

        return totalAmount;
    }



    // Create a new invoice for a user
    public Invoice Checkout(int userId)
    {
        try
        {
            // Get cart items for the user
            var cartItems = GetCartItems(userId);

            if (cartItems.Count > 0)
            {
                
                decimal totalAmount = cartItems.Sum(c => c.Price * c.Quantity);
                decimal discount = ApplyDiscount(totalAmount);
                decimal vatAmount = totalAmount * 0.15m; // 15% VAT
                decimal deliveryFee = 50.00m; 
                decimal finalTotal = totalAmount - discount + vatAmount + deliveryFee;

               
                var invoice = new Invoice
                {
                    UserId = userId,
                    Date = DateTime.Now,
                    TotalAmount = finalTotal 
                };

               
                data.Invoices.InsertOnSubmit(invoice);
                data.SubmitChanges();

                // Create and insert invoice items
                foreach (var item in cartItems)
                {
                    var invoiceItem = new InvoiceItem
                    {
                        InvoiceId = invoice.Id,
                        ProductId = item.ProductId,
                        Name = item.Name,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };

                    data.InvoiceItems.InsertOnSubmit(invoiceItem);
                }

             
                data.SubmitChanges();

               
                ClearCart(userId);

                return invoice; 
            }

            return null; 
        }
        catch (Exception ex)
        {
            
            return null;
        }
    }

   
    private void ClearCart(int userId)
    {
        var cartItems = (from c in data.CartItems
                         where c.UserId == userId
                         select c).ToList();

        data.CartItems.DeleteAllOnSubmit(cartItems);
        data.SubmitChanges();
    }


    // Retrieve invoice details based on invoice ID
    public Invoice GetInvoiceDetails(int userId)
    {

        var getInvoice = (from i in data.Invoices
                          where i.UserId == userId
                          orderby i.Date descending 
                          select i).FirstOrDefault();

        if (getInvoice != null)
        {
            
            var invoice = new Invoice
            {
                Id = getInvoice.Id,
                UserId = userId,
                Date = getInvoice.Date ,
                TotalAmount = getInvoice.TotalAmount
            };

            return invoice; 
        }
        else
        {
            return null; 
        }
    }

    public List<InvoiceItem> GetInvoiceItems(int invoiceId)
    {
        dynamic Lists_InvoiceItems = new List<InvoiceItem>();

        var tempInvoiceItems = (from ii in data.InvoiceItems
                                where ii.InvoiceId == invoiceId
                                select ii).ToList();

        if (tempInvoiceItems != null)
        {
            foreach (var item in tempInvoiceItems)
            {
                if (item != null)
                {
                    var invoiceItem = new InvoiceItem
                    {
                        
                        Name = item.Name,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };

                    Lists_InvoiceItems.Add(invoiceItem);
                }
            }

            return Lists_InvoiceItems;
        }
        else
        {
            return null;
        }
    }


    /**
     * This is the section for Reports
     * 
     */
    public int GetTotalProductsSold()
    {
        return data.InvoiceItems.Select(c => c.ProductId).Distinct().Count();
    }


    
    public int GetRegisteredUsersCountByDate(DateTime date)
    {
      
        var userCount = data.UserRegistrations.Count(u => u.CreateDate.Date == date.Date);
        return userCount;
    }



 

    public int GetTotalOrdersPlaced()
    {
       
        var totalOrders = data.Invoices.Count();
        return totalOrders;
    }

    public int GetProductsInSockCount()
    {
        
        var productsOnHandCount = data.Products.Count(p => p.Active == 1 || p.Active == 2);
        return productsOnHandCount;
    }

    public List<Product> GetProductsSoldByDateRange(DateTime startDate, DateTime endDate)
    {
        var soldProducts = from invoice in data.Invoices
                           where invoice.Date >= startDate && invoice.Date <= endDate
                           from item in invoice.InvoiceItems
                           select item.Product;

        return soldProducts.Distinct().ToList();
    }

  


    public List<int> GetTotalProductsSoldOverTime(DateTime startDate, DateTime endDate)
    {
        return data.Invoices
            .Where(i => i.Date >= startDate && i.Date <= endDate)
            .SelectMany(i => i.InvoiceItems)
            .GroupBy(item => item.ProductId)
            .Select(group => group.Count())
            .ToList();
    }

    public List<int> GetTotalOrdersPlacedOverTime(DateTime startDate, DateTime endDate)
    {
        return data.Invoices
            .Where(i => i.Date >= startDate && i.Date <= endDate)
            .GroupBy(i => i.Date)
            .Select(group => group.Count())
            .ToList();
    }
}


