using System.Collections.Generic;

class Properties
{

    public string name;

    public string connectsTo;

    public string connectsFrom;
    public Properties(string _name, string _connectsTo, string _connectsFrom)
    {
        name = _name;
        connectsTo = _connectsTo;
        connectsFrom = _connectsFrom;
    }

}