using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OrderReportFunction.Services;

namespace OrderReportFunction
{
    public class MonthlyReportFunction
    {
        private readonly ILogger<MonthlyReportFunction> _logger;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;

        public MonthlyReportFunction(ILogger<MonthlyReportFunction> logger, IPdfGenerator pdfGenerator, IOrderService orderService, IEmailService emailService)
        {
            _logger = logger;
            _orderService = orderService;
            _pdfGenerator = pdfGenerator;
            _emailService = emailService;
        }

        [Function("MonthlyReportFunction")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {

            DateTime today = DateTime.Today;
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

            if (!IsLastDayOfMonth(today))
            {
                _logger.LogInformation("Today is not the last day of the month. Exiting.");
                // return;
            }

            _logger.LogInformation("Today is the last day of the month. Proceeding with the task.");

            try
            {
                var reports = await _orderService.GetMonthlyUserReportsAsync(today);

                foreach (var report in reports)
                {
                    var pdf = _pdfGenerator.GenerateUserReport(report);

                    if (pdf != null && pdf.Length > 0)
                    {
                        _logger.LogInformation($"PDF generated successfully, size: {pdf.Length} bytes");
                    }
                    else
                    {
                        _logger.LogInformation($"PDF generation failed or returned empty");
                    }
                    var fileName = $"MonthlyReport_{today:yyyy_MM}_{report.UserEmail.Replace("@", "_at_")}.pdf";

                    await _emailService.SendReportAsync(report.UserEmail, pdf, fileName);
                    _logger.LogInformation($"Report sent to {report.UserEmail}");
                }

                _logger.LogInformation("All reports sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating or sending reports.");
            }
        }

        private bool IsLastDayOfMonth(DateTime date)
        {
            return date.Day == DateTime.DaysInMonth(date.Year, date.Month);
        }
    }
}
