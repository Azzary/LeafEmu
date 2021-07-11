using System;
using System.Collections.Generic;

namespace LeafEmu.World.Game.Entity.Npc
{
    public class NpcTemplate : Entity
    {
        public int BonusValue { get; set; }
        public int ExtraClip { get; set; }
        public int Information { get; set; }
        public int CustomArtWork { get; set; }
        public string Accessories { get; set; }
        public Dictionary<int, int> InitQuestion { get; }
        public string Vente { get; set; }
        public string Quest { get; set; }
        public string Exchanges { get; set; }
        public string Path { get; set; }

        public NpcTemplate(int _id, int _bonusValue, int _gfxID, int _scaleX, int _scaleY, int _sex, int _color1, int _color2, int _color3,
                       string _accessories, int _extraClip, int _customArtWork, string _initQuestion, string _ventes, string _quest, string _exchanges,
                       string _path, int _informations)
        {
            InitQuestion = new Dictionary<int, int>();
            this.id = _id;
            this.BonusValue = _bonusValue;
            this.gfxID = _gfxID;
            this.Scale = _scaleX;
            sexe = _sex;
            couleur1 = _color1;
            couleur2 = _color2;
            couleur3 = _color3;
            Accessories = _accessories;
            ExtraClip = _extraClip;
            CustomArtWork = _customArtWork;
            if (int.TryParse(_initQuestion, out int idQ))
                InitQuestion.Add(0, idQ);
            else
            {
                foreach (var item in _initQuestion.Split('|'))
                {
                    var info = item.Split(',');
                    InitQuestion.Add(Convert.ToInt32(info[0]), Convert.ToInt32(info[1]));
                }
            }
            Vente = _ventes;
            Quest = _quest;
            Exchanges = _exchanges;
            Path = _path;
            Information = _informations;
        }
    }

}
