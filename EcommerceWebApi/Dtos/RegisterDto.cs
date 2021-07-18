using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceWebApi.Dtos
{
    /// <summary>
    /// 註冊資訊
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// 使用者帳號
        /// </summary>
        [Required(ErrorMessage = "帳號為必填欄位")]
        public string UserName { get; set; }
        /// <summary>
        /// 使用者密碼
        /// </summary>
        [Required(ErrorMessage = "密碼為必填欄位")]
        [MinLength(8, ErrorMessage = "密碼不能小於八碼")]
        public string Password { get; set; }
        /// <summary>
        /// 確認密碼
        /// </summary>
        [Required(ErrorMessage = "確認密碼為必填欄位")]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不一致")]
        public string CheckPassword { get; set; }
        /// <summary>
        /// 信箱
        /// </summary>
        [Required(ErrorMessage = "信箱為必填欄位")]
        [EmailAddress(ErrorMessage = "信箱格式錯誤")]
        public string Email { get; set; }
        /// <summary>
        /// 姓氏
        /// </summary>
        [Required(ErrorMessage = "姓氏為必填欄位")]
        public string FirstName { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        [Required(ErrorMessage = "名字為必填欄位")]
        public string LastName { get; set; }
        /// <summary>
        /// 性別，男性：male，女性：female
        /// </summary>
        [Required(ErrorMessage = "性別為必填欄位")]
        public string Gender { get; set; }
        /// <summary>
        /// 地址(非必填)
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 出生日期(非必填)
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
}
