const fs = require("fs");
var exec = require("child_process").exec;
var execSync = require("child_process").execSync;
const child_process = require("child_process");

let GeoJsonFile = JSON.parse(fs.readFileSync("data/25.geojson"));

//console.log(GeoJsonFile)

let GeoFeatures = GeoJsonFile.features;

//console.log(GeoFeatures)

let JsonTemplate = {
    "type": "Feature",
    "geometry": {
      "type": "Point",
      "coordinates": [125.6, 10.1]
    },
    "properties": {
      "name": "Dinagat Islands"
    }
}

let finalFeatures = {
    "type": "FeatureCollection",
    "features":[],
}



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
        //features.push(`{"_lat":${feature.geometry.coordinates[0][0]},"_long":${feature.geometry.coordinates[0][1]},"_name":${feature.properties.From}}`)
        //features.push(`{"_lat":${feature.geometry.coordinates[1][0]},"_long":${feature.geometry.coordinates[1][1]},"_name":${feature.properties.To}}`)
        first = false;

        let pushJson = JsonTemplate;
        
        pushJson.geometry.coordinates = [feature.geometry.coordinates[0][0],feature.geometry.coordinates[0][1]]
        pushJson.properties.name = feature.properties.From;
        features.push(pushJson);

        let pushJson2 = JsonTemplate;
        pushJson.geometry.coordinates = [feature.geometry.coordinates[0][0],feature.geometry.coordinates[0][1]]
        pushJson.properties.name = feature.properties.From;
        features.push(pushJson2);
    }
    else
    {
        let pushJson = JsonTemplate;
        
        pushJson.geometry.coordinates = [feature.geometry.coordinates[0][0],feature.geometry.coordinates[0][1]]
        pushJson.properties.name = feature.properties.From;
        features.push({
            "type": "Feature",
            "geometry": {
              "type": "Point",
              "coordinates": [feature.geometry.coordinates[0][0], feature.geometry.coordinates[0][1]]
            },
            "properties": {
              "name": feature.properties.From
            }
        });

        let pushJson2 = JsonTemplate;
        pushJson.geometry.coordinates = [feature.geometry.coordinates[feature.geometry.coordinates.length -1][0],feature.geometry.coordinates[feature.geometry.coordinates.length -1][1]]
        pushJson.properties.name = feature.properties.From;
        features.push({
            "type": "Feature",
            "geometry": {
              "type": "Point",
              "coordinates": [feature.geometry.coordinates[feature.geometry.coordinates.length -1][0], feature.geometry.coordinates[feature.geometry.coordinates.length -1][1]]
            },
            "properties": {
              "name": feature.properties.To
            }
        });
    }
});
//featureString = features.join(',');
//features = JSON.parse('[' + featureString + ']');
//finalFeatures = []
//console.log(features)
let dupes = 1;
let valids = 1;
let cycles = 1;
finalFeatures.features.push(features[0]);
features.forEach(featureChecking =>
{
    cycles++
    valid = false;
    finalFeatures.features.forEach(element => {
        if(featureChecking.properties.name === element.properties.name && featureChecking.geometry.coordinates[0] === element.geometry.coordinates[0] && featureChecking.geometry.coordinates[1] === element.geometry.coordinates[1]){
            valid = false;
            return
        }
        else{
            valid = true
            
        }
    });
    if(valid){
        valids = finalFeatures.features.push(featureChecking)
    }
    else{
        dupes++
    }

});
console.log("Final Featurres!")
console.log(finalFeatures)
console.log(`Dupes: ${dupes} - Valids: ${valids} - Cycles: ${cycles}`);

let finalDupe = 0;
finalFeatures.features.forEach(feature => {
    let workingNum = 0;
    finalFeatures.features.forEach(innerFeature => {
        if(feature === innerFeature){
            workingNum++;
        }
    });
    if(workingNum >1){
        finalDupe++
    }
});
console.log(`Final Duplicates: ${finalDupe}`);
fs.writeFileSync("data/features.geojson",JSON.stringify(finalFeatures));

child_process.exec(`ruby ruby/ocean-name.rb 0 0`, function (err, stdout, stderr) {
    console.log(stdout);
    console.log(stderr);
});
/*
finalFeatures.features.forEach(feature => {
    execSync(`ruby ruby/ocean-name.rb ${feature.geometry.coordinates[0]} ${feature.geometry.coordinates[1]}`, function (err, stdout, stderr) {
        console.log(stdout);
    });
})

//CallRuby();

async function CallRuby(){
    let featCount = 0
    let running = false;
    while(featCount < finalFeatures.features.length){
        await execSync(`ruby ruby/ocean-name.rb ${finalFeatures.features[featCount].geometry.coordinates[0]} ${finalFeatures.features[featCount].geometry.coordinates[1]}`, function (err, stdout, stderr) {
            console.log(stdout);
            featCount++;
        });
        
        
    }
}*/