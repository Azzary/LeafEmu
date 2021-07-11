using System;
using System.Collections.Generic;
using System.Linq;

namespace LeafEmu.World.Game.Map.Cell
{
    public class ObjectInteractiveModel
    {

        public short[] gfxs { get; private set; }
        public bool caminable { get; private set; }
        public short[] habilidades { get; private set; }
        public string nombre { get; private set; }
        public bool recolectable { get; private set; }

        private static List<ObjectInteractiveModel> interactivos_modelo_cargados = new List<ObjectInteractiveModel>();

        public ObjectInteractiveModel(string _nombre, string _gfx, bool _caminable, string _habilidades, bool _recolectable)
        {
            nombre = _nombre;

            if (!_gfx.Equals("-1") && !string.IsNullOrEmpty(_gfx))
            {
                string[] separador = _gfx.Split(',');
                gfxs = new short[separador.Length];

                for (byte i = 0; i < gfxs.Length; i++)
                    gfxs[i] = short.Parse(separador[i]);
            }

            caminable = _caminable;

            if (!_habilidades.Equals("-1") && !string.IsNullOrEmpty(_habilidades))
            {
                string[] separador = _habilidades.Split(',');
                habilidades = new short[separador.Length];

                for (byte i = 0; i < habilidades.Length; ++i)
                    habilidades[i] = short.Parse(separador[i]);
            }

            recolectable = _recolectable;
            interactivos_modelo_cargados.Add(this);
        }

        public static ObjectInteractiveModel get_Modelo_Por_Gfx(short gfx_id)
        {
            foreach (ObjectInteractiveModel interactivo in interactivos_modelo_cargados)
            {
                if (interactivo.gfxs.Contains(gfx_id))
                    return interactivo;
            }
            return null;
        }

        public static ObjectInteractiveModel get_Modelo_Por_Habilidad(short habilidad_id)
        {
            IEnumerable<ObjectInteractiveModel> lista_interactivos = interactivos_modelo_cargados.Where(i => i.habilidades != null);

            foreach (ObjectInteractiveModel interactivo in lista_interactivos)
            {
                if (interactivo.habilidades.Contains(habilidad_id))
                    return interactivo;
            }
            return null;
        }

        public static List<ObjectInteractiveModel> get_Interactivos_Modelos_Cargados() => interactivos_modelo_cargados;
    }
}

