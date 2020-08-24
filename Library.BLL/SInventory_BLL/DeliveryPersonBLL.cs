using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class DeliveryPersonBLL
    {
        DeliveryPersonDAL aDeliveryPersonDal=new DeliveryPersonDAL();
        public bool SaveDeliveryPerson(DeliveryPersonDAO aDeliveryPersonDao)
        {
            //ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

            //aDeliveryPersonDao.DeliveryManId = aClsPrimaryKeyFind.PrimaryKeyMax("DeliveryManId", "tblDeliveryManInfo");
            //aDeliveryPersonDao.DeliveryManCode = CodeGenerator(aDeliveryPersonDao.DeliveryManId);
            return aDeliveryPersonDal.SaveDeliveryPerson(aDeliveryPersonDao);
        }

        public void LoadCompanyDropdownList(DropDownList ddl)
        {
            aDeliveryPersonDal.GetCompanyDropdownList(ddl);
        }


        public void EmpLoad(DropDownList aDropDownList, string companyId)
        {
            aDeliveryPersonDal.EmpLoad(aDropDownList, companyId);
        }

        public DataTable LoadDelivaryMan()
        {
            return aDeliveryPersonDal.LoadDelivaryMan();
        }
        public DataTable LoadDelivaryManById(string delId)
        {
            return aDeliveryPersonDal.LoadDelivaryManById(delId);

        }
        private string CodeGenerator(int id)
        {
            string code = string.Empty;
            string Id = id.ToString();
            if (Id.Length == 1)
            {
                Id = "1000" + Id;
            }
            if (Id.Length == 2)
            {
                Id = "100" + Id;
            }
            if (Id.Length == 3)
            {
                Id = "10" + Id;
            }
            if (Id.Length == 4)
            {
                Id = "1" + Id;
            }


            code = Id;
            return code;
        }
        public bool UpdateDeliveryInfo(DeliveryPersonDAO aDeliveryPersonDao)
        {
            return aDeliveryPersonDal.UpdateDeliveryInfo(aDeliveryPersonDao);
        }

        public DataTable LoadEmpInfoById(string empId)
        {
            return aDeliveryPersonDal.GetEmpInfoById(empId);
        }

        public DataTable LoadSRInfoById(string empId)
        {
            return aDeliveryPersonDal.GetSRInfoById(empId);
        }

        public void EmpLoad(DropDownList ddl)
        {
            aDeliveryPersonDal.EmpLoad(ddl);
        }
    }
}
