using System.Threading.Tasks;
using Rongban.Models.Entities;

namespace AuthSystem.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<UserInfo> GetUserByIdAsync(long userId);
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<UserInfo> CreateUserAsync(UserInfo user);
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUserAsync(UserInfo user);
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="credentialType"></param>
        /// <param name="credentialValue"></param>
        /// <returns></returns>
        Task<UserInfo> GetUserByPhoneNumberAsync(byte credentialType, string credentialValue);
        Task<UserCredential> GetCredentialAsync(byte credentialType, string credentialValue);
        /// <summary>
        /// 创建用户凭证
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        Task<UserCredential> CreateCredentialAsync(UserCredential credential);
        Task UpdateCredentialAsync(UserCredential credential);
        Task AddVerificationCodeAsync(VerificationCode code);
        Task<VerificationCode> GetValidVerificationCodeAsync(string phoneNumber, string code);
        Task MarkVerificationCodeAsUsedAsync(long codeId);
        Task DeleteVerificationCodeAsync(long id);
    }
}
