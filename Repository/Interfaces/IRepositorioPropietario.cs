namespace InmobiliariaPanelo.Models
{
    public interface IPropietarioRepositorio {

        List<Propietario> PropietarioObtenerTodos();
    //    int PropietarioAlta(Propietario p);
        int PropietarioEliminar(int id);
   //     int PropietarioModificacion(Propietario p);
        Propietario PropietarioObtenerPorId(int id);



    }
}
