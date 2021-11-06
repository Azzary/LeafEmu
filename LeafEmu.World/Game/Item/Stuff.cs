using System;
using System.Collections.Generic;
using System.Text;

namespace LeafEmu.World.Game.Item
{
    public class Stuff
    {
        public int Position { get; set; }
        public int UID;
        public Item Template { get; set; }
        public int Quantity { get; set; }
        public List<Effect> Effects { get; set; }
        public int CostInPa { get; set; }
        public int TauxCC { get; set; }
        public int TauxEC { get; set; }


        public Stuff(int _UID, int _position, List<Effect> _effect, Item item, int quantier = 1)
        {
            Quantity = quantier;
            Template = item;
            Position = _position;
            UID = _UID;
            Effects = _effect;
            //ParseWeaponInfos(item.infosWeapon);
        }

        public string StringEffect()
        {
            StringBuilder stringEffects = new StringBuilder();
            foreach (var item in Effects)
            {
                stringEffects.Append("," + item.ToString());
            }
            return stringEffects.ToString().Substring(1);
        }

        public static List<Stuff> RangerItem(List<Stuff> inv)
        {
            List<Stuff> newInv = new List<Game.Item.Stuff>();
            Dictionary<int, bool> uid = new Dictionary<int, bool>();
            foreach (Stuff item in inv)
            {
                if (item.Position != -1 && !uid.ContainsKey(item.UID))
                {
                    uid.Add(item.UID, true);
                    newInv.Add(item);
                }
                else if (!uid.ContainsKey(item.UID))
                {
                    int quantity = 0;
                    var temp = inv.FindAll(x => item.Template.ID == x.Template.ID && item.Position == -1 && ItemHaveSameEffect(x, item));
                    temp.ForEach(x => { uid.Add(x.UID, true); quantity += x.Quantity; });
                    newInv.Add(new Stuff(Database.LoadDataBase.GetNewUIDItem(), -1, item.Effects, item.Template, quantity));
                }
            }
            return newInv;
        }

        public static bool ItemHaveSameEffect(Stuff item1, Stuff item2)
        {
            if (item1.Effects.Count != item2.Effects.Count)
            {
                return false;
            }
            //List<int> allReadyUseEffect = new List<int>();
            foreach (var effect1 in item1.Effects)
            {
                if (item2.Effects.Exists(effect2 => effect2.ID == effect1.ID && effect1.CurrentJet == effect2.CurrentJet))
                    continue;
                return false;
            }
            return true;
        }

        public static List<Effect> GetRandomEffect(List<Effect> Effects)
        {
            List<Effect> randomEffects = new List<Effect>();
            Effects.ForEach(x => randomEffects.Add(CreateRandomEffect(x)));
            return randomEffects;
        }

        public string DisplayItem
        {
            get
            {
                string item = string.Empty;
                item += UID.ToString("x");
                item += "~";
                item += Template.ID.ToString("x");
                item += "~";
                item += Quantity.ToString("x");
                item += "~";
                item += Position == -1 ? string.Empty : Position.ToString("x");
                item += "~" + StringEffect();
                return item;
            }
        }

        public static Effect CreateRandomEffect(Effect effect)
        {
            Effect newEffect = new Effect();
            newEffect.ID = effect.ID;
            if (newEffect.IsWeaponEffect())
            {
                newEffect.Min = effect.Min + effect.Fix;
                newEffect.Fix = effect.Fix;
                newEffect.Max = effect.Max + effect.Fix;
            }
            else
            {
                newEffect.Min = effect.Min + effect.Fix;
                newEffect.CurrentJet = Util.rng.Next(effect.Min, effect.Max) + effect.Fix;
                newEffect.Max = effect.Max + effect.Fix;
            }
            return newEffect;
        }
        public static Stuff GenerateItem(int templateID, bool perfect)
        {
            try
            {
                Item template = Database.table.Item.model_item.AllItems[templateID];
                Stuff item = new Stuff(
                    Database.LoadDataBase.GetNewUIDItem(),
                    -1,
                    Parse(template.Stats, perfect),
                    template);
                item.Quantity = 1;
                //item.SaveAndFlush();

                //if (canGenerateMount)
                //{
                //    Database.Records.MountTemplateRecord mount = Helper.MountHelper.GetMountTemplateByScrool(item.Template);
                //    if (mount != null)
                //    {
                //        Database.Records.WorldMountRecord newWMount = Game.Mounts.MountFactory.CreateMount(mount, item.UID, client.Character.ID);
                //    }
                //}
                return item;
            }
            catch (Exception e)
            {
                Logger.Logger.Log("Cant generate items : " + e.ToString());
                return null;
            }
        }
        public static List<Effect> Parse(string toParse, bool perfect)
        {
            List<Effect> toReturn = new List<Effect>();
            foreach (string littleEffect in toParse.Split(','))
            {
                Effect newEffect = ParseLittleEffect(littleEffect, perfect);
                toReturn.Add(newEffect);
            }
            return toReturn;
        }
        private void ParseWeaponInfos(string infos)
        {
            if (infos != string.Empty)
            {
                string[] data = infos.Split(',');
                this.CostInPa = int.Parse(data[1]);
                this.TauxCC = int.Parse(data[4]);
                this.TauxEC = int.Parse(data[5]);
            }
        }
        private static Effect ParseLittleEffect(string littleEffect, bool perfect)
        {
            Effect e = new Effect();
            if (littleEffect != string.Empty)
            {
                string[] data = littleEffect.Split('#');
                if (data[0] != "-1")
                {
                    e.ID = Convert.ToInt32(data[0], 16);
                }
                if (data.Length > 1)
                {
                    if (data[1] != string.Empty)
                    {
                        e.Min = Convert.ToInt32(data[1], 16);
                    }
                }
                if (data.Length > 2)
                {
                    if (data[2] != string.Empty)
                    {
                        e.Max = Convert.ToInt32(data[2], 16);
                    }
                }
                if (data.Length > 4)
                {
                    if (data[4] != string.Empty)
                    {
                        if (data[4].Contains("+"))
                        {
                            string[] desEffect = data[4].Split('d');
                            e.Min = int.Parse(desEffect[0]);
                            e.Max = int.Parse(desEffect[1].Split('+')[0]);
                            e.Fix = int.Parse(data[4].Split('+')[1]);
                            e.CurrentJet = perfect ? e.Max : Util.rng.Next(e.Min, e.Max + 1) + e.Fix;
                        }
                    }
                }
            }
            return e;
        }

        public string getStatForDb()
        {
            StringBuilder eff = new StringBuilder();
            foreach (var item in Effects)
            {
                eff.Append($"{item.ID},{item.Fix},{item.Max},{item.Min},{item.CurrentJet};");
            }
            return eff.ToString();
        }
    }

}
