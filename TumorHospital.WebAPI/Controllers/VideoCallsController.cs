//using System.Security.Claims;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using TumorHospital.Application.Intefaces.Services;
//using TumorHospital.Domain.Constants;
//using TumorHospital.Domain.Enums;
//using TumorHospital.Infrastructure.Services;

//namespace TumorHospital.WebAPI.Controllers
//{
//    [ApiController]
//    [Route("api/video-calls")]
//    public class VideoCallsController : ControllerBase
//    {
//        private readonly IVideoCallService _videoCallService;

//        public VideoCallsController(IVideoCallService videoCallService)
//        {
//            _videoCallService = videoCallService;
//        }

//        //[Authorize(Roles = SystemRole.Patient)]
//        [HttpPost("start")]
//        public async Task<IActionResult> StartCall(Guid appointmentId, string doctorId, string patientId)
//        {
//            var callId = await _videoCallService.StartVideoCallAsync(
//                patientId,
//                doctorId,
//                appointmentId);

//            return Ok(new { callId });
//        }

//        //[Authorize(Roles = SystemRole.Doctor)]
//        [HttpPost("{callId}/accept")]
//        public async Task<IActionResult> Accept(Guid callId, string doctorId)
//        {
//            await _videoCallService.AcceptCallAsync(callId, doctorId);
//            return NoContent();
//        }

//        //[Authorize(Roles = SystemRole.Doctor)]
//        [HttpPost("{callId}/reject")]
//        public async Task<IActionResult> Reject(Guid callId, string doctorId)
//        {
//            await _videoCallService.RejectCallAsync(callId, doctorId);
//            return NoContent();
//        }

//        [Authorize(Roles = SystemRole.Doctor + ","  + SystemRole.Patient)]
//        [HttpPost("end/{callId}")]
//        public async Task<IActionResult> EndCall(Guid callId, string reason, string userId)
//        {
//            await _videoCallService.EndCallAsync(callId, userId, reason);
//            return NoContent();
//        }
//    }
//}
