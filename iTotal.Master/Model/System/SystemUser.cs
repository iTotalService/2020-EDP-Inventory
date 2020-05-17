using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTotal.Base
{
    public class SystemUser
    {
        [DisplayName("ID")]
        [Key]
        [Required]
        public string ID { get; set; }

        [DisplayName("帳號")]
        [Required]
        [StringLength(50)]
        [MinLength(5, ErrorMessage = "長度不可小於 5")]
        [MaxLength(50, ErrorMessage = "長度不可超過 50")]
        public string Account { get; set; }

        [DisplayName("密碼")]
        [Required]
        [StringLength(200)]
        [MinLength(5, ErrorMessage = "長度不可小於 5")]
        [MaxLength(200, ErrorMessage = "長度不可超過 200")]
        public string Password { get; set; }

        [DisplayName("名稱")]
        [Required]
        [StringLength(50)]
        [MinLength(2, ErrorMessage = "長度不可小於 2")]
        [MaxLength(50, ErrorMessage = "長度不可超過 50")]
        public string UserName { get; set; }

        [DisplayName("Email")]
        [Required]
        [StringLength(200)]
        [MinLength(2, ErrorMessage = "長度不可小於 2")]
        [MaxLength(200, ErrorMessage = "長度不可超過 200")]
        public string Email { get; set; }

        [DisplayName("是否使用")]
        [Required]
        public bool IsEnable { get; set; }

        [DisplayName("建立者")]
        [Required]
        public string CreateBy { get; set; }

        [DisplayName("建立時間")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateOn { get; set; }

        [DisplayName("更新者")]
        public string UpdateBy { get; set; }

        [DisplayName("更新時間")]
        public DateTime? UpdateOn { get; set; }

        public SystemUser()
        {
            this.ID = Guid.NewGuid().ToString();
            this.IsEnable = false;
            //this.CreateBy = new Guid();

            this.SystemRoles = new List<SystemRole>();
        }

        public SystemUser CopyFrom(SystemUser rhs)
        {
            this.ID = rhs.ID;
            this.IsEnable = rhs.IsEnable;
            this.UserName = rhs.UserName;
            this.Account = rhs.Account;
            this.Email = rhs.Email;
            this.Password = rhs.Password;
            this.Account = rhs.Account;
            this.CreateBy = rhs.CreateBy;
            this.CreateOn = DateTime.Now;
            return this;
        }
        public ICollection<SystemRole> SystemRoles { get; set; }
    }
}
