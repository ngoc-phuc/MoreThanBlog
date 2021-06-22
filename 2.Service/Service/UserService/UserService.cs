using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Repository;
using Abstraction.Repository.Model;
using Abstraction.Service.Common;
using Abstraction.Service.UserService;
using Core.Errors;
using Core.Helper;
using Core.Model.User;
using Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace Service.UserService
{
    public class UserService: BaseService.Service, IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUnitOfWork unitOfWork,
            AutoMapper.IMapper mapper,
            ITokenService tokenService) : base(unitOfWork, mapper)
        {
            _userRepository = UnitOfWork.GetRepository<UserEntity>();
            _tokenService = tokenService;
        }

        public async Task<JwtTokenResultModel> LoginAsync(LoginModel model, CancellationToken cancellationToken = default)
        {
            var userEntity = await _userRepository.Get(x => x.Email == model.Email)
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(true);

            // Check User is exist
            if (userEntity == null)
            {
                throw new MoreThanBlogException(nameof(ErrorCode.UserNotFound), ErrorCode.UserNotFound);
            }

            // Compare password hash from request and database
            var passwordHash = PasswordHelper.HashPassword(model.Password, userEntity.PasswordLastUpdatedTime.UtcDateTime);
            if (passwordHash != userEntity.PasswordHash)
            {
                throw new MoreThanBlogException(nameof(ErrorCode.WrongPassword), ErrorCode.WrongPassword);
            }

            // Generate Access Token

            var now = DateTimeOffset.UtcNow;

            var claimIdentity = GenerateClaimsIdentity(userEntity);

            return await _tokenService.RequestTokenAsync(claimIdentity);
        }

        public async Task InitAdminAccountAsync(CancellationToken cancellation = default)
        {
            if (!_userRepository.Get().Any())
            {
                var now = DateTimeOffset.UtcNow;
                _userRepository.Add(new UserEntity
                {
                    FirstName = "More Than Blog",
                    LastName = "Admin",
                    Email = "morethan.team@yopmail.com",
                    IsActive = true,
                    PasswordLastUpdatedTime = now,
                    PasswordHash = PasswordHelper.HashPassword("abcd@1234", now)
                });

                await UnitOfWork.SaveChangesAsync(cancellation);
            }
        }

        public LoggedInUser GetUserProfile(string userId)
        {
            return  _userRepository.Get(x => x.Id == userId)
                .Select(x => _mapper.Map<LoggedInUser>(x))
                .FirstOrDefault();
        }

        #region Utilities

        private ClaimsIdentity GenerateClaimsIdentity(UserEntity user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(Core.Constants.ClaimTypes.UserId, user.Id.ToString()),
                new Claim(Core.Constants.ClaimTypes.Email, user.Email),
                new Claim(Core.Constants.ClaimTypes.UserName, user.FirstName + " " + user.LastName),
            };

            return new ClaimsIdentity(new GenericIdentity(user.Email, "Token"), userClaims);
        }

        #endregion
    }
}