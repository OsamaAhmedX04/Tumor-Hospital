using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.Services
{
    public class VideoCallHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVideoCallService _videoCallService;
        private static readonly ConcurrentDictionary<string, DateTime> _lastIceSent
            = new();
        private static readonly ConcurrentDictionary<Guid, ConcurrentDictionary<string, byte>> _callConnections
            = new();

        public VideoCallHub(IUnitOfWork unitOfWork, IVideoCallService videoCallService)
        {
            _unitOfWork = unitOfWork;
            _videoCallService = videoCallService;
        }
        public async Task SendOffer(string toUserId, object offer)
        => await Clients.User(toUserId).SendAsync("ReceiveOffer", offer);

        public async Task SendAnswer(string toUserId, object answer)
            => await Clients.User(toUserId).SendAsync("ReceiveAnswer", answer);

        public async Task SendIceCandidate(Guid callId, string candidate)
        {
            var userId = Context.UserIdentifier;

            var call = await _unitOfWork.VideoCalls.GetByIdAsync(callId);

            if (call.CallerId != userId && call.ReceiverId != userId)
            throw new HubException("Unauthorized");

            if (call.Status != CallStatus.Accepted)
            throw new HubException("Call not accepted");

            var key = $"{Context.ConnectionId}:ice";
            var now = DateTime.Now;

            if (_lastIceSent.TryGetValue(key, out var last) &&
                (now - last).TotalMilliseconds < 50)
                return;

            _lastIceSent[key] = now;

            await Clients.Group(callId.ToString())
                .SendAsync("IceCandidate", userId, candidate);
        }

        public async Task JoinCall(Guid callId)
        {
            var userId = Context.UserIdentifier;

            var call = await _unitOfWork.VideoCalls.GetByIdAsync(callId);
            if (call == null)
            throw new HubException("Call not found");

            if (call.Status != CallStatus.Accepted)
            throw new HubException("Call has not been accepted yet");

            if (call.CallerId != userId && call.ReceiverId != userId)
            throw new HubException("Unauthorized");

            await Groups.AddToGroupAsync(Context.ConnectionId, callId.ToString());

            await Clients.Group(callId.ToString())
                .SendAsync("UserJoined", userId);

            _callConnections
                .GetOrAdd(callId, _ => new())
                .TryAdd(Context.ConnectionId, 0);

        }

        public async Task LeaveCall(Guid callId)
        {
            var userId = Context.UserIdentifier;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, callId.ToString());

            if (_callConnections.TryGetValue(callId, out var connections))
            {
                connections.TryRemove(Context.ConnectionId, out _);

                if (connections.Count == 0)
                {
                    await _videoCallService.EndCallAsync(callId, null, "Ended");
                    _callConnections.TryRemove(callId, out _);
                }
            }

            await Clients.Group(callId.ToString())
                .SendAsync("UserLeft", userId);
        }

    }

}
