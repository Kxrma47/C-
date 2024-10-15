using NewVariant.Interfaces;
using NewVariant.Models;

namespace NewVariant.N
{
    /// <summary>
    /// Provides methods for accessing and retrieving data from a database.
    /// </summary>
    public class DataAccessLayer : IDataAccessLayer
    {
        /// <summary>
        /// Retrieves all goods purchased by the buyer with the longest name.
        /// </summary>
        /// <param name="dataBase">The database containing the necessary tables.</param>
        /// <returns>An enumerable collection of <see cref="Good"/> objects.</returns>
        public IEnumerable<Good> GetAllGoodsOfLongestNameBuyer(IDataBase dataBase)
        {
            var buyers = dataBase.GetTable<Buyer>();
            var longestNameBuyer = buyers.OrderByDescending(b => b.Name.Length).First();
            var sales = dataBase.GetTable<Sale>();
            var goodIds = sales.Where(s => s.BuyerId == longestNameBuyer.Id).Select(s => s.GoodId);
            var goods = dataBase.GetTable<Good>();
            return goods.Where(g => goodIds.Contains(g.Id));
        }
        /// <summary>
        /// Retrieves the category of the most expensive good.
        /// </summary>
        /// <param name="dataBase">The database containing the necessary tables.</param>
        /// <returns>The name of the category with the most expensive goods, or null if no goods exist.</returns>
        public string? GetMostExpensiveGoodCategory(IDataBase dataBase)
        {
            var sales = dataBase.GetTable<Sale>();
            var goods = dataBase.GetTable<Good>();
            var categoryPrices = from sale in sales
                join good in goods on sale.GoodId equals good.Id
                group new { sale, good } by good.Category
                into g
                select new { Category = g.Key, TotalPrice = g.Sum(x => x.good.Price * x.sale.GoodCount) };
            return categoryPrices.OrderByDescending(x => x.TotalPrice).FirstOrDefault()?.Category;
        }

        /// <summary>
        /// Retrieves the city where the least number of sales occurred.
        /// </summary>
        /// <param name="dataBase">The database containing the necessary tables.</param>
        /// <returns>The name of the city with the least sales, or null if no sales exist.</returns>
        public string? GetMinimumSalesCity(IDataBase dataBase)
        {
            var sales = dataBase.GetTable<Sale>();
            var shops = dataBase.GetTable<Shop>();
            var salesByCity = from sale in sales
                join shop in shops on sale.ShopId equals shop.Id
                group sale by shop.City
                into g
                select new { City = g.Key, TotalSales = g.Sum(x => x.GoodCount) };
            return salesByCity.OrderBy(x => x.TotalSales).FirstOrDefault()?.City;
        }
        /// <summary>
        /// Retrieves the top 10 buyers who purchased the most goods.
        /// </summary>
        /// <param name="dataBase">The database containing the necessary tables.</param>
        /// <returns>An enumerable collection of <see cref="Buyer"/> objects.</returns>
        public IEnumerable<Buyer> GetMostPopularGoodBuyers(IDataBase dataBase)
        {
            var sales = dataBase.GetTable<Sale>();
            var buyers = dataBase.GetTable<Buyer>();
            var buyerIds = sales.GroupBy(s => s.BuyerId)
                .Select(g => new { BuyerId = g.Key, TotalCount = g.Sum(x => x.GoodCount) })
                .OrderByDescending(x => x.TotalCount)
                .Take(10)
                .Select(x => x.BuyerId);
            return buyers.Where(b => buyerIds.Contains(b.Id));
        }
        /// <summary>
        /// Retrieves the minimum number of shops in any one country.
        /// </summary>
        /// <param name="dataBase">The database containing the necessary tables.</param>
        /// <returns>The minimum number of shops in any one country, or 0 if no shops exist.</returns>
        public int GetMinimumNumberOfShopsInCountry(IDataBase dataBase)
        {
            var shops = dataBase.GetTable<Shop>();
            var shopsByCountry = shops.GroupBy(s => s.Country).Select(g => g.Count());
            return shopsByCountry.DefaultIfEmpty(0).Min();
        }
        /// <summary>
        /// Retrieves all sales made in cities other than the buyer's city.
        /// </summary>
        /// <param name="dataBase">The database containing the necessary tables.</param>
        /// <returns>An enumerable collection of <see cref="Sale"/> objects.</returns>
        public IEnumerable<Sale> GetOtherCitySales(IDataBase dataBase)
        {
            var shops = dataBase.GetTable<Shop>();
            var sales = dataBase.GetTable<Sale>();
            var salesInOtherCity = from sale in sales
                join shop in shops on sale.ShopId equals shop.Id
                where sale.BuyerId != shop.Id
                select sale;
            return salesInOtherCity;
        }
        /// <summary>
        /// Retrieves the total value of all sales.
        /// </summary>
        /// <param name="dataBase">The database containing the necessary tables.</param>
        /// <returns>The total value of all sales.</returns>
        public long GetTotalSalesValue(IDataBase dataBase)
        {
            var sales = dataBase.GetTable<Sale>();
            var goods = dataBase.GetTable<Good>();
            var totalSalesValue = from sale in sales
                join good in goods on sale.GoodId equals good.Id
                select sale.GoodCount * good.Price;
            return totalSalesValue.Sum();
        }
    }
}

