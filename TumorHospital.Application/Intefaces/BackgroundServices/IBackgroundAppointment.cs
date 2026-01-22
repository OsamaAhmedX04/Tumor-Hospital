using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorHospital.Application.Intefaces.BackgroundServices
{
    public interface IBackgroundAppointment
    {
        Task SetApprovedAppointmentsStatusToAbsent();
    }
}
