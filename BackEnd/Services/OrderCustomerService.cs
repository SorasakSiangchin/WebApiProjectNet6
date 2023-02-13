    using BackEnd.DTOS.OrderCustomer;
using BackEnd.interfaces;
using BackEnd.Interfaces;
using BackEnd.Models;
using BackEnd.Models.OrderCustomers;
using BackEnd.Models.Reposts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class OrderCustomerService : IOrderCustomerService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IUploadFileService uploadFileService;
        private readonly IProductService productService;
        private readonly IDeliveryService deliveryService1;

        public OrderCustomerService(DatabaseContext databaseContext, IUploadFileService uploadFileService , IProductService productService , IDeliveryService deliveryService1)
        {
            this.databaseContext = databaseContext;
            this.uploadFileService = uploadFileService;
            this.productService = productService;
            this.deliveryService1 = deliveryService1;
        }

        public async Task AddOrder(OrderCustomer orderCustomer, ProductListOrderRequest productListOrderRequest)
        {
            var productLists = new List<OrderProductList>();
            var orders = new List<OrderCustomer>();
            for (var i = 0; i < productListOrderRequest.ProductID.Length; i++)
            {
                // ค้นหาข้อมูลของสินค้า
                var result = await productService.GetByID(productListOrderRequest.ProductID[i]);
                // 
                var product = productLists.FirstOrDefault(e => e.CustomerID == result.CustomerID);
                if (product == null)
                {
                    var itemOrder = new OrderCustomer() 
                    {
                        ID = await GenerateIdOrderCustomer() ,
                        PaymentStatus = orderCustomer.PaymentStatus ,
                        ProofOfPayment = orderCustomer.ProofOfPayment ,
                        PriceTotal = productListOrderRequest.ProductPrice[i] * productListOrderRequest.ProductAmount[i],
                        CustomerStatus = orderCustomer.CustomerStatus ,
                        AddressID = orderCustomer.AddressID
                    };
                    var itemProductList = new OrderProductList()
                    {
                        ID = await GenerateIdProductListr(),
                        OrderID = itemOrder.ID,
                        ProductID = productListOrderRequest.ProductID[i],
                        ProductAmount = Convert.ToInt32(productListOrderRequest.ProductAmount[i]),
                        ProductPrice = productListOrderRequest.ProductPrice[i],
                        CustomerID = result.CustomerID
                    };
                    orders.Add(itemOrder);
                    productLists.Add(itemProductList);
                }
                else
                {
                    var itemProductList = new OrderProductList()
                    {
                        ID = await GenerateIdProductListr(),
                        OrderID = product.OrderID,
                        ProductID = productListOrderRequest.ProductID[i],
                        ProductAmount = Convert.ToInt32(productListOrderRequest.ProductAmount[i]),
                        ProductPrice = productListOrderRequest.ProductPrice[i],
                        CustomerID = result.CustomerID
                    };
                    for (int j = 0; j < orders.Count(); j++)
                    {
                        if (orders[j].ID.Equals(product.OrderID))
                        {
                            orders[j].PriceTotal += (productListOrderRequest.ProductPrice[i] * productListOrderRequest.ProductAmount[i]);
                        }
                    };
                    productLists.Add(itemProductList);
                }

            }
            var y = new List<ProductList>();
            for (int j = 0; j < productLists.Count(); j++)
            {
                var item = new ProductList()
                {
                    ID = productLists[j].ID,
                    OrderID = productLists[j].OrderID,
                    ProductID = productLists[j].ProductID,
                    ProductAmount = productLists[j].ProductAmount,
                    ProductPrice = productLists[j].ProductPrice
                };
                y.Add(item);
            }

            // ------------------ Add สินค้าทั้งหมดไว้ใน List ---------------
            await databaseContext.ProductLists.AddRangeAsync(y);
            //---------------------------------------------------------

            //---------- AddRangeAsync เป็นการ Add ทั้งหมด ------
            await databaseContext.OrderCustomers.AddRangeAsync(orders);

            await RemoveCartProduct(productListOrderRequest);

            await RemoveStockProduct(productListOrderRequest);

            await databaseContext.SaveChangesAsync();

        }

        public async Task RemoveCartProduct(ProductListOrderRequest productListOrderRequest)
        {
            for (var i = 0; i < productListOrderRequest.ProductID.Length; i++)
            {
                var result = await databaseContext.CartCustomers.AsNoTracking().FirstOrDefaultAsync(e => e.ID == productListOrderRequest.CartID[i]);
                databaseContext.Remove(result);
            }
        }

        public async Task RemoveStockProduct(ProductListOrderRequest productListOrderRequest)
        {

            for (var i = 0; i < productListOrderRequest.ProductID.Length; i++)
            {
                
                var result = await productService.GetByID(productListOrderRequest.ProductID[i]);
                result.Stock -= productListOrderRequest.ProductAmount[i];
                databaseContext.Update(result);
            }
        }

        public async Task<IEnumerable<OrderCustomer>> GetAll(string idCustomer)
        {
           return  await databaseContext.OrderCustomers.Where(e => e.Address.Customer.ID == idCustomer).ToListAsync();
        }

        public async Task<OrderCustomer> GetByID(string id)
        {
            var result = await databaseContext.OrderCustomers.Include(e => e.Address).Include(e => e.Address.AddressInformation).Include(e => e.Address.Customer).AsNoTracking().FirstOrDefaultAsync(e => e.ID == id);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<string> GenerateIdProductListr()
        {
            Random randomNumber = new Random();
            string IdProductListr = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                IdProductListr = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;

                var result = await databaseContext.ProductLists.FindAsync(IdProductListr);

                if (result == null)
                {
                    break;
                };
            }
            return IdProductListr;
        }

        public async Task<string> GenerateIdOrderCustomer()
        {
            Random randomNumber = new Random();
            string IdProductListr = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                IdProductListr = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;


                var result = await databaseContext.OrderCustomers.FindAsync(IdProductListr);

                if (result == null)
                {
                    break;
                };
            }
            return IdProductListr;
        }

        public Task<IEnumerable<double>> RepostOrderByIdCustomer(DateTime dateEnd, DateTime dateStart, string idCustomer)
        {
            throw new NotImplementedException();
        }
        
        public async Task<IEnumerable<double>> MoneyUsageReport(DateTime dateEnd, DateTime dateStart, string idCustomer)
        {
            //เก็บผลรวมของเงิน
            double sumPrice = 0;
            //เก็บค้าเป็น percent แต่ละเดือน
            List<double> percentMonth = new List<double>();
            var dataOrder = databaseContext.OrderCustomers.AsQueryable();
            if (!string.IsNullOrEmpty(idCustomer))
            {
                // แยกใบสั่งซื้อของแต่ละคน
                dataOrder = dataOrder.Where(e => e.Address.CustomerID.Equals(idCustomer));
            }

            // ได้ order มาแล้ว
            var result = await dataOrder.Where(e => e.Created.Month >= dateStart.Month && e.Created.Month <= dateEnd.Month).ToListAsync();
            if (result == null || result.Count() == 0) return percentMonth;
            var numMonth = dateEnd.Month - dateStart.Month ;
            for (var i = 0; i < 12; i++)
            {
                // 
                var data = result.Where(e => e.Created.Month == i + 1).ToList().Sum(p => p.PriceTotal);
                //Convert.ToDouble(data);
                percentMonth.Add(Convert.ToDouble(data));
            }

            sumPrice = percentMonth.Sum();
            for (var i = 0; i < percentMonth.Count(); i++)
            {
                percentMonth[i] = percentMonth[i] * 100 / sumPrice;
            }

            return percentMonth;
        }
        
        public async Task<IEnumerable<CategoryProductRepost>> OrderReportByProductType(DateTime dateNew, string idCustomer)
        {

            double sumNumCategory = 0;
            List<ProductList> productList = new List<ProductList>();
            List<CategoryProductRepost> categoryProduct = new List<CategoryProductRepost>();
            // หา Order ตามวันเวลาที่ส่งมา 
            var dataOrder = await databaseContext.OrderCustomers.Where(e => e.Created.Date == DateTime.Parse("2565-09-15")).ToListAsync();
            for (var i = 0; i < dataOrder.Count(); i++)
            {
                // หา productList ที่มี ไอดีของ Order
                var data = await databaseContext.ProductLists.Include(e => e.Product).Include(e => e.Product.CategoryProduct).Include(e => e.OrderCustomer.Address.Customer).Where(e => e.OrderID == dataOrder[i].ID && e.Product.CustomerID == idCustomer).ToListAsync();
                // เก็บไว้ เป็น ชุด
                for (var j = 0; j < data.Count(); j++)
                {
                    var result = productList.FirstOrDefault(e => e.ProductID == data[j].ProductID);
                    if (result != null)
                    {
                        result.ProductAmount += data[j].ProductAmount;
                    }
                    else
                    {
                        productList.Add(data[j]);
                    }
                }
            }
            for (var j = 0; j < productList.Count(); j++)
            {
                var result = categoryProduct.FirstOrDefault(e => e.ID == productList[j].Product.CategoryProductID);
                if (result != null)
                {
                    result.Num = result.Num + productList[j].ProductAmount;
                }
                else
                {
                    //CategoryProductRepost item = new CategoryProductRepost() { ID= productList[j].Product.CategoryProductID, Name= productList[j].Product.CategoryProduct.Name, Num= productList[j].ProductAmount , NumPercen=0 };
                    // var item = new CategoryProductRepost() { ID = "dd", Name = "sss", Num = productList[j].ProductAmount, NumPercen = 0 };
                    categoryProduct.Add(new CategoryProductRepost() { ID = productList[j].Product.CategoryProductID, Name = productList[j].Product.CategoryProduct.Name, Num = productList[j].ProductAmount, NumPercen = 0 });
                }
                // ผลรวมของจำนวนสินค้าในแต่ละประเภท
                
            }
            sumNumCategory = categoryProduct.Sum(e => e.Num);
            for (var x = 0; x < categoryProduct.Count(); x++)
            {
                categoryProduct[x].NumPercen = categoryProduct[x].Num * 100 / sumNumCategory;
            }
            return categoryProduct;
        }
       
        public async Task UpdateOrder(OrderCustomer orderCustomer)
        {
            databaseContext.Update(orderCustomer);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<(string errorMessage, string imageName)> UploadImage(IFormFileCollection formFiles)
        {
            var errorMessage = string.Empty;
            //var imageName = new List<string>();
            var imageName = string.Empty;
            if (uploadFileService.IsUpload(formFiles))
            {
                errorMessage = uploadFileService.Validation(formFiles);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    imageName = (await uploadFileService.UploadImages(formFiles))[0];
                }
            }
            return (errorMessage, imageName);
        }

        public async Task DeleteImage(string fileName)
        {
            await uploadFileService.DeleteImage(fileName);
        }

        public async Task<IEnumerable<OrderCustomer>> GetOrdered(string idCustomer)
        {

            var result = await databaseContext.ProductLists.Where(e => e.Product.CustomerID == idCustomer).Include(e => e.OrderCustomer).Include(e => e.Product).ToListAsync();
            if (result == null || result.Count() == 0)
            {
                return null;
            }
            List<OrderCustomer> orders = new List<OrderCustomer>();
            for (var i = 0; i < result.Count(); i++)
            {
                var data = orders.FirstOrDefault(e => e.ID == result[i].OrderID);
                if (data == null)
                {
                    var order = await databaseContext.OrderCustomers.Include(e => e.Address).AsNoTracking().FirstOrDefaultAsync(e => e.ID == result[i].OrderID);
                    if (order != null)
                    {
                        orders.Add(order);
                    }
                }

            }
            return orders;
        }

        // แสดง Order ที่่มีการส่งหลักฐานมา และยังไม่ยืนยัน
        public async Task<IEnumerable<OrderCustomer>> GetConfirm()
        {
           var result = await databaseContext.OrderCustomers.Include(e => e.Address).Where(e => e.ProofOfPayment != null && e.PaymentStatus == false).ToListAsync();
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<object> ConfirmOrder(List<OrderCustomer> orderCustomer)
        {
            List<ProductList> productList = new List<ProductList>(); 
            for(var i = 0; i < orderCustomer.Count(); i++)
            {
                orderCustomer[i].PaymentStatus = true;
                // ตอนที่ update ไม่ต้องการ Address
                databaseContext.Update(orderCustomer[i]);

                var dataProductList = await databaseContext.ProductLists.Include(d => d.Product).Where(e => e.OrderCustomer.ID == orderCustomer[i].ID).ToListAsync();
                productList.AddRange(dataProductList);
                for (var j = 0; j < dataProductList.Count(); j++)
                {
                    var dataBankAccount = await databaseContext.BankAccounts.AsNoTracking().FirstOrDefaultAsync(e => e.CustomerID == dataProductList[j].Product.CustomerID);
                    if (dataBankAccount == null) return null;
                    var sellerMustConfrim = await databaseContext.SellerMustConfirms.AsNoTracking().FirstOrDefaultAsync(e => e.BankAccount.CustomerID == dataBankAccount.CustomerID);
                    SellerMustConfrim sellerMustConfirm = new SellerMustConfrim() { ID = await GenerateIdSellerMustConfirms(), BankAccountID = dataBankAccount.ID, OrderID = orderCustomer[i].ID, ProductListID = dataProductList[j].ID };
                    await databaseContext.AddAsync(sellerMustConfirm);
                }
            }
           
            await databaseContext.SaveChangesAsync();
            return orderCustomer;
        }
        public async Task<object> CancelOrder(List<OrderCustomer> orderCustomer)
        {
           
            for (var i = 0; i < orderCustomer.Count(); i++)
            {
                var ProofoFPayment = new ProofOfPaymentCancel() { ID = await GenerateIdProofOfPaymentCancel(), Created = orderCustomer[i].Created, ProofOfPayment = orderCustomer[i].ProofOfPayment, OrderID = orderCustomer[i].ID };
                orderCustomer[i].ProofOfPayment = null;
                orderCustomer[i].PaymentStatus = false;
                // ตอนที่ update ไม่ต้องการ Address
                await  databaseContext.AddAsync(ProofoFPayment);
                databaseContext.Update(orderCustomer[i]);
            }
            await databaseContext.SaveChangesAsync();
            return orderCustomer;
        }

        public async Task ConfirmOrderForSeller(List<OrderCustomer> orderCustomer)
        {
            for (var i = 0; i < orderCustomer.Count(); i++)
            {
                orderCustomer[i].SellerStatus = true;

                databaseContext.Deliverys.Add(new Delivery { ID = await GenerateIdDelivery() , StatusDeliveryID = 1 , OrderCustomerID= orderCustomer[i].ID });
                // ตอนที่ update ไม่ต้องการ Address
                databaseContext.Update(orderCustomer[i]);
            }
            await databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderCustomer>> GetSucceedOrder()
        {
            var result = await databaseContext.OrderCustomers.Include(e => e.Address).Where(e => e.ProofOfPayment != null && e.PaymentStatus == true).ToListAsync();
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<string> GenerateIdSellerMustConfirms()
        {
            Random randomNumber = new Random();
            string Id = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                Id = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;

                var result = await databaseContext.SellerMustConfirms.FindAsync(Id);

                if (result == null)
                {
                    break;
                };
            }
            return Id;
        }

        public async Task<string> GenerateIdProofOfPaymentCancel()
        {
            Random randomNumber = new Random();
            string Id = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                Id = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;

                var result = await databaseContext.ProofOfPaymentCancels.FindAsync(Id);

                if (result == null)
                {
                    break;
                };
            }
            return Id;
        }

        public async Task<string> GenerateIdDelivery()
        {
            Random randomNumber = new Random();
            string Id = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                Id = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;

                var result = await databaseContext.Deliverys.FindAsync(Id);

                if (result == null)
                {
                    break;
                };
            }
            return Id;
        }

        public async Task ConfirmOrderForCustomer(OrderCustomer orderCustomer)
        {
            orderCustomer.CustomerStatus = true;
            databaseContext.Update(orderCustomer);
            await databaseContext.SaveChangesAsync();
        }
    }
}
