using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vendor_Portal.Models
{

    public class User
    {
        public string U_userType { get; set; }
        public int DocEntry { get; set; }

        public string U_userName { get; set; }
        public string U_userID { get; set; }

        public string U_password { get; set; }

        public string U_BranchID { get; set; }
        public string U_BranchName { get; set; }

        public string U_department { get; set; }

        public string U_isActive { get; set; }

        public string U_assignHead { get; set; }
        public string U_assignHeadName { get; set; }
        public string U_AssignHeadName2 { get; set; }
        public string U_assignHead2 { get; set; }

        public string U_CreatorRemarks { get; set; }
        public int U_Wrong_Attempts { get; set; }
        public string U_departmentID { get; set; }
        public List<User_R> User_R { get; set; }
    }
    public class User_R
    {
        public string ItmsGrpCod { get; set; }
        public string ItmsGrpNam { get; set; }
    }
}