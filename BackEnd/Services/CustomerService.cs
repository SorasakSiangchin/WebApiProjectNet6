using BackEnd.interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using BackEnd.Settings;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace BackEnd.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DatabaseContext databaseContext;
        private readonly jwtSetting jwtSetting;
        private readonly IUploadFileService uploadFileService;

        public CustomerService(DatabaseContext databaseContext , jwtSetting jwtSetting, IUploadFileService uploadFileService)
        {
            this.databaseContext = databaseContext;
            this.jwtSetting = jwtSetting;
            this.uploadFileService = uploadFileService;
        }

        public string GenerateToken(Customer customer)
        {
            #region PAYLOAD
            var claims = new[] {
                // Claim เป็นของเขา จะใส่อะไรก็ได้
                // ใส่ PAYLOAD 
                // JwtRegisteredClaimNames.Sub เป็น key ของเขา
                new Claim(JwtRegisteredClaimNames.Sub,customer.Email),
                //ตอนที่เรา login เราส่ง account.Role เข้ามา
                new Claim("role",customer.Role.Name),
                new Claim("id",customer.ID),
                new Claim("additonal","TestSomething"),
                new Claim("todo day","10/10/99"),
                
            };
            #endregion
            return BuildToken(claims);
        }


        public Customer GetInfo(string accessToken)
        {
            //as JwtSecurityToken แปลงค่า Token (ถอดรหัส)
            var token = new JwtSecurityTokenHandler().ReadToken(accessToken) as JwtSecurityToken;

            // ค้นหา sub 
            var username = token.Claims.First(claim => claim.Type == "sub").Value;
            // ค้นหา role 
            var role = token.Claims.First(claim => claim.Type == "role").Value;
            // ค้นหา id
            var id = token.Claims.First(claim => claim.Type == "id").Value;
            var account = new Customer
            {
                Email = username,
                Role = new Role
                {
                    Name = role
                },
                ID = id
            };

            return account;

        }

        public async Task<IEnumerable<Customer>> GetAll(string searchName = "", int searchRole = 0)
        {
            var data = databaseContext.Customers.Include(e => e.Role).AsQueryable(); 

            if (!string.IsNullOrEmpty(searchName)) data = data.Where(a => a.Name.Contains(searchName));

            if (searchRole != 0) data = data.Where(a => a.RoleID.Equals(searchRole));

            return data;
        }
        
        public async Task<Customer> GetByID(string ID)
        {
           var result = await databaseContext.Customers.Include(e => e.Role).AsNoTracking().FirstOrDefaultAsync(e => e.ID == ID);
           if(result == null)
           {
                return null;
           }
           return result;
        }

        public async Task<Customer> GetShopKeeperByID(string ID)
        {
            var result = await databaseContext.Customers.Include(e => e.Role).AsNoTracking().FirstOrDefaultAsync(e => e.ID == ID);
            if (result == null)
            {
                return null;
            }
            return result;
        }



        public async Task<Customer> Login(string email, string password)
        {
            var result = await databaseContext.Customers.Include(a => a.Role).SingleOrDefaultAsync(e => e.Email == email);
            if (result != null && VerifyPassword(result.Password , password))
            {
                return result;
            }
            return null;
        }

        public async Task<object> Register(Customer customer)
        {
           
            if (string.IsNullOrEmpty(customer.ID)) customer.ID = await GenerateID();
           
            if (customer.RoleID == 0) customer.RoleID = 1;
            var result = await databaseContext.Customers.SingleOrDefaultAsync(e => e.Email == customer.Email);

            if (result != null) return new {msg = "อีเมลซ้ำ"};
            //------------- Password ที่ไม่ผ่านการ Has ---------
            await AddPassword(customer.ID, customer.Password);
            //------------- Password ที่ผ่านการ Has ---------
            customer.Password = CreateHashPassword(customer.Password);
            await databaseContext.Customers.AddAsync(customer);
            await databaseContext.SaveChangesAsync();
            return null;
        }


        //คือการสร้าง Code แบบ Auto
        public async Task<string> GenerateID()
        {
            Random randomNumber = new Random();
            string Id = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                Id = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;

                var result = await databaseContext.Customers.FindAsync(Id);

                if (result == null)
                {
                    break;
                };
            }
            return Id;
        }

        //คือการสร้าง Code แบบ Auto
        public async Task<string> GenerateIDPassword()
        {
            Random randomNumber = new Random();
            string Id = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                Id = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;

                var result = await databaseContext.CustomerPasswords.FindAsync(Id);

                if (result == null)
                {
                    break;
                };
            }
            return Id;
        }


        private string CreateHashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            var hashed = HashPassword(password, salt);

            var hpw = $"{Convert.ToBase64String(salt)}.{hashed}";
            return hpw;
        }

        private bool VerifyPassword(string saltAndHashFromDB, string password)
        {
            // ทำการแยกส่วนเป็น 2 สว่น เป็นอเร
            var parts = saltAndHashFromDB.Split('.', 2);
            if (parts.Length != 2) return false;
            // ไปเอาเกลือมา
            // Convert.FromBase64String ให้กลับเหมือนเดิมปกติมันเป็นไบต์
            var salt = Convert.FromBase64String(parts[0]);
            var passwordHash = parts[1];

            string hashed = HashPassword(password, salt);

            return hashed == passwordHash;
        }

        private string HashPassword(string password, Byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
              password: password,
              salt: salt,
              prf: KeyDerivationPrf.HMACSHA256,
              iterationCount: 100000,
              numBytesRequested: 256 / 8));
            return hashed;
        }


        //private string BuildToken(Claim[] claims)
        //{
        //    // ดึงขอ้มูลเข้ามาและแปลงให้เป็นวันที่
        //    var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSetting.Expire));
        //    // ดึง key มาทำการเข้ารหัส
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
        //    // จะเข้ารหัสด้วยวิธีอะไร
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    //สร้าง Token *** 
        //    //JwtSecurityToken เป็นของเข้า
        //    var token = new JwtSecurityToken(
        //        issuer: jwtSetting.Issuer,
        //        audience: jwtSetting.Audience,
        //        claims: claims,
        //        // หมดอายุเมื่อไร
        //        expires: expires,
        //        signingCredentials: creds
        //    );
        //    // WriteToken เขียนออกมา เป็น string 
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        private async Task AddPassword(string customerID , string customerPassword)
        {
            var data = new CustomerPassword() { ID = await GenerateIDPassword() , CustomerID = customerID, Password = customerPassword };
            await databaseContext.CustomerPasswords.AddAsync(data);
        }

        private string BuildToken(Claim[] claims)
        {
            // ดึงขอ้มูลเข้ามาและแปลงให้เป็นวันที่
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSetting.Expire));
            // ดึง key มาทำการเข้ารหัส
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            // จะเข้ารหัสด้วยวิธีอะไร
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //สร้าง Token *** 
            //JwtSecurityToken เป็นของเข้า
            var token = new JwtSecurityToken(
                issuer: jwtSetting.Issuer,
                audience: jwtSetting.Audience,
                claims: claims,
                // หมดอายุเมื่อไร
                expires: expires,
                signingCredentials: creds
            );
            // WriteToken เขียนออกมา เป็น string 
            return new JwtSecurityTokenHandler().WriteToken(token);
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

        public async Task Update(Customer customer)
        {
            databaseContext.Update(customer);
            await databaseContext.SaveChangesAsync();
        }
    }
}
