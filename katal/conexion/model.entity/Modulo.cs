using System.Collections.Generic;

namespace katal.conexion.model.entity
{
    public class Modulo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }

    }
    public class Programa
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }

    }
    public class operacion
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }

    }

    public class ModuloAsignacion
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public List<ProgramaAsignacion> programas;
        public int assigned;

        public ModuloAsignacion(int user_id, string name, string slug)
        {
            this.id = user_id;
            this.name = name;
            this.slug = slug;
            programas = new List<ProgramaAsignacion>();
            assigned = -1;
        }

    }
    public class ProgramaAsignacion
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public List<OperacionAsignacion> operacions;
        public int assigned; // -1 no asignado  // 0 abierto por ser padre // 1 asignado 
        public ProgramaAsignacion(int user_id, string name, string slug)
        {
            this.id = user_id;
            this.name = name;
            this.slug = slug;
            operacions = new List<OperacionAsignacion>();
            assigned = -1;
        }

    }
    public class OperacionAsignacion
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int assigned;
        public OperacionAsignacion(int user_id, string name, string slug)
        {
            this.id = user_id;
            this.name = name;
            this.slug = slug;
            assigned = -1;
        }
    }

    public class DatatreeView
    {
        public string nodesInfo { get; set; }
        public List<object> nodesState { get; set; }
    }
    public class NodeModule
    {
        public string name { get; set; }
        public string value { get; set; }
        public List<NodePrograma> programas { get; set; }
        public NodeModule(string name, string value)
        {

            this.name = name;
            this.value = value;
            this.programas = new List<NodePrograma>();
        }


    }
    public class NodePrograma
    {
        public string name { get; set; }
        public string value { get; set; }
        public List<NodeOperacion> operacions { get; set; }

        public NodePrograma(string name, string value)
        {
            this.name = name;
            this.value = value;
            this.operacions = new List<NodeOperacion>();
        }
    }
    public class NodeOperacion
    {
        public string name { get; set; }
        public string value { get; set; }
        public NodeOperacion(string name, string value)
        {
            this.name = name;
            this.value = value;

        }
    }

}