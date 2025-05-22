using OrderReportFunction.Models;
using OrderReportFunction.Services;

namespace OrderReportFunction.Mock.Services;

public class MockOrderService : IOrderService
{
    public async Task<List<UserReport>> GetMonthlyUserReportsAsync(DateTime month)
    {
        await Task.Delay(100); // Simulate async

        return new List<UserReport>
        {
            new UserReport
            {
                UserEmail = "decaro.pietro@gmail.com",
                Orders = new List<Order>
                {
                    new Order { OrderId = "MOCK-001", TotalPrice = 100, VAT = 25 },
                    new Order { OrderId = "MOCK-002", TotalPrice = 200, VAT = 50 }
                }
            }
        };
    }
}
