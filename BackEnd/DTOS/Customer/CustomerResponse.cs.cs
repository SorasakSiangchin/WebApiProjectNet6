using BackEnd.Models;

namespace BackEnd.DTOS.Customer
{
    public class CustomerResponse
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public string RoleName { get; set; }
        public int RoleID { get; set; }

        // Models.Product product ส่งตัวจริงเข้ามาก่อน
        static public CustomerResponse FromCustomer(Models.Customer customer)
        {
            // return ตัวมันเอง
            return new CustomerResponse
            {
                ID = customer.ID,
                Name = customer.Name,
                Image = !string.IsNullOrEmpty(customer.Image) ? "http://10.103.0.15/cs63/s09/reactJs/backEnd/" + "images/" + customer.Image : "",
                // Stock = product.Stock,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Birthday = customer.Birthday,
                RoleName = customer.Role.Name,
                RoleID = customer.Role.ID
            };
        }
    }
}
