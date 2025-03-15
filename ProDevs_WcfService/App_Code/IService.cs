using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{


	//  Login and registeration
	[OperationContract]
	bool Register(string firstName, string lastName, string email, string password, string gender ,string userType, DateTime createDate);

	[OperationContract]
	UserRegistration Login(string email, string password);


	// Products
	[OperationContract]
	Product GetProduct(int id);

	[OperationContract]
	List<Product> GetAllProducts();

	[OperationContract]
	List<Product> GetBestSellingProducts();


	//The wcf function for Sort and filter
	[OperationContract]
	List<Product> GetProductsByCategory(string category);
	[OperationContract]
	List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);


	//Product Management
	[OperationContract]
	bool AddProduct(Product product);

	[OperationContract]
	bool UpdateProduct(Product product);

	[OperationContract]
	bool DeleteProduct(int id);



	//Shopping cart functions
	[OperationContract]
	bool AddToCart(int userId, int productId, int quantity);

	[OperationContract]
	int GetCartItemCount(int userId);
	[OperationContract]
	bool RemoveFromCart(int userId, int productId);

	[OperationContract]
	bool UpdateCart(int userId, int productId, int quantity);

	[OperationContract]
	List<CartItem> GetCartItems(int userId);


	//Invoice functions
	[OperationContract]
	Invoice Checkout(int userId);

	[OperationContract]
	Invoice GetInvoiceDetails(int userid);
	[OperationContract]
	List<InvoiceItem> GetInvoiceItems(int invoiceId);

	//discount function
	[OperationContract]
	decimal ApplyDiscount(decimal totalAmount);

	//The wcf function fo dashboard queries

	[OperationContract]
	int GetRegisteredUsersCountByDate(DateTime date);

	[OperationContract]
	int GetTotalProductsSold();


	[OperationContract]
	int GetTotalOrdersPlaced();


	[OperationContract]
	int GetProductsInSockCount();

	[OperationContract]
	List<int> GetTotalProductsSoldOverTime(DateTime startDate, DateTime endDate);

	[OperationContract]
	List<int> GetTotalOrdersPlacedOverTime(DateTime startDate, DateTime endDate);

	[OperationContract]
	List<Product> GetProductsSoldByDateRange(DateTime startDate, DateTime endDate);


}


