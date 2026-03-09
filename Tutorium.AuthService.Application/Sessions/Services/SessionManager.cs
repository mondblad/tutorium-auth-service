using Tutorium.AuthService.Application.Sessions.Abstractions;
using Tutorium.AuthService.Core.Sessions.Models;
using Tutorium.AuthService.Application.Sessions.Exceptions;

namespace Tutorium.AuthService.Application.Sessions.Services
{
    internal class SessionManager : ISessionManager
    {
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly ISessionIdGenerator _sessionIdGenerator;
        private readonly ISessionUnitOfWork _sessionUnitOfWork;

        public SessionManager(IUserSessionRepository userSessionRepository, ISessionRepository sessionRepository,
            ISessionIdGenerator sessionIdGenerator, ISessionUnitOfWork sessionUnitOfWork)
        {
            _userSessionRepository = userSessionRepository;
            _sessionRepository = sessionRepository;
            _sessionIdGenerator = sessionIdGenerator;
            _sessionUnitOfWork = sessionUnitOfWork;
        }

        public async Task<Session?> GetSessionAsync(string sessionId)
        {
            return await _sessionRepository.GetByIdAsync(sessionId);
        }

        public async Task<List<Session>> GetUserSessionsAsync(int userId)
        {
            var userSessions = await GetOrNewUserSessionsAsync(userId);

            return await _sessionRepository.GetByIdsAsync(userSessions.SessionIds);
        }

        public async Task<Session> CreateSessionAsync(int userId)
        {
            var session = CreateNewSession(userId);
            var userSessions = await GetOrNewUserSessionsAsync(userId);

            userSessions.SessionIds.Add(session.SessionId);

            var transaction = _sessionUnitOfWork.BeginTransaction();

            await _userSessionRepository.SetAsync(userSessions, transaction);
            await _sessionRepository.SetAsync(session, transaction);

            await _sessionUnitOfWork.CommitAsync(transaction);

            return session;
        }

        public async Task DeleteSessionAsync(string sessionId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session is null)
                throw new SessionNotFound();

            var userSessions = await _userSessionRepository.GetByIdAsync(session.UserId);
            
            var transaction = _sessionUnitOfWork.BeginTransaction();

            await _sessionRepository.DeleteByIdAsync(session.SessionId, transaction);

            if (userSessions is not null)
            {
                userSessions.TryDelete(session.SessionId);

                if (userSessions.SessionIds.Any())
                    await _userSessionRepository.SetAsync(userSessions, transaction);
                else
                    await _userSessionRepository.DeleteByIdAsync(userSessions.UserId, transaction);
            }

            await _sessionUnitOfWork.CommitAsync(transaction);
        }

        public async Task<Session> ResetUserSessionsAsync(int userId)
        {
            var userSessions = await _userSessionRepository.GetByIdAsync(userId);
            if (userSessions is null)
                throw new UserSessionNotFound();

            var transaction = _sessionUnitOfWork.BeginTransaction();

            foreach (var sessionId in userSessions.SessionIds)
                await _sessionRepository.DeleteByIdAsync(sessionId, transaction);

            var newSession = CreateNewSession(userId);

            userSessions.SessionIds.Clear();
            userSessions.SessionIds.Add(newSession.SessionId);

            await _sessionRepository.SetAsync(newSession, transaction);
            await _userSessionRepository.SetAsync(userSessions, transaction);

            await _sessionUnitOfWork.CommitAsync(transaction);

            return newSession;
        }

        private async Task<UserSessions> GetOrNewUserSessionsAsync(int userId)
        {
            var userSessions = await _userSessionRepository.GetByIdAsync(userId);
            if (userSessions == null)
                userSessions = new UserSessions(userId);
            return userSessions;
        }

        private Session CreateNewSession(int userId)
        {
            var sessionId = _sessionIdGenerator.GenerateSessionId();
            return new Session(sessionId, userId);
        }
    }
}
