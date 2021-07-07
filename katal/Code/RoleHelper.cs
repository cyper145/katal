using System.Collections.Generic;
using System.Linq;

using katal.conexion.model.entity;
using katal.conexion.model.neg;

namespace katal.Code
{
    public static class RoleHelper
    {



        public static List<Role> getroles()
        {
            RoleNeg negocio = new RoleNeg();
            return negocio.findAll();
        }


        public static void AddNewRecord(Role obj)
        {
            RoleNeg negocio = new RoleNeg();
            negocio.create(obj);
        }

        public static void UpdateRecord(Role obj)
        {
            RoleNeg negocio = new RoleNeg();
            negocio.update(obj);
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            RoleNeg negocio = new RoleNeg();
            List<int> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => int.Parse(id));
            negocio.DeleteUser(selectedIds);
        }
    }
}