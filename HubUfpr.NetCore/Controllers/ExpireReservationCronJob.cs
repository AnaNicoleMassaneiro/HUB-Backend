using HubUfpr.Service.Class;
using HubUfpr.Service.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HubUfpr.API.Controllers
{
    public class ExpireReservationCronJob : CronJobService
    {
        private readonly ILogger<ExpireReservationCronJob> _logger;
        private readonly IReservaService _reservaService;

        public ExpireReservationCronJob(IScheduleConfig<ExpireReservationCronJob> config, ILogger<ExpireReservationCronJob> logger, IReservaService reservaService)
             : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _reservaService = reservaService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob to expire pending reservations has started.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                var list = _reservaService.GetReservationsToExpire();
                for (int i = 0; i < list.Count; i++)
                {
                    _reservaService.UpdateReserveStatus(list[i], 2);
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Task.FromException(ex);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob to expire pending reservations has stopped.");
            return base.StopAsync(cancellationToken);
        }
    }
}
