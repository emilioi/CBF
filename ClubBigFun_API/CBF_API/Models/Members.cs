using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBF_API.Models
{
    public class Members
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Member_Id { get; set; }
        [StringLength(100)]
        public string Login_Id { get; set; }

        public string Password { get; set; }
        [StringLength(100)]
        public string First_Name { get; set; }
        [StringLength(100)]
        public string Last_Name { get; set; }
        [StringLength(40)]
        public string Phone_Number { get; set; }
        [StringLength(100)]
        public string User_Type { get; set; }
        [StringLength(100)]
        public string Email_Address { get; set; }
        [StringLength(100)]
        public string Address_Line1 { get; set; }
        [StringLength(100)]
        public string Address_Line2 { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string Zip_Code { get; set; }
        [StringLength(50)]
        public string Fax_Number { get; set; }
        [StringLength(50)]
        public string Business_Phone { get; set; }
        [StringLength(50)]
        public string Gender { get; set; }
        [StringLength(100)]
        public string State { get; set; }
        [StringLength(10)]
        public string Country { get; set; }
        public string Image_Url { get; set; }
        public string Image_Name { get; set; }
        public DateTime Last_Login { get; set; }
        public int Failed_Attempt { get; set; }
        public DateTime Last_Failed_Login { get; set; }
        public bool Is_Email_Verified { get; set; }
        public bool Is_Temp_Password { get; set; }
        public bool Is_Locked { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public int Last_Edited_By { get; set; }
        public DateTime DTS { get; set; }
        public bool Rules_Accepted { get; set; }
        public string Reference { get; set; }

        public string FullName
        {
            get
            {
                return First_Name + " " + Last_Name;
            }
        }
    }

    public class ReferenceListAPIResponse : APIResponses
    {
        public List<ReferenceList> ReferenceList { get; set; }
    }

    public class ReferenceList
    {
        public string Reference { get; set; }
        public int MemberCount { get; set; }
    }
    public class Administrators
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Member_Id { get; set; }
        [StringLength(100)]
        public string Login_Id { get; set; }

        public string Password { get; set; }
        [StringLength(100)]
        public string First_Name { get; set; }
        [StringLength(100)]
        public string Last_Name { get; set; }
        [StringLength(40)]
        public string Phone_Number { get; set; }
        [StringLength(100)]
        public string Admin_Type { get; set; }
        [StringLength(100)]
        public string Email_Address { get; set; }
        public string Image_Url { get; set; }
        public string Image_Name { get; set; }
        public DateTime Last_Login { get; set; }
        public int Failed_Attempt { get; set; }
        public DateTime Last_Failed_Login { get; set; }
        public bool Is_Email_Verified { get; set; }
        public bool Is_Temp_Password { get; set; }
        public bool Is_Locked { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public int Last_Edited_By { get; set; }
        public DateTime DTS { get; set; }
        //   public string Reference { get; set; }
    }
    public class UserAPIResponses : APIResponses
    {
        public Members Users { get; set; }
    }
    public class UserListAPIResponses : APIResponses
    {
        public List<Members> Users { get; set; }
    }
    public class AdminAPIResponses : APIResponses
    {
        public Administrators Administrators { get; set; }
    }
    public class AdminListAPIResponses : APIResponses
    {
        public List<Administrators> Administrators { get; set; }
    }
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50)]
        public string First_Name { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50)]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(50)]
        public string Email_Address { get; set; }


        public string Login_Id { get; set; }
        [Required(ErrorMessage = "Phone_Number is required")]
        public string Phone_Number { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Reference { get; set; }
    }
    public class RegisterViewModelResponse : APIResponses
    {
        public RegisterViewModel Register { get; set; }
    }
    public class LoginDetails
    {
        public string Email_Address { get; set; }
        public int Member_Id { get; set; }
        public String User_Type { get; set; }
        [Required(ErrorMessage = " Name is required")]
        public String Name { get; set; }

    }

    public class LoginInfoResponse : APIResponses
    {
        public string Key { get; set; }
        public int Member_Id { get; set; }
        public Members UserInfo { get; set; }
        public bool EmailPreference { get; set; }

    }
    public class AdminLoginInfoResponse : APIResponses
    {
        public string Key { get; set; }
        public int Member_Id { get; set; }
        public Administrators UserInfo { get; set; }

    }
    public class LoginRequest
    {
        public string Email_Address { get; set; }

        //[Display(Description = "Login Id Should be different.")]
        //public string Login_Id { get; set; }

        [Required(ErrorMessage = " Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class ResetPasswordViewModelResponse : APIResponses
    {
        public ResetPasswordViewModel ResetPassword { get; set; }
    }
    public class ResetPasswordViewModelPublicResponse : APIResponses
    {
        public ResetPasswordViewModelPublic ResetPassword { get; set; }
    }
    public class ResetPasswordViewModelPublic
    {
        public string Email_Address { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Old_Password { get; set; }
        public int Member_Id { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Description = "Required: No, Max Length: 128 (string), For example: xyz@example.com, Regular Expression Validation: ^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$")]
        public string Email_Address { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Old_Password { get; set; }
        public int Member_Id { get; set; }
    }
    public class ForgotPasswordViewModelResponse : APIResponses
    {
        public ForgotPasswordViewModel ForgotPassword { get; set; }
    }

    public class ForgotPasswordViewModelRequest
    {
        public ForgotPasswordViewModel ForgotPassword { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Description = "Required: No, Max Length: 128 (string), For example: xyz@example.com, Regular Expression Validation: ^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$")]

        public string Email_Address { get; set; }
    }
    public class MemberUpload
    {
        public string Image_Url { get; set; }
        public int Member_Id { get; set; }
        public string Image_Name { get; set; }
        public string User_Type { get; set; }
    }
    public class MemberUploadResponses : APIResponses
    {
        public MemberUpload upload { get; set; }
    }

    public class Role_Permissions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Route_Id { get; set; }
        public string Route_Url { get; set; }
        public string ActionName { get; set; }
        public string ActionType { get; set; }
        public string UserType { get; set; }
        public bool Allowed { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool View { get; set; }
    }
    public class MembersList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Member_Id { get; set; }
        [StringLength(100)]
        public string Login_Id { get; set; }

        public string Password { get; set; }
        [StringLength(100)]
        public string First_Name { get; set; }
        [StringLength(100)]
        public string Last_Name { get; set; }
        [StringLength(40)]
        public string Phone_Number { get; set; }
        [StringLength(100)]
        public string User_Type { get; set; }
        [StringLength(100)]
        public string Email_Address { get; set; }
        [StringLength(100)]
        public string Address_Line1 { get; set; }
        [StringLength(100)]
        public string Address_Line2 { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string Zip_Code { get; set; }
        [StringLength(50)]
        public string Fax_Number { get; set; }
        [StringLength(50)]
        public string Business_Phone { get; set; }
        [StringLength(50)]
        public string Gender { get; set; }
        [StringLength(100)]
        public string State { get; set; }
        [StringLength(10)]
        public string Country { get; set; }
        public string Image_Url { get; set; }
        public string Image_Name { get; set; }
        public DateTime Last_Login { get; set; }
        public int Failed_Attempt { get; set; }
        public DateTime Last_Failed_Login { get; set; }
        public bool Is_Email_Verified { get; set; }
        public bool Is_Temp_Password { get; set; }
        public bool Is_Locked { get; set; }
        public bool Is_Active { get; set; }
        public bool Is_Deleted { get; set; }
        public int Last_Edited_By { get; set; }
        public DateTime DTS { get; set; }
        public bool Rules_Accepted { get; set; }
        public string Reference { get; set; }
    }

    public class MemberAdminLink
    {
        [Key, Column(Order = 0)]
        public int Admin_ID { get; set; }
        [Key, Column(Order = 1)]
        public int Member_ID { get; set; }
    }
}
