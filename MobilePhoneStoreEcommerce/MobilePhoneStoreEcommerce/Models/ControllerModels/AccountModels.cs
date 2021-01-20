using MobilePhoneStoreEcommerce.Core.Domain;
using MobilePhoneStoreEcommerce.Persistence;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MobilePhoneStoreEcommerce.Models.ControllerModels
{
    public class AccountModels
    {
        private ApplicationDbContext _context;
        public AccountModels()
        {
            _context = new ApplicationDbContext();
        }

        private Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            if (String.IsNullOrEmpty(toEncrypt))
                throw new Exception("characters to encrypt is not valid");

            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            AppSettingsReader settingsReader = new AppSettingsReader();
            //get the key from config file
            string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public bool AddAcc(Account account)
        {
            try
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddSeller (int sellerId, string name, string phone, string email, string address)
        {
            try
            {
                var newSeller = new Seller();
                newSeller.ID = sellerId;
                newSeller.Name = name;
                newSeller.PhoneNumber = phone;
                newSeller.Email = email;
                newSeller.WarehouseAddress = address;
                _context.Sellers.Add(newSeller);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddCustomer(int customerId, string name, string phone, string email, string address)
        {
            try
            {
                var newCustomer = new Customer();
                newCustomer.ID = customerId;
                newCustomer.Name = name;
                newCustomer.PhoneNumber = phone;
                newCustomer.Email = email;
                newCustomer.DeliveryAddress = address;
                _context.Cutomers.Add(newCustomer);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePassword(Account Acc)
        {
            try
            {                
                _context.Accounts.AddOrUpdate(Acc);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}