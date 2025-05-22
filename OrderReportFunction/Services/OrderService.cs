using Microsoft.Extensions.Configuration;
using OrderReportFunction.Models;

namespace OrderReportFunction.Services;

public class OrderService : IOrderService
{

    private readonly string _connectionStrings;

    public OrderService(IConfiguration config)
    {
        _connectionStrings= config["ConnectionStrings:OrdersDb"] ?? throw new ArgumentNullException(nameof(config), "SendGridApiKey is missing in configuration.");
    }

    public async Task<List<UserReport>> GetMonthlyUserReportsAsync(DateTime month)
    {
        // Example: Use Dapper or EF Core to query your DB
        // var connectionString = _config["ConnectionStrings:OrdersDb"];
        // using var conn = new SqlConnection(connectionString);
        // var orders = await conn.QueryAsync<Order>("SELECT ...");

        throw new NotImplementedException("Replace with real DB logic");
    }
}