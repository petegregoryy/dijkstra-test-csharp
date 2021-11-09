const fs = require("fs");

let GeoJsonFile = JSON.parse(fs.readFileSync("data/25.geojson"));

console.log(GeoJsonFile)

let GeoFeatures = GeoJsonFile.features;

console.log(GeoFeatures)

let features = [];
let first = true;
GeoFeatures.forEach(feature =>
{
    //console.log(feature.properties)
});
GeoFeatures.forEach(feature =>
{
    if (first)
    {
        features.push(`{"_lat":${feature.geometry.coordinates[0][0]},"_long":${feature.geometry.coordinates[0][1]},"_name":${feature.properties.From}}`)
        features.push(`{"_lat":${feature.geometry.coordinates[1][0]},"_long":${feature.geometry.coordinates[1][1]},"_name":${feature.properties.To}}`)
        first = false;
    }
    else
    {

        let dupe = false;
        let dupe2 = false;
        /*
        features.forEach(element =>
        {
            if (element.name == feature.properties.From || element._lat == feature.geometry.coordinates[0][0] && element._long == feature.geometry.coordinates[0][1])
            {
                dupe = true;
            }
            if (element.name == feature.properties.To || element._lat == feature.geometry.coordinates[1][0] && element._long == feature.geometry.coordinates[1][1])
            {
                dupe2 = true;
            }
        });*/
        if (!dupe)
        {
            features.push(`{"_lat":${feature.geometry.coordinates[0][0]},"_long":${feature.geometry.coordinates[0][1]},"_name":${feature.properties.From}}`)
        }
        if (!dupe2)
        {
            features.push(`{"_lat":${feature.geometry.coordinates[1][0]},"_long":${feature.geometry.coordinates[1][1]},"_name":${feature.properties.To}}`)
        }
    }
});
featureString = features.join(',');
features = JSON.parse('[' + featureString + ']');
finalFeatures = []
features.forEach(featureChecking =>
{
    let dupe = false;
    features.forEach(element => 
    {

        if (featureChecking === element)
        {
            dupe = false;
            return;
        }
        if (featureChecking._lat === element._lat && featureChecking._long === element._long)
        {
            dupe = true;
        }
    });
    if (!dupe)
    {
        finalFeatures.push(featureChecking);
    }
});
console.log("Final Featurres!")
console.log(finalFeatures)