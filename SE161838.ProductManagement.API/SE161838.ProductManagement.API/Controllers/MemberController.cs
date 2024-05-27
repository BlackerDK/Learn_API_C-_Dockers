using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SE161838.ProductManagement.Repo.Interface;
using SE161838.ProductManagement.Repo.Models;
using SE161838.ProductManagement.Repo.Repository;
using SE161838.ProductManagement.Repo.ResponeModel;
using SE161838.ProductManagement.Repo.ResponeModels;
using SE161838.ProductManagement.Repo.ViewModels.Member;
using SE161838.ProductManagement.Repo.ViewModels.Product;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace SE161838.ProductManagement.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private int? page_size = 200;
        private int? index_page = 1;
        public MemberController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
        }
        [AllowAnonymous]
        [HttpPost()]
        public async Task<ActionResult<MemberViewModels>> Login([FromBody] MemberLoginViewModels login)
        {
            var Member = _unitOfWork.MembersRepository.Get(m => m.Email.Equals(login.Email) && m.Password.Equals(login.Password)).FirstOrDefault();
            if (Member == null)
            {
                return Unauthorized(new FailedResponseModel
                {
                    Status = Unauthorized().StatusCode,
                    Message = "Invalid email or password"
                });
            }
            //config token
            return Ok(new ResponeModel
            {
                Status =Ok().StatusCode,
                Message = "Login successfully",
                Result = GenerateJSONWebToken(Member)
            });
        }

        private string GenerateJSONWebToken(Member Member)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("MemberId", Member.MemberId.ToString()),
                    new Claim("Email", Member.Email),
                    new Claim(ClaimTypes.Role, Member.Role.ToString()),
                    new Claim("CompanyName", Member.CompanyName),
                    new Claim ("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Member>>> GetMembers([FromQuery] SearchMemberViewModels search)
        //{
        //    if (search.pageSize != null)
        //    {
        //        page_size = search.pageSize;
        //    }
        //    if (search.currentPage != null)
        //    {
        //        index_page = search.currentPage;
        //    }
        //    Expression<Func<Member, bool>> filter = null;
        //    if (search.email != null)
        //    {
        //        filter = filter.And(p => p.Email.Contains(search.email));
        //    }
        //    if (search.City != null)
        //    {
        //        filter = filter.And(p => p.City.Contains(search.City));
        //    }
        //    if (search.CompanyName != null)
        //    {
        //        filter = filter.And(p => p.CompanyName.Contains(search.CompanyName));
        //    }
        //    if (search.Country != null)
        //    {
        //        filter = filter.And(p => p.Country.Contains(search.Country));
        //    }
        //    Func<IQueryable<Member>, IOrderedQueryable<Member>> orderBy = null;
        //    var Members = _unitOfWork.MembersRepository.Get(filter, null, "", index_page, page_size).ToList();
        //    var list = _mapper.Map<IEnumerable<MemberViewModels>>(Members);
        //    var total = _unitOfWork.MembersRepository.Get(filter).Count();
        //    MemberModelResponse result = new MemberModelResponse();
        //    result.total = total;
        //    result.currentPage = index_page.Value;
        //    result.users = list.ToList();
        //    return Ok(result);

        //}


        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetMemberById(int id)
        //{
        //    try
        //    {
        //        if (_unitOfWork.MembersRepository == null)
        //        {
        //            return NotFound(new FailedResponseModel()
        //            {
        //                Status = NotFound().StatusCode,
        //                Message = "member is not found !"
        //            });
        //        }
        //        var member = await _unitOfWork.MembersRepository.GetByIdAsync(id);
        //        if (member != null)
        //        {
        //            var result = _mapper.Map<MemberViewModels>(member);
        //            return Ok(new ResponeModel()
        //            {
        //                Status = Ok().StatusCode,
        //                Message = "Get Member by Id Success",
        //                Result = result
        //            });

        //        }
        //        return NotFound(new FailedResponseModel()
        //        {
        //            Status = NotFound().StatusCode,
        //            Message = "Member is not found !"
        //        });
        //    }
        //    catch (DirectoryNotFoundException ex)
        //    {
        //        return BadRequest(new FailedResponseModel()
        //        {
        //            Status = NotFound().StatusCode,
        //            Message = ex.Message
        //        });
        //    }
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateMemberById(int Id, MemberUpdateViewModels memberUpdate)
        //{
        //    try
        //    {
        //        var exitMember = await _unitOfWork.MembersRepository.GetByIdAsync(Id);
        //        if (exitMember == null)
        //        {
        //            return BadRequest(new FailedResponseModel()
        //            {
        //                Status = BadRequest().StatusCode,
        //                Message = "Member not exist with id " + Id
        //            });
        //        }
        //        exitMember.Email = memberUpdate.Email;
        //        exitMember.City = memberUpdate.City;
        //        exitMember.Country = memberUpdate.Country;
        //        exitMember.CompanyName = memberUpdate.CompanyName;
        //        exitMember.Password = memberUpdate.Password;
        //        _unitOfWork.MembersRepository.Update(exitMember);
        //        _unitOfWork.Save();
        //        return Ok(new ResponeModel
        //        {
        //            Status = Ok().StatusCode,
        //            Message = "Update product Success",
        //            Result = memberUpdate
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new FailedResponseModel()
        //        {
        //            Status = BadRequest().StatusCode,
        //            Message = ex.Message
        //        });
        //    }
        //}
        //[HttpPost]
        //[Consumes(MediaTypeNames.Application.Json)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //public async Task<IActionResult> AddMember(MemberCreateViewModels memberViewModel)
        //{
        //    try
        //    {
        //        var exitsMember = await _unitOfWork.MembersRepository.GetByIdAsync(memberViewModel.MemberId);
        //        if (exitsMember != null)
        //        {
        //            return BadRequest(new FailedResponseModel()
        //            {
        //                Status = BadRequest().StatusCode,
        //                Message = "Member has exist with id " + memberViewModel.MemberId
        //            });
        //        }
        //        var member = _mapper.Map<Member>(memberViewModel);
        //        await _unitOfWork.MembersRepository.AddAsync(member);
        //        _unitOfWork.Save();
        //        return CreatedAtAction(nameof(AddMember), new { id = member.MemberId }, new ResponeModel()
        //        {
        //            Status = 201,
        //            Message = "Add Member Successfully",
        //            Result = memberViewModel
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new FailedResponseModel()
        //        {
        //            Status = BadRequest().StatusCode,
        //            Message = ex.Message
        //        });
        //    }
        //}       
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMemberId(int id)
        //{
        //    try
        //    {
        //        var result = await _unitOfWork.MembersRepository.GetByIdAsync(id);
        //        if (result == null)
        //        {
        //            return NotFound(new FailedResponseModel()
        //            {
        //                Status = NotFound().StatusCode,
        //                Message = "Member not exist with id " + id
        //            });
        //        }
        //        _unitOfWork.MembersRepository.Remove(result);
        //        _unitOfWork.Save();
        //        return Ok(new ResponeModel
        //        {
        //            Status = Ok().StatusCode,
        //            Message = "Delete member Success",
        //        });
        //    }
        //    catch (DirectoryNotFoundException ex)
        //    {
        //        return BadRequest(new FailedResponseModel()
        //        {
        //            Status = BadRequest().StatusCode,
        //            Message = ex.Message
        //        });
        //    }
        //}

        //    [HttpPost("register")]
        //public async Task<ActionResult<MemberViewModels>> Register([FromBody] MemberViewModels Member)
        //{
        //    if (ModelState.IsValid == false)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (Member.Password != Member.Password_Conform)
        //    {
        //        return BadRequest(new ApiResponse
        //        {
        //            Success = false,
        //            Message = "Password and Confirm Password must be the same"
        //        });
        //    }
        //    var exitedMember = _unitOfWork.MemberRepository.Get(m => m.Email.Equals(Member.Email)).FirstOrDefault();
        //    if (exitedMember != null)
        //    {
        //        return BadRequest(new ApiResponse
        //        {
        //            Success = false,
        //            Message = "Email is already existed"
        //        });
        //    }
        //    var u = _mapper.Map<Member>(Member);
        //    u.RoleId = 2;
        //    u.HireDate = DateTime.Now;
        //    u.Source = "Register";
        //    try
        //    {
        //        _unitOfWork.MemberRepository.Insert(u);
        //        _unitOfWork.Save();
        //        return Ok(u);
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}

        //private bool MemberExists(int id)
        //{
        //    return (_unitOfWork.MemberRepository.Get()?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
