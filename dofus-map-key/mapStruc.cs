using System.Collections.Generic;

public class mapStruc
{
    public int Id { get; set; }
    public string CreateTime { get; set; }
    public string Data { get; set; }
    public string DataKey { get; set; }
    public List<int[]> mobsIDs { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string X { get; set; }
    public string Y { get; set; }
    public string Capabilities { get; set; }
    public string SubArea { get; set; }
    public int MaxMobs { get; set; }
    public int NbGroups { get; set; }
    public string PosFight { get; set; }
    public string mob { get; set; }
    public string cell { get; set; }

    public mapStruc(string _id, string _createTime, string _data, string _datakey, string _mobs, string _width, string _height,
        string _x, string _y, string _capabilities, string _subAreaId, int _maxMobs, string _PosFight, int _nbGroup, string _cell)
    {
        cell = _cell;
        mob = _mobs;
        PosFight = _PosFight;
        Id = int.Parse(_id);
        CreateTime = _createTime;
        DataKey = _datakey;
        Data = _data;
        mobsIDs = new List<int[]>();
        Width = int.Parse(_width);
        Height = int.Parse(_height);
        X = _x;
        Y = _y;
        Capabilities = _capabilities;
        SubArea = _subAreaId;
        MaxMobs = _maxMobs;
        NbGroups = NbGroups > 3 ? 3 : _nbGroup;
    }




}