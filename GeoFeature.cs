using System;

class GeoFeature{

    public string type;
    public Geometry geometry;
    public Properties properties;

    public GeoFeature(string _type, string _geoType, float[] _geoCoords, string _name){
        type = _type;
        geometry = new Geometry(_geoType,_geoCoords);
        properties = new Properties(_name);
    }

    public string GetTypeString(){
        return type;
    }
}