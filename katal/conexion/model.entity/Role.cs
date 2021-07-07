namespace katal.conexion.model.entity
{
    public class Role
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int estado { get; set; }
        public string funciones { get; set; }
        public Role()
        {

        }
        public Role(int id)
        {
            this.id = id;
        }
        public Role(int id, string funciones)
        {
            this.id = id;
            this.funciones = funciones;
        }
        public Role(int role_id, string role_name, string role_descrition, string role_funciones)
        {
            this.id = role_id;
            this.name = role_name;
            this.description = role_descrition;
            this.funciones = role_funciones;
        }
    }
}