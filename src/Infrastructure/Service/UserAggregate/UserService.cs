using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.RepositoryContracts.RoleAggregate;
using Application.RepositoryContracts.UserAggregate;
using Application.RepositoryContracts.UsersLoginHistoryAggregate;
using Application.ServiceContracts.Captcha;
using Application.ServiceContracts.Fundamental;
using Application.ServiceContracts.Notifier;
using Application.ServiceContracts.Otp;
using Application.ServiceContracts.UserAggregate;
using Application.ViewModels.AccountAggregate;
using Application.ViewModels.Captcha;
using Application.ViewModels.Fundamental;
using Application.ViewModels.Notifier;
using Application.ViewModels.RoleAggregate;
using Application.ViewModels.UserAggregate;
using Common.Exceptions;
using Common.Exceptions.UserManagement;
using Common.Helper;
using Domain.Entities;

namespace Service.UserAggregate
{
    public class UserService : IUserService
    {

        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ICaptchaService _captchaService;
        private readonly IOtpService _otpService;
        private readonly IRoleRepository _roleRepository;
        private readonly INotifierService _notifierService;
        private readonly IFundamentalService _fundamentalService;
        private readonly IUsersLoginHistoryRepository _usersLoginHistoryRepository;


        public UserService(IUserRepository userRepository, IPasswordHelper passwordHelper,
            ICaptchaService captchaService, IOtpService otpService,
            IJwtService jwtService, IRoleRepository roleRepository, INotifierService notifierService, IFundamentalService fundamentalService, IUsersLoginHistoryRepository usersLoginHistoryRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _captchaService = captchaService;
            _otpService = otpService;
            _roleRepository = roleRepository;
            _notifierService = notifierService;
            _fundamentalService = fundamentalService;
            _usersLoginHistoryRepository = usersLoginHistoryRepository;
        }

        private async Task<User?> GetExistUser(GetExistUserCommandModel register)
        {
            User? user = null;

            if (register.NationalCode != null)
            {
                user = await _userRepository.GetUserByNationalCode(register.NationalCode);
            }

            if (user == null && register.MobileNumber != null)
            {
                user = await _userRepository.GetUserByMobileNumber(register.MobileNumber);
            }

            if (user == null && register.UserId != null)
            {
                user = await _userRepository.GetById((long)register.UserId);
            }

            return user;
        }

        public async Task<RegisterUserBaseViewMode> Register(RegisterUserAddCommandModel register)
        {
            switch (register.RegisterType)
            {
                case RegisterType.RegisterWithPasswordAndOtp:
                case RegisterType.RegisterWithPassword:
                    if (register.MobileNumber == null || register.Password == null)
                    {
                        throw UserManagementExceptions.RegisterBadRequestException;
                    }

                    break;
                case RegisterType.RegisterWithOtp:
                    if (register.MobileNumber == null)
                    {
                        throw UserManagementExceptions.RegisterBadRequestException;
                    }

                    break;
            }


            var checkModel = new CheckCaptchaAddCommandModel(register.CaptchaId, register.CaptchaCode);
            var captchaCheck = await _captchaService.Check(checkModel);
            if (captchaCheck.Status == false)
            {
                throw UserManagementExceptions.CptchaIsWrongException;
            }

            var existUserModel = new GetExistUserCommandModel()
            {
                MobileNumber = register.MobileNumber,
                NationalCode = register.NationalCode,
                UserId = register.UserId,
            };
            var existUser = await GetExistUser(existUserModel);

            if (existUser != null)
            {
                // todo:call shahkar
                if (!existUser.RegisterComplete)
                {
                    var updateUserOtp = _otpService.GenerateOtp(existUser.Id);
                    existUser.Phone = register.MobileNumber;
                    existUser.IsOtpUsed = false;
                    existUser.OtpCode = updateUserOtp;
                    existUser.OtpCodeCreatedTime = DateTime.Now;
                    _userRepository.UpdateUser(existUser);
                    var sendMessageModel = new SendMessageCommandModel()
                    {
                        Body = updateUserOtp,
                        Channel = 1,
                        Destination = register.MobileNumber,
                        MessageTypeId = 2,
                        Title = "Register Otp"

                    };
                    await _notifierService.SendMessageAsync(sendMessageModel);
                    return new RegisterUserBaseViewMode()
                    {
                        FirstName = existUser.FirstName,
                        LastName = existUser.LastName,
                        IsActive = true,
                        MobileNumber = register.MobileNumber,
                        UserId = existUser.Id,
                    };
                }
                else
                {
                    throw UserManagementExceptions.UseralreadyregisteredException;
                }
            }


            var reqUser = new User()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Phone = register.MobileNumber,
                ApplicationFk = register.AppId,
                Email = null,
                IsActive = true,
                EmailConfirm = false,
                IsDeleted = false,
                OtpConfirm = false,
                Password = null,
                NationalCode = null,
                RegisterComplete = false
            };
            User registeredUser;
            string? otp;
            switch (register.RegisterType)
            {
                case RegisterType.RegisterWithPassword:
                    {
                        if (register.Password == null)
                            throw UserManagementExceptions.PasswordNotValidException;
                        if (!register.Password.CheckPassword())
                        {
                            throw UserManagementExceptions.PasswordNotValidException;
                        }

                        var pass = _passwordHelper.EncodePasswordBcrypt(register.Password);
                        reqUser.Password = pass;
                        reqUser.RegisterComplete = true;
                        reqUser.IsActive = true;
                        registeredUser = await _userRepository.RegisterUser(reqUser);

                        var roleToUserModel = new RoleToUserAddCommandModel()
                        {
                            UserId = registeredUser.Id,
                            RoleIds = new List<long> { 3 }
                        };

                        var addNormalUser = await _roleRepository.AddNormalUserRoleToUser(roleToUserModel);
                        if (!addNormalUser.IsSucced)
                        {
                            throw UserManagementExceptions.CantAssignRoleToUserException;
                        }

                        otp = _otpService.GenerateOtp(registeredUser.Id);

                        registeredUser.OtpCode = otp;
                        registeredUser.IsOtpUsed = false;
                        registeredUser.OtpCodeCreatedTime = DateTime.Now;
                        _userRepository.UpdateUser(registeredUser);
                        var sendMessageModel = new SendMessageCommandModel()
                        {
                            Body = otp,
                            Channel = 1,
                            Destination = register.MobileNumber,
                            MessageTypeId = 2,
                            Title = "Register Otp"

                        };
                        await _notifierService.SendMessageAsync(sendMessageModel);
                        break;
                    }
                case RegisterType.RegisterWithOtp:
                    {
                        if (register.NationalCode == null)
                            throw UserManagementExceptions.NationalCodeNotValidException;
                        if (!register.NationalCode.IsValid())
                        {
                            throw UserManagementExceptions.NationalCodeNotValidException;
                        }

                        reqUser.NationalCode = register.NationalCode;
                        var customerModel = new CustomerCommandModel()
                        {
                            MobileNumber = register.MobileNumber,
                            NationalId = register.NationalCode
                        };
                        var customer = await _fundamentalService.Customer(customerModel);
                        if (!customer.isActive || !customer.isExists)
                        {
                            var massageError = new ContentExceptionCommandModel()
                            {
                                Key = "NotExistError"
                            };
                            var massage = await _fundamentalService.ContentException(massageError);
                            throw new IdentityException(-1425, massage.ErrorMessage, 404);
                        }
                        if (!customer.isMobileVerified)
                        {
                            var shahkarModel = new ShahkarCommandModel()
                            {
                                MobileNumber = register.MobileNumber,
                                NationalCode = register.NationalCode,
                            };
                            var shahkar = await _fundamentalService.Shahkar(shahkarModel);
                            if (!shahkar.matched)
                            {
                                var massageError = new ContentExceptionCommandModel()
                                {
                                    Key = "ShahkarError"
                                };
                                var massage = await _fundamentalService.ContentException(massageError);
                                throw new IdentityException(-1426, massage.ErrorMessage, 400);
                            }
                        }
                        reqUser.FirstName = customer.firstname;
                        reqUser.LastName = customer.lastname;
                        reqUser.CustomerNumber = (customer?.customerNumber) ?? long.Parse(register.MobileNumber);
                        reqUser.IsActive = true;
                        registeredUser = await _userRepository.RegisterUser(reqUser);

                        var roleToUserModel = new RoleToUserAddCommandModel()
                        {
                            UserId = registeredUser.Id,
                            RoleIds = new List<long> { 3 }
                        };
                        var addNormalUser = await _roleRepository.AddNormalUserRoleToUser(roleToUserModel);
                        if (!addNormalUser.IsSucced)
                        {
                            throw UserManagementExceptions.CantAssignRoleToUserException;
                        }

                        otp = _otpService.GenerateOtp(registeredUser.Id);
                        registeredUser.OtpCode = otp;
                        registeredUser.IsOtpUsed = false;
                        registeredUser.OtpCodeCreatedTime = DateTime.Now;
                        _userRepository.UpdateUser(registeredUser);

                        var sendMessageModel = new SendMessageCommandModel()
                        {
                            Body = otp,
                            Channel = 1,
                            Destination = register.MobileNumber,
                            MessageTypeId = 2,
                            Title = "Register Otp"

                        };
                        await _notifierService.SendMessageAsync(sendMessageModel);
                        //instead of this send otp and if that had error rollback the user

                        break;
                    }
                case RegisterType.RegisterWithPasswordAndOtp:
                    {
                        if (register.Password == null)
                            throw UserManagementExceptions.PasswordNotValidException;
                        if (!register.Password.CheckPassword())
                        {
                            throw UserManagementExceptions.PasswordNotValidException;
                        }

                        var pass = _passwordHelper.EncodePasswordBcrypt(register.Password);
                        reqUser.Password = pass;
                        reqUser.IsActive = true;

                        registeredUser = await _userRepository.RegisterUser(reqUser);

                        var roleToUserModel = new RoleToUserAddCommandModel()
                        {
                            UserId = registeredUser.Id,
                            RoleIds = new List<long> { 3 }
                        };
                        var addNormalUser = await _roleRepository.AddNormalUserRoleToUser(roleToUserModel);
                        if (!addNormalUser.IsSucced)
                        {
                            throw UserManagementExceptions.CantAssignRoleToUserException;
                        }

                        otp = _otpService.GenerateOtp(registeredUser.Id);
                        registeredUser.OtpCode = otp;
                        registeredUser.IsOtpUsed = false;
                        registeredUser.OtpCodeCreatedTime = DateTime.Now;
                        _userRepository.UpdateUser(registeredUser);

                        var sendMessageModel = new SendMessageCommandModel()
                        {
                            Body = otp,
                            Channel = 1,
                            Destination = register.MobileNumber,
                            MessageTypeId = 2,
                            Title = "Register Otp"

                        };
                        await _notifierService.SendMessageAsync(sendMessageModel);
                        break;
                    }

                default:
                    throw UserManagementExceptions.RegisterTypeNotFoundException;
            }

            return new RegisterUserBaseViewMode()
            {
                AppId = registeredUser.ApplicationFk,
                FirstName = registeredUser.FirstName,
                LastName = registeredUser.LastName,
                IsActive = true,
                MobileNumber = registeredUser.Phone,
                UserId = registeredUser.Id,
                NationalCode = registeredUser.NationalCode,
            };
        }

        public async Task<RegisterCompletedBaseViewModel> RegisterCompleted(RegisterCompletedAddCommandModel model)
        {
            var user = await _userRepository.GetById(model.UserId);
            if (user == null)
            {
                throw UserManagementExceptions.UserNotFoundException;
            }
            var captchaModel = new CheckCaptchaAddCommandModel(model.CaptchaId, model.CaptchaCode);

            var captchaCheck = await _captchaService.Check(captchaModel);


            if (captchaCheck.Status == false)
            {
                throw UserManagementExceptions.CptchaIsWrongException;
            }

            if (user.OtpCode == model.OtpCode && DateTime.Now.Subtract((DateTime)user.OtpCodeCreatedTime) < TimeSpan.FromMinutes(2) && user.IsOtpUsed==false)
            {
                var sendMessageModel = new SendMessageCommandModel()
                {
                    Body = "",
                    Channel = 1,
                    Destination = user.Phone,
                    MessageTypeId = 5,
                    Title = "Register Welcome Message"

                };
                await _notifierService.SendMessageAsync(sendMessageModel);
                user.OtpConfirm = true;
                user.RegisterComplete = true;
                user.IsOtpUsed = true;
                _userRepository.UpdateUser(user);
                return new RegisterCompletedBaseViewModel()
                {
                    IsCompleted = true,
                };
            }
            else
            {
                return new RegisterCompletedBaseViewModel()
                {
                    IsCompleted = false,
                };
            }
        }

        public async Task<TokenResponseBaseViewModel> LoginWithMobileNumberAndPassword(
            LoginWithMobileNumberAndPasswordAddCommandModel login)
        {
            var user = await _userRepository.GetUserByMobileNumber(login.MobileNumber);

            if (user == null)
            {
                throw UserManagementExceptions.InvalidLoginException;
            }
            if (user.IsActive == false)
            {
                throw UserManagementExceptions.UserNotActiveException;
            }
            if (user.OtpConfirm == false || user.RegisterComplete == false)
            {
                throw UserManagementExceptions.RegisterNotCompletedException;
            }

            var captchaModel = new CheckCaptchaAddCommandModel(login.CaptchaId, login.CaptchaCode);

            var captchaCheck = await _captchaService.Check(captchaModel);


            if (captchaCheck.Status == false)
            {
                throw UserManagementExceptions.CptchaIsWrongException;
            }

            if (user.Password == null)
            {
                throw UserManagementExceptions.PasswordNotValidException;
            }

            var verify = _passwordHelper.VerifyPassword(login.Password, user.Password);
            if (!verify)
            {
                throw UserManagementExceptions.InvalidLoginException;
            }

            var userPermissionIdsModel = new GetUserPermissionIdsCommandModel()
            {
                UserId = user.Id
            };
            var userPermissionIds = await _userRepository.GetUserPermissionIds(userPermissionIdsModel);
            var checkAdmin = await _userRepository.CheckUserIsAdmin(user.Id);


            var generateToken = new FullTokenResponseCommandModel()
            {
                PermissionIds = userPermissionIds,
                UserId = user.Id,
                ApplicationId = user.ApplicationFk,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                IsAdmin = checkAdmin,
                CustomerNumber = user.CustomerNumber
            };

            var token = _jwtService.GenerateFullToken(generateToken);


            return new TokenResponseBaseViewModel()
            {
                Token = token.Token,
                RefreshToken = token.RefreshToken,

            };
        }

        public async Task<LoginWithNationalCodeBaseViewModel> LoginWithNationalCode(
            LoginWithNationalCodeAddCommandModel login)
        {
            var user = await _userRepository.GetUserByNationalCode(login.NationalCode);
            if (user == null)
            {
                var massageError = new ContentExceptionCommandModel()
                {
                    Key = "InvalidCredential"
                };
                var massage = await _fundamentalService.ContentException(massageError);
                throw new IdentityException(-1427, massage.ErrorMessage, 400);
            }

            var captchaModel = new CheckCaptchaAddCommandModel(login.CaptchaId, login.CaptchaCode);

            var captchaCheck = await _captchaService.Check(captchaModel);


            if (captchaCheck.Status == false)
            {
                throw UserManagementExceptions.CptchaIsWrongException;
            }


            var otp = _otpService.GenerateOtp(user.Id);
            user.OtpCode = otp;
            user.IsOtpUsed = false;
            user.OtpCodeCreatedTime = DateTime.Now;
            _userRepository.UpdateUser(user);
            var sendMessageModel = new SendMessageCommandModel()
            {
                Body = otp,
                Channel = 1,
                Destination = user.Phone,
                MessageTypeId = 1,
                Title = "Login Otp"

            };
            await _notifierService.SendMessageAsync(sendMessageModel);
            return new LoginWithNationalCodeBaseViewModel()
            {
                UserId = user.Id,
                IsSend = true,
                MobileNumber = FormatPhoneNumber(user.Phone)
            };
        }

        public async Task<TokenResponseBaseViewModel> CompleteLoginWithNationalCode(CompleteLoginWithNationalCode model)
        {

            var user = await _userRepository.GetUserByNationalCode(model.NationalCode);
            if (user == null)
            {
                throw UserManagementExceptions.InvalidLoginException;
            }
            if (user.IsActive == false)
            {
                throw UserManagementExceptions.UserNotActiveException;
            }
            var checkModel = new CheckCaptchaAddCommandModel(model.CaptchaId, model.CaptchaCode);
            //var captchaCheck = await _captchaService.Check(checkModel);
            //if (captchaCheck.Status == false)
            //{
            //    throw UserManagementExceptions.CptchaIsWrongException;
            //}
            if (user.OtpCode == model.OtpCode && DateTime.Now.Subtract((DateTime)user.OtpCodeCreatedTime) < TimeSpan.FromMinutes(2) && user.IsOtpUsed==false)
            {
                var userPermissionIdsModel = new GetUserPermissionIdsCommandModel()
                {
                    UserId = user.Id
                };
                var userPermissionIds = await _userRepository.GetUserPermissionIds(userPermissionIdsModel);
                var checkAdmin = await _userRepository.CheckUserIsAdmin(user.Id);

                var generateToken = new FullTokenResponseCommandModel()
                {
                    PermissionIds = userPermissionIds,
                    UserId = user.Id,
                    ApplicationId = user.ApplicationFk,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    IsAdmin = checkAdmin,
                    CustomerNumber = user.CustomerNumber

                };

                var token = _jwtService.GenerateFullToken(generateToken);
                var sendMessageModel = new SendMessageCommandModel()
                {
                    Body = "",
                    Channel = 1,
                    Destination = user.Phone,
                    MessageTypeId = 4,
                    Title = "Login Welcome Message"

                };
                await _notifierService.SendMessageAsync(sendMessageModel);
                user.IsOtpUsed = true;
                _userRepository.UpdateUser(user);

                return new TokenResponseBaseViewModel()
                {
                    Token = token.Token,
                    RefreshToken = token.RefreshToken,

                };
            }
            else
            {
                throw UserManagementExceptions.OTPNotValidException;
            }
        }

        public async Task<bool> CheckUserPermissionAsync(CheckUserPermissionAddCommandModel model)
        {
            var userIdAndRoles = await GetRolesAndUserIdFromTokenAsync(model.Token);

            var checkModel = new GetRolesAndUserIdFromTokenAsyncAddCommandModel()
            {
                title = model.Title,
                userId = userIdAndRoles.UserId,
            };

            return await _userRepository.CheckUserPermissionAsync(checkModel);
        }

        public async Task<GetRolesAndUserIdFromTokenBaseViewModel> GetRolesAndUserIdFromTokenAsync(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(tokenString);

            var roles = token.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value)
                .ToList();
            var userId = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ??
                         throw UserManagementExceptions.TokenNotValidException;

            return await Task.FromResult(new GetRolesAndUserIdFromTokenBaseViewModel()
            {
                Roles = roles,
                UserId = long.Parse(userId)
            });
        }

        public async Task<GetUsersInfoBaseViewModel> GetUsersInfo(GetUsersInfoAddCommandModel model)
        {
            var usersInfo = await _userRepository.GetByIds(model.UsersIds);
            var res = new GetUsersInfoBaseViewModel();
            foreach (var user in usersInfo)
            {
                res.UsersInfo.Add((UserInfoViewModel)user);
            }

            return res;
        }

        public async Task<GetUsersWithRolesBaseViewModel> GetUsersWithRoles(GetUsersWithRolesAddCommandModel req)
        {
            var filterAllModel = new UserFilterCommandModel()
            {

                FirstName = req.FirstName,
                LastName = req.LastName,
                IsActive = req.IsActive,
                MobileNumber = req.MobileNumber,
                NationalCode = req.NationalCode,
                RoleNames = req.RoleNames,

            };
            var count = await _userRepository.FilterAllAsync(filterAllModel);
            var userRole = await _userRepository.GetUsersWithRoles(req.Page, req.PerPage);
            var res = new GetUsersWithRolesBaseViewModel()
            {
                Elements = userRole.Elements,
                TotalElements = count,
                TotalPages = (int)Math.Ceiling(count / (decimal)req.PerPage),
                HasNext = (req.Page + 1) < (int)Math.Ceiling(count / (decimal)req.PerPage),
                HasPrev = req.Page > 0 && (int)Math.Ceiling(count / (decimal)req.PerPage) != 0,
                Page = req.Page
            };
            return res;
        }

        public async Task<AdduserBaseViewModel> AddUser(AddUserCommandModel req)
        {
            //todo:check appId in req and Admin User
            switch (req.AddUserType)
            {
                case AddUserType.RegisterWithPasswordAndOtp:
                case AddUserType.RegisterWithPassword:
                    if (req.MobileNumber == null || req.Password == null)
                    {
                        throw UserManagementExceptions.RegisterBadRequestException;
                    }

                    break;
                case AddUserType.RegisterWithOtp:
                    if (req.MobileNumber == null || req.NationalCode == null)
                    {
                        throw UserManagementExceptions.RegisterBadRequestException;
                    }

                    break;
                default:
                    break;
            }


            var existUserModel = new GetExistUserCommandModel()
            {
                MobileNumber = req.MobileNumber,
                NationalCode = req.NationalCode,
                UserId = req.UserId,
            };
            var existUser = await GetExistUser(existUserModel);

            if (existUser != null)
            {
                // todo:call shahkar
                if (!existUser.RegisterComplete)
                {
                    existUser.Phone = req.MobileNumber;
                    existUser.OtpConfirm = true;
                    existUser.RegisterComplete = true;

                    _userRepository.UpdateUser(existUser);


                    return new AdduserBaseViewModel()
                    {
                        FirstName = existUser.FirstName,
                        LastName = existUser.LastName,
                        IsActive = true,
                        MobileNumber = req.MobileNumber,
                        UserId = existUser.Id,

                    };
                }
                else
                {
                    throw UserManagementExceptions.UseralreadyregisteredException;
                }
            }


            var reqUser = new User()
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Phone = req.MobileNumber,
                ApplicationFk = req.AppId,
                Email = null,
                IsActive = true,
                EmailConfirm = false,
                IsDeleted = false,
                OtpConfirm = true,
                Password = null,
                NationalCode = null,
                RegisterComplete = true
            };
            User registeredUser;
            switch (req.AddUserType)
            {
                case AddUserType.RegisterWithPassword:
                    {
                        if (req.Password == null)
                            throw UserManagementExceptions.PasswordNotValidException;
                        if (!req.Password.CheckPassword())
                        {
                            throw UserManagementExceptions.PasswordNotValidException;
                        }

                        var pass = _passwordHelper.EncodePasswordBcrypt(req.Password);
                        reqUser.Password = pass;
                        reqUser.RegisterComplete = true;
                        registeredUser = await _userRepository.RegisterUser(reqUser);

                        var roleToUserModel = new RoleToUserAddCommandModel()
                        {
                            UserId = registeredUser.Id,
                            RoleIds = new List<long> { 3 }
                        };

                        var addNormalUser = await _roleRepository.AddNormalUserRoleToUser(roleToUserModel);
                        if (!addNormalUser.IsSucced)
                        {
                            throw UserManagementExceptions.CantAssignRoleToUserException;
                        }
                        if (req.RoleIds == null || req.RoleIds.Count == 0)
                        {
                            break;
                        }
                        var updateRolesOfUserModel = new UpdateRolesOfUserCommandModel()
                        {
                            RoleIds = req.RoleIds,
                            UserId = registeredUser.Id
                        };
                        var resOfUpdateUserRoles = await _roleRepository.UpdateRolesOfUser(updateRolesOfUserModel);
                        if (!resOfUpdateUserRoles.IsSucced)
                        {
                            throw UserManagementExceptions.CantAssignRoleToUserException;
                        }

                        break;
                    }
                case AddUserType.RegisterWithOtp:
                    {
                        if (req.NationalCode == null)
                            throw UserManagementExceptions.NationalCodeNotValidException;
                        if (!req.NationalCode.IsValid())
                        {
                            throw UserManagementExceptions.NationalCodeNotValidException;
                        }

                        reqUser.NationalCode = req.NationalCode;
                        registeredUser = await _userRepository.RegisterUser(reqUser);

                        var roleToUserModel = new RoleToUserAddCommandModel()
                        {
                            UserId = registeredUser.Id,
                            RoleIds = new List<long> { 3 }
                        };
                        var addNormalUser = await _roleRepository.AddNormalUserRoleToUser(roleToUserModel);
                        if (!addNormalUser.IsSucced)
                        {
                            throw UserManagementExceptions.CantAssignRoleToUserException;
                        }
                        if (req.RoleIds == null || req.RoleIds.Count == 0)
                        {
                            break;
                        }
                        var updateRolesOfUserModel = new UpdateRolesOfUserCommandModel()
                        {
                            RoleIds = req.RoleIds,
                            UserId = registeredUser.Id
                        };
                        var resOfUpdateUserRoles = await _roleRepository.UpdateRolesOfUser(updateRolesOfUserModel);
                        if (!resOfUpdateUserRoles.IsSucced)
                        {
                            throw UserManagementExceptions.CantAssignRoleToUserException;
                        }
                        break;
                    }
                case AddUserType.RegisterWithPasswordAndOtp:
                    {
                        if (req.Password == null)
                            throw UserManagementExceptions.PasswordNotValidException;
                        if (!req.Password.CheckPassword())
                        {
                            throw UserManagementExceptions.PasswordNotValidException;
                        }

                        var pass = _passwordHelper.EncodePasswordBcrypt(req.Password);
                        reqUser.Password = pass;
                        registeredUser = await _userRepository.RegisterUser(reqUser);

                        var roleToUserModel = new RoleToUserAddCommandModel()
                        {
                            UserId = registeredUser.Id,
                            RoleIds = new List<long> { 3 }
                        };
                        var addNormalUser = await _roleRepository.AddNormalUserRoleToUser(roleToUserModel);
                        if (!addNormalUser.IsSucced)
                        {
                            throw UserManagementExceptions.CantAssignRoleToUserException;
                        }
                        if (req.RoleIds == null || req.RoleIds.Count == 0)
                        {
                            break;
                        }
                        var updateRolesOfUserModel = new UpdateRolesOfUserCommandModel()
                        {
                            RoleIds = req.RoleIds,
                            UserId = registeredUser.Id
                        };
                        var resOfUpdateUserRoles = await _roleRepository.UpdateRolesOfUser(updateRolesOfUserModel);
                        if (!resOfUpdateUserRoles.IsSucced)
                        {
                            throw UserManagementExceptions.CantAssignRoleToUserException;
                        }
                        break;
                    }

                default:
                    throw UserManagementExceptions.RegisterTypeNotFoundException;
            }

            return new AdduserBaseViewModel()
            {
                AppId = registeredUser.ApplicationFk,
                FirstName = registeredUser.FirstName,
                LastName = registeredUser.LastName,
                IsActive = true,
                MobileNumber = registeredUser.Phone,
                UserId = registeredUser.Id,
                NationalCode = registeredUser.NationalCode,
            };
        }

        public async Task<GetUserWithRoleBaseViewModel> GetUserWithRoles(GetUserWithRoleCommandModel req)
        {
            var user = await _userRepository.GetById(req.UserId);
            if (user == null)
            {
                throw UserManagementExceptions.UserNotFoundException;
            }

            return await _userRepository.GetUserWithRols(req);
        }

        public async Task<UpdatePasswordBaseViewModel> UpdatePassword(UpdatePasswordCommandModel req)
        {
            var user = await _userRepository.GetById(req.UserId);
            if (!req.Password.CheckPassword())
            {
                throw UserManagementExceptions.PasswordNotValidException;
            }
            var pass = _passwordHelper.EncodePasswordBcrypt(req.Password);
            user.Password = pass;
            _userRepository.UpdateUser(user);
            return new UpdatePasswordBaseViewModel()
            {
                IsSucced = true,
            };
        }

        public async Task<UpdateUserWithRolesBaseViewModel> UpdateUserWithRoles(UpdateUserWithRolesCommandModel req, long UserId, List<string> permissionIds)
        {
            // Check National Code validity
            if (!req.NationalCode.IsValid())
            {
                throw UserManagementExceptions.NationalCodeNotValidException;
            }

            // Check Mobile Number validity
            if (!req.MobileNumber.CheckMobileNo())
            {
                throw UserManagementExceptions.MobileNumberNotValidException;
            }

            // Get user roles
            var getUserWithRolesModel = new GetUserWithRoleCommandModel()
            {
                UserId = req.UserId,
            };
            var userRoles = await _userRepository.GetUserWithRols(getUserWithRolesModel) ?? throw UserManagementExceptions.UserNotFoundException;

            // Get user by Id
            var user = await _userRepository.GetById(req.UserId);

            // Update user information if National Code or Mobile Number is provided
            if (!string.IsNullOrEmpty(req.NationalCode) || !string.IsNullOrEmpty(req.MobileNumber))
            {
                var userWithNationalCode = await _userRepository.GetUserByNationalCode(req.NationalCode);
                var userWithMobileNumber = await _userRepository.GetUserByMobileNumber(req.MobileNumber);

                if (userWithNationalCode == null)
                {
                    if (userWithMobileNumber == null)
                    {
                        UpdateUserInformation(user, req);
                    }
                    else if (userWithMobileNumber.Id == user.Id)
                    {
                        UpdateUserInformation(user, req);
                    }
                    else
                    {
                        throw UserManagementExceptions.MobileNumberOrNationalCodeException;
                    }

                    UpdateUserInformation(user, req);
                }
                else if (userWithMobileNumber == null)
                {
                    if (userWithNationalCode == null)
                    {
                        UpdateUserInformation(user, req);
                    }
                    else if (userWithNationalCode.Id == user.Id)
                    {
                        UpdateUserInformation(user, req);
                    }
                    else
                    {
                        throw UserManagementExceptions.MobileNumberOrNationalCodeException;
                    }
                }
                else
                {
                    // Check if the user with National Code is the same as the current user
                    if ((userWithNationalCode.Id == user.Id && userWithNationalCode.NationalCode == req.NationalCode) && (userWithMobileNumber.Id == user.Id && userWithMobileNumber.Phone == req.MobileNumber))
                    {
                        UpdateUserInformation(user, req);
                    }
                    else
                    {
                        throw UserManagementExceptions.MobileNumberOrNationalCodeException;
                    }
                }
            }
            else
            {
                // Update user information if National Code or Mobile Number is not provided
                UpdateUserInformation(user, req);
            }

            // Update user roles
            var updateRolesOfUserModel = new UpdateRolesOfUserCommandModel()
            {
                RoleIds = req.RoleIds,
                UserId = req.UserId
            };
            _userRepository.UpdateUser(user);

            // Check if the current user is an administrator
            var isUserAdminstrator = await _roleRepository.IsUserAdministrator(UserId);
            string permissionOfAssignmentRoles = "193";
            var UserHasRoleAssignmentAccess = permissionIds.Any(id => id == permissionOfAssignmentRoles);
            if (!isUserAdminstrator)
            {
                if (!UserHasRoleAssignmentAccess)
                {
                    throw UserManagementExceptions.AddAdministratorRoleToUserException;
                }
                else
                {
                    if (req.RoleIds.Any(id => id == 1) || userRoles.UserRoles.Select(a => a.RoleId).Contains(1))
                    {

                        throw UserManagementExceptions.AddAdministratorRoleToUserException;
                    }
                    // Update user roles
                    var resOfUpdateUserRoles = await _roleRepository.UpdateRolesOfUser(updateRolesOfUserModel);

                    // Return the result
                    return new UpdateUserWithRolesBaseViewModel()
                    {
                        UserId = req.UserId,
                    };

                }
            }
            else
            {
                // Update user roles
                var resOfUpdateUserRoles = await _roleRepository.UpdateRolesOfUser(updateRolesOfUserModel);

                // Return the result
                return new UpdateUserWithRolesBaseViewModel()
                {
                    UserId = req.UserId,
                };

            }


        }

        private void UpdateUserInformation(User user, UpdateUserWithRolesCommandModel req)
        {
            user.FirstName = req.FirstName;
            user.LastName = req.LastName;
            user.Phone = req.MobileNumber;
            user.NationalCode = req.NationalCode;
        }


        public async Task<ActiveDeductiveUserViewModel> ActiveInactiveUser(ActiveDeductiveUserCommandModel model)
        {
            var user = await _userRepository.GetById(model.UserId);
            user.IsActive = model.IsActive;
            _userRepository.UpdateUser(user);
            return new ActiveDeductiveUserViewModel()
            {
                IsSuccess = true
            };
        }

        public async Task<GenerateTokenWithRefreshTokenBaseViewModel> GenerateTokenWithRefreshToken(GenerateTokenWithRefreshTokenCommandModel req)
        {
            var validateRefreshToken = _jwtService.ValidateRefreshToken(req.RefreshToken);
            if (!validateRefreshToken)
            {
                throw UserManagementExceptions.RefreshTokenNotValidException;
            }
            var userId = await _jwtService.DecodeRefreshToken(req.RefreshToken);

            var user = await _userRepository.GetById(long.Parse(userId.UserId));


            var userPermissionIdsModel = new GetUserPermissionIdsCommandModel()
            {
                UserId = user.Id
            };
            var userPermissionIds = await _userRepository.GetUserPermissionIds(userPermissionIdsModel);
            var checkAdmin = await _userRepository.CheckUserIsAdmin(user.Id);

            var generateToken = new GenerateTokenAddCommandModel()
            {
                PermissionIds = userPermissionIds,
                UserId = user.Id,
                ApplicationId = user.ApplicationFk,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                IsAdmin = checkAdmin,
                CustomerNumber = user.CustomerNumber
            };

            var token = _jwtService.GenerateToken(generateToken);

            return new GenerateTokenWithRefreshTokenBaseViewModel()
            {
                Token = token.Token
            };

        }

        public async Task<GetUserByIdBaseViewModel> GetUserById(GetUserByIdCommandModel req)
        {
            var res = await _userRepository.GetById(req.UserId);
            return new GetUserByIdBaseViewModel()
            {
                CustomerNumber = res.CustomerNumber,
                FirstName = res.FirstName,
                LastName = res.LastName,
                Phone = res.Phone,
                NationalCode = res.NationalCode,
            };
        }

        public async Task SendOtp(SendOtpAddCommandModel model)
        {
            var user = await _userRepository.GetById(model.UserId);
            var otp = _otpService.GenerateOtp(user.Id);
      
            if (DateTime.Now.Subtract((DateTime)user.OtpCodeCreatedTime) < TimeSpan.FromMinutes(2))
            {
                throw UserManagementExceptions.OtpCodeException;
            }
      
            if (model.Title == TitleType.LoginOtp)
            {
                var sendMessageModel = new SendMessageCommandModel()
                {
                    Body = otp,
                    Channel = 1,
                    Destination = user.Phone,
                    MessageTypeId = 1,
                    Title = "Login Otp"

                };
                await _notifierService.SendMessageAsync(sendMessageModel);
                user.OtpCode = otp;
                user.IsOtpUsed = false;
                user.OtpCodeCreatedTime = DateTime.Now;
                _userRepository.UpdateUser(user);
            }
            else if (model.Title == TitleType.ForgotOtp)
            {
                var sendMessageModel = new SendMessageCommandModel()
                {
                    Body = otp,
                    Channel = 1,
                    Destination = user.Phone,
                    MessageTypeId = 1,
                    Title = "ForgotOtp"

                };
                await _notifierService.SendMessageAsync(sendMessageModel);
                user.OtpCode = otp;
                user.OtpCodeCreatedTime = DateTime.Now;
                _userRepository.UpdateUser(user);
            }
            else
            {
                var sendMessageModel = new SendMessageCommandModel()
                {
                    Body = otp,
                    Channel = 1,
                    Destination = user.Phone,
                    MessageTypeId = 2,
                    Title = "Register Otp"

                };
                await _notifierService.SendMessageAsync(sendMessageModel);
                user.OtpCode = otp;
                user.OtpCodeCreatedTime = DateTime.Now;
                _userRepository.UpdateUser(user);
            }
        }

        public async Task<GetUsersForBiBaseViewModel> FilterForBiAsync(GetUsersForBiAddCommandModel model)
        {

            var userList = await _userRepository.FilterForBiAsync(model);
            var users = userList.Select(a => new UserForBiResponse()
            {
                CreateDate = a.CreatedAt,
                Email = a.Email,
                FirstName = a.FirstName,
                LastName = a.LastName,
                IsActive = a.IsActive,
                MobileNumber = a.Phone,
                NationalCode = a.NationalCode,
                UserId = a.Id,
                CostumerNumber = a.CustomerNumber
            }).ToList();
            var res = new GetUsersForBiBaseViewModel()
            {
                users = users
            };

            return res;
        }

        public async Task<List<GetUsersLoginHistoryForBiBaseViewModel>> GetUsersLoginHistoryForBi(GetUsersLoginHistoryForBiAddCommandModel model)
        {
            var loginHistory = await _userRepository.GetUsersLoginHistoryForBi(model);
            var res = loginHistory.Select(x => new GetUsersLoginHistoryForBiBaseViewModel()
            {
                UserId = x.UserFk,
                LogDate = x.LogDate,
                HistoryType = x.HistoryType,


            }).ToList();
            return res;
        }

        public async Task<ForgotPasswordUserInformationViewModel> ForgotPasswordUserInformation(ForgotPasswordUserInformationCommandModel model)
        {

            var user = await _userRepository.GetUserByMobileNumber(model.MobileNumber);
            if (user == null)
            {
                throw UserManagementExceptions.UserNotFoundException;
            }

            var checkModel = new CheckCaptchaAddCommandModel(model.CaptchaId, model.CaptchaCode);
            var captchaCheck = await _captchaService.Check(checkModel);
            if (captchaCheck.Status == false)
            {
                throw UserManagementExceptions.CptchaIsWrongException;
            }
            var otp = _otpService.GenerateOtp(user.Id);
            user.OtpCode = otp;
            user.IsOtpUsed = false;
            user.OtpCodeCreatedTime = DateTime.Now;
            _userRepository.UpdateUser(user);
            // todo:change Sms Model
            var sendMessageModel = new SendMessageCommandModel()
            {
                Body = otp,
                Channel = 1,
                Destination = user.Phone,
                MessageTypeId = 3,
                Title = "ForgotOtp"

            };
            await _notifierService.SendMessageAsync(sendMessageModel);
            return new ForgotPasswordUserInformationViewModel()
            {
                UserId = user.Id,
            };
        }

        public async Task<GetOtpForForgotPasswordViewModel> GetOtpForForgotPassword(GetOtpForForgotPasswordCommandModel model)
        {
            var user = await _userRepository.GetById(model.UserId);
            var checkModel = new CheckCaptchaAddCommandModel(model.CaptchaId, model.CaptchaCode);
            var captchaCheck = await _captchaService.Check(checkModel);
            if (captchaCheck.Status == false)
            {
                throw UserManagementExceptions.CptchaIsWrongException;
            }

            if (user.OtpCode == model.Otp && DateTime.Now.Subtract((DateTime)user.OtpCodeCreatedTime) < TimeSpan.FromMinutes(2) && user.IsOtpUsed == false)
            {
                var codeModel = new GenerateTokenForForgotPasswordCommandModel()
                {
                    UserId = model.UserId,
                };
                var code = await Task.Run(() => _jwtService.GenerateTokenForForgotPassword(codeModel));
                user.IsOtpUsed = true;
                _userRepository.UpdateUser(user);
                return new GetOtpForForgotPasswordViewModel()
                {
                    Token = code
                };
            }
            else
            {
                throw UserManagementExceptions.OTPNotValidException;
            }
        }
        public async Task<ChangeForgotPasswordViewModel> ChangeForgotPassword(ChangeForgotPasswordCommandModel model)
        {
            var validCode = await Task.Run(() => _jwtService.ValidateTokenForForgotPassword(model.Code));
            if (!validCode)
            {
                throw UserManagementExceptions.ForgotTokenNotValidException;
            }
            var decodeToken = await _jwtService.DecodeTokenForForgotPassword(model.Code);
            if (model.Password == null)
                throw UserManagementExceptions.PasswordNotValidException;
            if (!model.Password.CheckPassword())
            {
                throw UserManagementExceptions.PasswordNotValidException;
            }
            var pass = await Task.Run(() => _passwordHelper.EncodePasswordBcrypt(model.Password));
            var user = await _userRepository.GetById(long.Parse(decodeToken.UserId));
            user.Password = pass;
            await Task.Run(() => _userRepository.UpdateUser(user));
            return new ChangeForgotPasswordViewModel()
            {
                IsSuccess = true,
            };
        }

        public async Task<UserCountReportViewModel> UserCountReport()
        {
            var AllUsersCount = await _userRepository.GetTotalUserCountAsync();
            var LastMonthUsersCount = await _userRepository.GetUsersCreatedLastMonthAsync();
            var LastWeekUsersCount = await _userRepository.GetUsersCreatedLastWeekAsync();
            var YesterdayUsersCount = await _userRepository.GetUsersCreatedYesterdayAsync();
            return new UserCountReportViewModel()
            {
                AllUsersCount = AllUsersCount,
                LastMonthUsersCount = LastMonthUsersCount,
                LastWeekUsersCount = LastWeekUsersCount,
                YesterdayUsersCount = YesterdayUsersCount
            };
        }
        private string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != 11)
            {
                return phoneNumber;
            }
            string formattedNumber = $"{phoneNumber.Substring(0, 4)}****{phoneNumber.Substring(8)}";

            return formattedNumber;
        }

        public async Task<GetUserByCustomerNumberBaseViewModel> GetUserByCustomerNumber(GetUserByCustomerNumberCommandModel req)
        {
            var res = await _userRepository.GetByCustomerNumber(req.customerNumber);
            return new GetUserByCustomerNumberBaseViewModel()
            {
                FirstName = res.FirstName,
                LastName = res.LastName
            };
        }
    }
}