﻿namespace BackEnd.DTOS.ProductList
{
    public class ProductListRequest
    {
        public string? ID { get; set; }
        public string? OrderID { get; set; }
        public string[] ProductID { get; set; }
        public int[] ProductPrice { get; set; }
        public int[] ProductAmount { get; set; }

    }
}
