namespace LeafEmu.World.Game.Map.Cell
{
    public class InteractiveObject
    {
        public short gfx { get; private set; }
        public Cell celda { get; private set; }
        public ObjectInteractiveModel modelo { get; private set; }
        public bool es_utilizable { get; set; } = false;

        public InteractiveObject(short _gfx, Cell _celda)
        {
            gfx = _gfx;
            celda = _celda;

            ObjectInteractiveModel _modelo = ObjectInteractiveModel.get_Modelo_Por_Gfx(gfx);

            if (_modelo != null)
            {
                modelo = _modelo;
                es_utilizable = true;
            }
        }
    }

}
