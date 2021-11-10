using System.Collections.Generic;

class FeatureCollection
{
    public string type;

    public List<GeoFeature> features;

    public FeatureCollection(string _type, List<GeoFeature> _features)
    {
        type = _type;
        features = _features;
    }
}